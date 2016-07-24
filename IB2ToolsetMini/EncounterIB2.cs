using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IB2ToolsetMini
{
    public class EncounterIB2
    {
        public string encounterName = "newEncounter";
        public string MapImage = "none";
        public bool UseMapImage = false;
        public bool UseDayNightCycle = false;
        public int MapSizeX = 7;
        public int MapSizeY = 7;
        public List<TileEnc> encounterTiles = new List<TileEnc>();
        public List<CreatureRefs> encounterCreatureRefsList = new List<CreatureRefs>();
        public List<Creature> encounterCreatureList = new List<Creature>();
        public List<ItemRefs> encounterInventoryRefsList = new List<ItemRefs>();
        public List<Coordinate> encounterPcStartLocations = new List<Coordinate>();
        public int goldDrop = 0;
        public string AreaMusic = "none";
        public int AreaMusicDelay = 0;
        public int AreaMusicDelayRandomAdder = 0;
        public string OnSetupCombatIBScript = "none";
        public string OnSetupCombatIBScriptParms = "";
        public string OnStartCombatRoundIBScript = "none";
        public string OnStartCombatRoundIBScriptParms = "";
        public string OnStartCombatTurnIBScript = "none";
        public string OnStartCombatTurnIBScriptParms = "";
        public string OnEndCombatIBScript = "none";
        public string OnEndCombatIBScriptParms = "";

        public EncounterIB2()
        {
        }

        public EncounterIB2 DeepCopy()
        {
            EncounterIB2 copy = new EncounterIB2();
            copy = (EncounterIB2)this.MemberwiseClone();

            copy.encounterCreatureRefsList = new List<CreatureRefs>();
            foreach (CreatureRefs s in this.encounterCreatureRefsList)
            {
                CreatureRefs newCrtRef = new CreatureRefs();
                newCrtRef.creatureResRef = s.creatureResRef;
                newCrtRef.creatureTag = s.creatureTag;
                newCrtRef.creatureStartLocationX = s.creatureStartLocationX;
                newCrtRef.creatureStartLocationY = s.creatureStartLocationY;
                copy.encounterCreatureRefsList.Add(newCrtRef);
            }                        
            copy.encounterInventoryRefsList = new List<ItemRefs>();
            foreach (ItemRefs s in this.encounterInventoryRefsList)
            {
                ItemRefs newItRef = new ItemRefs();
                newItRef = s.DeepCopy();
                copy.encounterInventoryRefsList.Add(newItRef);
            }
            copy.encounterPcStartLocations = new List<Coordinate>();
            foreach (Coordinate s in this.encounterPcStartLocations)
            {
                Coordinate newCoor = new Coordinate();
                newCoor.X = s.X;
                newCoor.Y = s.Y;
                copy.encounterPcStartLocations.Add(newCoor);
            }            
            return copy;
        }
    }
}
