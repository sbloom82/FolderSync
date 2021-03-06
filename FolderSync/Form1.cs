﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FolderSync
{
    public partial class Form1 : Form
    {

        public static Form1 instance;
        Color highlighted = Color.Green;
        Settings settings = new Settings();

        public Form1()
        {
            instance = this;

            InitializeComponent();

            settings.Read();

            txtSourcePath.Text = settings.LastSourceFolder ?? "";
            chkSortSourceLastModified.Checked = settings.SortSourceByModified;
            txtDestinationPath.Text = settings.LastDestinationFolder ?? "";
            chkSortDestLastModified.Checked = settings.SortDestinationByModified;

            tvSource.ImageList = images;
            tvDestination.ImageList = images;

            btnLoadSource_Click(null, null);
            btnLoadDestination_Click(null, null);
        }

        bool sourceLoaded = false;
        bool destLoaded = false;
        private void btnLoadSource_Click(object sender, EventArgs e)
        {
            if (txtSourcePath.Text.Length < 1)
            {
                return;
            }

            DirectoryInfo directory = new DirectoryInfo(txtSourcePath.Text);
            if (directory.Exists)
            {
                foreach (SyncCommand command in commands.Values.Where(c => c is AddSyncCommand).ToList())
                {
                    if (!command.FileSystemInfo.FullName.StartsWith(
                        txtDestinationPath.Text, StringComparison.OrdinalIgnoreCase))
                    {
                        commands.Remove(command.Path);
                    }
                }

                tvSource.Nodes.Clear();
                TreeNode root = tvSource.Nodes.Add(directory.Name);
                root.ImageIndex = root.SelectedImageIndex = -1;
                this.LoadDirectory(directory, root, chkSortSourceLastModified.Checked);
                root.Expand();

                sourceLoaded = true;

                settings.SetValue(nameof(settings.LastSourceFolder), txtSourcePath.Text);
                settings.SetValue(nameof(settings.SortSourceByModified), chkSortSourceLastModified.Checked);
            }
        }

        private void btnLoadDestination_Click(object sender, EventArgs e)
        {
            if (txtDestinationPath.Text.Length < 1)
            {
                return;
            }

            DirectoryInfo directory = new DirectoryInfo(txtDestinationPath.Text);
            if (directory.Exists)
            {
                foreach (SyncCommand command in commands.Values.Where(c => c is RemoveSyncCommand).ToList())
                {
                    if (!command.FileSystemInfo.FullName.StartsWith(
                        txtDestinationPath.Text, StringComparison.OrdinalIgnoreCase))
                    {
                        commands.Remove(command.Path);
                    }
                }

                tvDestination.Nodes.Clear();
                TreeNode root = tvDestination.Nodes.Add(directory.Name);
                root.ImageIndex = root.SelectedImageIndex = -1;
                this.LoadDirectory(directory, root, chkSortDestLastModified.Checked);
                root.Expand();

                destLoaded = true;


                settings.SetValue(nameof(settings.LastDestinationFolder), txtDestinationPath.Text);
                settings.SetValue(nameof(settings.SortDestinationByModified), chkSortDestLastModified.Checked);
            }
        }

        bool cancelCompare;
        bool comparing;
        private void Compare()
        {
            if (comparing)
            {
                cancelCompare = true;
                return;
            }
            else
            {
                cancelCompare = false;
            }

            if (destLoaded && sourceLoaded)
            {
                DirectoryInfo source = new DirectoryInfo(this.txtSourcePath.Text);
                DirectoryInfo destination = new DirectoryInfo(this.txtDestinationPath.Text);
                if (!source.Exists || !destination.Exists)
                {
                    return;
                }

                if (!comparing)
                {
                    comparing = true;

                    CompareDirectories(source, tvSource.Nodes[0], destination);

                    CompareDirectories(destination, tvDestination.Nodes[0], source);

                    comparing = false;
                }
            }
            SetTreeNodeIndex(null, 0);
        }

        public bool CompareDirectories(DirectoryInfo source, TreeNode sourceNode, DirectoryInfo destination)
        {
            if (cancelCompare)
            {
                return false;
            }

            var sourceFiles = source.EnumerateFiles().ToDictionary(f => f.Name, f => f);
            var destinationFiles = destination.EnumerateFiles().ToDictionary(f => f.Name, f => f);
            var sourceDirs = source.EnumerateDirectories().ToDictionary(f => f.Name, f => f);
            var destinationDirs = destination.EnumerateDirectories().ToDictionary(f => f.Name, f => f);

            bool equal = true;

            foreach (FileInfo file in sourceFiles.Values)
            {
                TreeNode fileNode = null;
                if (sourceNode != null)
                {
                    foreach (TreeNode node in sourceNode.Nodes)
                    {
                        if (node.Tag != null && ((FileSystemInfo)node.Tag).FullName == file.FullName)
                        {
                            fileNode = node;
                            break;
                        }
                    }
                }

                if (destinationFiles.ContainsKey(file.Name))
                {
                    FileInfo destinationFile = destinationFiles[file.Name];
                    if (file.Length == destinationFile.Length)
                    {
                        //checkmark on node 
                        SetTreeNodeIndex(fileNode, 1);
                    }
                    else
                    {
                        SetTreeNodeIndex(fileNode, 2);
                        equal = false;
                    }
                }
                else
                {
                    SetTreeNodeIndex(fileNode, 3);
                    equal = false;
                }
            }


            foreach (DirectoryInfo dir in sourceDirs.Values)
            {
                TreeNode dirNode = null;
                if (sourceNode != null)
                {
                    foreach (TreeNode node in sourceNode.Nodes)
                    {
                        if (node.Tag != null && ((FileSystemInfo)node.Tag).FullName == dir.FullName)
                        {
                            dirNode = node;
                            break;
                        }
                    }
                }

                if (destinationDirs.ContainsKey(dir.Name))
                {
                    DirectoryInfo destDir = destinationDirs[dir.Name];
                    if (CompareDirectories(dir, dirNode, destDir))
                    {
                        SetTreeNodeIndex(dirNode, 1);
                    }
                    else
                    {
                        SetTreeNodeIndex(dirNode, 2);
                        equal = false;
                    }
                }
                else
                {
                    SetTreeNodeIndex(dirNode, 3);
                    equal = false;
                }
            }

            SetTreeNodeIndex(sourceNode, equal ? 1 : 2);

            return equal;
        }

        private void tvSource_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            DirectoryInfo dir = e.Node.Tag as DirectoryInfo;
            if (dir != null)
            {
                LoadDirectory(dir, e.Node, chkSortSourceLastModified.Checked);
            }
        }

        private void tvDestination_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            DirectoryInfo dir = e.Node.Tag as DirectoryInfo;
            if (dir != null)
            {
                LoadDirectory(dir, e.Node, chkSortDestLastModified.Checked);
            }
        }
        private void LoadDirectory(DirectoryInfo dir, TreeNode node, bool sortByLastModified)
        {
            IEnumerable<DirectoryInfo> directories = dir.GetDirectories().OrderBy(d => d.Name);
            if (sortByLastModified)
            {
                directories = directories.OrderByDescending(d => d.LastWriteTimeUtc);
            }

            IEnumerable<FileInfo> files = dir.GetFiles().OrderBy(d => d.Name);
            if (sortByLastModified)
            {
                files = files.OrderByDescending(d => d.LastWriteTimeUtc);
            }

            node.Nodes.Clear();
            foreach (DirectoryInfo child in directories)
            {
                TreeNode folderNode = new TreeNode(child.Name);
                folderNode.ImageIndex = folderNode.SelectedImageIndex = -1;
                folderNode.Tag = child;
                string menuText = "Add to Sync";
                if (node.TreeView == tvDestination)
                {
                    menuText += " (Delete Folder)";
                }
                if (commands.ContainsKey(child.FullName))
                {
                    folderNode.ForeColor = highlighted;
                    menuText = "Remove from Sync";
                }
                folderNode.ContextMenu = new ContextMenu();
                MenuItem item = new MenuItem(menuText, new EventHandler(node_Sync));
                item.Tag = folderNode;
                folderNode.ContextMenu.MenuItems.Add(item);
                node.Nodes.Add(folderNode);
                folderNode.Nodes.Add("");
            }

            foreach (FileInfo file in files)
            {
                TreeNode fileNode = new TreeNode(file.Name);
                fileNode.ImageIndex = fileNode.SelectedImageIndex = -1;
                fileNode.Tag = file;
                string menuText = "Add to Sync";
                if (node.TreeView == tvDestination)
                {
                    menuText += " (Delete File)";
                }
                if (commands.ContainsKey(file.FullName))
                {
                    menuText = "Remove from Sync";
                    fileNode.ForeColor = highlighted;
                }
                fileNode.ContextMenu = new ContextMenu();
                MenuItem item = new MenuItem(menuText, new EventHandler(node_Sync));
                item.Tag = fileNode;
                fileNode.ContextMenu.MenuItems.Add(item);
                fileNode.ContextMenu.MenuItems.Add(new MenuItem("Open", new EventHandler(node_Open)) { Tag = fileNode });
                node.Nodes.Add(fileNode);
            }
        }

        private void node_Sync(object sender, EventArgs e)
        {
            TreeNode node = ((MenuItem)sender).Tag as TreeNode;
            if (node.TreeView == tvSource)
            {
                this.SetAdd(node);
            }
            else
            {
                this.SetRemove(node);
            }
        }
        private void node_Open(object sender, EventArgs e)
        {
            TreeNode node = ((MenuItem)sender).Tag as TreeNode;
            var file = node.Tag as FileSystemInfo;
            if (file != null)
            {
                using (System.Diagnostics.Process p = new System.Diagnostics.Process())
                {
                    p.StartInfo = new System.Diagnostics.ProcessStartInfo(file.FullName);
                    p.Start();
                }
            }
        }


        private void tvSource_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            this.SetAdd(e.Node);
        }

        private void tvDestination_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            this.SetRemove(e.Node);
        }

        Dictionary<string, SyncCommand> commands =
            new Dictionary<string, SyncCommand>(StringComparer.OrdinalIgnoreCase);
        private void SetAdd(TreeNode node)
        {
            FileSystemInfo info = (FileSystemInfo)node.Tag;
            if (commands.ContainsKey(info.FullName))
            {
                node.ContextMenu.MenuItems[0].Text = "Add to Sync";
                node.ForeColor = Color.Black;
                commands.Remove(info.FullName);
            }
            else if (!commands.Keys.Any(p => info.FullName.StartsWith(p, StringComparison.OrdinalIgnoreCase)))
            {
                node.ContextMenu.MenuItems[0].Text = "Remove from Sync";
                node.ForeColor = highlighted;
                SyncCommand command = new AddIfNewerSyncCommand(((FileSystemInfo)node.Tag));
                commands[command.Path] = command;
            }
        }

        private void SetRemove(TreeNode node)
        {
            FileSystemInfo info = (FileSystemInfo)node.Tag;
            if (commands.ContainsKey(info.FullName))
            {
                node.ContextMenu.MenuItems[0].Text = "Add to Sync";
                node.ForeColor = Color.Black;
                commands.Remove(info.FullName);
            }
            else if (!commands.Keys.Any(p => info.FullName.StartsWith(p, StringComparison.OrdinalIgnoreCase)))
            {
                node.ContextMenu.MenuItems[0].Text = "Remove from Sync";
                node.ForeColor = highlighted;
                SyncCommand command = new RemoveSyncCommand(((FileSystemInfo)node.Tag));
                commands[command.Path] = command;
            }
        }

        bool syncing = false;
        bool cancel = false;
        private void btnSync_Click(object sender, EventArgs e)
        {
            if (syncing)
            {
                cancel = true;
                return;
            }

            if (!sourceLoaded)
            {
                MessageBox.Show("Source Directory not loaded");
                return;
            }

            if (!destLoaded)
            {
                MessageBox.Show("Destination Directory not loaded");
                return;
            }

            Task.Run(() => Sync());
        }

        public void Sync()
        {
            syncing = true;
            IEnumerable<SyncCommand> removes = commands.Values.Where(c => c is RemoveSyncCommand).ToList();
            IEnumerable<SyncCommand> adds = commands.Values.Where(c => c is AddSyncCommand).ToList();

            foreach (SyncCommand command in removes)
            {
                if (!cancel)
                {
                    try
                    {
                        command.FileSystemInfo.Refresh();
                        if (command.FileSystemInfo.Exists)
                        {
                            WriteMessage($"Deleting {command.Path}");
                            command.Execute();
                        }
                    }
                    catch (Exception ex)
                    {
                        WriteMessage($"Error when removing {command.FileSystemInfo.FullName} {ex}");
                    }
                }
            }

            foreach (AddSyncCommand command in adds)
            {
                if (!cancel)
                {
                    try
                    {
                        WriteMessage($"Copying {command.Path}");
                        string relativePath = command.Path.Replace(txtSourcePath.Text, "");
                        string copyToPath = txtDestinationPath.Text + relativePath;
                        command.CopyToPath = copyToPath;
                        command.Execute();
                    }
                    catch (Exception ex)
                    {
                        WriteMessage($"Error when adding {command.FileSystemInfo.FullName} {ex}");
                    }
                }
            }

            cancel = false;
            syncing = false;

            WriteMessage("\r\nComplete!");

            this.Compare();
        }

        public void CopyFile(string source, string destination, bool force = false)
        {
            if (cancel)
            {
                return;
            }

            WriteMessage($"Copying File {source}");

            string dir = Path.GetDirectoryName(destination);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            if (File.Exists(destination))
            {
                FileInfo sourceFile = new FileInfo(source);
                FileInfo destinationFile = new FileInfo(destination);
                if (force || sourceFile.Length == destinationFile.Length)
                {
                    WriteMessage($"Copying File {source} skipped");
                    return;
                }
            }

            File.Copy(source, destination, true);
        }

        public void CopyDirectory(string source, string destination)
        {
            if (cancel)
            {
                return;
            }

            WriteMessage($"Copying Directory {source}");

            if (!Directory.Exists(destination))
            {
                Directory.CreateDirectory(destination);
            }

            foreach (DirectoryInfo child in new DirectoryInfo(source).GetDirectories())
            {
                CopyDirectory(
                    Path.Combine(source, child.Name),
                    Path.Combine(destination, child.Name));
            }

            foreach (FileInfo child in new DirectoryInfo(source).GetFiles())
            {
                CopyFile(
                    Path.Combine(source, child.Name),
                    Path.Combine(destination, child.Name));
            }
        }


        public delegate void WriteMessageDelegate(string msg);
        public void WriteMessage(string msg)
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(delegate
                {
                    WriteMessage(msg);
                }));
                return;
            }

            txtStatus.AppendText($"\r\n{msg}");
            btnSync.Text = syncing ? "Cancel" : "Sync";
        }

        private void tvSource_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (tvSource.SelectedNode != null &&
               e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                SetAdd(tvSource.SelectedNode);
            }
        }

        private void tvDestination_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (tvDestination.SelectedNode != null &&
                e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                SetRemove(tvDestination.SelectedNode);
            }
        }

        private void btnDestinationChoose_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                txtDestinationPath.Text = fbd.SelectedPath;
                btnLoadDestination_Click(sender, e);
            }
        }

        private void btnChooseSource_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                txtSourcePath.Text = fbd.SelectedPath;
                btnLoadSource_Click(sender, e);
            }
        }

        public delegate void SetTreeNodeImageIndexDelegate(TreeNode node, int index);
        public void SetTreeNodeIndex(TreeNode node, int index)
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(delegate
                {
                    SetTreeNodeIndex(node, index);
                }));
                return;
            }

            btnCompare.Text = comparing ? "Cancel" : "Compare";

            if (node != null)
            {
                node.ImageIndex = index;
                node.SelectedImageIndex = index;
            }
        }

        private void btnCompare_Click(object sender, EventArgs e)
        {
            Task.Run(() => Compare());
        }
    }

    public abstract class SyncCommand
    {
        public FileSystemInfo FileSystemInfo { get; private set; }
        public string Path => FileSystemInfo.FullName;
        public SyncCommand(FileSystemInfo fileSystemInfo)
        {
            this.FileSystemInfo = fileSystemInfo;
        }

        public abstract void Execute();
    }

    public class AddSyncCommand : SyncCommand
    {
        public AddSyncCommand(FileSystemInfo fileSystemInfo) : base(fileSystemInfo) { }

        public string CopyToPath { get; set; }

        public override void Execute()
        {
            if (this.FileSystemInfo is FileInfo)
            {
                Form1.instance.CopyFile(this.FileSystemInfo.FullName, CopyToPath);
            }
            else
            {
                Form1.instance.CopyDirectory(this.FileSystemInfo.FullName, CopyToPath);
            }
        }
    }

    public class AddIfNewerSyncCommand : AddSyncCommand
    {
        public AddIfNewerSyncCommand(FileSystemInfo fileSystemInfo) : base(fileSystemInfo) { }

        public override void Execute()
        {
            base.Execute();

            /*
            FileInfo copyToFile = new FileInfo(CopyToPath);
            if (!copyToFile.Exists ||
                copyToFile
                copyToFile.LastWriteTimeUtc < this.FileSystemInfo.LastWriteTimeUtc)
            {
                base.Execute();
            } */
        }
    }

    public class RemoveSyncCommand : SyncCommand
    {
        public RemoveSyncCommand(FileSystemInfo fileSystemInfo) : base(fileSystemInfo) { }

        public override void Execute()
        {
            if (this.FileSystemInfo is DirectoryInfo)
            {
                ((DirectoryInfo)this.FileSystemInfo).Delete(true);
            }
            else
            {
                this.FileSystemInfo.Delete();
            }
        }
    }
}
