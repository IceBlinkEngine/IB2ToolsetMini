﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
//using IceBlinkCore;
using WeifenLuo.WinFormsUI.Docking;
using System.IO;

namespace IB2ToolsetMini
{
    public partial class IBScriptForm : DockContent
    {
        public ParentForm prntForm;

        public IBScriptForm(ParentForm pf)
        {
            InitializeComponent();
            prntForm = pf;
        }

        #region IBScript Stuff
        public void refreshListBoxIBScripts()
        {
            lbxIBScripts.BeginUpdate();
            lbxIBScripts.DataSource = null;
            lbxIBScripts.DataSource = prntForm.mod.moduleIBScriptList;
            lbxIBScripts.DisplayMember = "scriptName";
            lbxIBScripts.EndUpdate();            
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            IBScript newIBScript = new IBScript();
            prntForm.mod.moduleIBScriptList.Add(newIBScript);
            refreshListBoxIBScripts();
        }
        private void btnRemove_Click(object sender, EventArgs e)
        {
            if ((lbxIBScripts.Items.Count > 0) && (lbxIBScripts.SelectedIndex >= 0))
            {
                try
                {
                    // The Remove button was clicked.
                    int selectedIndex = lbxIBScripts.SelectedIndex;
                    prntForm.mod.moduleIBScriptList.RemoveAt(selectedIndex);
                }
                catch { }
                prntForm._selectedLbxIBScriptIndex = 0;
                lbxIBScripts.SelectedIndex = 0;
                refreshListBoxIBScripts();
            }
        }
        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if ((lbxIBScripts.Items.Count > 0) && (lbxIBScripts.SelectedIndex >= 0))
                {
                    EditIBScript();                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("failed: " + ex.ToString());
            }
        }
        private void lbxIBScripts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbxIBScripts.SelectedIndex >= 0)
            {
                prntForm._selectedLbxIBScriptIndex = lbxIBScripts.SelectedIndex;
                lbxIBScripts.SelectedIndex = prntForm._selectedLbxIBScriptIndex;
            }
        }
        private void btnRename_Click(object sender, EventArgs e)
        {
            if ((lbxIBScripts.Items.Count > 0) && (lbxIBScripts.SelectedIndex >= 0))
            {
                //string scriptname = prntForm.mod.moduleIBScriptsList[prntForm._selectedLbxIBScriptIndex];
                RenameDialog newName = new RenameDialog();
                DialogResult result = newName.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    try
                    {
                        prntForm.mod.moduleIBScriptList[prntForm._selectedLbxIBScriptIndex].scriptName = newName.RenameText;
                        refreshListBoxIBScripts();
                        /*
                        #region New IB Script
                        if (scriptname == "newIBScript")
                        {
                            //if file exists, rename the file
                            string filePath = prntForm._mainDirectory + "\\modules\\" + prntForm.mod.moduleName + "\\ibscript";
                            if (File.Exists(filePath + "\\" + scriptname + ".ibs"))
                            {
                                try
                                {
                                    //rename file
                                    File.Move(filePath + "\\" + scriptname + ".ibs", filePath + "\\" + newName.RenameText + ".ibs");
                                    try
                                    {
                                        prntForm.mod.moduleIBScriptsList[prntForm._selectedLbxIBScriptIndex] = newName.RenameText;
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show("failed to open file: " + ex.ToString());
                                    }
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show(ex.ToString()); // Write error
                                }
                            }
                            else
                            {
                                prntForm.mod.moduleIBScriptsList[prntForm._selectedLbxIBScriptIndex] = newName.RenameText;
                            }
                            refreshListBoxIBScripts();
                        }
                        #endregion
                        #region Existing IB Script
                        else
                        {
                            DialogResult sure = MessageBox.Show("Are you sure you wish to change this IB Script's file name? (make sure to update any references to this LogicTree name such as script hooks and trigger events)", "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk);
                            if (sure == System.Windows.Forms.DialogResult.Yes)
                            {
                                //if file exists, rename the file
                                string filePath = prntForm._mainDirectory + "\\modules\\" + prntForm.mod.moduleName + "\\ibscript";
                                if (File.Exists(filePath + "\\" + scriptname + ".ibs"))
                                {
                                    try
                                    {
                                        //rename file
                                        File.Move(filePath + "\\" + scriptname + ".ibs", filePath + "\\" + newName.RenameText + ".ibs");
                                        try
                                        {                                            
                                            prntForm.mod.moduleIBScriptsList[prntForm._selectedLbxIBScriptIndex] = newName.RenameText;
                                        }
                                        catch (Exception ex)
                                        {
                                            MessageBox.Show("failed to open file: " + ex.ToString());
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show(ex.ToString()); // Write error
                                    }
                                }
                                else
                                {
                                    prntForm.mod.moduleIBScriptsList[prntForm._selectedLbxIBScriptIndex] = newName.RenameText;
                                }
                                refreshListBoxIBScripts();
                            }
                        }
                        #endregion
                        */
                    }
                    catch { }
                }
            }
        }
        private void lbxIBScripts_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int index = this.lbxIBScripts.IndexFromPoint(e.Location);
            if (index != System.Windows.Forms.ListBox.NoMatches)
            {
                //do your stuff here
                try
                {
                    if ((lbxIBScripts.Items.Count > 0) && (lbxIBScripts.SelectedIndex >= 0))
                    {
                        EditIBScript();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("failed: " + ex.ToString());
                }
            }
        }
        private void btnSort_Click(object sender, EventArgs e)
        {
            prntForm.mod.moduleIBScriptList = prntForm.mod.moduleIBScriptList.OrderBy(o => o.scriptName).ToList();
            refreshListBoxIBScripts();
        }
        private void btnDuplicate_Click(object sender, EventArgs e)
        {
            //TODO need a DeepCopy() on IBScript
            refreshListBoxIBScripts();
        }
        /*private void btnLoadAll_Click(object sender, EventArgs e)
        {
            try
            {
                string jobDir = "";
                jobDir = prntForm._mainDirectory + "\\modules\\" + prntForm.mod.moduleName + "\\ibscript";
                prntForm.mod.moduleIBScriptsList.Clear();
                foreach (string f in Directory.GetFiles(jobDir, "*.ibs"))
                {
                    string filename = Path.GetFileNameWithoutExtension(f);
                    prntForm.mod.moduleIBScriptsList.Add(filename);
                }
                refreshListBoxIBScripts();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }*/      
        #endregion 

        private void EditIBScript()
        {
            IBScriptEditor newChild = new IBScriptEditor(prntForm.mod, prntForm); //add new child
            newChild.Text = prntForm.mod.moduleIBScriptList[prntForm._selectedLbxIBScriptIndex].scriptName;
            newChild.LoadScript();
            newChild.Show(prntForm.dockPanel1);  //as new form created so that corresponding tab and child form is active
            refreshListBoxIBScripts();
            /*newChild.filename = prntForm.mod.moduleIBScriptsList[prntForm._selectedLbxIBScriptIndex] + ".ibs";
            if (!File.Exists(prntForm._mainDirectory + "\\modules\\" + prntForm.mod.moduleName + "\\ibscript\\" + newChild.filename))
            {
                newChild.SaveScript();
            }
            try
            {
                newChild.LoadScript();
            }
            catch { }*/
            //newChild.SaveScript();
        }
    }
}
