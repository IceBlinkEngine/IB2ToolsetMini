﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
using System.ComponentModel;
//using IceBlink;
using System.Drawing.Design;
//using System.Design;
using System.ComponentModel.Design;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace IB2ToolsetMini
{
    public class Item : INotifyPropertyChanged
    {
                        
        public event PropertyChangedEventHandler PropertyChanged;

        #region Fields
        [JsonIgnore]
        public Bitmap itemIconBitmap;
        private bool _moduleItem = false;
        public List<string> classesAllowed = new List<string>();
        private string _armorWeightType = "Light"; //Light, Medium, Heavy           
        private string _itemImage = "blank";
        private string _name = "none"; //item name    
        private string _tag = "none"; //item unique tag name    
        private string _resref = "none"; //item resref name
        private string _desc = "enter item's description here"; //item detailed description
        private string _itemCategoryName = "newCategory";
        private string _useableInSituation = "Always"; //InCombat, OutOfCombat, Always, Passive
        private string _projectileSpriteFilename = "none"; //sprite to use for projectiles
        private string _spriteEndingFilename = "none"; //sprite to use for end effect of projectiles
        private string _itemOnUseSound = "none";
        private string _category = "Armor"; //catergory type (Armor, Ranged, Melee, General, Ring, Shield, Ammo)
        private bool _plotItem = false;
        private int _value = 0; //cost in credits
        private int _quantity = 1; //useful for stacking and ammo
        private int _groupSizeForSellingStackableItems = 1;
        private int _charges = 0; //useful for items like wands
        private string _ammoType = "none";
        private bool _twoHanded = false; //requires the use of two hands
        private bool _canNotBeUnequipped = false; //set to true for cursed items or summon creature items
        private bool _isStackable = false;
        private bool _automaticallyHitsTarget = false; //does not require a successful to hit roll, always hits target (ex. mage bolt wand)
        private int _attackBonus = 0; //attack bonus
        private int _attackRange = 1; //attack range
        private int _AreaOfEffect = 0; //AoE
        private AreaOfEffectShape _aoeShape = AreaOfEffectShape.Circle;
        private int _damageNumDice = 1; //number of dice to roll for damage
        private int _damageDie = 2; //type of dice to roll for damage
        private int _damageAdder = 0; //the adder like 2d4+1 where "1" is the adder
        private int _armorBonus = 0; //armor bonus
        private int _maxDexBonus = 99; //maximum Dexterity bonus allowed with this armor
        private int _attributeBonusModifierStr = 0;
        private int _attributeBonusModifierDex = 0;
        private int _attributeBonusModifierInt = 0;
        private int _attributeBonusModifierCha = 0;
        private int _attributeBonusModifierCon = 0;
        private int _attributeBonusModifierWis = 0;
        private int _savingThrowModifierReflex = 0;
        private int _savingThrowModifierFortitude = 0;
        private int _savingThrowModifierWill = 0;
        private int movementPointModifier = 0;
        private int _spRegenPerRoundInCombat = 0;
        private int _roundsPerSpRegenOutsideCombat = 0;
        private int _hpRegenPerRoundInCombat = 0;
        private int _roundsPerHpRegenOutsideCombat = 0;
        //private string _onScoringHit = "none";
        //private string _onScoringHitParms = "none";
        //private string _onUseItem = "none";
        private string _onUseItemIBScript = "none";
        private string _onUseItemIBScriptParms = "";
        private bool _destroyItemAfterOnUseItemIBScript = false;
        private string _onWhileEquipped = "none";
        private string _onScoringHitCastSpellTag = "none";
        private string _onUseItemCastSpellTag = "none";
        private bool _destroyItemAfterOnUseItemCastSpell = false;
        private int _levelOfItemForCastSpell = 1;
        private bool _usePlayerClassLevelForOnUseItemCastSpell = false;
        private int _damageTypeResistanceValueAcid = 0;
        private int _damageTypeResistanceValueCold = 0;
        private int _damageTypeResistanceValueNormal = 0;
        private int _damageTypeResistanceValueElectricity = 0;
        private int _damageTypeResistanceValueFire = 0;
        private int _damageTypeResistanceValueMagic = 0;
        private int _damageTypeResistanceValuePoison = 0;
        private string _typeOfDamage = "Normal"; //Normal,Acid,Cold,Electricity,Fire,Magic,Poison        
        private bool _isRation = false;
        #endregion

        #region Properties  
        [CategoryAttribute("01 - Main"), DescriptionAttribute("Can only change this in 'Advanced Mode'.")]
        public bool moduleItem
        {
            get
            {
                return _moduleItem;
            }
            set
            {
                _moduleItem = value; ;
            }
        }
        [CategoryAttribute("01 - Main"), DescriptionAttribute("Name of the Item")]
        public string name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                this.NotifyPropertyChanged("name");
            }
        }
        [CategoryAttribute("01 - Main"), DescriptionAttribute("Tag of the Item (will be given an unique tag for each placed instance of an item)")]
        public string tag
        {
            get
            {
                return _tag;
            }
            set
            {
                _tag = value;
                this.NotifyPropertyChanged("tag");
            }
        }
        [CategoryAttribute("01 - Main"), DescriptionAttribute("Resref of the Item (Must be unique from other item blueprints). All placed items will be linked to this blueprint via this resref tag so that updates here will be reflected on all placed items (unless a specific item property uses the saved game data like item charges.")]
        public string resref
        {
            get
            {
                return _resref;
            }
            set
            {
                _resref = value;
                this.NotifyPropertyChanged("resref");
            }
        }
        [CategoryAttribute("01 - Main"), DescriptionAttribute("Sets this item to be a ration when ration system is being used in this module.")]
        public bool isRation
        {
            get
            {
                return _isRation;
            }
            set
            {
                _isRation = value;;
            }
        }
        [CategoryAttribute("01 - Main"), DescriptionAttribute("Armor Weight Type (used when the item is an armor)")]
        public string ArmorWeightType
        {
            get
            {
                return _armorWeightType;
            }
            set
            {
                _armorWeightType = value;
            }

        }
        [CategoryAttribute("01 - Main"), DescriptionAttribute("grouping purposes in toolset")]
        public string ItemCategoryName
        {
            get
            {
                return _itemCategoryName;
            }
            set
            {
                _itemCategoryName = value;
                this.NotifyPropertyChanged("ItemCategoryName");
            }
        }
        [Browsable(true), TypeConverter(typeof(ItemTypeConverter))]
        [CategoryAttribute("01 - Main"), DescriptionAttribute("Category that this Item belongs to")]
        public string category
        {
            get
            {
                return _category;
            }
            set
            {
                _category = value;
                this.NotifyPropertyChanged("category");
            }
        }
        [Editor(typeof(MultilineStringEditor), typeof(UITypeEditor))]
        [CategoryAttribute("01 - Main"), DescriptionAttribute("Description of the Item")]
        public string desc
        {
            get
            {
                return _desc;
            }
            set
            {
                _desc = value;
                this.NotifyPropertyChanged("desc");
            }
        }
        [Browsable(true), TypeConverter(typeof(UseableWhenConverter))]
        [CategoryAttribute("01 - Main"), DescriptionAttribute("When can this be used: Always means that it can be used in combat and on the main maps, Passive means that it is always on and doesn't need to be activated.")]
        public string useableInSituation
        {
            get { return _useableInSituation; }
            set { _useableInSituation = value; }
        }
        [Browsable(true), TypeConverter(typeof(SpriteConverter))]
        [CategoryAttribute("01 - Main"), DescriptionAttribute("Sprite to use for the projectile (Sprite Filename with extension)")]
        public string projectileSpriteFilename
        {
            get
            {
                return _projectileSpriteFilename;
            }
            set
            {
                _projectileSpriteFilename = value;
            }
        }        
        [Browsable(true), TypeConverter(typeof(SpriteConverter))]
        [CategoryAttribute("01 - Main"), DescriptionAttribute("Sprite to use for the projectile ending effect (Sprite Filename with extension)")]
        public string spriteEndingFilename
        {
            get
            {
                return _spriteEndingFilename;
            }
            set
            {
                _spriteEndingFilename = value;
            }
        }
        [Browsable(true), TypeConverter(typeof(SoundConverter))]
        [CategoryAttribute("01 - Main"), DescriptionAttribute("Filename of sound to play when the item is used (no extension)")]
        public string itemOnUseSound
        {
            get { return _itemOnUseSound; }
            set { _itemOnUseSound = value; }
        }
        [CategoryAttribute("01 - Main"), DescriptionAttribute("Icon Filename of the Item"), ReadOnly(true)]
        public string itemImage
        {
            get
            {
                return _itemImage;
            }
            set
            {
                _itemImage = value;
            }
        }
        public bool plotItem
        {
            get
            {
                return _plotItem;
            }
            set
            {
                _plotItem = value;
            }
        }
        [CategoryAttribute("01 - Main"), DescriptionAttribute("Cost of the Item or Item group (see groupSizeForSellingStackableItems) in Gold Pieces")]
        public int value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
                this.NotifyPropertyChanged("value");
            }
        }
        [CategoryAttribute("01 - Main"), DescriptionAttribute("Number quantity of this item (useful for stacking items and ammo.")]
        public int quantity
        {
            get { return _quantity; }
            set { _quantity = value; }
        }
        [CategoryAttribute("01 - Main"), DescriptionAttribute("When selling items that are stackable, this number represents the quantity of this item that will be sold in one transactionn and the value in the 'value' property represents the cost for this group size.")]
        public int groupSizeForSellingStackableItems
        {
            get { return _groupSizeForSellingStackableItems; }
            set { _groupSizeForSellingStackableItems = value; }
        }
        [CategoryAttribute("01 - Main"), DescriptionAttribute("Number of charges the item has (useful for items like wands.")]
        public int charges
        {
            get { return _charges; }
            set { _charges = value; }
        }
        [CategoryAttribute("01 - Main"), DescriptionAttribute("tag of ammo type that links ranged weapon launchers with ammo (the ammoType tag for the launcher and ammo must match exactly).")]
        public string ammoType
        {
            get { return _ammoType; }
            set { _ammoType = value; }
        }
        [CategoryAttribute("01 - Main"), DescriptionAttribute("True if item requires the use of two hands.")]
        public bool twoHanded
        {
            get
            {
                return _twoHanded;
            }
            set
            {
                _twoHanded = value;
            }
        }
        [CategoryAttribute("01 - Main"), DescriptionAttribute("Set True if item can NOT be unequipped. Useful for temporary companion's specific items and cursed items.")]
        public bool canNotBeUnequipped
        {
            get
            {
                return _canNotBeUnequipped;
            }
            set
            {
                _canNotBeUnequipped = value;
            }
        }
        [CategoryAttribute("01 - Main"), DescriptionAttribute("Set True if item can be stacked, false for non stackable items.")]
        public bool isStackable
        {
            get
            {
                return _isStackable;
            }
            set
            {
                _isStackable = value;
            }
        }
        [CategoryAttribute("01 - Main"), DescriptionAttribute("Does not require a successful 'to hit' roll, always hits target (ex. mage bolt wand).")]
        public bool automaticallyHitsTarget
        {
            get
            {
                return _automaticallyHitsTarget;
            }
            set
            {
                _automaticallyHitsTarget = value;
            }
        }
        [CategoryAttribute("01 - Main"), DescriptionAttribute("Item Attack Bonus...Can be used to account for enchantments as well.")]
        public int attackBonus
        {
            get
            {
                return _attackBonus;
            }
            set
            {
                _attackBonus = value;
            }
        }
        [CategoryAttribute("01 - Main"), DescriptionAttribute("Item Attack Range measured in squares (ex 5 = 5 squares)")]
        public int attackRange
        {
            get
            {
                return _attackRange;
            }
            set
            {
                _attackRange = value;
            }
        }
        [CategoryAttribute("05 - Spell/Effect System"), DescriptionAttribute("Item's Area of Effect radius measured in squares for Spell/Effect properties of item if this item uses the Spell/Effect system (0 = 1 square, 1 = 9 squares, etc.)")]
        public int AreaOfEffect
        {
            get
            {
                return _AreaOfEffect;
            }
            set
            {
                _AreaOfEffect = value;
            }
        }
        [CategoryAttribute("05 - Spell/Effect System"), DescriptionAttribute("the shape of the AoE for the Effect/Spell system if used for this item.")]
        [JsonConverter(typeof(StringEnumConverter))]
        public AreaOfEffectShape aoeShape
        {
            get
            {
                return _aoeShape;
            }
            set
            {
                _aoeShape = value;
            }
        }
        [CategoryAttribute("01 - Main"), DescriptionAttribute("number of dice to roll for damage")]
        public int damageNumDice
        {
            get
            {
                return _damageNumDice;
            }
            set
            {
                _damageNumDice = value;
            }
        }
        [CategoryAttribute("01 - Main"), DescriptionAttribute("number of sided die to roll for damage")]
        public int damageDie
        {
            get
            {
                return _damageDie;
            }
            set
            {
                _damageDie = value;
            }
        }
        [CategoryAttribute("01 - Main"), DescriptionAttribute("The damage adder...for example with 2d4+1, the '1' is the adder. Can be used to account for enchantments as well.")]
        public int damageAdder
        {
            get
            {
                return _damageAdder;
            }
            set
            {
                _damageAdder = value;
            }
        }
        [CategoryAttribute("01 - Main"), DescriptionAttribute("Item's armor bonus")]
        public int armorBonus
        {
            get
            {
                return _armorBonus;
            }
            set
            {
                _armorBonus = value;
            }
        }
        [CategoryAttribute("01 - Main"), DescriptionAttribute("Maximum Dexterity Bonus allowed with this Item")]
        public int maxDexBonus
        {
            get
            {
                return _maxDexBonus;
            }
            set
            {
                _maxDexBonus = value;
            }
        }
        [CategoryAttribute("02 - Resistance Modifiers"), DescriptionAttribute("Damage resistance value (default is to use -100 to 100 as a percentage of immunity, damage multiplied by percentage so 0 = full damage, 100 = no damage, -100 = double damage)")]
        public int damageTypeResistanceValuePoison
        {
            get { return _damageTypeResistanceValuePoison; }
            set { _damageTypeResistanceValuePoison = value; }
        }
        [CategoryAttribute("02 - Resistance Modifiers"), DescriptionAttribute("Damage resistance value (default is to use -100 to 100 as a percentage of immunity, damage multiplied by percentage so 0 = full damage, 100 = no damage, -100 = double damage)")]
        public int damageTypeResistanceValueMagic
        {
            get { return _damageTypeResistanceValueMagic; }
            set { _damageTypeResistanceValueMagic = value; }
        }
        [CategoryAttribute("02 - Resistance Modifiers"), DescriptionAttribute("Damage resistance value (default is to use -100 to 100 as a percentage of immunity, damage multiplied by percentage so 0 = full damage, 100 = no damage, -100 = double damage)")]
        public int damageTypeResistanceValueNormal
        {
            get { return _damageTypeResistanceValueNormal; }
            set { _damageTypeResistanceValueNormal = value; }
        }
        [CategoryAttribute("02 - Resistance Modifiers"), DescriptionAttribute("Damage resistance value (default is to use -100 to 100 as a percentage of immunity, damage multiplied by percentage so 0 = full damage, 100 = no damage, -100 = double damage)")]
        public int damageTypeResistanceValueAcid
        {
            get { return _damageTypeResistanceValueAcid; }
            set { _damageTypeResistanceValueAcid = value; }
        }
        [CategoryAttribute("02 - Resistance Modifiers"), DescriptionAttribute("Damage resistance value (default is to use -100 to 100 as a percentage of immunity, damage multiplied by percentage so 0 = full damage, 100 = no damage, -100 = double damage)")]
        public int damageTypeResistanceValueCold
        {
            get { return _damageTypeResistanceValueCold; }
            set { _damageTypeResistanceValueCold = value; }
        }
        [CategoryAttribute("02 - Resistance Modifiers"), DescriptionAttribute("Damage resistance value (default is to use -100 to 100 as a percentage of immunity, damage multiplied by percentage so 0 = full damage, 100 = no damage, -100 = double damage)")]
        public int damageTypeResistanceValueElectricity
        {
            get { return _damageTypeResistanceValueElectricity; }
            set { _damageTypeResistanceValueElectricity = value; }
        }
        [CategoryAttribute("02 - Resistance Modifiers"), DescriptionAttribute("Damage resistance value (default is to use -100 to 100 as a percentage of immunity, damage multiplied by percentage so 0 = full damage, 100 = no damage, -100 = double damage)")]
        public int damageTypeResistanceValueFire
        {
            get { return _damageTypeResistanceValueFire; }
            set { _damageTypeResistanceValueFire = value; }
        }
        [CategoryAttribute("02 - Modifiers"), DescriptionAttribute("The Type of Damage (useful with immunity checks)")]
        [Browsable(true), TypeConverter(typeof(DamageTypeConverter))]
        public string typeOfDamage
        {
            get { return _typeOfDamage; }
            set { _typeOfDamage = value; }
        }
        [CategoryAttribute("02 - Modifiers"), DescriptionAttribute("The modifier amount for the Attribute")]
        public int attributeBonusModifierStr
        {
            get { return _attributeBonusModifierStr; }
            set { _attributeBonusModifierStr = value; }
        }
        [CategoryAttribute("02 - Modifiers"), DescriptionAttribute("The modifier amount for the Attribute")]
        public int attributeBonusModifierDex
        {
            get { return _attributeBonusModifierDex; }
            set { _attributeBonusModifierDex = value; }
        }
        [CategoryAttribute("02 - Modifiers"), DescriptionAttribute("The modifier amount for the Attribute")]
        public int attributeBonusModifierInt
        {
            get { return _attributeBonusModifierInt; }
            set { _attributeBonusModifierInt = value; }
        }
        [CategoryAttribute("02 - Modifiers"), DescriptionAttribute("The modifier amount for the Attribute")]
        public int attributeBonusModifierCha
        {
            get { return _attributeBonusModifierCha; }
            set { _attributeBonusModifierCha = value; }
        }
        [CategoryAttribute("02 - Modifiers"), DescriptionAttribute("The modifier amount for the Attribute")]
        public int attributeBonusModifierCon
        {
            get { return _attributeBonusModifierCon; }
            set { _attributeBonusModifierCon = value; }
        }
        [CategoryAttribute("02 - Modifiers"), DescriptionAttribute("The modifier amount for the Attribute")]
        public int attributeBonusModifierWis
        {
            get { return _attributeBonusModifierWis; }
            set { _attributeBonusModifierWis = value; }
        }
        [CategoryAttribute("02 - Modifiers"), DescriptionAttribute("The modifier amount for the Reflex Saving Throw")]
        public int savingThrowModifierReflex
        {
            get { return _savingThrowModifierReflex; }
            set { _savingThrowModifierReflex = value; }
        }
        [CategoryAttribute("02 - Modifiers"), DescriptionAttribute("The modifier amount for the Will Saving Throw")]
        public int savingThrowModifierWill
        {
            get { return _savingThrowModifierWill; }
            set { _savingThrowModifierWill = value; }
        }
        [CategoryAttribute("02 - Modifiers"), DescriptionAttribute("The modifier amount for the Fortitude Saving Throw")]
        public int savingThrowModifierFortitude
        {
            get { return _savingThrowModifierFortitude; }
            set { _savingThrowModifierFortitude = value; }
        }
        [CategoryAttribute("02 - Modifiers"), DescriptionAttribute("Modifier to movement")]
        public int MovementPointModifier
        {
            get { return movementPointModifier; }
            set { movementPointModifier = value; }
        }
        [CategoryAttribute("02 - Modifiers"), DescriptionAttribute("The number of SP to regenerate per round of combat (regen happens at start of new combat round)")]
        public int spRegenPerRoundInCombat
        {
            get { return _spRegenPerRoundInCombat; }
            set { _spRegenPerRoundInCombat = value; }
        }
        [CategoryAttribute("02 - Modifiers"), DescriptionAttribute("The number of HP to regenerate per round of combat (regen happens at start of new combat round)")]
        public int hpRegenPerRoundInCombat
        {
            get { return _hpRegenPerRoundInCombat; }
            set { _hpRegenPerRoundInCombat = value; }
        }
        [CategoryAttribute("02 - Modifiers"), DescriptionAttribute("The number of rounds that need to pass in order for 1 SP to regenerate when outside of combat.")]
        public int roundsPerSpRegenOutsideCombat
        {
            get { return _roundsPerSpRegenOutsideCombat; }
            set { _roundsPerSpRegenOutsideCombat = value; }
        }
        [CategoryAttribute("02 - Modifiers"), DescriptionAttribute("The number of rounds that need to pass in order for 1 HP to regenerate when outside of combat.")]
        public int roundsPerHpRegenOutsideCombat
        {
            get { return _roundsPerHpRegenOutsideCombat; }
            set { _roundsPerHpRegenOutsideCombat = value; }
        }
        /*[CategoryAttribute("03 - Scripts"), DescriptionAttribute("fires when the item makes a successful hit on a target")]
        public string onScoringHit
        {
            get { return _onScoringHit; }
            set { _onScoringHit = value; }
        }*/
        /*[CategoryAttribute("03 - Scripts"), DescriptionAttribute("(not used yet)optional input parameters if using a LogicTree...comma separated parameters")]
        public string onScoringHitParms
        {
            get { return _onScoringHitParms; }
            set { _onScoringHitParms = value; }
        }*/     
        [Browsable(true), TypeConverter(typeof(IBScriptConverter))]
        [CategoryAttribute("04 - IBScript Hooks"), DescriptionAttribute("IBScript name to be run upon using an item (onUseItem must be set to 'none' for this to work properly)")]
        public string onUseItemIBScript
        {
            get { return _onUseItemIBScript; }
            set { _onUseItemIBScript = value; }
        }
        [CategoryAttribute("04 - IBScript Hooks"), DescriptionAttribute("Parameters to be used for this IBScript hook (as many parameters as needed, comma deliminated with no spaces)")]
        public string onUseItemIBScriptParms
        {
            get { return _onUseItemIBScriptParms; }
            set { _onUseItemIBScriptParms = value; }
        }
        [CategoryAttribute("04 - IBScript Hooks"), DescriptionAttribute("If set to true, the item will be destroyed (or decremented by one if stacked) after the IBScript is completed.")]
        public bool destroyItemAfterOnUseItemIBScript
        {
            get { return _destroyItemAfterOnUseItemIBScript; }
            set { _destroyItemAfterOnUseItemIBScript = value; }
        }
        /*[CategoryAttribute("03 - Scripts"), DescriptionAttribute("fires when the item is used (onUseItemLogicTree must be set to 'none' for this to work properly)")]
        public string onUseItem
        {
            get { return _onUseItem; }
            set { _onUseItem = value; }
        }*/
        [CategoryAttribute("03 - Scripts"), DescriptionAttribute("fires every time the UpdateStats() function is called. Useful for modifying PC's stats.")]
        public string onWhileEquipped
        {
            get { return _onWhileEquipped; }
            set { _onWhileEquipped = value; }
        }
        [Browsable(true), TypeConverter(typeof(SpellTagTypeConverter))]
        [CategoryAttribute("05 - Spell/Effect System"), DescriptionAttribute("Cast this spell upon the target after making a successful attack (melee/ranged) hit on the target")]
        public string onScoringHitCastSpellTag
        {
            get { return _onScoringHitCastSpellTag; }
            set { _onScoringHitCastSpellTag = value; }
        }
        [Browsable(true), TypeConverter(typeof(SpellTagTypeConverter))]
        [CategoryAttribute("05 - Spell/Effect System"), DescriptionAttribute("Cast this spell upon using the item")]
        public string onUseItemCastSpellTag
        {
            get { return _onUseItemCastSpellTag; }
            set { _onUseItemCastSpellTag = value; }
        }
        [CategoryAttribute("05 - Spell/Effect System"), DescriptionAttribute("If set to true, the item will be destroyed (or decremented by one if stacked) after the Spell is completed.")]
        public bool destroyItemAfterOnUseItemCastSpell
        {
            get { return _destroyItemAfterOnUseItemCastSpell; }
            set { _destroyItemAfterOnUseItemCastSpell = value; }
        }
        [CategoryAttribute("05 - Spell/Effect System"), DescriptionAttribute("Effective level of item used in Effect calculations that are based on level adjustments.")]
        public int levelOfItemForCastSpell
        {
            get { return _levelOfItemForCastSpell; }
            set { _levelOfItemForCastSpell = value; }
        }
        [CategoryAttribute("05 - Spell/Effect System"), DescriptionAttribute("If set to true, the item will use the Player's class level instead of the item's effective level used in Effect calculations that are based on level adjustments.")]
        public bool usePlayerClassLevelForOnUseItemCastSpell
        {
            get { return _usePlayerClassLevelForOnUseItemCastSpell; }
            set { _usePlayerClassLevelForOnUseItemCastSpell = value; }
        }
        #endregion

        public Item()
        {
        }
        private void NotifyPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
        public void LoadItemBitmap(ParentForm prntForm)
        {
            this.itemIconBitmap = prntForm.LoadBitmapGDI(this.itemImage);
        }
        public override string ToString()
        {
            return name;
        }
        public Bitmap LoadItemIconBitmap(string filename)
        {
            // Sets up an image object to be displayed.
            if (itemIconBitmap != null)
            {
                itemIconBitmap.Dispose();
            }
            itemIconBitmap = new Bitmap(filename);
            return itemIconBitmap;
        }
        public Item ShallowCopy()
        {
            return (Item)this.MemberwiseClone();
        }
        public Item DeepCopy()
        {
            Item other = (Item)this.MemberwiseClone();
            other.classesAllowed = new List<string>();
            foreach (string s in this.classesAllowed)
            {
                other.classesAllowed.Add(s);
            }
            return other;
        }
    }

    public class ItemRefs
    {
        private string _resref = "none";
        private string _tag = "none";
        private string _name = "none";
        private bool _canNotBeUnequipped = false;
        private int _quantity = 1; //useful for stacking and ammo
        private bool _isRation = false;

        [CategoryAttribute("01 - Main"), DescriptionAttribute("ResRef of the item reference (must be left the same as the blueprint item's resref)")]
        public string resref
        {
            get { return _resref; }
            set { _resref = value; }
        }
        [CategoryAttribute("01 - Main"), DescriptionAttribute("Tag of the item (Must be unique from all other placed item tags)")]
        public string tag
        {
            get { return _tag; }
            set { _tag = value; }
        }
        [CategoryAttribute("01 - Main"), DescriptionAttribute("Name of the item as show in inventory and other places.")]
        public string name
        {
            get { return _name; }
            set { _name = value; }
        }
        [CategoryAttribute("01 - Main"), DescriptionAttribute("Set True if item can NOT be unequipped. Useful for temporary companion's specific items and cursed items.")]
        public bool canNotBeUnequipped
        {
            get { return _canNotBeUnequipped; }
            set { _canNotBeUnequipped = value; }
        }
        [CategoryAttribute("01 - Main"), DescriptionAttribute("Number quantity of this item (useful for stacking items and ammo.")]
        public int quantity
        {
            get { return _quantity; }
            set { _quantity = value; }
        }
        [CategoryAttribute("01 - Main"), DescriptionAttribute("Sets this item to be a ration if ration system is being used.")]
        public bool isRation
        {
            get { return _isRation; }
            set { _isRation = value; }
        }

        public ItemRefs()
        {
        }
        public override string ToString()
        {
            return name;
        }
        public ItemRefs DeepCopy()
        {
            ItemRefs other = (ItemRefs)this.MemberwiseClone();
            return other;
        }
    }
}
