﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json;
using WeifenLuo.WinFormsUI.Docking;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using SharpDX.Direct2D1;
using Bitmap = System.Drawing.Bitmap;
using SharpDX.DXGI;
using System.Runtime.InteropServices;

namespace IB2ToolsetMini
{
    public struct selectionStruct
    {
        public int index;
        public int oldIndex;
        public int x, y;
    }
    public struct tilepropToBePlacedSettings
    {
        public int angle;
        public int mirror;
    }

    public partial class WorldMapEditor : DockContent
    {
        public ParentForm prntForm;
        public Module mod;

        private Bitmap selectedBitmap;
        public string selectedBitmapFilename = "";
        private int sqr = 25;
        private Point currentPoint = new Point(0, 0);
        private Point lastPoint = new Point(0, 0);
        private int gridX = 0;
        private int gridY = 0;
        public string saveFilenameNoExt = "";
        public string returnMapFilenameNoExt;
        public int _selectedLbxEncounterIndex = 0;
        public int _selectedLbxCreatureIndex = 0;
        public string currentTileFilename = "t_grass";
        public tilepropToBePlacedSettings tileToBePlaced;
        public bool tileSelected = true;
        public Point currentSquareClicked = new Point(0, 0);
        public Point lastSquareClicked = new Point(0, 0);
        public Area area;
        public selectionStruct selectedTile;
        public Point lastSelectedCreaturePropIcon;
        public string lastSelectedObjectTag;
        public string lastSelectedObjectResRef;
        public Prop le_selectedProp = new Prop();

        #region Direct2D Stuff
        public bool useDirect2D = true;
        public SharpDX.Direct2D1.Factory Factory2D { get; private set; }
        public SharpDX.DirectWrite.Factory FactoryDWrite { get; private set; }
        public WindowRenderTarget RenderTarget2D { get; private set; }
        public SolidColorBrush SceneColorBrush { get; private set; }
        public Dictionary<string, SharpDX.Direct2D1.Bitmap> commonBitmapList = new Dictionary<string, SharpDX.Direct2D1.Bitmap>();
        public Point selectionBoxLocation = new Point(-1, -1);
        #endregion

