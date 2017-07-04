﻿namespace IB2ToolsetMini
{
    partial class MergerEditor
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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtFolderImport = new System.Windows.Forms.TextBox();
            this.lbxImport = new System.Windows.Forms.ListBox();
            this.pgImport = new System.Windows.Forms.PropertyGrid();
            this.btnFolderImport = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lbxMain = new System.Windows.Forms.ListBox();
            this.btnRemove = new System.Windows.Forms.Button();
            this.pgMain = new System.Windows.Forms.PropertyGrid();
            this.btnImport = new System.Windows.Forms.Button();
            this.cmbDataType = new System.Windows.Forms.ComboBox();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.btnInstructions = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtFolderImport);
            this.groupBox2.Controls.Add(this.lbxImport);
            this.groupBox2.Controls.Add(this.pgImport);
            this.groupBox2.Controls.Add(this.btnFolderImport);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox2.Size = new System.Drawing.Size(572, 423);
            this.groupBox2.TabIndex = 17;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Import Data";
            // 
            // txtFolderImport
            // 
            this.txtFolderImport.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFolderImport.Location = new System.Drawing.Point(8, 23);
            this.txtFolderImport.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtFolderImport.Name = "txtFolderImport";
            this.txtFolderImport.ReadOnly = true;
            this.txtFolderImport.Size = new System.Drawing.Size(506, 22);
            this.txtFolderImport.TabIndex = 3;
            // 
            // lbxImport
            // 
            this.lbxImport.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbxImport.FormattingEnabled = true;
            this.lbxImport.ItemHeight = 16;
            this.lbxImport.Location = new System.Drawing.Point(356, 66);
            this.lbxImport.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.lbxImport.Name = "lbxImport";
            this.lbxImport.Size = new System.Drawing.Size(205, 340);
            this.lbxImport.TabIndex = 0;
            this.lbxImport.SelectedIndexChanged += new System.EventHandler(this.lbxImport_SelectedIndexChanged);
            // 
            // pgImport
            // 
            this.pgImport.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pgImport.LineColor = System.Drawing.SystemColors.ControlDark;
            this.pgImport.Location = new System.Drawing.Point(8, 66);
            this.pgImport.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pgImport.Name = "pgImport";
            this.pgImport.Size = new System.Drawing.Size(340, 347);
            this.pgImport.TabIndex = 2;
            this.pgImport.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.pgImport_PropertyValueChanged);
            // 
            // btnFolderImport
            // 
            this.btnFolderImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFolderImport.Location = new System.Drawing.Point(524, 22);
            this.btnFolderImport.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnFolderImport.Name = "btnFolderImport";
            this.btnFolderImport.Size = new System.Drawing.Size(37, 28);
            this.btnFolderImport.TabIndex = 4;
            this.btnFolderImport.Text = "...";
            this.btnFolderImport.UseVisualStyleBackColor = true;
            this.btnFolderImport.Click += new System.EventHandler(this.btnFolderImport_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lbxMain);
            this.groupBox1.Controls.Add(this.btnRemove);
            this.groupBox1.Controls.Add(this.pgMain);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Size = new System.Drawing.Size(570, 423);
            this.groupBox1.TabIndex = 16;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Current Module Data";
            // 
            // lbxMain
            // 
            this.lbxMain.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lbxMain.FormattingEnabled = true;
            this.lbxMain.ItemHeight = 16;
            this.lbxMain.Location = new System.Drawing.Point(8, 66);
            this.lbxMain.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.lbxMain.Name = "lbxMain";
            this.lbxMain.Size = new System.Drawing.Size(205, 340);
            this.lbxMain.TabIndex = 5;
            this.lbxMain.SelectedIndexChanged += new System.EventHandler(this.lbxMain_SelectedIndexChanged);
            // 
            // btnRemove
            // 
            this.btnRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRemove.Location = new System.Drawing.Point(8, 559);
            this.btnRemove.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(207, 28);
            this.btnRemove.TabIndex = 11;
            this.btnRemove.Text = "Remove Selected";
            this.btnRemove.UseVisualStyleBackColor = true;
            // 
            // pgMain
            // 
            this.pgMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pgMain.LineColor = System.Drawing.SystemColors.ControlDark;
            this.pgMain.Location = new System.Drawing.Point(224, 66);
            this.pgMain.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pgMain.Name = "pgMain";
            this.pgMain.Size = new System.Drawing.Size(338, 347);
            this.pgMain.TabIndex = 7;
            this.pgMain.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.pgMain_PropertyValueChanged);
            // 
            // btnImport
            // 
            this.btnImport.Location = new System.Drawing.Point(481, 47);
            this.btnImport.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(220, 30);
            this.btnImport.TabIndex = 15;
            this.btnImport.Text = "<<< copy over selected <<<";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // cmbDataType
            // 
            this.cmbDataType.FormattingEnabled = true;
            this.cmbDataType.Items.AddRange(new object[] {
            "Creatures",
            "Items",
            "Areas",
            "Convos",
            "Encounters",
            "IBScripts"});
            this.cmbDataType.Location = new System.Drawing.Point(481, 14);
            this.cmbDataType.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cmbDataType.Name = "cmbDataType";
            this.cmbDataType.Size = new System.Drawing.Size(219, 24);
            this.cmbDataType.TabIndex = 14;
            this.cmbDataType.SelectedIndexChanged += new System.EventHandler(this.cmbDataType_SelectedIndexChanged);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(16, 79);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupBox2);
            this.splitContainer1.Size = new System.Drawing.Size(1147, 423);
            this.splitContainer1.SplitterDistance = 570;
            this.splitContainer1.SplitterWidth = 5;
            this.splitContainer1.TabIndex = 18;
            // 
            // btnInstructions
            // 
            this.btnInstructions.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnInstructions.Location = new System.Drawing.Point(864, 21);
            this.btnInstructions.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnInstructions.Name = "btnInstructions";
            this.btnInstructions.Size = new System.Drawing.Size(155, 39);
            this.btnInstructions.TabIndex = 19;
            this.btnInstructions.Text = "Instructions";
            this.btnInstructions.UseVisualStyleBackColor = true;
            this.btnInstructions.Click += new System.EventHandler(this.btnInstructions_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // MergerEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1179, 513);
            this.Controls.Add(this.btnInstructions);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.btnImport);
            this.Controls.Add(this.cmbDataType);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximumSize = new System.Drawing.Size(2661, 1466);
            this.MinimumSize = new System.Drawing.Size(1194, 549);
            this.Name = "MergerEditor";
            this.Text = "MergerEditor";
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtFolderImport;
        private System.Windows.Forms.ListBox lbxImport;
        private System.Windows.Forms.PropertyGrid pgImport;
        private System.Windows.Forms.Button btnFolderImport;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListBox lbxMain;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.PropertyGrid pgMain;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.ComboBox cmbDataType;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button btnInstructions;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}