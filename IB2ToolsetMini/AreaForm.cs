using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using System.IO;

namespace IB2ToolsetMini
{
    public partial class AreaForm : DockContent
    {
        public ParentForm prntForm;

        public AreaForm(ParentForm pf)
        {
            InitializeComponent();
            prntForm = pf;
        }

        #region Area Stuff
        public void refreshListBoxAreas()
        {
            lbxAreas.BeginUpdate();
            lbxAreas.DataSource = null;
            lbxAreas.DataSource = prntForm.mod.moduleAreasObjects;
            lbxAreas.DisplayMember = "Filename";
            lbxAreas.EndUpdate();
        }
        private void lbxAreas_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if ((lbxAreas.Items.Count > 0) && (lbxAreas.SelectedIndex >= 0))
            {
                int index = this.lbxAreas.IndexFromPoint(e.Location);
                if (index != System.Windows.Forms.ListBox.NoMatches)
                {
                    EditArea();
                }
            }
        }
        private void lbxAreas_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (lbxAreas.SelectedIndex >= 0)
            {
                prntForm._selectedLbxAreaIndex = lbxAreas.SelectedIndex;
                txtAreaName.Text = prntForm.mod.moduleAreasObjects[prntForm._selectedLbxAreaIndex].Filename;
                lbxAreas.SelectedIndex = prntForm._selectedLbxAreaIndex;                
            }
        }
        private void txtAreaName_TextChanged_1(object sender, EventArgs e)
        {
            try
            {
                prntForm.mod.moduleAreasObjects[prntForm._selectedLbxAreaIndex].Filename = txtAreaName.Text;
                refreshListBoxAreas();
            }
            catch { }
        }
        private void btnAddArea_Click_1(object sender, EventArgs e)
        {
            Area newArea = new Area();
            newArea.Filename = "new area";
            newArea.SetAllToGrass();
            //prntForm.mod.moduleAreasList.Add(newArea.Filename);
            prntForm.mod.moduleAreasObjects.Add(newArea);
            refreshListBoxAreas();
        }
        private void btnRemoveArea_Click_1(object sender, EventArgs e)
        {
            if ((lbxAreas.Items.Count > 0) && (lbxAreas.SelectedIndex >= 0))
            {
                try
                {
                    // The Remove button was clicked.
                    int selectedIndex = lbxAreas.SelectedIndex;
                    prntForm.mod.moduleAreasObjects.RemoveAt(selectedIndex);
                }
                catch { }
                prntForm._selectedLbxAreaIndex = 0;
                lbxAreas.SelectedIndex = 0;
                refreshListBoxAreas();
            }
        }
        private void btnEditArea_Click_1(object sender, EventArgs e)
        {
            if ((lbxAreas.Items.Count > 0) && (lbxAreas.SelectedIndex >= 0))
            {
                EditArea();                
            }
        }
        private void btnRename_Click(object sender, EventArgs e)
        {
            if ((lbxAreas.Items.Count > 0) && (lbxAreas.SelectedIndex >= 0))
            {
                RenameDialog newName = new RenameDialog();
                DialogResult result = newName.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    try
                    {
                        prntForm.mod.moduleAreasObjects[prntForm._selectedLbxAreaIndex].Filename = newName.RenameText;
                        refreshListBoxAreas();
                    }
                    catch { }
                }
            }
        }
        private void btnSort_Click(object sender, EventArgs e)
        {
            prntForm.mod.moduleAreasObjects = prntForm.mod.moduleAreasObjects.OrderBy(o => o.Filename).ToList();
            refreshListBoxAreas();
        }
        private void btnDuplicate_Click(object sender, EventArgs e)
        {
            if ((lbxAreas.Items.Count > 0) && (lbxAreas.SelectedIndex >= 0))
            {
                try
                {
                    Area newArea = new Area();
                    newArea = prntForm.mod.moduleAreasObjects[prntForm._selectedLbxAreaIndex].DeepCopy();
                    newArea.Filename = prntForm.mod.moduleAreasObjects[prntForm._selectedLbxAreaIndex].Filename + "-Copy";
                    prntForm.mod.moduleAreasObjects.Add(newArea);
                    refreshListBoxAreas();
                }
                catch { }
            }
        }        
        #endregion

        private void EditArea()
        {
            //if (prntForm.mod.moduleAreasList[prntForm._selectedLbxAreaIndex].StartsWith("wm_"))
            //{
                WorldMapEditor newChild = new WorldMapEditor(prntForm.mod, prntForm); //add new child
                newChild.Text = prntForm.mod.moduleAreasObjects[prntForm._selectedLbxAreaIndex].Filename;
                newChild.Show(prntForm.dockPanel1); //as new form created so that corresponding tab and child form is active
                refreshListBoxAreas();
                //newChild.g_directory = prntForm._mainDirectory + "\\modules\\" + prntForm.mod.moduleName + "\\areas";
                //newChild.g_filename = prntForm.mod.moduleAreasList[prntForm._selectedLbxAreaIndex];
            /*}
            else
            {
                LevelEditor newChild = new LevelEditor(prntForm.mod, prntForm); //add new child
                newChild.Text = prntForm.mod.moduleAreasList[prntForm._selectedLbxAreaIndex];
                newChild.Show(prntForm.dockPanel1); //as new form created so that corresponding tab and child form is active
                refreshListBoxAreas();
                newChild.g_directory = prntForm._mainDirectory + "\\modules\\" + prntForm.mod.moduleName + "\\areas";
                newChild.g_filename = prntForm.mod.moduleAreasList[prntForm._selectedLbxAreaIndex];
            }*/
        }

        /*private void btnLoadAllArea_Click(object sender, EventArgs e)
        {
            string jobDir = "";
            jobDir = prntForm._mainDirectory + "\\modules\\" + prntForm.mod.moduleName + "\\areas";
            prntForm.mod.moduleAreasList.Clear();
            foreach (string f in Directory.GetFiles(jobDir, "*.lvl"))
            {
                string filename = Path.GetFileNameWithoutExtension(f);
                prntForm.mod.moduleAreasList.Add(filename);
            }
            refreshListBoxAreas();
        }*/
    }
}