        public WorldMapEditor(Module m, ParentForm p)
        {
            InitializeComponent();            
            mod = m;
            prntForm = p;
            area = prntForm.mod.moduleAreasObjects[prntForm._selectedLbxAreaIndex];
            resetTileToBePlacedSettings();            
            createTileImageButtons("t_");            
        }
        private void WorldMapEditor_Load(object sender, EventArgs e)
        {
            radioButton1.Checked = true;
            checkBox1.Checked = true;
            checkBox2.Checked = true;
            checkBox3.Checked = true;
            //checkBox4.Checked = true;
            //checkBox5.Checked = true;
                        
            lblMapSizeX.Text = area.MapSizeX.ToString();
            lblMapSizeY.Text = area.MapSizeY.ToString();

            rbtnInfo.Checked = true;
            rbtnZoom1x.Checked = true;
            InitDirect2DAndDirectWrite();
            loadAllModuleImageData();
        }
        public void loadAllModuleImageData()
        {
            foreach(ImageData imd in mod.moduleImageDataList)
            {
                commonBitmapList.Add(imd.name, ConvertGDIBitmapToD2D(prntForm.bsc.ConvertImageDataToBitmap(imd)));
            }
        }
        public void resetTileToBePlacedSettings()
        {
            tileToBePlaced.angle = 0;
            tileToBePlaced.mirror = 0;
        }
        private void resetPanelAndDeviceSize()
        {
            
            if (useDirect2D)
            {
                //TODO add D2D stuff
            }
            else
            {
                //GDI panelView.Width = area.MapSizeX * sqr;
                //GDI panelView.Height = area.MapSizeY * sqr;
                //GDI surface = new Bitmap(area.MapSizeX * sqr, area.MapSizeY * sqr);
                //GDI device = Graphics.FromImage(surface);
            }
        }
        private void createTileImageButtons(string filter)
        {
            try
            {
                //create list of tile prefixes
                //loop through list and create buttons using name of prefix for btn.Text
                this.flTileFilters.Controls.Clear();
                foreach (string f in prntForm.tilePrefixFilterList)
                {
                    Button btnTileFilter = new Button();
                    btnTileFilter.FlatAppearance.BorderColor = System.Drawing.Color.Black;
                    btnTileFilter.FlatAppearance.BorderSize = 2;
                    btnTileFilter.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
                    btnTileFilter.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Green;
                    btnTileFilter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                    btnTileFilter.Size = new System.Drawing.Size(52, 40);
                    //btnTileFilter.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    btnTileFilter.Text = f;
                    if (f.StartsWith("t_es_"))
                    {
                        btnTileFilter.Text = "Elderin Stone";
                    }
                    else if (f.StartsWith("t_hw_"))
                    {
                        btnTileFilter.Text = "Hearkenwold";
                    }
                    else if (f.StartsWith("t_w_"))
                    {
                        btnTileFilter.Text = "Walls";
                    }
                    else if (f.StartsWith("t_f_"))
                    {
                        btnTileFilter.Text = "Floors";
                    }
                    else if (f.StartsWith("t_fc_"))
                    {
                        btnTileFilter.Text = "FRUA";
                    }
                    else if (f.StartsWith("t_a_"))
                    {
                        btnTileFilter.Text = "MISC";
                    }
                    else if (f.StartsWith("t_n_"))
                    {
                        btnTileFilter.Text = "Nature";
                    }
                    else if (f.StartsWith("t_m_"))
                    {
                        btnTileFilter.Text = "Man Made";
                    }
                    else if (f.StartsWith("t_"))
                    {
                        btnTileFilter.Text = "All";
                    }
                    btnTileFilter.Tag = f;
                    btnTileFilter.UseVisualStyleBackColor = true;
                    btnTileFilter.Click += new System.EventHandler(this.btnTileFilter_Click);
                    this.flTileFilters.Controls.Add(btnTileFilter);
                }
                //resize groupbox4 and change start y location of panel3
                                
                this.flPanelTab1.Controls.Clear();
                //tileList.Clear();
                foreach (ImageData imd in mod.moduleImageDataList)
                {
                    if (!imd.name.StartsWith(filter))
                    {
                        continue;
                    }
                    using (Bitmap bit = prntForm.bsc.ConvertImageDataToBitmap(imd))
                    {
                        Button btnNew = new Button();
                        btnNew.BackgroundImage = (Image)bit.Clone();
                        btnNew.FlatAppearance.BorderColor = System.Drawing.Color.Black;
                        btnNew.FlatAppearance.BorderSize = 2;
                        btnNew.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
                        btnNew.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Green;
                        btnNew.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                        btnNew.Size = new System.Drawing.Size(50 + 2, 50 + 2);
                        btnNew.BackgroundImageLayout = ImageLayout.Zoom;
                        btnNew.Text = imd.name;
                        btnNew.UseVisualStyleBackColor = true;
                        btnNew.Click += new System.EventHandler(this.btnSelectedTerrain_Click);
                        this.flPanelTab1.Controls.Add(btnNew);
                    }
                }
                foreach (string f in Directory.GetFiles(prntForm._mainDirectory + "\\default\\NewModule\\tiles\\", "*.png"))
                {
                    if (!Path.GetFileName(f).StartsWith(filter))
                    {
                        continue;
                    }
                    string filename = Path.GetFullPath(f);
                    using (Bitmap bit = new Bitmap(filename))
                    {
                        Button btnNew = new Button();
                        btnNew.BackgroundImage = (Image)bit.Clone();
                        btnNew.FlatAppearance.BorderColor = System.Drawing.Color.Black;
                        btnNew.FlatAppearance.BorderSize = 2;
                        btnNew.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
                        btnNew.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Green;
                        btnNew.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                        btnNew.Size = new System.Drawing.Size(50 + 2, 50 + 2);
                        btnNew.BackgroundImageLayout = ImageLayout.Zoom;
                        btnNew.Text = Path.GetFileNameWithoutExtension(f);
                        btnNew.UseVisualStyleBackColor = true;
                        btnNew.Click += new System.EventHandler(this.btnSelectedTerrain_Click);
                        this.flPanelTab1.Controls.Add(btnNew);
                        //fill tileList as well
                        //TileBitmapNamePair t = new TileBitmapNamePair((Bitmap)bit.Clone(), Path.GetFileNameWithoutExtension(f));
                        //tileList.Add(t);
                    }
                }                
            }
            catch (Exception ex)
            {
                MessageBox.Show("error: " + ex.ToString());
            }
        }
        private void refreshMap(bool refreshAll)
        {
            /*//GDI        
            if (area == null) { return; }
            //if area is small, always do a full redraw (refreshAll == true)
            if ((area.MapSizeX < 20) && (area.MapSizeY < 20))
            {
                refreshAll = true;
            }
            if (refreshAll)
            {
                device.Clear(Color.Gainsboro);
            }
            try
            {
                //draw background image first if using one
                if ((!area.ImageFileName.Equals("none")) && (gameMapBitmap != null))
                {
                    Rectangle srcBG = new Rectangle(0, 0, gameMapBitmap.Width, gameMapBitmap.Height);
                    Rectangle dstBG = new Rectangle(area.backgroundImageStartLocX * sqr, area.backgroundImageStartLocY * sqr, sqr * (gameMapBitmap.Width / 50), sqr * (gameMapBitmap.Height / 50));
                    device.DrawImage(gameMapBitmap, dstBG, srcBG, GraphicsUnit.Pixel);
                }
                //draw map en block
                //new code for drawing layer 0, aka puzzle pieces of hand drawn map
                //if (calledFromLoadButton == true)
                //{
                calledFromLoadButton = false;
                if (mod.useAllTileSystem)
                {
                    #region Draw Layer 0
                    if (area.sourceBitmapName != "")
                    {
                        int tileCounter = 0;
                        for (int y = 0; y < area.MapSizeY; y++)
                        {
                            for (int x = 0; x < area.MapSizeX; x++)
                            {
                                if ((refreshAll) || (currentSquareClicked == new Point(x, y)) || (lastSquareClicked == new Point(x, y)))
                                {
                                    Tile tile = area.Tiles[y * area.MapSizeX + x];
                                    Bitmap lyr0 = null;
                                    try
                                    {
                                        tile.Layer0Filename = area.sourceBitmapName + tileCounter.ToString();
                                    }
                                    catch { }
                                    if ((tile.Layer0Filename != null) && (tile.Layer0Filename != "") && tile.Layer0Filename != "t_a_blank")
                                    {
                                        if (area.isPNGMap)
                                        {
                                            string bitMapPath = prntForm._mainDirectory + "\\modules\\" + prntForm.mod.moduleName + "\\graphics\\" + area.sourceBitmapName + "\\" + tile.Layer0Filename + ".png";
                                        }
                                        if (area.isJPGMap)
                                        {
                                            string bitMapPath = prntForm._mainDirectory + "\\modules\\" + prntForm.mod.moduleName + "\\graphics\\" + area.sourceBitmapName + "\\" + tile.Layer0Filename + ".jpg";
                                        }
                                        tileCounter++;
                                        //Rectangle src1 = new Rectangle(0, 0, 100, 100);

                                        //if (tile.Layer0Filename != null)
                                        //{

                                        //lyr0 = new Bitmap(Path.GetFullPath(tile.Layer0Filename + ".png"));
                                        //g_walkPass = new Bitmap(prntForm._mainDirectory + "\\modules\\" + prntForm.mod.moduleName + "\\graphics\\walk_pass.png");
                                        //lyr0 = getTileByName(tile.Layer0Filename).bitmap;
                                        try
                                        {
                                            if (area.isPNGMap)
                                            {
                                                lyr0 = new Bitmap(prntForm._mainDirectory + "\\modules\\" + prntForm.mod.moduleName + "\\graphics\\" + area.sourceBitmapName + "\\" + tile.Layer0Filename + ".png");
                                                //int block = 3;
                                            }
                                            if (area.isJPGMap)
                                            {
                                                lyr0 = new Bitmap(prntForm._mainDirectory + "\\modules\\" + prntForm.mod.moduleName + "\\graphics\\" + area.sourceBitmapName + "\\" + tile.Layer0Filename + ".jpg");
                                                //int block = 3;
                                            }
                                        }
                                        catch
                                        {

                                        }
                                        //lyr1 = (Bitmap)getTileByName(tile.Layer1Filename).bitmap.Clone();
                                        //flip about y-axis layer
                                        //lyr1 = Flip(lyr1, tile.Layer1Flip);
                                        //rotate layer
                                        //lyr1 = Rotate(lyr1, tile.Layer1Rotate);
                                        //src1 = new Rectangle(0, 0, lyr1.Width, lyr1.Height);
                                        //}
                                        //Rectangle src = new Rectangle(0, 0, 100, 100);
                                        //Rectangle dst = new Rectangle(x * sqr, y * sqr, sqr, sqr);
                                        //draw layer 1 first


                                        if (lyr0 != null)
                                        {
                                            //float scalerX = lyr0.Width / 100;
                                            //float scalerY = lyr0.Height / 100;
                                            float scalerX = 1;
                                            float scalerY = 1;
                                            Rectangle src = new Rectangle(0, 0, lyr0.Width, lyr0.Height);
                                            Rectangle dst = new Rectangle(x * sqr, y * sqr, (int)(sqr * scalerX), (int)(sqr * scalerY));
                                            device.DrawImage(lyr0, dst, src, GraphicsUnit.Pixel);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    #endregion
                }
                #region Draw Layer 1
                if (checkBox1.Checked)
                {
                    for (int y = 0; y < area.MapSizeY; y++)
                    {
                        for (int x = 0; x < area.MapSizeX; x++)
                        {
                            if ((refreshAll) || (currentSquareClicked == new Point(x, y)) || (lastSquareClicked == new Point(x, y)))
                            {
                                Tile tile = area.Tiles[y * area.MapSizeX + x];
                                Bitmap lyr1 = null;
                                if (getTileByName(tile.Layer1Filename) != null)
                                {
                                    lyr1 = getTileByName(tile.Layer1Filename).bitmap;
                                }
                                //draw layer 1 first
                                if (lyr1 != null)
                                {
                                    float scalerX = lyr1.Width / 100;
                                    float scalerY = lyr1.Height / 100;
                                    Rectangle src = new Rectangle(0, 0, lyr1.Width, lyr1.Height);
                                    Rectangle dst = new Rectangle(x * sqr, y * sqr, (int)(sqr * scalerX), (int)(sqr * scalerY));
                                    device.DrawImage(lyr1, dst, src, GraphicsUnit.Pixel);
                                }
                            }
                        }
                    }
                }
                #endregion
                #region Draw Layer 2
                if (checkBox2.Checked)
                {
                    for (int y = 0; y < area.MapSizeY; y++)
                    {
                        for (int x = 0; x < area.MapSizeX; x++)
                        {
                            if ((refreshAll) || (currentSquareClicked == new Point(x, y)) || (lastSquareClicked == new Point(x, y)))
                            {
                                Tile tile = area.Tiles[y * area.MapSizeX + x];
                                Bitmap lyr1 = null;
                                if (getTileByName(tile.Layer2Filename) != null)
                                {
                                    lyr1 = getTileByName(tile.Layer2Filename).bitmap;
                                }
                                if (lyr1 != null)
                                {
                                    float scalerX = lyr1.Width / 100;
                                    float scalerY = lyr1.Height / 100;
                                    Rectangle src = new Rectangle(0, 0, lyr1.Width, lyr1.Height);
                                    Rectangle dst = new Rectangle(x * sqr, y * sqr, (int)(sqr * scalerX), (int)(sqr * scalerY));
                                    device.DrawImage(lyr1, dst, src, GraphicsUnit.Pixel);
                                }
                            }
                        }
                    }
                }
                #endregion
                #region Draw Layer 3
                if (checkBox3.Checked)
                {
                    for (int y = 0; y < area.MapSizeY; y++)
                    {
                        for (int x = 0; x < area.MapSizeX; x++)
                        {
                            if ((refreshAll) || (currentSquareClicked == new Point(x, y)) || (lastSquareClicked == new Point(x, y)))
                            {
                                Tile tile = area.Tiles[y * area.MapSizeX + x];
                                Bitmap lyr1 = null;
                                if (getTileByName(tile.Layer3Filename) != null)
                                {
                                    lyr1 = getTileByName(tile.Layer3Filename).bitmap;
                                }
                                if (lyr1 != null)
                                {
                                    float scalerX = lyr1.Width / 100;
                                    float scalerY = lyr1.Height / 100;
                                    Rectangle src = new Rectangle(0, 0, lyr1.Width, lyr1.Height);
                                    Rectangle dst = new Rectangle(x * sqr, y * sqr, (int)(sqr * scalerX), (int)(sqr * scalerY));
                                    device.DrawImage(lyr1, dst, src, GraphicsUnit.Pixel);
                                }
                            }
                        }
                    }
                }
                #endregion
                #region Draw Layer 4
                if (checkBox4.Checked)
                {
                    for (int y = 0; y < area.MapSizeY; y++)
                    {
                        for (int x = 0; x < area.MapSizeX; x++)
                        {
                            if ((refreshAll) || (currentSquareClicked == new Point(x, y)) || (lastSquareClicked == new Point(x, y)))
                            {
                                Tile tile = area.Tiles[y * area.MapSizeX + x];
                                Bitmap lyr1 = null;
                                if (getTileByName(tile.Layer4Filename) != null)
                                {
                                    lyr1 = getTileByName(tile.Layer4Filename).bitmap;
                                }
                                if (lyr1 != null)
                                {
                                    float scalerX = lyr1.Width / 100;
                                    float scalerY = lyr1.Height / 100;
                                    Rectangle src = new Rectangle(0, 0, lyr1.Width, lyr1.Height);
                                    Rectangle dst = new Rectangle(x * sqr, y * sqr, (int)(sqr * scalerX), (int)(sqr * scalerY));
                                    device.DrawImage(lyr1, dst, src, GraphicsUnit.Pixel);
                                }
                            }
                        }
                    }
                }
                #endregion
                #region Draw Layer 5
                if (checkBox5.Checked)
                {
                    for (int y = 0; y < area.MapSizeY; y++)
                    {
                        for (int x = 0; x < area.MapSizeX; x++)
                        {
                            if ((refreshAll) || (currentSquareClicked == new Point(x, y)) || (lastSquareClicked == new Point(x, y)))
                            {
                                Tile tile = area.Tiles[y * area.MapSizeX + x];
                                Bitmap lyr1 = null;
                                if (getTileByName(tile.Layer5Filename) != null)
                                {
                                    lyr1 = getTileByName(tile.Layer5Filename).bitmap;
                                }
                                if (lyr1 != null)
                                {
                                    float scalerX = lyr1.Width / 100;
                                    float scalerY = lyr1.Height / 100;
                                    Rectangle src = new Rectangle(0, 0, lyr1.Width, lyr1.Height);
                                    Rectangle dst = new Rectangle(x * sqr, y * sqr, (int)(sqr * scalerX), (int)(sqr * scalerY));
                                    device.DrawImage(lyr1, dst, src, GraphicsUnit.Pixel);
                                }
                            }
                        }
                    }
                }
                #endregion

                #region Draw Grid
                for (int y = 0; y < area.MapSizeY; y++)
                {
                    for (int x = 0; x < area.MapSizeX; x++)
                    {
                        if ((refreshAll) || (currentSquareClicked == new Point(x, y)) || (lastSquareClicked == new Point(x, y)))
                        {
                            Tile tile = area.Tiles[y * area.MapSizeX + x];
                            //draw square walkmesh and LoS stuff
                            Rectangle src = new Rectangle(0, 0, g_walkPass.Width, g_walkPass.Height);
                            Rectangle target = new Rectangle(x * sqr, y * sqr, sqr, sqr);
                            if (chkGrid.Checked) //if show grid is turned on, draw grid squares
                            {
                                if (tile.LoSBlocked)
                                {
                                    device.DrawImage(g_LoSBlock, target, src, GraphicsUnit.Pixel);
                                    device.DrawImage(g_LoSBlock, target, src, GraphicsUnit.Pixel);
                                }
                                if (tile.Walkable)
                                {
                                    device.DrawImage(g_walkPass, target, src, GraphicsUnit.Pixel);
                                }
                                else
                                {
                                    target = new Rectangle(x * sqr + 1, y * sqr + 1, sqr - 1, sqr - 1);
                                    device.DrawImage(g_walkBlock, target, src, GraphicsUnit.Pixel);
                                    device.DrawImage(g_walkBlock, target, src, GraphicsUnit.Pixel);
                                    device.DrawImage(g_walkBlock, target, src, GraphicsUnit.Pixel);
                                }
                            }
                            target = new Rectangle(x * sqr, y * sqr, sqr, sqr);
                            if (chkGrid.Checked)
                            {
                                //device.DrawRectangle(blackPen, target);
                            }
                        }
                    }
                }
                #endregion
                #region Old stuff for reference (to be deleted later once fully tested)
                /*for (int y = 0; y < area.MapSizeY; y++)
                {
                    for (int x = 0; x < area.MapSizeX; x++)
                    {
                        if ((refreshAll) || (currentSquareClicked == new Point(x, y)) || (lastSquareClicked == new Point(x, y)))
                        {
                            Tile tile = area.Tiles[y * area.MapSizeX + x];
                            Bitmap lyr1 = null;
                            Bitmap lyr2 = null;
                            Bitmap lyr3 = null;
                            Bitmap lyr4 = null;
                            Bitmap lyr5 = null;
                            Rectangle src1 = new Rectangle(0, 0, 100, 100);
                            Rectangle src2 = new Rectangle(0, 0, 100, 100);
                            Rectangle src3 = new Rectangle(0, 0, 100, 100);
                            Rectangle src4 = new Rectangle(0, 0, 100, 100);
                            Rectangle src5 = new Rectangle(0, 0, 100, 100);
                            if (getTileByName(tile.Layer1Filename) != null)
                            {
                                lyr1 = getTileByName(tile.Layer1Filename).bitmap;
                                //lyr1 = (Bitmap)getTileByName(tile.Layer1Filename).bitmap.Clone();
                                //flip about y-axis layer
                                //lyr1 = Flip(lyr1, tile.Layer1Flip);
                                //rotate layer
                                //lyr1 = Rotate(lyr1, tile.Layer1Rotate);
                                //src1 = new Rectangle(0, 0, lyr1.Width, lyr1.Height);
                            }
                            if (getTileByName(tile.Layer2Filename) != null)
                            {
                                lyr2 = getTileByName(tile.Layer2Filename).bitmap;
                                //flip about y-axis layer
                                //lyr2 = Flip(lyr2, tile.Layer2Flip);
                                //rotate layer
                                //lyr2 = Rotate(lyr2, tile.Layer2Rotate);
                                //src2 = new Rectangle(0, 0, lyr2.Width, lyr2.Height);
                            }
                            if (getTileByName(tile.Layer3Filename) != null)
                            {
                                lyr3 = getTileByName(tile.Layer3Filename).bitmap;
                                //flip about y-axis layer
                                //lyr3 = Flip(lyr3, tile.Layer3Flip);
                                //rotate layer
                                //lyr3 = Rotate(lyr3, tile.Layer3Rotate);
                                //src3 = new Rectangle(0, 0, lyr3.Width, lyr3.Height);
                            }
                            if (getTileByName(tile.Layer4Filename) != null)
                            {
                                lyr4 = getTileByName(tile.Layer4Filename).bitmap;
                                //flip about y-axis layer
                                //lyr4 = Flip(lyr4, tile.Layer4Flip);
                                //rotate layer
                                //lyr4 = Rotate(lyr4, tile.Layer4Rotate);
                                //src4 = new Rectangle(0, 0, lyr4.Width, lyr4.Height);
                            }
                            if (getTileByName(tile.Layer5Filename) != null)
                            {
                                lyr5 = getTileByName(tile.Layer5Filename).bitmap;
                                //flip about y-axis layer
                                //lyr5 = Flip(lyr5, tile.Layer5Flip);
                                //rotate layer
                                //lyr5 = Rotate(lyr5, tile.Layer5Rotate);
                                //src5 = new Rectangle(0, 0, lyr5.Width, lyr5.Height);
                            }

                            Rectangle src = new Rectangle(0, 0, 100, 100);
                            Rectangle dst = new Rectangle(x * sqr, y * sqr, sqr, sqr);
                            //draw layer 1 first
                            if (checkBox1.Checked)
                            {
                                if (lyr1 != null)
                                {
                                    float scalerX = lyr1.Width / 100;
                                    float scalerY = lyr1.Height / 100;
                                    src = new Rectangle(0, 0, lyr1.Width, lyr1.Height);
                                    dst = new Rectangle(x * sqr, y * sqr, (int)(sqr * scalerX), (int)(sqr * scalerY));
                                    device.DrawImage(lyr1, dst, src, GraphicsUnit.Pixel);
                                }
                            }
                            //draw layer 2
                            if (checkBox2.Checked)
                            {
                                if (lyr2 != null)
                                {
                                    float scalerX = lyr2.Width / 100;
                                    float scalerY = lyr2.Height / 100;
                                    src = new Rectangle(0, 0, lyr2.Width, lyr2.Height);
                                    dst = new Rectangle(x * sqr, y * sqr, (int)(sqr * scalerX), (int)(sqr * scalerY));
                                    device.DrawImage(lyr2, dst, src, GraphicsUnit.Pixel);
                                }
                            }
                            //draw layer 3
                            if (checkBox3.Checked)
                            {
                                if (lyr3 != null)
                                {
                                    float scalerX = lyr3.Width / 100;
                                    float scalerY = lyr3.Height / 100;
                                    src = new Rectangle(0, 0, lyr3.Width, lyr3.Height);
                                    dst = new Rectangle(x * sqr, y * sqr, (int)(sqr * scalerX), (int)(sqr * scalerY));
                                    device.DrawImage(lyr3, dst, src, GraphicsUnit.Pixel);
                                }
                            }
                            //draw layer 4
                            if (checkBox4.Checked)
                            {
                                if (lyr4 != null)
                                {
                                    float scalerX = lyr4.Width / 100;
                                    float scalerY = lyr4.Height / 100;
                                    src = new Rectangle(0, 0, lyr4.Width, lyr4.Height);
                                    dst = new Rectangle(x * sqr, y * sqr, (int)(sqr * scalerX), (int)(sqr * scalerY));
                                    device.DrawImage(lyr4, dst, src, GraphicsUnit.Pixel);
                                }
                            }
                            //draw layer 5
                            if (checkBox5.Checked)
                            {
                                if (lyr5 != null)
                                {
                                    float scalerX = lyr5.Width / 100;
                                    float scalerY = lyr5.Height / 100;
                                    src = new Rectangle(0, 0, lyr5.Width, lyr5.Height);
                                    dst = new Rectangle(x * sqr, y * sqr, (int)(sqr * scalerX), (int)(sqr * scalerY));
                                    device.DrawImage(lyr5, dst, src, GraphicsUnit.Pixel);
                                }
                            }
                            //draw square walkmesh and LoS stuff
                            src = new Rectangle(0, 0, g_walkPass.Width, g_walkPass.Height);
                            Rectangle target = new Rectangle(x * sqr, y * sqr, sqr, sqr);
                            if (chkGrid.Checked) //if show grid is turned on, draw grid squares
                            {
                                if (tile.LoSBlocked)
                                {
                                    device.DrawImage(g_LoSBlock, target, src, GraphicsUnit.Pixel);
                                    device.DrawImage(g_LoSBlock, target, src, GraphicsUnit.Pixel);
                                }
                                if (tile.Walkable)
                                {
                                    device.DrawImage(g_walkPass, target, src, GraphicsUnit.Pixel);
                                }
                                else
                                {
                                    target = new Rectangle(x * sqr + 1, y * sqr + 1, sqr - 1, sqr - 1);
                                    device.DrawImage(g_walkBlock, target, src, GraphicsUnit.Pixel);
                                    device.DrawImage(g_walkBlock, target, src, GraphicsUnit.Pixel);
                                    device.DrawImage(g_walkBlock, target, src, GraphicsUnit.Pixel);
                                }
                            }
                            target = new Rectangle(x * sqr, y * sqr, sqr, sqr);
                            if (chkGrid.Checked)
                            {
                                //device.DrawRectangle(blackPen, target);
                            }
                        }
                    }
                }*/
            /*//GDI #endregion

                        int cnt = 0;
                            foreach (Prop prpRef in area.Props)
                            {
                                int cspx = prpRef.LocationX;
                                int cspy = prpRef.LocationY;
                                if ((refreshAll) || (currentSquareClicked == new Point(cspx, cspy)) || (lastSquareClicked == new Point(cspx, cspy)))
                                {
                                    spritePropDraw(cspx, cspy, cnt);
                                }
                                cnt++;
                            }
                            foreach (Trigger t in area.Triggers)
                            {
                                foreach (Coordinate p in t.TriggerSquaresList)
                                {
                                    if ((refreshAll) || (currentSquareClicked == new Point(p.X, p.Y)) || (lastSquareClicked == new Point(p.X, p.Y)))
                                    {
                                        int dx = p.X * sqr;
                                        int dy = p.Y * sqr;
                                        Pen pen = new Pen(Color.Orange, 2);
                                        if ((t.Event1Type == "encounter") || (t.Event2Type == "encounter") || (t.Event3Type == "encounter"))
                                        {
                                            pen = new Pen(Color.Red, 2);
                                        }
                                        else if (t.Event1Type == "conversation")
                                        {
                                            pen = new Pen(Color.Yellow, 2);
                                        }
                                        else if (t.Event1Type == "script")
                                        {
                                            pen = new Pen(Color.Blue, 2);
                                        }
                                        else if (t.Event1Type == "transition")
                                        {
                                            pen = new Pen(Color.Lime, 2);
                                        }
                                        Rectangle rect = new Rectangle(dx + 3, dy + 3, sqr - 6, sqr - 6);
                                        device.DrawRectangle(pen, rect);
                                    }
                                }
                            }
                            UpdatePB();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("failed on refresh map: " + ex.ToString());
                        }            
                    }
                    private void spritePropDraw(int cspx, int cspy, int spriteListIndex)
                    {
                        //source image
                        Bitmap prpBitmap = propBitmapList[spriteListIndex];
                        //Rectangle source = new Rectangle(0, 0, le_selectedProp.propBitmap.Width, le_selectedProp.propBitmap.Height);
                        Rectangle source = new Rectangle(0, 0, prpBitmap.Width, prpBitmap.Height);
                        //target location
                        Rectangle target = new Rectangle(cspx * sqr, cspy * sqr, sqr, sqr);
                        //draw sprite
                        device.DrawImage((Image)prpBitmap, target, source, GraphicsUnit.Pixel);
                    }
                    private void drawTileSettings()
                    {
                        //for (int index = 0; index < area.MapSizeInSquares.Width * area.MapSizeInSquares.Height; index++)
                        /*for (int x = 0; x < area.MapSizeX; x++)
                        {
                            for (int y = 0; y < area.MapSizeY; y++)
                            {
                                if (chkGrid.Checked) //if show grid is turned on, draw grid squares
                                {
                                    if (area.Tiles[y * this.area.MapSizeX + x].LoSBlocked)
                                    {
                                        Rectangle src = new Rectangle(0, 0, sqr, sqr);
                                        int dx = x * sqr;
                                        int dy = y * sqr;
                                        Rectangle target = new Rectangle(x * sqr, y * sqr, sqr, sqr);
                                        device.DrawImage(g_LoSBlock, target, src, GraphicsUnit.Pixel);
                                    }
                                    if (area.Tiles[y * this.area.MapSizeX + x].Walkable)
                                    {
                                        Rectangle src = new Rectangle(0, 0, sqr, sqr);
                                        int dx = x * sqr;
                                        int dy = y * sqr;
                                        Rectangle target = new Rectangle(x * sqr, y * sqr, sqr, sqr);
                                        device.DrawImage(g_walkPass, target, src, GraphicsUnit.Pixel);
                                    }
                                    else
                                    {
                                        Rectangle src = new Rectangle(0, 0, sqr, sqr);
                                        int dx = x * sqr;
                                        int dy = y * sqr;
                                        Rectangle target = new Rectangle(x * sqr, y * sqr, sqr, sqr);
                                        device.DrawImage(g_walkBlock, target, src, GraphicsUnit.Pixel);
                                    }
                                }
                            }
                        }*/
            /*foreach (Trigger t in area.Triggers)
            {
                foreach (Coordinate p in t.TriggerSquaresList)
                {
                    int dx = p.X * sqr;
                    int dy = p.Y * sqr;
                    Pen pen = new Pen(Color.Orange, 2);
                    if ((t.Event1Type == "encounter") || (t.Event2Type == "encounter") || (t.Event3Type == "encounter"))
                    {
                        pen = new Pen(Color.Red, 2);
                    }
                    else if (t.Event1Type == "conversation")
                    {
                        pen = new Pen(Color.Yellow, 2);
                    }
                    else if (t.Event1Type == "script")
                    {
                        pen = new Pen(Color.Blue, 2);
                    }
                    else if (t.Event1Type == "transition")
                    {
                        pen = new Pen(Color.Lime, 2);
                    }
                    Rectangle rect = new Rectangle(dx + 3, dy + 3, sqr - 6, sqr - 6);
                    device.DrawRectangle(pen, rect);
                }
            }*/
            //panelView.BackgroundImage = surface;
        }
        public void drawSelectionBox(int gridx, int gridy)
        {
            //GDI int dx = gridx * sqr;
            //GDI int dy = gridy * sqr;
            //draw selection box around tile                
            //GDI Pen pen = new Pen(Color.DarkMagenta, 2);
            //GDI Rectangle rect = new Rectangle(dx + 1, dy + 1, sqr - 2, sqr - 2);
            //GDI device.DrawRectangle(pen, rect);
            //save changes
            //GDI UpdatePB();            
        }
        public void UpdatePB()
        {
            //this.Cursor = Cursors.Default;
            //GDI panelView.BackgroundImage = surface;
            //GDI panelView.Invalidate();
        }

