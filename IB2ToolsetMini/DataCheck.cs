﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace IB2ToolsetMini
{
    public class DataCheck
    {
        public ParentForm frm;
        public Module mod;
        public Area area = new Area();
        public Convo convo = new Convo();

        public DataCheck(Module m, ParentForm p)
        {
            mod = m;
            frm = p;
        }

        public void CheckAllData()
        {
            frm.Cursor = Cursors.WaitCursor;
            frm.logText(Environment.NewLine + Environment.NewLine + "Start Data Check." + Environment.NewLine);
            CheckCasterNoKnownSpells();
            CheckEncounterMapsNoFileExtension();
            CheckDuplicateTagsOrResRef();
            CheckModuleProperties();
            CheckAreas();
            CheckContainers();
            CheckEncounters();
            frm.logText("Completed Data Check." + Environment.NewLine + Environment.NewLine);
            frm.Cursor = Cursors.Arrow;
        }

        public void CheckCasterNoKnownSpells()
        {
            foreach (Creature crt in frm.allCreaturesList)
            {
                if ((crt.cr_ai == "GeneralCaster") && (crt.knownSpellsTags.Count == 0))
                {
                    frm.logText("CREATURE ERROR: " + crt.cr_name + " has a 'GeneralCaster' AI, but has no 'KnownSpells' selected" + Environment.NewLine);
                }
            }
        }
        public void CheckEncounterMapsNoFileExtension()
        {
            /*foreach (Encounter enc in frm.encountersList)
            {
                if ((enc.UseMapImage) && (enc.MapImage.Contains('.')))
                {
                    frm.logText("ENCOUNTER ERROR: " + enc.encounterName + " should NOT have a file extension at end of map image filename: " + enc.MapImage + Environment.NewLine);
                }
            }*/
        }
        public void CheckDuplicateTagsOrResRef()
        {
            //check creatures, items, props, containers, shops, encounters, races, classes, spells, traits, journal
            foreach (Creature crt in frm.allCreaturesList)
            {
                foreach (Creature crtck in frm.allCreaturesList)
                {
                    if ((crt != crtck) && (crt.cr_tag.Equals(crtck.cr_tag)))
                    {
                        frm.logText("CREATURE ERROR: " + crt.cr_name + " has the same tag as " + crtck.cr_name + Environment.NewLine);
                    }
                    if ((crt != crtck) && (crt.cr_resref.Equals(crtck.cr_resref)))
                    {
                        frm.logText("CREATURE ERROR: " + crt.cr_name + " has the same resref as " + crtck.cr_name + Environment.NewLine);
                    }
                }
            }
            foreach (Item crt in frm.allItemsList)
            {
                foreach (Item crtck in frm.allItemsList)
                {
                    if ((crt != crtck) && (crt.tag.Equals(crtck.tag)))
                    {
                        frm.logText("ITEM ERROR: " + crt.name + " has the same tag as " + crtck.name + Environment.NewLine);
                    }
                    if ((crt != crtck) && (crt.resref.Equals(crtck.resref)))
                    {
                        frm.logText("ITEM ERROR: " + crt.name + " has the same resref as " + crtck.name + Environment.NewLine);
                    }
                }
            }
            foreach (Prop it in frm.allPropsList)
            {
                foreach (Prop itck in frm.allPropsList)
                {
                    if ((it != itck) && (it.PropTag.Equals(itck.PropTag)))
                    {
                        frm.logText("PROP ERROR: " + it.PropName + " has the same tag as " + itck.PropName + Environment.NewLine);
                    }
                }
            }
            foreach (Container it in frm.mod.moduleContainersList)
            {
                foreach (Container itck in frm.mod.moduleContainersList)
                {
                    if ((it != itck) && (it.containerTag.Equals(itck.containerTag)))
                    {
                        frm.logText("CONTAINER ERROR: " + it.containerTag + " has the same tag as " + itck.containerTag + Environment.NewLine);
                    }
                }
            }
            foreach (Shop it in frm.mod.moduleShopsList)
            {
                foreach (Shop itck in frm.mod.moduleShopsList)
                {
                    if ((it != itck) && (it.shopTag.Equals(itck.shopTag)))
                    {
                        frm.logText("SHOP ERROR: " + it.shopName + " has the same tag as " + itck.shopName + Environment.NewLine);
                    }
                }
            }
            foreach (Encounter it in frm.mod.moduleEncountersList)
            {
                foreach (Encounter itck in frm.mod.moduleEncountersList)
                {
                    if ((it != itck) && (it.encounterName.Equals(itck.encounterName)))
                    {
                        frm.logText("ENCOUNTER ERROR: " + it.encounterName + " has the same name as " + itck.encounterName + Environment.NewLine);
                    }
                }
            }
            foreach (Race it in frm.datafile.dataRacesList)
            {
                foreach (Race itck in frm.datafile.dataRacesList)
                {
                    if ((it != itck) && (it.tag.Equals(itck.tag)))
                    {
                        frm.logText("RACE ERROR: " + it.name + " has the same tag as " + itck.name + Environment.NewLine);
                    }
                }
            }
            foreach (PlayerClass it in frm.datafile.dataPlayerClassList)
            {
                foreach (PlayerClass itck in frm.datafile.dataPlayerClassList)
                {
                    if ((it != itck) && (it.tag.Equals(itck.tag)))
                    {
                        frm.logText("PLAYER CLASS ERROR: " + it.name + " has the same tag as " + itck.name + Environment.NewLine);
                    }
                }
            }
            foreach (Spell it in frm.datafile.dataSpellsList)
            {
                foreach (Spell itck in frm.datafile.dataSpellsList)
                {
                    if ((it != itck) && (it.tag.Equals(itck.tag)))
                    {
                        frm.logText("SPELLS ERROR: " + it.name + " has the same tag as " + itck.name + Environment.NewLine);
                    }
                }
            }
            foreach (Trait it in frm.datafile.dataTraitsList)
            {
                foreach (Trait itck in frm.datafile.dataTraitsList)
                {
                    if ((it != itck) && (it.tag.Equals(itck.tag)))
                    {
                        frm.logText("TRAITS ERROR: " + it.name + " has the same tag as " + itck.name + Environment.NewLine);
                    }
                }
            }
        }
        public void CheckModuleProperties()
        {
            //check that starting area is an actual file
            bool foundOne = false;
            foreach (Area a in mod.moduleAreasObjects)
            {
                if (mod.startingArea.Equals(a.Filename))
                {
                    foundOne = true;
                }
            }
            if (!foundOne)
            {
                frm.logText("MODULE ERROR: Starting Area: " + mod.startingArea + " file is not found in the 'areas' list of your module" + Environment.NewLine);
            }
            //check that party token is a valid file
            /*foundOne = false;
            jobDir = "";
            jobDir = frm._mainDirectory + "\\modules\\" + mod.moduleName + "\\graphics";
            foreach (string f in Directory.GetFiles(jobDir, "*.png"))
            {
                string filename = Path.GetFileNameWithoutExtension(f);
                if (mod.partyTokenFilename.Equals(filename))
                {
                    foundOne = true;
                }
            }
            if (!foundOne)
            {
                frm.logText("MODULE ERROR: Party Token: " + mod.partyTokenFilename + " file is not found in the 'graphics' folder of your module" + Environment.NewLine);
            }*/
            //check that default player is a valid file name
            /*foundOne = false;
            jobDir = "";
            jobDir = frm._mainDirectory + "\\modules\\" + mod.moduleName + "\\data";
            foreach (string f in Directory.GetFiles(jobDir, "*.json"))
            {
                string filename = Path.GetFileName(f);
                if (mod.defaultPlayerFilename.Equals(filename))
                {
                    foundOne = true;
                }
            }
            if (!foundOne)
            {
                frm.logText("MODULE ERROR: Default Player: " + mod.defaultPlayerFilename + " file is not found in the 'data' folder of your module. (make sure to include file extension on name)." + Environment.NewLine);
            }*/
        }
        public void CheckAmmoTypes()
        {
            //go through all items and see if ammo type is used and find any items that have unique ammo name not used anywhere else
            //maybe state all types and how many times they show up
        }        
        public void CheckAreas()
        {
            //iterate through all areas from the area list and load one at a time and check
            //if load fails report            
            foreach (Area area in frm.mod.moduleAreasObjects)
            {
                //area = area.loadAreaFile(frm._mainDirectory + "\\modules\\" + frm.mod.moduleName + "\\areas\\" + areafilename + ".lvl");
                /*if (area == null)
                {
                    frm.logText("AREA ERROR: returned a null area for " + areafilename + ", most likely couldn't find file" + Environment.NewLine);
                    continue;
                }*/
                //go through all triggers and check for missing data
                foreach (Trigger trg in area.Triggers)
                {
                    //check if transition with no destination
                    if ((trg.Event1Type == "transition") && ((trg.Event1TransPointX == 0) && (trg.Event1TransPointY == 0)))
                    {
                        frm.logText("TRIGGER ERROR: " + area.Filename + ": trigger " + trg.TriggerTag + "event1 has a x=0 and y=0 location, is that intended?" + Environment.NewLine);
                    }
                    if ((trg.Event2Type == "transition") && ((trg.Event2TransPointX == 0) && (trg.Event2TransPointY == 0)))
                    {
                        frm.logText("TRIGGER ERROR: " + area.Filename + ": trigger " + trg.TriggerTag + "event2 has a x=0 and y=0 location, is that intended?" + Environment.NewLine);
                    }
                    if ((trg.Event3Type == "transition") && ((trg.Event3TransPointX == 0) && (trg.Event3TransPointY == 0)))
                    {
                        frm.logText("TRIGGER ERROR: " + area.Filename + ": trigger " + trg.TriggerTag + "event3 has a x=0 and y=0 location, is that intended?" + Environment.NewLine);
                    }
                    if ((trg.Event1Type != "none") && (trg.Event1FilenameOrTag == "none"))
                    {
                        frm.logText("TRIGGER ERROR: " + area.Filename + ": trigger " + trg.TriggerTag + ": event1 has type of " +  trg.Event1Type + " but filename/tag of 'none'" + Environment.NewLine);
                    }
                    if ((trg.Event2Type != "none") && (trg.Event2FilenameOrTag == "none"))
                    {
                        frm.logText("TRIGGER ERROR: " + area.Filename + ": trigger " + trg.TriggerTag + ": event2 has type of " + trg.Event2Type + " but filename/tag of 'none'" + Environment.NewLine);
                    }
                    if ((trg.Event3Type != "none") && (trg.Event3FilenameOrTag == "none"))
                    {
                        frm.logText("TRIGGER ERROR: " + area.Filename + ": trigger " + trg.TriggerTag + ": event3 has type of " + trg.Event3Type + " but filename/tag of 'none'" + Environment.NewLine);
                    }
                }
            }
        }
        public void CheckConvos()
        {
            //look for end points on red nodes
            foreach (Convo convo in frm.mod.moduleConvoList)
            {
                //convo = convo.GetConversation(frm._mainDirectory + "\\modules\\" + frm.mod.moduleName + "\\dialog\\", convofilename + ".json");
                if (convo == null)
                {
                    frm.logText("CONVO ERROR: returned a null convo for " + convo.ConvoFileName + ", most likely couldn't find file" + Environment.NewLine);
                    continue;
                }
                //go through all nodes recursively and look for end points that are red
                
            }
        }
        public void CheckContainers()
        {
            //check for empty containers
            foreach (Container c in frm.mod.moduleContainersList)
            {
                if (c.containerItemRefs.Count == 0)
                {
                    frm.logText("CONTAINER ERROR: " + c.containerTag + " has no items" + Environment.NewLine);
                }
            }
        }
        public void CheckEncounters()
        {
            //check for encounter with no creatures and/or no starting PC locations or less than 6
            foreach (Encounter enc in frm.mod.moduleEncountersList)
            {
                if (enc.encounterCreatureRefsList.Count == 0)
                {
                    frm.logText("ENCOUNTER ERROR: " + enc.encounterName + " has no creatures" + Environment.NewLine);
                }
                if (enc.encounterPcStartLocations.Count < 6)
                {
                    frm.logText("ENCOUNTER ERROR: " + enc.encounterName + " has less than 6 PC start locations (has " + enc.encounterPcStartLocations.Count.ToString() + ")" + Environment.NewLine);
                }
                //check to see if any tiles are non-walkable
                int foundOne = 0;
                foreach (int t in enc.Walkable)
                {
                    if (t == 0)
                    {
                        foundOne++;
                        break;
                    }
                }
                if (foundOne == 0)
                {
                    frm.logText("ENCOUNTER ERROR: " + enc.encounterName + " all tiles are walkable, was that intended?" + Environment.NewLine);
                }
                //go through all triggers and check for missing data
                foreach (Trigger trg in enc.Triggers)
                {
                    //check if transition with no destination
                    if ((trg.Event1Type == "transition") && ((trg.Event1TransPointX == 0) && (trg.Event1TransPointY == 0)))
                    {
                        frm.logText("TRIGGER ERROR: " + enc.encounterName + ": trigger " + trg.TriggerTag + "event1 has a x=0 and y=0 location, is that intended?" + Environment.NewLine);
                    }
                    if ((trg.Event2Type == "transition") && ((trg.Event2TransPointX == 0) && (trg.Event2TransPointY == 0)))
                    {
                        frm.logText("TRIGGER ERROR: " + enc.encounterName + ": trigger " + trg.TriggerTag + "event2 has a x=0 and y=0 location, is that intended?" + Environment.NewLine);
                    }
                    if ((trg.Event3Type == "transition") && ((trg.Event3TransPointX == 0) && (trg.Event3TransPointY == 0)))
                    {
                        frm.logText("TRIGGER ERROR: " + enc.encounterName + ": trigger " + trg.TriggerTag + "event3 has a x=0 and y=0 location, is that intended?" + Environment.NewLine);
                    }
                    if ((trg.Event1Type != "none") && (trg.Event1FilenameOrTag == "none"))
                    {
                        frm.logText("TRIGGER ERROR: " + enc.encounterName + ": trigger " + trg.TriggerTag + ": event1 has type of " + trg.Event1Type + " but filename/tag of 'none'" + Environment.NewLine);
                    }
                    if ((trg.Event2Type != "none") && (trg.Event2FilenameOrTag == "none"))
                    {
                        frm.logText("TRIGGER ERROR: " + enc.encounterName + ": trigger " + trg.TriggerTag + ": event2 has type of " + trg.Event2Type + " but filename/tag of 'none'" + Environment.NewLine);
                    }
                    if ((trg.Event3Type != "none") && (trg.Event3FilenameOrTag == "none"))
                    {
                        frm.logText("TRIGGER ERROR: " + enc.encounterName + ": trigger " + trg.TriggerTag + ": event3 has type of " + trg.Event3Type + " but filename/tag of 'none'" + Environment.NewLine);
                    }
                }
            }
        }
    }
}
