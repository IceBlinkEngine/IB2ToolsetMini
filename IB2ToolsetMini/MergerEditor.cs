using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace IB2ToolsetMini
{
    public partial class MergerEditor : Form
    {
        private Module mod = new Module();
        private Module mergeMod = new Module();
        private ParentForm prntForm;
                
        public MergerEditor(Module m, ParentForm pf)
        {
            InitializeComponent();
            mod = m;
            prntForm = pf;
        }

        #region Handlers
        private void btnImport_Click(object sender, EventArgs e)
        {
            if (lbxImport.SelectedIndex >= 0)
            {
                if (cmbDataType.SelectedIndex == 0) //Class
                {
                    if (!classExists(mergeMod.modulePlayerClassList[lbxImport.SelectedIndex]))
                    {
                        prntForm.mod.modulePlayerClassList.Add(mergeMod.modulePlayerClassList[lbxImport.SelectedIndex].DeepCopy());
                        refreshImportListBox();
                        refreshMainListBox();
                    }
                }
                else if (cmbDataType.SelectedIndex == 1) //Race
                {
                    if (!raceExists(mergeMod.moduleRacesList[lbxImport.SelectedIndex]))
                    {
                        prntForm.mod.moduleRacesList.Add(mergeMod.moduleRacesList[lbxImport.SelectedIndex].DeepCopy());
                        refreshImportListBox();
                        refreshMainListBox();
                    }
                }
                else if (cmbDataType.SelectedIndex == 2) //Spell
                {
                    if (!spellExists(mergeMod.moduleSpellsList[lbxImport.SelectedIndex]))
                    {
                        prntForm.mod.moduleSpellsList.Add(mergeMod.moduleSpellsList[lbxImport.SelectedIndex].DeepCopy());
                        refreshImportListBox();
                        refreshMainListBox();
                    }
                }
                else if (cmbDataType.SelectedIndex == 3) //Trait
                {
                    if (!traitExists(mergeMod.moduleTraitsList[lbxImport.SelectedIndex]))
                    {
                        prntForm.mod.moduleTraitsList.Add(mergeMod.moduleTraitsList[lbxImport.SelectedIndex].DeepCopy());
                        refreshImportListBox();
                        refreshMainListBox();
                    }
                }
                else if (cmbDataType.SelectedIndex == 4) //Effect
                {
                    if (!effectExists(mergeMod.moduleEffectsList[lbxImport.SelectedIndex]))
                    {
                        prntForm.mod.moduleEffectsList.Add(mergeMod.moduleEffectsList[lbxImport.SelectedIndex].DeepCopy());
                        refreshImportListBox();
                        refreshMainListBox();
                    }
                }
                else if (cmbDataType.SelectedIndex == 5) //Creature
                {
                    if (!creatureExists(mergeMod.moduleCreaturesList[lbxImport.SelectedIndex]))
                    {
                        prntForm.mod.moduleCreaturesList.Add(mergeMod.moduleCreaturesList[lbxImport.SelectedIndex].DeepCopy());
                        refreshImportListBox();
                        refreshMainListBox();
                    }
                }
                else if (cmbDataType.SelectedIndex == 6) //Item
                {
                    if (!itemExists(mergeMod.moduleItemsList[lbxImport.SelectedIndex]))
                    {
                        prntForm.mod.moduleItemsList.Add(mergeMod.moduleItemsList[lbxImport.SelectedIndex].DeepCopy());
                        refreshImportListBox();
                        refreshMainListBox();
                    }
                }
                else if (cmbDataType.SelectedIndex == 7) //Area
                {
                    if (!areaExists(mergeMod.moduleAreasObjects[lbxImport.SelectedIndex]))
                    {
                        prntForm.mod.moduleAreasObjects.Add(mergeMod.moduleAreasObjects[lbxImport.SelectedIndex].DeepCopy());
                        refreshImportListBox();
                        refreshMainListBox();
                    }
                }
                else if (cmbDataType.SelectedIndex == 8) //Convo
                {
                    if (!convoExists(mergeMod.moduleConvoList[lbxImport.SelectedIndex]))
                    {
                        prntForm.mod.moduleConvoList.Add(mergeMod.moduleConvoList[lbxImport.SelectedIndex].Clone());
                        refreshImportListBox();
                        refreshMainListBox();
                    }
                }
                else if (cmbDataType.SelectedIndex == 9) //Encounter
                {
                    if (!encExists(mergeMod.moduleEncountersList[lbxImport.SelectedIndex]))
                    {
                        prntForm.mod.moduleEncountersList.Add(mergeMod.moduleEncountersList[lbxImport.SelectedIndex].DeepCopy());
                        refreshImportListBox();
                        refreshMainListBox();
                    }
                }
                else if (cmbDataType.SelectedIndex == 10) //IBScript
                {
                    if (!ibscriptExists(mergeMod.moduleIBScriptList[lbxImport.SelectedIndex]))
                    {
                        prntForm.mod.moduleIBScriptList.Add(mergeMod.moduleIBScriptList[lbxImport.SelectedIndex].DeepCopy());
                        refreshImportListBox();
                        refreshMainListBox();
                    }
                }
            }
        }
        private void btnFolderImport_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = Environment.CurrentDirectory + "\\modules";
            //Empty the FileName text box of the dialog
            openFileDialog1.FileName = String.Empty;
            openFileDialog1.Filter = "Module files (*.mod)|*.mod|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;
            DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {
                loadMergeModule(Path.GetFullPath(openFileDialog1.FileName));
            }            
        }
        private void cmbDataType_SelectedIndexChanged(object sender, EventArgs e)
        {
            refreshImportListBox();
            refreshMainListBox();
        }
        private void pgMain_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            refreshImportListBox();
            refreshMainListBox();
        }
        private void pgImport_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            refreshImportListBox();
            refreshMainListBox();
        }
        private void lbxMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbxMain.SelectedIndex >= 0)
            {
                if (cmbDataType.SelectedIndex == 0) //Class
                {
                    pgMain.SelectedObject = prntForm.mod.modulePlayerClassList[lbxMain.SelectedIndex];
                }
                else if (cmbDataType.SelectedIndex == 1) //Race
                {
                    pgMain.SelectedObject = prntForm.mod.moduleRacesList[lbxMain.SelectedIndex];
                }
                else if (cmbDataType.SelectedIndex == 2) //Spell
                {
                    pgMain.SelectedObject = prntForm.mod.moduleSpellsList[lbxMain.SelectedIndex];
                }
                else if (cmbDataType.SelectedIndex == 3) //Trait
                {
                    pgMain.SelectedObject = prntForm.mod.moduleTraitsList[lbxMain.SelectedIndex];
                }
                else if (cmbDataType.SelectedIndex == 4) //Effect
                {
                    pgMain.SelectedObject = prntForm.mod.moduleEffectsList[lbxMain.SelectedIndex];
                }
                else if (cmbDataType.SelectedIndex == 5) //Creature
                {
                    pgMain.SelectedObject = prntForm.mod.moduleCreaturesList[lbxMain.SelectedIndex];
                }
                else if (cmbDataType.SelectedIndex == 6) //Item
                {
                    pgMain.SelectedObject = prntForm.mod.moduleItemsList[lbxMain.SelectedIndex];
                }
                else if (cmbDataType.SelectedIndex == 7) //Areas
                {
                    pgMain.SelectedObject = prntForm.mod.moduleAreasObjects[lbxMain.SelectedIndex];
                }
                else if (cmbDataType.SelectedIndex == 8) //Convos
                {
                    pgMain.SelectedObject = prntForm.mod.moduleConvoList[lbxMain.SelectedIndex];
                }
                else if (cmbDataType.SelectedIndex == 9) //Encounters
                {
                    pgMain.SelectedObject = prntForm.mod.moduleEncountersList[lbxMain.SelectedIndex];
                }
                else if (cmbDataType.SelectedIndex == 10) //IBscripts
                {
                    pgMain.SelectedObject = prntForm.mod.moduleIBScriptList[lbxMain.SelectedIndex];
                }
            }
        }
        private void lbxImport_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbxImport.SelectedIndex >= 0)
            {
                if (cmbDataType.SelectedIndex == 0) //Class
                {
                    pgImport.SelectedObject = mergeMod.modulePlayerClassList[lbxImport.SelectedIndex];
                }
                else if (cmbDataType.SelectedIndex == 1) //Race
                {
                    pgImport.SelectedObject = mergeMod.moduleRacesList[lbxImport.SelectedIndex];
                }
                else if (cmbDataType.SelectedIndex == 2) //Spell
                {
                    pgImport.SelectedObject = mergeMod.moduleSpellsList[lbxImport.SelectedIndex];
                }
                else if (cmbDataType.SelectedIndex == 3) //Trait
                {
                    pgImport.SelectedObject = mergeMod.moduleTraitsList[lbxImport.SelectedIndex];
                }
                else if (cmbDataType.SelectedIndex == 4) //Effect
                {
                    pgImport.SelectedObject = mergeMod.moduleEffectsList[lbxImport.SelectedIndex];
                }
                else if (cmbDataType.SelectedIndex == 5) //Creature
                {
                    pgImport.SelectedObject = mergeMod.moduleCreaturesList[lbxImport.SelectedIndex];
                }
                else if (cmbDataType.SelectedIndex == 6) //Item
                {
                    pgImport.SelectedObject = mergeMod.moduleItemsList[lbxImport.SelectedIndex];
                }
                else if (cmbDataType.SelectedIndex == 7) //Areas
                {
                    pgImport.SelectedObject = mergeMod.moduleAreasObjects[lbxImport.SelectedIndex];
                }
                else if (cmbDataType.SelectedIndex == 8) //Convos
                {
                    pgImport.SelectedObject = mergeMod.moduleConvoList[lbxImport.SelectedIndex];
                }
                else if (cmbDataType.SelectedIndex == 9) //Encounters
                {
                    pgImport.SelectedObject = mergeMod.moduleEncountersList[lbxImport.SelectedIndex];
                }
                else if (cmbDataType.SelectedIndex == 10) //IBScripts
                {
                    pgImport.SelectedObject = mergeMod.moduleIBScriptList[lbxImport.SelectedIndex];
                }
            }
        }
        #endregion

        #region Methods
        public void loadMergeModule(string modfilepath)
        {
            using (StreamReader sr = File.OpenText(modfilepath))
            {
                string s = "";
                s = sr.ReadLine();
                if (!s.Equals("MODULE"))
                {
                    MessageBox.Show("module file did not have 'MODULE' on first line, aborting...");
                    return;
                }
                //Read in the module file line
                for (int i = 0; i < 99; i++)
                {
                    s = sr.ReadLine();
                    if ((s == null) || (s.Equals("AREAS")))
                    {
                        break;
                    }
                    mergeMod = (Module)JsonConvert.DeserializeObject(s, typeof(Module));
                }
                //Read in the areas
                mergeMod.moduleAreasObjects.Clear();
                Area ar;
                for (int i = 0; i < 9999; i++)
                {
                    s = sr.ReadLine();
                    if ((s == null) || (s.Equals("ENCOUNTERS")))
                    {
                        break;
                    }
                    ar = (Area)JsonConvert.DeserializeObject(s, typeof(Area));
                    mergeMod.moduleAreasObjects.Add(ar);
                }
                //Read in the encounters
                mergeMod.moduleEncountersList.Clear();
                Encounter enc;
                for (int i = 0; i < 9999; i++)
                {
                    s = sr.ReadLine();
                    if ((s == null) || (s.Equals("CONVOS")))
                    {
                        break;
                    }
                    enc = (Encounter)JsonConvert.DeserializeObject(s, typeof(Encounter));
                    mergeMod.moduleEncountersList.Add(enc);
                }
                //Read in the convos
                mergeMod.moduleConvoList.Clear();
                Convo cnv;
                for (int i = 0; i < 9999; i++)
                {
                    s = sr.ReadLine();
                    if ((s == null) || (s.Equals("IMAGES")) || (s.Equals("END")))
                    {
                        break;
                    }
                    cnv = (Convo)JsonConvert.DeserializeObject(s, typeof(Convo));
                    mergeMod.moduleConvoList.Add(cnv);
                }
                //Read in the images
                mergeMod.moduleImageDataList.Clear();
            }
        }
        private void refreshMainListBox()
        {
            if (cmbDataType.SelectedIndex == 0) //Class
            {
                lbxMain.BeginUpdate();
                lbxMain.DataSource = null;
                lbxMain.DataSource = prntForm.mod.modulePlayerClassList;
                lbxMain.DisplayMember = "name";
                lbxMain.EndUpdate();
            }
            else if (cmbDataType.SelectedIndex == 1) //Race
            {
                lbxMain.BeginUpdate();
                lbxMain.DataSource = null;
                lbxMain.DataSource = prntForm.mod.moduleRacesList;
                lbxMain.DisplayMember = "name";
                lbxMain.EndUpdate();
            }
            else if (cmbDataType.SelectedIndex == 2) //Spell
            {
                lbxMain.BeginUpdate();
                lbxMain.DataSource = null;
                lbxMain.DataSource = prntForm.mod.moduleSpellsList;
                lbxMain.DisplayMember = "name";
                lbxMain.EndUpdate();
            }
            else if (cmbDataType.SelectedIndex == 3) //Trait
            {
                lbxMain.BeginUpdate();
                lbxMain.DataSource = null;
                lbxMain.DataSource = prntForm.mod.moduleTraitsList;
                lbxMain.DisplayMember = "name";
                lbxMain.EndUpdate();
            }
            else if (cmbDataType.SelectedIndex == 4) //Effect
            {
                lbxMain.BeginUpdate();
                lbxMain.DataSource = null;
                lbxMain.DataSource = prntForm.mod.moduleEffectsList;
                lbxMain.DisplayMember = "name";
                lbxMain.EndUpdate();
            }
            else if (cmbDataType.SelectedIndex == 5) //Creature
            {
                lbxMain.BeginUpdate();
                lbxMain.DataSource = null;
                lbxMain.DataSource = prntForm.mod.moduleCreaturesList;
                lbxMain.DisplayMember = "cr_name";
                lbxMain.EndUpdate();
            }
            else if (cmbDataType.SelectedIndex == 6) //Item
            {
                lbxMain.BeginUpdate();
                lbxMain.DataSource = null;
                lbxMain.DataSource = prntForm.mod.moduleItemsList;
                lbxMain.DisplayMember = "name";
                lbxMain.EndUpdate();
            }
            else if (cmbDataType.SelectedIndex == 7) //Areas
            {
                lbxMain.BeginUpdate();
                lbxMain.DataSource = null;
                lbxMain.DataSource = prntForm.mod.moduleAreasObjects;
                lbxMain.DisplayMember = "Filename";
                lbxMain.EndUpdate();
            }
            else if (cmbDataType.SelectedIndex == 8) //Convos
            {
                lbxMain.BeginUpdate();
                lbxMain.DataSource = null;
                lbxMain.DataSource = prntForm.mod.moduleConvoList;
                lbxMain.DisplayMember = "ConvoFileName";
                lbxMain.EndUpdate();
            }
            else if (cmbDataType.SelectedIndex == 9) //Encounters
            {
                lbxMain.BeginUpdate();
                lbxMain.DataSource = null;
                lbxMain.DataSource = prntForm.mod.moduleEncountersList;
                lbxMain.DisplayMember = "encounterName";
                lbxMain.EndUpdate();
            }
            else if (cmbDataType.SelectedIndex == 10) //IBScripts
            {
                lbxMain.BeginUpdate();
                lbxMain.DataSource = null;
                lbxMain.DataSource = prntForm.mod.moduleIBScriptList;
                lbxMain.DisplayMember = "scriptName";
                lbxMain.EndUpdate();
            }
        }
        private void refreshImportListBox()
        {
            if (cmbDataType.SelectedIndex == 0) //Class
            {
                lbxImport.BeginUpdate();
                lbxImport.DataSource = null;
                lbxImport.DataSource = mergeMod.modulePlayerClassList;
                lbxImport.DisplayMember = "name";
                lbxImport.EndUpdate();
            }
            else if (cmbDataType.SelectedIndex == 1) //Race
            {
                lbxImport.BeginUpdate();
                lbxImport.DataSource = null;
                lbxImport.DataSource = mergeMod.moduleRacesList;
                lbxImport.DisplayMember = "name";
                lbxImport.EndUpdate();
            }
            else if (cmbDataType.SelectedIndex == 2) //Spell
            {
                lbxImport.BeginUpdate();
                lbxImport.DataSource = null;
                lbxImport.DataSource = mergeMod.moduleSpellsList;
                lbxImport.DisplayMember = "name";
                lbxImport.EndUpdate();
            }
            else if (cmbDataType.SelectedIndex == 3) //Trait
            {
                lbxImport.BeginUpdate();
                lbxImport.DataSource = null;
                lbxImport.DataSource = mergeMod.moduleTraitsList;
                lbxImport.DisplayMember = "name";
                lbxImport.EndUpdate();
            }
            else if (cmbDataType.SelectedIndex == 4) //Effect
            {
                lbxImport.BeginUpdate();
                lbxImport.DataSource = null;
                lbxImport.DataSource = mergeMod.moduleEffectsList;
                lbxImport.DisplayMember = "name";
                lbxImport.EndUpdate();
            }
            else if (cmbDataType.SelectedIndex == 5) //Creature
            {
                lbxImport.BeginUpdate();
                lbxImport.DataSource = null;
                lbxImport.DataSource = mergeMod.moduleCreaturesList;
                lbxImport.DisplayMember = "cr_name";
                lbxImport.EndUpdate();
            }
            else if (cmbDataType.SelectedIndex == 6) //Item
            {
                lbxImport.BeginUpdate();
                lbxImport.DataSource = null;
                lbxImport.DataSource = mergeMod.moduleItemsList;
                lbxImport.DisplayMember = "name";
                lbxImport.EndUpdate();
            }
            else if (cmbDataType.SelectedIndex == 7) //Areas
            {
                lbxImport.BeginUpdate();
                lbxImport.DataSource = null;
                lbxImport.DataSource = mergeMod.moduleAreasObjects;
                lbxImport.DisplayMember = "Filename";
                lbxImport.EndUpdate();
            }
            else if (cmbDataType.SelectedIndex == 8) //Convos
            {
                lbxImport.BeginUpdate();
                lbxImport.DataSource = null;
                lbxImport.DataSource = mergeMod.moduleConvoList;
                lbxImport.DisplayMember = "ConvoFileName";
                lbxImport.EndUpdate();
            }
            else if (cmbDataType.SelectedIndex == 9) //Encounters
            {
                lbxImport.BeginUpdate();
                lbxImport.DataSource = null;
                lbxImport.DataSource = mergeMod.moduleEncountersList;
                lbxImport.DisplayMember = "encounterName";
                lbxImport.EndUpdate();
            }
            else if (cmbDataType.SelectedIndex == 10) //IBScripts
            {
                lbxImport.BeginUpdate();
                lbxImport.DataSource = null;
                lbxImport.DataSource = mergeMod.moduleIBScriptList;
                lbxImport.DisplayMember = "scriptName";
                lbxImport.EndUpdate();
            }
        }        
        private bool classExists(PlayerClass itImp)
        {
            foreach (PlayerClass it in prntForm.mod.modulePlayerClassList)
            {
                if (it.tag == itImp.tag)
                {
                    return true;
                }
            }
            return false;
        }
        private bool raceExists(Race itImp)
        {
            foreach (Race it in prntForm.mod.moduleRacesList)
            {
                if (it.tag == itImp.tag)
                {
                    return true;
                }
            }
            return false;
        }
        private bool spellExists(Spell itImp)
        {
            foreach (Spell it in prntForm.mod.moduleSpellsList)
            {
                if (it.tag == itImp.tag)
                {
                    return true;
                }
            }
            return false;
        }
        private bool traitExists(Trait itImp)
        {
            foreach (Trait it in prntForm.mod.moduleTraitsList)
            {
                if (it.tag == itImp.tag)
                {
                    return true;
                }
            }
            return false;
        }
        private bool effectExists(Effect itImp)
        {
            foreach (Effect it in prntForm.mod.moduleEffectsList)
            {
                if (it.tag == itImp.tag)
                {
                    return true;
                }
            }
            return false;
        }
        private bool creatureExists(Creature itImp)
        {
            foreach (Creature it in prntForm.mod.moduleCreaturesList)
            {
                if (it.cr_resref == itImp.cr_resref)
                {
                    return true;
                }
            }
            return false;
        }
        private bool itemExists(Item itImp)
        {
            foreach (Item it in prntForm.mod.moduleItemsList)
            {
                if (it.resref == itImp.resref)
                {
                    return true;
                }
            }
            return false;
        }
        private bool areaExists(Area itImp)
        {
            foreach (Area it in prntForm.mod.moduleAreasObjects)
            {
                if (it.Filename == itImp.Filename)
                {
                    return true;
                }
            }
            return false;
        }
        private bool convoExists(Convo itImp)
        {
            foreach (Convo it in prntForm.mod.moduleConvoList)
            {
                if (it.ConvoFileName == itImp.ConvoFileName)
                {
                    return true;
                }
            }
            return false;
        }
        private bool ibscriptExists(IBScript itImp)
        {
            foreach (IBScript it in prntForm.mod.moduleIBScriptList)
            {
                if (it.scriptName == itImp.scriptName)
                {
                    return true;
                }
            }
            return false;
        }
        private bool encExists(Encounter itImp)
        {
            foreach (Encounter it in prntForm.mod.moduleEncountersList)
            {
                if (it.encounterName == itImp.encounterName)
                {
                    return true;
                }
            }
            return false;
        }
        #endregion        

        private void btnInstructions_Click(object sender, EventArgs e)
        {
            string text = "To copy/merge data from another module, first click on the '...' button"
                        + " and browse to the 'data' folder inside the module folder (the one you"
                        + " want to copy data from). Click on that 'data' folder and then click on"
                        + " the 'okay' button. Next, select a data type from the dropdown at the top"
                        + " center of this editor. Once the data type is selected you will be able"
                        + " to see and compare the individual items of that data group from your module"
                        + " compared to the module you want to copy data from. Editing the data in the"
                        + " PropertyGrid of you module's data ('Current Module Data') will actually modify the data in your module"
                        + " so feel free to edit/update data in your module while comparing to the other"
                        + " module. To copy a data item over to your module, select the item in the Import Data"
                        + " list that you want to copy and then click on the '<<<copy over selected<<<' button.";
            MessageBox.Show(text);
        }
    }
}
