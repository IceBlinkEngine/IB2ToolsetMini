﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.IO;

namespace IB2ToolsetMini
{
    public partial class PlayerEditor : Form
    {
        public ParentForm prntForm;
        public bool refreshingList = false;
        private int selectedLbxIndex = 0;
        public Bitmap iconBitmap;
        public Bitmap playerPortrait;
        public Player pc = new Player();
        public List<ItemRefs> itemsHeadList = new List<ItemRefs>();
        public List<ItemRefs> itemsNeckList = new List<ItemRefs>();
        public List<ItemRefs> itemsBodyList = new List<ItemRefs>();
        public List<ItemRefs> itemsMainHandList = new List<ItemRefs>();
        public List<ItemRefs> itemsOffHandList = new List<ItemRefs>();
        public List<ItemRefs> itemsRing1List = new List<ItemRefs>();
        public List<ItemRefs> itemsRing2List = new List<ItemRefs>();
        public List<ItemRefs> itemsFeetList = new List<ItemRefs>();
        public List<ItemRefs> itemsAmmoList = new List<ItemRefs>();
        public bool userPressedCmbHead = false;
        public bool userPressedCmbNeck = false;
        public bool userPressedCmbBody = false;
        public bool userPressedCmbMainHand = false;
        public bool userPressedCmbOffHand = false;
        public bool userPressedCmbRing1 = false;
        public bool userPressedCmbRing2 = false;
        public bool userPressedCmbFeet = false;
        public bool userPressedCmbAmmo = false;

        public PlayerEditor(ParentForm pf)
        {
            InitializeComponent();
            prntForm = pf;
            refreshListBox();
        }
        
