using System.Collections.Generic;
using System.IO;
using System.ComponentModel;
using Newtonsoft.Json;

namespace IB2ToolsetMini
{
    public class Module
    {
        #region Fields
        private string _moduleName = "";
        private string _moduleLabelName = "";
        public string titleImageName = "title";
        private int _moduleVersion = 1;
        public string saveName = "empty";
        private string _defaultPlayerFilename = "drin.json";
        private bool _mustUsePreMadePC = false;
        private int _numberOfPlayerMadePcsAllowed = 1;
        private int maxPartySize = 6;
        private string _moduleDescription = "";
        private string _moduleCredits = "<big><b>Lanterna - The Exile</b></big><BR><BR>"
                        + "-Story, coding and some art by <b>slowdive_fan</b><BR><BR>"
                        + "-Portrait art by Bree Arts<BR><BR>"
                        + "<i>(an IceBlink Engine conversion to Android)</i><BR><BR>"
                        + "visit the forums for more info:<BR>"
                        + "www.iceblinkengine.com/forums";
        private int _nextIdNumber = 100;
        private int worldTime = 0;
        public int TimePerRound = 6;
        public bool debugMode = true;
        public float diagonalMoveCost = 1.5f;
        public bool use3d6 = false;
        public bool ArmorClassAscending = true;
        public List<Item> moduleItemsList = new List<Item>();
        public List<Container> moduleContainersList = new List<Container>();
        public List<Shop> moduleShopsList = new List<Shop>();
        public List<Creature> moduleCreaturesList = new List<Creature>();
        public List<Prop> modulePropsList = new List<Prop>();
        public List<JournalQuest> moduleJournal = new List<JournalQuest>();
        public List<PlayerClass> modulePlayerClassList = new List<PlayerClass>();
        public List<Race> moduleRacesList = new List<Race>();
        public List<Spell> moduleSpellsList = new List<Spell>();
        public List<Trait> moduleTraitsList = new List<Trait>();
        public List<Effect> moduleEffectsList = new List<Effect>();
        public List<IBScript> moduleIBScriptList = new List<IBScript>();

        [JsonIgnore]
        public List<ImageData> moduleImageDataList = new List<ImageData>();
        [JsonIgnore]
        public List<Area> moduleAreasObjects = new List<Area>();
        [JsonIgnore]
        public List<Encounter> moduleEncountersList = new List<Encounter>();
        [JsonIgnore]
        public List<Convo> moduleConvoList = new List<Convo>();
        
        public List<GlobalInt> moduleGlobalInts = new List<GlobalInt>();
        public List<GlobalString> moduleGlobalStrings = new List<GlobalString>();
        public List<ConvoSavedValues> moduleConvoSavedValuesList = new List<ConvoSavedValues>();
        public List<GlobalListItem> ModuleGlobalListItems = new List<GlobalListItem>();
        public List<LocalListItem> ModuleLocalListItems = new List<LocalListItem>();
        private string _startingArea = "";
        private int _startingPlayerPositionX = 0;
        private int _startingPlayerPositionY = 0;
        public int playerLocationX = 0;
        public int playerLocationY = 0;
        public int playerLastLocationX = 0;
        public int playerLastLocationY = 0;
        private int _partyGold = 0;
        public bool showPartyToken = false;
        private string _partyTokenFilename = "prp_party";
        public List<Player> playerList = new List<Player>();
        public List<Player> companionPlayerList = new List<Player>();
        public List<ItemRefs> partyInventoryRefsList = new List<ItemRefs>();
        public List<JournalQuest> partyJournalQuests = new List<JournalQuest>();
        public List<JournalQuest> partyJournalCompleted = new List<JournalQuest>();
        public string partyJournalNotes = "";
        public int selectedPartyLeader = 0;
        public bool returnCheck = false;
        public bool addPCScriptFired = false;
        public bool uncheckConvo = false;
        public bool removeCreature = false;
        public bool deleteItemUsedScript = false;
        public int indexOfPCtoLastUseItem = 0;
        public bool com_showGrid = false;
        public bool playMusic = false;
        public bool playButtonSounds = false;
        public bool playButtonHaptic = false;
        public bool showTutorialParty = true;
        public bool showTutorialInventory = true;
        public bool showTutorialCombat = true;
        public bool showAutosaveMessage = true;
        public bool allowAutosave = true;
        public int combatAnimationSpeed = 25;
        private string onHeartBeatIBScript = "none";
        private string onHeartBeatIBScriptParms = "";
        //private bool _useRealTimeTimer = false;
        //private int _realTimeTimerLengthInMilliSeconds = 1500;
        public int attackFromBehindToHitModifier = 2;
        public int attackFromBehindDamageModifier = 0;
        private bool _useUIBackground = false;
        //private string _fontName = "Metamorphous";
        //private string _fontFilename = "Metamorphous-Regular.ttf";
        //private float _fontD2DScaleMultiplier = 1.0f;
        private int _logNumberOfLines = 20;
        //private string _spellLabelSingular = "Spell";
        //private string _spellLabelPlural = "Spells";
        private string _goldLabelSingular = "Gold";
        private string _goldLabelPlural = "Gold";
        #endregion

