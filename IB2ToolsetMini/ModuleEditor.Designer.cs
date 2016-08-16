namespace IB2ToolsetMini
{
    partial class ModuleEditor
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
            this.gbMoveDiagonalCost = new System.Windows.Forms.GroupBox();
            this.rbtnOnePointFiveSquares = new System.Windows.Forms.RadioButton();
            this.rbtnOneSquare = new System.Windows.Forms.RadioButton();
            this.rtxtInfo = new System.Windows.Forms.RichTextBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.gbToHitBonusFromBehind = new System.Windows.Forms.GroupBox();
            this.rbtnPlusOneToHitFromBehind = new System.Windows.Forms.RadioButton();
            this.rbtnPlusTwoToHitFromBehind = new System.Windows.Forms.RadioButton();
            this.rbtnPlusThreeToHitFromBehind = new System.Windows.Forms.RadioButton();
            this.rbtnPlusFourToHitFromBehind = new System.Windows.Forms.RadioButton();
            this.gbArmorClassDisplay = new System.Windows.Forms.GroupBox();
            this.rbtnDescendingAC = new System.Windows.Forms.RadioButton();
            this.rbtnAscendingAC = new System.Windows.Forms.RadioButton();
            this.gbRollingSystem = new System.Windows.Forms.GroupBox();
            this.rbtnUse3d6 = new System.Windows.Forms.RadioButton();
            this.rbtnUse6Plusd12 = new System.Windows.Forms.RadioButton();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.gbDescription = new System.Windows.Forms.GroupBox();
            this.pbModuleTitleImage = new System.Windows.Forms.PictureBox();
            this.gbTitleImage = new System.Windows.Forms.GroupBox();
            this.btnLoadTitleImage = new System.Windows.Forms.Button();
            this.flwArtResources = new System.Windows.Forms.FlowLayoutPanel();
            this.gbArtResources = new System.Windows.Forms.GroupBox();
            this.btnLoadArtResources = new System.Windows.Forms.Button();
            this.btnRemoveSelectedArtResource = new System.Windows.Forms.Button();
            this.gbMoveDiagonalCost.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.gbToHitBonusFromBehind.SuspendLayout();
            this.gbArmorClassDisplay.SuspendLayout();
            this.gbRollingSystem.SuspendLayout();
            this.gbDescription.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbModuleTitleImage)).BeginInit();
            this.gbTitleImage.SuspendLayout();
            this.gbArtResources.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbMoveDiagonalCost
            // 
            this.gbMoveDiagonalCost.Controls.Add(this.rbtnOnePointFiveSquares);
            this.gbMoveDiagonalCost.Controls.Add(this.rbtnOneSquare);
            this.gbMoveDiagonalCost.Location = new System.Drawing.Point(12, 12);
            this.gbMoveDiagonalCost.Name = "gbMoveDiagonalCost";
            this.gbMoveDiagonalCost.Size = new System.Drawing.Size(145, 64);
            this.gbMoveDiagonalCost.TabIndex = 0;
            this.gbMoveDiagonalCost.TabStop = false;
            this.gbMoveDiagonalCost.Text = "Diagonal Move Cost";
            this.gbMoveDiagonalCost.MouseHover += new System.EventHandler(this.gbMoveDiagonalCost_MouseHover);
            // 
            // rbtnOnePointFiveSquares
            // 
            this.rbtnOnePointFiveSquares.AutoSize = true;
            this.rbtnOnePointFiveSquares.Location = new System.Drawing.Point(11, 38);
            this.rbtnOnePointFiveSquares.Name = "rbtnOnePointFiveSquares";
            this.rbtnOnePointFiveSquares.Size = new System.Drawing.Size(88, 17);
            this.rbtnOnePointFiveSquares.TabIndex = 1;
            this.rbtnOnePointFiveSquares.TabStop = true;
            this.rbtnOnePointFiveSquares.Text = "1.5 per Move";
            this.rbtnOnePointFiveSquares.UseVisualStyleBackColor = true;
            this.rbtnOnePointFiveSquares.CheckedChanged += new System.EventHandler(this.rbtnOnePointFiveSquares_CheckedChanged);
            this.rbtnOnePointFiveSquares.MouseHover += new System.EventHandler(this.rbtnOnePointFiveSquares_MouseHover);
            // 
            // rbtnOneSquare
            // 
            this.rbtnOneSquare.AutoSize = true;
            this.rbtnOneSquare.Location = new System.Drawing.Point(11, 18);
            this.rbtnOneSquare.Name = "rbtnOneSquare";
            this.rbtnOneSquare.Size = new System.Drawing.Size(88, 17);
            this.rbtnOneSquare.TabIndex = 0;
            this.rbtnOneSquare.TabStop = true;
            this.rbtnOneSquare.Text = "1.0 per Move";
            this.rbtnOneSquare.UseVisualStyleBackColor = true;
            this.rbtnOneSquare.CheckedChanged += new System.EventHandler(this.rbtnOneSquare_CheckedChanged);
            this.rbtnOneSquare.MouseHover += new System.EventHandler(this.rbtnOneSquare_MouseHover);
            // 
            // rtxtInfo
            // 
            this.rtxtInfo.Location = new System.Drawing.Point(6, 19);
            this.rtxtInfo.Name = "rtxtInfo";
            this.rtxtInfo.Size = new System.Drawing.Size(183, 290);
            this.rtxtInfo.TabIndex = 1;
            this.rtxtInfo.Text = "";
            this.rtxtInfo.MouseHover += new System.EventHandler(this.rtxtInfo_MouseHover);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.gbArtResources);
            this.splitContainer1.Panel1.Controls.Add(this.gbTitleImage);
            this.splitContainer1.Panel1.Controls.Add(this.gbDescription);
            this.splitContainer1.Panel1.Controls.Add(this.gbToHitBonusFromBehind);
            this.splitContainer1.Panel1.Controls.Add(this.gbArmorClassDisplay);
            this.splitContainer1.Panel1.Controls.Add(this.gbMoveDiagonalCost);
            this.splitContainer1.Panel1.Controls.Add(this.gbRollingSystem);
            this.splitContainer1.Panel1.MouseHover += new System.EventHandler(this.splitContainer1_Panel1_MouseHover);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.propertyGrid1);
            this.splitContainer1.Size = new System.Drawing.Size(881, 620);
            this.splitContainer1.SplitterDistance = 588;
            this.splitContainer1.TabIndex = 2;
            // 
            // gbToHitBonusFromBehind
            // 
            this.gbToHitBonusFromBehind.Controls.Add(this.rbtnPlusOneToHitFromBehind);
            this.gbToHitBonusFromBehind.Controls.Add(this.rbtnPlusTwoToHitFromBehind);
            this.gbToHitBonusFromBehind.Controls.Add(this.rbtnPlusThreeToHitFromBehind);
            this.gbToHitBonusFromBehind.Controls.Add(this.rbtnPlusFourToHitFromBehind);
            this.gbToHitBonusFromBehind.Location = new System.Drawing.Point(12, 222);
            this.gbToHitBonusFromBehind.Name = "gbToHitBonusFromBehind";
            this.gbToHitBonusFromBehind.Size = new System.Drawing.Size(145, 118);
            this.gbToHitBonusFromBehind.TabIndex = 2;
            this.gbToHitBonusFromBehind.TabStop = false;
            this.gbToHitBonusFromBehind.Text = "To Hit Bonus From Behind";
            this.gbToHitBonusFromBehind.MouseHover += new System.EventHandler(this.gbToHitBonusFromBehind_MouseHover);
            // 
            // rbtnPlusOneToHitFromBehind
            // 
            this.rbtnPlusOneToHitFromBehind.AutoSize = true;
            this.rbtnPlusOneToHitFromBehind.Location = new System.Drawing.Point(11, 19);
            this.rbtnPlusOneToHitFromBehind.Name = "rbtnPlusOneToHitFromBehind";
            this.rbtnPlusOneToHitFromBehind.Size = new System.Drawing.Size(121, 17);
            this.rbtnPlusOneToHitFromBehind.TabIndex = 0;
            this.rbtnPlusOneToHitFromBehind.TabStop = true;
            this.rbtnPlusOneToHitFromBehind.Text = "+1 to hit from behind";
            this.rbtnPlusOneToHitFromBehind.UseVisualStyleBackColor = true;
            this.rbtnPlusOneToHitFromBehind.CheckedChanged += new System.EventHandler(this.rbtnPlusOneToHitFromBehind_CheckedChanged);
            this.rbtnPlusOneToHitFromBehind.MouseHover += new System.EventHandler(this.rbtnPlusOneToHitFromBehind_MouseHover);
            // 
            // rbtnPlusTwoToHitFromBehind
            // 
            this.rbtnPlusTwoToHitFromBehind.AutoSize = true;
            this.rbtnPlusTwoToHitFromBehind.Location = new System.Drawing.Point(11, 42);
            this.rbtnPlusTwoToHitFromBehind.Name = "rbtnPlusTwoToHitFromBehind";
            this.rbtnPlusTwoToHitFromBehind.Size = new System.Drawing.Size(121, 17);
            this.rbtnPlusTwoToHitFromBehind.TabIndex = 1;
            this.rbtnPlusTwoToHitFromBehind.TabStop = true;
            this.rbtnPlusTwoToHitFromBehind.Text = "+2 to hit from behind";
            this.rbtnPlusTwoToHitFromBehind.UseVisualStyleBackColor = true;
            this.rbtnPlusTwoToHitFromBehind.CheckedChanged += new System.EventHandler(this.rbtnPlusTwoToHitFromBehind_CheckedChanged);
            this.rbtnPlusTwoToHitFromBehind.MouseHover += new System.EventHandler(this.rbtnPlusTwoToHitFromBehind_MouseHover);
            // 
            // rbtnPlusThreeToHitFromBehind
            // 
            this.rbtnPlusThreeToHitFromBehind.AutoSize = true;
            this.rbtnPlusThreeToHitFromBehind.Location = new System.Drawing.Point(11, 65);
            this.rbtnPlusThreeToHitFromBehind.Name = "rbtnPlusThreeToHitFromBehind";
            this.rbtnPlusThreeToHitFromBehind.Size = new System.Drawing.Size(121, 17);
            this.rbtnPlusThreeToHitFromBehind.TabIndex = 2;
            this.rbtnPlusThreeToHitFromBehind.TabStop = true;
            this.rbtnPlusThreeToHitFromBehind.Text = "+3 to hit from behind";
            this.rbtnPlusThreeToHitFromBehind.UseVisualStyleBackColor = true;
            this.rbtnPlusThreeToHitFromBehind.CheckedChanged += new System.EventHandler(this.rbtnPlusThreeToHitFromBehind_CheckedChanged);
            this.rbtnPlusThreeToHitFromBehind.MouseHover += new System.EventHandler(this.rbtnPlusThreeToHitFromBehind_MouseHover);
            // 
            // rbtnPlusFourToHitFromBehind
            // 
            this.rbtnPlusFourToHitFromBehind.AutoSize = true;
            this.rbtnPlusFourToHitFromBehind.Location = new System.Drawing.Point(11, 88);
            this.rbtnPlusFourToHitFromBehind.Name = "rbtnPlusFourToHitFromBehind";
            this.rbtnPlusFourToHitFromBehind.Size = new System.Drawing.Size(121, 17);
            this.rbtnPlusFourToHitFromBehind.TabIndex = 3;
            this.rbtnPlusFourToHitFromBehind.TabStop = true;
            this.rbtnPlusFourToHitFromBehind.Text = "+4 to hit from behind";
            this.rbtnPlusFourToHitFromBehind.UseVisualStyleBackColor = true;
            this.rbtnPlusFourToHitFromBehind.CheckedChanged += new System.EventHandler(this.rbtnPlusFourToHitFromBehind_CheckedChanged);
            this.rbtnPlusFourToHitFromBehind.MouseHover += new System.EventHandler(this.rbtnPlusFourToHitFromBehind_MouseHover);
            // 
            // gbArmorClassDisplay
            // 
            this.gbArmorClassDisplay.Controls.Add(this.rbtnDescendingAC);
            this.gbArmorClassDisplay.Controls.Add(this.rbtnAscendingAC);
            this.gbArmorClassDisplay.Location = new System.Drawing.Point(12, 82);
            this.gbArmorClassDisplay.Name = "gbArmorClassDisplay";
            this.gbArmorClassDisplay.Size = new System.Drawing.Size(145, 64);
            this.gbArmorClassDisplay.TabIndex = 1;
            this.gbArmorClassDisplay.TabStop = false;
            this.gbArmorClassDisplay.Text = "Armor Class Shown";
            this.gbArmorClassDisplay.MouseHover += new System.EventHandler(this.gbArmorClassDisplay_MouseHover);
            // 
            // rbtnDescendingAC
            // 
            this.rbtnDescendingAC.AutoSize = true;
            this.rbtnDescendingAC.Location = new System.Drawing.Point(11, 38);
            this.rbtnDescendingAC.Name = "rbtnDescendingAC";
            this.rbtnDescendingAC.Size = new System.Drawing.Size(82, 17);
            this.rbtnDescendingAC.TabIndex = 1;
            this.rbtnDescendingAC.TabStop = true;
            this.rbtnDescendingAC.Text = "Descending";
            this.rbtnDescendingAC.UseVisualStyleBackColor = true;
            this.rbtnDescendingAC.CheckedChanged += new System.EventHandler(this.rbtnDescendingAC_CheckedChanged);
            this.rbtnDescendingAC.MouseHover += new System.EventHandler(this.rbtnDescendingAC_MouseHover);
            // 
            // rbtnAscendingAC
            // 
            this.rbtnAscendingAC.AutoSize = true;
            this.rbtnAscendingAC.Location = new System.Drawing.Point(11, 18);
            this.rbtnAscendingAC.Name = "rbtnAscendingAC";
            this.rbtnAscendingAC.Size = new System.Drawing.Size(75, 17);
            this.rbtnAscendingAC.TabIndex = 0;
            this.rbtnAscendingAC.TabStop = true;
            this.rbtnAscendingAC.Text = "Ascending";
            this.rbtnAscendingAC.UseVisualStyleBackColor = true;
            this.rbtnAscendingAC.CheckedChanged += new System.EventHandler(this.rbtnAscendingAC_CheckedChanged);
            this.rbtnAscendingAC.MouseHover += new System.EventHandler(this.rbtnAscendingAC_MouseHover);
            // 
            // gbRollingSystem
            // 
            this.gbRollingSystem.Controls.Add(this.rbtnUse3d6);
            this.gbRollingSystem.Controls.Add(this.rbtnUse6Plusd12);
            this.gbRollingSystem.Location = new System.Drawing.Point(12, 152);
            this.gbRollingSystem.Name = "gbRollingSystem";
            this.gbRollingSystem.Size = new System.Drawing.Size(145, 64);
            this.gbRollingSystem.TabIndex = 1;
            this.gbRollingSystem.TabStop = false;
            this.gbRollingSystem.Text = "Attribute System";
            this.gbRollingSystem.MouseHover += new System.EventHandler(this.gbRollingSystem_MouseHover);
            // 
            // rbtnUse3d6
            // 
            this.rbtnUse3d6.AutoSize = true;
            this.rbtnUse3d6.Location = new System.Drawing.Point(11, 38);
            this.rbtnUse3d6.Name = "rbtnUse3d6";
            this.rbtnUse3d6.Size = new System.Drawing.Size(65, 17);
            this.rbtnUse3d6.TabIndex = 1;
            this.rbtnUse3d6.TabStop = true;
            this.rbtnUse3d6.Text = "Use 3d6";
            this.rbtnUse3d6.UseVisualStyleBackColor = true;
            this.rbtnUse3d6.CheckedChanged += new System.EventHandler(this.rbtnUse3d6_CheckedChanged);
            this.rbtnUse3d6.MouseHover += new System.EventHandler(this.rbtnUse3d6_MouseHover);
            // 
            // rbtnUse6Plusd12
            // 
            this.rbtnUse6Plusd12.AutoSize = true;
            this.rbtnUse6Plusd12.Location = new System.Drawing.Point(11, 18);
            this.rbtnUse6Plusd12.Name = "rbtnUse6Plusd12";
            this.rbtnUse6Plusd12.Size = new System.Drawing.Size(77, 17);
            this.rbtnUse6Plusd12.TabIndex = 0;
            this.rbtnUse6Plusd12.TabStop = true;
            this.rbtnUse6Plusd12.Text = "Use 2d6+6";
            this.rbtnUse6Plusd12.UseVisualStyleBackColor = true;
            this.rbtnUse6Plusd12.CheckedChanged += new System.EventHandler(this.rbtnUse6Plusd12_CheckedChanged);
            this.rbtnUse6Plusd12.MouseHover += new System.EventHandler(this.rbtnUse6Plusd12_MouseHover);
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid1.Location = new System.Drawing.Point(0, 0);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(289, 620);
            this.propertyGrid1.TabIndex = 1;
            // 
            // gbDescription
            // 
            this.gbDescription.Controls.Add(this.rtxtInfo);
            this.gbDescription.Location = new System.Drawing.Point(382, 12);
            this.gbDescription.Name = "gbDescription";
            this.gbDescription.Size = new System.Drawing.Size(195, 315);
            this.gbDescription.TabIndex = 3;
            this.gbDescription.TabStop = false;
            this.gbDescription.Text = "Description";
            // 
            // pbModuleTitleImage
            // 
            this.pbModuleTitleImage.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pbModuleTitleImage.Location = new System.Drawing.Point(6, 19);
            this.pbModuleTitleImage.Name = "pbModuleTitleImage";
            this.pbModuleTitleImage.Size = new System.Drawing.Size(200, 100);
            this.pbModuleTitleImage.TabIndex = 4;
            this.pbModuleTitleImage.TabStop = false;
            // 
            // gbTitleImage
            // 
            this.gbTitleImage.Controls.Add(this.btnLoadTitleImage);
            this.gbTitleImage.Controls.Add(this.pbModuleTitleImage);
            this.gbTitleImage.Location = new System.Drawing.Point(163, 12);
            this.gbTitleImage.Name = "gbTitleImage";
            this.gbTitleImage.Size = new System.Drawing.Size(213, 154);
            this.gbTitleImage.TabIndex = 5;
            this.gbTitleImage.TabStop = false;
            this.gbTitleImage.Text = "Module Title Image";
            // 
            // btnLoadTitleImage
            // 
            this.btnLoadTitleImage.Location = new System.Drawing.Point(6, 124);
            this.btnLoadTitleImage.Name = "btnLoadTitleImage";
            this.btnLoadTitleImage.Size = new System.Drawing.Size(200, 23);
            this.btnLoadTitleImage.TabIndex = 5;
            this.btnLoadTitleImage.Text = "Load Title Image";
            this.btnLoadTitleImage.UseVisualStyleBackColor = true;
            this.btnLoadTitleImage.Click += new System.EventHandler(this.btnLoadTitleImage_Click);
            // 
            // flwArtResources
            // 
            this.flwArtResources.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flwArtResources.AutoScroll = true;
            this.flwArtResources.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.flwArtResources.Location = new System.Drawing.Point(6, 48);
            this.flwArtResources.Name = "flwArtResources";
            this.flwArtResources.Size = new System.Drawing.Size(553, 208);
            this.flwArtResources.TabIndex = 6;
            // 
            // gbArtResources
            // 
            this.gbArtResources.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbArtResources.Controls.Add(this.btnRemoveSelectedArtResource);
            this.gbArtResources.Controls.Add(this.btnLoadArtResources);
            this.gbArtResources.Controls.Add(this.flwArtResources);
            this.gbArtResources.Location = new System.Drawing.Point(12, 346);
            this.gbArtResources.Name = "gbArtResources";
            this.gbArtResources.Size = new System.Drawing.Size(565, 262);
            this.gbArtResources.TabIndex = 7;
            this.gbArtResources.TabStop = false;
            this.gbArtResources.Text = "Module Custom Art Resources (Embedded)";
            // 
            // btnLoadArtResources
            // 
            this.btnLoadArtResources.Location = new System.Drawing.Point(6, 19);
            this.btnLoadArtResources.Name = "btnLoadArtResources";
            this.btnLoadArtResources.Size = new System.Drawing.Size(145, 23);
            this.btnLoadArtResources.TabIndex = 7;
            this.btnLoadArtResources.Text = "Load More Art Resources";
            this.btnLoadArtResources.UseVisualStyleBackColor = true;
            this.btnLoadArtResources.Click += new System.EventHandler(this.btnLoadArtResources_Click);
            // 
            // btnRemoveSelectedArtResource
            // 
            this.btnRemoveSelectedArtResource.Location = new System.Drawing.Point(157, 19);
            this.btnRemoveSelectedArtResource.Name = "btnRemoveSelectedArtResource";
            this.btnRemoveSelectedArtResource.Size = new System.Drawing.Size(244, 23);
            this.btnRemoveSelectedArtResource.TabIndex = 8;
            this.btnRemoveSelectedArtResource.Text = "Remove the Currently Selected Art Resource";
            this.btnRemoveSelectedArtResource.UseVisualStyleBackColor = true;
            this.btnRemoveSelectedArtResource.Click += new System.EventHandler(this.btnRemoveSelectedArtResource_Click);
            // 
            // ModuleEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(881, 620);
            this.Controls.Add(this.splitContainer1);
            this.Name = "ModuleEditor";
            this.Text = "ModuleEditor";
            this.gbMoveDiagonalCost.ResumeLayout(false);
            this.gbMoveDiagonalCost.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.gbToHitBonusFromBehind.ResumeLayout(false);
            this.gbToHitBonusFromBehind.PerformLayout();
            this.gbArmorClassDisplay.ResumeLayout(false);
            this.gbArmorClassDisplay.PerformLayout();
            this.gbRollingSystem.ResumeLayout(false);
            this.gbRollingSystem.PerformLayout();
            this.gbDescription.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbModuleTitleImage)).EndInit();
            this.gbTitleImage.ResumeLayout(false);
            this.gbArtResources.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbMoveDiagonalCost;
        private System.Windows.Forms.RadioButton rbtnOnePointFiveSquares;
        private System.Windows.Forms.RadioButton rbtnOneSquare;
        private System.Windows.Forms.RichTextBox rtxtInfo;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox gbArmorClassDisplay;
        private System.Windows.Forms.RadioButton rbtnDescendingAC;
        private System.Windows.Forms.RadioButton rbtnAscendingAC;
        private System.Windows.Forms.GroupBox gbToHitBonusFromBehind;
        private System.Windows.Forms.RadioButton rbtnPlusOneToHitFromBehind;
        private System.Windows.Forms.RadioButton rbtnPlusTwoToHitFromBehind;
        private System.Windows.Forms.RadioButton rbtnPlusThreeToHitFromBehind;
        private System.Windows.Forms.RadioButton rbtnPlusFourToHitFromBehind;
        private System.Windows.Forms.GroupBox gbRollingSystem;
        private System.Windows.Forms.RadioButton rbtnUse3d6;
        private System.Windows.Forms.RadioButton rbtnUse6Plusd12;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.GroupBox gbTitleImage;
        private System.Windows.Forms.Button btnLoadTitleImage;
        private System.Windows.Forms.PictureBox pbModuleTitleImage;
        private System.Windows.Forms.GroupBox gbDescription;
        private System.Windows.Forms.GroupBox gbArtResources;
        private System.Windows.Forms.Button btnRemoveSelectedArtResource;
        private System.Windows.Forms.Button btnLoadArtResources;
        private System.Windows.Forms.FlowLayoutPanel flwArtResources;
    }
}