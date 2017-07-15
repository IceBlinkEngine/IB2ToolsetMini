namespace IB2ToolsetMini
{
    partial class IconSprite
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
            this.gbCreatureIconSelect = new System.Windows.Forms.GroupBox();
            this.pbIcon = new System.Windows.Forms.PictureBox();
            this.btnSelectIcon = new System.Windows.Forms.Button();
            this.gbUsableByClass = new System.Windows.Forms.GroupBox();
            this.cbxUsableByClass = new System.Windows.Forms.CheckedListBox();
            this.gbKnownSpells = new System.Windows.Forms.GroupBox();
            this.cbxKnownSpells = new System.Windows.Forms.CheckedListBox();
            this.gbCreatureIconSelect.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbIcon)).BeginInit();
            this.gbUsableByClass.SuspendLayout();
            this.gbKnownSpells.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbCreatureIconSelect
            // 
            this.gbCreatureIconSelect.Controls.Add(this.pbIcon);
            this.gbCreatureIconSelect.Controls.Add(this.btnSelectIcon);
            this.gbCreatureIconSelect.Location = new System.Drawing.Point(12, 12);
            this.gbCreatureIconSelect.Name = "gbCreatureIconSelect";
            this.gbCreatureIconSelect.Size = new System.Drawing.Size(188, 214);
            this.gbCreatureIconSelect.TabIndex = 44;
            this.gbCreatureIconSelect.TabStop = false;
            this.gbCreatureIconSelect.Text = "Icon/Sprite";
            // 
            // pbIcon
            // 
            this.pbIcon.BackColor = System.Drawing.Color.Silver;
            this.pbIcon.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pbIcon.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pbIcon.Location = new System.Drawing.Point(16, 19);
            this.pbIcon.Name = "pbIcon";
            this.pbIcon.Padding = new System.Windows.Forms.Padding(2);
            this.pbIcon.Size = new System.Drawing.Size(154, 154);
            this.pbIcon.TabIndex = 25;
            this.pbIcon.TabStop = false;
            // 
            // btnSelectIcon
            // 
            this.btnSelectIcon.Location = new System.Drawing.Point(16, 179);
            this.btnSelectIcon.Name = "btnSelectIcon";
            this.btnSelectIcon.Size = new System.Drawing.Size(154, 26);
            this.btnSelectIcon.TabIndex = 26;
            this.btnSelectIcon.Text = "Select";
            this.btnSelectIcon.UseVisualStyleBackColor = true;
            this.btnSelectIcon.Click += new System.EventHandler(this.btnSelectIcon_Click);
            // 
            // gbUsableByClass
            // 
            this.gbUsableByClass.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbUsableByClass.Controls.Add(this.cbxUsableByClass);
            this.gbUsableByClass.Location = new System.Drawing.Point(12, 232);
            this.gbUsableByClass.Name = "gbUsableByClass";
            this.gbUsableByClass.Size = new System.Drawing.Size(188, 185);
            this.gbUsableByClass.TabIndex = 45;
            this.gbUsableByClass.TabStop = false;
            this.gbUsableByClass.Text = "Usable by Class";
            // 
            // cbxUsableByClass
            // 
            this.cbxUsableByClass.CheckOnClick = true;
            this.cbxUsableByClass.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbxUsableByClass.FormattingEnabled = true;
            this.cbxUsableByClass.Location = new System.Drawing.Point(3, 19);
            this.cbxUsableByClass.Name = "cbxUsableByClass";
            this.cbxUsableByClass.ScrollAlwaysVisible = true;
            this.cbxUsableByClass.Size = new System.Drawing.Size(182, 163);
            this.cbxUsableByClass.TabIndex = 1;
            this.cbxUsableByClass.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.cbxUsableByClass_ItemCheck);
            // 
            // gbKnownSpells
            // 
            this.gbKnownSpells.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbKnownSpells.Controls.Add(this.cbxKnownSpells);
            this.gbKnownSpells.Location = new System.Drawing.Point(12, 423);
            this.gbKnownSpells.Name = "gbKnownSpells";
            this.gbKnownSpells.Size = new System.Drawing.Size(188, 297);
            this.gbKnownSpells.TabIndex = 4;
            this.gbKnownSpells.TabStop = false;
            this.gbKnownSpells.Text = "Known Spells";
            // 
            // cbxKnownSpells
            // 
            this.cbxKnownSpells.CheckOnClick = true;
            this.cbxKnownSpells.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbxKnownSpells.FormattingEnabled = true;
            this.cbxKnownSpells.Location = new System.Drawing.Point(3, 19);
            this.cbxKnownSpells.Name = "cbxKnownSpells";
            this.cbxKnownSpells.ScrollAlwaysVisible = true;
            this.cbxKnownSpells.Size = new System.Drawing.Size(182, 275);
            this.cbxKnownSpells.TabIndex = 1;
            this.cbxKnownSpells.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.cbxKnownSpells_ItemCheck);
            // 
            // IconSprite
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(213, 732);
            this.Controls.Add(this.gbKnownSpells);
            this.Controls.Add(this.gbUsableByClass);
            this.Controls.Add(this.gbCreatureIconSelect);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HideOnClose = true;
            this.Name = "IconSprite";
            this.Text = "IconSprite";
            this.gbCreatureIconSelect.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbIcon)).EndInit();
            this.gbUsableByClass.ResumeLayout(false);
            this.gbKnownSpells.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.GroupBox gbCreatureIconSelect;
        public System.Windows.Forms.PictureBox pbIcon;
        public System.Windows.Forms.Button btnSelectIcon;
        private System.Windows.Forms.CheckedListBox cbxKnownSpells;
        public System.Windows.Forms.GroupBox gbKnownSpells;
        public System.Windows.Forms.GroupBox gbUsableByClass;
        private System.Windows.Forms.CheckedListBox cbxUsableByClass;
    }
}