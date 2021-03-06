﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace IB2ToolsetMini
{
    public partial class EncountersForm : DockContent
    {
        public ParentForm prntForm;
        public Encounter encToCopy;

        public EncountersForm(ParentForm pf)
        {
            InitializeComponent();
            prntForm = pf;
        }

        #region Encounter Stuff
        public void refreshListBoxEncounters()
        {
            lbxEncounters.BeginUpdate();
            lbxEncounters.DataSource = null;
            lbxEncounters.DataSource = prntForm.mod.moduleEncountersList;
            lbxEncounters.DisplayMember = "encounterName";
            lbxEncounters.EndUpdate();
        }
        private void txtEncounterName_TextChanged_1(object sender, EventArgs e)
        {
            try
            {
                prntForm.mod.moduleEncountersList[prntForm._selectedLbxEncounterIndex].encounterName = txtEncounterName.Text;
                refreshListBoxEncounters();
            }
            catch { }
        }
        private void btnAddEncounter_Click_1(object sender, EventArgs e)
        {
            Encounter newEncounter = new Encounter();
            newEncounter.encounterName = "new encounter";
            newEncounter.SetAllToGrass();
            //newEncounter.passRefs(prntForm.game, prntForm);
            prntForm.mod.moduleEncountersList.Add(newEncounter);
            refreshListBoxEncounters();
        }
        private void btnRemoveEncounter_Click_1(object sender, EventArgs e)
        {
            if ((lbxEncounters.Items.Count > 0) && (lbxEncounters.SelectedIndex >= 0))
            {
                try
                {
                    // The Remove button was clicked.
                    int selectedIndex = lbxEncounters.SelectedIndex;
                    prntForm.mod.moduleEncountersList.RemoveAt(selectedIndex);
                }
                catch { }
                prntForm._selectedLbxEncounterIndex = 0;
                lbxEncounters.SelectedIndex = 0;
                refreshListBoxEncounters();
            }
        }
        private void btnEditEncounter_Click_1(object sender, EventArgs e)
        {
            if ((lbxEncounters.Items.Count > 0) && (lbxEncounters.SelectedIndex >= 0))
            {
                EditEncounter();                
            }
        }
        private void lbxEncounters_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (lbxEncounters.SelectedIndex >= 0)
            {
                prntForm._selectedLbxEncounterIndex = lbxEncounters.SelectedIndex;
                txtEncounterName.Text = prntForm.mod.moduleEncountersList[prntForm._selectedLbxEncounterIndex].encounterName;
                lbxEncounters.SelectedIndex = prntForm._selectedLbxEncounterIndex;
            }
        }
        private void btnRename_Click(object sender, EventArgs e)
        {
            if ((lbxEncounters.Items.Count > 0) && (lbxEncounters.SelectedIndex >= 0))
            {
                RenameDialog newName = new RenameDialog();
                DialogResult result = newName.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    try
                    {
                        prntForm.mod.moduleEncountersList[prntForm._selectedLbxEncounterIndex].encounterName = newName.RenameText;
                        refreshListBoxEncounters();
                    }
                    catch { }
                }
            }
        }
        private void lbxEncounters_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int index = this.lbxEncounters.IndexFromPoint(e.Location);

            if (index != System.Windows.Forms.ListBox.NoMatches)
            {

                //MessageBox.Show(index.ToString());
                //do your stuff here
                //prntForm._selectedLbxEncounterIndex = index;
                if ((lbxEncounters.Items.Count > 0) && (lbxEncounters.SelectedIndex >= 0))
                {
                    EditEncounter();
                }
            }
        }
        private void btnSort_Click(object sender, EventArgs e)
        {
            prntForm.mod.moduleEncountersList = prntForm.mod.moduleEncountersList.OrderBy(o => o.encounterName).ToList();
            refreshListBoxEncounters();
        }
        private void btnDuplicate_Click(object sender, EventArgs e)
        {
            if ((lbxEncounters.Items.Count > 0) && (lbxEncounters.SelectedIndex >= 0))
            {
                try
                {
                    Encounter newEncounter = new Encounter();
                    newEncounter = prntForm.mod.moduleEncountersList[prntForm._selectedLbxEncounterIndex].DeepCopy();
                    newEncounter.encounterName = prntForm.mod.moduleEncountersList[prntForm._selectedLbxEncounterIndex].encounterName + "-Copy";
                    prntForm.mod.moduleEncountersList.Add(newEncounter);
                    refreshListBoxEncounters();
                }
                catch { }
            }
        } 
        #endregion 
       
        private void EditEncounter()
        {
            //Encounter enc = prntForm.mod.moduleEncountersList[prntForm._selectedLbxEncounterIndex];
            EncounterEditor newChild = new EncounterEditor(prntForm.mod, prntForm); //add new child
            newChild.Text = prntForm.mod.moduleEncountersList[prntForm._selectedLbxEncounterIndex].encounterName;
            newChild.Show(prntForm.dockPanel1); //as new form created so that corresponding tab and child form is active
            refreshListBoxEncounters();
        }
    }
}
