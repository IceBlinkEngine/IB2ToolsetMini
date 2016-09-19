namespace IB2ToolsetMini
{
    partial class PlayerEditor
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
            this.gbKnownSpells = new System.Windows.Forms.GroupBox();
            this.cbxKnownSpells = new System.Windows.Forms.CheckedListBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label10 = new System.Windows.Forms.Label();
            this.cmbAmmo = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cmbFeet = new System.Windows.Forms.ComboBox();
            this.cmbRing2 = new System.Windows.Forms.ComboBox();
            this.cmbRing1 = new System.Windows.Forms.ComboBox();
            this.cmbOffHand = new System.Windows.Forms.ComboBox();
            this.cmbMainHand = new System.Windows.Forms.ComboBox();
            this.cmbBody = new System.Windows.Forms.ComboBox();
            this.cmbNeck = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbHead = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbxKnownTraits = new System.Windows.Forms.CheckedListBox();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.gbCreatureIconSelect = new System.Windows.Forms.GroupBox();
            this.pbIcon = new System.Windows.Forms.PictureBox();
            this.btnSelectIcon = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnSort = new System.Windows.Forms.Button();
            this.btnDuplicate = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.lbxPlayers = new System.Windows.Forms.ListBox();
            this.pbPortrait = new System.Windows.Forms.PictureBox();
            this.btnSelectPortrait = new System.Windows.Forms.Button();
            this.gbKnownSpells.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.gbCreatureIconSelect.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbIcon)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbPortrait)).BeginInit();
            this.SuspendLayout();
            // 
            // gbKnownSpells
            // 
            this.gbKnownSpells.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.gbKnownSpells.Controls.Add(this.cbxKnownSpells);
            this.gbKnownSpells.Location = new System.Drawing.Point(683, 8);
            this.gbKnownSpells.Name = "gbKnownSpells";
            this.gbKnownSpells.Size = new System.Drawing.Size(196, 305);
            this.gbKnownSpells.TabIndex = 5;
            this.gbKnownSpells.TabStop = false;
            this.gbKnownSpells.Text = "Known Spells";
            // 
            // cbxKnownSpells
            // 
            this.cbxKnownSpells.CheckOnClick = true;
            this.cbxKnownSpells.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbxKnownSpells.FormattingEnabled = true;
            this.cbxKnownSpells.Location = new System.Drawing.Point(3, 16);
            this.cbxKnownSpells.Name = "cbxKnownSpells";
            this.cbxKnownSpells.ScrollAlwaysVisible = true;
            this.cbxKnownSpells.Size = new System.Drawing.Size(190, 286);
            this.cbxKnownSpells.TabIndex = 1;
            this.cbxKnownSpells.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.cbxKnownSpells_ItemCheck);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.cmbAmmo);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.cmbFeet);
            this.groupBox2.Controls.Add(this.cmbRing2);
            this.groupBox2.Controls.Add(this.cmbRing1);
            this.groupBox2.Controls.Add(this.cmbOffHand);
            this.groupBox2.Controls.Add(this.cmbMainHand);
            this.groupBox2.Controls.Add(this.cmbBody);
            this.groupBox2.Controls.Add(this.cmbNeck);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.cmbHead);
            this.groupBox2.Location = new System.Drawing.Point(451, 8);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(222, 331);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Equipped Items";
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(72, 277);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(141, 46);
            this.label10.TabIndex = 87;
            this.label10.Text = "The default quantity of this ammo item will be added to the party equipment list." +
    "";
            // 
            // cmbAmmo
            // 
            this.cmbAmmo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAmmo.FormattingEnabled = true;
            this.cmbAmmo.Location = new System.Drawing.Point(75, 253);
            this.cmbAmmo.Name = "cmbAmmo";
            this.cmbAmmo.Size = new System.Drawing.Size(138, 21);
            this.cmbAmmo.TabIndex = 86;
            this.cmbAmmo.SelectedIndexChanged += new System.EventHandler(this.cmbAmmo_SelectedIndexChanged);
            this.cmbAmmo.MouseDown += new System.Windows.Forms.MouseEventHandler(this.cmbAmmo_MouseDown);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(31, 256);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(39, 13);
            this.label6.TabIndex = 85;
            this.label6.Text = "Ammo:";
            // 
            // cmbFeet
            // 
            this.cmbFeet.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFeet.FormattingEnabled = true;
            this.cmbFeet.Location = new System.Drawing.Point(75, 223);
            this.cmbFeet.Name = "cmbFeet";
            this.cmbFeet.Size = new System.Drawing.Size(138, 21);
            this.cmbFeet.TabIndex = 84;
            this.cmbFeet.SelectedIndexChanged += new System.EventHandler(this.cmbFeet_SelectedIndexChanged);
            this.cmbFeet.MouseDown += new System.Windows.Forms.MouseEventHandler(this.cmbFeet_MouseDown);
            // 
            // cmbRing2
            // 
            this.cmbRing2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRing2.FormattingEnabled = true;
            this.cmbRing2.Location = new System.Drawing.Point(75, 194);
            this.cmbRing2.Name = "cmbRing2";
            this.cmbRing2.Size = new System.Drawing.Size(138, 21);
            this.cmbRing2.TabIndex = 83;
            this.cmbRing2.SelectedIndexChanged += new System.EventHandler(this.cmbRing2_SelectedIndexChanged);
            this.cmbRing2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.cmbRing2_MouseDown);
            // 
            // cmbRing1
            // 
            this.cmbRing1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRing1.FormattingEnabled = true;
            this.cmbRing1.Location = new System.Drawing.Point(75, 165);
            this.cmbRing1.Name = "cmbRing1";
            this.cmbRing1.Size = new System.Drawing.Size(138, 21);
            this.cmbRing1.TabIndex = 82;
            this.cmbRing1.SelectedIndexChanged += new System.EventHandler(this.cmbRing1_SelectedIndexChanged);
            this.cmbRing1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.cmbRing1_MouseDown);
            // 
            // cmbOffHand
            // 
            this.cmbOffHand.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbOffHand.FormattingEnabled = true;
            this.cmbOffHand.Location = new System.Drawing.Point(75, 136);
            this.cmbOffHand.Name = "cmbOffHand";
            this.cmbOffHand.Size = new System.Drawing.Size(138, 21);
            this.cmbOffHand.TabIndex = 81;
            this.cmbOffHand.SelectedIndexChanged += new System.EventHandler(this.cmbOffHand_SelectedIndexChanged);
            this.cmbOffHand.MouseDown += new System.Windows.Forms.MouseEventHandler(this.cmbOffHand_MouseDown);
            // 
            // cmbMainHand
            // 
            this.cmbMainHand.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMainHand.FormattingEnabled = true;
            this.cmbMainHand.Location = new System.Drawing.Point(75, 107);
            this.cmbMainHand.Name = "cmbMainHand";
            this.cmbMainHand.Size = new System.Drawing.Size(138, 21);
            this.cmbMainHand.TabIndex = 80;
            this.cmbMainHand.SelectedIndexChanged += new System.EventHandler(this.cmbMainHand_SelectedIndexChanged);
            this.cmbMainHand.MouseDown += new System.Windows.Forms.MouseEventHandler(this.cmbMainHand_MouseDown);
            // 
            // cmbBody
            // 
            this.cmbBody.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBody.FormattingEnabled = true;
            this.cmbBody.Location = new System.Drawing.Point(75, 78);
            this.cmbBody.Name = "cmbBody";
            this.cmbBody.Size = new System.Drawing.Size(138, 21);
            this.cmbBody.TabIndex = 79;
            this.cmbBody.SelectedIndexChanged += new System.EventHandler(this.cmbBody_SelectedIndexChanged);
            this.cmbBody.MouseDown += new System.Windows.Forms.MouseEventHandler(this.cmbBody_MouseDown);
            // 
            // cmbNeck
            // 
            this.cmbNeck.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbNeck.FormattingEnabled = true;
            this.cmbNeck.Location = new System.Drawing.Point(75, 49);
            this.cmbNeck.Name = "cmbNeck";
            this.cmbNeck.Size = new System.Drawing.Size(138, 21);
            this.cmbNeck.TabIndex = 78;
            this.cmbNeck.SelectedIndexChanged += new System.EventHandler(this.cmbNeck_SelectedIndexChanged);
            this.cmbNeck.MouseDown += new System.Windows.Forms.MouseEventHandler(this.cmbNeck_MouseDown);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(36, 81);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(34, 13);
            this.label9.TabIndex = 77;
            this.label9.Text = "Body:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(29, 168);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(41, 13);
            this.label8.TabIndex = 76;
            this.label8.Text = "Ring 1:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(39, 226);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(31, 13);
            this.label7.TabIndex = 75;
            this.label7.Text = "Feet:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(29, 197);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 13);
            this.label5.TabIndex = 73;
            this.label5.Text = "Ring 2:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(17, 139);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 13);
            this.label4.TabIndex = 72;
            this.label4.Text = "Off Hand:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(34, 52);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 13);
            this.label3.TabIndex = 71;
            this.label3.Text = "Neck:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 110);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 13);
            this.label2.TabIndex = 70;
            this.label2.Text = "Main Hand:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(34, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 13);
            this.label1.TabIndex = 69;
            this.label1.Text = "Head:";
            // 
            // cmbHead
            // 
            this.cmbHead.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbHead.FormattingEnabled = true;
            this.cmbHead.Location = new System.Drawing.Point(75, 20);
            this.cmbHead.Name = "cmbHead";
            this.cmbHead.Size = new System.Drawing.Size(138, 21);
            this.cmbHead.TabIndex = 68;
            this.cmbHead.SelectedIndexChanged += new System.EventHandler(this.cmbHead_SelectedIndexChanged);
            this.cmbHead.MouseDown += new System.Windows.Forms.MouseEventHandler(this.cmbHead_MouseDown);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.cbxKnownTraits);
            this.groupBox1.Location = new System.Drawing.Point(683, 319);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(196, 313);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Known Traits";
            // 
            // cbxKnownTraits
            // 
            this.cbxKnownTraits.CheckOnClick = true;
            this.cbxKnownTraits.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbxKnownTraits.FormattingEnabled = true;
            this.cbxKnownTraits.Location = new System.Drawing.Point(3, 16);
            this.cbxKnownTraits.Name = "cbxKnownTraits";
            this.cbxKnownTraits.ScrollAlwaysVisible = true;
            this.cbxKnownTraits.Size = new System.Drawing.Size(190, 294);
            this.cbxKnownTraits.TabIndex = 1;
            this.cbxKnownTraits.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.cbxKnownTraits_ItemCheck);
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.propertyGrid1.Location = new System.Drawing.Point(202, 12);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(241, 622);
            this.propertyGrid1.TabIndex = 8;
            this.propertyGrid1.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertyGrid1_PropertyValueChanged);
            // 
            // gbCreatureIconSelect
            // 
            this.gbCreatureIconSelect.Controls.Add(this.btnSelectPortrait);
            this.gbCreatureIconSelect.Controls.Add(this.pbPortrait);
            this.gbCreatureIconSelect.Controls.Add(this.pbIcon);
            this.gbCreatureIconSelect.Controls.Add(this.btnSelectIcon);
            this.gbCreatureIconSelect.Location = new System.Drawing.Point(462, 344);
            this.gbCreatureIconSelect.Name = "gbCreatureIconSelect";
            this.gbCreatureIconSelect.Size = new System.Drawing.Size(202, 236);
            this.gbCreatureIconSelect.TabIndex = 45;
            this.gbCreatureIconSelect.TabStop = false;
            this.gbCreatureIconSelect.Text = "Token and Portrait";
            // 
            // pbIcon
            // 
            this.pbIcon.BackColor = System.Drawing.Color.Silver;
            this.pbIcon.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pbIcon.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pbIcon.Location = new System.Drawing.Point(13, 19);
            this.pbIcon.Name = "pbIcon";
            this.pbIcon.Padding = new System.Windows.Forms.Padding(2);
            this.pbIcon.Size = new System.Drawing.Size(54, 104);
            this.pbIcon.TabIndex = 25;
            this.pbIcon.TabStop = false;
            // 
            // btnSelectIcon
            // 
            this.btnSelectIcon.Location = new System.Drawing.Point(13, 129);
            this.btnSelectIcon.Name = "btnSelectIcon";
            this.btnSelectIcon.Size = new System.Drawing.Size(54, 43);
            this.btnSelectIcon.TabIndex = 26;
            this.btnSelectIcon.Text = "Select Token";
            this.btnSelectIcon.UseVisualStyleBackColor = true;
            this.btnSelectIcon.Click += new System.EventHandler(this.btnSelectIcon_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox3.Controls.Add(this.btnSort);
            this.groupBox3.Controls.Add(this.btnDuplicate);
            this.groupBox3.Controls.Add(this.btnAdd);
            this.groupBox3.Controls.Add(this.btnRemove);
            this.groupBox3.Controls.Add(this.lbxPlayers);
            this.groupBox3.Location = new System.Drawing.Point(12, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(177, 622);
            this.groupBox3.TabIndex = 48;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Module Players/Companions";
            // 
            // btnSort
            // 
            this.btnSort.Location = new System.Drawing.Point(8, 14);
            this.btnSort.Name = "btnSort";
            this.btnSort.Size = new System.Drawing.Size(40, 23);
            this.btnSort.TabIndex = 90;
            this.btnSort.Text = "Sort";
            this.btnSort.UseVisualStyleBackColor = true;
            this.btnSort.Click += new System.EventHandler(this.btnSort_Click);
            // 
            // btnDuplicate
            // 
            this.btnDuplicate.Location = new System.Drawing.Point(108, 14);
            this.btnDuplicate.Name = "btnDuplicate";
            this.btnDuplicate.Size = new System.Drawing.Size(61, 23);
            this.btnDuplicate.TabIndex = 87;
            this.btnDuplicate.Text = "Duplicate";
            this.btnDuplicate.UseVisualStyleBackColor = true;
            this.btnDuplicate.Click += new System.EventHandler(this.btnDuplicate_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(8, 37);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(161, 23);
            this.btnAdd.TabIndex = 85;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.Location = new System.Drawing.Point(48, 14);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(60, 23);
            this.btnRemove.TabIndex = 86;
            this.btnRemove.Text = "Remove";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // lbxPlayers
            // 
            this.lbxPlayers.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lbxPlayers.FormattingEnabled = true;
            this.lbxPlayers.Location = new System.Drawing.Point(10, 61);
            this.lbxPlayers.Name = "lbxPlayers";
            this.lbxPlayers.Size = new System.Drawing.Size(157, 550);
            this.lbxPlayers.TabIndex = 82;
            this.lbxPlayers.SelectedIndexChanged += new System.EventHandler(this.lbxPlayers_SelectedIndexChanged);
            // 
            // pbPortrait
            // 
            this.pbPortrait.BackColor = System.Drawing.Color.Silver;
            this.pbPortrait.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pbPortrait.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pbPortrait.Location = new System.Drawing.Point(75, 19);
            this.pbPortrait.Name = "pbPortrait";
            this.pbPortrait.Padding = new System.Windows.Forms.Padding(2);
            this.pbPortrait.Size = new System.Drawing.Size(114, 174);
            this.pbPortrait.TabIndex = 27;
            this.pbPortrait.TabStop = false;
            // 
            // btnSelectPortrait
            // 
            this.btnSelectPortrait.Location = new System.Drawing.Point(75, 195);
            this.btnSelectPortrait.Name = "btnSelectPortrait";
            this.btnSelectPortrait.Size = new System.Drawing.Size(114, 30);
            this.btnSelectPortrait.TabIndex = 28;
            this.btnSelectPortrait.Text = "Select Portrait";
            this.btnSelectPortrait.UseVisualStyleBackColor = true;
            this.btnSelectPortrait.Click += new System.EventHandler(this.btnSelectPortrait_Click);
            // 
            // PlayerEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(890, 646);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.gbCreatureIconSelect);
            this.Controls.Add(this.propertyGrid1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.gbKnownSpells);
            this.Name = "PlayerEditor";
            this.Text = "PlayerEditor";
            this.Load += new System.EventHandler(this.PlayerEditor_Load);
            this.gbKnownSpells.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.gbCreatureIconSelect.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbIcon)).EndInit();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbPortrait)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.GroupBox gbKnownSpells;
        private System.Windows.Forms.CheckedListBox cbxKnownSpells;
        private System.Windows.Forms.GroupBox groupBox2;
        public System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckedListBox cbxKnownTraits;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.ComboBox cmbHead;
        public System.Windows.Forms.GroupBox gbCreatureIconSelect;
        public System.Windows.Forms.PictureBox pbIcon;
        public System.Windows.Forms.Button btnSelectIcon;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbFeet;
        private System.Windows.Forms.ComboBox cmbRing2;
        private System.Windows.Forms.ComboBox cmbRing1;
        private System.Windows.Forms.ComboBox cmbOffHand;
        private System.Windows.Forms.ComboBox cmbMainHand;
        private System.Windows.Forms.ComboBox cmbBody;
        private System.Windows.Forms.ComboBox cmbNeck;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox cmbAmmo;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnSort;
        private System.Windows.Forms.Button btnDuplicate;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.ListBox lbxPlayers;
        public System.Windows.Forms.Button btnSelectPortrait;
        public System.Windows.Forms.PictureBox pbPortrait;
    }
}