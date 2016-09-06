using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace IB2ToolsetMini
{
    public partial class NWNtoIBmini : Form
    {
        private ParentForm prntForm;
        public Module modIBmini = new Module();
        public string camDescription = "";
        public string camDisplayName = "";

        //Transitions stuff
        public List<NwnWayPoint> waypointList = new List<NwnWayPoint>();
        public List<NwnTrigger> triggerList = new List<NwnTrigger>();
        public List<NwnDoor> doorList = new List<NwnDoor>();

        //Conversation conversion stuff
        public List<string> portraits_needed = new List<string>();
        public List<string> scripts_needed = new List<string>();
        public List<string> scriptTypesNeeded = new List<string>();
        public List<string> possibleCompanions = new List<string>();
        public int nextIndex = 100000;
        public int nextLinkIdNumber = 100000;
        public int nextIdNumber = 1;
        public List<DlgSyncStruct> dlgStartingnodeList = new List<DlgSyncStruct>();
        public List<ContentNode> dlgEntrynodeList = new List<ContentNode>();
        public List<ContentNode> dlgReplynodeList = new List<ContentNode>();


        public NWNtoIBmini(ParentForm pf)
        {
            InitializeComponent();
            prntForm = pf;
        }

        public void doConversionInfoAndSave()
        {
            //Summary Report and Todo List
            string summaryReportPath = prntForm._mainDirectory + "\\modules\\" + modIBmini.moduleName + "_ConversionSummaryAndTodoList.txt";
            try
            {
                File.Delete(summaryReportPath);
            }
            catch { }
            string todolist = "TASKS TODO" + Environment.NewLine +
                              "-build areas" + Environment.NewLine +
                              "-build encounters" + Environment.NewLine +
                              "-build shops" + Environment.NewLine +
                              "-build containers" + Environment.NewLine +
                              "-go through all convos and verify scripts" + Environment.NewLine +
                              "-find and assign portraits to convos" + Environment.NewLine +
                              "-find and assign tokens to custom creatures" + Environment.NewLine +
                              "-find and assign item images to custom items" + Environment.NewLine;
            File.AppendAllText(summaryReportPath, todolist);
            File.AppendAllText(summaryReportPath, Environment.NewLine + "POSSIBLE COMPANIONS" + Environment.NewLine);
            File.AppendAllLines(summaryReportPath, possibleCompanions);
            portraits_needed.Sort();
            File.AppendAllText(summaryReportPath, Environment.NewLine + "PORTRAITS NEEDED" + Environment.NewLine);
            File.AppendAllLines(summaryReportPath, portraits_needed);
            File.AppendAllText(summaryReportPath, Environment.NewLine + "NON-IB SCRIPT TYPES FOUND" + Environment.NewLine);
            File.AppendAllLines(summaryReportPath, scriptTypesNeeded);
            File.AppendAllText(summaryReportPath, Environment.NewLine + "NON-IB SCRIPTS FOUND (DETAILS)" + Environment.NewLine);
            File.AppendAllLines(summaryReportPath, scripts_needed);

            //SAVE out the module file   
            string fullPathFilename = prntForm._mainDirectory + "\\modules\\" + modIBmini.moduleName + ".mod";
            //save module data
            prntForm.saveModule(modIBmini, fullPathFilename);
            //save areas
            prntForm.saveAreas(modIBmini, fullPathFilename);
            //save encounters
            prntForm.saveEncounters(modIBmini, fullPathFilename);
            //save convos
            prntForm.saveConvos(modIBmini, fullPathFilename);
            //finished
            MessageBox.Show("Moduled saved as: " + modIBmini.moduleName + ".mod in the module folder.");
            //delete 'temp' directory
            try
            {
                Directory.Delete(prntForm._mainDirectory + "\\temp", true);
            }
            catch { }
            this.Close();
        }
        private void btnConvertFromModFile_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = Environment.CurrentDirectory;
            //Empty the FileName text box of the dialog
            openFileDialog1.FileName = String.Empty;
            openFileDialog1.Filter = "NWN2 Module file (*.mod)|*.mod|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;

            DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {
                string filename = Path.GetFullPath(openFileDialog1.FileName);
                //string directory = Path.GetDirectoryName(openFileDialog1.FileName);
                //read the .mod, iterate through all the files and convert them one by one and add to IBmini module
                ErfFile erf = new ErfFile(filename);

                // first load the default module and clear out some of the Lists
                loadDefaultModule();
                modIBmini.moduleName = Path.GetFileNameWithoutExtension(filename);
                modIBmini.moduleLabelName = Path.GetFileNameWithoutExtension(filename);
                modIBmini.moduleAreasObjects.Clear();
                modIBmini.moduleEncountersList.Clear();
                modIBmini.moduleJournal.Clear();

                //create 'temp' directory
                Directory.CreateDirectory(prntForm._mainDirectory + "\\temp");
                //iterate through all files and add to modIBmini
                for (int i = 0; i < erf.thisHeader.EntryCount; i++)
                {
                    byte[] newArray = new byte[erf.ResourceList[i].ResourceSize];
                    Array.Copy(erf.fileBytes, erf.ResourceList[i].OffsetToResource, newArray, 0, erf.ResourceList[i].ResourceSize);
                    try
                    {
                        File.WriteAllBytes(prntForm._mainDirectory + "\\temp\\" + erf.KeyList[i].ResRef + "." + ((ResourceType)erf.KeyList[i].ResType).ToString(), newArray);
                    }
                    catch (Exception ex)
                    {

                    }
                }
                //go through all files and convert them one by one and add to IBmini module
                processFiles(prntForm._mainDirectory + "\\temp");
                overrideModWithCampaignInfo();
                doConversionInfoAndSave();       
            }
        }
        private void btnConvertFromModFolder_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.SelectedPath = Environment.CurrentDirectory;
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                if (folderBrowserDialog1.SelectedPath != "")
                {
                    //first load the default module and clear out some of the Lists
                    loadDefaultModule();
                    modIBmini.moduleName = Path.GetFileName(folderBrowserDialog1.SelectedPath);
                    modIBmini.moduleLabelName = Path.GetFileName(folderBrowserDialog1.SelectedPath);
                    modIBmini.moduleAreasObjects.Clear();
                    modIBmini.moduleEncountersList.Clear();
                    modIBmini.moduleJournal.Clear();

                    //go through all files and convert them one by one and add to IBmini module
                    processFiles(folderBrowserDialog1.SelectedPath);
                    overrideModWithCampaignInfo();
                    doConversionInfoAndSave();
                }
            }
        }
        public void overrideModWithCampaignInfo()
        {
            if (!camDescription.Equals(""))
            {
                modIBmini.moduleDescription = camDescription;
            }
            if (!camDisplayName.Equals(""))
            {
                modIBmini.moduleLabelName = camDisplayName;
            }
        }
        public void loadDefaultModule()
        {
            using (StreamReader sr = File.OpenText(prntForm._mainDirectory + "\\default\\NewModule\\NewModule.mod"))
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
                    modIBmini = (Module)JsonConvert.DeserializeObject(s, typeof(Module));
                }
                //Read in the areas
                modIBmini.moduleAreasObjects.Clear();
                Area ar;
                for (int i = 0; i < 9999; i++)
                {
                    s = sr.ReadLine();
                    if ((s == null) || (s.Equals("ENCOUNTERS")))
                    {
                        break;
                    }
                    ar = (Area)JsonConvert.DeserializeObject(s, typeof(Area));
                    modIBmini.moduleAreasObjects.Add(ar);
                }
                //Read in the encounters
                modIBmini.moduleEncountersList.Clear();
                Encounter enc;
                for (int i = 0; i < 9999; i++)
                {
                    s = sr.ReadLine();
                    if ((s == null) || (s.Equals("CONVOS")))
                    {
                        break;
                    }
                    enc = (Encounter)JsonConvert.DeserializeObject(s, typeof(Encounter));
                    modIBmini.moduleEncountersList.Add(enc);
                }
                //Read in the convos
                modIBmini.moduleConvoList.Clear();
                Convo cnv;
                for (int i = 0; i < 9999; i++)
                {
                    s = sr.ReadLine();
                    if ((s == null) || (s.Equals("IMAGES")) || (s.Equals("END")))
                    {
                        break;
                    }
                    cnv = (Convo)JsonConvert.DeserializeObject(s, typeof(Convo));
                    modIBmini.moduleConvoList.Add(cnv);
                }
                //Read in the images
                modIBmini.moduleImageDataList.Clear();
            }
        }
        public void processFiles(string folderPath)
        {
            foreach (string filename in Directory.GetFiles(folderPath, "*.*", SearchOption.AllDirectories))
            {
                string filenameNoExt = Path.GetFileNameWithoutExtension(filename);
                filenameNoExt = filenameNoExt.Replace(" ", "_");
                if ((filename.EndsWith(".UTI")) || (filename.EndsWith(".UTC")) || (filename.EndsWith(".uti")) || (filename.EndsWith(".utc")))
                {
                    GffFile gff = new GffFile(filename);
                    if (gff != null)
                    {
                        if ((filename.EndsWith(".UTI")) || (filename.EndsWith(".uti")))
                        {
                            modIBmini.moduleItemsList.Add(addItem(gff));
                        }
                        else if ((filename.EndsWith(".UTC")) || (filename.EndsWith(".utc")))
                        {
                            modIBmini.moduleCreaturesList.Add(addCreature(gff));
                            modIBmini.modulePropsList.Add(addProp(gff));
                        }
                    }
                    gff = null;
                }
                else if ((filename.EndsWith(".JRL") || filename.EndsWith(".jrl")))
                {
                    GffFile gff = new GffFile(filename);
                    if (gff != null)
                    {
                        convertJournal(gff);
                    }
                    gff = null;
                }
                else if ((filename.EndsWith(".IFO") || filename.EndsWith(".ifo")))
                {
                    GffFile gff = new GffFile(filename);
                    if (gff != null)
                    {
                        getModuleInfo(gff);
                    }
                    gff = null;
                }
                else if ((filename.EndsWith(".CAM") || filename.EndsWith(".cam")))
                {
                    GffFile gff = new GffFile(filename);
                    if (gff != null)
                    {
                        getCampaignInfo(gff);
                    }
                    gff = null;
                }
                else if ((filename.EndsWith(".DLG") || filename.EndsWith(".dlg")))
                {
                    GffFile gff = new GffFile(filename);                    
                    if (gff != null)
                    {
                        fillDlgLists(gff);
                        Convo newConvo = makeIbCon(filenameNoExt);
                        modIBmini.moduleConvoList.Add(newConvo);
                    }
                    gff = null;
                }
                else if ((filename.EndsWith(".GIT")) || (filename.EndsWith(".git")))
                {
                    GffFile gff = new GffFile(filename);                    
                    if (gff != null)
                    {
                        string fullpathForArea = "";
                        if (filename.EndsWith("GIT"))
                        {
                            fullpathForArea = filename.Replace(".GIT", ".ARE");
                        }
                        else
                        {
                            fullpathForArea = filename.Replace(".git", ".are");
                        }                      
                        GffFile gffArea = new GffFile(fullpathForArea);                        
                        if (gffArea != null)
                        {
                            Area newArea = createNewArea(gffArea);
                            //go through GIT and find all creatures/npcs and place in area file as props
                            addPropsToArea(gff, gffArea, newArea);
                            //go through GIT and find all transitions and place in area file
                            getTransitionsData(gff, gffArea, newArea);
                            modIBmini.moduleAreasObjects.Add(newArea);
                        }
                    }
                    gff = null;
                }
            }
            //once complete, go through each area and create transitions
            setupAllTransitions();
        }
        public void getModuleInfo(GffFile gff)
        {
            modIBmini.moduleDescription = GetFieldByLabel(gff.TopLevelStruct, "Mod_Description").Data.ToString();
            modIBmini.startingArea = GetFieldByLabel(gff.TopLevelStruct, "Mod_Entry_Area").Data.ToString();
            //modIBmini.moduleLabelName = GetFieldByLabel(gff.TopLevelStruct, "Mod_Name").Data.ToString();
        }
        public void getCampaignInfo(GffFile gff)
        {
            camDescription = GetFieldByLabel(gff.TopLevelStruct, "Description").Data.ToString();
            camDisplayName = GetFieldByLabel(gff.TopLevelStruct, "DisplayName").Data.ToString();
        }
        public Item addItem(GffFile gff)
        {
            Item newItem = new Item();
            foreach (GffField field in gff.TopLevelStruct.Fields)
            {
                string key = field.Label;
                //resref
                if (key.Equals("TemplateResRef"))
                {
                    newItem.resref = field.Data.ToString();
                }
                //tag
                else if (key.Equals("Tag"))
                {
                    newItem.tag = field.Data.ToString();
                }
                //desc or descId
                else if ((key.Equals("Description")) || (key.Equals("DescIdentified")))
                {
                    newItem.desc = field.Data.ToString();
                }
                //cost + modifycost
                else if (key.Equals("Cost"))
                {
                    newItem.value += (int)field.ValueInt;
                }
                //cost + modifycost
                else if (key.Equals("ModifyCost"))
                {
                    newItem.value += (int)field.ValueInt;
                }
                //plot
                else if (key.Equals("Plot"))
                {
                    int isPlot = (int)field.ValueByte;
                    if (isPlot > 0)
                    {
                        newItem.plotItem = true;
                    }
                    else
                    {
                        newItem.plotItem = false;
                    }
                }
                //name
                else if (key.Equals("LocalizedName"))
                {
                    newItem.name = field.Data.ToString();
                }
                //category
                else if (key.Equals("Classification"))
                {
                    string cat = field.Data.ToString();
                    if (cat.Contains("184395"))
                    {
                        newItem.ItemCategoryName = "Plot-HC";
                        newItem.category = "General";
                    }
                    else if (cat.Contains("184383"))
                    {
                        newItem.ItemCategoryName = "Shields-HC";
                        newItem.category = "Shield";
                    }
                    else if (cat.Contains("184349"))
                    {
                        newItem.ItemCategoryName = "Boots-HC";
                        newItem.category = "Feet";
                    }
                    else if (cat.Contains("184359"))
                    {
                        newItem.ItemCategoryName = "Helmet-HC";
                        newItem.category = "Head";
                    }
                    else if (cat.Contains("184348"))
                    {
                        newItem.ItemCategoryName = "Armor-HC";
                        newItem.category = "Armor";
                    }
                    else if (cat.Contains("184425"))
                    {
                        newItem.ItemCategoryName = "Books-HC";
                        newItem.category = "General";
                    }
                    else if (cat.Contains("184418"))
                    {
                        newItem.ItemCategoryName = "Amulets-HC";
                        newItem.category = "Neck";
                    }
                    else if (cat.Contains("184420"))
                    {
                        newItem.ItemCategoryName = "Rings-HC";
                        newItem.category = "Ring";
                    }
                    else if (cat.Contains("184419"))
                    {
                        newItem.ItemCategoryName = "Potions-HC";
                        newItem.category = "General";
                    }
                    else if (cat.Contains("184350"))
                    {
                        newItem.ItemCategoryName = "Misc-HC";
                        newItem.category = "General";
                    }
                    else if (cat.Contains("184386"))
                    {
                        newItem.ItemCategoryName = "Weapons-HC";
                        newItem.category = "Ranged";
                    }
                    else if (cat.Contains("184363"))
                    {
                        newItem.ItemCategoryName = "Weapons-HC";
                        newItem.category = "Melee";
                    }
                    else
                    {
                        newItem.ItemCategoryName = field.Data.ToString();
                    }
                }
            }
            return newItem;
        }
        public Creature addCreature(GffFile gff)
        {
            Creature newItem = new Creature();
            string firstname = "";
            string lastname = "";
            foreach (GffField field in gff.TopLevelStruct.Fields)
            {
                string key = field.Label;
                //resref
                if (key.Equals("TemplateResRef"))
                {
                    newItem.cr_resref = field.Data.ToString();
                }
                //tag
                else if (key.Equals("Tag"))
                {
                    newItem.cr_tag = field.Data.ToString();
                }
                //desc
                else if (key.Equals("Description"))
                {
                    newItem.cr_desc = field.Data.ToString();
                }
                //firstname
                else if (key.Equals("FirstName"))
                {
                    firstname = field.Data.ToString();
                }
                //lastname
                else if (key.Equals("LastName"))
                {
                    lastname = field.Data.ToString();
                }
                //conversation
                else if (key.Equals("Conversation"))
                {
                    //OEIShared.IO.GFF.GFFResRefField val1 = (OEIShared.IO.GFF.GFFResRefField)field.Value;
                    //newItem. = val1.ValueCResRef.Value;
                }
                //hp
                else if (key.Equals("MaxHitPoints"))
                {
                    newItem.hp = (int)field.ValueShort;
                    newItem.hpMax = (int)field.ValueShort;
                }
                else if (key.Equals("refbonus"))
                {
                    newItem.reflex = (int)field.ValueShort;
                }
                else if (key.Equals("willbonus"))
                {
                    newItem.will = (int)field.ValueShort;
                }
                else if (key.Equals("fortbonus"))
                {
                    newItem.fortitude = (int)field.ValueShort;
                }
                //category
                else if (key.Equals("Classification"))
                {
                    string cat = field.Data.ToString();
                    if (cat.Contains("184305"))
                    {
                        newItem.cr_parentNodeName = "Animals-HC";
                    }
                    else if (cat.Contains("184321"))
                    {
                        newItem.cr_parentNodeName = "Elementals-HC";
                    }
                    else if (cat.Contains("184320"))
                    {
                        newItem.cr_parentNodeName = "Fey-HC";
                    }
                    else if (cat.Contains("184330"))
                    {
                        newItem.cr_parentNodeName = "Giants-HC";
                    }
                    else if (cat.Contains("184314"))
                    {
                        newItem.cr_parentNodeName = "Humanoids-HC";
                    }
                    else if (cat.Contains("184335"))
                    {
                        newItem.cr_parentNodeName = "NPCs-HC";
                    }
                    else if (cat.Contains("184308"))
                    {
                        newItem.cr_parentNodeName = "Outsiders-HC";
                    }
                    else if (cat.Contains("184329"))
                    {
                        newItem.cr_parentNodeName = "Undead-HC";
                    }
                    else
                    {
                        newItem.cr_parentNodeName = field.Data.ToString();
                    }
                }
            }
            newItem.cr_name = firstname;
            if (lastname.Length > 0)
            {
                newItem.cr_name = firstname + " " + lastname;
            }
            newItem.cr_tokenFilename = "prp_captive";
            return newItem;
        }
        public Prop addProp(GffFile gff)
        {
            Prop newItem = new Prop();
            string firstname = "";
            string lastname = "";
            foreach (GffField field in gff.TopLevelStruct.Fields)
            {
                string key = field.Label;
                //tag
                if (key.Equals("TemplateResRef"))
                {
                    newItem.PropTag = field.Data.ToString();
                }
                //tag
                else if (key.Equals("Tag"))
                {
                    //OEIShared.IO.GFF.GFFOEIExoStringField val1 = (OEIShared.IO.GFF.GFFOEIExoStringField)field.Value;
                    //newItem.PropTag = val1.ValueCExoString.Value;
                }
                //desc
                else if (key.Equals("Description"))
                {
                    newItem.MouseOverText = field.Data.ToString();
                }
                //firstname
                else if (key.Equals("FirstName"))
                {
                    firstname = field.Data.ToString();
                }
                //lastname
                else if (key.Equals("LastName"))
                {
                    lastname = field.Data.ToString();
                }
                //conversation
                else if (key.Equals("Conversation"))
                {
                    newItem.ConversationWhenOnPartySquare = field.Data.ToString();
                }
                //category
                else if (key.Equals("Classification"))
                {
                    string cat = field.Data.ToString();
                    if (cat.Contains("184305"))
                    {
                        newItem.PropCategoryName = "Animals-HC";
                    }
                    else if (cat.Contains("184321"))
                    {
                        newItem.PropCategoryName = "Elementals-HC";
                    }
                    else if (cat.Contains("184320"))
                    {
                        newItem.PropCategoryName = "Fey-HC";
                    }
                    else if (cat.Contains("184330"))
                    {
                        newItem.PropCategoryName = "Giants-HC";
                    }
                    else if (cat.Contains("184314"))
                    {
                        newItem.PropCategoryName = "Humanoids-HC";
                    }
                    else if (cat.Contains("184335"))
                    {
                        newItem.PropCategoryName = "NPCs-HC";
                    }
                    else if (cat.Contains("184308"))
                    {
                        newItem.PropCategoryName = "Outsiders-HC";
                    }
                    else if (cat.Contains("184329"))
                    {
                        newItem.PropCategoryName = "Undead-HC";
                    }
                    else
                    {
                        newItem.PropCategoryName = field.Data.ToString();
                    }
                }
            }
            newItem.PropName = firstname;
            newItem.MouseOverText = firstname;
            if (lastname.Length > 0)
            {
                newItem.PropName = firstname + " " + lastname;
                newItem.MouseOverText = firstname + " " + lastname;
            }
            newItem.ImageFileName = "prp_captive";
            return newItem;
        }
        public void addPropsToArea(GffFile gff, GffFile gffARE, Area area)
        {
            //used to see if indoor or outdoor area
            GffList tileList = (GffList)GetFieldByLabel(gffARE.TopLevelStruct, "TileList").Data;

            if (area.Filename.Equals(modIBmini.startingArea))
            {
                //x
                if (tileList.StructList.Count > 0) //indoors
                {
                    int xLoc = (int)(((GetFieldByLabel(gff.TopLevelStruct, "Mod_Entry_X").ValueFloat + 0.5f) / 1.5f) - 6);
                    modIBmini.startingPlayerPositionX = xLoc - 1;
                    if (modIBmini.startingPlayerPositionX < 0) { modIBmini.startingPlayerPositionX = 0; }
                }
                else //outdoors
                {
                    int xLoc = (int)(((GetFieldByLabel(gff.TopLevelStruct, "Mod_Entry_X").ValueFloat + 0.5f) / 5f) - 16);
                    modIBmini.startingPlayerPositionX = xLoc - 1;
                    if (modIBmini.startingPlayerPositionX < 0) { modIBmini.startingPlayerPositionX = 0; }
                }

                //y
                if (tileList.StructList.Count > 0) //indoors
                {
                    int yLoc = (int)(((GetFieldByLabel(gff.TopLevelStruct, "Mod_Entry_Y").ValueFloat + 0.5f) / 1.5f) - 6);
                    //need to invert the y value since IB2 measure top to bottom and nwn2 bottom to top so use MapSizeY - yLoc  
                    modIBmini.startingPlayerPositionY = area.MapSizeY - yLoc - 1;
                    if (modIBmini.startingPlayerPositionY < 0) { modIBmini.startingPlayerPositionY = 0; }
                }
                else //outdoors
                {
                    int yLoc = (int)(((GetFieldByLabel(gff.TopLevelStruct, "Mod_Entry_Y").ValueFloat + 0.5f) / 5f) - 16);
                    //need to invert the y value since IB2 measure top to bottom and nwn2 bottom to top so use MapSizeY - yLoc  
                    modIBmini.startingPlayerPositionY = area.MapSizeY - yLoc - 1;
                    if (modIBmini.startingPlayerPositionY < 0) { modIBmini.startingPlayerPositionY = 0; }
                }
            }

            //go through list of creatures and create Props
            foreach (GffField field in gff.TopLevelStruct.Fields)
            {
                string key = field.Label;
                //tag
                if (key.Equals("Creature List"))
                {
                    GffList valList = (GffList)field.Data;
                    foreach (GffStruct fld in valList.StructList)
                    {
                        Prop newItem = new Prop();
                        string firstname = "";
                        string lastname = "";
                        foreach (GffField field2 in fld.Fields)
                        {
                            string key2 = field2.Label;
                            //tag
                            if (key2.Equals("TemplateResRef"))
                            {
                                newItem.PropTag = field2.Data.ToString() + "_" + nextIndex.ToString();
                                nextIndex++;
                            }
                            //desc
                            else if (key2.Equals("Description"))
                            {
                                newItem.MouseOverText = field2.Data.ToString();
                            }
                            //firstname
                            else if (key2.Equals("FirstName"))
                            {
                                firstname = field2.Data.ToString();
                            }
                            //lastname
                            else if (key2.Equals("LastName"))
                            {
                                lastname = field2.Data.ToString();
                            }
                            //Xposition
                            else if (key2.Equals("XPosition"))
                            {
                                if (tileList.StructList.Count > 0) //indoors
                                {
                                    int xLoc = (int)(((field2.ValueFloat + 0.5f) / 1.5f) - 6);
                                    newItem.LocationX = xLoc - 1;
                                    if (newItem.LocationX < 0) { newItem.LocationX = 0; }
                                }
                                else //outdoors
                                {
                                    int xLoc = (int)(((field2.ValueFloat + 0.5f) / 5f) - 16);
                                    newItem.LocationX = xLoc - 1;
                                    if (newItem.LocationX < 0) { newItem.LocationX = 0; }
                                }
                                //nwn2 each square is 9x9 units so a 4x4 area is 36x36 units and all creatures will be located in the inner 2x2 space so (9,9) to (27,27)
                                //IB2 will assume each 6x6 squares are equal to 9x9 units in nwn2
                            }
                            //Yposition
                            else if (key2.Equals("YPosition"))
                            {
                                if (tileList.StructList.Count > 0) //indoors
                                {
                                    int yLoc = (int)(((field2.ValueFloat + 0.5f) / 1.5f) - 6);
                                    //need to invert the y value since IB2 measure top to bottom and nwn2 bottom to top so use MapSizeY - yLoc  
                                    newItem.LocationY = area.MapSizeY - yLoc - 1;
                                    if (newItem.LocationY < 0) { newItem.LocationY = 0; }
                                }
                                else //outdoors
                                {
                                    int yLoc = (int)(((field2.ValueFloat + 0.5f) / 5f) - 16);
                                    //need to invert the y value since IB2 measure top to bottom and nwn2 bottom to top so use MapSizeY - yLoc  
                                    newItem.LocationY = area.MapSizeY - yLoc - 1;
                                    if (newItem.LocationY < 0) { newItem.LocationY = 0; }
                                }
                                //nwn2 each square is 9x9 units so a 4x4 area is 36x36 units and all creatures will be located in the inner 2x2 space so (9,9) to (27,27)
                                //IB2 will assume each 6x6 squares are equal to 9x9 units in nwn2
                            }
                            //conversation
                            else if (key2.Equals("Conversation"))
                            {
                                newItem.ConversationWhenOnPartySquare = field2.Data.ToString();
                            }
                        }
                        newItem.PropName = firstname;
                        newItem.MouseOverText = firstname;
                        if (lastname.Length > 0)
                        {
                            newItem.PropName = firstname + " " + lastname;
                            newItem.MouseOverText = firstname + " " + lastname;
                        }
                        newItem.ImageFileName = "prp_captive";
                        area.Props.Add(newItem);
                    }
                }
            }
        }
        public void getTransitionsData(GffFile gff, GffFile gffARE, Area area)
        {
            //used to see if indoor or outdoor area
            GffList tileList = (GffList)GetFieldByLabel(gffARE.TopLevelStruct, "TileList").Data;

            //go through door list, waypoint list and trigger list and store data
            #region WayPoints
            GffList wpList = (GffList)GetFieldByLabel(gff.TopLevelStruct, "WaypointList").Data;
            foreach (GffStruct strct in wpList.StructList)
            {
                NwnWayPoint newWP = new NwnWayPoint();
                newWP.Tag = GetFieldByLabel(strct, "Tag").Data.ToString();
                newWP.InAreaName = area.Filename;
                
                //Xposition
                float xPos = GetFieldByLabel(strct, "XPosition").ValueFloat;                
                if (tileList.StructList.Count > 0) //indoors
                {
                    int xLoc = (int)(((xPos + 0.5f) / 1.5f) - 6);
                    newWP.XPositionInSquares = xLoc;
                }
                else //outdoors
                {
                    int xLoc = (int)(((xPos + 0.5f) / 5f) - 16);
                    newWP.XPositionInSquares = xLoc;
                }
                //nwn2 each square is 9x9 units so a 4x4 area is 36x36 units and all creatures will be located in the inner 2x2 space so (9,9) to (27,27)
                //IB2 will assume each 6x6 squares are equal to 9x9 units in nwn2
                
                //Yposition
                float yPos = GetFieldByLabel(strct, "YPosition").ValueFloat;                
                if (tileList.StructList.Count > 0) //indoors
                {
                    int yLoc = (int)(((yPos + 0.5f) / 1.5f) - 6);
                    //need to invert the y value since IB2 measure top to bottom and nwn2 bottom to top so use MapSizeY - yLoc  
                    newWP.YPositionInSquares = area.MapSizeY - yLoc;
                }
                else //outdoors
                {
                    int yLoc = (int)(((yPos + 0.5f) / 5f) - 16);
                    //need to invert the y value since IB2 measure top to bottom and nwn2 bottom to top so use MapSizeY - yLoc  
                    newWP.YPositionInSquares = area.MapSizeY - yLoc;
                }
                //nwn2 each square is 9x9 units so a 4x4 area is 36x36 units and all creatures will be located in the inner 2x2 space so (9,9) to (27,27)
                //IB2 will assume each 6x6 squares are equal to 9x9 units in nwn2    
                waypointList.Add(newWP);            
            }
            #endregion
            #region Triggers
            GffList trigList = (GffList)GetFieldByLabel(gff.TopLevelStruct, "TriggerList").Data;
            foreach (GffStruct strct in trigList.StructList)
            {
                //check if has a LinkedTo or skip
                if (GetFieldByLabel(strct, "LinkedTo").Data.ToString() == "") { continue; }

                NwnTrigger newTrig = new NwnTrigger();
                newTrig.Tag = GetFieldByLabel(strct, "Tag").Data.ToString();
                newTrig.InAreaName = area.Filename;
                newTrig.LinkedTo = GetFieldByLabel(strct, "LinkedTo").Data.ToString();

                //Xposition
                float xPos = GetFieldByLabel(strct, "XPosition").ValueFloat;
                if (tileList.StructList.Count > 0) //indoors
                {
                    int xLoc = (int)(((xPos + 0.5f) / 1.5f) - 6);
                    newTrig.XPositionInSquares = xLoc;
                }
                else //outdoors
                {
                    int xLoc = (int)(((xPos + 0.5f) / 5f) - 16);
                    newTrig.XPositionInSquares = xLoc;
                }
                //nwn2 each square is 9x9 units so a 4x4 area is 36x36 units and all creatures will be located in the inner 2x2 space so (9,9) to (27,27)
                //IB2 will assume each 6x6 squares are equal to 9x9 units in nwn2

                //Yposition
                float yPos = GetFieldByLabel(strct, "YPosition").ValueFloat;
                if (tileList.StructList.Count > 0) //indoors
                {
                    int yLoc = (int)(((yPos + 0.5f) / 1.5f) - 6);
                    //need to invert the y value since IB2 measure top to bottom and nwn2 bottom to top so use MapSizeY - yLoc  
                    newTrig.YPositionInSquares = area.MapSizeY - yLoc;
                }
                else //outdoors
                {
                    int yLoc = (int)(((yPos + 0.5f) / 5f) - 16);
                    //need to invert the y value since IB2 measure top to bottom and nwn2 bottom to top so use MapSizeY - yLoc  
                    newTrig.YPositionInSquares = area.MapSizeY - yLoc;
                }
                //nwn2 each square is 9x9 units so a 4x4 area is 36x36 units and all creatures will be located in the inner 2x2 space so (9,9) to (27,27)
                //IB2 will assume each 6x6 squares are equal to 9x9 units in nwn2   
                triggerList.Add(newTrig);                         
            }
            #endregion
            #region Doors
            GffList drList = (GffList)GetFieldByLabel(gff.TopLevelStruct, "Door List").Data;
            foreach (GffStruct strct in drList.StructList)
            {
                //check if has a LinkedTo or skip
                if (GetFieldByLabel(strct, "LinkedTo").Data.ToString() == "") { continue; }

                NwnDoor newDoor = new NwnDoor();
                newDoor.Tag = GetFieldByLabel(strct, "Tag").Data.ToString();
                newDoor.InAreaName = area.Filename;
                newDoor.LinkedTo = GetFieldByLabel(strct, "LinkedTo").Data.ToString();

                //Xposition
                float xPos = GetFieldByLabel(strct, "X").ValueFloat;
                if (tileList.StructList.Count > 0) //indoors
                {
                    int xLoc = (int)(((xPos + 0.5f) / 1.5f) - 6);
                    newDoor.XPositionInSquares = xLoc;
                }
                else //outdoors
                {
                    int xLoc = (int)(((xPos + 0.5f) / 5f) - 16);
                    newDoor.XPositionInSquares = xLoc;
                }
                //nwn2 each square is 9x9 units so a 4x4 area is 36x36 units and all creatures will be located in the inner 2x2 space so (9,9) to (27,27)
                //IB2 will assume each 6x6 squares are equal to 9x9 units in nwn2

                //Yposition
                float yPos = GetFieldByLabel(strct, "Y").ValueFloat;
                if (tileList.StructList.Count > 0) //indoors
                {
                    int yLoc = (int)(((yPos + 0.5f) / 1.5f) - 6);
                    //need to invert the y value since IB2 measure top to bottom and nwn2 bottom to top so use MapSizeY - yLoc  
                    newDoor.YPositionInSquares = area.MapSizeY - yLoc;
                }
                else //outdoors
                {
                    int yLoc = (int)(((yPos + 0.5f) / 5f) - 16);
                    //need to invert the y value since IB2 measure top to bottom and nwn2 bottom to top so use MapSizeY - yLoc  
                    newDoor.YPositionInSquares = area.MapSizeY - yLoc;
                }
                //nwn2 each square is 9x9 units so a 4x4 area is 36x36 units and all creatures will be located in the inner 2x2 space so (9,9) to (27,27)
                //IB2 will assume each 6x6 squares are equal to 9x9 units in nwn2   
                doorList.Add(newDoor);
            }
            #endregion
        }
        private Area createNewArea(GffFile gffARE)
        {
            //create tilemap
            Area area = new Area();

            foreach (GffField field in gffARE.TopLevelStruct.Fields)
            {
                string key = field.Label;
                if (key.Equals("ResRef"))
                {
                    string resref = field.Data.ToString();
                    area.Filename = resref.Replace(" ", "_");
                }
                //in game name
                else if (key.Equals("Name"))
                {
                    area.inGameAreaName = field.Data.ToString();
                }
                //width
                else if (key.Equals("Width"))
                {
                    GffList tileList = (GffList)GetFieldByLabel(gffARE.TopLevelStruct, "TileList").Data;
                    if (tileList.StructList.Count > 0) //indoors
                    {
                        area.MapSizeX = (int)((field.ValueInt * 6) - 12);
                    }
                    else //outdoors
                    {
                        area.MapSizeX = (int)(field.ValueInt * 2);
                    }
                    
                }
                //height
                else if (key.Equals("Height"))
                {
                    GffList tileList = (GffList)GetFieldByLabel(gffARE.TopLevelStruct, "TileList").Data;
                    if (tileList.StructList.Count > 0) //indoors
                    {
                        area.MapSizeY = (int)((field.ValueInt * 6) - 12);
                    }
                    else //outdoors
                    {
                        area.MapSizeY = (int)(field.ValueInt * 2);
                    }                    
                }
            }
            
            //setup blank area tiles
            for (int index = 0; index < (area.MapSizeX * area.MapSizeY); index++)
            {
                area.Layer1Filename.Add("t_grass");
                area.Layer1Rotate.Add(0);
                area.Layer1Mirror.Add(0);
                area.Layer2Filename.Add("t_blank");
                area.Layer2Rotate.Add(0);
                area.Layer2Mirror.Add(0);
                area.Layer3Filename.Add("t_blank");
                area.Layer3Rotate.Add(0);
                area.Layer3Mirror.Add(0);
                area.Walkable.Add(1);
                area.LoSBlocked.Add(0);
                area.Visible.Add(0);
            }

            //assign black tiles to non-tile spaces            
            GffList tiles = (GffList)GetFieldByLabel(gffARE.TopLevelStruct, "TileList").Data;
            if (tiles.StructList.Count > 0) //indoors
            {
                foreach (GffStruct strct in tiles.StructList)
                {
                    int appearance = GetFieldByLabel(strct, "Appearance").ValueInt;
                    GffStruct PositionStruct = (GffStruct)GetFieldByLabel(strct, "Position").Data;
                    float Xpos = GetFieldByLabel(PositionStruct, "x").ValueFloat;
                    float Ypos = GetFieldByLabel(PositionStruct, "y").ValueFloat;
                    int nwn2mapsizeX = GetFieldByLabel(gffARE.TopLevelStruct, "Width").ValueInt;
                    int nwn2mapsizeY = GetFieldByLabel(gffARE.TopLevelStruct, "Height").ValueInt;
                    int x = (int)((Xpos - 4.5f) / 9.0f);
                    int y = (nwn2mapsizeY - 1) - (int)((Ypos - 4.5f) / 9.0f); //area.MapSizeY - yLoc;
                    //skip if a border row or column
                    if ((x <= 0) || (y <= 0) || (x >= (nwn2mapsizeX - 1)) || (y >= (nwn2mapsizeY - 1))) { continue; }
                    if (appearance == 39)
                    {
                        int xx = x - 1;
                        int yy = y - 1;
                        //an NWN2 square is 6x6 IBmini squares...skip outer border NWN2 squares
                        for (int ibx = 0; ibx < 6; ibx++)
                        {
                            for (int iby = 0; iby < 6; iby++)
                            {
                                int x2 = (xx * 6) + ibx;
                                int y2 = (yy * 6) + iby;
                                area.Layer2Filename[y2 * (area.MapSizeX) + x2] = "t_black_tile";
                            }
                        }                        
                    }
                }
            }

            return area;
        }
        public void convertJournal(GffFile gff)
        {
            foreach (GffField field in gff.TopLevelStruct.Fields)
            {
                string key0 = field.Label;
                if (key0.Equals("Categories"))
                {
                    GffList valList0 = (GffList)field.Data;
                    foreach (GffStruct fld0 in valList0.StructList)
                    {
                        JournalQuest newQuest = new JournalQuest();
                        foreach (GffField field20 in fld0.Fields)
                        {
                            string key = field20.Label;
                            //resref
                            if (key.Equals("Name"))
                            {
                                newQuest.Name = field20.Data.ToString();
                            }
                            //tag
                            else if (key.Equals("Tag"))
                            {
                                newQuest.Tag = field20.Data.ToString();
                            }
                            else if (key.Equals("EntryList"))
                            {
                                GffList valList = (GffList)field20.Data;
                                foreach (GffStruct fld in valList.StructList)
                                {
                                    #region Quests
                                    JournalEntry newEntry = new JournalEntry();
                                    newEntry.EntryTitle = newQuest.Name;
                                    foreach (GffField field2 in fld.Fields)
                                    {
                                        #region Entries
                                        string key2 = field2.Label;
                                        //EntryText
                                        if (key2.Equals("Text"))
                                        {
                                            newEntry.EntryText = field2.Data.ToString();
                                        }
                                        //EntryId (int)
                                        else if (key2.Equals("ID"))
                                        {
                                            newEntry.EntryId = field2.ValueInt;
                                            newEntry.Tag = field2.ValueInt.ToString();
                                        }
                                        //EndPoint (bool)
                                        else if (key2.Equals("End"))
                                        {
                                            int isEnd = field2.ValueByte;
                                            if (isEnd > 0)
                                            {
                                                newEntry.EndPoint = true;
                                            }
                                            else
                                            {
                                                newEntry.EndPoint = false;
                                            }
                                        }
                                        #endregion
                                    }
                                    newQuest.Entries.Add(newEntry);
                                    #endregion
                                }
                            }
                        }
                        modIBmini.moduleJournal.Add(newQuest);
                    }
                }
            }
        }

        public void setupAllTransitions()
        {
            //transitions from triggers
            foreach (NwnTrigger trig in triggerList)
            {
                //get this trigger's area object
                Area thisArea = getAreaByFilename(trig.InAreaName);
                if (thisArea == null) { continue; }
                Trigger newtrig = new Trigger();
                newtrig.TriggerTag = trig.Tag + trig.XPositionInSquares.ToString() + trig.YPositionInSquares.ToString();
                //trigger location in this area
                int x = trig.XPositionInSquares - 1;
                if (x < 0) { x = 0; }
                int y = trig.YPositionInSquares - 1;
                if (y < 0) { y = 0; }
                newtrig.TriggerSquaresList.Add(new Coordinate(x, y));
                //define trigger type as a transition
                newtrig.Event1Type = "transition";
                //find linkedto area name
                newtrig.Event1FilenameOrTag = getAreaNameOfDoorOrWaypointByTag(trig.LinkedTo);
                if (newtrig.Event1FilenameOrTag.Equals("")) { continue; }
                //find linkedto location
                newtrig.Event1TransPointX = getTransitionLocationOfDoorOrWaypointByTag(trig.LinkedTo).X;
                if (newtrig.Event1TransPointX == -1) { continue; } 
                newtrig.Event1TransPointY = getTransitionLocationOfDoorOrWaypointByTag(trig.LinkedTo).Y;

                thisArea.Triggers.Add(newtrig);                
            }
            //transitions from doors
            foreach (NwnDoor dr in doorList)
            {
                //get this trigger's area object
                Area thisArea = getAreaByFilename(dr.InAreaName);
                if (thisArea == null) { continue; }
                Trigger newtrig = new Trigger();
                newtrig.TriggerTag = dr.Tag + dr.XPositionInSquares.ToString() + dr.YPositionInSquares.ToString();
                //trigger location in this area
                int x = dr.XPositionInSquares - 1;
                if (x < 0) { x = 0; }
                int y = dr.YPositionInSquares - 1;
                if (y < 0) { y = 0; }
                newtrig.TriggerSquaresList.Add(new Coordinate(x, y));
                //define trigger type as a transition
                newtrig.Event1Type = "transition";
                //find linkedto area name
                newtrig.Event1FilenameOrTag = getAreaNameOfDoorOrWaypointByTag(dr.LinkedTo);
                if (newtrig.Event1FilenameOrTag.Equals("")) { continue; }
                //find linkedto location
                newtrig.Event1TransPointX = getTransitionLocationOfDoorOrWaypointByTag(dr.LinkedTo).X;
                if (newtrig.Event1TransPointX == -1) { continue; }
                newtrig.Event1TransPointY = getTransitionLocationOfDoorOrWaypointByTag(dr.LinkedTo).Y;

                thisArea.Triggers.Add(newtrig);
            }
        }
        public Area getAreaByFilename(string name)
        {
            foreach (Area a in modIBmini.moduleAreasObjects)
            {
                if (a.Filename.Equals(name))
                {
                    return a;
                }
            }
            return null;
        }
        public string getAreaNameOfDoorOrWaypointByTag(string tag)
        {
            foreach (NwnDoor d in doorList)
            {
                if (d.Tag.Equals(tag))
                {
                    return d.InAreaName;
                }                    
            }
            foreach (NwnWayPoint w in waypointList)
            {
                if (w.Tag.Equals(tag))
                {
                    return w.InAreaName;
                }
            }
            return "";
        }
        public Coordinate getTransitionLocationOfDoorOrWaypointByTag(string tag)
        {
            foreach (NwnDoor d in doorList)
            {
                if (d.Tag.Equals(tag))
                {
                    return new Coordinate(d.XPositionInSquares, d.YPositionInSquares);
                }
            }
            foreach (NwnWayPoint w in waypointList)
            {
                if (w.Tag.Equals(tag))
                {
                    return new Coordinate(w.XPositionInSquares, w.YPositionInSquares);
                }
            }
            return new Coordinate(-1,-1);
        }

        public void fillDlgLists(GffFile gff)
        {
            dlgStartingnodeList.Clear();
            dlgEntrynodeList.Clear();
            dlgReplynodeList.Clear();
            nextIdNumber = 1;

            foreach (GffField ibf in gff.TopLevelStruct.Fields)
            {
                if (ibf.Label.Equals("StartingList")) //add all startingList nodes, these are NPC nodes
                {
                    GffList thisList = (GffList)ibf.Data;
                    foreach (GffStruct istr in thisList.StructList) //this is a list of the root level entry nodes
                    {
                        //STRUCTS are containers of field(s), the fields have the actual data for this node
                        //these nodes will only have pointers (any data in SyncStruct actually) added at this time, later 
                        //we'll get the actual node data once the EntryList is populated with nodes
                        DlgSyncStruct newSyncStruct = new DlgSyncStruct();
                        newSyncStruct.Index = GetFieldByLabel(istr, "Index").ValueDword;
                        newSyncStruct.ShowOnce = false;
                        if (GetFieldByLabel(istr, "ShowOnce").ValueInt != 0)
                        {
                            newSyncStruct.ShowOnce = true;
                        }
                        newSyncStruct.IsChild = false;
                        if (GetFieldByLabel(istr, "IsChild").ValueInt != 0)
                        {
                            newSyncStruct.IsChild = true;
                        }
                        GffList conditionalList = (GffList)GetFieldByLabel(istr, "ActiveConditiona").Data;
                        foreach (GffStruct irepstr2 in conditionalList.StructList)
                        {
                            Condition newCondition = GetCondition(irepstr2, gff.Filename, newSyncStruct);
                            if (newCondition != null)
                            {
                                newSyncStruct.conditions.Add(newCondition);
                            }
                        }
                        dlgStartingnodeList.Add(newSyncStruct);
                    }
                }
                else if (ibf.Label.Equals("EntryList")) //these are all NPC nodes
                {
                    GffList thisList = (GffList)ibf.Data;
                    foreach (GffStruct istr in thisList.StructList)
                    {
                        //this node 'newEntryNode' will get its conditionals, showonce, isLink from its parent node's subNode list once it is assigned
                        ContentNode newEntryNode = new ContentNode();
                        newEntryNode.pcNode = false;
                        newEntryNode.idNum = nextIdNumber;
                        nextIdNumber++;
                        newEntryNode.conversationText = replaceText(GetFieldByLabel(istr, "Text").Data.ToString());
                        if ((newEntryNode.conversationText.Equals("")) || (newEntryNode.conversationText.Equals("||")))
                        {
                            newEntryNode.conversationText = "Continue";
                        }
                        if (!GetFieldByLabel(istr, "Speaker").Data.ToString().Equals(""))
                        {
                            string speaker = GetFieldByLabel(istr, "Speaker").Data.ToString();
                            newEntryNode.NodePortraitBitmap = "ptr_" + speaker;
                            if (!portraits_needed.Contains(newEntryNode.NodePortraitBitmap))
                            {
                                portraits_needed.Add(newEntryNode.NodePortraitBitmap);
                            }
                            newEntryNode.NodeNpcName = char.ToUpper(speaker[0]) + speaker.Substring(1);
                        }
                        
                        #region ScriptList
                        GffList scriptList = (GffList)GetFieldByLabel(istr, "ScriptList").Data;
                        foreach (GffStruct irepstr in scriptList.StructList)
                        {
                            Action newAction = GetAction(irepstr, gff.Filename, newEntryNode);
                            if (newAction != null)
                            {
                                newEntryNode.actions.Add(newAction);
                            }
                        }
                        #endregion
                        
                        #region RepliesList
                        GffList replyIndexList = (GffList)GetFieldByLabel(istr, "RepliesList").Data;
                        foreach (GffStruct irepstr in replyIndexList.StructList)
                        {
                            //ContentNode subNodeOnEntryNode = new ContentNode();
                            DlgSyncStruct newSyncStruct = new DlgSyncStruct();
                            newSyncStruct.Index = GetFieldByLabel(irepstr, "Index").ValueDword;
                            newSyncStruct.ShowOnce = false;
                            if (GetFieldByLabel(irepstr, "ShowOnce").ValueInt != 0)
                            {
                                newSyncStruct.ShowOnce = true;
                            }
                            newSyncStruct.IsChild = false;
                            if (GetFieldByLabel(irepstr, "IsChild").ValueInt != 0)
                            {
                                newSyncStruct.IsChild = true;
                            }
                            GffList conditionalList = (GffList)GetFieldByLabel(irepstr, "ActiveConditiona").Data;
                            foreach (GffStruct irepstr2 in conditionalList.StructList)
                            {
                                Condition newCondition = GetCondition(irepstr2, gff.Filename, newSyncStruct);
                                if (newCondition != null)
                                {
                                    newSyncStruct.conditions.Add(newCondition);
                                }
                            }
                            newEntryNode.syncStructs.Add(newSyncStruct);
                        }
                        #endregion
                        
                        dlgEntrynodeList.Add(newEntryNode);
                    }
                }
                else if (ibf.Label.Equals("ReplyList")) //These are all PC nodes
                {
                    GffList thisList = (GffList)ibf.Data;
                    foreach (GffStruct istr in thisList.StructList)
                    {
                        //this node 'newReplyNode' will get its conditionals, showonce, isLink from its parent node's subNode list once it is assigned
                        ContentNode newReplyNode = new ContentNode();
                        newReplyNode.pcNode = true;
                        newReplyNode.idNum = nextIdNumber;
                        nextIdNumber++;
                        newReplyNode.conversationText = replaceText(GetFieldByLabel(istr, "Text").Data.ToString());
                        if ((newReplyNode.conversationText.Equals("")) || (newReplyNode.conversationText.Equals("||")))
                        {
                            newReplyNode.conversationText = "Continue";
                        }

                        #region ScriptList
                        GffList scriptList = (GffList)GetFieldByLabel(istr, "ScriptList").Data;
                        foreach (GffStruct irepstr in scriptList.StructList)
                        {
                            Action newAction = GetAction(irepstr, gff.Filename, newReplyNode);
                            if (newAction != null)
                            {
                                newReplyNode.actions.Add(newAction);
                            }
                        }
                        #endregion

                        #region EntriesList
                        //this is the index of EntryList Dialog Struct
                        GffList replyIndexList = (GffList)GetFieldByLabel(istr, "EntriesList").Data;
                        foreach (GffStruct irepstr in replyIndexList.StructList)
                        {
                            //ContentNode subNodeOnReplyNode = new ContentNode();
                            DlgSyncStruct newSyncStruct = new DlgSyncStruct();
                            newSyncStruct.Index = GetFieldByLabel(irepstr, "Index").ValueDword;
                            newSyncStruct.ShowOnce = false;
                            if (GetFieldByLabel(irepstr, "ShowOnce").ValueInt != 0)
                            {
                                newSyncStruct.ShowOnce = true;
                            }
                            newSyncStruct.IsChild = false;
                            if (GetFieldByLabel(irepstr, "IsChild").ValueInt != 0)
                            {
                                newSyncStruct.IsChild = true;
                            }
                            GffList conditionalList = (GffList)GetFieldByLabel(irepstr, "ActiveConditiona").Data;
                            foreach (GffStruct irepstr2 in conditionalList.StructList)
                            {
                                Condition newCondition = GetCondition(irepstr2, gff.Filename, newSyncStruct);
                                if (newCondition != null)
                                {
                                    newSyncStruct.conditions.Add(newCondition);
                                }
                            }
                            newReplyNode.syncStructs.Add(newSyncStruct);
                        }
                        #endregion

                        dlgReplynodeList.Add(newReplyNode);
                    }
                }
            }
        }
        public Convo makeIbCon(string filename)
        {
            //THE BASICS
            //the DialogStruct data will be merged with the SyncStruct data on the parent node's subNodes by 
            //going through each parent subNode, using the pointer to get the actual DialogStruct data and adding that
            //data to the subNode.            
            Convo newConvo = new Convo();
            newConvo.ConvoFileName = filename;
            newConvo.DefaultNpcName = filename;
            newConvo.NpcPortraitBitmap = "ptr_" + filename;
            if (!portraits_needed.Contains(newConvo.NpcPortraitBitmap))
            {
                portraits_needed.Add(newConvo.NpcPortraitBitmap);
            }

            //this is the root node of the convo, all the StartingList nodes will be subnodes of this rootNode
            ContentNode rootNode = new ContentNode();
            rootNode.idNum = 0;
            rootNode.conversationText = "root";
            //nodes in the StartingList, these nodes are added to the rootNode after they find (pointers) and load their subnodes (all branches recursively)
            foreach (DlgSyncStruct startingListSync in dlgStartingnodeList)
            {
                ContentNode subNodeOfRootNode = dlgEntrynodeList[(int)startingListSync.Index].DeepCopy();
                //add syncstruct data to this node
                //subNodeOfRootNode.isLink = startingListSync.IsChild;
                subNodeOfRootNode.ShowOnlyOnce = startingListSync.ShowOnce;
                subNodeOfRootNode.conversationText += startingListSync.scriptInfoForEndOfText;
                //add all conditionals in sync to this above node as well
                foreach (Condition cond in startingListSync.conditions)
                {
                    subNodeOfRootNode.conditions.Add(cond.DeepCopy());
                }

                if (!startingListSync.IsChild) //node is NOT a link so add all subNodes
                {
                    foreach (DlgSyncStruct sync2 in subNodeOfRootNode.syncStructs) //pointers to nodes in 
                    {
                        subNodeOfRootNode.subNodes.Add(addIbContentNode(dlgReplynodeList[(int)sync2.Index], sync2));
                    }
                    if (subNodeOfRootNode.syncStructs.Count == 0) //no subnodes on a NPC node so add an [END DIALOG] PC node
                    {
                        ContentNode subNode = new ContentNode();
                        subNode.idNum = nextIdNumber;
                        nextIdNumber++;
                        subNode.conversationText = "[END DIALOG]";
                        subNodeOfRootNode.subNodes.Add(subNode);
                    }
                }
                else //is a link so create a blank node with a idNum incremented start at 100000 and assign the link to ID
                {
                    //make a blank node
                    subNodeOfRootNode = new ContentNode();
                    //add syncstruct data to this node
                    //subNodeOfRootNode.isLink = startingListSync.IsChild;
                    subNodeOfRootNode.ShowOnlyOnce = startingListSync.ShowOnce;
                    subNodeOfRootNode.conversationText += startingListSync.scriptInfoForEndOfText;
                    //assign LinkIdNumber
                    subNodeOfRootNode.idNum = nextLinkIdNumber;
                    nextLinkIdNumber++;
                    //assign linkTo
                    if (startingListSync.Index < dlgEntrynodeList.Count)
                    {
                        subNodeOfRootNode.linkTo = dlgEntrynodeList[(int)startingListSync.Index].idNum;
                    }
                    else //broken or bad link so comment
                    {
                        subNodeOfRootNode.linkTo = -1;
                        subNodeOfRootNode.conversationText = "[BROKEN LINK:]";
                    }
                }
                rootNode.subNodes.Add(subNodeOfRootNode);
            }
            newConvo.subNodes.Add(rootNode);
            return newConvo;
        }
        public ContentNode addIbContentNode(ContentNode thisNode, DlgSyncStruct thisSync)
        {
            ContentNode newNode = thisNode.DeepCopy(); //does not copy the subnodes or pointers
            //need to add sync stuff to this newNode
            //newNode.isLink = thisSync.IsChild;
            newNode.ShowOnlyOnce = thisSync.ShowOnce;
            newNode.conversationText += thisSync.scriptInfoForEndOfText;
            foreach (Condition cond in thisSync.conditions)
            {
                newNode.conditions.Add(cond.DeepCopy());
            }
            if (thisSync.IsChild) //Is a Link node
            {
                if (newNode.pcNode) //PC node
                {
                    //assign LinkIdNumber
                    newNode.idNum = nextLinkIdNumber;
                    nextLinkIdNumber++;
                    //assign linkTo
                    if (thisSync.Index < dlgEntrynodeList.Count)
                    {
                        newNode.linkTo = dlgEntrynodeList[(int)thisSync.Index].idNum;
                    }
                    else //broken or bad link so comment
                    {
                        newNode.linkTo = -1;
                        newNode.conversationText += "[BROKEN LINK:]";
                    }
                }
                else //NPC node
                {
                    //assign LinkIdNumber
                    newNode.idNum = nextLinkIdNumber;
                    nextLinkIdNumber++;
                    //assign linkTo
                    if (thisSync.Index < dlgReplynodeList.Count)
                    {
                        newNode.linkTo = dlgReplynodeList[(int)thisSync.Index].idNum;
                    }
                    else //broken or bad link so comment
                    {
                        newNode.linkTo = -1;
                        newNode.conversationText += "[BROKEN LINK:]";
                    }
                }
                return newNode;
            }
            if (newNode.syncStructs.Count == 0) //no subnodes on a NPC node so add an [END DIALOG] PC node
            {
                if (!newNode.pcNode) //NPC node
                {
                    ContentNode subNode = new ContentNode();
                    subNode.idNum = nextIdNumber;
                    nextIdNumber++;
                    subNode.conversationText = "[END DIALOG]";
                    newNode.subNodes.Add(subNode);
                    return newNode;
                }
            }
            foreach (DlgSyncStruct s in newNode.syncStructs)
            {
                if (newNode.pcNode) //PC node
                {
                    if (s.IsChild) //node is a link
                    {
                        //make a blank node
                        ContentNode subNodeOfNewNode = new ContentNode();
                        //add syncstruct data to this node
                        //subNodeOfNewNode.isLink = s.IsChild;
                        subNodeOfNewNode.ShowOnlyOnce = s.ShowOnce;
                        subNodeOfNewNode.conversationText += s.scriptInfoForEndOfText;
                        //assign LinkIdNumber
                        subNodeOfNewNode.idNum = nextLinkIdNumber;
                        nextLinkIdNumber++;
                        //assign linkTo
                        if (s.Index < dlgEntrynodeList.Count)
                        {
                            subNodeOfNewNode.linkTo = dlgEntrynodeList[(int)s.Index].idNum;
                        }
                        else //broken or bad link so comment
                        {
                            subNodeOfNewNode.linkTo = -1;
                            subNodeOfNewNode.conversationText = "[BROKEN LINK:]";
                        }
                        newNode.subNodes.Add(subNodeOfNewNode);
                    }
                    else
                    {
                        newNode.subNodes.Add(addIbContentNode(dlgEntrynodeList[(int)s.Index], s));
                    }
                }
                else //NPC node
                {
                    if (s.IsChild) //node is a link
                    {
                        //make a blank node
                        ContentNode subNodeOfNewNode = new ContentNode();
                        //add syncstruct data to this node
                        //subNodeOfNewNode.isLink = s.IsChild;
                        subNodeOfNewNode.ShowOnlyOnce = s.ShowOnce;
                        subNodeOfNewNode.conversationText += s.scriptInfoForEndOfText;
                        //assign LinkIdNumber
                        subNodeOfNewNode.idNum = nextLinkIdNumber;
                        nextLinkIdNumber++;
                        //assign linkTo
                        if (s.Index < dlgReplynodeList.Count)
                        {
                            subNodeOfNewNode.linkTo = dlgReplynodeList[(int)s.Index].idNum;
                        }
                        else //broken or bad link so comment
                        {
                            subNodeOfNewNode.linkTo = -1;
                            subNodeOfNewNode.conversationText = "[BROKEN LINK:]";
                        }
                        newNode.subNodes.Add(subNodeOfNewNode);
                    }
                    else
                    {
                        newNode.subNodes.Add(addIbContentNode(dlgReplynodeList[(int)s.Index], s));
                    }
                }
            }
            return newNode;
        }
        public Action GetAction(GffStruct thisStruct, string filename, ContentNode newEntryNode)
        {
            Action newAction = new Action();
            string scriptName = GetFieldByLabel(thisStruct, "Script").Data.ToString();
            if (scriptName.Equals(""))
            {
                return null;
            }
            if (scriptName.Equals("ga_global_int"))
            {
                newAction.a_script = "gaSetGlobalInt.cs";
                newAction.a_parameter_1 = GetParameterDataByFieldLabelAndNumber(thisStruct, 1);
                newAction.a_parameter_2 = GetParameterDataByFieldLabelAndNumber(thisStruct, 2);
            }
            else if (scriptName.Equals("ga_local_int"))
            {
                newAction.a_script = "gaSetLocalInt.cs";
                string parm1 = GetParameterDataByFieldLabelAndNumber(thisStruct, 3);
                if ((parm1.Equals("")) || (parm1.Equals("OBJECT_SELF")))
                {
                    newAction.a_parameter_1 = "thisprop";
                }
                else
                {
                    newAction.a_parameter_1 = GetParameterDataByFieldLabelAndNumber(thisStruct, 3);
                }
                newAction.a_parameter_2 = GetParameterDataByFieldLabelAndNumber(thisStruct, 1);
                string parm3 = GetParameterDataByFieldLabelAndNumber(thisStruct, 2);
                if (parm3.StartsWith("="))
                {
                    newAction.a_parameter_3 = parm3.Substring(1);
                }
                else
                {
                    newAction.a_parameter_3 = parm3;
                }
            }
            else if (scriptName.Equals("ga_give_xp"))
            {
                newAction.a_script = "gaGiveXP.cs";
                newAction.a_parameter_1 = GetParameterDataByFieldLabelAndNumber(thisStruct, 1);
                newAction.a_parameter_2 = GetParameterDataByFieldLabelAndNumber(thisStruct, 2);
                newAction.a_parameter_3 = GetParameterDataByFieldLabelAndNumber(thisStruct, 3);
                newAction.a_parameter_4 = GetParameterDataByFieldLabelAndNumber(thisStruct, 4);
            }
            else if (scriptName.Equals("ga_roster_add_object"))
            {
                newAction.a_script = "gaAddPartyMember.cs";
                newAction.a_parameter_1 = GetParameterDataByFieldLabelAndNumber(thisStruct, 1);
                if (!possibleCompanions.Contains(newAction.a_parameter_1)) { possibleCompanions.Add(newAction.a_parameter_1); }
            }
            else if (scriptName.Equals("ga_give_gold"))
            {
                newAction.a_script = "gaGiveGold.cs";
                newAction.a_parameter_1 = GetParameterDataByFieldLabelAndNumber(thisStruct, 1);
            }
            else if (scriptName.Equals("ga_take_gold"))
            {
                newAction.a_script = "gaTakeGold.cs";
                newAction.a_parameter_1 = GetParameterDataByFieldLabelAndNumber(thisStruct, 1);
            }
            else if (scriptName.Equals("ga_open_store"))
            {
                newAction.a_script = "gaOpenShopByTag.cs";
                newAction.a_parameter_1 = GetParameterDataByFieldLabelAndNumber(thisStruct, 1);
                newAction.a_parameter_2 = GetParameterDataByFieldLabelAndNumber(thisStruct, 2);
                newAction.a_parameter_3 = GetParameterDataByFieldLabelAndNumber(thisStruct, 3);
                newAction.a_parameter_4 = GetParameterDataByFieldLabelAndNumber(thisStruct, 4);
            }
            else if (scriptName.Equals("ga_give_item"))
            {
                newAction.a_script = "gaGiveItem.cs";
                newAction.a_parameter_1 = GetParameterDataByFieldLabelAndNumber(thisStruct, 1);
                newAction.a_parameter_2 = GetParameterDataByFieldLabelAndNumber(thisStruct, 2);
            }
            else if ((scriptName.Equals("ga_destroy_item")) || (scriptName.Equals("ga_take_item")))
            {
                newAction.a_script = "gaTakeItem.cs";
                newAction.a_parameter_1 = GetParameterDataByFieldLabelAndNumber(thisStruct, 1);
                newAction.a_parameter_2 = GetParameterDataByFieldLabelAndNumber(thisStruct, 2);
            }
            else if (scriptName.Equals("ga_journal"))
            {
                newAction.a_script = "gaAddJournalEntryByTag.cs";
                newAction.a_parameter_1 = GetParameterDataByFieldLabelAndNumber(thisStruct, 1);
                newAction.a_parameter_2 = GetParameterDataByFieldLabelAndNumber(thisStruct, 2);
            }
            else if (scriptName.Equals("ga_jump_players"))
            {
                newAction.a_script = "gaTransitionPartyToMapLocation.cs";
                string ar = getAreaNameOfDoorOrWaypointByTag(GetParameterDataByFieldLabelAndNumber(thisStruct, 1));
                if (ar.Equals("")) { return null; }
                int x = getTransitionLocationOfDoorOrWaypointByTag(GetParameterDataByFieldLabelAndNumber(thisStruct, 1)).X;
                if (x == -1) { return null; }
                int y = getTransitionLocationOfDoorOrWaypointByTag(GetParameterDataByFieldLabelAndNumber(thisStruct, 1)).Y;
                newAction.a_parameter_1 = ar;
                newAction.a_parameter_2 = x.ToString();
                newAction.a_parameter_3 = y.ToString();
            }
            else
            {
                string scriptInfo = "|***" + scriptName + "(nonIBscript)" + " --> ";
                scriptInfo += "::p1 = " + GetParameterDataByFieldLabelAndNumber(thisStruct, 1) + "    ";
                scriptInfo += "::p2 = " + GetParameterDataByFieldLabelAndNumber(thisStruct, 2) + "    ";
                scriptInfo += "::p3 = " + GetParameterDataByFieldLabelAndNumber(thisStruct, 3) + "    ";
                scriptInfo += "::p4 = " + GetParameterDataByFieldLabelAndNumber(thisStruct, 4) + "    ";
                scripts_needed.Add("|" + filename + scriptInfo);
                if (!scriptTypesNeeded.Contains(scriptName)) { scriptTypesNeeded.Add(scriptName); }
                newEntryNode.conversationText += scriptInfo;
                return null;
            }
            return newAction;
        }
        public Condition GetCondition(GffStruct thisStruct, string filename, DlgSyncStruct newSyncStruct)
        {
            Condition newCondition = new Condition();
            newCondition.c_not = false;
            if (GetFieldByLabel(thisStruct, "Not").ValueInt != 0)
            {
                newCondition.c_not = true;
            }
            newCondition.c_and = false;
            if (GetFieldByLabel(thisStruct, "And").ValueInt != 0)
            {
                newCondition.c_and = true;
            }
            
            string scriptName = GetFieldByLabel(thisStruct, "Script").Data.ToString();
            if (scriptName.Equals(""))
            {
                return null;
            }
            if (scriptName.Equals("gc_global_int"))
            {
                newCondition.c_script = "gcCheckGlobalInt.cs";
                newCondition.c_parameter_1 = GetParameterDataByFieldLabelAndNumber(thisStruct, 1);
                string parm2 = GetParameterDataByFieldLabelAndNumber(thisStruct, 2);
                if (parm2.StartsWith("="))
                {
                    newCondition.c_parameter_2 = "=";
                    newCondition.c_parameter_3 = parm2.Substring(1);
                }
                else if (parm2.StartsWith("<"))
                {
                    newCondition.c_parameter_2 = "<";
                    newCondition.c_parameter_3 = parm2.Substring(1);
                }
                else if (parm2.StartsWith(">"))
                {
                    newCondition.c_parameter_2 = ">";
                    newCondition.c_parameter_3 = parm2.Substring(1);
                }
                else if (parm2.StartsWith("!"))
                {
                    newCondition.c_parameter_2 = "!";
                    newCondition.c_parameter_3 = parm2.Substring(1);
                }
                else
                {
                    newCondition.c_parameter_2 = "=";
                    newCondition.c_parameter_3 = parm2;
                }
            }
            else if (scriptName.Equals("gc_local_int"))
            {
                newCondition.c_script = "gcCheckLocalInt.cs";
                string parm1 = GetParameterDataByFieldLabelAndNumber(thisStruct, 3);
                if ((parm1.Equals("")) || (parm1.Equals("OBJECT_SELF")))
                {
                    newCondition.c_parameter_1 = "thisprop";
                }
                else
                {
                    newCondition.c_parameter_1 = GetParameterDataByFieldLabelAndNumber(thisStruct, 3);
                }
                newCondition.c_parameter_2 = GetParameterDataByFieldLabelAndNumber(thisStruct, 1);                
                string parm3 = GetParameterDataByFieldLabelAndNumber(thisStruct, 2);
                if (parm3.StartsWith("="))
                {
                    newCondition.c_parameter_3 = "=";
                    newCondition.c_parameter_4 = parm3.Substring(1);
                }
                else if (parm3.StartsWith("<"))
                {
                    newCondition.c_parameter_3 = "<";
                    newCondition.c_parameter_4 = parm3.Substring(1);
                }
                else if (parm3.StartsWith(">"))
                {
                    newCondition.c_parameter_3 = ">";
                    newCondition.c_parameter_4 = parm3.Substring(1);
                }
                else if (parm3.StartsWith("!"))
                {
                    newCondition.c_parameter_3 = "!";
                    newCondition.c_parameter_4 = parm3.Substring(1);
                }
                else
                {
                    newCondition.c_parameter_3 = "=";
                    newCondition.c_parameter_4 = parm3;
                }
            }
            else if ((scriptName.Equals("gc_check_class")) || (scriptName.Equals("gc_class_level")))
            {
                //Example: to check if the current party leader PC is a ranger (-1, ranger, 1)
                //         to check if the main PC is a level 4 or greater wizard (0, wizard, 4)
                newCondition.c_script = "gcCheckIsClassLevel.cs";
                newCondition.c_parameter_1 = "-1";
                int pcClass = -1;
                try
                {
                    pcClass = Convert.ToInt32(GetParameterDataByFieldLabelAndNumber(thisStruct, 1));
                }
                catch { }
                if (pcClass == 0) { newCondition.c_parameter_2 = "barbarian"; }
                else if (pcClass == 1) { newCondition.c_parameter_2 = "bard"; }
                else if (pcClass == 2) { newCondition.c_parameter_2 = "cleric"; }
                else if (pcClass == 3) { newCondition.c_parameter_2 = "druid"; }
                else if (pcClass == 4) { newCondition.c_parameter_2 = "fighter"; }
                else if (pcClass == 5) { newCondition.c_parameter_2 = "monk"; }
                else if (pcClass == 6) { newCondition.c_parameter_2 = "paladin"; }
                else if (pcClass == 7) { newCondition.c_parameter_2 = "ranger"; }
                else if (pcClass == 8) { newCondition.c_parameter_2 = "thief"; }
                else if ((pcClass == 9) || (pcClass == 10) || (pcClass == 39)) { newCondition.c_parameter_2 = "wizard"; }

                string parm3 = GetParameterDataByFieldLabelAndNumber(thisStruct, 2);
                if (parm3.StartsWith(">"))
                {
                    newCondition.c_parameter_3 = parm3.Substring(1);
                }
                else
                {
                    newCondition.c_parameter_3 = parm3;
                }
                newCondition.c_parameter_4 = GetParameterDataByFieldLabelAndNumber(thisStruct, 3);
            }
            else if (scriptName.Equals("gc_check_race_pc"))
            {
                newCondition.c_script = "gcCheckIsRace.cs";
                newCondition.c_parameter_1 = "-1";
                int race = -1;
                try
                {
                    race = Convert.ToInt32(GetParameterDataByFieldLabelAndNumber(thisStruct, 1));
                }
                catch { }
                if (race == 1) { newCondition.c_parameter_2 = "dwarf"; }
                else if (race == 2) { newCondition.c_parameter_2 = "elf"; }
                else if (race == 3) { newCondition.c_parameter_2 = "gnome"; }
                else if (race == 4) { newCondition.c_parameter_2 = "halfelf"; }
                else if (race == 5) { newCondition.c_parameter_2 = "halfling"; }
                else if (race == 6) { newCondition.c_parameter_2 = "halforc"; }
                else if (race == 7) { newCondition.c_parameter_2 = "human"; }
                else if (race == 8) { newCondition.c_parameter_2 = "outsider"; }
                else newCondition.c_parameter_2 = "nonIB_race:" + GetParameterDataByFieldLabelAndNumber(thisStruct, 1);
            }
            else if (scriptName.Equals("gc_check_stats"))
            {
                //str 0, dex 1, con 2, int 3, wis 4, cha 5
                newCondition.c_script = "gcCheckAttribute.cs";
                newCondition.c_parameter_1 = "-1";
                int att = -1;
                try
                {
                    att = Convert.ToInt32(GetParameterDataByFieldLabelAndNumber(thisStruct, 1));
                }
                catch { }
                if (att == 0) { newCondition.c_parameter_2 = "str"; }
                else if (att == 1) { newCondition.c_parameter_2 = "dex"; }
                else if (att == 2) { newCondition.c_parameter_2 = "con"; }
                else if (att == 3) { newCondition.c_parameter_2 = "int"; }
                else if (att == 4) { newCondition.c_parameter_2 = "wis"; }
                else if (att == 5) { newCondition.c_parameter_2 = "cha"; }
                int val = -1;
                try
                {
                    val = Convert.ToInt32(GetParameterDataByFieldLabelAndNumber(thisStruct, 2));
                }
                catch { }
                newCondition.c_parameter_3 = ">";
                newCondition.c_parameter_4 = (val - 1).ToString();
            }
            else if (scriptName.Equals("gc_is_in_party"))
            {
                newCondition.c_script = "gcCheckPcInPartyByName.cs";
                newCondition.c_parameter_1 = GetParameterDataByFieldLabelAndNumber(thisStruct, 1);
            }
            else if (scriptName.Equals("gc_is_male"))
            {
                newCondition.c_script = "gcCheckIsMale.cs";
                newCondition.c_parameter_1 = "-1";
            }
            else if (scriptName.Equals("gc_rand_1of"))
            {
                newCondition.c_script = "gcRand1of.cs";
                newCondition.c_parameter_1 = GetParameterDataByFieldLabelAndNumber(thisStruct, 1);
            }
            else if (scriptName.Equals("gc_skill_rank"))
            {
                newCondition.c_script = "gcCheckHasTrait.cs";
                newCondition.c_parameter_1 = "-1";
                int skill = -1;
                int rank = -1;
                try
                {
                    skill = Convert.ToInt32(GetParameterDataByFieldLabelAndNumber(thisStruct, 2));
                }
                catch { }
                try
                {
                    rank = Convert.ToInt32(GetParameterDataByFieldLabelAndNumber(thisStruct, 3));
                }
                catch { }
                /*skill ints
                0   APPRAISE
                1   BLUFF
                2   CONCENTRATION
                3   CRAFT ALCHEMY
                4   CRAFT ARMOR
                5   CRAFT WEAPON
                6   DIPLOMACY
                7   DISABLE DEVICE
                8   DISCIPLINE
                9   HEAL
                10  HIDE
                11  INTIMIDATE
                12  LISTEN
                13  LORE
                14  MOVE SILENTLY
                15  OPEN LOCK
                16  PARRY
                17  PERFORM
                18  RIDE
                19  SEARCH
                20  CRAFT TRAP
                21  SLEIGHT OF HAND
                22  SPELL CRAFT
                23  SPOT
                24  SURVIVAL
                25  TAUNT
                26  TUMBLE
                27  USE MAGIC DEVICE*/
                if ((skill == 1) && (rank < 5)) { newCondition.c_parameter_2 = "bluff"; }
                else if ((skill == 1) && (rank >= 5)) { newCondition.c_parameter_2 = "bluff2"; }
                else if ((skill == 6) && (rank < 5)) { newCondition.c_parameter_2 = "diplomacy"; }
                else if ((skill == 6) && (rank >= 5)) { newCondition.c_parameter_2 = "diplomacy2"; }
                else if (((skill == 7) || (skill == 15)) && (rank < 5)) { newCondition.c_parameter_2 = "disarmdevice"; }
                else if (((skill == 7) || (skill == 15)) && (rank >= 5)) { newCondition.c_parameter_2 = "disarmdevice2"; }
                else if ((skill == 11) && (rank < 5)) { newCondition.c_parameter_2 = "intimidate"; }
                else if ((skill == 11) && (rank >= 5)) { newCondition.c_parameter_2 = "intimidate2"; }
                else if ((skill == 21) && (rank < 5)) { newCondition.c_parameter_2 = "pickpocket"; }
                else if ((skill == 21) && (rank >= 5)) { newCondition.c_parameter_2 = "pickpocket2"; }
                else if (((skill == 23) || (skill == 19)) && (rank < 5)) { newCondition.c_parameter_2 = "spot"; }
                else if (((skill == 23) || (skill == 19)) && (rank >= 5)) { newCondition.c_parameter_2 = "spot2"; }
                else if (((skill == 14) || (skill == 10)) && (rank < 5)) { newCondition.c_parameter_2 = "stealth"; }
                else if (((skill == 14) || (skill == 10)) && (rank >= 5)) { newCondition.c_parameter_2 = "stealth2"; }
                else newCondition.c_parameter_2 = "nonIB_skill:" + skill.ToString();
            }
            else if (scriptName.Equals("gc_skill_dc"))
            {
                newCondition.c_script = "gcPassSkillCheck.cs";
                newCondition.c_parameter_1 = "-1";
                int skill = -1;
                try
                {
                    skill = Convert.ToInt32(GetParameterDataByFieldLabelAndNumber(thisStruct, 1));
                }
                catch { }
                /*skill ints
                0   APPRAISE
                1   BLUFF
                2   CONCENTRATION
                3   CRAFT ALCHEMY
                4   CRAFT ARMOR
                5   CRAFT WEAPON
                6   DIPLOMACY
                7   DISABLE DEVICE
                8   DISCIPLINE
                9   HEAL
                10  HIDE
                11  INTIMIDATE
                12  LISTEN
                13  LORE
                14  MOVE SILENTLY
                15  OPEN LOCK
                16  PARRY
                17  PERFORM
                18  RIDE
                19  SEARCH
                20  CRAFT TRAP
                21  SLEIGHT OF HAND
                22  SPELL CRAFT
                23  SPOT
                24  SURVIVAL
                25  TAUNT
                26  TUMBLE
                27  USE MAGIC DEVICE*/
                if (skill == 1) { newCondition.c_parameter_2 = "bluff"; }
                else if (skill == 6) { newCondition.c_parameter_2 = "diplomacy"; }
                else if ((skill == 7) || (skill == 15)) { newCondition.c_parameter_2 = "disarmdevice"; }
                else if (skill == 11) { newCondition.c_parameter_2 = "intimidate"; }
                else if (skill == 21) { newCondition.c_parameter_2 = "pickpocket"; }
                else if ((skill == 23) || (skill == 19)) { newCondition.c_parameter_2 = "spot"; }
                else if ((skill == 14) || (skill == 10)) { newCondition.c_parameter_2 = "stealth"; }
                else newCondition.c_parameter_2 = "nonIB_skill:" + skill.ToString();
                newCondition.c_parameter_3 = GetParameterDataByFieldLabelAndNumber(thisStruct, 2);
            }
            else if (scriptName.Equals("gc_check_gold"))
            {
                newCondition.c_script = "gcCheckForGold.cs";
                newCondition.c_parameter_1 = GetParameterDataByFieldLabelAndNumber(thisStruct, 1);
                newCondition.c_parameter_2 = GetParameterDataByFieldLabelAndNumber(thisStruct, 2);
                newCondition.c_parameter_3 = GetParameterDataByFieldLabelAndNumber(thisStruct, 3);
                newCondition.c_parameter_4 = GetParameterDataByFieldLabelAndNumber(thisStruct, 4);
            }
            else if (scriptName.Equals("gc_check_item"))
            {
                newCondition.c_script = "gcCheckForItem.cs";
                newCondition.c_parameter_1 = GetParameterDataByFieldLabelAndNumber(thisStruct, 1);
                newCondition.c_parameter_2 = GetParameterDataByFieldLabelAndNumber(thisStruct, 2);
                newCondition.c_parameter_3 = GetParameterDataByFieldLabelAndNumber(thisStruct, 3);
                newCondition.c_parameter_4 = GetParameterDataByFieldLabelAndNumber(thisStruct, 4);
            }
            else if (scriptName.Equals("gc_journal_entry"))
            {
                newCondition.c_script = "gcCheckJournalEntryByTag.cs";
                newCondition.c_parameter_1 = GetParameterDataByFieldLabelAndNumber(thisStruct, 1);
                string parm2 = GetParameterDataByFieldLabelAndNumber(thisStruct, 2);
                if (parm2.StartsWith("="))
                {
                    newCondition.c_parameter_2 = "=";
                    newCondition.c_parameter_3 = parm2.Substring(1);
                }
                else if (parm2.StartsWith("<"))
                {
                    newCondition.c_parameter_2 = "<";
                    newCondition.c_parameter_3 = parm2.Substring(1);
                }
                else if (parm2.StartsWith(">"))
                {
                    newCondition.c_parameter_2 = ">";
                    newCondition.c_parameter_3 = parm2.Substring(1);
                }
                else if (parm2.StartsWith("!"))
                {
                    newCondition.c_parameter_2 = "!";
                    newCondition.c_parameter_3 = parm2.Substring(1);
                }
                else
                {
                    newCondition.c_parameter_2 = "=";
                    newCondition.c_parameter_3 = parm2;
                }
            }
            else
            {
                string scriptInfo = "|***" + scriptName + "(nonIBscript)" + " --> ";
                scriptInfo += "::p1 = " + GetParameterDataByFieldLabelAndNumber(thisStruct, 1) + "    ";
                scriptInfo += "::p2 = " + GetParameterDataByFieldLabelAndNumber(thisStruct, 2) + "    ";
                scriptInfo += "::p3 = " + GetParameterDataByFieldLabelAndNumber(thisStruct, 3) + "    ";
                scriptInfo += "::p4 = " + GetParameterDataByFieldLabelAndNumber(thisStruct, 4) + "    ";
                scripts_needed.Add("|" + filename + scriptInfo);
                if (!scriptTypesNeeded.Contains(scriptName)) { scriptTypesNeeded.Add(scriptName); }
                newSyncStruct.scriptInfoForEndOfText += scriptInfo;
                return null;
            }            
            return newCondition;
        }
        public GffField GetFieldByLabel(GffStruct thisStruct, string label)
        {
            foreach (GffField ifld in thisStruct.Fields)
            {
                if (ifld.Label.Equals(label))
                {
                    return ifld;
                }
            }
            MessageBox.Show("Couldn't find label: " + label + "in GffField");
            return null;
        }
        public string GetParameterDataByFieldLabelAndNumber(GffStruct thisStruct, int number)
        {            
            GffList parameterList = (GffList)GetFieldByLabel(thisStruct, "Parameters").Data;
            if (number <= parameterList.StructList.Count)
            {
                return GetFieldByLabel(parameterList.StructList[number - 1], "Parameter").Data.ToString();
            }
            //MessageBox.Show("Couldn't find parameter number: " + number + "in GffList of size: " + parameterList.StructList.Count);
            return "";
        }
        public string replaceText(string originalText)
        {
            string newString = originalText;
                        
            newString = newString.Replace("<color=#FFFFFF>", "<wh>");
            newString = newString.Replace("</color=#FFFFFF>", "</wh>");
            newString = newString.Replace("<color=#FF0000>", "<rd>");
            newString = newString.Replace("</color=#FF0000>", "</rd>");
            newString = newString.Replace("<color=#00FF00>", "<gn>");
            newString = newString.Replace("</color=#00FF00>", "</gn>");
            newString = newString.Replace("<color=#FFFF00>", "<yl>");
            newString = newString.Replace("</color=#FFFF00>", "</yl>");
            newString = newString.Replace("<i>", "");
            newString = newString.Replace("</i>", "");
            newString = newString.Replace("<b>", "");
            newString = newString.Replace("</b>", "");
            
            return newString;
        }
    }

    public class NwnWayPoint
    {
        public string Tag = "";
        public string InAreaName = "";
        public int XPositionInSquares = 0;
        public int YPositionInSquares = 0;

        public NwnWayPoint()
        {

        }
    }
    public class NwnTrigger
    {
        public string Tag = "";
        public string InAreaName = "";
        public int XPositionInSquares = 0;
        public int YPositionInSquares = 0;
        public string LinkedTo = "";

        public NwnTrigger()
        {

        }
    }
    public class NwnDoor
    {
        public string Tag = "";
        public string InAreaName = "";
        public int XPositionInSquares = 0;
        public int YPositionInSquares = 0;
        public string LinkedTo = "";

        public NwnDoor()
        {

        }
    }
}
