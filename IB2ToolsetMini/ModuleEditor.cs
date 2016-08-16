using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IB2ToolsetMini
{
    public partial class ModuleEditor : Form
    {
        private Module mod;
        private ParentForm prntForm;

        public string moveDiagCostInfo = "This defines the amount of movement points that are consumed for diagonal moves in combat";
        public string ArmorClassDisplayInfo = "Defines the way Armor Class is displayed. Ascending goes from 10 -> 30+ (think 3e) and Descending goes from 10 -> -10- (think 1e)";
        public string toHitBonusFromBehindInfo = "To hit bonus when attacking from behind";
        public string useLuckInfo = "Luck is an additional attribute that's heigh wehn the attribute scores of a char are low; can e.g. be checked by authors for special event or dialgoue situations";
        public string rollingSystemInfo = "The 3d6 system generates results between 3 and 18, by three times adding numbers from 1 to 6; the 2d6+6 method generates results from 8 to 18 by adding 6 to a range of numbers from 2 - 12.";

        public ModuleEditor(Module m, ParentForm pf)
        {
            InitializeComponent();
            mod = m;
            prntForm = pf;
            resetForm();
            propertyGrid1.SelectedObject = mod;
        }

        public void resetForm()
        {
            //Diagonal Move Cost
            if (mod.diagonalMoveCost == 1.0f)
            {
                rbtnOneSquare.Checked = true;
            }
            else
            {
                rbtnOnePointFiveSquares.Checked = true;
            }
            //Armor Class Diplay
            if (mod.ArmorClassAscending)
            {
                rbtnAscendingAC.Checked = true;
            }
            else
            {
                rbtnDescendingAC.Checked = true;
            }
            //to hit bonus from behind
            if (mod.attackFromBehindToHitModifier == 1)
            {
                rbtnPlusOneToHitFromBehind.Checked = true;
            }
            else if (mod.attackFromBehindToHitModifier == 2)
            {
                rbtnPlusTwoToHitFromBehind.Checked = true;
            }
            else if (mod.attackFromBehindToHitModifier == 3)
            {
                rbtnPlusThreeToHitFromBehind.Checked = true;
            }
            else if (mod.attackFromBehindToHitModifier == 4)
            {
                rbtnPlusFourToHitFromBehind.Checked = true;
            }

            // decide for attribute rolling system
            if (mod.use3d6)
            {
                rbtnUse3d6.Checked = true;
            }
            else
            {
                rbtnUse6Plusd12.Checked = true;
            }
        }

        #region Diagonal Move Cost Stuff
        private void gbMoveDiagonalCost_MouseHover(object sender, EventArgs e)
        {
            rtxtInfo.Text = moveDiagCostInfo;
        }
        private void rbtnOneSquare_MouseHover(object sender, EventArgs e)
        {
            rtxtInfo.Text = moveDiagCostInfo;
        }
        private void rbtnOnePointFiveSquares_MouseHover(object sender, EventArgs e)
        {
            rtxtInfo.Text = moveDiagCostInfo;
        }
        private void rbtnOneSquare_CheckedChanged(object sender, EventArgs e)
        {
            mod.diagonalMoveCost = 1.0f;
        }
        private void rbtnOnePointFiveSquares_CheckedChanged(object sender, EventArgs e)
        {
            mod.diagonalMoveCost = 1.5f;
        }
        #endregion
        #region Armor Class Display
        private void gbArmorClassDisplay_MouseHover(object sender, EventArgs e)
        {
            rtxtInfo.Text = ArmorClassDisplayInfo;
        }
        private void rbtnAscendingAC_MouseHover(object sender, EventArgs e)
        {
            rtxtInfo.Text = ArmorClassDisplayInfo;
        }
        private void rbtnDescendingAC_MouseHover(object sender, EventArgs e)
        {
            rtxtInfo.Text = ArmorClassDisplayInfo;
        }
        private void rbtnAscendingAC_CheckedChanged(object sender, EventArgs e)
        {
            mod.ArmorClassAscending = true;
        }
        private void rbtnDescendingAC_CheckedChanged(object sender, EventArgs e)
        {
            mod.ArmorClassAscending = false;
        }
        #endregion
        #region To hit bonus from behind
        private void gbToHitBonusFromBehind_MouseHover(object sender, EventArgs e)
        {
            rtxtInfo.Text = toHitBonusFromBehindInfo;
        }
        private void rbtnPlusOneToHitFromBehind_MouseHover(object sender, EventArgs e)
        {
            rtxtInfo.Text = toHitBonusFromBehindInfo;
        }
        private void rbtnPlusTwoToHitFromBehind_MouseHover(object sender, EventArgs e)
        {
            rtxtInfo.Text = toHitBonusFromBehindInfo;
        }
        private void rbtnPlusThreeToHitFromBehind_MouseHover(object sender, EventArgs e)
        {
            rtxtInfo.Text = toHitBonusFromBehindInfo;
        }
        private void rbtnPlusFourToHitFromBehind_MouseHover(object sender, EventArgs e)
        {
            rtxtInfo.Text = toHitBonusFromBehindInfo;
        }

        private void rbtnPlusOneToHitFromBehind_CheckedChanged(object sender, EventArgs e)
        {
            mod.attackFromBehindToHitModifier = 1;
        }
        private void rbtnPlusTwoToHitFromBehind_CheckedChanged(object sender, EventArgs e)
        {
            mod.attackFromBehindToHitModifier = 2;
        }
        private void rbtnPlusThreeToHitFromBehind_CheckedChanged(object sender, EventArgs e)
        {
            mod.attackFromBehindToHitModifier = 3;
        }
        private void rbtnPlusFourToHitFromBehind_CheckedChanged(object sender, EventArgs e)
        {
            mod.attackFromBehindToHitModifier = 4;
        }

        #endregion
        #region Decide for attribute rolling system
        private void gbRollingSystem_MouseHover(object sender, EventArgs e)
        {
            rtxtInfo.Text = rollingSystemInfo;
        }
        private void rbtnUse3d6_MouseHover(object sender, EventArgs e)
        {
            rtxtInfo.Text = rollingSystemInfo;
        }
        private void rbtnUse6Plusd12_MouseHover(object sender, EventArgs e)
        {
            rtxtInfo.Text = rollingSystemInfo;
        }
        private void rbtnUse3d6_CheckedChanged(object sender, EventArgs e)
        {
            mod.use3d6 = true;
        }
        private void rbtnUse6Plusd12_CheckedChanged(object sender, EventArgs e)
        {
            mod.use3d6 = false;
        }
        #endregion

        private void splitContainer1_Panel1_MouseHover(object sender, EventArgs e)
        {
            rtxtInfo.Text = "";
        }
        private void rtxtInfo_MouseHover(object sender, EventArgs e)
        {
            rtxtInfo.Text = "";
        }

        private void btnLoadTitleImage_Click(object sender, EventArgs e)
        {
            
        }

        private void btnLoadArtResources_Click(object sender, EventArgs e)
        {
            //load file dialog and multiselect images to load
            OpenFileDialog ofd = new OpenFileDialog();            
            ofd.InitialDirectory = prntForm._mainDirectory;
            ofd.FileName = String.Empty;
            ofd.Filter = "Image files (*.png)|*.png|All files (*.*)|*.*";
            ofd.FilterIndex = 1;
            ofd.Multiselect = true;

            DialogResult result = ofd.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {                
                string[] filenames = ofd.FileNames;
                string directory = Path.GetDirectoryName(ofd.FileName);
                //convert all images to ImageData and add to the Module list List<ImageData>
                //also add images to imageDictionary Dictionary<string, Bitmap>
                foreach (string s in filenames)
                {
                    string name = Path.GetFileNameWithoutExtension(s);
                    prntForm.mod.moduleImageDataList.Add(prntForm.bsc.ConvertBitmapToImageData(name, s));
                    prntForm.resourcesBitmapList.Add(name, new Bitmap(s));
                }
                //repopulate the flow panel with updated imageDictionary
                this.flwArtResources.Controls.Clear();
                foreach (KeyValuePair<string, Bitmap> kvp in prntForm.resourcesBitmapList)
                {
                    RadioButton btnNew = new RadioButton();
                    btnNew.Appearance = System.Windows.Forms.Appearance.Button;
                    btnNew.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
                    btnNew.BackgroundImage = kvp.Value;
                    btnNew.Name = kvp.Key;
                    btnNew.Size = new System.Drawing.Size(kvp.Value.Width + 6, kvp.Value.Height + 6);
                    btnNew.Text = kvp.Key;
                    btnNew.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
                    btnNew.UseVisualStyleBackColor = true;
                    this.flwArtResources.Controls.Add(btnNew);
                }
            }            
        }

        private void btnRemoveSelectedArtResource_Click(object sender, EventArgs e)
        {

        }
    }
}