        private void panelView_MouseMove(object sender, MouseEventArgs e)
        {
            gridX = e.X / sqr;
            gridY = e.Y / sqr;
            if (!mouseInMapArea(gridX, gridY)) { return; }
            lblMouseInfo.Text = "gridX = " + gridX.ToString() + " : gridY = " + gridY.ToString();
            panelView.Focus();
            if (prntForm.PropSelected)
            {
                // TODO re-implement continuous drawing of props once converted to use Direct2D
                /*refreshMap(true);
                try
                {
                    if (selectedBitmap != null)
                    {
                        //source image size
                        Rectangle frame = new Rectangle(0, 0, selectedBitmap.Width, selectedBitmap.Height);
                        //target location
                        Rectangle target = new Rectangle(gridX * sqr, gridY * sqr, sqr, sqr);
                        //draw sprite
                        device.DrawImage((Image)selectedBitmap, target, frame, GraphicsUnit.Pixel);
                    }
                }
                catch (Exception ex) { MessageBox.Show("failed mouse move: " + ex.ToString()); }
                //save changes
                UpdatePB();
                */
            }
            else if (currentPoint != new Point(gridX, gridY))
            {
                //if painting tiles or walkable or Line-of-sight squares, allow multiple square painting if left mouse button down and move
                if (prntForm.CreatureSelected)
                {
                    return; //don't allow painting multiple creatures by mouse down and move
                }
                if (e.Button == MouseButtons.Left)
                {
                    //if painting tiles or walkable or Line-of-sight squares, allow multiple square painting if LEFT mouse button down and move
                    if ((rbtnPaintTile.Checked) || (rbtnWalkable.Checked) || (rbtnLoS.Checked))
                    {
                        clickDrawArea(e);
                    }
                }
                else if (e.Button == MouseButtons.Right)
                {
                    //if painting walkable or Line-of-sight squares, allow multiple square painting if RIGHT mouse button down and move
                    if ((rbtnWalkable.Checked) || (rbtnLoS.Checked))
                    {
                        clickDrawArea(e);
                    }
                }
            }//GDI             
        }
        private void panelView_MouseEnter(object sender, EventArgs e)
        {
            panelView.Select();
            try
            {
                if (prntForm.selectedLevelMapPropTag != "")
                {
                    prntForm.PropSelected = true;
                }
                if (prntForm.PropSelected)
                {
                    string selectedProp = prntForm.selectedLevelMapPropTag;
                    le_selectedProp = prntForm.getPropByTag(selectedProp);
                    if (le_selectedProp != null)
                    {
                        selectedBitmap = le_selectedProp.propBitmap;
                        selectedBitmapFilename = le_selectedProp.ImageFileName;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("failed on mouse enter map: " + ex.ToString());
            }
        }
        private void panelView_MouseClick(object sender, MouseEventArgs e)
        {
            clickDrawArea(e);
        }
        public bool mouseInMapArea(int gridX, int gridY)
        {
            if (gridX < 0) { return false; }
            if (gridY < 0) { return false; }
            if (gridX > area.MapSizeX - 1) { return false; }
            if (gridY > area.MapSizeY - 1) { return false; }
            return true;
        }
        private void clickDrawArea(MouseEventArgs e)
        {
            gridX = e.X / sqr;
            gridY = e.Y / sqr;
            lastSquareClicked.X = currentSquareClicked.X;
            lastSquareClicked.Y = currentSquareClicked.Y;
            currentSquareClicked.X = gridX;
            currentSquareClicked.Y = gridY;
            if (!mouseInMapArea(gridX, gridY))
            {
                return;
            }
            switch (e.Button)
            {
                #region Left Button
                case MouseButtons.Left:
                    refreshLeftPanelInfo();
                    prntForm.currentSelectedTrigger = null;
                    #region Tile Selected
                    if (rbtnPaintTile.Checked)
                    {
                        //gridX = e.X / sqr;
                        //gridY = e.Y / sqr;
                        selectedTile.index = gridY * area.MapSizeX + gridX;
                        prntForm.logText("gridx = " + gridX.ToString() + "gridy = " + gridY.ToString());
                        prntForm.logText(Environment.NewLine);
                        #region Layer 1
                        if (radioButton1.Checked)
                        {
                            area.Layer1Filename[selectedTile.index] = currentTileFilename;
                            area.Layer1Rotate[selectedTile.index] = tileToBePlaced.angle;
                            area.Layer1Mirror[selectedTile.index] = tileToBePlaced.mirror;
                            //if shift key is down, draw all between here and lastclickedsquare
                            if (Control.ModifierKeys == Keys.Shift)
                            {
                                Point cSqr = new Point(currentSquareClicked.X, currentSquareClicked.Y);
                                Point lSqr = new Point(lastSquareClicked.X, lastSquareClicked.Y);

                                int startX = lSqr.X;
                                int startY = lSqr.Y;
                                int endX = cSqr.X;
                                int endY = cSqr.Y;
                                if (lSqr.X >= cSqr.X)
                                {
                                    startX = cSqr.X;
                                    endX = lSqr.X;
                                }
                                if (lSqr.Y >= cSqr.Y)
                                {
                                    startY = cSqr.Y;
                                    endY = lSqr.Y;
                                }
                                for (int x = startX; x <= endX; x++)
                                {
                                    for (int y = startY; y <= endY; y++)
                                    {
                                        area.Layer1Filename[y * area.MapSizeX + x] = currentTileFilename;
                                        area.Layer1Rotate[y * area.MapSizeX + x] = tileToBePlaced.angle;
                                        area.Layer1Mirror[y * area.MapSizeX + x] = tileToBePlaced.mirror;
                                        currentSquareClicked = new Point(x, y);
                                        //GDI refreshMap(false);
                                    }
                                }
                            }
                        }
                        #endregion
                        #region Layer 2
                        else if (radioButton2.Checked)
                        {
                            area.Layer2Filename[selectedTile.index] = currentTileFilename;
                            area.Layer2Rotate[selectedTile.index] = tileToBePlaced.angle;
                            area.Layer2Mirror[selectedTile.index] = tileToBePlaced.mirror;
                            if (Control.ModifierKeys == Keys.Shift)
                            {
                                Point cSqr = new Point(currentSquareClicked.X, currentSquareClicked.Y);
                                Point lSqr = new Point(lastSquareClicked.X, lastSquareClicked.Y);
                                int startX = lSqr.X;
                                int startY = lSqr.Y;
                                int endX = cSqr.X;
                                int endY = cSqr.Y;
                                if (lSqr.X >= cSqr.X)
                                {
                                    startX = cSqr.X;
                                    endX = lSqr.X;
                                }
                                if (lSqr.Y >= cSqr.Y)
                                {
                                    startY = cSqr.Y;
                                    endY = lSqr.Y;
                                }
                                for (int x = startX; x <= endX; x++)
                                {
                                    for (int y = startY; y <= endY; y++)
                                    {
                                        area.Layer2Filename[y * area.MapSizeX + x] = currentTileFilename;
                                        area.Layer2Rotate[y * area.MapSizeX + x] = tileToBePlaced.angle;
                                        area.Layer2Mirror[y * area.MapSizeX + x] = tileToBePlaced.mirror;
                                        currentSquareClicked = new Point(x, y);
                                    }
                                }
                            }
                        }
                        #endregion
                        #region Layer 3
                        else if (radioButton3.Checked)
                        {
                            area.Layer3Filename[selectedTile.index] = currentTileFilename;
                            area.Layer3Rotate[selectedTile.index] = tileToBePlaced.angle;
                            area.Layer3Mirror[selectedTile.index] = tileToBePlaced.mirror;
                            if (Control.ModifierKeys == Keys.Shift)
                            {
                                Point cSqr = new Point(currentSquareClicked.X, currentSquareClicked.Y);
                                Point lSqr = new Point(lastSquareClicked.X, lastSquareClicked.Y);
                                int startX = lSqr.X;
                                int startY = lSqr.Y;
                                int endX = cSqr.X;
                                int endY = cSqr.Y;
                                if (lSqr.X >= cSqr.X)
                                {
                                    startX = cSqr.X;
                                    endX = lSqr.X;
                                }
                                if (lSqr.Y >= cSqr.Y)
                                {
                                    startY = cSqr.Y;
                                    endY = lSqr.Y;
                                }
                                for (int x = startX; x <= endX; x++)
                                {
                                    for (int y = startY; y <= endY; y++)
                                    {
                                        area.Layer3Filename[y * area.MapSizeX + x] = currentTileFilename;
                                        area.Layer3Rotate[y * area.MapSizeX + x] = tileToBePlaced.angle;
                                        area.Layer3Mirror[y * area.MapSizeX + x] = tileToBePlaced.mirror;
                                        currentSquareClicked = new Point(x, y);
                                    }
                                }
                            }
                        }
                        #endregion
                    }
                    #endregion
                    #region Prop Selected
                    else if (prntForm.PropSelected)
                    {
                        string selectedProp = prntForm.selectedLevelMapPropTag;
                        prntForm.logText(selectedProp);
                        prntForm.logText(Environment.NewLine);

                        prntForm.logText("gridx = " + gridX.ToString() + "gridy = " + gridY.ToString());
                        prntForm.logText(Environment.NewLine);
                        // verify that there is no creature, blocked, or PC already on this location
                        // add to a List<> a new item with the x,y coordinates
                        if (le_selectedProp.ImageFileName == "blank")
                        {
                            return;
                        }
                        Prop newProp = new Prop();
                        newProp = le_selectedProp.DeepCopy();
                        newProp.PropTag = le_selectedProp.PropTag + "_" + prntForm.mod.nextIdNumber;
                        newProp.LocationX = gridX;
                        newProp.LocationY = gridY;
                        area.Props.Add(newProp);                        
                    }
                    #endregion
                    #region Paint New Trigger Selected
                    else if (rbtnPaintTrigger.Checked)
                    {
                        string selectedTrigger = prntForm.selectedLevelMapTriggerTag;
                        prntForm.logText(selectedTrigger);
                        prntForm.logText(Environment.NewLine);

                        prntForm.logText("gridx = " + gridX.ToString() + "gridy = " + gridY.ToString());
                        prntForm.logText(Environment.NewLine);
                        Point newPoint = new Point(gridX, gridY);
                        //add the selected square to the squareList if doesn't already exist
                        try
                        {
                            //check: if click square already exists, then erase from list                            
                            Trigger newTrigger = area.getTriggerByTag(selectedTrigger);
                            bool exists = false;
                            foreach (Coordinate p in newTrigger.TriggerSquaresList)
                            {
                                if ((p.X == newPoint.X) && (p.Y == newPoint.Y))
                                {
                                    //already exists, erase
                                    newTrigger.TriggerSquaresList.Remove(p);
                                    exists = true;
                                    break;
                                }
                            }
                            if (!exists) //doesn't exist so is a new point, add to list
                            {
                                Coordinate newCoor = new Coordinate();
                                newCoor.X = newPoint.X;
                                newCoor.Y = newPoint.Y;
                                newTrigger.TriggerSquaresList.Add(newCoor);
                            }
                            prntForm.currentSelectedTrigger = newTrigger;
                            prntForm.frmTriggerEvents.refreshTriggers();
                        }
                        catch
                        {
                            MessageBox.Show("The tag of the selected Trigger was not found in the area's trigger list");
                        }
                    }
                    #endregion
                    #region Edit Trigger Selected
                    else if (rbtnEditTrigger.Checked)
                    {
                        if (prntForm.selectedLevelMapTriggerTag != null)
                        {
                            string selectedTrigger = prntForm.selectedLevelMapTriggerTag;
                            prntForm.logText(selectedTrigger);
                            prntForm.logText(Environment.NewLine);

                            prntForm.logText("gridx = " + gridX.ToString() + "gridy = " + gridY.ToString());
                            prntForm.logText(Environment.NewLine);
                            Point newPoint = new Point(gridX, gridY);
                            try
                            {
                                //check: if click square already exists, then erase from list  
                                Trigger newTrigger = area.getTriggerByTag(selectedTrigger);
                                if (newTrigger == null)
                                {
                                    MessageBox.Show("error: make sure to select a trigger to edit first (click info button then click on trigger)");
                                }
                                bool exists = false;
                                foreach (Coordinate p in newTrigger.TriggerSquaresList)
                                {
                                    if ((p.X == newPoint.X) && (p.Y == newPoint.Y))
                                    {
                                        //already exists, erase
                                        newTrigger.TriggerSquaresList.Remove(p);
                                        exists = true;
                                        break;
                                    }
                                }
                                if (!exists) //doesn't exist so is a new point, add to list
                                {
                                    Coordinate newCoor = new Coordinate();
                                    newCoor.X = newPoint.X;
                                    newCoor.Y = newPoint.Y;
                                    newTrigger.TriggerSquaresList.Add(newCoor);
                                }
                            }
                            catch
                            {
                                MessageBox.Show("The tag of the selected Trigger was not found in the area's trigger list");
                            }
                        }
                    }
                    #endregion
                    #region Walkmesh Toggle Selected (Make Non-Walkable)
                    else if (rbtnWalkable.Checked)
                    {
                        selectedTile.index = gridY * area.MapSizeX + gridX;
                        prntForm.logText("gridx = " + gridX.ToString() + "gridy = " + gridY.ToString());
                        prntForm.logText(Environment.NewLine);
                        area.Walkable[selectedTile.index] = 0;
                        //GDI refreshMap(false);
                    }
                    #endregion
                    #region LoS mesh Toggle Selected (Make LoS Blocked)
                    else if (rbtnLoS.Checked)
                    {
                        selectedTile.index = gridY * area.MapSizeX + gridX;
                        prntForm.logText("gridx = " + gridX.ToString() + "gridy = " + gridY.ToString());
                        prntForm.logText(Environment.NewLine);
                        area.LoSBlocked[selectedTile.index] = 1;
                        //GDI refreshMap(false);
                    }
                    #endregion
                    #region None Selected
                    else // not placing, just getting info and possibly deleteing icon
                    {
                        contextMenuStrip1.Items.Clear();
                        //when left click, get location
                        //gridX = e.X / sqr;
                        //gridY = e.Y / sqr;
                        Point newPoint = new Point(gridX, gridY);
                        EventHandler handler = new EventHandler(HandleContextMenuClick);
                        //loop through all the objects
                        //if has that location, add the tag to the list                    
                        //draw selection box
                        //GDI refreshMap(false);
                        //GDI drawSelectionBox(gridX, gridY);
                        selectionBoxLocation = new Point(gridX, gridY);
                        txtSelectedIconInfo.Text = "";

                        foreach (Prop prp in area.Props)
                        {
                            if ((prp.LocationX == newPoint.X) && (prp.LocationY == newPoint.Y))
                            {
                                // if so then give details about that icon (name, tag, etc.)
                                txtSelectedIconInfo.Text = "name: " + prp.ImageFileName + Environment.NewLine
                                                            + "tag: " + prp.PropTag + Environment.NewLine;
                                lastSelectedObjectTag = prp.PropTag;
                                //prntForm.selectedLevelMapPropTag = prp.PropTag;
                                panelView.ContextMenuStrip.Items.Add(prp.PropTag, null, handler); //string, image, handler
                                prp.PassInParentForm(prntForm);
                                prntForm.frmIceBlinkProperties.propertyGrid1.SelectedObject = prp;
                            }
                        }
                        foreach (Trigger t in area.Triggers)
                        {
                            foreach (Coordinate p in t.TriggerSquaresList)
                            {
                                if ((p.X == newPoint.X) && (p.Y == newPoint.Y))
                                {
                                    txtSelectedIconInfo.Text = "Trigger Tag: " + Environment.NewLine + t.TriggerTag;
                                    lastSelectedObjectTag = t.TriggerTag;
                                    prntForm.currentSelectedTrigger = t;
                                    prntForm.frmTriggerEvents.refreshTriggers();
                                    panelView.ContextMenuStrip.Items.Add(t.TriggerTag, null, handler); //string, image, handler
                                    //prntForm.frmIceBlinkProperties.propertyGrid1.SelectedObject = t;
                                }
                            }
                        }
                        //if the list is less than 2, do nothing
                        if (panelView.ContextMenuStrip.Items.Count > 1)
                        {
                            contextMenuStrip1.Show(panelView, e.Location);
                        }
                        prntForm.frmTriggerEvents.refreshTriggers();
                    }
                    #endregion
                    break;
                #endregion
                #region Right Button
                case MouseButtons.Right:
                    #region Walkmesh Toggle Selected (Make Walkable)
                    if (rbtnWalkable.Checked)
                    {
                        selectedTile.index = gridY * area.MapSizeX + gridX;
                        prntForm.logText("gridx = " + gridX.ToString() + "gridy = " + gridY.ToString());
                        prntForm.logText(Environment.NewLine);
                        area.Walkable[selectedTile.index] = 1;
                        //GDI refreshMap(false);
                    }
                    #endregion
                    #region LoS mesh Toggle Selected (Make LoS Visible)
                    else if (rbtnLoS.Checked)
                    {
                        selectedTile.index = gridY * area.MapSizeX + gridX;
                        prntForm.logText("gridx = " + gridX.ToString() + "gridy = " + gridY.ToString());
                        prntForm.logText(Environment.NewLine);
                        area.LoSBlocked[selectedTile.index] = 0;
                        //GDI refreshMap(false);
                    }
                    #endregion
                    else
                    {
                        // exit by right click or ESC
                        prntForm.logText("entered right-click");
                        prntForm.logText(Environment.NewLine);
                        prntForm.selectedLevelMapCreatureTag = "";
                        prntForm.selectedLevelMapPropTag = "";
                        prntForm.CreatureSelected = false;
                        prntForm.PropSelected = false;
                        prntForm.currentSelectedTrigger = null;
                        //GDI refreshMap(true);
                        //GDI UpdatePB();
                        rbtnInfo.Checked = true;
                        resetTileToBePlacedSettings();
                    }
                    break;
                #endregion
            }
        }
        
        private void btnFillWithSelected_Click(object sender, EventArgs e)
        {
            for (int x = 0; x < area.MapSizeX; x++)
            {
                for (int y = 0; y < area.MapSizeY; y++)
                {
                    selectedTile.index = y * area.MapSizeX + x;
                    if (radioButton1.Checked)
                    {
                        area.Layer1Filename[selectedTile.index] = currentTileFilename;
                    }
                    else if (radioButton2.Checked)
                    {
                        area.Layer2Filename[selectedTile.index] = currentTileFilename;
                    }
                    else if (radioButton3.Checked)
                    {
                        area.Layer3Filename[selectedTile.index] = currentTileFilename;
                    }
                }
            }
            //GDI refreshMap(true);
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            //GDI refreshMap(true);
        }
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            //GDI refreshMap(true);
        }
        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            //GDI refreshMap(true);
        }
        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            //GDI refreshMap(true);
        }
        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            //GDI refreshMap(true);
        }

        #region DIRECT2D STUFF
        private void InitDirect2DAndDirectWrite()
        {
            Factory2D = new SharpDX.Direct2D1.Factory();
            FactoryDWrite = new SharpDX.DirectWrite.Factory();

            var properties = new HwndRenderTargetProperties();
            properties.Hwnd = panelView.Handle;
            properties.PixelSize = new SharpDX.Size2(panelView.ClientSize.Width, panelView.ClientSize.Height);
            properties.PresentOptions = PresentOptions.None;

            RenderTarget2D = new WindowRenderTarget(Factory2D, new RenderTargetProperties(new SharpDX.Direct2D1.PixelFormat(Format.R8G8B8A8_UNorm, AlphaMode.Premultiplied)), properties);
            RenderTarget2D.AntialiasMode = AntialiasMode.PerPrimitive;
            RenderTarget2D.TextAntialiasMode = TextAntialiasMode.Cleartype;

            SceneColorBrush = new SolidColorBrush(RenderTarget2D, SharpDX.Color.Black);            
            timerRenderLoop.Start();
        }
        public void DrawD2DBitmap(SharpDX.Direct2D1.Bitmap bitmap, SharpDX.RectangleF source, SharpDX.RectangleF target, int angleInDegrees, int mirror)
        {
            int mir = 1;
            if (mirror == 1) { mir = -1; }
            //convert degrees to radians
            float angle = (float)(Math.PI * 2 * (float)angleInDegrees / (float)360);
            SharpDX.Vector2 center = new SharpDX.Vector2(target.Left + (target.Width / 2), target.Top + (target.Height / 2));
            RenderTarget2D.Transform = SharpDX.Matrix.Transformation2D(center, 0, new SharpDX.Vector2(mir * 1.0f, 1.0f), center, angle, new SharpDX.Vector2(0, 0));
            SharpDX.RectangleF trg = new SharpDX.RectangleF(target.Left, target.Top, target.Width, target.Height);
            SharpDX.RectangleF src = new SharpDX.RectangleF(source.Left, source.Top, source.Width, source.Height);
            RenderTarget2D.DrawBitmap(bitmap, trg, 1.0f, BitmapInterpolationMode.NearestNeighbor, src);
            RenderTarget2D.Transform = SharpDX.Matrix3x2.Identity;
        }
        public void DrawRectangle(SharpDX.RectangleF rect, SharpDX.Color penColor, int penWidth)
        {
            using (SolidColorBrush scb = new SolidColorBrush(RenderTarget2D, penColor))
            {
                RenderTarget2D.DrawRectangle(rect, scb, penWidth);
            }
        }
        public SharpDX.Direct2D1.Bitmap ConvertGDIBitmapToD2D(System.Drawing.Bitmap gdibitmap)
        {
            var sourceArea = new System.Drawing.Rectangle(0, 0, gdibitmap.Width, gdibitmap.Height);
            var bitmapProperties = new BitmapProperties(new SharpDX.Direct2D1.PixelFormat(Format.R8G8B8A8_UNorm, AlphaMode.Premultiplied));
            var size = new SharpDX.Size2(gdibitmap.Width, gdibitmap.Height);

            // Transform pixels from BGRA to RGBA
            int stride = gdibitmap.Width * sizeof(int);
            using (var tempStream = new SharpDX.DataStream(gdibitmap.Height * stride, true, true))
            {
                // Lock System.Drawing.Bitmap
                var bitmapData = gdibitmap.LockBits(sourceArea, ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);

                // Convert all pixels 
                for (int y = 0; y < gdibitmap.Height; y++)
                {
                    int offset = bitmapData.Stride * y;
                    for (int x = 0; x < gdibitmap.Width; x++)
                    {
                        // Not optimized 
                        byte B = Marshal.ReadByte(bitmapData.Scan0, offset++);
                        byte G = Marshal.ReadByte(bitmapData.Scan0, offset++);
                        byte R = Marshal.ReadByte(bitmapData.Scan0, offset++);
                        byte A = Marshal.ReadByte(bitmapData.Scan0, offset++);
                        int rgba = R | (G << 8) | (B << 16) | (A << 24);
                        tempStream.Write(rgba);
                    }
                }
                gdibitmap.UnlockBits(bitmapData);
                tempStream.Position = 0;
                return new SharpDX.Direct2D1.Bitmap(RenderTarget2D, size, tempStream, stride, bitmapProperties);
            }
        }
        public SharpDX.Direct2D1.Bitmap LoadBitmap(string file) //change this to LoadBitmap
        {
            // Loads from file using System.Drawing.Image
            using (var bitmap = prntForm.LoadBitmapGDI(file)) //change this to LoadBitmapGDI
            {
                var sourceArea = new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height);
                var bitmapProperties = new BitmapProperties(new SharpDX.Direct2D1.PixelFormat(Format.R8G8B8A8_UNorm, AlphaMode.Premultiplied));
                var size = new SharpDX.Size2(bitmap.Width, bitmap.Height);

                // Transform pixels from BGRA to RGBA
                int stride = bitmap.Width * sizeof(int);
                using (var tempStream = new SharpDX.DataStream(bitmap.Height * stride, true, true))
                {
                    // Lock System.Drawing.Bitmap
                    var bitmapData = bitmap.LockBits(sourceArea, ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);

                    // Convert all pixels 
                    for (int y = 0; y < bitmap.Height; y++)
                    {
                        int offset = bitmapData.Stride * y;
                        for (int x = 0; x < bitmap.Width; x++)
                        {
                            // Not optimized 
                            byte B = System.Runtime.InteropServices.Marshal.ReadByte(bitmapData.Scan0, offset++);
                            byte G = System.Runtime.InteropServices.Marshal.ReadByte(bitmapData.Scan0, offset++);
                            byte R = System.Runtime.InteropServices.Marshal.ReadByte(bitmapData.Scan0, offset++);
                            byte A = System.Runtime.InteropServices.Marshal.ReadByte(bitmapData.Scan0, offset++);
                            int rgba = R | (G << 8) | (B << 16) | (A << 24);
                            tempStream.Write(rgba);
                        }
                    }
                    bitmap.UnlockBits(bitmapData);
                    tempStream.Position = 0;
                    return new SharpDX.Direct2D1.Bitmap(RenderTarget2D, size, tempStream, stride, bitmapProperties);
                }
            }
        }
        public SharpDX.Direct2D1.Bitmap GetFromBitmapList(string fileNameWithOutExt)
        {
            //check to see if in list already and return bitmap if it is found
            if (commonBitmapList.ContainsKey(fileNameWithOutExt))
            {
                return commonBitmapList[fileNameWithOutExt];
            }
            //try loading and adding to list and return bitmap
            else
            {
                commonBitmapList.Add(fileNameWithOutExt, LoadBitmap(fileNameWithOutExt));
                return commonBitmapList[fileNameWithOutExt];
            }
        }
        public void DisposeAllD2D()
        {
            //dispose of all bitmaps
            foreach(SharpDX.Direct2D1.Bitmap bm in commonBitmapList.Values)
            {
                if (bm != null)
                {
                    bm.Dispose();
                }
            }
            commonBitmapList.Clear();
            SceneColorBrush.Dispose();
            RenderTarget2D.Dispose();
            FactoryDWrite.Dispose();
            Factory2D.Dispose();
        }
        public void DisposeOfBitmap(ref SharpDX.Direct2D1.Bitmap bmp)
        {
            if (bmp != null)
            {
                bmp.Dispose();
                bmp = null;
            }
        }
        public void Render()
        {
            try
            {
                RenderTarget2D.BeginDraw();
                RenderTarget2D.Clear(SharpDX.Color.Black);
                redrawMain();
                RenderTarget2D.EndDraw();
            }
            catch (Exception ex)
            {
                prntForm.errorLog(ex.ToString());
            }
        }
        public void redrawMain()
        {            
            #region Draw Layer 1
            if (checkBox1.Checked)
            {
                for (int y = 0; y < area.MapSizeY; y++)
                {
                    for (int x = 0; x < area.MapSizeX; x++)
                    {                        
                        string tile = area.Layer1Filename[y * area.MapSizeX + x];
                        if (!tile.Equals("t_a_blank"))
                        {
                            float scalerX = GetFromBitmapList(tile).PixelSize.Width / prntForm.tileSizeInPixels;
                            if (scalerX == 0) { scalerX = 1.0f; }
                            float scalerY = GetFromBitmapList(tile).PixelSize.Height / prntForm.tileSizeInPixels;
                            if (scalerY == 0) { scalerY = 1.0f; }
                            SharpDX.RectangleF src = new SharpDX.RectangleF(0, 0, GetFromBitmapList(tile).PixelSize.Width, GetFromBitmapList(tile).PixelSize.Height);
                            SharpDX.RectangleF dst = new SharpDX.RectangleF(x * sqr, y * sqr, (int)(sqr * scalerX), (int)(sqr * scalerY));
                            DrawD2DBitmap(GetFromBitmapList(tile), src, dst, area.Layer1Rotate[y * area.MapSizeX + x], area.Layer1Mirror[y * area.MapSizeX + x]);
                        }
                    }
                }
            }
            #endregion
            #region Draw Layer 2
            if (checkBox2.Checked)
            {
                for (int y = 0; y < area.MapSizeY; y++)
                {
                    for (int x = 0; x < area.MapSizeX; x++)
                    {
                        string tile = area.Layer2Filename[y * area.MapSizeX + x];
                        if (!tile.Equals("t_a_blank"))
                        {
                            float scalerX = GetFromBitmapList(tile).PixelSize.Width / prntForm.tileSizeInPixels;
                            if (scalerX == 0) { scalerX = 1.0f; }
                            float scalerY = GetFromBitmapList(tile).PixelSize.Height / prntForm.tileSizeInPixels;
                            if (scalerY == 0) { scalerY = 1.0f; }
                            SharpDX.RectangleF src = new SharpDX.RectangleF(0, 0, GetFromBitmapList(tile).PixelSize.Width, GetFromBitmapList(tile).PixelSize.Height);
                            SharpDX.RectangleF dst = new SharpDX.RectangleF(x * sqr, y * sqr, (int)(sqr * scalerX), (int)(sqr * scalerY));
                            DrawD2DBitmap(GetFromBitmapList(tile), src, dst, area.Layer2Rotate[y * area.MapSizeX + x], area.Layer2Mirror[y * area.MapSizeX + x]);
                        }
                    }
                }
            }
            #endregion
            #region Draw Layer 3
            if (checkBox3.Checked)
            {
                for (int y = 0; y < area.MapSizeY; y++)
                {
                    for (int x = 0; x < area.MapSizeX; x++)
                    {
                        string tile = area.Layer3Filename[y * area.MapSizeX + x];
                        if (!tile.Equals("t_a_blank"))
                        {
                            float scalerX = GetFromBitmapList(tile).PixelSize.Width / prntForm.tileSizeInPixels;
                            if (scalerX == 0) { scalerX = 1.0f; }
                            float scalerY = GetFromBitmapList(tile).PixelSize.Height / prntForm.tileSizeInPixels;
                            if (scalerY == 0) { scalerY = 1.0f; }
                            SharpDX.RectangleF src = new SharpDX.RectangleF(0, 0, GetFromBitmapList(tile).PixelSize.Width, GetFromBitmapList(tile).PixelSize.Height);
                            SharpDX.RectangleF dst = new SharpDX.RectangleF(x * sqr, y * sqr, (int)(sqr * scalerX), (int)(sqr * scalerY));
                            DrawD2DBitmap(GetFromBitmapList(tile), src, dst, area.Layer3Rotate[y * area.MapSizeX + x], area.Layer3Mirror[y * area.MapSizeX + x]);
                        }
                    }
                }
            }
            #endregion

            #region Draw Grid
            for (int y = 0; y < area.MapSizeY; y++)
            {
                for (int x = 0; x < area.MapSizeX; x++)
                {                    
                    //Tile tile = area.Tiles[y * area.MapSizeX + x];                    
                    //draw square walkmesh and LoS stuff
                    //SharpDX.Direct2D1.Bitmap bm = GetFromBitmapList("walk_pass");
                    //Rectangle src = new Rectangle(0, 0, g_walkPass.Width, g_walkPass.Height);
                    //Rectangle target = new Rectangle(x * sqr, y * sqr, sqr, sqr);
                    SharpDX.RectangleF src = new SharpDX.RectangleF(0, 0, GetFromBitmapList("walk_pass").PixelSize.Width, GetFromBitmapList("walk_pass").PixelSize.Height);
                    SharpDX.RectangleF dst = new SharpDX.RectangleF(x * sqr, y * sqr, sqr, sqr);
                    if (chkGrid.Checked) //if show grid is turned on, draw grid squares
                    {
                        if (area.LoSBlocked[y * area.MapSizeX + x] == 1)
                        {
                            DrawD2DBitmap(GetFromBitmapList("los_block"), src, dst, 0, 0);
                        }
                        if (area.Walkable[y * area.MapSizeX + x] == 1)
                        {
                            DrawD2DBitmap(GetFromBitmapList("walk_pass"), src, dst, 0, 0);
                        }
                        else
                        {
                            DrawD2DBitmap(GetFromBitmapList("walk_block"), src, dst, 0, 0);
                            
                        }
                    }                   
                }
            }
            #endregion

            #region Draw Props
            foreach (Prop prpRef in area.Props)
            {
                float scalerX = GetFromBitmapList(prpRef.ImageFileName).PixelSize.Width / prntForm.standardTokenSize;
                float scalerY = GetFromBitmapList(prpRef.ImageFileName).PixelSize.Height / prntForm.standardTokenSize;
                SharpDX.RectangleF src = new SharpDX.RectangleF(0, 0, GetFromBitmapList(prpRef.ImageFileName).PixelSize.Width, GetFromBitmapList(prpRef.ImageFileName).PixelSize.Height);
                SharpDX.RectangleF dst = new SharpDX.RectangleF(prpRef.LocationX * sqr, prpRef.LocationY * sqr, (int)(sqr * scalerX), (int)(sqr * scalerY));
                if (prpRef.ImageFileName.StartsWith("tkn_"))
                {
                    scalerX = GetFromBitmapList(prpRef.ImageFileName).PixelSize.Width / prntForm.standardTokenSize;
                    scalerY = (GetFromBitmapList(prpRef.ImageFileName).PixelSize.Height / 2) / prntForm.standardTokenSize;
                    src = new SharpDX.RectangleF(0, 0, GetFromBitmapList(prpRef.ImageFileName).PixelSize.Width, (GetFromBitmapList(prpRef.ImageFileName).PixelSize.Height) / 2);
                    dst = new SharpDX.RectangleF(prpRef.LocationX * sqr, prpRef.LocationY * sqr, (int)(sqr * scalerX), (int)(sqr * scalerY));
                }
                int mirror = 0;
                if (!prpRef.PropFacingLeft) { mirror = 1; }
                DrawD2DBitmap(GetFromBitmapList(prpRef.ImageFileName), src, dst, 0, mirror);
            }
            #endregion
            #region Draw Triggers
            foreach (Trigger t in area.Triggers)
            {
                foreach (Coordinate p in t.TriggerSquaresList)
                {
                    int dx = p.X * sqr;
                    int dy = p.Y * sqr;
                    //Pen pen = new Pen(Color.Orange, 2);
                    SharpDX.Color clr = SharpDX.Color.Orange;
                    if ((t.Event1Type == "encounter") || (t.Event2Type == "encounter") || (t.Event3Type == "encounter"))
                    {
                        clr = SharpDX.Color.Red;
                    }
                    else if (t.Event1Type == "conversation")
                    {
                        clr = SharpDX.Color.Yellow;
                    }
                    else if (t.Event1Type == "script")
                    {
                        clr = SharpDX.Color.Blue;
                    }
                    else if (t.Event1Type == "transition")
                    {
                        clr = SharpDX.Color.Lime;
                    }
                    SharpDX.RectangleF rect = new SharpDX.RectangleF(dx + 3, dy + 3, sqr - 6, sqr - 6);
                    DrawRectangle(rect, clr, 2);
                }
            }
            #endregion
            #region Draw Selection Box
            if (selectionBoxLocation.X != -1)
            {
                int dx = selectionBoxLocation.X * sqr;
                int dy = selectionBoxLocation.Y * sqr;
                SharpDX.RectangleF rect = new SharpDX.RectangleF(dx + 1, dy + 1, sqr - 2, sqr - 2);
                DrawRectangle(rect, SharpDX.Color.Magenta, 2);
            }
            #endregion
            #region Draw To Be Placed Prop/Tile
            if (prntForm.PropSelected)
            {
                try
                {
                    if (!selectedBitmapFilename.Equals(""))
                    {
                        SharpDX.RectangleF src = new SharpDX.RectangleF(0, 0, GetFromBitmapList(selectedBitmapFilename).PixelSize.Width, GetFromBitmapList(selectedBitmapFilename).PixelSize.Height);
                        SharpDX.RectangleF dst = new SharpDX.RectangleF(gridX * sqr, gridY * sqr, sqr, sqr);
                        DrawD2DBitmap(GetFromBitmapList(selectedBitmapFilename), src, dst, 0, 0);
                    }
                }
                catch (Exception ex) { MessageBox.Show("failed mouse move update to be placed prop: " + ex.ToString()); }
            }
            else if (rbtnPaintTile.Checked)
            {
                try
                {
                    if (!currentTileFilename.Equals(""))
                    {
                        float scalerX = GetFromBitmapList(currentTileFilename).PixelSize.Width / prntForm.tileSizeInPixels;
                        float scalerY = GetFromBitmapList(currentTileFilename).PixelSize.Height / prntForm.tileSizeInPixels;
                        SharpDX.RectangleF src = new SharpDX.RectangleF(0, 0, GetFromBitmapList(currentTileFilename).PixelSize.Width, GetFromBitmapList(currentTileFilename).PixelSize.Height);
                        SharpDX.RectangleF dst = new SharpDX.RectangleF(gridX * sqr, gridY * sqr, (int)(sqr), (int)(sqr));
                        int mirror = 0;
                        if (tileToBePlaced.mirror == 1) { mirror = 1; }
                        DrawD2DBitmap(GetFromBitmapList(currentTileFilename), src, dst, tileToBePlaced.angle, mirror);
                    }
                }
                catch (Exception ex) { MessageBox.Show("failed mouse move update to be placed tile: " + ex.ToString()); }
            }
            #endregion
        }
        #endregion

        #region Methods
        private void loadAreaObjectBitmapLists()
        {
            /*//GDI foreach (Prop prp in area.Props)
            {
                // get Prop by Tag and then get Icon filename, add Bitmap to list
                if (File.Exists(prntForm._mainDirectory + "\\modules\\" + prntForm.mod.moduleName + "\\graphics\\" + prp.ImageFileName + ".png"))
                {
                    Bitmap newBitmap = new Bitmap(prntForm._mainDirectory + "\\modules\\" + prntForm.mod.moduleName + "\\graphics\\" + prp.ImageFileName + ".png");
                    propBitmapList.Add(newBitmap);
                }
                else
                {
                    MessageBox.Show("Failed to find prop image (" + prp.ImageFileName + ") in graphics folder");
                }
            }*/
        }
        private void openLevel(string g_dir, string g_fil, string g_filNoEx)
        {
            //this.Cursor = Cursors.WaitCursor;

            try
            {
                area = area.loadAreaFile(g_dir + "\\" + g_fil + ".lvl");
                if (area == null)
                {
                    MessageBox.Show("returned a null area");
                }
                loadAreaObjectBitmapLists();
            }
            catch (Exception ex)
            {
                MessageBox.Show("failed to open file: " + ex.ToString());
            }
            // load JPG Map first
            /*//GDI try
            {
                if (!area.ImageFileName.Equals("none"))
                {
                    if (File.Exists(prntForm._mainDirectory + "\\modules\\" + prntForm.mod.moduleName + "\\graphics\\" + area.ImageFileName + ".jpg"))
                    {
                        gameMapBitmap = new Bitmap(prntForm._mainDirectory + "\\modules\\" + prntForm.mod.moduleName + "\\graphics\\" + area.ImageFileName + ".jpg");
                    }
                    else if (File.Exists(prntForm._mainDirectory + "\\modules\\" + prntForm.mod.moduleName + "\\graphics\\" + area.ImageFileName))
                    {
                        gameMapBitmap = new Bitmap(prntForm._mainDirectory + "\\modules\\" + prntForm.mod.moduleName + "\\graphics\\" + area.ImageFileName);
                    }
                    else if (File.Exists(prntForm._mainDirectory + "\\modules\\" + prntForm.mod.moduleName + "\\graphics\\" + area.ImageFileName + ".png"))
                    {
                        gameMapBitmap = new Bitmap(prntForm._mainDirectory + "\\modules\\" + prntForm.mod.moduleName + "\\graphics\\" + area.ImageFileName + ".png");
                    }
                    else
                    {
                        gameMapBitmap = null;
                    }
                }
            }
            catch
            {
                gameMapBitmap = null;
            }*/


            if (useDirect2D)
            {
                //TODO add D2D stuff
                InitDirect2DAndDirectWrite();
            }
            /*//GDI else
            {
                panelView.Width = area.MapSizeX * sqr;
                panelView.Height = area.MapSizeY * sqr;
                panelView.BackgroundImage = (Image)surface;
                device = Graphics.FromImage(surface);
                if (surface == null)
                {
                    MessageBox.Show("returned a null Map bitmap");
                    return;
                }
            }*/
            refreshLeftPanelInfo();
            //GDI refreshMap(true);
            //this.Cursor = Cursors.Arrow;
        }
        private void createNewArea(int width, int height)
        {
            //create tilemap
            area = null;
            area = new Area();
            area.MapSizeX = width;
            area.MapSizeY = height;
            for (int index = 0; index < (width * height); index++)
            {
                area.Layer1Filename.Add("t_f_grass");
                area.Layer1Rotate.Add(0);
                area.Layer1Mirror.Add(0);
                area.Layer2Filename.Add("t_a_blank");
                area.Layer2Rotate.Add(0);
                area.Layer2Mirror.Add(0);
                area.Layer3Filename.Add("t_a_blank");
                area.Layer3Rotate.Add(0);
                area.Layer3Mirror.Add(0);
                area.Walkable.Add(1);
                area.LoSBlocked.Add(0);
                area.Visible.Add(0);
            }
            refreshLeftPanelInfo();
        }        
        public void refreshLeftPanelInfo()
        {
            lblMapSizeX.Text = area.MapSizeX.ToString();
            lblMapSizeY.Text = area.MapSizeY.ToString();
            //numBGLocX.Value = area.backgroundImageStartLocX;
            //numBGLocY.Value = area.backgroundImageStartLocY;
            selectedTile.x = gridX;
            selectedTile.y = gridY;
            selectedTile.index = gridY * area.MapSizeX + gridX;
            //GDI drawSelectionBox(gridX, gridY);
            selectionBoxLocation = new Point(gridX, gridY);
        }
        private void mapSizeChangeStuff()
        {
            lblMapSizeX.Text = area.MapSizeX.ToString();
            lblMapSizeY.Text = area.MapSizeY.ToString();
            //GDI resetPanelAndDeviceSize();
        }
        #endregion

        #region Event Handlers
        private void btnLoadMap_Click(object sender, EventArgs e)
        {
            
                #region old system
                if (mod.moduleName != "NewModule")
                {
                    openFileDialog1.InitialDirectory = prntForm._mainDirectory + "\\modules\\" + mod.moduleName + "\\graphics";
                }
                else
                {
                    openFileDialog1.InitialDirectory = prntForm._mainDirectory + "\\default\\NewModule";
                }
                openFileDialog1.FileName = String.Empty;
                openFileDialog1.Filter = "Map (*.jpg)|*.jpg";
                openFileDialog1.FilterIndex = 1;

                DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
                if (result == DialogResult.OK) // Test result.
                {
                    Bitmap testSize = new Bitmap(Path.GetFullPath(openFileDialog1.FileName));
                    if ((testSize.Width > 800) || (testSize.Height > 800))
                    {
                        MessageBox.Show("Map images must be less than 800x800 pixels");
                        return;
                    }
                    string filename = Path.GetFullPath(openFileDialog1.FileName);
                    //area.ImageFileName = Path.GetFileNameWithoutExtension(openFileDialog1.FileName);
                    //GDI gameMapBitmap = new Bitmap(filename);

                    //GDI if (gameMapBitmap == null)
                    //GDI {
                    //GDI     MessageBox.Show("returned a null bitmap");
                    //GDI }
                    //GDI refreshMap(true);
                }
                #endregion
            
        }
        private void btnRemoveMap_Click(object sender, EventArgs e)
        {
            //area.ImageFileName = "none";
            //GDI refreshMap(true);
        }
        private void numBGLocX_ValueChanged(object sender, EventArgs e)
        {
            //area.backgroundImageStartLocX = (int)numBGLocX.Value;
        }
        private void numBGLocY_ValueChanged(object sender, EventArgs e)
        {
            //area.backgroundImageStartLocY = (int)numBGLocY.Value;
        }
        private void WorldMapEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            //MessageBox.Show("closing editor and removing from openAreaList");
            //prntForm.openAreasList.Remove(area);
        }
        private void WorldMapEditor_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (useDirect2D)
            {
                DisposeAllD2D();
            }
            else
            {
                //device.Dispose();
                //surface.Dispose();
            }
            //gfxSelected.Dispose();
            //selectedBitmap.Dispose();
            //this.Close();
        }
        private void btnSelectedTerrain_Click(object sender, EventArgs e)
        {
            Button selectBtn = (Button)sender;
            currentTileFilename = selectBtn.Text;
            selectedBitmap = (Bitmap)selectBtn.BackgroundImage.Clone();
            panel1.BackgroundImage = selectedBitmap;
            rbtnPaintTile.Checked = true;
            prntForm.selectedLevelMapCreatureTag = "";
            prntForm.selectedLevelMapPropTag = "";
            prntForm.CreatureSelected = false;
            prntForm.PropSelected = false;
        }
        private void btnTileFilter_Click(object sender, EventArgs e)
        {
            Button selectBtn = (Button)sender;
            createTileImageButtons((string)selectBtn.Tag);
        }
        private void rbtnInfo_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtnInfo.Checked)
            {
                prntForm.logText("info on selecting map objects");
                prntForm.logText(Environment.NewLine);
                prntForm.selectedLevelMapCreatureTag = "";
                prntForm.selectedLevelMapPropTag = "";
                prntForm.CreatureSelected = false;
                prntForm.PropSelected = false;
                //refreshMap(true);
                //UpdatePB();
            }
        }
        private void rbtnPaintTile_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtnPaintTile.Checked)
            {
                prntForm.logText("painting tiles" + Environment.NewLine);
                prntForm.selectedLevelMapCreatureTag = "";
                prntForm.selectedLevelMapPropTag = "";
                prntForm.CreatureSelected = false;
                prntForm.PropSelected = false;
            }
        }
        private void rbtnWalkable_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtnWalkable.Checked)
            {
                prntForm.logText("editing walkmesh");
                prntForm.logText(Environment.NewLine);
                prntForm.selectedLevelMapCreatureTag = "";
                prntForm.selectedLevelMapPropTag = "";
                prntForm.selectedLevelMapTriggerTag = "";
                prntForm.CreatureSelected = false;
                prntForm.PropSelected = false;
                //refreshMap(true);
                //UpdatePB();
            }
        }
        private void rbtnLoS_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtnLoS.Checked)
            {
                prntForm.logText("editing line-of-sight mesh");
                prntForm.logText(Environment.NewLine);
                prntForm.selectedLevelMapCreatureTag = "";
                prntForm.selectedLevelMapPropTag = "";
                prntForm.selectedLevelMapTriggerTag = "";
                prntForm.CreatureSelected = false;
                prntForm.PropSelected = false;
                //refreshMap(true);
                //UpdatePB();
            }
        }
        private void btnRemoveSelectedObject_Click(object sender, EventArgs e)
        {
            int cnt = 0;
            foreach (Prop prp in area.Props)
            {
                if (prp.PropTag == lastSelectedObjectTag)
                {
                    // remove at index of matched tag
                    area.Props.RemoveAt(cnt);
                    return;
                }
                cnt++;
            }
            foreach (Trigger t in area.Triggers)
            {
                if (t.TriggerTag == lastSelectedObjectTag)
                {
                    // remove at index of matched tag
                    area.Triggers.Remove(t);
                    return;
                }
            }
        }
        private void rbtnPaintTrigger_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtnPaintTrigger.Checked)
            {
                //create a new trigger object
                Trigger newTrigger = new Trigger();
                //increment the tag to something unique
                newTrigger.TriggerTag = "newTrigger_" + prntForm.mod.nextIdNumber;
                prntForm.selectedLevelMapTriggerTag = newTrigger.TriggerTag;
                area.Triggers.Add(newTrigger);
                //set propertygrid to the new object
                prntForm.frmIceBlinkProperties.propertyGrid1.SelectedObject = newTrigger;
                prntForm.logText("painting a new trigger");
                prntForm.logText(Environment.NewLine);
                prntForm.selectedLevelMapCreatureTag = "";
                prntForm.selectedLevelMapPropTag = "";
                prntForm.CreatureSelected = false;
                prntForm.PropSelected = false;
            }
        }
        private void rbtnEditTrigger_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtnEditTrigger.Checked)
            {
                prntForm.logText("edit trigger: ");
                prntForm.logText(Environment.NewLine);
                prntForm.selectedLevelMapCreatureTag = "";
                prntForm.selectedLevelMapPropTag = "";
                prntForm.CreatureSelected = false;
                prntForm.PropSelected = false;
                prntForm.selectedLevelMapTriggerTag = lastSelectedObjectTag;
            }
        }
        private void chkGrid_CheckedChanged(object sender, EventArgs e)
        {
            //GDI refreshMap(true);
        }
        public void HandleContextMenuClick(object sender, EventArgs e)
        {
            //else, handler returns the selected tag
            ToolStripMenuItem menuItm = (ToolStripMenuItem)sender;
            foreach (Prop prp in area.Props)
            {
                if (prp.PropTag == menuItm.Text)
                {
                    // if so then give details about that icon (name, tag, etc.)
                    txtSelectedIconInfo.Text = "name: " + prp.PropName + Environment.NewLine + "tag: " + prp.PropTag;
                    lastSelectedObjectTag = prp.PropTag;
                    //prntForm.selectedLevelMapPropTag = prp.PropTag;
                    prp.PassInParentForm(prntForm);
                    prntForm.frmIceBlinkProperties.propertyGrid1.SelectedObject = prp;
                    return;
                }
            }
            foreach (Trigger t in area.Triggers)
            {
                if (t.TriggerTag == menuItm.Text)
                {
                    txtSelectedIconInfo.Text = "Trigger Tag: " + Environment.NewLine + t.TriggerTag;
                    lastSelectedObjectTag = t.TriggerTag;
                    prntForm.frmIceBlinkProperties.propertyGrid1.SelectedObject = t;
                    return;
                }
            }
        }
        private void rbtnZoom1x_CheckedChanged(object sender, EventArgs e)
        {
            sqr = 50;
            resetPanelAndDeviceSize();
            //GDI refreshMap(true);
        }
        private void rbtnZoom2x_CheckedChanged(object sender, EventArgs e)
        {
            sqr = 25;
            resetPanelAndDeviceSize();
            //GDI refreshMap(true);
        }
        private void rbtnZoom5x_CheckedChanged(object sender, EventArgs e)
        {
            sqr = 10;
            resetPanelAndDeviceSize();
            //GDI refreshMap(true);
        }
        private void btnPlusLeftX_Click(object sender, EventArgs e)
        {
            //y * area.MapSizeX + x
            int oldX = area.MapSizeX;
            for (int i = area.Layer1Filename.Count - oldX; i >= 0; i -= oldX)
            {
                //Tile newTile = new Tile();
                //area.Tiles.Insert(i, newTile);
                area.Layer1Filename.Insert(i, "t_f_grass");
                area.Layer1Rotate.Insert(i, 0);
                area.Layer1Mirror.Insert(i, 0);
                area.Layer2Filename.Insert(i, "t_a_blank");
                area.Layer2Rotate.Insert(i, 0);
                area.Layer2Mirror.Insert(i, 0);
                area.Layer3Filename.Insert(i, "t_a_blank");
                area.Layer3Rotate.Insert(i, 0);
                area.Layer3Mirror.Insert(i, 0);
                area.Walkable.Insert(i, 1);
                area.LoSBlocked.Insert(i, 0);
                area.Visible.Insert(i, 0);
            }
            foreach (Prop prpRef in area.Props)
            {
                prpRef.LocationX++;
            }
            foreach (Trigger t in area.Triggers)
            {
                foreach (Coordinate p in t.TriggerSquaresList)
                {
                    p.X++;
                }
            }
            area.MapSizeX++;
            mapSizeChangeStuff();
        }
        private void btnMinusLeftX_Click(object sender, EventArgs e)
        {
            //y * area.MapSizeX + x
            int oldX = area.MapSizeX;
            for (int i = area.Layer1Filename.Count - oldX; i >= 0; i -= oldX)
            {
                //area.Tiles.RemoveAt(i);
                area.Layer1Filename.RemoveAt(i);
                area.Layer1Rotate.RemoveAt(i);
                area.Layer1Mirror.RemoveAt(i);
                area.Layer2Filename.RemoveAt(i);
                area.Layer2Rotate.RemoveAt(i);
                area.Layer2Mirror.RemoveAt(i);
                area.Layer3Filename.RemoveAt(i);
                area.Layer3Rotate.RemoveAt(i);
                area.Layer3Mirror.RemoveAt(i);
                area.Walkable.RemoveAt(i);
                area.LoSBlocked.RemoveAt(i);
                area.Visible.RemoveAt(i);
            }
            foreach (Prop prpRef in area.Props)
            {
                prpRef.LocationX--;
            }
            foreach (Trigger t in area.Triggers)
            {
                foreach (Coordinate p in t.TriggerSquaresList)
                {
                    p.X--;
                }
            }
            area.MapSizeX--;
            mapSizeChangeStuff();
        }
        private void btnPlusRightX_Click(object sender, EventArgs e)
        {
            //y * area.MapSizeX + x
            int oldX = area.MapSizeX;
            for (int i = area.Layer1Filename.Count - 1; i >= 0; i -= oldX)
            {
                //Tile newTile = new Tile();
                //area.Tiles.Insert(i + 1, newTile);
                area.Layer1Filename.Insert(i+1, "t_f_grass");
                area.Layer1Rotate.Insert(i+1, 0);
                area.Layer1Mirror.Insert(i+1, 0);
                area.Layer2Filename.Insert(i+1, "t_a_blank");
                area.Layer2Rotate.Insert(i+1, 0);
                area.Layer2Mirror.Insert(i+1, 0);
                area.Layer3Filename.Insert(i + 1, "t_a_blank");
                area.Layer3Rotate.Insert(i + 1, 0);
                area.Layer3Mirror.Insert(i + 1, 0);
                area.Walkable.Insert(i+1, 1);
                area.LoSBlocked.Insert(i+1, 0);
                area.Visible.Insert(i+1, 0);
            }
            area.MapSizeX++;
            mapSizeChangeStuff();
        }
        private void btnMinusRightX_Click(object sender, EventArgs e)
        {
            //y * area.MapSizeX + x
            int oldX = area.MapSizeX;
            for (int i = area.Layer1Filename.Count - 1; i >= 0; i -= oldX)
            {
                //area.Tiles.RemoveAt(i);
                area.Layer1Filename.RemoveAt(i);
                area.Layer1Rotate.RemoveAt(i);
                area.Layer1Mirror.RemoveAt(i);
                area.Layer2Filename.RemoveAt(i);
                area.Layer2Rotate.RemoveAt(i);
                area.Layer2Mirror.RemoveAt(i);
                area.Layer3Filename.RemoveAt(i);
                area.Layer3Rotate.RemoveAt(i);
                area.Layer3Mirror.RemoveAt(i);
                area.Walkable.RemoveAt(i);
                area.LoSBlocked.RemoveAt(i);
                area.Visible.RemoveAt(i);
            }
            area.MapSizeX--;
            mapSizeChangeStuff();
        }
        private void btnPlusTopY_Click(object sender, EventArgs e)
        {
            //y * area.MapSizeX + x
            for (int i = 0; i < area.MapSizeX; i++)
            {
                //Tile newTile = new Tile();
                //area.Tiles.Insert(0, newTile);
                area.Layer1Filename.Insert(0, "t_f_grass");
                area.Layer1Rotate.Insert(0, 0);
                area.Layer1Mirror.Insert(0, 0);
                area.Layer2Filename.Insert(0, "t_a_blank");
                area.Layer2Rotate.Insert(0, 0);
                area.Layer2Mirror.Insert(0, 0);
                area.Layer3Filename.Insert(0, "t_a_blank");
                area.Layer3Rotate.Insert(0, 0);
                area.Layer3Mirror.Insert(0, 0);
                area.Walkable.Insert(0, 1);
                area.LoSBlocked.Insert(0, 0);
                area.Visible.Insert(0, 0);
            }
            foreach (Prop prpRef in area.Props)
            {
                prpRef.LocationY++;
            }
            foreach (Trigger t in area.Triggers)
            {
                foreach (Coordinate p in t.TriggerSquaresList)
                {
                    p.Y++;
                }
            }
            area.MapSizeY++;
            mapSizeChangeStuff();
        }
        private void btnMinusTopY_Click(object sender, EventArgs e)
        {
            //y * area.MapSizeX + x
            for (int i = 0; i < area.MapSizeX; i++)
            {
                //area.Tiles.RemoveAt(0);
                area.Layer1Filename.RemoveAt(0);
                area.Layer1Rotate.RemoveAt(0);
                area.Layer1Mirror.RemoveAt(0);
                area.Layer2Filename.RemoveAt(0);
                area.Layer2Rotate.RemoveAt(0);
                area.Layer2Mirror.RemoveAt(0);
                area.Layer3Filename.RemoveAt(0);
                area.Layer3Rotate.RemoveAt(0);
                area.Layer3Mirror.RemoveAt(0);
                area.Walkable.RemoveAt(0);
                area.LoSBlocked.RemoveAt(0);
                area.Visible.RemoveAt(0);
            }
            foreach (Prop prpRef in area.Props)
            {
                prpRef.LocationY--;
            }
            foreach (Trigger t in area.Triggers)
            {
                foreach (Coordinate p in t.TriggerSquaresList)
                {
                    p.Y--;
                }
            }
            area.MapSizeY--;
            mapSizeChangeStuff();
        }
        private void btnPlusBottumY_Click(object sender, EventArgs e)
        {
            //y * area.MapSizeX + x
            for (int i = 0; i < area.MapSizeX; i++)
            {
                //Tile newTile = new Tile();
                //area.Tiles.Add(newTile);
                area.Layer1Filename.Add("t_f_grass");
                area.Layer1Rotate.Add(0);
                area.Layer1Mirror.Add(0);
                area.Layer2Filename.Add("t_a_blank");
                area.Layer2Rotate.Add(0);
                area.Layer2Mirror.Add(0);
                area.Layer3Filename.Add("t_a_blank");
                area.Layer3Rotate.Add(0);
                area.Layer3Mirror.Add(0);
                area.Walkable.Add(1);
                area.LoSBlocked.Add(0);
                area.Visible.Add(0);
            }
            area.MapSizeY++;
            mapSizeChangeStuff();
        }
        private void btnMinusBottumY_Click(object sender, EventArgs e)
        {
            //y * area.MapSizeX + x
            for (int i = 0; i < area.MapSizeX; i++)
            {
                //area.Tiles.RemoveAt(area.Tiles.Count - 1);
                int total = area.Visible.Count;
                area.Layer1Filename.RemoveAt(total - 1);
                area.Layer1Rotate.RemoveAt(total - 1);
                area.Layer1Mirror.RemoveAt(total - 1);
                area.Layer2Filename.RemoveAt(total - 1);
                area.Layer2Rotate.RemoveAt(total - 1);
                area.Layer2Mirror.RemoveAt(total - 1);
                area.Layer3Filename.RemoveAt(total - 1);
                area.Layer3Rotate.RemoveAt(total - 1);
                area.Layer3Mirror.RemoveAt(total - 1);
                area.Walkable.RemoveAt(total - 1);
                area.LoSBlocked.RemoveAt(total - 1);
                area.Visible.RemoveAt(total - 1);
            }
            area.MapSizeY--;
            mapSizeChangeStuff();
        }
        private void btnProperties_Click(object sender, EventArgs e)
        {
            prntForm.frmIceBlinkProperties.propertyGrid1.SelectedObject = area;
        }
        private void panelView_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {            
            if (e.KeyCode == Keys.Escape)
            {
                // exit by right click or ESC
                prntForm.logText("pressed escape");
                prntForm.logText(Environment.NewLine);
                //prntForm.selectedEncounterCreatureTag = "";
                prntForm.selectedLevelMapCreatureTag = "";
                prntForm.selectedLevelMapPropTag = "";
                prntForm.CreatureSelected = false;
                prntForm.PropSelected = false;
                //GDI refreshMap(true);
                //GDI UpdatePB();
                rbtnInfo.Checked = true;
                resetTileToBePlacedSettings();
            }
            else if (e.KeyCode == Keys.R)
            {
                if (rbtnPaintTile.Checked)
                {
                    if (Control.ModifierKeys == Keys.Shift)
                    {
                        tileToBePlaced.angle -= 1;
                    }
                    else
                    {
                        tileToBePlaced.angle -= 90;
                    }
                    if (tileToBePlaced.angle > 360)
                    {
                        tileToBePlaced.angle -= 360;
                    }
                    if (tileToBePlaced.angle < 0)
                    {
                        tileToBePlaced.angle += 360;
                    }
                }
            }
            else if (e.KeyCode == Keys.M)
            {
                if (rbtnPaintTile.Checked)
                {
                    if (tileToBePlaced.mirror == 0)
                    {
                        tileToBePlaced.mirror = 1;
                    }
                    else
                    {
                        tileToBePlaced.mirror = 0;
                    }
                }
            }
            else if (e.KeyCode == Keys.Delete)
            {
                int cnt = 0;
                foreach (Prop prp in area.Props)
                {
                    if (prp.PropTag == lastSelectedObjectTag)
                    {
                        // remove at index of matched tag
                        area.Props.RemoveAt(cnt);
                        return;
                    }
                    cnt++;
                }
                foreach (Trigger t in area.Triggers)
                {
                    if (t.TriggerTag == lastSelectedObjectTag)
                    {
                        // remove at index of matched tag
                        area.Triggers.Remove(t);
                        return;
                    }
                }
            }
            panelView.Focus();
        }
        private void btnMovePropUp_Click(object sender, EventArgs e)
        {
            foreach (Prop prp in area.Props)
            {
                if (prp.PropTag == lastSelectedObjectTag)
                {
                    if (prp.LocationY > 0)
                    {
                        prp.LocationY--;
                    }
                }
            }
        }
        private void btnMovePropDown_Click(object sender, EventArgs e)
        {
            foreach (Prop prp in area.Props)
            {
                if (prp.PropTag == lastSelectedObjectTag)
                {
                    if (prp.LocationY < area.MapSizeY - 1)
                    {
                        prp.LocationY++;
                    }
                }
            }
        }
        private void btnMovePropRight_Click(object sender, EventArgs e)
        {
            foreach (Prop prp in area.Props)
            {
                if (prp.PropTag == lastSelectedObjectTag)
                {
                    if (prp.LocationX < area.MapSizeX - 1)
                    {
                        prp.LocationX++;
                    }
                }
            }
        }
        private void btnMovePropLeft_Click(object sender, EventArgs e)
        {
            foreach (Prop prp in area.Props)
            {
                if (prp.PropTag == lastSelectedObjectTag)
                {
                    if (prp.LocationX > 0)
                    {
                        prp.LocationX--;
                    }
                }
            }
        }
        #endregion

        private void timerRenderLoop_Tick(object sender, EventArgs e)
        {
            Render();
        }
    }
}
