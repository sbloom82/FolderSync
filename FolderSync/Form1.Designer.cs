namespace FolderSync
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.tvSource = new System.Windows.Forms.TreeView();
            this.panel6 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.txtSourcePath = new System.Windows.Forms.TextBox();
            this.btnLoadSource = new System.Windows.Forms.Button();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.panel2 = new System.Windows.Forms.Panel();
            this.tvDestination = new System.Windows.Forms.TreeView();
            this.panel7 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.txtDestinationPath = new System.Windows.Forms.TextBox();
            this.btnLoadDestination = new System.Windows.Forms.Button();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.txtStatus = new System.Windows.Forms.TextBox();
            this.btnSync = new System.Windows.Forms.Button();
            this.splitter2 = new System.Windows.Forms.Splitter();
            this.splitter3 = new System.Windows.Forms.Splitter();
            this.btnChooseSource = new System.Windows.Forms.Button();
            this.btnDestinationChoose = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel7.SuspendLayout();
            this.panel5.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.splitter1);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 25);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(878, 509);
            this.panel1.TabIndex = 0;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.tvSource);
            this.panel3.Controls.Add(this.panel6);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(436, 509);
            this.panel3.TabIndex = 3;
            // 
            // tvSource
            // 
            this.tvSource.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvSource.Location = new System.Drawing.Point(0, 82);
            this.tvSource.Name = "tvSource";
            this.tvSource.Size = new System.Drawing.Size(436, 427);
            this.tvSource.TabIndex = 1;
            this.tvSource.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.tvSource_BeforeExpand);
            this.tvSource.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvSource_NodeMouseDoubleClick);
            this.tvSource.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tvSource_KeyPress);
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.btnChooseSource);
            this.panel6.Controls.Add(this.label1);
            this.panel6.Controls.Add(this.txtSourcePath);
            this.panel6.Controls.Add(this.btnLoadSource);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel6.Location = new System.Drawing.Point(0, 0);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(436, 82);
            this.panel6.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 17);
            this.label1.TabIndex = 3;
            this.label1.Text = "Source";
            // 
            // txtSourcePath
            // 
            this.txtSourcePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSourcePath.Location = new System.Drawing.Point(6, 23);
            this.txtSourcePath.Name = "txtSourcePath";
            this.txtSourcePath.Size = new System.Drawing.Size(383, 22);
            this.txtSourcePath.TabIndex = 0;
            // 
            // btnLoadSource
            // 
            this.btnLoadSource.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLoadSource.Location = new System.Drawing.Point(6, 51);
            this.btnLoadSource.Name = "btnLoadSource";
            this.btnLoadSource.Size = new System.Drawing.Size(424, 23);
            this.btnLoadSource.TabIndex = 2;
            this.btnLoadSource.Text = "Load";
            this.btnLoadSource.UseVisualStyleBackColor = true;
            this.btnLoadSource.Click += new System.EventHandler(this.btnLoadSource_Click);
            // 
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Right;
            this.splitter1.Location = new System.Drawing.Point(436, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 509);
            this.splitter1.TabIndex = 5;
            this.splitter1.TabStop = false;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.tvDestination);
            this.panel2.Controls.Add(this.panel7);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(439, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(439, 509);
            this.panel2.TabIndex = 4;
            // 
            // tvDestination
            // 
            this.tvDestination.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvDestination.Location = new System.Drawing.Point(0, 82);
            this.tvDestination.Name = "tvDestination";
            this.tvDestination.Size = new System.Drawing.Size(439, 427);
            this.tvDestination.TabIndex = 2;
            this.tvDestination.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.tvDestination_BeforeExpand);
            this.tvDestination.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvDestination_NodeMouseDoubleClick);
            this.tvDestination.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tvDestination_KeyPress);
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.btnDestinationChoose);
            this.panel7.Controls.Add(this.label2);
            this.panel7.Controls.Add(this.txtDestinationPath);
            this.panel7.Controls.Add(this.btnLoadDestination);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel7.Location = new System.Drawing.Point(0, 0);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(439, 82);
            this.panel7.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 17);
            this.label2.TabIndex = 3;
            this.label2.Text = "Destination";
            // 
            // txtDestinationPath
            // 
            this.txtDestinationPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDestinationPath.Location = new System.Drawing.Point(6, 23);
            this.txtDestinationPath.Name = "txtDestinationPath";
            this.txtDestinationPath.Size = new System.Drawing.Size(385, 22);
            this.txtDestinationPath.TabIndex = 0;
            // 
            // btnLoadDestination
            // 
            this.btnLoadDestination.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLoadDestination.Location = new System.Drawing.Point(6, 51);
            this.btnLoadDestination.Name = "btnLoadDestination";
            this.btnLoadDestination.Size = new System.Drawing.Size(426, 23);
            this.btnLoadDestination.TabIndex = 2;
            this.btnLoadDestination.Text = "Load";
            this.btnLoadDestination.UseVisualStyleBackColor = true;
            this.btnLoadDestination.Click += new System.EventHandler(this.btnLoadDestination_Click);
            // 
            // panel4
            // 
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(878, 22);
            this.panel4.TabIndex = 1;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.txtStatus);
            this.panel5.Controls.Add(this.btnSync);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel5.Location = new System.Drawing.Point(0, 537);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(878, 188);
            this.panel5.TabIndex = 2;
            // 
            // txtStatus
            // 
            this.txtStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtStatus.Location = new System.Drawing.Point(0, 23);
            this.txtStatus.Multiline = true;
            this.txtStatus.Name = "txtStatus";
            this.txtStatus.ReadOnly = true;
            this.txtStatus.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtStatus.Size = new System.Drawing.Size(878, 165);
            this.txtStatus.TabIndex = 1;
            // 
            // btnSync
            // 
            this.btnSync.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnSync.Location = new System.Drawing.Point(0, 0);
            this.btnSync.Name = "btnSync";
            this.btnSync.Size = new System.Drawing.Size(878, 23);
            this.btnSync.TabIndex = 0;
            this.btnSync.Text = "Sync";
            this.btnSync.UseVisualStyleBackColor = true;
            this.btnSync.Click += new System.EventHandler(this.btnSync_Click);
            // 
            // splitter2
            // 
            this.splitter2.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitter2.Location = new System.Drawing.Point(0, 22);
            this.splitter2.Name = "splitter2";
            this.splitter2.Size = new System.Drawing.Size(878, 3);
            this.splitter2.TabIndex = 3;
            this.splitter2.TabStop = false;
            // 
            // splitter3
            // 
            this.splitter3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter3.Location = new System.Drawing.Point(0, 534);
            this.splitter3.Name = "splitter3";
            this.splitter3.Size = new System.Drawing.Size(878, 3);
            this.splitter3.TabIndex = 4;
            this.splitter3.TabStop = false;
            // 
            // btnChooseSource
            // 
            this.btnChooseSource.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnChooseSource.Location = new System.Drawing.Point(395, 23);
            this.btnChooseSource.Name = "btnChooseSource";
            this.btnChooseSource.Size = new System.Drawing.Size(35, 23);
            this.btnChooseSource.TabIndex = 4;
            this.btnChooseSource.Text = "...";
            this.btnChooseSource.UseVisualStyleBackColor = true;
            this.btnChooseSource.Click += new System.EventHandler(this.btnChooseSource_Click);
            // 
            // btnDestinationChoose
            // 
            this.btnDestinationChoose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDestinationChoose.Location = new System.Drawing.Point(397, 22);
            this.btnDestinationChoose.Name = "btnDestinationChoose";
            this.btnDestinationChoose.Size = new System.Drawing.Size(35, 23);
            this.btnDestinationChoose.TabIndex = 5;
            this.btnDestinationChoose.Text = "...";
            this.btnDestinationChoose.UseVisualStyleBackColor = true;
            this.btnDestinationChoose.Click += new System.EventHandler(this.btnDestinationChoose_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(878, 725);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.splitter3);
            this.Controls.Add(this.splitter2);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel4);
            this.Name = "Form1";
            this.Text = "Folder Sync";
            this.panel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TreeView tvDestination;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btnLoadSource;
        private System.Windows.Forms.TreeView tvSource;
        private System.Windows.Forms.TextBox txtSourcePath;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Splitter splitter2;
        private System.Windows.Forms.Splitter splitter3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtDestinationPath;
        private System.Windows.Forms.Button btnLoadDestination;
        private System.Windows.Forms.Button btnSync;
        private System.Windows.Forms.TextBox txtStatus;
        private System.Windows.Forms.Button btnChooseSource;
        private System.Windows.Forms.Button btnDestinationChoose;
    }
}

