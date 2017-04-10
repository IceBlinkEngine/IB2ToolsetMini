using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
using System.ComponentModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
//using IceBlink;

namespace IB2ToolsetMini
{
    public class Trait
    {        
        #region Fields        
        private string _name = "newTrait"; //item name
        private string _tag = "newTraitTag"; //item unique tag name
        private string _traitImage = "sp_magebolt";
        private string _description = "";
        private string _prerequisiteTrait = "none";
        private int _skillModifier = 0;
        private string _skillModifierAttribute = "str";
        private string _useableInSituation = "Always";
        private string _spriteFilename = "none";
        private string _spriteEndingFilename = "none";
        private string _traitStartSound = "none";
        private string _traitEndSound = "none";
        private int _costSP = 0;
        private string _traitTargetType = "Enemy";
        private string _traitEffectType = "Damage";
        private bool _isUsedForCombatSquareEffect = false;
        private AreaOfEffectShape _aoeShape = AreaOfEffectShape.Circle;
        private int _aoeRadius = 0;
        private int _range = 0;
        private string _traitScript = "none";
        private bool _isPassive = true; //passive traits are on all the time like two attack, cleave, evasion, etc. non-passive (or active) traits are used like spells (think power attack, remove trap, etc.)
        private bool _usesTurnToActivate = true; //some traits are meant to be used in the same turn such as Power Attack and Set Trap
        private List<EffectTagForDropDownList> _traitEffectTagList = new List<EffectTagForDropDownList>();
        private List<EffectTagForDropDownList> _removeEffectTagList = new List<EffectTagForDropDownList>();
        #endregion