        #region Properties
        [CategoryAttribute("01 - Main"), DescriptionAttribute("FileName of the Module and the Folder name as well (no spaces allowed)")]
        public string moduleName
        {
            get { return _moduleName; }
            set { _moduleName = value; }
        }
        [CategoryAttribute("01 - Main"), DescriptionAttribute("Name of the Module displayed to the player")]
        public string moduleLabelName
        {
            get { return _moduleLabelName; }
            set { _moduleLabelName = value; }
        }
        [CategoryAttribute("01 - Main"), DescriptionAttribute("Version of module (must be an integer...1,2,3,4,etc.)")]
        public int moduleVersion
        {
            get { return _moduleVersion; }
            set { _moduleVersion = value; }
        }
        [CategoryAttribute("03 - Player and Party"), DescriptionAttribute("Filename of the Player to use as the default starting PC or the forced pre-made PC if that flag (mustUsePreMadePC) is true.")]
        public string defaultPlayerFilename
        {
            get { return _defaultPlayerFilename; }
            set { _defaultPlayerFilename = value; }
        }
        [CategoryAttribute("03 - Player and Party"), DescriptionAttribute("Set this flag to true if you only want the player to use the pre-made PC identified in defaultPlayerFilename property. The player will not be given an option to create a character.")]
        public bool mustUsePreMadePC
        {
            get { return _mustUsePreMadePC; }
            set { _mustUsePreMadePC = value; }
        }
        [CategoryAttribute("04 - Fonts and UI"), DescriptionAttribute("This flag activates using UI background graphics.")]
        public bool useUIBackground
        {
            get { return _useUIBackground; }
            set { _useUIBackground = value; }
        }
        [CategoryAttribute("04 - Fonts and UI"), DescriptionAttribute("This sets the number of lines of text that will be shown in the main map and combat log box. Best way to determine the appropriate vaule is to first pick the font size multipler (see fontD2DScaleMultiplier) and then run the game and click on some of the toggle buttons until you fill the log box and then count the number of lines that are displayed. Use that number for this property.")]
        public int logNumberOfLines
        {
            get { return _logNumberOfLines; }
            set { _logNumberOfLines = value; }
        }
        /*[CategoryAttribute("05 - Labels"), DescriptionAttribute("Label used for Spell singular form (ex. Spell, Power, etc.)")]
        public string spellLabelSingular
        {
            get { return _spellLabelSingular; }
            set { _spellLabelSingular = value; }
        }*/
        /*[CategoryAttribute("05 - Labels"), DescriptionAttribute("Label used for Spell plural form (ex. Spells, Powers, etc.)")]
        public string spellLabelPlural
        {
            get { return _spellLabelPlural; }
            set { _spellLabelPlural = value; }
        }*/
        [CategoryAttribute("05 - Labels"), DescriptionAttribute("Label used for Gold singular form (ex. Gold, Credit, etc.)")]
        public string goldLabelSingular
        {
            get { return _goldLabelSingular; }
            set { _goldLabelSingular = value; }
        }
        [CategoryAttribute("05 - Labels"), DescriptionAttribute("Label used for Gold plural form (ex. Gold, Credits, etc.)")]
        public string goldLabelPlural
        {
            get { return _goldLabelPlural; }
            set { _goldLabelPlural = value; }
        }

