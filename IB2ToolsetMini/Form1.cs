﻿/* IB2ToolsetMini by Jeremy Smith, copyright 2016 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using WeifenLuo.WinFormsUI.Docking;
using Newtonsoft.Json;

namespace IB2ToolsetMini
{
    public partial class ParentForm : Form
    {
        public string _mainDirectory;
        public Module mod = new Module();
        public BitmapStringConversion bsc;
        public int tileSizeInPixels = 48;
        public Dictionary<string, Bitmap> resourcesBitmapList = new Dictionary<string, Bitmap>();
        //public List<JournalQuest> journal = new List<JournalQuest>();
        //public List<Creature> creaturesList = new List<Creature>();
        //public List<Container> containersList = new List<Container>();
        //public List<Shop> shopsList = new List<Shop>();
        //public List<Encounter> encountersList = new List<Encounter>();
        //public List<Item> itemsList = new List<Item>();
        //public List<Prop> propsList = new List<Prop>();
        //public List<PlayerClass> playerClassesList = new List<PlayerClass>();
        //public List<Race> racesList = new List<Race>();
        //public List<Spell> spellsList = new List<Spell>();
        //public List<Trait> traitsList = new List<Trait>();
        //public List<Effect> effectsList = new List<Effect>();
        public List<string> itemsParentNodeList = new List<string>();
        public List<string> creaturesParentNodeList = new List<string>();
        public List<string> propsParentNodeList = new List<string>();
        //public List<Area> openAreasList = new List<Area>();
        //public List<Convo> openConvosList = new List<Convo>();
        public List<string> scriptList = new List<string>();
        public List<Condition> copiedConditionalsList = new List<Condition>();
        public List<Action> copiedActionsList = new List<Action>();
        public string selectedEncounterCreatureTag = "";
        public string selectedEncounterPropTag = "";
        public string selectedEncounterTriggerTag = "";
        public string selectedLevelMapCreatureTag = "";
        public string selectedLevelMapPropTag = "";
        public string selectedLevelMapTriggerTag = "";
        public bool CreatureSelected = false;
        public bool PropSelected = false;
        public int nodeCount = 1;
        public int createdTab = 0;
        public int _selectedLbxAreaIndex;
        public int _selectedLbxConvoIndex;
        public int _selectedLbxLogicTreeIndex;
        public int _selectedLbxIBScriptIndex;
        public int _selectedLbxContainerIndex;
        public int _selectedLbxEncounterIndex;
        public string lastSelectedCreatureNodeName = "";
        public string lastSelectedItemNodeName = "";
        public string lastSelectedPropNodeName = "";
        public Trigger currentSelectedTrigger = null;
        public Bitmap iconBitmap;
        public string returnImageFilenameFromImageSelector = "";
        public string lastModuleFullPath;
        public string versionMessage = "IceBlink 2 Mini Toolset for creating adventure modules for the PC and Android.\r\n\r\n IceBlink 2 Mini Toolset ver 0.90";
        
        private DeserializeDockContent m_deserializeDockContent;
        public IceBlinkProperties frmIceBlinkProperties;
        public IconSprite frmIconSprite;
        public TriggerEventsForm frmTriggerEvents;
        public Blueprints frmBlueprints;
        public AreaForm frmAreas;
        public ConversationsForm frmConversations;
        public IBScriptForm frmIBScript;
        public EncountersForm frmEncounters;
        public ContainersForm frmContainers;
        public LogForm frmLog;
        public bool m_bSaveLayout = true;


        public ParentForm()
        {
            InitializeComponent();
            _mainDirectory = Directory.GetCurrentDirectory();
            bsc = new BitmapStringConversion();
            dockPanel1.Dock = DockStyle.Fill;
            dockPanel1.BackColor = Color.Beige;
            dockPanel1.BringToFront();
            frmIceBlinkProperties = new IceBlinkProperties(this);
            frmIconSprite = new IconSprite(this);
            frmTriggerEvents = new TriggerEventsForm(this);
            frmBlueprints = new Blueprints(this);
            frmAreas = new AreaForm(this);
            frmConversations = new ConversationsForm(this);
            frmIBScript = new IBScriptForm(this);
            frmEncounters = new EncountersForm(this);
            frmContainers = new ContainersForm(this);
            frmLog = new LogForm(this);
            m_deserializeDockContent = new DeserializeDockContent(GetContentFromPersistString);
        }        
        private void ParentForm_Load(object sender, EventArgs e)
        {
            string configFile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "DockPanel.config");
            string configDefaultFile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "DefaultLayout.config");

            if (File.Exists(configFile))
            {
                dockPanel1.LoadFromXml(configFile, m_deserializeDockContent);
            }
            else if (File.Exists(configDefaultFile))
            {
                dockPanel1.LoadFromXml(configDefaultFile, m_deserializeDockContent);
            }
            else
            {
                //do nothing
            }

            openModule(_mainDirectory + "\\default\\NewModule\\NewModule.mod");
            //openCreatures(_mainDirectory + "\\default\\NewModule\\data\\creatures.json");
            //openItems(_mainDirectory + "\\default\\NewModule\\data\\items.json");
            //openProps(_mainDirectory + "\\default\\NewModule\\data\\props.json");
            //openJournal(_mainDirectory + "\\default\\NewModule\\data\\journal.json");
            //openPlayerClasses(_mainDirectory + "\\default\\NewModule\\data\\playerClasses.json");
            //openRaces(_mainDirectory + "\\default\\NewModule\\data\\races.json");
            //openSkills(_mainDirectory + "\\data\\NewModule\\data\\" + mod.SkillsFileName);
            //openSpells(_mainDirectory + "\\default\\NewModule\\data\\spells.json");
            //openTraits(_mainDirectory + "\\default\\NewModule\\data\\traits.json");
            //openEffects(_mainDirectory + "\\default\\NewModule\\data\\effects.json");
            //game.errorLog("Starting IceBlink Toolset");

            frmBlueprints.UpdateTreeViewCreatures();
            loadCreatureSprites();
            frmBlueprints.UpdateTreeViewItems();
            loadItemSprites();
            frmContainers.refreshListBoxContainers();
            frmEncounters.refreshListBoxEncounters();
            frmBlueprints.UpdateTreeViewProps();
            loadPropSprites();
            refreshDropDownLists();

            //saveAsTemp();

            //fill all lists
            DropdownStringLists.aiTypeStringList = new List<string> { "BasicAttacker", "GeneralCaster" };
            DropdownStringLists.damageTypeStringList = new List<string> { "Normal", "Acid", "Cold", "Electricity", "Fire", "Magic", "Poison" };
            DropdownStringLists.itemTypeStringList = new List<string> { "Head", "Neck", "Armor", "Ranged", "Melee", "General", "Ring", "Shield", "Feet", "Ammo" };
            DropdownStringLists.useableWhenStringList = new List<string> { "InCombat", "OutOfCombat", "Always", "Passive" };
            DropdownStringLists.weaponTypeStringList = new List<string> { "Ranged", "Melee" };
            DropdownStringLists.moverTypeStringList = new List<string> { "post", "random", "patrol", "daily", "weekly", "monthly", "yearly"};

            this.WindowState = FormWindowState.Maximized;
        }
        private void ParentForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //close all open tab documents first
            CloseAllDocuments();
            string configFile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "DockPanel.config");
            if (m_bSaveLayout)
                dockPanel1.SaveAsXml(configFile);
            else if (File.Exists(configFile))
                File.Delete(configFile);
        }
        private IDockContent GetContentFromPersistString(string persistString)
        {
            if (persistString == typeof(IceBlinkProperties).ToString())
                return frmIceBlinkProperties;
            else if (persistString == typeof(IconSprite).ToString())
                return frmIconSprite;
            else if (persistString == typeof(Blueprints).ToString())
                return frmBlueprints;
            else if (persistString == typeof(AreaForm).ToString())
                return frmAreas;
            else if (persistString == typeof(TriggerEventsForm).ToString())
                return frmTriggerEvents;
            else if (persistString == typeof(ConversationsForm).ToString())
                return frmConversations;
            //REMOVEelse if (persistString == typeof(LogicTreeForm).ToString())
            //REMOVE    return frmLogicTree;
            else if (persistString == typeof(IBScriptForm).ToString())
                return frmIBScript;
            else if (persistString == typeof(EncountersForm).ToString())
                return frmEncounters;
            else if (persistString == typeof(ContainersForm).ToString())
                return frmContainers;
            else //(persistString == typeof(LogForm).ToString())
                return frmLog;
        }
        private void CloseAllDocuments()
        {            
            for (int index = dockPanel1.Contents.Count - 1; index >= 0; index--)
            {
                if (dockPanel1.Contents[index] is IDockContent)
                {                    
                    IDockContent content = (IDockContent)dockPanel1.Contents[index];
                    if ((content.DockHandler.TabText == "Areas") ||
                        (content.DockHandler.TabText == "Conversations") ||
                        (content.DockHandler.TabText == "Containers") ||
                        (content.DockHandler.TabText == "Encounters") ||
                        (content.DockHandler.TabText == "TriggerEvents") ||
                        (content.DockHandler.TabText == "IBScripts") ||
                        (content.DockHandler.TabText == "LogForm") ||
                        (content.DockHandler.TabText == "Blueprints") ||
                        (content.DockHandler.TabText == "Properties") ||
                        (content.DockHandler.TabText == "IconSprite"))
                    {
                        //skip these, do not close them
                    }
                    else
                    {
                        content.DockHandler.Close();
                    }
                }
            }
        }
        private void loadAllDefaultToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("not implemented yet");            
        }

        public void errorLog(string text)
        {
            if (_mainDirectory == null)
            {
                _mainDirectory = Directory.GetCurrentDirectory();
            }
            using (StreamWriter writer = new StreamWriter(_mainDirectory + "//IB2ToolsetErrorLog.txt", true))
            {
                writer.Write(DateTime.Now + ": ");
                writer.WriteLine(text);
            }
        }

        #region File Handling
        private void openModule(string filename)
        {            
            //mod = mod.loadModuleFile(filename);
            //if (mod == null)
            //{
            //    MessageBox.Show("returned a null module");
            //}

            //new method
            using (StreamReader sr = File.OpenText(filename))
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
                    mod = (Module)JsonConvert.DeserializeObject(s, typeof(Module));
                }
                //Read in the areas
                mod.moduleAreasObjects.Clear();
                Area ar;
                for (int i = 0; i < 9999; i++)
                {
                    s = sr.ReadLine();
                    if ((s == null) || (s.Equals("ENCOUNTERS")))
                    {
                        break;
                    }
                    ar = (Area)JsonConvert.DeserializeObject(s, typeof(Area));
                    mod.moduleAreasObjects.Add(ar);
                }
                //Read in the encounters
                mod.moduleEncountersList.Clear();
                Encounter enc;
                for (int i = 0; i < 9999; i++)
                {
                    s = sr.ReadLine();
                    if ((s == null) || (s.Equals("CONVOS")))
                    {
                        break;
                    }
                    enc = (Encounter)JsonConvert.DeserializeObject(s, typeof(Encounter));
                    mod.moduleEncountersList.Add(enc);
                }
                //Read in the convos
                mod.moduleConvoList.Clear();
                Convo cnv;
                for (int i = 0; i < 9999; i++)
                {
                    s = sr.ReadLine();
                    if ((s == null) || (s.Equals("IMAGES")) || (s.Equals("END")))
                    {
                        break;
                    }
                    cnv = (Convo)JsonConvert.DeserializeObject(s, typeof(Convo));
                    mod.moduleConvoList.Add(cnv);
                }
                //Read in the images
                mod.moduleImageDataList.Clear();
                resourcesBitmapList.Clear();
                ImageData imd;
                for (int i = 0; i < 9999; i++)
                {
                    s = sr.ReadLine();
                    if ((s == null) || (s.Equals("END")))
                    {
                        break;
                    }
                    imd = (ImageData)JsonConvert.DeserializeObject(s, typeof(ImageData));
                    mod.moduleImageDataList.Add(imd);
                    resourcesBitmapList.Add(imd.name, bsc.ConvertImageDataToBitmap(imd));
                }
            }

            //frmAreas.lbxAreas.DataSource = null;
            //frmAreas.lbxAreas.DataSource = mod.moduleAreasObjects;
            //frmAreas.lbxAreas.DisplayMember = "Filename";
            frmAreas.refreshListBoxAreas();            
            frmConversations.refreshListBoxConvos();
            //REMOVEfrmLogicTree.refreshListBoxLogicTrees();
            frmIBScript.refreshListBoxIBScripts();
        }
        private void openCreatures(string filename)
        {
            if (File.Exists(filename))
            {
                mod.moduleCreaturesList.Clear();
                //mod.moduleCreaturesList = loadCreaturesFile(filename);
            }
            else
            {
                MessageBox.Show("Couldn't find creatures.json file. Will create a new one upon saving module.");
            }
            frmBlueprints.UpdateTreeViewCreatures();
            loadCreatureSprites();
        }
        private void loadCreatureSprites()
        {
            foreach (Creature crt in mod.moduleCreaturesList)
            {
                crt.LoadCreatureBitmap(this);                
            }     
        }
        private void openItems(string filename)
        {
            if (File.Exists(filename))
            {
                mod.moduleItemsList.Clear();
                //mod.moduleItemsList = loadItemsFile(filename);
            }
            else
            {
                MessageBox.Show("Couldn't find items.json file. Will create a new one upon saving module.");
            }
            frmBlueprints.UpdateTreeViewItems();
            loadItemSprites();
        }
        private void loadItemSprites()
        {
            foreach (Item it in mod.moduleItemsList)
            {
                it.LoadItemBitmap(this);
            }
        }
        private void openProps(string filename)
        {
            if (File.Exists(filename))
            {
                mod.modulePropsList.Clear();
                //mod.modulePropsList = loadPropsFile(filename);
            }
            else
            {
                MessageBox.Show("Couldn't find props.json file. Will create a new one upon saving module.");
            }
            frmBlueprints.UpdateTreeViewProps();
            loadPropSprites();
        }
        private void loadPropSprites()
        {
            foreach (Prop prp in mod.modulePropsList)
            {
                prp.LoadPropBitmap(this);
            }
        }
        private void openShops(string filename)
        {
            if (File.Exists(filename))
            {
                mod.moduleShopsList.Clear();
                //mod.moduleShopsList = loadShopsFile(filename);
            }
            else
            {
                MessageBox.Show("Couldn't find shops.json file. Will create a new one upon saving module.");
            }
        }
        private void openContainers(string filename)
        {
            if (File.Exists(filename))
            {
                mod.moduleContainersList.Clear();
                //mod.moduleContainersList = loadContainersFile(filename);                
            }
            else
            {
                MessageBox.Show("Couldn't find containers.json file. Will create a new one upon saving module.");
            }
            frmContainers.refreshListBoxContainers();
        }
        private void openEncounters(string filename)
        {            
            if (File.Exists(filename))
            {
                mod.moduleEncountersList.Clear();
                //mod.moduleEncountersList = loadEncountersFile(filename);                
            }
            else
            {
                MessageBox.Show("Couldn't find encounters.json file. Will create a new one upon saving module.");
            }
            frmEncounters.refreshListBoxEncounters();
        }
        private void openJournal(string filename)
        {
            if (File.Exists(filename))
            {
                mod.moduleJournal.Clear();
                //mod.moduleJournal = loadJournalFile(filename);
            }
            else
            {
                MessageBox.Show("Couldn't find journal.json file. Will create a new one upon saving module.");
            }
        }
        private void openEffects(string filename)
        {
            if (File.Exists(filename))
            {
                mod.moduleEffectsList.Clear();
                //mod.moduleEffectsList = loadEffectsFile(filename);
            }
            else
            {
                MessageBox.Show("Couldn't find effects.json file. Will create a new one upon saving module.");
            }            
        }
        private void openPlayerClasses(string filename)
        {
            if (File.Exists(filename))
            {
                mod.modulePlayerClassList.Clear();
                //mod.modulePlayerClassList = loadPlayerClassesFile(filename);
            }
            else
            {
                MessageBox.Show("Couldn't find playerClasses.json file. Will create a new one upon saving module.");
            }
        }
        private void openRaces(string filename)
        {
            if (File.Exists(filename))
            {
                mod.moduleRacesList.Clear();
                //mod.moduleRacesList = loadRacesFile(filename);
            }
            else
            {
                MessageBox.Show("Couldn't find races.json file. Will create a new one upon saving module.");
            }
        }
        private void openSpells(string filename)
        {
            if (File.Exists(filename))
            {
                mod.moduleSpellsList.Clear();
                //mod.moduleSpellsList = loadSpellsFile(filename);
            }
            else
            {
                MessageBox.Show("Couldn't find spells.json file. Will create a new one upon saving module.");
            }            
        }
        private void openTraits(string filename)
        {
            if (File.Exists(filename))
            {
                mod.moduleTraitsList.Clear();
                //mod.moduleTraitsList = loadTraitsFile(filename);
            }
            else
            {
                MessageBox.Show("Couldn't find traits.json file. Will create a new one upon saving module.");
            }            
        }
                
        private void openFiles()
        {
            openFileDialog1.InitialDirectory = Environment.CurrentDirectory + "\\modules";
            //Empty the FileName text box of the dialog
            openFileDialog1.FileName = String.Empty;
            openFileDialog1.Filter = "Module files (*.mod)|*.mod|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;

            DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {
                string filename = Path.GetFullPath(openFileDialog1.FileName);
                string directory = Path.GetDirectoryName(openFileDialog1.FileName);
                openModule(filename);
                //openCreatures(directory + "\\data\\creatures.json");
                frmBlueprints.UpdateTreeViewCreatures();
                loadCreatureSprites();
                //openItems(directory + "\\data\\items.json");
                frmBlueprints.UpdateTreeViewItems();
                loadItemSprites();
                //openContainers(directory + "\\data\\containers.json");
                frmContainers.refreshListBoxContainers();
                //openShops(directory + "\\data\\shops.json");
                //openEncounters(directory + "\\data\\encounters.json");
                frmEncounters.refreshListBoxEncounters();
                //openProps(directory + "\\data\\props.json");
                frmBlueprints.UpdateTreeViewProps();
                loadPropSprites();
                //openJournal(directory + "\\data\\journal.json");
                //openPlayerClasses(directory + "\\data\\playerClasses.json");
                //openRaces(directory + "\\data\\races.json");
                //openSpells(directory + "\\data\\spells.json");
                //openTraits(directory + "\\data\\traits.json");
                //openEffects(directory + "\\data\\effects.json");
                refreshDropDownLists();
                this.Text = "IceBlink 2 Mini Toolset - " + mod.moduleLabelName;
            }
        }
        public void refreshDropDownLists()
        {
            fillScriptList();
            loadSpriteDropdownList();
            loadSoundDropdownList();
            //loadLogicTreeDropdownList();
            loadIBScriptDropdownList();
            //loadMusicDropdownList();
            loadConversationDropdownList();
            loadEncounterDropdownList();
            loadPlayerClassesTagsList();
            loadRacesTagsList();
            loadSpellTagsList();
            loadEffectTagsList();
        }
        public void loadSpriteDropdownList()
        {
            DropdownStringLists.spriteStringList = new List<string>();
            DropdownStringLists.spriteStringList.Add("none");
            string jobDir = "";
            jobDir = this._mainDirectory + "\\override";
            foreach (string f in Directory.GetFiles(jobDir, "fx_*.png"))
            {
                string filename = Path.GetFileNameWithoutExtension(f);
                DropdownStringLists.spriteStringList.Add(filename);
            }
            jobDir = this._mainDirectory + "\\default\\NewModule\\graphics";
            foreach (string f in Directory.GetFiles(jobDir, "fx_*.png"))
            {
                string filename = Path.GetFileNameWithoutExtension(f);
                DropdownStringLists.spriteStringList.Add(filename);
            }
        }
        public void loadSoundDropdownList()
        {
            DropdownStringLists.soundStringList = new List<string>();
            DropdownStringLists.soundStringList.Add("none");
            string jobDir = "";
            jobDir = this._mainDirectory + "\\override";
            foreach (string f in Directory.GetFiles(jobDir, "*.wav", SearchOption.AllDirectories))
            {
                string filename = Path.GetFileNameWithoutExtension(f);
                DropdownStringLists.soundStringList.Add(filename);
            }
            jobDir = this._mainDirectory + "\\default\\NewModule\\sounds";
            foreach (string f in Directory.GetFiles(jobDir, "*.wav", SearchOption.AllDirectories))
            {
                string filename = Path.GetFileNameWithoutExtension(f);
                DropdownStringLists.soundStringList.Add(filename);
            }
        }
        /*public void loadLogicTreeDropdownList()
        {
            DropdownStringLists.logicTreeStringList = new List<string>();
            DropdownStringLists.logicTreeStringList.Add("none");
            string jobDir = "";
            jobDir = this._mainDirectory + "\\modules\\" + mod.moduleName + "\\logictree";
            foreach (string f in Directory.GetFiles(jobDir, "*.*", SearchOption.AllDirectories))
            {
                string filename = Path.GetFileNameWithoutExtension(f);
                DropdownStringLists.logicTreeStringList.Add(filename);
            }
        }*/
        public void loadIBScriptDropdownList()
        {
            DropdownStringLists.ibScriptStringList = new List<string>();
            DropdownStringLists.ibScriptStringList.Add("none");
            foreach (IBScript scr in this.mod.moduleIBScriptList)
            {
                DropdownStringLists.ibScriptStringList.Add(scr.scriptName);
            }
        }
        public void loadConversationDropdownList()
        {
            DropdownStringLists.conversationTypeStringList = new List<string>();
            DropdownStringLists.conversationTypeStringList.Add("none");
            foreach (Convo conv in mod.moduleConvoList)
            {
                DropdownStringLists.conversationTypeStringList.Add(conv.ConvoFileName);
            }            
        }
        public void loadEncounterDropdownList()
        {
            DropdownStringLists.encounterTypeStringList = new List<string>();
            DropdownStringLists.encounterTypeStringList.Add("none");
            foreach (Encounter enc in this.mod.moduleEncountersList)
            {
                DropdownStringLists.encounterTypeStringList.Add(enc.encounterName);
            }
        }
        /*public void loadMusicDropdownList()
        {
            DropdownStringLists.musicStringList = new List<string>();
            DropdownStringLists.musicStringList.Add("none");
            string jobDir = "";
            jobDir = this._mainDirectory + "\\modules\\" + mod.moduleName + "\\music";
            foreach (string f in Directory.GetFiles(jobDir, "*.mp3", SearchOption.AllDirectories))
            {
                string filename = Path.GetFileNameWithoutExtension(f);
                DropdownStringLists.musicStringList.Add(filename);
            }
        }*/
        public void loadPlayerClassesTagsList()
        {
            DropdownStringLists.playerClassTagsTypeStringList = new List<string>();
            foreach (PlayerClass pcl in this.mod.modulePlayerClassList)
            {
                DropdownStringLists.playerClassTagsTypeStringList.Add(pcl.tag);
            }
        }
        public void loadRacesTagsList()
        {
            DropdownStringLists.raceTagsTypeStringList = new List<string>();
            foreach (Race rc in this.mod.moduleRacesList)
            {
                DropdownStringLists.raceTagsTypeStringList.Add(rc.tag);
            }
        }
        public void loadSpellTagsList()
        {
            DropdownStringLists.spellTagsTypeStringList = new List<string>();
            DropdownStringLists.spellTagsTypeStringList.Add("none");
            foreach (Spell sp in this.mod.moduleSpellsList)
            {
                DropdownStringLists.spellTagsTypeStringList.Add(sp.tag);
            }
        }
        public void loadEffectTagsList()
        {
            DropdownStringLists.effectTagsTypeStringList = new List<string>();
            DropdownStringLists.effectTagsTypeStringList.Add("none");
            foreach (Effect ef in this.mod.moduleEffectsList)
            {
                DropdownStringLists.effectTagsTypeStringList.Add(ef.tag);
            }
        }
        
        /*public class EffectTagTypeConverter : StringConverter
        {
            public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
            {
                //true means show a combobox
                return true;
            }
            public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
            {
                //true will limit to list. false will show the list, but allow free-form entry
                return true;
            }
            public override System.ComponentModel.TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
            {
                return new StandardValuesCollection(DropdownStringLists.effectTagsTypeStringList);
            }
        }*/
                
        private void fillScriptList()
        {
            scriptList.Clear();
            string jobDir = this._mainDirectory + "\\default\\NewModule\\scripts";            
            foreach (string f in Directory.GetFiles(jobDir, "*.cs"))
            {
                string filename = Path.GetFileName(f);
                scriptList.Add(filename);
            }            
        }
        private void saveAsTemp()
        {
            lastModuleFullPath = _mainDirectory + "\\default\\NewModule";
            mod.moduleName = "temp01";
            string directory = _mainDirectory + "\\modules\\" + mod.moduleName;
            try
            {
                if (!Directory.Exists(directory)) // if folder does not exist, create it and copy contents from previous folder
                {
                    createDirectory(directory);
                    createDirectory(directory + "//data");
                    DirectoryCopy(lastModuleFullPath, directory, true); // needs to copy contents from previous folder into new folder and overwrite files with new updates
                    createFiles(directory);
                }
                else
                {
                    createDirectory(directory + "//data");
                    createFiles(directory); // if folder exists, then overwrite all files in folder
                }
                //MessageBox.Show("temp01 module saved");
                refreshDropDownLists();
            }
            catch (Exception e)
            {
                MessageBox.Show("failed to save temp01 module: " + e.ToString());
            }
        }
        private void saveFiles()
        {
            if ((mod.startingArea == null) || (mod.startingArea == ""))
            {
                MessageBox.Show("Starting area was not detected, please type in the starting area's name in module properties (Edit/Modules Properties). Your module will not work without a starting area defined.");
                //return;
            }
            if ((mod.moduleName.Length == 0) || (mod.moduleName == "NewModule"))
            {
                saveAsFiles();
                return;
            }
            string file = _mainDirectory + "\\modules\\" + mod.moduleName + ".mod";
            try
            {
                createFiles(file);                
                MessageBox.Show("Moduled saved");
                refreshDropDownLists();
                this.Text = "IceBlinkRPG Toolset - " + mod.moduleLabelName;
            }
            catch (Exception e)
            {
                MessageBox.Show("failed to save module: " + e.ToString());
            }            
            /*string directory = _mainDirectory + "\\modules\\" + mod.moduleName;
            try
            {
                if (!Directory.Exists(directory)) // if folder does not exist, create it and copy contents from previous folder
                {
                    createDirectory(directory);
                    DirectoryCopy(lastModuleFullPath, directory, true); // needs to copy contents from previous folder into new folder and overwrite files with new updates
                    createFiles(directory);
                }
                else
                {
                    createFiles(directory); // if folder exists, then overwrite all files in folder
                }
                MessageBox.Show("Moduled saved");
                refreshDropDownLists();
                this.Text = "IceBlinkRPG Toolset - " + mod.moduleLabelName;
            }
            catch (Exception e)
            {
                MessageBox.Show("failed to save module: " + e.ToString());
            }*/
        }
        private void saveAsFiles()
        {
            ModuleNameDialog mnd = new ModuleNameDialog();
            mnd.ShowDialog();
            mod.moduleName = mnd.ModText;
            saveFiles();
        }
        private void incrementalSave() //incremental save option
        {
            if ((mod.startingArea == null) || (mod.startingArea == ""))
            {
                MessageBox.Show("Starting area was not detected, please type in the starting area's name in module properties (Edit/Modules Properties). Your module will not work without a starting area defined.");
                //return;
            }
            else
            {
                // save a backup with an incremental file name
                //string lastDir = mod.moduleName;
                string workingDir = _mainDirectory + "\\modules";
                string backupDir = _mainDirectory + "\\module_backups";
                string fileName = mod.moduleName;
                string incrementFileName = "";
                for (int i = 0; i < 999; i++) // add an incremental save option (uses directoryName plus number for folder name)
                {
                    if (!File.Exists(backupDir + "\\" + fileName + "(" + i.ToString() + ").mod"))
                    {
                        incrementFileName = fileName + "(" + i.ToString() + ").mod";
                        //createDirectory(backupDir + "\\" + incrementFileName);
                        //DirectoryCopy(workingDir + "\\" + lastDir, backupDir + "\\" + incrementFileName, true); // needs to copy contents from previous folder into new folder and overwrite files with new updates
                        //DirectoryInfo dir = Directory.CreateDirectory(workingDir + "\\" + incrementDirName);
                        createFiles(backupDir + "\\" + incrementFileName);
                        break;
                    }
                    else
                    {
                        //lastDir = workingDir + "\\" + fileName + "(" + i.ToString() + ")";
                    }
                }
                MessageBox.Show("Moduled backup " + incrementFileName + " was saved");

                // save over original module
                string file = _mainDirectory + "\\modules\\" + mod.moduleName + ".mod";
                try
                {                    
                    createFiles(file);
                    MessageBox.Show("Moduled saved");
                }
                catch
                {
                    MessageBox.Show("failed to save module");
                }
            }
        }        
        private void createFiles(string fullPathFilename)
        {
            try
            {
                //clean up the spellsAllowed and traitsAllowed
                foreach (PlayerClass pcls in mod.modulePlayerClassList)
                {
                    for (int i = pcls.spellsAllowed.Count - 1; i >= 0; i--)
                    {
                        if (!pcls.spellsAllowed[i].allow)
                        {
                            pcls.spellsAllowed.RemoveAt(i);
                        }
                    }
                    for (int i = pcls.traitsAllowed.Count - 1; i >= 0; i--)
                    {
                        if (!pcls.traitsAllowed[i].allow)
                        {
                            pcls.traitsAllowed.RemoveAt(i);
                        }
                    }
                }
                //mod.VersionIB = game.IBVersion;
                //mod.saveModuleFile(fullPathFilename, tsBtnJSON.Checked);
                saveModule(mod, fullPathFilename);
                //save areas
                saveAreas(mod, fullPathFilename);
                //save encounters
                saveEncounters(mod, fullPathFilename);
                //save convos
                saveConvos(mod, fullPathFilename);
                //save images
                saveImages(mod, fullPathFilename);
                //saveCreaturesFile(fullPathDirectory + "\\data\\creatures.json");
                //saveItemsFile(fullPathDirectory + "\\data\\items.json");
                //saveContainersFile(fullPathDirectory + "\\data\\containers.json");
                //saveShopsFile(fullPathDirectory + "\\data\\shops.json");
                //saveEncountersFile(fullPathDirectory + "\\data\\encounters.json");
                //savePropsFile(fullPathDirectory + "\\data\\props.json");
                //saveJournalFile(fullPathDirectory + "\\data\\journal.json");
                //savePlayerClassesFile(fullPathDirectory + "\\data\\playerClasses.json");
                //saveRacesFile(fullPathDirectory + "\\data\\races.json");
                //saveSpellsFile(fullPathDirectory + "\\data\\spells.json");
                //saveTraitsFile(fullPathDirectory + "\\data\\traits.json");
                //saveEffectsFile(fullPathDirectory + "\\data\\effects.json");
                // save convos that are open
                /*foreach (Convo convo in openConvosList)
                {
                    try
                    {
                        convo.SaveContentConversation(this._mainDirectory + "\\modules\\" + mod.moduleName + "\\dialog", convo.ConvoFileName + ".json");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: Could not save Convo file to disk. Original error: " + ex.Message);
                    }
                }*/
                // save logic trees that are open
                /*//REMOVEforeach (LogicTree logtre in openLogicTreesList)
                {
                    try
                    {
                        logtre.SaveLogicTree(this._mainDirectory + "\\modules\\" + mod.moduleName + "\\logictree", logtre.Filename + ".json");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: Could not save Logic Tree file to disk. Original error: " + ex.Message);
                    }
                }*/
                // save areas that are open
                /*foreach (Area a in openAreasList)
                {
                    try
                    {
                        a.saveAreaFile(this._mainDirectory + "\\modules\\" + mod.moduleName + "\\areas\\" + a.Filename + ".lvl");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: Could not save area file to disk. Original error: " + ex.Message);
                    }
                }*/
            }
            catch { MessageBox.Show("failed to createFiles"); }
        }
        private void saveModule(Module mod, string moduleFullPathFilename)
        {
            File.WriteAllText(moduleFullPathFilename, "MODULE" + Environment.NewLine);
            string output = JsonConvert.SerializeObject(mod, Formatting.None);
            File.AppendAllText(moduleFullPathFilename, output + Environment.NewLine);            
        }
        private void saveAreas(Module mod, string moduleFullPathFilename)
        {
            File.AppendAllText(moduleFullPathFilename, "AREAS" + Environment.NewLine);
            foreach (Area a in mod.moduleAreasObjects)
            {
                string output = JsonConvert.SerializeObject(a, Formatting.None);
                File.AppendAllText(moduleFullPathFilename, output + Environment.NewLine);
            }
        }
        private void saveEncounters(Module mod, string moduleFullPathFilename)
        {
            File.AppendAllText(moduleFullPathFilename, "ENCOUNTERS" + Environment.NewLine);
            foreach (Encounter enc in mod.moduleEncountersList)
            {
                string output = JsonConvert.SerializeObject(enc, Formatting.None);
                File.AppendAllText(moduleFullPathFilename, output + Environment.NewLine);
            }
        }
        private void saveConvos(Module mod, string moduleFullPathFilename)
        {
            File.AppendAllText(moduleFullPathFilename, "CONVOS" + Environment.NewLine);
            foreach (Convo c in mod.moduleConvoList)
            {
                string output = JsonConvert.SerializeObject(c, Formatting.None);
                File.AppendAllText(moduleFullPathFilename, output + Environment.NewLine);
            }
        }
        private void saveImages(Module mod, string moduleFullPathFilename)
        {
            //file listImages
            /*bsc.listImages.Clear();
            ImageData imd = new ImageData();
            foreach (string str in Directory.GetFiles(_mainDirectory + "\\override"))
            {
                if ((str.EndsWith(".png")) || (str.EndsWith(".PNG")) || (str.EndsWith(".jpg")) || (str.EndsWith(".JPG")))
                {
                    string filenameNoExt = Path.GetFileNameWithoutExtension(str);
                    imd = bsc.ConvertBitmapToImageData(filenameNoExt, str);
                    bsc.listImages.Add(imd);
                }
            }*/
            //save them out
            File.AppendAllText(moduleFullPathFilename, "IMAGES" + Environment.NewLine);
            foreach (ImageData imd in mod.moduleImageDataList)
            {
                string output = JsonConvert.SerializeObject(imd, Formatting.None);
                File.AppendAllText(moduleFullPathFilename, output + Environment.NewLine);
            }
        }

        /*private void newModule()
        {
            openModule(_mainDirectory + "\\default\\NewModule\\NewModule.mod");
            openCreatures(_mainDirectory + "\\default\\NewModule\\data\\creatures.json");
            openItems(_mainDirectory + "\\default\\NewModule\\data\\items.json");
        }*/
        private void createDirectory(string fullPath)
        {
            try
            {
                DirectoryInfo dir = Directory.CreateDirectory(fullPath);
            }
            catch { MessageBox.Show("failed to create the directory: " + fullPath); }
        }
        private void DirectoryCopy(string sourceDirPath, string destDirPath, bool copySubDirs)
        {
            try
            {
                //string _currentDir = System.IO.Directory.GetCurrentDirectory();
                DirectoryInfo dir = new DirectoryInfo(sourceDirPath);
                DirectoryInfo[] dirs = dir.GetDirectories();

                FileInfo[] files = dir.GetFiles(); // Get the file contents of the directory to copy.
                foreach (FileInfo file in files)
                {
                    try
                    {
                        if (file.Name != "NewModule.mod")
                        {
                            string temppath = Path.Combine(destDirPath, file.Name); // Create the path to the new copy of the file.
                            file.CopyTo(temppath, false); // Copy the file.
                        }
                    }
                    catch (Exception ex)
                    { MessageBox.Show("failed to copy file: " + ex.ToString()); }
                }

                if (copySubDirs) // If copySubDirs is true, copy the subdirectories.
                {
                    foreach (DirectoryInfo subdir in dirs)
                    {
                        try
                        {
                            string temppath = Path.Combine(destDirPath, subdir.Name); // Create the subdirectory.
                            createDirectory(temppath);
                            DirectoryCopy(subdir.FullName, temppath, copySubDirs); // Copy the subdirectories.
                        }
                        catch (Exception ex)
                        { MessageBox.Show("failed to copy sub folders: " + ex.ToString()); }
                    }
                }
            }
            catch (Exception ex) 
            { MessageBox.Show("failed to copy the directory: " + ex.ToString()); }
        }
        private void refreshIcon()
        {
            if (frmBlueprints.tabCreatureItem.SelectedIndex == 0) //creature
            {
                refreshIconCreatures();
            }
            else if (frmBlueprints.tabCreatureItem.SelectedIndex == 1) //item
            {
                refreshIconItems();
            }
            else //prop
            {
                refreshIconProps();
            }
        }
        public void logText(string text)
        {
            frmLog.rtxtLog.AppendText(text);
            frmLog.rtxtLog.ScrollToCaret();
        }
        #endregion

        #region Event Handlers
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFiles();
        }
        private void openToolStripButton_Click(object sender, EventArgs e)
        {
            openFiles();
        }
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFiles();
        }
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveAsFiles();
        }
        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            saveFiles();
        }
        private void tsbSaveIncremental_Click(object sender, EventArgs e)
        {
            incrementalSave();
        }
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // maybe should ask to save first if any changes have been made
            this.Close();
        }
        private void propertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmIceBlinkProperties.Show(dockPanel1);
        }
        private void spriteIconToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmIconSprite.Show(dockPanel1);
        }
        private void blueprintsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmBlueprints.Show(dockPanel1);
        }
        private void triggerEventsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmTriggerEvents.Show(dockPanel1);
        }
        private void areasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAreas.Show(dockPanel1);
        }
        /*//REMOVEprivate void logicTreesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmLogicTree.Show(dockPanel1);
        }*/
        private void iBScriptsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmIBScript.Show(dockPanel1);
        }
        private void conversationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmConversations.Show(dockPanel1);
        }
        private void encountersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmEncounters.Show(dockPanel1);
        }
        private void containersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmContainers.Show(dockPanel1);
        }
        private void logToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmLog.Show(dockPanel1);
        }
        private void modulePropertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //frmIceBlinkProperties.propertyGrid1.SelectedObject = mod;
            ModuleEditor modEdit = new ModuleEditor(mod, this);
            modEdit.ShowDialog();
        }
        private void journalEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            JournalEditor journalEdit = new JournalEditor(mod, this);
            journalEdit.ShowDialog();
        }
        private void shopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShopEditor shopEdit = new ShopEditor(mod, this);
            shopEdit.ShowDialog();
        }
        private void playerClassEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PlayerClassEditor playerClassEdit = new PlayerClassEditor(mod, this);
            playerClassEdit.ShowDialog();
        }
        private void raceEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RacesEditor raceEdit = new RacesEditor(mod, this);
            raceEdit.ShowDialog();
        }
        private void spellEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SpellEditor sEdit = new SpellEditor(mod, this);
            sEdit.ShowDialog();
        }
        private void traitEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TraitEditor tEdit = new TraitEditor(mod, this);
            tEdit.ShowDialog();
        }        
        private void effectEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EffectEditor eEdit = new EffectEditor(mod, this);
            eEdit.ShowDialog();
        }
        private void playerEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PlayerEditor pcEdit = new PlayerEditor(this);
            pcEdit.ShowDialog();
        }
        
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(versionMessage);
            /*MessageBox.Show(test.EventTag1.TagOrFilename + " " +
                test.EventTag1.Parm1 + " " +
                test.EventTag1.Parm2 + " " +
                test.EventTag1.Parm3 + " " +
                test.EventTag1.Parm4 + " " +
                test.EventTag1.TransPoint.X.ToString() + " " +
                test.EventTag1.TransPoint.Y.ToString());*/
        }
        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("not implemented yet");
        }
        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("not implemented yet");
        }
        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("not implemented yet");
        }
        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("not implemented yet");
        }
        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("not implemented yet");
        }
        private void tabCreatureItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            //MessageBox.Show("changed tab");
            if (frmBlueprints.tabCreatureItem.SelectedIndex == 0)
            {
                //show sprite for currently selected creature
                refreshIcon();
            }
            else if (frmBlueprints.tabCreatureItem.SelectedIndex == 1) //item
            {
                //show icon for currently selected item
                refreshIcon();
            }
            else //prop
            {
                //show sprite for currently selected prop
                refreshIcon();
            }
        }
        private void btnSelectIcon_Click(object sender, EventArgs e)
        {
            if (frmBlueprints.tabCreatureItem.SelectedIndex == 0) //creature
            {
                LoadCreatureSprite();
            }
            else if (frmBlueprints.tabCreatureItem.SelectedIndex == 1) //item
            {
                LoadItemIcon();
            }
            else //prop
            {
                LoadPropSprite();
            }
        }
        #endregion        

        public string GetImageFilename(string filter)
        {
            /*if (mod.moduleName != "NewModule")
            {
                openFileDialog2.InitialDirectory = this._mainDirectory + "\\modules\\" + mod.moduleName + "\\graphics";
            }
            else
            {
                openFileDialog2.InitialDirectory = this._mainDirectory + "\\default\\NewModule\\graphics";
            }
            //Empty the FileName text box of the dialog
            openFileDialog2.FileName = filter + "*";
            openFileDialog2.Filter = "Image (*.png)|*.png|All Files (*.*)|*.*";
            openFileDialog2.FilterIndex = 1;

            DialogResult result = openFileDialog2.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {
                return Path.GetFileNameWithoutExtension(openFileDialog2.FileName);
            }*/
            //return "none";

            using (var sel = new ImageSelector(mod, this, filter))
            {                
                var result = sel.ShowDialog();
                if (result == DialogResult.OK) // Test result.
                {
                    return returnImageFilenameFromImageSelector;
                }
                else
                {
                    return "none";
                }
            }
        }

        public System.Drawing.Bitmap LoadBitmapGDI(string filename)
        {
            System.Drawing.Bitmap bm = null;
            try
            {
                if (File.Exists(_mainDirectory + "\\override\\" + filename + ".png"))
                {
                    bm = new Bitmap(_mainDirectory + "\\override\\" + filename + ".png");
                }
                else if (File.Exists(_mainDirectory + "\\override\\" + filename + ".jpg"))
                {
                    bm = new Bitmap(_mainDirectory + "\\override\\" + filename + ".jpg");
                }
                else if (File.Exists(_mainDirectory + "\\override\\" + filename))
                {
                    bm = new Bitmap(_mainDirectory + "\\override\\" + filename);
                }

                else if (File.Exists(_mainDirectory + "\\default\\NewModule\\graphics\\" + filename + ".png"))
                {
                    bm = new Bitmap(_mainDirectory + "\\default\\NewModule\\graphics\\" + filename + ".png");
                }
                else if (File.Exists(_mainDirectory + "\\default\\NewModule\\graphics\\" + filename + ".jpg"))
                {
                    bm = new Bitmap(_mainDirectory + "\\default\\NewModule\\graphics\\" + filename + ".jpg");
                }
                else if (File.Exists(_mainDirectory + "\\default\\NewModule\\graphics\\" + filename))
                {
                    bm = new Bitmap(_mainDirectory + "\\default\\NewModule\\graphics\\" + filename);
                }                
                else if (File.Exists(_mainDirectory + "\\default\\NewModule\\tiles\\" + filename + ".png"))
                {
                    bm = new Bitmap(_mainDirectory + "\\default\\NewModule\\tiles\\" + filename + ".png");
                }
                else if (File.Exists(_mainDirectory + "\\default\\NewModule\\tiles\\" + filename + ".jpg"))
                {
                    bm = new Bitmap(_mainDirectory + "\\default\\NewModule\\tiles\\" + filename + ".jpg");
                }
                else if (File.Exists(_mainDirectory + "\\default\\NewModule\\tiles\\" + filename))
                {
                    bm = new Bitmap(_mainDirectory + "\\default\\NewModule\\tiles\\" + filename);
                }
                else if (File.Exists(_mainDirectory + "\\default\\NewModule\\portraits\\" + filename + ".png"))
                {
                    bm = new Bitmap(_mainDirectory + "\\default\\NewModule\\portraits\\" + filename + ".png");
                }
                else if (File.Exists(_mainDirectory + "\\default\\NewModule\\portraits\\" + filename + ".PNG"))
                {
                    bm = new Bitmap(_mainDirectory + "\\default\\NewModule\\portraits\\" + filename + ".PNG");
                }
                else if (File.Exists(_mainDirectory + "\\default\\NewModule\\portraits\\" + filename + ".jpg"))
                {
                    bm = new Bitmap(_mainDirectory + "\\default\\NewModule\\portraits\\" + filename + ".jpg");
                }
                else if (File.Exists(_mainDirectory + "\\default\\NewModule\\portraits\\" + filename))
                {
                    bm = new Bitmap(_mainDirectory + "\\default\\NewModule\\portraits\\" + filename);
                }
                else if (File.Exists(_mainDirectory + "\\default\\NewModule\\ui\\" + filename + ".png"))
                {
                    bm = new Bitmap(_mainDirectory + "\\default\\NewModule\\ui\\" + filename + ".png");
                }
                else if (File.Exists(_mainDirectory + "\\default\\NewModule\\ui\\" + filename + ".jpg"))
                {
                    bm = new Bitmap(_mainDirectory + "\\default\\NewModule\\ui\\" + filename + ".jpg");
                }
                else if (File.Exists(_mainDirectory + "\\default\\NewModule\\ui\\" + filename))
                {
                    bm = new Bitmap(_mainDirectory + "\\default\\NewModule\\ui\\" + filename);
                }

                else
                {
                    bm = new Bitmap(_mainDirectory + "\\default\\NewModule\\graphics\\missingtexture.png");
                }
            }
            catch (Exception ex)
            {
                errorLog(ex.ToString());
                if (bm == null)
                {
                    bm = new Bitmap(_mainDirectory + "\\default\\NewModule\\graphics\\missingtexture.png");
                    return bm;
                }
            }
            return bm;
        }

        #region Creature Stuff
        public void refreshIconCreatures()
        {
            if (frmBlueprints.tvCreatures.SelectedNode != null)
            {
                try
                {
                    string _nodeTag = frmBlueprints.tvCreatures.SelectedNode.Name;
                    iconBitmap = (Bitmap)mod.moduleCreaturesList[frmBlueprints.GetCreatureIndex(_nodeTag)].creatureIconBitmap.Clone();
                    frmIconSprite.pbIcon.BackgroundImage = (Image)iconBitmap;
                    if (iconBitmap == null) { MessageBox.Show("returned a null icon bitmap"); }
                }
                catch { }
            }
        }
        public void LoadCreatureSprite()
        {
            string _nodeTag = frmBlueprints.tvCreatures.SelectedNode.Name;
            string name = GetImageFilename("tkn_");
            if (name != "none")
            {
                mod.moduleCreaturesList[frmBlueprints.GetCreatureIndex(_nodeTag)].cr_tokenFilename = name;
            }
            mod.moduleCreaturesList[frmBlueprints.GetCreatureIndex(_nodeTag)].LoadCreatureBitmap(this);
            refreshIconCreatures();
            /*using (var sel = new SpriteSelector(game, mod))
            {
                var result = sel.ShowDialog();
                if (result == DialogResult.OK) // Test result.
                {
                    string _nodeTag = lastSelectedCreatureNodeName;
                    string filename = game.returnSpriteFilename;
                    //IBMessageBox.Show(game, "filename selected = " + filename);
                    try
                    {
                        Creature crt = creaturesList.creatures[frmBlueprints.GetCreatureIndex(_nodeTag)];
                        crt.SpriteFilename = filename;
                        //creaturesList.creatures[frmBlueprints.GetCreatureIndex(_nodeTag)].LoadCharacterSprite(directory, filename);
                        crt.LoadSpriteStuff(_mainDirectory + "\\modules\\" + mod.ModuleFolderName);

                        //thisPC.SpriteFilename = filename;
                        //thisPC.LoadSpriteStuff(ccr_game.mainDirectory + "\\modules\\" + ccr_game.module.ModuleFolderName);
                        iconBitmap = (Bitmap)crt.CharSprite.Image.Clone();
                        //iconGameMap = new Bitmap(ccr_game.mainDirectory + "\\modules\\" + ccr_game.module.ModuleFolderName + "\\graphics\\sprites\\tokens\\player\\" + thisPC.CharSprite.SpriteSheetFilename);
                        frmIconSprite.pbIcon.BackgroundImage = (Image)iconBitmap;
                        if (iconBitmap == null) { MessageBox.Show("returned a null icon bitmap"); }
                        //iconBitmap.Image = (Image)iconGameMap;

                        //if (iconGameMap == null)
                        //{
                        //    IBMessageBox.Show(ccr_game, "returned a null icon bitmap");
                        //}
                    }
                    catch
                    {
                        MessageBox.Show("failed to load image...make sure to select a creature from the list first");
                    }
                }
            }
            loadCreatureSprites();      
            */
        }
        #endregion

        #region Item Stuff
        public void refreshIconItems()
        {
            if (frmBlueprints.tvItems.SelectedNode != null)
            {
                try
                {
                    string _nodeTag = frmBlueprints.tvItems.SelectedNode.Name;
                    iconBitmap = (Bitmap)mod.moduleItemsList[frmBlueprints.GetItemIndex(_nodeTag)].itemIconBitmap.Clone();
                    frmIconSprite.pbIcon.BackgroundImage = (Image)iconBitmap;
                    if (iconBitmap == null) { MessageBox.Show("returned a null icon bitmap"); }
                }
                catch { }
            }
        }
        public void LoadItemIcon()
        {
            string _nodeTag = frmBlueprints.tvItems.SelectedNode.Name;
            string name = GetImageFilename("it_");
            if (name != "none")
            {
                mod.moduleItemsList[frmBlueprints.GetItemIndex(_nodeTag)].itemImage = name;
            }
            mod.moduleItemsList[frmBlueprints.GetItemIndex(_nodeTag)].LoadItemBitmap(this);
            refreshIconItems();
            /*using (var sel = new ItemImageSelector(game, mod))
            {
                var result = sel.ShowDialog();
                if (result == DialogResult.OK) // Test result.
                {
                    string filename = game.returnSpriteFilename;
                    string _nodeTag = lastSelectedItemNodeName;
                    try
                    {
                        itemsList.itemsList[frmBlueprints.GetItemIndex(_nodeTag)].ItemIconFilename = filename;
                        if (File.Exists(_mainDirectory + "\\modules\\" + mod.ModuleFolderName + "\\graphics\\sprites\\items\\" + filename))
                        {
                            iconBitmap = new Bitmap(_mainDirectory + "\\modules\\" + mod.ModuleFolderName + "\\graphics\\sprites\\items\\" + filename);
                        }
                        else if (File.Exists(_mainDirectory + "\\modules\\" + mod.ModuleFolderName + "\\graphics\\sprites\\" + filename))
                        {
                            iconBitmap = new Bitmap(_mainDirectory + "\\modules\\" + mod.ModuleFolderName + "\\graphics\\sprites\\" + filename);
                        }
                        else if (File.Exists(_mainDirectory + "\\data\\graphics\\sprites\\items\\" + filename))
                        {
                            iconBitmap = new Bitmap(_mainDirectory + "\\data\\graphics\\sprites\\items\\" + filename);
                        }
                        else if (File.Exists(_mainDirectory + "\\data\\graphics\\sprites\\" + filename))
                        {
                            iconBitmap = new Bitmap(_mainDirectory + "\\data\\graphics\\sprites\\" + filename);
                        }
                        else
                        {
                            MessageBox.Show("The image selected is not from one of the designated items folder locations...will use blank.png");
                            iconBitmap = new Bitmap(_mainDirectory + "\\data\\graphics\\blank.png");
                        }
                        frmIconSprite.pbIcon.BackgroundImage = (Image)iconBitmap;
                        if (iconBitmap == null) { MessageBox.Show("returned a null icon bitmap"); }
                    }
                    catch
                    {
                        MessageBox.Show("failed to load image...make sure to select an item from the list first");
                    }
                }
            }*/

            
        }
        public ItemRefs createItemRefsFromItem(Item it)
        {
            ItemRefs newIR = new ItemRefs();
            newIR.tag = it.tag + "_" + mod.nextIdNumber;
            newIR.name = it.name;
            newIR.resref = it.resref;
            newIR.quantity = it.quantity;
            newIR.canNotBeUnequipped = it.canNotBeUnequipped;
            return newIR;
        }
        #endregion

        #region Props Stuff
        public void refreshIconProps()
        {
            if (frmBlueprints.tvProps.SelectedNode != null)
            {
                try
                {
                    string _nodeTag = frmBlueprints.tvProps.SelectedNode.Name;
                    iconBitmap = (Bitmap)mod.modulePropsList[frmBlueprints.GetPropIndex(_nodeTag)].propBitmap.Clone();
                    frmIconSprite.pbIcon.BackgroundImage = (Image)iconBitmap;
                    if (iconBitmap == null) { MessageBox.Show("returned a null icon bitmap"); }
                }
                catch { }
            }
        }
        public void LoadPropSprite()
        {
            string _nodeTag = frmBlueprints.tvProps.SelectedNode.Name;
            string name = GetImageFilename("prp_");
            if (name != "none")
            {
                mod.modulePropsList[frmBlueprints.GetPropIndex(_nodeTag)].ImageFileName = name;
            }
            mod.modulePropsList[frmBlueprints.GetPropIndex(_nodeTag)].LoadPropBitmap(this);
            refreshIconProps();
            /*using (var sel = new PropSpriteSelector(game, mod))
            {
                var result = sel.ShowDialog();
                if (result == DialogResult.OK) // Test result.
                {
                    string _nodeTag = lastSelectedPropNodeName;
                    string filename = game.returnSpriteFilename;
                    try
                    {
                        Prop prp = propsList.propsList[frmBlueprints.GetPropIndex(_nodeTag)];
                        prp.PropSpriteFilename = filename;
                        prp.LoadPropSpriteStuffForTS(_mainDirectory + "\\modules\\" + mod.ModuleFolderName);

                        iconBitmap = (Bitmap)prp.PropSprite.Image.Clone();
                        frmIconSprite.pbIcon.BackgroundImage = (Image)iconBitmap;
                        if (iconBitmap == null) { MessageBox.Show("returned a null icon bitmap"); }
                    }
                    catch
                    {
                        MessageBox.Show("failed to load image...make sure to select a prop from the list first");
                    }
                }
            }
            loadPropSprites();
            */
            
        }
        #endregion                                                                        

        #region Save, Load and Get module data files                
        /*public void saveCreaturesFile(string filename)
        {
            string json = JsonConvert.SerializeObject(mod.moduleCreaturesList, Newtonsoft.Json.Formatting.Indented);
            using (StreamWriter sw = new StreamWriter(filename))
            {
                sw.Write(json.ToString());
            }
        }*/
        public List<Creature> loadCreaturesFile(string filename)
        {
            List<Creature> toReturn = null;

            // deserialize JSON directly from a file
            using (StreamReader file = File.OpenText(filename))
            {
                JsonSerializer serializer = new JsonSerializer();
                toReturn = (List<Creature>)serializer.Deserialize(file, typeof(List<Creature>));
            }
            return toReturn;
        }
        public Creature getCreature(string name)
        {
            foreach (Creature cr in mod.moduleCreaturesList)
            {
                if (cr.cr_name == name) return cr;
            }
            return null;
        }
        public Creature getCreatureByTag(string tag)
        {
            foreach (Creature crtag in mod.moduleCreaturesList)
            {
                if (crtag.cr_tag == tag) return crtag;
            }
            return null;
        }
        public Creature getCreatureByResRef(string resref)
        {
            foreach (Creature crt in mod.moduleCreaturesList)
            {
                if (crt.cr_resref == resref) return crt;
            }
            return null;
        }

        /*public void saveItemsFile(string filename)
        {
            string json = JsonConvert.SerializeObject(mod.moduleItemsList, Newtonsoft.Json.Formatting.Indented);
            using (StreamWriter sw = new StreamWriter(filename))
            {
                sw.Write(json.ToString());
            }
        }*/
        public List<Item> loadItemsFile(string filename)
        {
            List<Item> toReturn = null;

            // deserialize JSON directly from a file
            using (StreamReader file = File.OpenText(filename))
            {
                JsonSerializer serializer = new JsonSerializer();
                toReturn = (List<Item>)serializer.Deserialize(file, typeof(List<Item>));
            }
            return toReturn;
        }
        public Item getItem(string name)
        {
            foreach (Item it in mod.moduleItemsList)
            {
                if (it.name == name) return it;
            }
            return null;
        }
        public Item getItemByTag(string tag)
        {
            foreach (Item it in mod.moduleItemsList)
            {
                if (it.tag == tag) return it;
            }
            return null;
        }

        /*public void saveContainersFile(string filename)
        {
            string json = JsonConvert.SerializeObject(mod.moduleContainersList, Newtonsoft.Json.Formatting.Indented);
            using (StreamWriter sw = new StreamWriter(filename))
            {
                sw.Write(json.ToString());
            }
        }*/
        public List<Container> loadContainersFile(string filename)
        {
            List<Container> toReturn = null;

            // deserialize JSON directly from a file
            using (StreamReader file = File.OpenText(filename))
            {
                JsonSerializer serializer = new JsonSerializer();
                toReturn = (List<Container>)serializer.Deserialize(file, typeof(List<Container>));
            }
            return toReturn;
        }
        public Container getContainer(string tag)
        {
            foreach (Container cont in mod.moduleContainersList)
            {
                if (cont.containerTag == tag) return cont;
            }
            return null;
        }

        /*public void saveShopsFile(string filename)
        {
            string json = JsonConvert.SerializeObject(mod.moduleShopsList, Newtonsoft.Json.Formatting.Indented);
            using (StreamWriter sw = new StreamWriter(filename))
            {
                sw.Write(json.ToString());
            }
        }*/
        public List<Shop> loadShopsFile(string filename)
        {
            List<Shop> toReturn = null;

            // deserialize JSON directly from a file
            using (StreamReader file = File.OpenText(filename))
            {
                JsonSerializer serializer = new JsonSerializer();
                toReturn = (List<Shop>)serializer.Deserialize(file, typeof(List<Shop>));
            }
            return toReturn;
        }
        public Shop getShopByTag(string tag)
        {
            foreach (Shop shp in mod.moduleShopsList)
            {
                if (shp.shopTag == tag)
                {
                    return shp;
                }
            }
            return null;
        }

        /*public void saveEncountersFile(string filename)
        {
            string json = JsonConvert.SerializeObject(mod.moduleEncountersList, Newtonsoft.Json.Formatting.Indented);
            using (StreamWriter sw = new StreamWriter(filename))
            {
                sw.Write(json.ToString());
            }
        }*/
        /*public List<Encounter> loadEncountersFile(string filename)
        {
            List<Encounter> toReturn = null;

            // deserialize JSON directly from a file
            using (StreamReader file = File.OpenText(filename))
            {
                JsonSerializer serializer = new JsonSerializer();
                toReturn = (List<Encounter>)serializer.Deserialize(file, typeof(List<Encounter>));
            }
            return toReturn;
        }*/
        public Encounter getEncounter(string name)
        {
            foreach (Encounter encounter in mod.moduleEncountersList)
            {
                if (encounter.encounterName == name) return encounter;
            }
            return null;
        }

        /*public void savePropsFile(string filename)
        {
            string json = JsonConvert.SerializeObject(mod.modulePropsList, Newtonsoft.Json.Formatting.Indented);
            using (StreamWriter sw = new StreamWriter(filename))
            {
                sw.Write(json.ToString());
            }
        }*/
        public List<Prop> loadPropsFile(string filename)
        {
            List<Prop> toReturn = null;

            // deserialize JSON directly from a file
            using (StreamReader file = File.OpenText(filename))
            {
                JsonSerializer serializer = new JsonSerializer();
                toReturn = (List<Prop>)serializer.Deserialize(file, typeof(List<Prop>));
            }
            return toReturn;
        }
        public Prop getPropByTag(string tag)
        {
            foreach (Prop it in mod.modulePropsList)
            {
                if (it.PropTag == tag) return it;
            }
            return null;
        }

        /*public void saveJournalFile(string filename)
        {
            string json = JsonConvert.SerializeObject(mod.moduleJournal, Newtonsoft.Json.Formatting.Indented);
            using (StreamWriter sw = new StreamWriter(filename))
            {
                sw.Write(json.ToString());
            }
        }*/
        public List<JournalQuest> loadJournalFile(string filename)
        {
            List<JournalQuest> toReturn = null;

            // deserialize JSON directly from a file
            using (StreamReader file = File.OpenText(filename))
            {
                JsonSerializer serializer = new JsonSerializer();
                toReturn = (List<JournalQuest>)serializer.Deserialize(file, typeof(List<JournalQuest>));
            }
            return toReturn;
        }
        public JournalQuest getJournalCategoryByName(string name)
        {
            foreach (JournalQuest it in mod.moduleJournal)
            {
                if (it.Name == name) return it;
            }
            return null;
        }
        public JournalQuest getJournalCategoryByTag(string tag)
        {
            foreach (JournalQuest it in mod.moduleJournal)
            {
                if (it.Tag == tag) return it;
            }
            return null;
        }

        /*public void savePlayerClassesFile(string filename)
        {
            string json = JsonConvert.SerializeObject(mod.modulePlayerClassList, Newtonsoft.Json.Formatting.Indented);
            using (StreamWriter sw = new StreamWriter(filename))
            {
                sw.Write(json.ToString());
            }
        }*/
        public List<PlayerClass> loadPlayerClassesFile(string filename)
        {
            List<PlayerClass> toReturn = null;

            // deserialize JSON directly from a file
            using (StreamReader file = File.OpenText(filename))
            {
                JsonSerializer serializer = new JsonSerializer();
                toReturn = (List<PlayerClass>)serializer.Deserialize(file, typeof(List<PlayerClass>));
            }
            return toReturn;
        }
        public PlayerClass getPlayerClassByTag(string tag)
        {
            foreach (PlayerClass ts in mod.modulePlayerClassList)
            {
                if (ts.tag == tag) return ts;
            }
            return null;
        }

        /*public void saveRacesFile(string filename)
        {
            string json = JsonConvert.SerializeObject(mod.moduleRacesList, Newtonsoft.Json.Formatting.Indented);
            using (StreamWriter sw = new StreamWriter(filename))
            {
                sw.Write(json.ToString());
            }
        }*/
        public List<Race> loadRacesFile(string filename)
        {
            List<Race> toReturn = null;

            // deserialize JSON directly from a file
            using (StreamReader file = File.OpenText(filename))
            {
                JsonSerializer serializer = new JsonSerializer();
                toReturn = (List<Race>)serializer.Deserialize(file, typeof(List<Race>));
            }
            return toReturn;
        }
        public Race getRaceByTag(string tag)
        {
            foreach (Race ts in mod.moduleRacesList)
            {
                if (ts.tag == tag) return ts;
            }
            return null;
        }

        /*public void saveSpellsFile(string filename)
        {
            string json = JsonConvert.SerializeObject(mod.moduleSpellsList, Newtonsoft.Json.Formatting.Indented);
            using (StreamWriter sw = new StreamWriter(filename))
            {
                sw.Write(json.ToString());
            }
        }*/
        public List<Spell> loadSpellsFile(string filename)
        {
            List<Spell> toReturn = null;

            // deserialize JSON directly from a file
            using (StreamReader file = File.OpenText(filename))
            {
                JsonSerializer serializer = new JsonSerializer();
                toReturn = (List<Spell>)serializer.Deserialize(file, typeof(List<Spell>));
            }
            return toReturn;
        }
        public Spell getSpellByTag(string tag)
        {
            foreach (Spell s in mod.moduleSpellsList)
            {
                if (s.tag == tag) return s;
            }
            return null;
        }
        public Spell getSpellByName(string name)
        {
            foreach (Spell s in mod.moduleSpellsList)
            {
                if (s.name == name) return s;
            }
            return null;
        }

        /*public void saveTraitsFile(string filename)
        {
            string json = JsonConvert.SerializeObject(mod.moduleTraitsList, Newtonsoft.Json.Formatting.Indented);
            using (StreamWriter sw = new StreamWriter(filename))
            {
                sw.Write(json.ToString());
            }
        }*/
        public List<Trait> loadTraitsFile(string filename)
        {
            List<Trait> toReturn = null;

            // deserialize JSON directly from a file
            using (StreamReader file = File.OpenText(filename))
            {
                JsonSerializer serializer = new JsonSerializer();
                toReturn = (List<Trait>)serializer.Deserialize(file, typeof(List<Trait>));
            }
            return toReturn;
        }
        public Trait getTraitByTag(string tag)
        {
            foreach (Trait ts in mod.moduleTraitsList)
            {
                if (ts.tag == tag) return ts;
            }
            return null;
        }
        public Trait getTraitByName(string name)
        {
            foreach (Trait ts in mod.moduleTraitsList)
            {
                if (ts.name == name) return ts;
            }
            return null;
        }

        /*public void saveEffectsFile(string filename)
        {
            string json = JsonConvert.SerializeObject(mod.moduleEffectsList, Newtonsoft.Json.Formatting.Indented);
            using (StreamWriter sw = new StreamWriter(filename))
            {
                sw.Write(json.ToString());
            }
        }*/
        public List<Effect> loadEffectsFile(string filename)
        {
            List<Effect> toReturn = null;

            // deserialize JSON directly from a file
            using (StreamReader file = File.OpenText(filename))
            {
                JsonSerializer serializer = new JsonSerializer();
                toReturn = (List<Effect>)serializer.Deserialize(file, typeof(List<Effect>));
            }
            return toReturn;
        }
        public Effect getEffectByTag(string tag)
        {
            foreach (Effect ef in mod.moduleEffectsList)
            {
                if (ef.tag == tag) return ef;
            }
            return null;
        }
        #endregion
                
        private void tsBtnResetDropDowns_Click(object sender, EventArgs e)
        {
            refreshDropDownLists();
        }
        private void tsBtnDataCheck_Click(object sender, EventArgs e)
        {
            DataCheck dc = new DataCheck(mod, this);
            dc.CheckAllData();
            dc = null;
        }
        private void mergerEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MergerEditor mergerEdit = new MergerEditor(mod, this);
            mergerEdit.ShowDialog();
        }
        private void tsBtnIBScriptEditor_Click(object sender, EventArgs e)
        {

        }
        private void rulesEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RulesEditor rulesEdit = new RulesEditor(mod, this);
            rulesEdit.ShowDialog();
        }
        private void tilesUsedInModuleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<string> tilenames = new List<string>();
            //iterate over each area and add tile names to tilenames if not already contained
            foreach (Area ar in mod.moduleAreasObjects)
            {
                foreach (string t in ar.Layer1Filename)
                {
                    if (!tilenames.Contains(t)) { tilenames.Add(t); }
                }
                foreach (string t in ar.Layer2Filename)
                {
                    if (!tilenames.Contains(t)) { tilenames.Add(t); }
                }
                foreach (string t in ar.Layer3Filename)
                {
                    if (!tilenames.Contains(t)) { tilenames.Add(t); }
                }

                // try and load the file selected if it exists
                /*string g_directory = this._mainDirectory + "\\modules\\" + mod.moduleName + "\\areas";
                if (File.Exists(g_directory + "\\" + ar + ".lvl"))
                {
                    try
                    {
                        areaObj = areaObj.loadAreaFile(g_directory + "\\" + ar + ".lvl");
                        foreach (string t in areaObj.Layer1Filename)
                        {
                            if (!tilenames.Contains(t)) { tilenames.Add(t); }
                        }
                        foreach (string t in areaObj.Layer2Filename)
                        {
                            if (!tilenames.Contains(t)) { tilenames.Add(t); }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("failed to open file: " + ex.ToString());
                    }                    
                }*/
            }
            
            //iterate over each encounter and add tile names to List<string> if not already contained
            foreach (Encounter enc in mod.moduleEncountersList)
            {
                foreach (string t in enc.Layer1Filename)
                {
                    if (!tilenames.Contains(t)) { tilenames.Add(t); }
                }
                foreach (string t in enc.Layer2Filename)
                {
                    if (!tilenames.Contains(t)) { tilenames.Add(t); }
                }
                foreach (string t in enc.Layer3Filename)
                {
                    if (!tilenames.Contains(t)) { tilenames.Add(t); }
                }
            }
            //write out list to a file 'tiles_used.txt', one tile per line
            if (Directory.Exists(this._mainDirectory + "\\modules\\" + mod.moduleName + "\\tiles\\tiles_used"))
            {
                try
                {
                    //delete folder and all contents first then create a clean folder to fill
                    Directory.Delete(this._mainDirectory + "\\modules\\" + mod.moduleName + "\\tiles\\tiles_used", true);
                    createDirectory(this._mainDirectory + "\\modules\\" + mod.moduleName + "\\tiles\\tiles_used");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to delete 'tiles_used' folder and then create a new version: " + ex.ToString());
                }
            }  
            else
            {
                try
                {
                    createDirectory(this._mainDirectory + "\\modules\\" + mod.moduleName + "\\tiles\\tiles_used");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to create the folder 'tiles_used': " + ex.ToString());
                }
            }
            try
            {
                tilenames.Sort();
                File.WriteAllLines(this._mainDirectory + "\\modules\\" + mod.moduleName + "\\tiles\\tiles_used\\00_tiles_used_list.txt", tilenames);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to create a text file with list of tiles used: " + ex.ToString());
            }
            //create a folder 'tiles_used' and copy the tiles into it
            foreach (string s in tilenames)
            {
                try
                {
                    File.Copy(
                        this._mainDirectory + "\\modules\\" + mod.moduleName + "\\tiles\\" + s + ".png",
                        this._mainDirectory + "\\modules\\" + mod.moduleName + "\\tiles\\tiles_used\\" + s + ".png",
                        true);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to copy file: s  " + ex.ToString());
                }
            }

            MessageBox.Show("A list of tiles and the tile files have been copied to a folder called 'tiles_used' in your " +
                            "module's 'tiles' folder. If the 'tiles_used' folder existed before, it was deleted first and then updated " + 
                            "to the currently used tiles in your module.");
        }

        private void convertAnIB2ModuleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Ask for a module file/folder location
            openFileDialog1.InitialDirectory = Environment.CurrentDirectory + "\\modules";
            //Empty the FileName text box of the dialog
            openFileDialog1.FileName = String.Empty;
            openFileDialog1.Filter = "Module files (*.mod)|*.mod|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;
            DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {
                try
                {
                    //LOAD module file and copy over information
                    string filename = Path.GetFullPath(openFileDialog1.FileName);
                    string directory = Path.GetDirectoryName(openFileDialog1.FileName);
                    Module modIBmini = new Module();
                    modIBmini = modIBmini.loadModuleFile(filename);
                    if (modIBmini == null)
                    {
                        MessageBox.Show("returned a null module");
                    }
                    //LOAD all data (creatures, items, spells, races, player classes, etc.) and convert as needed (move distance, attack range, etc.)
                    modIBmini.moduleCreaturesList = loadCreaturesFile(directory + "\\data\\creatures.json");
                    foreach (Creature crt in modIBmini.moduleCreaturesList)
                    {
                        crt.moveDistance = (int)Math.Round(((double)crt.moveDistance / 2f), MidpointRounding.AwayFromZero);
                        crt.cr_attRange = (int)Math.Round(((double)crt.cr_attRange / 2f), MidpointRounding.AwayFromZero);
                    }
                    modIBmini.moduleContainersList = loadContainersFile(directory + "\\data\\containers.json");
                    modIBmini.moduleItemsList = loadItemsFile(directory + "\\data\\items.json");
                    foreach (Item it in modIBmini.moduleItemsList)
                    {
                        it.attackRange = (int)Math.Round((double)(it.attackRange / 2), MidpointRounding.AwayFromZero);
                    }
                    modIBmini.moduleShopsList = loadShopsFile(directory + "\\data\\shops.json");
                    modIBmini.modulePropsList = loadPropsFile(directory + "\\data\\props.json");
                    modIBmini.moduleJournal = loadJournalFile(directory + "\\data\\journal.json");
                    modIBmini.modulePlayerClassList = loadPlayerClassesFile(directory + "\\data\\playerClasses.json");
                    modIBmini.moduleRacesList = loadRacesFile(directory + "\\data\\races.json");
                    foreach (Race rce in modIBmini.moduleRacesList)
                    {
                        rce.MoveDistanceLightArmor = (int)Math.Round((double)(rce.MoveDistanceLightArmor / 2), MidpointRounding.AwayFromZero);
                        rce.MoveDistanceMediumHeavyArmor = (int)Math.Round((double)(rce.MoveDistanceMediumHeavyArmor / 2), MidpointRounding.AwayFromZero);
                    }
                    modIBmini.moduleSpellsList = loadSpellsFile(directory + "\\data\\spells.json");
                    foreach (Spell sp in modIBmini.moduleSpellsList)
                    {
                        sp.range = (int)Math.Round((double)(sp.range / 2), MidpointRounding.AwayFromZero);
                    }
                    modIBmini.moduleTraitsList = loadTraitsFile(directory + "\\data\\traits.json");
                    foreach (Trait tr in modIBmini.moduleTraitsList)
                    {
                        tr.range = (int)Math.Round((double)(tr.range / 2), MidpointRounding.AwayFromZero);
                    }
                    modIBmini.moduleEffectsList = loadEffectsFile(directory + "\\data\\effects.json");

                    //LOAD all areas, convos, and encounters and convert to new format as needed
                    ImportAreas(modIBmini, directory);
                    ImportConvos(modIBmini, directory);
                    ImportEncounters(modIBmini, directory);
                    //make a list of images that need to be copied over to the hak folder (images that do not exist in default/NewModule/graphics folder)      
                    //SAVE out the module file   
                    string fullPathFilename = _mainDirectory + "\\modules\\" + modIBmini.moduleName + ".mod";
                    saveModule(modIBmini, fullPathFilename);
                    //save areas
                    saveAreas(modIBmini, fullPathFilename);
                    //save encounters
                    saveEncounters(modIBmini, fullPathFilename);
                    //save convos
                    saveConvos(modIBmini, fullPathFilename);
                    //save images
                    MessageBox.Show("Moduled saved as: " + modIBmini.moduleName + ".mod in the module folder.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("failed to create IBmini module: " + ex.ToString());
                }
            }
        }
        public void ImportAreas(Module mod, string directory)
        {
            mod.moduleAreasObjects.Clear();

            AreaIB2 IB2area = new AreaIB2();

            string jobDir = "";
            jobDir = directory + "\\areas";
            foreach (string f in Directory.GetFiles(jobDir, "*.*", SearchOption.AllDirectories))
            {
                string filename = Path.GetFileName(f);
                // deserialize JSON directly from a file
                using (StreamReader file = File.OpenText(directory + "\\areas\\" + filename))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    IB2area = (AreaIB2)serializer.Deserialize(file, typeof(AreaIB2));
                }
                
                Area copyEnc = new Area();
                copyEnc.Filename = IB2area.Filename;
                copyEnc.UseDayNightCycle = IB2area.UseDayNightCycle;
                copyEnc.MapSizeX = IB2area.MapSizeX;
                copyEnc.MapSizeY = IB2area.MapSizeY;
                copyEnc.AreaVisibleDistance = IB2area.AreaVisibleDistance;
                copyEnc.RestingAllowed = IB2area.RestingAllowed;
                copyEnc.UseMiniMapFogOfWar = IB2area.UseMiniMapFogOfWar;
                copyEnc.areaDark = IB2area.areaDark;
                copyEnc.TimePerSquare = IB2area.TimePerSquare;

                copyEnc.Layer1Filename = new List<string>();
                copyEnc.Layer1Mirror = new List<int>();
                copyEnc.Layer1Rotate = new List<int>();
                copyEnc.Layer2Filename = new List<string>();
                copyEnc.Layer2Mirror = new List<int>();
                copyEnc.Layer2Rotate = new List<int>();
                copyEnc.Layer3Filename = new List<string>();
                copyEnc.Layer3Mirror = new List<int>();
                copyEnc.Layer3Rotate = new List<int>();
                copyEnc.Walkable = new List<int>();
                copyEnc.LoSBlocked = new List<int>();
                foreach (Tile tile in IB2area.Tiles)
                {
                    copyEnc.Layer1Filename.Add(tile.Layer1Filename);
                    copyEnc.Layer1Rotate.Add(tile.Layer1Rotate);
                    if (tile.Layer1Mirror) { copyEnc.Layer1Mirror.Add(1); }
                    else { copyEnc.Layer1Mirror.Add(0); }

                    copyEnc.Layer2Filename.Add(tile.Layer2Filename);
                    copyEnc.Layer2Rotate.Add(tile.Layer2Rotate);
                    if (tile.Layer2Mirror) { copyEnc.Layer2Mirror.Add(1); }
                    else { copyEnc.Layer2Mirror.Add(0); }

                    copyEnc.Layer3Filename.Add(tile.Layer3Filename);
                    copyEnc.Layer3Rotate.Add(tile.Layer3Rotate);
                    if (tile.Layer3Mirror) { copyEnc.Layer3Mirror.Add(1); }
                    else { copyEnc.Layer3Mirror.Add(0); }

                    if (tile.Walkable) { copyEnc.Walkable.Add(1); }
                    else { copyEnc.Walkable.Add(0); }
                    if (tile.LoSBlocked) { copyEnc.LoSBlocked.Add(1); }
                    else { copyEnc.LoSBlocked.Add(0); }
                    if (tile.Visible) { copyEnc.Visible.Add(1); }
                    else { copyEnc.Visible.Add(0); }
                }

                copyEnc.Props = new List<Prop>();
                foreach (Prop s in IB2area.Props)
                {
                    Prop newCrtRef = new Prop();
                    newCrtRef = s.DeepCopy();
                    copyEnc.Props.Add(newCrtRef);
                }


                copyEnc.Triggers = new List<Trigger>();
                foreach (Trigger s in IB2area.Triggers)
                {
                    Trigger newItRef = new Trigger();
                    newItRef = s.DeepCopy();
                    copyEnc.Triggers.Add(newItRef);
                }
                copyEnc.NextIdNumber = IB2area.NextIdNumber;
                copyEnc.OnHeartBeatIBScript = IB2area.OnHeartBeatIBScript;
                copyEnc.OnHeartBeatIBScriptParms = IB2area.OnHeartBeatIBScriptParms;
                copyEnc.inGameAreaName = IB2area.inGameAreaName;

                mod.moduleAreasObjects.Add(copyEnc);
            }
        }
        public void ImportConvos(Module mod, string directory)
        {
            mod.moduleConvoList.Clear();

            Convo IB2convo = new Convo();
            string jobDir = "";
            jobDir = directory + "\\dialog";
            foreach (string f in Directory.GetFiles(jobDir, "*.*", SearchOption.AllDirectories))
            {
                string filename = Path.GetFileName(f);
                if (!filename.EndsWith(".json"))
                {
                    continue;
                }
                // deserialize JSON directly from a file
                using (StreamReader file = File.OpenText(directory + "\\dialog\\" + filename))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    IB2convo = (Convo)serializer.Deserialize(file, typeof(Convo));
                }
                mod.moduleConvoList.Add(IB2convo);
            }
        }
        public void ImportEncounters(Module mod, string directory)
        {
            List<EncounterIB2> IB2encList = new List<EncounterIB2>();
            if (File.Exists(directory + "\\data\\encounters.json"))
            {
                //mod.moduleEncountersList.Clear();
                // deserialize JSON directly from a file
                using (StreamReader file = File.OpenText(directory + "\\data\\encounters.json"))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    IB2encList = (List<EncounterIB2>)serializer.Deserialize(file, typeof(List<EncounterIB2>));
                }
            }
            else
            {
                MessageBox.Show("Couldn't find encounters.json file.");
                return;
            }
            //copy over important data
            mod.moduleEncountersList.Clear();
            foreach (EncounterIB2 encIB2 in IB2encList)
            {
                Encounter copyEnc = new Encounter();
                copyEnc.encounterName = encIB2.encounterName;
                copyEnc.UseDayNightCycle = encIB2.UseDayNightCycle;
                copyEnc.MapSizeX = encIB2.MapSizeX;
                copyEnc.MapSizeY = encIB2.MapSizeY;

                copyEnc.Layer1Filename = new List<string>();
                copyEnc.Layer1Mirror = new List<int>();
                copyEnc.Layer1Rotate = new List<int>();
                copyEnc.Layer2Filename = new List<string>();
                copyEnc.Layer2Mirror = new List<int>();
                copyEnc.Layer2Rotate = new List<int>();
                copyEnc.Layer3Filename = new List<string>();
                copyEnc.Layer3Mirror = new List<int>();
                copyEnc.Layer3Rotate = new List<int>();
                copyEnc.Walkable = new List<int>();
                copyEnc.LoSBlocked = new List<int>();
                foreach (TileEnc tile in encIB2.encounterTiles)
                {
                    copyEnc.Layer1Filename.Add(tile.Layer1Filename);
                    copyEnc.Layer1Rotate.Add(tile.Layer1Rotate);
                    if (tile.Layer1Mirror) { copyEnc.Layer1Mirror.Add(1); }
                    else { copyEnc.Layer1Mirror.Add(0); }

                    copyEnc.Layer2Filename.Add(tile.Layer2Filename);
                    copyEnc.Layer2Rotate.Add(tile.Layer2Rotate);
                    if (tile.Layer2Mirror) { copyEnc.Layer2Mirror.Add(1); }
                    else { copyEnc.Layer2Mirror.Add(0); }

                    copyEnc.Layer3Filename.Add(tile.Layer3Filename);
                    copyEnc.Layer3Rotate.Add(tile.Layer3Rotate);
                    if (tile.Layer3Mirror) { copyEnc.Layer3Mirror.Add(1); }
                    else { copyEnc.Layer3Mirror.Add(0); }

                    if (tile.Walkable) { copyEnc.Walkable.Add(1); }
                    else { copyEnc.Walkable.Add(0); }
                    if (tile.LoSBlocked) { copyEnc.LoSBlocked.Add(1); }
                    else { copyEnc.LoSBlocked.Add(0); }
                }

                //public List<CreatureRefs> encounterCreatureRefsList = new List<CreatureRefs>();
                copyEnc.encounterCreatureRefsList = new List<CreatureRefs>();
                foreach (CreatureRefs s in encIB2.encounterCreatureRefsList)
                {
                    CreatureRefs newCrtRef = new CreatureRefs();
                    newCrtRef.creatureResRef = s.creatureResRef;
                    newCrtRef.creatureTag = s.creatureTag;
                    newCrtRef.creatureStartLocationX = s.creatureStartLocationX;
                    newCrtRef.creatureStartLocationY = s.creatureStartLocationY;
                    copyEnc.encounterCreatureRefsList.Add(newCrtRef);
                }
                //public List<ItemRefs> encounterInventoryRefsList = new List<ItemRefs>();
                copyEnc.encounterInventoryRefsList = new List<ItemRefs>();
                foreach (ItemRefs s in encIB2.encounterInventoryRefsList)
                {
                    ItemRefs newItRef = new ItemRefs();
                    newItRef = s.DeepCopy();
                    copyEnc.encounterInventoryRefsList.Add(newItRef);
                }
                //public List<Coordinate> encounterPcStartLocations = new List<Coordinate>();
                copyEnc.encounterPcStartLocations = new List<Coordinate>();
                foreach (Coordinate s in encIB2.encounterPcStartLocations)
                {
                    Coordinate newCoor = new Coordinate();
                    newCoor.X = s.X;
                    newCoor.Y = s.Y;
                    copyEnc.encounterPcStartLocations.Add(newCoor);
                }

                copyEnc.goldDrop = encIB2.goldDrop;
                copyEnc.OnSetupCombatIBScript = encIB2.OnSetupCombatIBScript;
                copyEnc.OnSetupCombatIBScriptParms = encIB2.OnSetupCombatIBScriptParms;
                copyEnc.OnStartCombatRoundIBScript = encIB2.OnStartCombatRoundIBScript;
                copyEnc.OnStartCombatRoundIBScriptParms = encIB2.OnStartCombatRoundIBScriptParms;
                copyEnc.OnStartCombatTurnIBScript = encIB2.OnStartCombatTurnIBScript;
                copyEnc.OnStartCombatTurnIBScriptParms = encIB2.OnStartCombatTurnIBScriptParms;
                copyEnc.OnEndCombatIBScript = encIB2.OnEndCombatIBScript;
                copyEnc.OnEndCombatIBScriptParms = encIB2.OnEndCombatIBScriptParms;

                mod.moduleEncountersList.Add(copyEnc);
            }
        }

        private void moduleEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ModuleEditor modEdit = new ModuleEditor(mod, this);
            modEdit.ShowDialog();
        }
    }
}
