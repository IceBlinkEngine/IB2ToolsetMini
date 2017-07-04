﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
//using IceBlinkCore;

namespace IB2ToolsetMini
{
    public partial class IconSprite : DockContent
    {
        public ParentForm prntForm;
        public bool refreshingList = false;

        public IconSprite(ParentForm pf)
        {
            InitializeComponent();
            prntForm = pf;
        }

        private void btnSelectIcon_Click(object sender, EventArgs e)
        {
            try
            {
                if (prntForm.frmBlueprints.tabCreatureItem.SelectedIndex == 0) //creature
                {
                    prntForm.LoadCreatureSprite();
                }
                else if (prntForm.frmBlueprints.tabCreatureItem.SelectedIndex == 1) //item
                {
                    prntForm.LoadItemIcon();
                }
                else //prop
                {
                    prntForm.LoadPropSprite();
                }
            }
            catch
            {
                MessageBox.Show("failed to load sprite...make sure to select a creature, item, or prop just before clicking this button.");
            }
        }

        private void cbxKnownSpells_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            //A nice trick to deal with events that you cannot process when they are raised is to delay 
            //the processing. Which you can do with the Control.BeginInvoke() method, it runs as soon 
            //as all events are dispatched, side-effects are complete and the UI thread goes idle again.
            //http://stackoverflow.com/questions/4454058/no-itemchecked-event-in-a-checkedlistbox/4454594#4454594
            //
            this.BeginInvoke((MethodInvoker)delegate
            {
                if (!refreshingList)
                {
                    string _nodeTag = prntForm.lastSelectedCreatureNodeName;
                    int index = prntForm.frmBlueprints.GetCreatureIndex(_nodeTag);
                    if (index >= 0)
                    {
                        Creature crt = prntForm.allCreaturesList[index];
                        crt.knownSpellsTags.Clear();
                        foreach (object itemChecked in cbxKnownSpells.CheckedItems)
                        {
                            Spell chkdSpell = (Spell)itemChecked;
                            crt.knownSpellsTags.Add(chkdSpell.tag);
                        }
                    }
                }
            });            
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
            if (prntForm.frmBlueprints != null)
            {
                if (prntForm.frmBlueprints.tabCreatureItem.SelectedIndex == 0) //creature
                {
                    if (prntForm.lastSelectedCreatureNodeName != "")
                    {
                        string _nodeTag = prntForm.lastSelectedCreatureNodeName;
                        Creature crt = prntForm.allCreaturesList[prntForm.frmBlueprints.GetCreatureIndex(_nodeTag)];
                        refreshingList = true;
                        for (int i = 0; i < cbxKnownSpells.Items.Count; i++)
                        {
                            Spell thisSpell = (Spell)cbxKnownSpells.Items[i];
                            if (crt.knownSpellsTags.Contains((string)thisSpell.tag))
                            {
                                cbxKnownSpells.SetItemChecked(i, true);
                            }
                        }
                        refreshingList = false;
                    }
                }
            }
        }
        
        private void cbxUsableByClass_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            //A nice trick to deal with events that you cannot process when they are raised is to delay 
            //the processing. Which you can do with the Control.BeginInvoke() method, it runs as soon 
            //as all events are dispatched, side-effects are complete and the UI thread goes idle again.
            //http://stackoverflow.com/questions/4454058/no-itemchecked-event-in-a-checkedlistbox/4454594#4454594
            //
            this.BeginInvoke((MethodInvoker)delegate
            {
                if (!refreshingList)
                {
                    string _nodeTag = prntForm.lastSelectedItemNodeName;
                    int index = prntForm.frmBlueprints.GetItemIndex(_nodeTag);
                    if (index >= 0)
                    {
                        Item itm = prntForm.allItemsList[index];
                        itm.classesAllowed.Clear();
                        foreach (object itemChecked in cbxUsableByClass.CheckedItems)
                        {
                            PlayerClass chkdClass = (PlayerClass)itemChecked;
                            itm.classesAllowed.Add(chkdClass.tag);
                        }
                    }
                }
            });
        }

        public void refreshUsableByClassesList()
        {
            cbxUsableByClass.BeginUpdate();
            cbxUsableByClass.DataSource = null;
            cbxUsableByClass.DataSource = prntForm.datafile.dataPlayerClassList;
            cbxUsableByClass.DisplayMember = "name";
            cbxUsableByClass.EndUpdate();

            //uncheck all first
            for (int i = 0; i < cbxUsableByClass.Items.Count; i++)
            {
                cbxUsableByClass.SetItemChecked(i, false);
            }
            //iterate and check ones in list
            if (prntForm.frmBlueprints != null)
            {
                if (prntForm.frmBlueprints.tabCreatureItem.SelectedIndex == 1) //item
                {
                    if (prntForm.lastSelectedItemNodeName != "")
                    {
                        string _nodeTag = prntForm.lastSelectedItemNodeName;
                        Item itm = prntForm.allItemsList[prntForm.frmBlueprints.GetItemIndex(_nodeTag)];
                        refreshingList = true;
                        for (int i = 0; i < cbxUsableByClass.Items.Count; i++)
                        {
                            PlayerClass thisClass = (PlayerClass)cbxUsableByClass.Items[i];
                            if (itm.classesAllowed.Contains((string)thisClass.tag))
                            {
                                cbxUsableByClass.SetItemChecked(i, true);
                            }
                        }
                        refreshingList = false;
                    }
                }
            }
        }
    }
}