        [CategoryAttribute("03 - Player and Party"), DescriptionAttribute("The total number of player made characters allowed in the party (default is 1, max PCs in a party is 6)")]
        public int numberOfPlayerMadePcsAllowed
        {
            get { return _numberOfPlayerMadePcsAllowed; }
            set
            {
                if (value > 6)
                {
                    _numberOfPlayerMadePcsAllowed = 6;
                }
                else if (value < 1)
                {
                    _numberOfPlayerMadePcsAllowed = 1;
                }
                else
                {
                    _numberOfPlayerMadePcsAllowed = value;
                }
            }
        }
        [CategoryAttribute("03 - Player and Party"), DescriptionAttribute("The maximum total number of players that can be in the party (only values of 1-6 accepted)")]
        public int MaxPartySize
        {
            get { return maxPartySize; }
            set
            {
                if (value > 6)
                {
                    maxPartySize = 6;
                }
                else if (value < 1)
                {
                    maxPartySize = 1;
                }
                else
                {
                    maxPartySize = value;
                }
            }
        }
        [Editor(typeof(MultilineStringEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [CategoryAttribute("01 - Main"), DescriptionAttribute("Description of the Module")]
        public string moduleDescription
        {
            get { return _moduleDescription; }
            set { _moduleDescription = value; }
        }

        [Editor(typeof(MultilineStringEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [CategoryAttribute("01 - Main"), DescriptionAttribute("Text to show up when the 'Credits' button is pressed (use html tags for formatting <b><i><u><br><big><small><tt><h1>…<h6><div><p><font>)")]
        public string moduleCredits
        {
            get { return _moduleCredits; }
            set { _moduleCredits = value; }
        }

        [CategoryAttribute("01 - Main"), DescriptionAttribute("Used for making unique Tags"), ReadOnly(true)]
        public int nextIdNumber
        {
            get
            {
                _nextIdNumber++;
                return _nextIdNumber;
            }
            set { _nextIdNumber = value; }
        }

        [CategoryAttribute("02 - Starting Conditions"), DescriptionAttribute("Current value for World Time in generic units")]
        public int WorldTime
        {
            get { return worldTime; }
            set { worldTime = value; }
        }

        [Browsable(true), TypeConverter(typeof(AreaTypeConverter))]
        [CategoryAttribute("02 - Starting Conditions"), DescriptionAttribute("Filename of starting Area (no extension)")]
        public string startingArea
        {
            get { return _startingArea; }
            set { _startingArea = value; }
        }

        [CategoryAttribute("02 - Starting Conditions"), DescriptionAttribute("Starting X location in starting area")]
        public int startingPlayerPositionX
        {
            get { return _startingPlayerPositionX; }
            set { _startingPlayerPositionX = value; }
        }

        [CategoryAttribute("02 - Starting Conditions"), DescriptionAttribute("Starting Y location in starting area")]
        public int startingPlayerPositionY
        {
            get { return _startingPlayerPositionY; }
            set { _startingPlayerPositionY = value; }
        }

        [CategoryAttribute("02 - Starting Conditions"), DescriptionAttribute("Starting party gold")]
        public int partyGold
        {
            get { return _partyGold; }
            set { _partyGold = value; }
        }

        [CategoryAttribute("03 - Player and Party"), DescriptionAttribute("filename of the default party token image (minus the extension)")]
        public string partyTokenFilename
        {
            get { return _partyTokenFilename; }
            set { _partyTokenFilename = value; }
        }
        
        [Browsable(true), TypeConverter(typeof(IBScriptConverter))]
        [CategoryAttribute("06 - IBScript Hooks"), DescriptionAttribute("IBScript name to be run at the end of each move on any area map (not combat)")]
        public string OnHeartBeatIBScript
        {
            get { return onHeartBeatIBScript; }
            set { onHeartBeatIBScript = value; }
        }
        [CategoryAttribute("06 - IBScript Hooks"), DescriptionAttribute("Parameters to be used for this IBScript hook (as many parameters as needed, comma deliminated with no spaces)")]
        public string OnHeartBeatIBScriptParms
        {
            get { return onHeartBeatIBScriptParms; }
            set { onHeartBeatIBScriptParms = value; }
        }        
        #endregion

        public Module()
        {
        }
        public void saveModuleFile(string filename, bool indent)
        {
            string json = "";    
            if (indent)
            {
                json = JsonConvert.SerializeObject(this, Newtonsoft.Json.Formatting.Indented);
            }   
            else
            {
                json = JsonConvert.SerializeObject(this, Newtonsoft.Json.Formatting.None);
            }            
            using (StreamWriter sw = new StreamWriter(filename))
            {
                sw.Write(json.ToString());
            }
        }
        public Module loadModuleFile(string filename)
        {
            Module toReturn = null;

            // deserialize JSON directly from a file
            using (StreamReader file = File.OpenText(filename))
            {
                JsonSerializer serializer = new JsonSerializer();
                toReturn = (Module)serializer.Deserialize(file, typeof(Module));
            }
            return toReturn;
        }
    }
}
