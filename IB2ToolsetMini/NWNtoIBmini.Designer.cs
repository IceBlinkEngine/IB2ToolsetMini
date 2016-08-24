namespace IB2ToolsetMini
{
    partial class NWNtoIBmini
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
            this.btnConvertFromModFolder = new System.Windows.Forms.Button();
            this.btnConvertFromModFile = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // btnConvertFromModFolder
            // 
            this.btnConvertFromModFolder.Location = new System.Drawing.Point(12, 79);
            this.btnConvertFromModFolder.Name = "btnConvertFromModFolder";
            this.btnConvertFromModFolder.Size = new System.Drawing.Size(260, 50);
            this.btnConvertFromModFolder.TabIndex = 3;
            this.btnConvertFromModFolder.Text = "Convert a NWN2 module Folder to IBmini";
            this.btnConvertFromModFolder.UseVisualStyleBackColor = true;
            this.btnConvertFromModFolder.Click += new System.EventHandler(this.btnConvertFromModFolder_Click);
            // 
            // btnConvertFromModFile
            // 
            this.btnConvertFromModFile.Location = new System.Drawing.Point(12, 12);
            this.btnConvertFromModFile.Name = "btnConvertFromModFile";
            this.btnConvertFromModFile.Size = new System.Drawing.Size(260, 50);
            this.btnConvertFromModFile.TabIndex = 2;
            this.btnConvertFromModFile.Text = "Convert a NWN2 .mod File to IBmini";
            this.btnConvertFromModFile.UseVisualStyleBackColor = true;
            this.btnConvertFromModFile.Click += new System.EventHandler(this.btnConvertFromModFile_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // NWNtoIBmini
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 148);
            this.Controls.Add(this.btnConvertFromModFolder);
            this.Controls.Add(this.btnConvertFromModFile);
            this.Name = "NWNtoIBmini";
            this.Text = "NWNtoIBmini";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnConvertFromModFolder;
        private System.Windows.Forms.Button btnConvertFromModFile;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}