        #region Event Handlers    
        private void PlayerEditor_Load(object sender, EventArgs e)
        {
            refreshForm();
        }
        private void btnSort_Click(object sender, EventArgs e)
        {
            prntForm.mod.companionPlayerList = prntForm.mod.companionPlayerList.OrderBy(o => o.name).ToList();
            refreshListBox();
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            Player newTS = new Player();
            newTS.name = "newPlayer";
            newTS.tag = "newPlayer";
            prntForm.mod.companionPlayerList.Add(newTS);
            refreshListBox();
        }
        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (lbxPlayers.Items.Count > 0)
            {
                try
                {
                    // The Remove button was clicked.
                    int selectedIndex = lbxPlayers.SelectedIndex;
                    //mod.ModuleContainersList.containers.RemoveAt(selectedIndex);
                    prntForm.mod.companionPlayerList.RemoveAt(selectedIndex);
                }
                catch { }
                selectedLbxIndex = 0;
                lbxPlayers.SelectedIndex = 0;
                refreshListBox();
            }
        }
        private void btnDuplicate_Click(object sender, EventArgs e)
        {
            //TODO Need a deepcopy method on Player
            //Player newCopy = prntForm.mod.companionPlayerList[selectedLbxIndex].DeepCopy();
            //newCopy.tag = "newSpellTag_" + prntForm.mod.nextIdNumber.ToString();
            //prntForm.mod.companionPlayerList.Add(newCopy);
            //refreshListBox();
        }
        private void lbxPlayers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((lbxPlayers.SelectedIndex >= 0) && (prntForm.mod.companionPlayerList != null))
            {
                selectedLbxIndex = lbxPlayers.SelectedIndex;
                lbxPlayers.SelectedIndex = selectedLbxIndex;
                pc = prntForm.mod.companionPlayerList[selectedLbxIndex];
                propertyGrid1.SelectedObject = prntForm.mod.companionPlayerList[selectedLbxIndex];
                refreshForm();
                LoadPlayerToken();
                LoadPlayerPortrait();
            }
        }
        private void propertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            refreshListBox();
        }
        private void btnLoadPlayer_Click(object sender, EventArgs e)
        {
            if (prntForm.mod.moduleName != "NewModule")
            {
                openFileDialog1.InitialDirectory = prntForm._mainDirectory + "\\modules\\" + prntForm.mod.moduleName + "\\data";
            }
            else
            {
                openFileDialog1.InitialDirectory = prntForm._mainDirectory + "\\default\\NewModule\\data";
            }
            //Empty the FileName text box of the dialog
            openFileDialog1.Filter = "Json (*.json)|*.json|All Files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;

            DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {
                string PcFilename = Path.GetFullPath(openFileDialog1.FileName);
                try
                {
                    pc = loadPlayerFile(PcFilename);
                    LoadPlayerToken();
                    LoadPlayerPortrait();
                    refreshForm();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Could not load this Player file, may be corrupt or wrong type... try another - Error: " + ex.ToString());
                }
            }            
        }
        private void btnSavePlayer_Click(object sender, EventArgs e)
        {
            try
            {
                pc.HeadRefs = (ItemRefs)cmbHead.SelectedItem;
                pc.NeckRefs = (ItemRefs)cmbNeck.SelectedItem;
                pc.BodyRefs = (ItemRefs)cmbBody.SelectedItem;
                pc.MainHandRefs = (ItemRefs)cmbMainHand.SelectedItem;
                pc.OffHandRefs = (ItemRefs)cmbOffHand.SelectedItem;
                pc.RingRefs = (ItemRefs)cmbRing1.SelectedItem;
                pc.Ring2Refs = (ItemRefs)cmbRing2.SelectedItem;
                pc.FeetRefs = (ItemRefs)cmbFeet.SelectedItem;
                pc.AmmoRefs = (ItemRefs)cmbAmmo.SelectedItem;
                
                savePlayerFile(prntForm._mainDirectory + "\\modules\\" + prntForm.mod.moduleName + "\\data\\" + pc.tag + ".json");
                MessageBox.Show("Using player's 'tag' for filename, Saved as: " + pc.tag + ".json");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not save this Player file - Error: " + ex.ToString());
            }
        }
        private void btnSelectIcon_Click(object sender, EventArgs e)
        {
            try
            {
                List<string> prefixlist = new List<string>();
                prefixlist.Add("tkn_");
                string name = prntForm.GetImageFilename(prefixlist);
                if (name != "none")
                {
                    pc.tokenFilename = name;
                    LoadPlayerToken();
                }                
            }
            catch (Exception ex)
            {
                MessageBox.Show("failed to load token...Error: " + ex.ToString());
            }
        }
        private void btnSelectPortrait_Click(object sender, EventArgs e)
        {
            try
            {
                List<string> prefixlist = new List<string>();
                prefixlist.Add("ptr_");
                prefixlist.Add("pptr_");
                string name = prntForm.GetImageFilename(prefixlist);
                if (name != "none")
                {
                    pc.portraitFilename = name;
                    LoadPlayerPortrait();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("failed to load portrait...Error: " + ex.ToString());
            }
        }
        private void cbxKnownSpells_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            //A nice trick to deal with events that you cannot process when they are raised is to delay 
            //the processing. Which you can do with the Control.BeginInvoke() method, it runs as soon 
            //as all events are dispatched, side-effects are complete and the UI thread goes idle again.
            //http://stackoverflow.com/questions/4454058/no-itemchecked-event-in-a-checkedlistbox/4454594#4454594
            //
            if (!refreshingList)
            {
                this.BeginInvoke((MethodInvoker)delegate
                {

                    pc.knownSpellsTags.Clear();
                    foreach (object itemChecked in cbxKnownSpells.CheckedItems)
                    {
                        Spell chkdSpell = (Spell)itemChecked;
                        pc.knownSpellsTags.Add(chkdSpell.tag);
                    }
                });
            }
        }
        private void cbxKnownTraits_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            //A nice trick to deal with events that you cannot process when they are raised is to delay 
            //the processing. Which you can do with the Control.BeginInvoke() method, it runs as soon 
            //as all events are dispatched, side-effects are complete and the UI thread goes idle again.
            //http://stackoverflow.com/questions/4454058/no-itemchecked-event-in-a-checkedlistbox/4454594#4454594
            //
            if (!refreshingList)
            {
                this.BeginInvoke((MethodInvoker)delegate
                {

                    pc.knownTraitsTags.Clear();
                    foreach (object itemChecked in cbxKnownTraits.CheckedItems)
                    {
                        Trait chkdTrait = (Trait)itemChecked;
                        pc.knownTraitsTags.Add(chkdTrait.tag);
                    }
                });
            }
        }
        private void cmbHead_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (userPressedCmbHead)
            {
                pc.HeadRefs = (ItemRefs)cmbHead.SelectedItem;
                userPressedCmbHead = false;
            }
        }
        private void cmbHead_MouseDown(object sender, MouseEventArgs e)
        {
            userPressedCmbHead = true;
        }
        private void cmbNeck_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (userPressedCmbNeck)
            {
                pc.NeckRefs = (ItemRefs)cmbNeck.SelectedItem;
                userPressedCmbNeck = false;
            }
        }
        private void cmbNeck_MouseDown(object sender, MouseEventArgs e)
        {
            userPressedCmbNeck = true;
        }
        private void cmbBody_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (userPressedCmbBody)
            {
                pc.BodyRefs = (ItemRefs)cmbBody.SelectedItem;
                userPressedCmbBody = false;
            }
        }
        private void cmbBody_MouseDown(object sender, MouseEventArgs e)
        {
            userPressedCmbBody = true;
        }
        private void cmbMainHand_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (userPressedCmbMainHand)
            {
                pc.MainHandRefs = (ItemRefs)cmbMainHand.SelectedItem;
                userPressedCmbMainHand = false;
            }
        }
        private void cmbMainHand_MouseDown(object sender, MouseEventArgs e)
        {
            userPressedCmbMainHand = true;
        }
        private void cmbOffHand_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (userPressedCmbOffHand)
            {
                pc.OffHandRefs = (ItemRefs)cmbOffHand.SelectedItem;
                userPressedCmbOffHand = false;
            }
        }
        private void cmbOffHand_MouseDown(object sender, MouseEventArgs e)
        {
            userPressedCmbOffHand = true;
        }
        private void cmbRing1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (userPressedCmbRing1)
            {
                pc.RingRefs = (ItemRefs)cmbRing1.SelectedItem;
                userPressedCmbRing1 = false;
            }
        }
        private void cmbRing1_MouseDown(object sender, MouseEventArgs e)
        {
            userPressedCmbRing1 = true;
        }
        private void cmbRing2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (userPressedCmbRing2)
            {
                pc.Ring2Refs = (ItemRefs)cmbRing2.SelectedItem;
                userPressedCmbRing2 = false;
            }
        }
        private void cmbRing2_MouseDown(object sender, MouseEventArgs e)
        {
            userPressedCmbRing2 = true;
        }
        private void cmbFeet_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (userPressedCmbFeet)
            {
                pc.FeetRefs = (ItemRefs)cmbFeet.SelectedItem;
                userPressedCmbFeet = false;
            }
        }
        private void cmbFeet_MouseDown(object sender, MouseEventArgs e)
        {
            userPressedCmbFeet = true;
        }
        private void cmbAmmo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (userPressedCmbAmmo)
            {
                pc.AmmoRefs = (ItemRefs)cmbAmmo.SelectedItem;
                userPressedCmbAmmo = false;
            }
        }
        private void cmbAmmo_MouseDown(object sender, MouseEventArgs e)
        {
            userPressedCmbAmmo = true;
        }
        #endregion

        #region Methods
        public void fillItemLists()
        {
            itemsHeadList.Clear();
            itemsHeadList.Add(new ItemRefs());
            foreach (Item it in prntForm.allItemsList)
            {
                if (it.category == "Head")
                {
                    itemsHeadList.Add(prntForm.createItemRefsFromItem(it));
                }
            }
            
            itemsNeckList.Clear();
            itemsNeckList.Add(new ItemRefs());
            foreach (Item it in prntForm.allItemsList)
            {
                if (it.category == "Neck")
                {
                    itemsNeckList.Add(prntForm.createItemRefsFromItem(it));
                }
            }

            itemsBodyList.Clear();
            itemsBodyList.Add(new ItemRefs());
            foreach (Item it in prntForm.allItemsList)
            {
                if (it.category == "Armor")
                {
                    itemsBodyList.Add(prntForm.createItemRefsFromItem(it));
                }
            }

            itemsMainHandList.Clear();
            itemsMainHandList.Add(new ItemRefs());
            foreach (Item it in prntForm.allItemsList)
            {
                if ((it.category == "Melee") || (it.category == "Ranged"))
                {
                    itemsMainHandList.Add(prntForm.createItemRefsFromItem(it));
                }
            }

            itemsOffHandList.Clear();
            itemsOffHandList.Add(new ItemRefs());
            foreach (Item it in prntForm.allItemsList)
            {
                if (it.category == "Shield")
                {
                    itemsOffHandList.Add(prntForm.createItemRefsFromItem(it));
                }
            }

            itemsRing1List.Clear();
            itemsRing1List.Add(new ItemRefs());
            foreach (Item it in prntForm.allItemsList)
            {
                if (it.category == "Ring")
                {
                    itemsRing1List.Add(prntForm.createItemRefsFromItem(it));
                }
            }

            itemsRing2List.Clear();
            itemsRing2List.Add(new ItemRefs());
            foreach (Item it in prntForm.allItemsList)
            {
                if (it.category == "Ring")
                {
                    itemsRing2List.Add(prntForm.createItemRefsFromItem(it));
                }
            }

            itemsFeetList.Clear();
            itemsFeetList.Add(new ItemRefs());
            foreach (Item it in prntForm.allItemsList)
            {
                if (it.category == "Feet")
                {
                    itemsFeetList.Add(prntForm.createItemRefsFromItem(it));
                }
            }

            itemsAmmoList.Clear();
            itemsAmmoList.Add(new ItemRefs());
            foreach (Item it in prntForm.allItemsList)
            {
                if (it.category == "Ammo")
                {
                    itemsAmmoList.Add(prntForm.createItemRefsFromItem(it));
                }
            }
        }
        public void refreshForm()
        {
            fillItemLists();
            propertyGrid1.SelectedObject = pc;
            refreshSpellsKnown();
            refreshTraitsKnown();
            refreshTokenDisplay();
            refreshPortraitDisplay();
            refreshCmbItems();
            refreshCmbSelected();
        }
        private void refreshListBox()
        {
            lbxPlayers.BeginUpdate();
            lbxPlayers.DataSource = null;
            lbxPlayers.DataSource = prntForm.mod.companionPlayerList;
            lbxPlayers.DisplayMember = "name";
            lbxPlayers.EndUpdate();
        }
        public void savePlayerFile(string filename)
        {
            string json = JsonConvert.SerializeObject(pc, Newtonsoft.Json.Formatting.Indented);
            using (StreamWriter sw = new StreamWriter(filename))
            {
                sw.Write(json.ToString());
            }
        }
        public Player loadPlayerFile(string filename)
        {
            Player toReturn = null;

            // deserialize JSON directly from a file
            using (StreamReader file = File.OpenText(filename))
            {
                JsonSerializer serializer = new JsonSerializer();
                toReturn = (Player)serializer.Deserialize(file, typeof(Player));
            }
            return toReturn;
        }
        public void refreshTokenDisplay()
        {
            try
            {
                if ((pc.tag != "newPlayer") && (iconBitmap != null))
                {
                    pbIcon.BackgroundImage = (Image)iconBitmap;
                    if (iconBitmap == null) { MessageBox.Show("returned a null icon bitmap"); }
                }
            }
            catch { }
        }
        public void refreshPortraitDisplay()
        {
            try
            {
                if ((pc.tag != "newPlayer") && (playerPortrait != null))
                {
                    pbPortrait.BackgroundImage = (Image)playerPortrait;
                    if (playerPortrait == null) { MessageBox.Show("returned a null portrait bitmap"); }
                }
            }
            catch { }
        }
        public void LoadPlayerToken()
        {
            iconBitmap = prntForm.LoadBitmapGDI(pc.tokenFilename);
            /*if (File.Exists(prntForm._mainDirectory + "\\modules\\" + prntForm.mod.moduleName + "\\graphics\\" + pc.tokenFilename + ".png"))
            {
                iconBitmap = new Bitmap(prntForm._mainDirectory + "\\modules\\" + prntForm.mod.moduleName + "\\graphics\\" + pc.tokenFilename + ".png");
            }
            else if (File.Exists(prntForm._mainDirectory + "\\default\\NewModule\\graphics\\" + pc.tokenFilename + ".png"))
            {
                iconBitmap = new Bitmap(prntForm._mainDirectory + "\\default\\NewModule\\graphics\\" + pc.tokenFilename + ".png");
            }
            else
            {
                iconBitmap = new Bitmap(prntForm._mainDirectory + "\\default\\NewModule\\graphics\\" + "missingtexture.png");
            }*/
            refreshTokenDisplay();
        }
        public void LoadPlayerPortrait()
        {
            playerPortrait = prntForm.LoadBitmapGDI(pc.portraitFilename);
            /*if (File.Exists(prntForm._mainDirectory + "\\modules\\" + prntForm.mod.moduleName + "\\graphics\\" + pc.tokenFilename + ".png"))
            {
                iconBitmap = new Bitmap(prntForm._mainDirectory + "\\modules\\" + prntForm.mod.moduleName + "\\graphics\\" + pc.tokenFilename + ".png");
            }
            else if (File.Exists(prntForm._mainDirectory + "\\default\\NewModule\\graphics\\" + pc.tokenFilename + ".png"))
            {
                iconBitmap = new Bitmap(prntForm._mainDirectory + "\\default\\NewModule\\graphics\\" + pc.tokenFilename + ".png");
            }
            else
            {
                iconBitmap = new Bitmap(prntForm._mainDirectory + "\\default\\NewModule\\graphics\\" + "missingtexture.png");
            }*/
            refreshPortraitDisplay();
        }
        public void refreshSpellsKnown()
        {
            cbxKnownSpells.BeginUpdate();
            cbxKnownSpells.DataSource = null;
            cbxKnownSpells.DataSource = prntForm.datafile.dataSpellsList;
            cbxKnownSpells.DisplayMember = "name";
            cbxKnownSpells.EndUpdate();

            //uncheck all first
            for (int i = 0; i < cbxKnownSpells.Items.Count; i++)
            {
                cbxKnownSpells.SetItemChecked(i, false);
            }
            //iterate and check ones in list
            refreshingList = true;
            for (int i = 0; i < cbxKnownSpells.Items.Count; i++)
            {
                Spell thisSpell = (Spell)cbxKnownSpells.Items[i];
                if (pc.knownSpellsTags.Contains((string)thisSpell.tag))
                {
                    cbxKnownSpells.SetItemChecked(i, true);
                }
            }
            refreshingList = false;
        }
        public void refreshTraitsKnown()
        {
            cbxKnownTraits.BeginUpdate();
            cbxKnownTraits.DataSource = null;
            cbxKnownTraits.DataSource = prntForm.datafile.dataTraitsList;
            cbxKnownTraits.DisplayMember = "name";
            cbxKnownTraits.EndUpdate();

            //uncheck all first
            for (int i = 0; i < cbxKnownTraits.Items.Count; i++)
            {
                cbxKnownTraits.SetItemChecked(i, false);
            }
            //iterate and check ones in list
            refreshingList = true;
            for (int i = 0; i < cbxKnownTraits.Items.Count; i++)
            {
                Trait thisTrait = (Trait)cbxKnownTraits.Items[i];
                if (pc.knownTraitsTags.Contains((string)thisTrait.tag))
                {
                    cbxKnownTraits.SetItemChecked(i, true);
                }
            }
            refreshingList = false;            
        }
        public void refreshCmbSelected()
        {
            foreach (ItemRefs itref in itemsBodyList)
            {
                if (itref.resref == pc.BodyRefs.resref)
                {
                    cmbBody.SelectedItem = itref;
                }
            }
            foreach (ItemRefs itref in itemsFeetList)
            {
                if (itref.resref == pc.FeetRefs.resref)
                {
                    cmbFeet.SelectedItem = itref;
                }
            }
            foreach (ItemRefs itref in itemsHeadList)
            {
                if (itref.resref == pc.HeadRefs.resref)
                {
                    cmbHead.SelectedItem = itref;
                }
            }
            foreach (ItemRefs itref in itemsMainHandList)
            {
                if (itref.resref == pc.MainHandRefs.resref)
                {
                    cmbMainHand.SelectedItem = itref;
                }
            }
            foreach (ItemRefs itref in itemsNeckList)
            {
                if (itref.resref == pc.NeckRefs.resref)
                {
                    cmbNeck.SelectedItem = itref;
                }
            }
            foreach (ItemRefs itref in itemsOffHandList)
            {
                if (itref.resref == pc.OffHandRefs.resref)
                {
                    cmbOffHand.SelectedItem = itref;
                }
            }
            foreach (ItemRefs itref in itemsRing1List)
            {
                if (itref.resref == pc.RingRefs.resref)
                {
                    cmbRing1.SelectedItem = itref;
                }
            }
            foreach (ItemRefs itref in itemsRing2List)
            {
                if (itref.resref == pc.Ring2Refs.resref)
                {
                    cmbRing2.SelectedItem = itref;
                }
            }
            foreach (ItemRefs itref in itemsAmmoList)
            {
                if (itref.resref == pc.AmmoRefs.resref)
                {
                    cmbAmmo.SelectedItem = itref;
                }
            }
            this.Invalidate();
        }
        public void refreshCmbItems()
        {
            cmbHead.BeginUpdate();
            cmbHead.DataSource = null;
            cmbHead.DataSource = itemsHeadList;
            cmbHead.DisplayMember = "name";
            cmbHead.EndUpdate();
            cmbHead.SelectedItem = pc.HeadRefs;

            cmbNeck.BeginUpdate();
            cmbNeck.DataSource = null;
            cmbNeck.DataSource = itemsNeckList;
            cmbNeck.DisplayMember = "name";
            cmbNeck.EndUpdate();
            cmbNeck.SelectedItem = pc.NeckRefs;
            
            cmbBody.BeginUpdate();
            cmbBody.DataSource = null;
            cmbBody.DataSource = itemsBodyList;
            cmbBody.DisplayMember = "name";
            cmbBody.EndUpdate();
            cmbBody.SelectedItem = pc.BodyRefs;
            
            cmbMainHand.BeginUpdate();
            cmbMainHand.DataSource = null;
            cmbMainHand.DataSource = itemsMainHandList;
            cmbMainHand.DisplayMember = "name";            
            cmbMainHand.SelectedItem = pc.MainHandRefs;
            cmbMainHand.EndUpdate();
            
            cmbOffHand.BeginUpdate();
            cmbOffHand.DataSource = null;
            cmbOffHand.DataSource = itemsOffHandList;
            cmbOffHand.DisplayMember = "name";
            cmbOffHand.EndUpdate();
            cmbOffHand.SelectedItem = pc.OffHandRefs;
            
            cmbRing1.BeginUpdate();
            cmbRing1.DataSource = null;
            cmbRing1.DataSource = itemsRing1List;
            cmbRing1.DisplayMember = "name";
            cmbRing1.EndUpdate();
            cmbRing1.SelectedItem = pc.RingRefs;
            
            cmbRing2.BeginUpdate();
            cmbRing2.DataSource = null;
            cmbRing2.DataSource = itemsRing2List;
            cmbRing2.DisplayMember = "name";
            cmbRing2.EndUpdate();
            cmbRing2.SelectedItem = pc.Ring2Refs;
            
            cmbFeet.BeginUpdate();
            cmbFeet.DataSource = null;
            cmbFeet.DataSource = itemsFeetList;
            cmbFeet.DisplayMember = "name";
            cmbFeet.EndUpdate();
            cmbFeet.SelectedItem = pc.FeetRefs;

            cmbAmmo.BeginUpdate();
            cmbAmmo.DataSource = null;
            cmbAmmo.DataSource = itemsAmmoList;
            cmbAmmo.DisplayMember = "name";
            cmbAmmo.EndUpdate();
            cmbAmmo.SelectedItem = pc.AmmoRefs;
        }
        #endregion

        
    }
}
