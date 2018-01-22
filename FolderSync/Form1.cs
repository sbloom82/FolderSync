using System;
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
        Color highlighted = Color.Green;

        public Form1()
        {
            InitializeComponent();
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
                this.LoadDirectory(directory, root, chkSortSourceLastModified.Checked);
                root.Expand();

                sourceLoaded = true;
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
                this.LoadDirectory(directory, root, chkSortDestLastModified.Checked);
                root.Expand();

                destLoaded = true;
            }
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

            node.Nodes.Clear();
            foreach (DirectoryInfo child in directories)
            {
                TreeNode folderNode = new TreeNode(child.Name);
                folderNode.Tag = child;
                string menuText = "Add to Sync";
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

            foreach (FileInfo file in dir.GetFiles().OrderBy(f => f.Name))
            {
                TreeNode fileNode = new TreeNode(file.Name);
                fileNode.Tag = file;
                string menuText = "Add to Sync";
                if (commands.ContainsKey(file.FullName))
                {
                    menuText = "Remove from Sync";
                    fileNode.ForeColor = highlighted;
                }
                fileNode.ContextMenu = new ContextMenu();
                MenuItem item = new MenuItem(menuText, new EventHandler(node_Sync));
                item.Tag = fileNode;
                fileNode.ContextMenu.MenuItems.Add(item);
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
                SyncCommand command = new AddSyncCommand(((FileSystemInfo)node.Tag));
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
                            if (command.FileSystemInfo is DirectoryInfo)
                            {
                                ((DirectoryInfo)command.FileSystemInfo).Delete(true);
                            }
                            else
                            {
                                command.FileSystemInfo.Delete();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        WriteMessage($"Error when removing {command.FileSystemInfo.FullName} {ex}");
                    }
                }
            }

            foreach (SyncCommand command in adds)
            {
                if (!cancel)
                {
                    try
                    {
                        WriteMessage($"Copying {command.Path}");

                        string relativePath = command.Path.Replace(txtSourcePath.Text, "");
                        string copyToPath = txtDestinationPath.Text + relativePath;

                        if (command.FileSystemInfo is FileInfo)
                        {
                            CopyFile(command.FileSystemInfo.FullName, copyToPath);
                        }
                        else
                        {
                            CopyDirectory(command.FileSystemInfo.FullName, copyToPath);
                        }
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
        }

        public void CopyFile(string source, string destination)
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
                if (sourceFile.Length == destinationFile.Length)
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
    }

    public abstract class SyncCommand
    {
        public FileSystemInfo FileSystemInfo { get; private set; }
        public string Path => FileSystemInfo.FullName;
        public SyncCommand(FileSystemInfo fileSystemInfo)
        {
            this.FileSystemInfo = fileSystemInfo;
        }
    }

    public class AddSyncCommand : SyncCommand
    {
        public AddSyncCommand(FileSystemInfo fileSystemInfo) : base(fileSystemInfo) { }
    }

    public class RemoveSyncCommand : SyncCommand
    {
        public RemoveSyncCommand(FileSystemInfo fileSystemInfo) : base(fileSystemInfo) { }
    }
}