        #region Properties  
        [CategoryAttribute("01 - Main"), DescriptionAttribute("Name of the Trait")]
        public string name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }
        [CategoryAttribute("01 - Main"), DescriptionAttribute("Tag of the Trait (Must be unique)")]
        public string tag
        {
            get
            {
                return _tag;
            }
            set
            {
                _tag = value;
            }
        }
        [CategoryAttribute("01 - Main"), DescriptionAttribute("Image icon of the Trait")]
        public string traitImage
        {
            get
            {
                return _traitImage;
            }
            set
            {
                _traitImage = value;
            }
        }
        [Editor(typeof(MultilineStringEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [CategoryAttribute("01 - Main"), DescriptionAttribute("Detailed description of trait with some stats and cost as well")]
        public string description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
            }
        }
        [CategoryAttribute("01 - Main"), DescriptionAttribute("tag of the prerequisite trait if there is one, else use 'none'")]
        public string prerequisiteTrait
        {
            get { return _prerequisiteTrait; }
            set { _prerequisiteTrait = value; }
        }
        [CategoryAttribute("01 - Main"), DescriptionAttribute("The modifier value used during skill checks if this trait is a skill type")]
        public int skillModifier
        {
            get { return _skillModifier; }
            set { _skillModifier = value; }
        }
        [CategoryAttribute("01 - Main"), DescriptionAttribute("If the trait is a skill type trait, this is the attribute that is used for additional modifiers to skill check (str, dex, int, cha)")]
        public string skillModifierAttribute
        {
            get { return _skillModifierAttribute; }
            set { _skillModifierAttribute = value; }
        }
        [CategoryAttribute("01 - Main"), DescriptionAttribute("When can this be used: Always means that it can be used in combat and on the main maps, Passive means that it is always on and doesn't need to be activated.")]
        public string useableInSituation
        {
            get { return _useableInSituation; }
            set { _useableInSituation = value; }
        }
        [Browsable(true), TypeConverter(typeof(SpriteConverter))]
        [CategoryAttribute("01 - Main"), DescriptionAttribute("Sprite to use for the effect (Sprite Filename with extension)")]
        public string spriteFilename
        {
            get
            {
                return _spriteFilename;
            }
            set
            {
                _spriteFilename = value;
            }
        }
        [Browsable(true), TypeConverter(typeof(SpriteConverter))]
        [CategoryAttribute("01 - Main"), DescriptionAttribute("Sprite to use for the ending effect (Sprite Filename with extension)")]
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
        [CategoryAttribute("01- Main"), DescriptionAttribute("Filename of sound to play when the trait starts (no extension)")]
        public string traitStartSound
        {
            get { return _traitStartSound; }
            set { _traitStartSound = value; }
        }
        [Browsable(true), TypeConverter(typeof(SoundConverter))]
        [CategoryAttribute("01- Main"), DescriptionAttribute("Filename of sound to play when the trait ends (no extension)")]
        public string traitEndSound
        {
            get { return _traitEndSound; }
            set { _traitEndSound = value; }
        }
        [CategoryAttribute("01 - Main"), DescriptionAttribute("How much SP this Trait costs")]
        public int costSP
        {
            get
            {
                return _costSP;
            }
            set
            {
                _costSP = value;
            }
        }
        [CategoryAttribute("02 - Target"), DescriptionAttribute("The type of target for this trait")]
        public string traitTargetType
        {
            get
            {
                return _traitTargetType;
            }
            set
            {
                _traitTargetType = value;
            }
        }
        [CategoryAttribute("03 - Effect"), DescriptionAttribute("damage = persistent, negative; heal = persistent, positive; buff = temporary, positive; debuff = temporary, negative")]
        public string traitEffectType
        {
            get
            {
                return _traitEffectType;
            }
            set
            {
                _traitEffectType = value;
            }
        }
        [CategoryAttribute("02 - Target"), DescriptionAttribute("Does this trait apply effects to combat squares?")]
        public bool isUsedForCombatSquareEffect
        {
            get
            {
                return _isUsedForCombatSquareEffect;
            }
            set
            {
                _isUsedForCombatSquareEffect = value;
            }
        }
        [CategoryAttribute("02 - Target"), DescriptionAttribute("the shape of the AoE")]
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
        [CategoryAttribute("02 - Target"), DescriptionAttribute("the radius of the AoE")]
        public int aoeRadius
        {
            get
            {
                return _aoeRadius;
            }
            set
            {
                _aoeRadius = value;
            }
        }
        [CategoryAttribute("02 - Target"), DescriptionAttribute("the range allowed to the center or beginning of the AoE")]
        public int range
        {
            get
            {
                return _range;
            }
            set
            {
                _range = value;
            }
        }
        [CategoryAttribute("01 - Main"), DescriptionAttribute("the script to use for this Spell")]
        public string traitScript
        {
            get { return _traitScript; }
            set { _traitScript = value; }
        }
        [CategoryAttribute("01 - Main"), DescriptionAttribute("if true, the trait is always on...if false, the trait is used like a spell and must be activated.")]
        public bool isPassive
        {
            get { return _isPassive; }
            set { _isPassive = value; }
        }
        [CategoryAttribute("01 - Main"), DescriptionAttribute("if true, the use of this trait in combat will consume that player's turn. If false, the player will get to use this trait and continue their turn. Some traits are meant to be used in the same turn such as Power Attack and Set Trap.")]
        public bool usesTurnToActivate
        {
            get { return _usesTurnToActivate; }
            set { _usesTurnToActivate = value; }
        }
        [CategoryAttribute("05 - Trait/Effect System"), DescriptionAttribute("List of EffectTags that will be used for this trait.")]
        public List<EffectTagForDropDownList> traitEffectTagList
        {
            get { return _traitEffectTagList; }
            set { _traitEffectTagList = value; }
        }
        [CategoryAttribute("05 - Trait/Effect System"), DescriptionAttribute("List of EffectTags that will be removed from the target when this trait is used (used for dispell magic, free action, neutralize poison, etc.)")]
        public List<EffectTagForDropDownList> removeEffectTagList
        {
            get { return _removeEffectTagList; }
            set { _removeEffectTagList = value; }
        }
        #endregion

        public Trait()
        {            
        }
        public override string ToString()
        {
            return name;
        }
        public Trait ShallowCopy()
        {
            return (Trait)this.MemberwiseClone();
        }
        public Trait DeepCopy()
        {
            Trait other = (Trait)this.MemberwiseClone();

            other.traitEffectTagList = new List<EffectTagForDropDownList>();
            foreach (EffectTagForDropDownList s in this.traitEffectTagList)
            {
                other.traitEffectTagList.Add(s);
            }

            other.removeEffectTagList = new List<EffectTagForDropDownList>();
            foreach (EffectTagForDropDownList s in this.removeEffectTagList)
            {
                other.removeEffectTagList.Add(s);
            }

            return other;
        }
    }
}
