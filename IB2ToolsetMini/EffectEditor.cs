﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
//using IceBlinkCore;
using System.IO;
using System.Threading;

namespace IB2ToolsetMini
{
    public partial class EffectEditor : Form
    {
        private Module mod = new Module();
        //private Game game;
        private ParentForm prntForm;
        private int selectedLbxIndex = 0;

        public EffectEditor(Module m, ParentForm pf)
        {
            InitializeComponent();
            mod = m;
            //game = g;
            prntForm = pf;
            refreshListBox();
        }
        private void refreshListBox()
        {
            lbxEffects.BeginUpdate();
            lbxEffects.DataSource = null;
            lbxEffects.DataSource = prntForm.datafile.dataEffectsList;
            lbxEffects.DisplayMember = "EffectName";
            lbxEffects.EndUpdate();
        }
        private void lbxEffects_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((lbxEffects.SelectedIndex >= 0) && (prntForm.datafile.dataEffectsList != null))
            {
                selectedLbxIndex = lbxEffects.SelectedIndex;
                lbxEffects.SelectedIndex = selectedLbxIndex;
                propertyGrid1.SelectedObject = prntForm.datafile.dataEffectsList[selectedLbxIndex];
                refreshGroupBoxes();
            }
        }
        private void propertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            refreshListBox();
        }
        private void btnAddEffect_Click_1(object sender, EventArgs e)
        {
            Effect newE = new Effect();
            newE.name = "newEffect";
            newE.tag = "newEffectTag_" + prntForm.mod.nextIdNumber.ToString();
            prntForm.datafile.dataEffectsList.Add(newE);
            refreshListBox();
        }
        private void btnRemoveEffect_Click_1(object sender, EventArgs e)
        {
            if (lbxEffects.Items.Count > 0)
            {
                try
                {
                    // The Remove button was clicked.
                    int selectedIndex = lbxEffects.SelectedIndex;
                    //mod.ModuleContainersList.containers.RemoveAt(selectedIndex);
                    prntForm.datafile.dataEffectsList.RemoveAt(selectedIndex);
                }
                catch { }
                selectedLbxIndex = 0;
                lbxEffects.SelectedIndex = 0;
                refreshListBox();
            }
        }
        private void btnDuplicateEffect_Click_1(object sender, EventArgs e)
        {
            Effect newCopy = prntForm.datafile.dataEffectsList[selectedLbxIndex].DeepCopy();
            newCopy.tag = "newEffectTag_" + prntForm.mod.nextIdNumber.ToString();
            prntForm.datafile.dataEffectsList.Add(newCopy);
            refreshListBox();
        }

        private void btnSort_Click(object sender, EventArgs e)
        {
            prntForm.datafile.dataEffectsList = prntForm.datafile.dataEffectsList.OrderBy(o => o.name).ToList();
            refreshListBox();
        }

        public void refreshGroupBoxes()
        {
            if (prntForm.datafile.dataEffectsList != null)
            {
                Effect ef = prntForm.datafile.dataEffectsList[lbxEffects.SelectedIndex];
                numDamActionA.Value = ef.damNumOfDice;
                numDamActionB.Value = ef.damDie;
                numDamActionC.Value = ef.damAdder;
                numDamActionD.Value = ef.damAttacksEveryNLevels;
                numDamActionE.Value = ef.damAttacksAfterLevelN;
                numDamActionF.Value = ef.damAttacksUpToNLevelsTotal;
                numDamNumActionsA.Value = ef.damNumberOfAttacks;
                numDamNumActionsB.Value = ef.damNumberOfAttacksForEveryNLevels;
                numDamNumActionsC.Value = ef.damNumberOfAttacksAfterLevelN;
                numDamNumActionsD.Value = ef.damNumberOfAttacksUpToNAttacksTotal;
                numHealActionA.Value = ef.healNumOfDice;
                numHealActionB.Value = ef.healDie;
                numHealActionC.Value = ef.healAdder;
                numHealActionD.Value = ef.healActionsEveryNLevels;
                numHealActionE.Value = ef.healActionsAfterLevelN;
                numHealActionF.Value = ef.healActionsUpToNLevelsTotal;
            }
        }

        //Damage Action
        private void numDamActionA_ValueChanged(object sender, EventArgs e)
        {
            if ((lbxEffects.SelectedIndex >= 0) && (prntForm.datafile.dataEffectsList != null))
            {
                prntForm.datafile.dataEffectsList[lbxEffects.SelectedIndex].damNumOfDice = (int)numDamActionA.Value;
            }
        }
        private void numDamActionB_ValueChanged(object sender, EventArgs e)
        {
            if ((lbxEffects.SelectedIndex >= 0) && (prntForm.datafile.dataEffectsList != null))
            {
                prntForm.datafile.dataEffectsList[lbxEffects.SelectedIndex].damDie = (int)numDamActionB.Value;
            }
        }
        private void numDamActionC_ValueChanged(object sender, EventArgs e)
        {
            if ((lbxEffects.SelectedIndex >= 0) && (prntForm.datafile.dataEffectsList != null))
            {
                prntForm.datafile.dataEffectsList[lbxEffects.SelectedIndex].damAdder = (int)numDamActionC.Value;
            }
        }
        private void numDamActionD_ValueChanged(object sender, EventArgs e)
        {
            if ((lbxEffects.SelectedIndex >= 0) && (prntForm.datafile.dataEffectsList != null))
            {
                prntForm.datafile.dataEffectsList[lbxEffects.SelectedIndex].damAttacksEveryNLevels = (int)numDamActionD.Value;
            }
        }
        private void numDamActionE_ValueChanged(object sender, EventArgs e)
        {
            if ((lbxEffects.SelectedIndex >= 0) && (prntForm.datafile.dataEffectsList != null))
            {
                prntForm.datafile.dataEffectsList[lbxEffects.SelectedIndex].damAttacksAfterLevelN = (int)numDamActionE.Value;
            }
        }
        private void numDamActionF_ValueChanged(object sender, EventArgs e)
        {
            if ((lbxEffects.SelectedIndex >= 0) && (prntForm.datafile.dataEffectsList != null))
            {
                prntForm.datafile.dataEffectsList[lbxEffects.SelectedIndex].damAttacksUpToNLevelsTotal = (int)numDamActionF.Value;
            }
        }
        //Damage Number of Actions
        private void numDamNumActionsA_ValueChanged(object sender, EventArgs e)
        {
            if ((lbxEffects.SelectedIndex >= 0) && (prntForm.datafile.dataEffectsList != null))
            {
                prntForm.datafile.dataEffectsList[lbxEffects.SelectedIndex].damNumberOfAttacks = (int)numDamNumActionsA.Value;
            }
        }
        private void numDamNumActionsB_ValueChanged(object sender, EventArgs e)
        {
            if ((lbxEffects.SelectedIndex >= 0) && (prntForm.datafile.dataEffectsList != null))
            {
                prntForm.datafile.dataEffectsList[lbxEffects.SelectedIndex].damNumberOfAttacksForEveryNLevels = (int)numDamNumActionsB.Value;
            }
        }
        private void numDamNumActionsC_ValueChanged(object sender, EventArgs e)
        {
            if ((lbxEffects.SelectedIndex >= 0) && (prntForm.datafile.dataEffectsList != null))
            {
                prntForm.datafile.dataEffectsList[lbxEffects.SelectedIndex].damNumberOfAttacksAfterLevelN = (int)numDamNumActionsC.Value;
            }
        }
        private void numDamNumActionsD_ValueChanged(object sender, EventArgs e)
        {
            if ((lbxEffects.SelectedIndex >= 0) && (prntForm.datafile.dataEffectsList != null))
            {
                prntForm.datafile.dataEffectsList[lbxEffects.SelectedIndex].damNumberOfAttacksUpToNAttacksTotal = (int)numDamNumActionsD.Value;
            }
        }
        //Heal Action
        private void numHealActionA_ValueChanged(object sender, EventArgs e)
        {
            if ((lbxEffects.SelectedIndex >= 0) && (prntForm.datafile.dataEffectsList != null))
            {
                prntForm.datafile.dataEffectsList[lbxEffects.SelectedIndex].healNumOfDice = (int)numHealActionA.Value;
            }
        }
        private void numHealActionB_ValueChanged(object sender, EventArgs e)
        {
            if ((lbxEffects.SelectedIndex >= 0) && (prntForm.datafile.dataEffectsList != null))
            {
                prntForm.datafile.dataEffectsList[lbxEffects.SelectedIndex].healDie = (int)numHealActionB.Value;
            }
        }
        private void numHealActionC_ValueChanged(object sender, EventArgs e)
        {
            if ((lbxEffects.SelectedIndex >= 0) && (prntForm.datafile.dataEffectsList != null))
            {
                prntForm.datafile.dataEffectsList[lbxEffects.SelectedIndex].healAdder = (int)numHealActionC.Value;
            }
        }
        private void numHealActionD_ValueChanged(object sender, EventArgs e)
        {
            if ((lbxEffects.SelectedIndex >= 0) && (prntForm.datafile.dataEffectsList != null))
            {
                prntForm.datafile.dataEffectsList[lbxEffects.SelectedIndex].healActionsEveryNLevels = (int)numHealActionD.Value;
            }
        }
        private void numHealActionE_ValueChanged(object sender, EventArgs e)
        {
            if ((lbxEffects.SelectedIndex >= 0) && (prntForm.datafile.dataEffectsList != null))
            {
                prntForm.datafile.dataEffectsList[lbxEffects.SelectedIndex].healActionsAfterLevelN = (int)numHealActionE.Value;
            }
        }
        private void numHealActionF_ValueChanged(object sender, EventArgs e)
        {
            if ((lbxEffects.SelectedIndex >= 0) && (prntForm.datafile.dataEffectsList != null))
            {
                prntForm.datafile.dataEffectsList[lbxEffects.SelectedIndex].healActionsUpToNLevelsTotal = (int)numHealActionF.Value;
            }
        }
    }
}
