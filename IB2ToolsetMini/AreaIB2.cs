﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IB2ToolsetMini
{
    public class AreaIB2
    {
        public string Filename = "newArea";
        public int AreaVisibleDistance = 4;
        public bool RestingAllowed = false;
        public bool UseMiniMapFogOfWar = true;
        public bool areaDark = false;
        public bool UseDayNightCycle = false;
        public bool useMiniProps = false;
        public bool useSuperTinyProps = false;
        public int TimePerSquare = 6; //in minutes for now
        public string MusicFileName = "forest.mp3";
        public string ImageFileName = "none";
        public int backgroundImageStartLocX = 0;
        public int backgroundImageStartLocY = 0;
        public int MapSizeX = 16;
        public int MapSizeY = 16;
        public string AreaMusic = "none";
        public int AreaMusicDelay = 0;
        public int AreaMusicDelayRandomAdder = 0;
        public string AreaSounds = "none";
        public int AreaSoundsDelay = 0;
        public int AreaSoundsDelayRandomAdder = 0;
        public List<Tile> Tiles = new List<Tile>();
        public List<Prop> Props = new List<Prop>();
        public List<string> InitialAreaPropTagsList = new List<string>();
        public List<Trigger> Triggers = new List<Trigger>();
        public int NextIdNumber = 100;
        public string OnHeartBeatIBScript = "none";
        public string OnHeartBeatIBScriptParms = "";
        public List<LocalInt> AreaLocalInts = new List<LocalInt>();
        public List<LocalString> AreaLocalStrings = new List<LocalString>();
        public string inGameAreaName = "newArea";
        public string areaWeatherScript = "";
        public string areaWeatherScriptParms = "";
        public string effectChannelScript1 = "";
        public string effectChannelScript2 = "";
        public string effectChannelScript3 = "";
        public string effectChannelScript4 = "";
        public string effectChannelScriptParms1 = "";
        public string effectChannelScriptParms2 = "";
        public string effectChannelScriptParms3 = "";
        public string effectChannelScriptParms4 = "";
        public string areaWeatherName = "";
        public int weatherDurationMultiplierForScale = 1;
        public string westernNeighbourArea = "";
        public string easternNeighbourArea = "";
        public string northernNeighbourArea = "";
        public string southernNeighbourArea = "";
        public string sourceBitmapName = "";
                
        public AreaIB2()
        {
        }
    }    
}