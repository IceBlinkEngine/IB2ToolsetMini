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
using SharpDX.DirectWrite;
using System.Runtime.InteropServices;

namespace IB2ToolsetMini
{
    public partial class EncounterEditor : DockContent
    {
        public ParentForm prntForm;
        public Module mod;

        private Bitmap selectedBitmap;
        public string selectedBitmapFilename = "";
        private int sqr = 50;
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
        public bool PcSelected = false;
        public bool CrtSelected = false;
        public Point currentSquareClicked = new Point(0, 0);
        public Point lastSquareClicked = new Point(0, 0);
        public selectionStruct selectedTile;
        public Point lastSelectedCreaturePropIcon;
        public string lastSelectedObjectTag;
        public string lastSelectedObjectResRef;
        public Creature le_selectedCreature = new Creature();
        public Prop le_selectedProp = new Prop();
        public Encounter thisEnc = new Encounter();

        #region Direct2D Stuff
        public bool useDirect2D = true;
        public SharpDX.Direct2D1.Factory Factory2D { get; private set; }
        public SharpDX.DirectWrite.Factory FactoryDWrite { get; private set; }
        public WindowRenderTarget RenderTarget2D { get; private set; }
        public SolidColorBrush SceneColorBrush { get; private set; }
        public Dictionary<string, SharpDX.Direct2D1.Bitmap> commonBitmapList = new Dictionary<string, SharpDX.Direct2D1.Bitmap>();
        public Point selectionBoxLocation = new Point(-1, -1);
        public TextFormat textFormat;
        public TextLayout textLayout;
        #endregion

        public EncounterEditor(Module m, ParentForm p)
        {
            InitializeComponent();
            mod = m;
            prntForm = p;
            thisEnc = prntForm.mod.moduleEncountersList[prntForm._selectedLbxEncounterIndex];
            createTileImageButtons("t_");            
            refreshCmbItems();
            refreshLbxItems();
            refreshGoldDrop();
        }        
        private void EncounterEditor_Load(object sender, EventArgs e)
        {
            radioButton1.Checked = true;
            checkBox1.Checked = true;
            checkBox2.Checked = true;
            checkBox3.Checked = true;
            
            rbtnInfo.Checked = true;
            //rbtnZoom1x.Checked = true;
            refreshLeftPanelInfo();
            InitDirect2DAndDirectWrite();
            loadAllModuleImageData();
        }
        public void resetTileToBePlacedSettings()
        {
            tileToBePlaced.angle = 0;
            tileToBePlaced.mirror = 0;
        }

        #region Encounter Stuff
        public void refreshCmbItems()
        {
            cmbItems.BeginUpdate();
            cmbItems.DataSource = null;
            cmbItems.DataSource = prntForm.allItemsList;
            cmbItems.DisplayMember = "name";
            cmbItems.EndUpdate();
        }
        private void refreshLbxItems()
        {
            lbxItems.BeginUpdate();
            lbxItems.DataSource = null;
            lbxItems.DataSource = thisEnc.encounterInventoryRefsList;
            lbxItems.DisplayMember = "name";
            lbxItems.EndUpdate();
        }
        public void refreshGoldDrop()
        {
            numGold.Value = (int)thisEnc.goldDrop;
        }
        private void numGold_ValueChanged(object sender, EventArgs e)
        {

        }        
        public void saveEncountersFile()
        {
            try
            {
                string json = JsonConvert.SerializeObject(prntForm.mod.moduleEncountersList, Formatting.Indented);
                using (StreamWriter sw = new StreamWriter(prntForm._mainDirectory + "\\modules\\" + prntForm.mod.moduleName + "\\data\\encounters.json"))
                {
                    sw.Write(json.ToString());
                }
                MessageBox.Show("Saved File");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to Save File: " + ex.ToString());
            }
        }
        public List<Encounter> loadEncountersFile()
        {
            List<Encounter> toReturn = null;

            // deserialize JSON directly from a file
            using (StreamReader file = File.OpenText(prntForm._mainDirectory + "\\data\\encounters.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                toReturn = (List<Encounter>)serializer.Deserialize(file, typeof(List<Encounter>));
            }
            return toReturn;
        }
        public List<Item> loadItemsFile()
        {
            List<Item> toReturn = null;

            // deserialize JSON directly from a file
            using (StreamReader file = File.OpenText(prntForm._mainDirectory + "\\data\\items.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                toReturn = (List<Item>)serializer.Deserialize(file, typeof(List<Item>));
            }
            return toReturn;
        }
        #endregion

        private void resetPanelAndDeviceSize()
        {
            //GDI panelView.Width = thisEnc.MapSizeX * sqr;
            //GDI panelView.Height = thisEnc.MapSizeY * sqr;
            //GDI surface = new Bitmap(thisEnc.MapSizeX * sqr, thisEnc.MapSizeY * sqr);
            //GDI device = Graphics.FromImage(surface);
        }
        public void loadAllModuleImageData()
        {
            foreach (ImageData imd in mod.moduleImageDataList)
            {
                commonBitmapList.Add(imd.name, ConvertGDIBitmapToD2D(prntForm.bsc.ConvertImageDataToBitmap(imd)));
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
                    if (!Path.GetFileName(f).StartsWith(filter)) { continue; }
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
            /*//GDI this.Cursor = Cursors.WaitCursor;
            if (thisEnc == null) { return; }
            if (refreshAll)
            {
                device.Clear(Color.Gainsboro);
            }
            try
            {
                //draw map from single image
                
                if ((thisEnc.UseMapImage) && (thisEnc.MapImage != "none"))
                {
                    #region Draw Single Image Map
                    if (mapBitmap == null)
                    {                        
                        if (File.Exists(prntForm._mainDirectory + "\\modules\\" + prntForm.mod.moduleName + "\\graphics\\" + thisEnc.MapImage + ".jpg"))
                        {
                            mapBitmap = new Bitmap(prntForm._mainDirectory + "\\modules\\" + prntForm.mod.moduleName + "\\graphics\\" + thisEnc.MapImage + ".jpg");
                        }
                        else if (File.Exists(prntForm._mainDirectory + "\\modules\\" + prntForm.mod.moduleName + "\\graphics\\" + thisEnc.MapImage))
                        {
                            mapBitmap = new Bitmap(prntForm._mainDirectory + "\\modules\\" + prntForm.mod.moduleName + "\\graphics\\" + thisEnc.MapImage);
                        }
                        else if (File.Exists(prntForm._mainDirectory + "\\modules\\" + prntForm.mod.moduleName + "\\graphics\\" + thisEnc.MapImage + ".png"))
                        {
                            mapBitmap = new Bitmap(prntForm._mainDirectory + "\\modules\\" + prntForm.mod.moduleName + "\\graphics\\" + thisEnc.MapImage + ".png");
                        }
                        else
                        {
                            MessageBox.Show("Couldn't find map image " + thisEnc.MapImage + " in graphics folder, setting UseMapImage to false.");
                            thisEnc.UseMapImage = false;
                            return;
                        }
                    }
                    Rectangle src = new Rectangle(0, 0, mapBitmap.Width, mapBitmap.Height);
                    Rectangle dst = new Rectangle(0, 0, thisEnc.MapSizeX * sqr, thisEnc.MapSizeY * sqr);
                    device.DrawImage(mapBitmap, dst, src, GraphicsUnit.Pixel);
                    //Draw Grid stuff
                    for (int x = 0; x < thisEnc.MapSizeY; x++)
                    {
                        for (int y = 0; y < thisEnc.MapSizeX; y++)
                        {
                            TileEnc tile = thisEnc.encounterTiles[y * thisEnc.MapSizeX + x];
                            Rectangle target = new Rectangle(x * sqr, y * sqr, sqr, sqr);
                            //draw square walkmesh and LoS stuff
                            src = new Rectangle(0, 0, g_walkPass.Width, g_walkPass.Height);
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
                        }
                    }
                    #endregion
                }
                else //draw tile map instead
                {
                    #region Draw Layer1
                    if (checkBox1.Checked)
                    {
                        for (int y = 0; y < thisEnc.MapSizeY; y++)
                        {
                            for (int x = 0; x < thisEnc.MapSizeX; x++)
                            {
                                if ((refreshAll) || (currentSquareClicked == new Point(x, y)) || (lastSquareClicked == new Point(x, y)))
                                {
                                    TileEnc tile = thisEnc.encounterTiles[y * thisEnc.MapSizeX + x];
                                    Bitmap lyr1 = null;
                                    if (getTileByName(tile.Layer1Filename) != null)
                                    {
                                        lyr1 = getTileByName(tile.Layer1Filename).bitmap;
                                    }
                                    Rectangle target = new Rectangle(x * sqr, y * sqr, sqr, sqr);
                                    if (lyr1 != null)
                                    {
                                        float scalerX = lyr1.Width / 100;
                                        float scalerY = lyr1.Height / 100;
                                        Rectangle src1 = new Rectangle(0, 0, lyr1.Width, lyr1.Height);
                                        Rectangle dst = new Rectangle(x * sqr, y * sqr, (int)(sqr * scalerX), (int)(sqr * scalerY));
                                        device.DrawImage(lyr1, dst, src1, GraphicsUnit.Pixel);
                                    }
                                }
                            }
                        }
                    }
                    #endregion
                    #region Draw Layer2
                    if (checkBox2.Checked)
                    {
                        for (int y = 0; y < thisEnc.MapSizeY; y++)
                        {
                            for (int x = 0; x < thisEnc.MapSizeX; x++)
                            {
                                if ((refreshAll) || (currentSquareClicked == new Point(x, y)) || (lastSquareClicked == new Point(x, y)))
                                {
                                    TileEnc tile = thisEnc.encounterTiles[y * thisEnc.MapSizeX + x];
                                    Bitmap lyr1 = null;
                                    if (getTileByName(tile.Layer2Filename) != null)
                                    {
                                        lyr1 = getTileByName(tile.Layer2Filename).bitmap;
                                    }
                                    Rectangle target = new Rectangle(x * sqr, y * sqr, sqr, sqr);
                                    if (lyr1 != null)
                                    {
                                        float scalerX = lyr1.Width / 100;
                                        float scalerY = lyr1.Height / 100;
                                        Rectangle src1 = new Rectangle(0, 0, lyr1.Width, lyr1.Height);
                                        Rectangle dst = new Rectangle(x * sqr, y * sqr, (int)(sqr * scalerX), (int)(sqr * scalerY));
                                        device.DrawImage(lyr1, dst, src1, GraphicsUnit.Pixel);
                                    }
                                }
                            }
                        }
                    }
                    #endregion
                    #region Draw Layer3
                    if (checkBox3.Checked)
                    {
                        for (int y = 0; y < thisEnc.MapSizeY; y++)
                        {
                            for (int x = 0; x < thisEnc.MapSizeX; x++)
                            {
                                if ((refreshAll) || (currentSquareClicked == new Point(x, y)) || (lastSquareClicked == new Point(x, y)))
                                {
                                    TileEnc tile = thisEnc.encounterTiles[y * thisEnc.MapSizeX + x];
                                    Bitmap lyr1 = null;
                                    if (getTileByName(tile.Layer3Filename) != null)
                                    {
                                        lyr1 = getTileByName(tile.Layer3Filename).bitmap;
                                    }
                                    Rectangle target = new Rectangle(x * sqr, y * sqr, sqr, sqr);
                                    if (lyr1 != null)
                                    {
                                        float scalerX = lyr1.Width / 100;
                                        float scalerY = lyr1.Height / 100;
                                        Rectangle src1 = new Rectangle(0, 0, lyr1.Width, lyr1.Height);
                                        Rectangle dst = new Rectangle(x * sqr, y * sqr, (int)(sqr * scalerX), (int)(sqr * scalerY));
                                        device.DrawImage(lyr1, dst, src1, GraphicsUnit.Pixel);
                                    }
                                }
                            }
                        }
                    }
                    #endregion
                    #region Draw Grid
                    //draw tile map
                    if (chkGrid.Checked) //if show grid is turned on, draw grid squares
                    {
                        for (int y = 0; y < thisEnc.MapSizeY; y++)
                        {
                            for (int x = 0; x < thisEnc.MapSizeX; x++)
                            {
                                if ((refreshAll) || (currentSquareClicked == new Point(x, y)) || (lastSquareClicked == new Point(x, y)))
                                {
                                    TileEnc tile = thisEnc.encounterTiles[y * thisEnc.MapSizeX + x];
                                    Rectangle target = new Rectangle(x * sqr, y * sqr, sqr, sqr);
                                    //draw square walkmesh and LoS stuff
                                    Rectangle src = new Rectangle(0, 0, g_walkPass.Width, g_walkPass.Height);

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
                            }
                        }
                    }
                    #endregion
                }

                //draw creatures
                foreach (CreatureRefs crtRef in thisEnc.encounterCreatureRefsList)
                {
                    int cspx = crtRef.creatureStartLocationX * sqr;
                    int cspy = crtRef.creatureStartLocationY * sqr;
                    Creature crt = prntForm.getCreatureByResRef(crtRef.creatureResRef);
                    Rectangle src = new Rectangle(0, 0, crt.creatureIconBitmap.Width, crt.creatureIconBitmap.Width);
                    Rectangle dst = new Rectangle(cspx, cspy, sqr, sqr);
                    if (crt.creatureIconBitmap.Width > 100)
                    {                    
                        dst = new Rectangle(cspx, cspy, sqr * 2, sqr * 2);
                    }
                    device.DrawImage(crt.creatureIconBitmap, dst, src, GraphicsUnit.Pixel);
                    if (sqr == 50) { drawFont = new Font("Arial", 6); }
                    else if (sqr == 25) { drawFont = new Font("Arial", 3); }
                    else if (sqr == 10) { drawFont = new Font("Arial", 2); }
                    device.DrawString(crtRef.creatureTag, drawFont, drawBrush, new Point(cspx, cspy + (sqr/2)));
                }
                //draw PCs
                int cnt = 0;
                foreach (Coordinate PCpoint in thisEnc.encounterPcStartLocations)
                {
                    int cspx = PCpoint.X * sqr;
                    int cspy = PCpoint.Y * sqr;
                    if (sqr == 50) { drawFontNum = new Font("Arial", 24); }
                    else if (sqr == 25) { drawFontNum = new Font("Arial", 12); }
                    else if (sqr == 10) { drawFontNum = new Font("Arial", 5); }
                    device.DrawString((cnt + 1).ToString(), drawFontNum, drawBrush, new Point(cspx, cspy + 5));
                    cnt++;
                }

                            /*TODOint cnt = 0;
                            foreach (Prop prpRef in thisEnc.Props)
                            {
                                int cspx = prpRef.LocationX;
                                int cspy = prpRef.LocationY;
                                if ((refreshAll) || (currentSquareClicked == new Point(cspx, cspy)) || (lastSquareClicked == new Point(cspx, cspy)))
                                {                        
                                    spritePropDraw(cspx, cspy, cnt);
                                }
                                cnt++;
                            }*/
            /*TODOforeach (Trigger t in thisEnc.Triggers)
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
            }*/
            /*//GDI UpdatePB();
            }
            catch (Exception ex)
            {
                MessageBox.Show("failed on refresh map: " + ex.ToString());                
            }*/
        }
        private void spritePropDraw(int cspx, int cspy, int spriteListIndex)
        {
            //source image
            //GDI Bitmap prpBitmap = propBitmapList[spriteListIndex];
            //Rectangle source = new Rectangle(0, 0, le_selectedProp.propBitmap.Width, le_selectedProp.propBitmap.Height);
            //GDI Rectangle source = new Rectangle(0, 0, prpBitmap.Width, prpBitmap.Height);
            //target location
            //GDI Rectangle target = new Rectangle(cspx * sqr, cspy * sqr, sqr, sqr);
            //draw sprite
            //GDI device.DrawImage((Image)prpBitmap, target, source, GraphicsUnit.Pixel);
        }
        public void drawSelectionBox(int gridx, int gridy)
        {
            //draw selection box around tile
            //GDI int dx = gridx * sqr;
            //GDI int dy = gridy * sqr;
            //GDI Pen pen = new Pen(Color.DarkMagenta, 2);
            //GDI Rectangle rect = new Rectangle(dx + 1, dy + 1, sqr - 2, sqr - 2);
            //GDI device.DrawRectangle(pen, rect);

            //save changes
            //GDI UpdatePB();
        }
        public void UpdatePB()
        {
            //GDI this.Cursor = Cursors.Default;
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
                UpdatePB();*/
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
            }
        }
        private void panelView_MouseDown(object sender, MouseEventArgs e)
        {
            /*
            int col = e.X / sqr;
            int row = e.Y / sqr;

            if (tileSelected)
            {
                //prntForm.encountersList[_selectedLbxEncounterIndex].encounterMapLayout[row, col] = currentTile;
                if (radioButton1.Checked)
                {
                    prntForm.encountersList[_selectedLbxEncounterIndex].encounterTiles[row * 7 + col].Layer1Filename = currentTileFilename;
                }
                else if (radioButton2.Checked)
                {
                    prntForm.encountersList[_selectedLbxEncounterIndex].encounterTiles[row * 7 + col].Layer2Filename = currentTileFilename;
                }
            }
            else if (rbtnWalkable.Checked)
            {
                if (prntForm.encountersList[_selectedLbxEncounterIndex].encounterTiles[row * 7 + col].Walkable == true)
                {
                    prntForm.encountersList[_selectedLbxEncounterIndex].encounterTiles[row * 7 + col].Walkable = false;
                }
                else
                {
                    prntForm.encountersList[_selectedLbxEncounterIndex].encounterTiles[row * 7 + col].Walkable = true;
                }
                refreshMap();
            }
            else if (rbtnLoS.Checked)
            {
                if (prntForm.encountersList[_selectedLbxEncounterIndex].encounterTiles[row * 7 + col].LoSBlocked == true)
                {
                    prntForm.encountersList[_selectedLbxEncounterIndex].encounterTiles[row * 7 + col].LoSBlocked = false;
                }
                else
                {
                    prntForm.encountersList[_selectedLbxEncounterIndex].encounterTiles[row * 7 + col].LoSBlocked = true;
                }
                refreshMap();
            }
            refreshMap();  
            */
        }
        private void panelView_MouseLeave(object sender, EventArgs e)
        {
            /*//GDI try
            {
                //refreshMap();
                //UpdatePB();
            }
            catch (Exception ex)
            {
                MessageBox.Show("failed on mouse leave map: " + ex.ToString());
            }*/
        }
        private void panelView_MouseEnter(object sender, EventArgs e)
        {
            panelView.Select();
            try
            {
                if (prntForm.selectedLevelMapCreatureTag != "")
                {
                    prntForm.CreatureSelected = true;
                    tileSelected = false;
                    PcSelected = false;
                    CrtSelected = true;
                }
                if (prntForm.selectedLevelMapPropTag != "")
                {
                    prntForm.PropSelected = true;
                    tileSelected = false;
                    PcSelected = false;
                    CrtSelected = false;
                }
                if (prntForm.CreatureSelected)
                {
                    string selectedCrt = prntForm.selectedEncounterCreatureTag;
                    le_selectedCreature = prntForm.getCreatureByTag(selectedCrt);
                    if (le_selectedCreature != null)
                    {
                        selectedBitmap = le_selectedCreature.creatureIconBitmap;
                        //selectedBitmapSize = le_selectedCreature.Size;
                    }
                }
                else if (prntForm.PropSelected)
                {
                    string selectedProp = prntForm.selectedLevelMapPropTag;
                    le_selectedProp = prntForm.getPropByTag(selectedProp);
                    if (le_selectedProp != null)
                    {
                        selectedBitmap = le_selectedProp.propBitmap;
                        //selectedBitmapSize = le_selectedProp.propBitmap.Width / (tileSize * 2);
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
            if (gridX > thisEnc.MapSizeX - 1) { return false; }
            if (gridY > thisEnc.MapSizeY - 1) { return false; }
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
                    #region PC Selected
                    if (PcSelected)
                    {
                        if (thisEnc.encounterPcStartLocations.Count < 6)
                        {
                            thisEnc.encounterPcStartLocations.Add(new Coordinate(gridX, gridY));
                        }
                        //GDI refreshMap(true);
                    }
                    #endregion
                    #region Creature Selected
                    else if (prntForm.CreatureSelected)
                    {
                        CreatureRefs crtRef = new CreatureRefs();
                        string _nodeTag = prntForm.frmBlueprints.tvCreatures.SelectedNode.Name;
                        crtRef.creatureResRef = prntForm.allCreaturesList[prntForm.frmBlueprints.GetCreatureIndex(_nodeTag)].cr_resref;
                        crtRef.creatureTag = prntForm.allCreaturesList[prntForm.frmBlueprints.GetCreatureIndex(_nodeTag)].cr_tag + "_" + prntForm.mod.nextIdNumber;
                        crtRef.creatureStartLocationX = gridX;
                        crtRef.creatureStartLocationY = gridY;
                        thisEnc.encounterCreatureRefsList.Add(crtRef);
                        //GDI refreshMap(true);
                    }
                    #endregion
                    #region Tile Selected
                    else if (rbtnPaintTile.Checked)
                    {
                        //gridX = e.X / sqr;
                        //gridY = e.Y / sqr;
                        selectedTile.index = gridY * thisEnc.MapSizeX + gridX;
                        prntForm.logText("gridx = " + gridX.ToString() + "gridy = " + gridY.ToString());
                        prntForm.logText(Environment.NewLine);
                        if (radioButton1.Checked)
                        {
                            thisEnc.Layer1Filename[selectedTile.index] = currentTileFilename;
                            thisEnc.Layer1Rotate[selectedTile.index] = tileToBePlaced.angle;
                            thisEnc.Layer1Mirror[selectedTile.index] = tileToBePlaced.mirror;
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
                                        thisEnc.Layer1Filename[y * thisEnc.MapSizeX + x] = currentTileFilename;
                                        thisEnc.Layer1Rotate[y * thisEnc.MapSizeX + x] = tileToBePlaced.angle;
                                        thisEnc.Layer1Mirror[y * thisEnc.MapSizeX + x] = tileToBePlaced.mirror;
                                        currentSquareClicked = new Point(x, y);
                                        //GDI refreshMap(false);
                                    }
                                }
                            }
                        }
                        else if (radioButton2.Checked)
                        {
                            thisEnc.Layer2Filename[selectedTile.index] = currentTileFilename;
                            thisEnc.Layer2Rotate[selectedTile.index] = tileToBePlaced.angle;
                            thisEnc.Layer2Mirror[selectedTile.index] = tileToBePlaced.mirror;
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
                                        thisEnc.Layer2Filename[y * thisEnc.MapSizeX + x] = currentTileFilename;
                                        thisEnc.Layer2Rotate[y * thisEnc.MapSizeX + x] = tileToBePlaced.angle;
                                        thisEnc.Layer2Mirror[y * thisEnc.MapSizeX + x] = tileToBePlaced.mirror;
                                        currentSquareClicked = new Point(x, y);
                                        //GDI refreshMap(false);
                                    }
                                }
                            }
                        }
                        else if (radioButton3.Checked)
                        {
                            thisEnc.Layer3Filename[selectedTile.index] = currentTileFilename;
                            thisEnc.Layer3Rotate[selectedTile.index] = tileToBePlaced.angle;
                            thisEnc.Layer3Mirror[selectedTile.index] = tileToBePlaced.mirror;
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
                                        thisEnc.Layer3Filename[y * thisEnc.MapSizeX + x] = currentTileFilename;
                                        thisEnc.Layer3Rotate[y * thisEnc.MapSizeX + x] = tileToBePlaced.angle;
                                        thisEnc.Layer3Mirror[y * thisEnc.MapSizeX + x] = tileToBePlaced.mirror;
                                        currentSquareClicked = new Point(x, y);
                                        //GDI refreshMap(false);
                                    }
                                }
                            }
                        }
                        //GDI refreshMap(false);
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
                        thisEnc.propsList.Add(newProp);                        
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
                            Trigger newTrigger = thisEnc.getTriggerByTag(selectedTrigger);
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
                                Trigger newTrigger = thisEnc.getTriggerByTag(selectedTrigger);
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
                        selectedTile.index = gridY * thisEnc.MapSizeX + gridX;
                        prntForm.logText("gridx = " + gridX.ToString() + "gridy = " + gridY.ToString());
                        prntForm.logText(Environment.NewLine);
                        thisEnc.Walkable[selectedTile.index] = 0;
                        //GDI refreshMap(false);
                    }
                    #endregion
                    #region LoS mesh Toggle Selected (Make LoS Blocked)
                    else if (rbtnLoS.Checked)
                    {
                        selectedTile.index = gridY * thisEnc.MapSizeX + gridX;
                        prntForm.logText("gridx = " + gridX.ToString() + "gridy = " + gridY.ToString());
                        prntForm.logText(Environment.NewLine);
                        thisEnc.LoSBlocked[selectedTile.index] = 1;
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

                        foreach (CreatureRefs crt in thisEnc.encounterCreatureRefsList)
                        {
                            if ((crt.creatureStartLocationX == newPoint.X) && (crt.creatureStartLocationY == newPoint.Y))
                            {
                                Creature c = prntForm.getCreatureByResRef(crt.creatureResRef);
                                // if so then give details about that icon (name, tag, etc.)
                                txtSelectedIconInfo.Text = "name: " + c.cr_name + Environment.NewLine
                                                            + "tag: " + crt.creatureTag + Environment.NewLine
                                                            + "resref: " + crt.creatureResRef;
                                //lastSelectedObjectResRef = crt.CreatureResRef;
                                lastSelectedObjectTag = crt.creatureTag;
                                prntForm.frmIceBlinkProperties.propertyGrid1.SelectedObject = crt;
                            }
                        }
                        foreach (Prop prp in thisEnc.propsList)
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
                        foreach (Trigger t in thisEnc.Triggers)
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
                        selectedTile.index = gridY * thisEnc.MapSizeX + gridX;
                        prntForm.logText("gridx = " + gridX.ToString() + "gridy = " + gridY.ToString());
                        prntForm.logText(Environment.NewLine);
                        thisEnc.Walkable[selectedTile.index] = 1;
                        //GDI refreshMap(false);
                    }
                    #endregion
                    #region LoS mesh Toggle Selected (Make LoS Visible)
                    else if (rbtnLoS.Checked)
                    {
                        selectedTile.index = gridY * thisEnc.MapSizeX + gridX;
                        prntForm.logText("gridx = " + gridX.ToString() + "gridy = " + gridY.ToString());
                        prntForm.logText(Environment.NewLine);
                        thisEnc.LoSBlocked[selectedTile.index] = 0;
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
            for (int x = 0; x < thisEnc.MapSizeX; x++)
            {
                for (int y = 0; y < thisEnc.MapSizeY; y++)
                {
                    selectedTile.index = y * thisEnc.MapSizeX + x;
                    if (radioButton1.Checked)
                    {
                        thisEnc.Layer1Filename[selectedTile.index] = currentTileFilename;
                    }
                    else if (radioButton2.Checked)
                    {
                        thisEnc.Layer2Filename[selectedTile.index] = currentTileFilename;
                    }
                    else if (radioButton3.Checked)
                    {
                        thisEnc.Layer3Filename[selectedTile.index] = currentTileFilename;
                    }
                }
            }
            refreshMap(true);
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
        public void CleanUpDrawTextResources()
        {
            if (textFormat != null)
            {
                textFormat.Dispose();
                textFormat = null;
            }
            if (textLayout != null)
            {
                textLayout.Dispose();
                textLayout = null;
            }
        }
        public void DrawText(string text, float xLoc, float yLoc, float scaler)
        {
            DrawText(text, xLoc, yLoc, scaler, SharpDX.Color.White);
        }
        public void DrawText(string text, float x, float y, float scaler, SharpDX.Color fontColor)
        {
            CleanUpDrawTextResources();
            using (SolidColorBrush scb = new SolidColorBrush(RenderTarget2D, fontColor))
            {
                textFormat = new TextFormat(FactoryDWrite, "Arial", scaler);
                //textFormat = new TextFormat(factoryDWrite, thisFont.FontFamily.Name, fw, fs, FontStretch.Normal, thisFont.Height) { TextAlignment = TextAlignment.Leading, ParagraphAlignment = ParagraphAlignment.Near };
                //textFormat = new TextFormat(factoryDWrite, FontFamilyName, CurrentFontCollection, fw, fs, FontStretch.Normal, scaler) { TextAlignment = TextAlignment.Leading, ParagraphAlignment = ParagraphAlignment.Near };
                textLayout = new TextLayout(FactoryDWrite, text, textFormat, this.Width, this.Height);
                RenderTarget2D.DrawTextLayout(new SharpDX.Vector2(x, y), textLayout, scb, DrawTextOptions.None);
            }
        }
        public void DrawD2DBitmap(SharpDX.Direct2D1.Bitmap bitmap, SharpDX.RectangleF source, SharpDX.RectangleF target, int angleInDegrees, int mirror)
        {
            int mir = 1;
            if (mirror == 1) { mir = -1; }
            //convert degrees to radians
            float angle = (float)(Math.PI * 2 * (float)angleInDegrees / (float)360);
            SharpDX.Vector2 center = new SharpDX.Vector2(target.Left + (target.Width / 2), target.Top + (target.Height / 2));
            RenderTarget2D.Transform = SharpDX.Matrix.Transformation2D(center, 0, new SharpDX.Vector2(mir, 1.0f), center, angle, new SharpDX.Vector2(0, 0));
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
            foreach (SharpDX.Direct2D1.Bitmap bm in commonBitmapList.Values)
            {
                if (bm != null)
                {
                    bm.Dispose();
                }
            }
            commonBitmapList.Clear();
            //DisposeOfBitmap(ref mapBitmapD2D);
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
            if (thisEnc == null) { return; }            
            #region Draw Layer 1
            if (checkBox1.Checked)
            {
                for (int y = 0; y < thisEnc.MapSizeY; y++)
                {
                    for (int x = 0; x < thisEnc.MapSizeX; x++)
                    {
                        string tile = thisEnc.Layer1Filename[y * thisEnc.MapSizeX + x];
                        if (!tile.Equals("t_a_blank"))
                        {
                            float scalerX = GetFromBitmapList(tile).PixelSize.Width / prntForm.tileSizeInPixels;
                            if (scalerX == 0) { scalerX = 1.0f; }
                            float scalerY = GetFromBitmapList(tile).PixelSize.Height / prntForm.tileSizeInPixels;
                            if (scalerY == 0) { scalerY = 1.0f; }
                            SharpDX.RectangleF src = new SharpDX.RectangleF(0, 0, GetFromBitmapList(tile).PixelSize.Width, GetFromBitmapList(tile).PixelSize.Height);
                            SharpDX.RectangleF dst = new SharpDX.RectangleF(x * sqr, y * sqr, (int)(sqr * scalerX), (int)(sqr * scalerY));
                            DrawD2DBitmap(GetFromBitmapList(tile), src, dst, thisEnc.Layer1Rotate[y * thisEnc.MapSizeX + x], thisEnc.Layer1Mirror[y * thisEnc.MapSizeX + x]);
                        }
                    }
                }
            }
            #endregion
            #region Draw Layer 2
            if (checkBox2.Checked)
            {
                for (int y = 0; y < thisEnc.MapSizeY; y++)
                {
                    for (int x = 0; x < thisEnc.MapSizeX; x++)
                    {
                        //TileEnc tile = thisEnc.encounterTiles[y * thisEnc.MapSizeX + x];
                        string tile = thisEnc.Layer2Filename[y * thisEnc.MapSizeX + x];
                        if (!tile.Equals("t_a_blank"))
                        {
                            float scalerX = GetFromBitmapList(tile).PixelSize.Width / prntForm.tileSizeInPixels;
                            if (scalerX == 0) { scalerX = 1.0f; }
                            float scalerY = GetFromBitmapList(tile).PixelSize.Height / prntForm.tileSizeInPixels;
                            if (scalerY == 0) { scalerY = 1.0f; }
                            SharpDX.RectangleF src = new SharpDX.RectangleF(0, 0, GetFromBitmapList(tile).PixelSize.Width, GetFromBitmapList(tile).PixelSize.Height);
                            SharpDX.RectangleF dst = new SharpDX.RectangleF(x * sqr, y * sqr, (int)(sqr * scalerX), (int)(sqr * scalerY));
                            DrawD2DBitmap(GetFromBitmapList(tile), src, dst, thisEnc.Layer2Rotate[y * thisEnc.MapSizeX + x], thisEnc.Layer2Mirror[y * thisEnc.MapSizeX + x]);
                        }
                    }
                }
            }
            #endregion
            #region Draw Layer 3
            if (checkBox3.Checked)
            {
                for (int y = 0; y < thisEnc.MapSizeY; y++)
                {
                    for (int x = 0; x < thisEnc.MapSizeX; x++)
                    {
                        //TileEnc tile = thisEnc.encounterTiles[y * thisEnc.MapSizeX + x];
                        string tile = thisEnc.Layer3Filename[y * thisEnc.MapSizeX + x];
                        if (!tile.Equals("t_a_blank"))
                        {
                            float scalerX = GetFromBitmapList(tile).PixelSize.Width / prntForm.tileSizeInPixels;
                            if (scalerX == 0) { scalerX = 1.0f; }
                            float scalerY = GetFromBitmapList(tile).PixelSize.Height / prntForm.tileSizeInPixels;
                            if (scalerY == 0) { scalerY = 1.0f; }
                            SharpDX.RectangleF src = new SharpDX.RectangleF(0, 0, GetFromBitmapList(tile).PixelSize.Width, GetFromBitmapList(tile).PixelSize.Height);
                            SharpDX.RectangleF dst = new SharpDX.RectangleF(x * sqr, y * sqr, (int)(sqr * scalerX), (int)(sqr * scalerY));
                            DrawD2DBitmap(GetFromBitmapList(tile), src, dst, thisEnc.Layer3Rotate[y * thisEnc.MapSizeX + x], thisEnc.Layer3Mirror[y * thisEnc.MapSizeX + x]);
                        }
                    }
                }
            }
            #endregion

            #region Draw Grid
            for (int y = 0; y < thisEnc.MapSizeY; y++)
            {
                for (int x = 0; x < thisEnc.MapSizeX; x++)
                {
                    //draw square walkmesh and LoS stuff
                    SharpDX.RectangleF src = new SharpDX.RectangleF(0, 0, GetFromBitmapList("walk_pass").PixelSize.Width, GetFromBitmapList("walk_pass").PixelSize.Height);
                    SharpDX.RectangleF dst = new SharpDX.RectangleF(x * sqr, y * sqr, sqr, sqr);
                    if (chkGrid.Checked) //if show grid is turned on, draw grid squares
                    {
                        if (thisEnc.LoSBlocked[y * thisEnc.MapSizeX + x] == 1)
                        {
                            DrawD2DBitmap(GetFromBitmapList("los_block"), src, dst, 0, 0);
                        }
                        if (thisEnc.Walkable[y * thisEnc.MapSizeX + x] == 1)
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

            #region Draw creatures
            foreach (CreatureRefs crtRef in thisEnc.encounterCreatureRefsList)
            {
                int cspx = crtRef.creatureStartLocationX * sqr;
                int cspy = crtRef.creatureStartLocationY * sqr;
                Creature crt = prntForm.getCreatureByResRef(crtRef.creatureResRef);
                float scalerX = GetFromBitmapList(crt.cr_tokenFilename).PixelSize.Width / prntForm.standardTokenSize;
                if (scalerX == 0) { scalerX = 1.0f; }
                float scalerY = GetFromBitmapList(crt.cr_tokenFilename).PixelSize.Height / (prntForm.standardTokenSize * 2);
                if (scalerY == 0) { scalerY = 1.0f; }
                SharpDX.RectangleF src = new SharpDX.RectangleF(0, 0, GetFromBitmapList(crt.cr_tokenFilename).PixelSize.Width, GetFromBitmapList(crt.cr_tokenFilename).PixelSize.Height / 2);
                SharpDX.RectangleF dst = new SharpDX.RectangleF(cspx, cspy, (int)(sqr * scalerX), (int)(sqr * scalerY));
                DrawD2DBitmap(GetFromBitmapList(crt.cr_tokenFilename), src, dst, 0, 0);
                //GDI device.DrawImage(crt.creatureIconBitmap, dst, src, GraphicsUnit.Pixel);
                float scaler = 1.0f;
                if (sqr == 50) { scaler = 10.0f; }
                else if (sqr == 25) { scaler = 5.0f; }
                else if (sqr == 10) { scaler = 2.0f; }
                DrawText(crtRef.creatureTag, cspx, cspy + (sqr / 2), scaler, SharpDX.Color.Yellow);
                //TODO device.DrawString(crtRef.creatureTag, drawFont, drawBrush, new Point(cspx, cspy + (sqr / 2)));
            }
            #endregion
            #region Draw PCs
            int cnt = 0;
            foreach (Coordinate PCpoint in thisEnc.encounterPcStartLocations)
            {
                int cspx = PCpoint.X * sqr;
                int cspy = PCpoint.Y * sqr;
                float scaler = 1.0f;
                if (sqr == 50) { scaler = 50f; }
                else if (sqr == 25) { scaler = 25f; }
                else if (sqr == 10) { scaler = 10f; }
                DrawText((cnt + 1).ToString(), cspx + 5, cspy, scaler, SharpDX.Color.Yellow);
                //TODO device.DrawString((cnt + 1).ToString(), drawFontNum, drawBrush, new Point(cspx, cspy + 5));
                cnt++;
            }
            #endregion

            #region Draw Props
            foreach (Prop prpRef in thisEnc.propsList)
            {
                SharpDX.RectangleF src = new SharpDX.RectangleF(0, 0, GetFromBitmapList(prpRef.ImageFileName).PixelSize.Width, GetFromBitmapList(prpRef.ImageFileName).PixelSize.Height);
                SharpDX.RectangleF dst = new SharpDX.RectangleF(prpRef.LocationX * sqr, prpRef.LocationY * sqr, sqr, sqr);
                if (prpRef.ImageFileName.StartsWith("tkn_"))
                {
                    src = new SharpDX.RectangleF(0, 0, GetFromBitmapList(prpRef.ImageFileName).PixelSize.Width, (GetFromBitmapList(prpRef.ImageFileName).PixelSize.Height) / 2);
                    dst = new SharpDX.RectangleF(prpRef.LocationX * sqr, prpRef.LocationY * sqr, sqr, sqr);
                }
                int mirror = 0;
                if (!prpRef.PropFacingLeft) { mirror = 1; }
                DrawD2DBitmap(GetFromBitmapList(prpRef.ImageFileName), src, dst, 0, mirror);
            }
            #endregion
            #region Draw Triggers
            foreach (Trigger t in thisEnc.Triggers)
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
                /*TODO try
                {
                    if (!selectedBitmapFilename.Equals(""))
                    {
                        SharpDX.RectangleF src = new SharpDX.RectangleF(0, 0, GetFromBitmapList(selectedBitmapFilename).PixelSize.Width, GetFromBitmapList(selectedBitmapFilename).PixelSize.Height);
                        SharpDX.RectangleF dst = new SharpDX.RectangleF(gridX * sqr, gridY * sqr, sqr, sqr);
                        DrawD2DBitmap(GetFromBitmapList(selectedBitmapFilename), src, dst, 0, false, 0, 0);
                    }
                }
                catch (Exception ex) { MessageBox.Show("failed mouse move update to be placed prop: " + ex.ToString()); }
                */
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
                        SharpDX.RectangleF dst = new SharpDX.RectangleF(gridX * sqr, gridY * sqr, (int)(sqr * scalerX), (int)(sqr * scalerY));
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
            /*TODO add props to encounters
            foreach (Prop prp in thisEnc.Props)
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
            }
            */
        }
        /*private void openLevel(string g_dir, string g_fil, string g_filNoEx)
        {
            this.Cursor = Cursors.WaitCursor;

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
            
            refreshLeftPanelInfo();
            panelView.Width = area.MapSizeX * sqr;
            panelView.Height = area.MapSizeY * sqr;
            panelView.BackgroundImage = (Image)surface;
            device = Graphics.FromImage(surface);
            if (surface == null)
            {
                MessageBox.Show("returned a null Map bitmap");
                return;
            }
            refreshMap(true);
            this.Cursor = Cursors.Arrow;
        }*/
        private void createNewArea(int width, int height)
        {
            //create tilemap
            thisEnc = null;
            thisEnc = new Encounter();
            thisEnc.MapSizeX = width;
            thisEnc.MapSizeY = height;
            for (int index = 0; index < (width * height); index++)
            {
                thisEnc.Layer1Filename.Add("t_grass");
                thisEnc.Layer1Rotate.Add(0);
                thisEnc.Layer1Mirror.Add(0);
                thisEnc.Layer2Filename.Add("t_a_blank");
                thisEnc.Layer2Rotate.Add(0);
                thisEnc.Layer2Mirror.Add(0);
                thisEnc.Layer3Filename.Add("t_a_blank");
                thisEnc.Layer3Rotate.Add(0);
                thisEnc.Layer3Mirror.Add(0);
                thisEnc.Walkable.Add(1);
                thisEnc.LoSBlocked.Add(0);
            }
            refreshLeftPanelInfo();
            //GDI panelView.Width = thisEnc.MapSizeX * sqr;
            //GDI panelView.Height = thisEnc.MapSizeY * sqr;
            //GDI panelView.BackgroundImage = (Image)surface;
            //GDI device = Graphics.FromImage(surface);
            //GDI if (surface == null)
            //GDI {
            //GDI MessageBox.Show("returned a null Map bitmap");
            //GDI return;
            //GDI }
            //GDI refreshMap(true);
        }
        private void resetAreaTileValues(int width, int height)
        {
            thisEnc.MapSizeX = width;
            thisEnc.MapSizeY = height;
            thisEnc.Layer1Filename.Clear();
            thisEnc.Layer1Rotate.Clear();
            thisEnc.Layer1Mirror.Clear();
            thisEnc.Layer2Filename.Clear();
            thisEnc.Layer2Rotate.Clear();
            thisEnc.Layer2Mirror.Clear();
            thisEnc.Layer3Filename.Clear();
            thisEnc.Layer3Rotate.Clear();
            thisEnc.Layer3Mirror.Clear();
            thisEnc.Walkable.Clear();
            thisEnc.LoSBlocked.Clear();
            for (int index = 0; index < (width * height); index++)
            {
                thisEnc.Layer1Filename.Add("t_grass");
                thisEnc.Layer1Rotate.Add(0);
                thisEnc.Layer1Mirror.Add(0);
                thisEnc.Layer2Filename.Add("t_a_blank");
                thisEnc.Layer2Rotate.Add(0);
                thisEnc.Layer2Mirror.Add(0);
                thisEnc.Layer3Filename.Add("t_a_blank");
                thisEnc.Layer3Rotate.Add(0);
                thisEnc.Layer3Mirror.Add(0);
                thisEnc.Walkable.Add(1);
                thisEnc.LoSBlocked.Add(0);
            }
        }
        public void refreshLeftPanelInfo()
        {
            lblMapSizeX.Text = thisEnc.MapSizeX.ToString();
            lblMapSizeY.Text = thisEnc.MapSizeY.ToString();
            if ((thisEnc.MapSizeX != 11) || (thisEnc.MapSizeY != 11))
            {
                lblMapSizeError.Visible = true;
            }
            else
            {
                lblMapSizeError.Visible = false;
            }
            selectedTile.x = gridX;
            selectedTile.y = gridY;
            selectedTile.index = gridY * thisEnc.MapSizeX + gridX;
            //GDI drawSelectionBox(gridX, gridY);
            selectionBoxLocation = new Point(gridX, gridY);
        }
        private void mapSizeChangeStuff()
        {
            lblMapSizeX.Text = thisEnc.MapSizeX.ToString();
            lblMapSizeY.Text = thisEnc.MapSizeY.ToString();
            refreshLeftPanelInfo();
            //GDI resetPanelAndDeviceSize();
        }
        public void ResetAllToFalse()
        {
            prntForm.selectedLevelMapCreatureTag = "";
            prntForm.selectedLevelMapPropTag = "";
            prntForm.CreatureSelected = false;
            prntForm.PropSelected = false;
            tileSelected = false;
            PcSelected = false;
            CrtSelected = false;
        }
        #endregion

        #region Event Handlers  
        private void btnAddItems_Click(object sender, EventArgs e)
        {
            try
            {
                Item it = prntForm.allItemsList[cmbItems.SelectedIndex];
                ItemRefs newIR = prntForm.createItemRefsFromItem(it);
                thisEnc.encounterInventoryRefsList.Add(newIR);
                refreshLbxItems();
            }
            catch { }
        }
        private void btnRemoveItems_Click(object sender, EventArgs e)
        {
            if (lbxItems.Items.Count > 0)
            {
                try
                {
                    if (lbxItems.SelectedIndex >= 0)
                        thisEnc.encounterInventoryRefsList.RemoveAt(lbxItems.SelectedIndex);
                }
                catch { }
                refreshLbxItems();
            }
        }
        private void btnPlacePCs_Click(object sender, EventArgs e)
        {
            rbtnInfo.Checked = true;
            ResetAllToFalse();
            PcSelected = true;
        }
        private void btnDeletePCs_Click(object sender, EventArgs e)
        {
            thisEnc.encounterPcStartLocations.Clear();
            //GDI refreshMap(true);
        }
        private void btnDeleteCreatures_Click(object sender, EventArgs e)
        {
            thisEnc.encounterCreatureRefsList.Clear();
            //GDI refreshMap(true);
        }
        private void flPanelTab1_MouseClick(object sender, MouseEventArgs e)
        {
            rbtnPaintTile.Checked = true;            
            ResetAllToFalse();
            tileSelected = true;
        }
        private void btnSelectedTerrain_Click(object sender, EventArgs e)
        {
            Button selectBtn = (Button)sender;
            currentTileFilename = selectBtn.Text;
            selectedBitmap = (Bitmap)selectBtn.BackgroundImage.Clone();
            panel1.BackgroundImage = selectedBitmap;
            rbtnPaintTile.Checked = true;
            ResetAllToFalse();
            tileSelected = true;
        }
        private void btnTileFilter_Click(object sender, EventArgs e)
        {
            Button selectBtn = (Button)sender;
            createTileImageButtons((string)selectBtn.Tag);
        }
        private void EncounterEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            //MessageBox.Show("closing editor and removing from openAreaList");
            //prntForm.openAreasList.Remove(area);
        }
        private void EncounterEditor_FormClosed(object sender, FormClosedEventArgs e)
        {
            DisposeAllD2D();
            //GDI device.Dispose();
            //GDI surface.Dispose();
            //gfxSelected.Dispose();
            //selectedBitmap.Dispose();
            //this.Close();
        }
        private void rbtnInfo_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtnInfo.Checked)
            {
                prntForm.logText("info on selecting map objects");
                prntForm.logText(Environment.NewLine);
                ResetAllToFalse();
            }
        }
        private void rbtnPaintTile_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtnPaintTile.Checked)
            {
                prntForm.logText("painting tiles" + Environment.NewLine);
                ResetAllToFalse();
                rbtnPaintTile.Checked = true;
                tileSelected = true;
            }
        }
        private void rbtnWalkable_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtnWalkable.Checked)
            {
                prntForm.logText("editing walkmesh");
                prntForm.logText(Environment.NewLine);
                ResetAllToFalse();
            }
        }
        private void rbtnLoS_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtnLoS.Checked)
            {
                prntForm.logText("editing line-of-sight mesh");
                prntForm.logText(Environment.NewLine);
                ResetAllToFalse();
            }
        }
        private void btnRemoveSelectedObject_Click(object sender, EventArgs e)
        {
            int cnt = 0;
            foreach (CreatureRefs crt in thisEnc.encounterCreatureRefsList)
            {
                if (crt.creatureTag == lastSelectedObjectTag)
                {
                    // remove at index of matched location
                    thisEnc.encounterCreatureRefsList.RemoveAt(cnt);
                    //crtBitmapList.RemoveAt(cnt);
                    //GDI refreshMap(true);
                    return;
                }
                cnt++;
            }
            cnt = 0;
            foreach (Prop prp in thisEnc.propsList)
            {
                if (prp.PropTag == lastSelectedObjectTag)
                {
                    // remove at index of matched tag
                    thisEnc.propsList.RemoveAt(cnt);
                    return;
                }
                cnt++;
            }
            foreach (Trigger t in thisEnc.Triggers)
            {
                if (t.TriggerTag == lastSelectedObjectTag)
                {
                    // remove at index of matched tag
                    thisEnc.Triggers.Remove(t);
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
                thisEnc.Triggers.Add(newTrigger);
                //set propertygrid to the new object
                prntForm.frmIceBlinkProperties.propertyGrid1.SelectedObject = newTrigger;
                prntForm.logText("painting a new trigger");
                prntForm.logText(Environment.NewLine);
                ResetAllToFalse();
            }
        }
        private void rbtnEditTrigger_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtnEditTrigger.Checked)
            {
                prntForm.logText("edit trigger: ");
                prntForm.logText(Environment.NewLine);
                ResetAllToFalse();
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
            foreach (Prop prp in thisEnc.propsList)
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
            foreach (Trigger t in thisEnc.Triggers)
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
        private void btnLoadMap_Click(object sender, EventArgs e)
        {
            if (mod.moduleName != "NewModule")
            {
                openFileDialog1.InitialDirectory = prntForm._mainDirectory + "\\modules\\" + mod.moduleName + "\\graphics";
            }
            else
            {
                openFileDialog1.InitialDirectory = prntForm._mainDirectory + "\\default\\NewModule\\graphics";
            }
            //Empty the FileName text box of the dialog
            openFileDialog1.FileName = String.Empty;
            openFileDialog1.Filter = "Map (*.jpg)|*.jpg";
            openFileDialog1.FilterIndex = 1;

            DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {
                using (Bitmap testSize = new Bitmap(Path.GetFullPath(openFileDialog1.FileName)))
                {
                    thisEnc.MapSizeX = testSize.Width / sqr;
                    thisEnc.MapSizeY = testSize.Height / sqr;
                }
                string filename = Path.GetFullPath(openFileDialog1.FileName);
                //thisEnc.MapImage = Path.GetFileNameWithoutExtension(openFileDialog1.FileName);
                //mapBitmapD2D = GetFromBitmapList(thisEnc.MapImage);
                //thisEnc.UseMapImage = true;
                resetAreaTileValues(thisEnc.MapSizeX, thisEnc.MapSizeY);
                //GDI mapSizeChangeStuff();
                //chkUseMapImage.Checked = true;
                //GDI refreshMap(true);
            }
        }
        private void chkUseMapImage_CheckedChanged(object sender, EventArgs e)
        {
            //thisEnc.UseMapImage = chkUseMapImage.Checked;
            //GDI refreshMap(true);
        }
        private void btnPlusLeftX_Click(object sender, EventArgs e)
        {
            //y * area.MapSizeX + x
            int oldX = thisEnc.MapSizeX;
            for (int i = thisEnc.Layer1Filename.Count - oldX; i >= 0; i -= oldX)
            {
                //TileEnc newTile = new TileEnc();
                //thisEnc.encounterTiles.Insert(i, newTile);
                thisEnc.Layer1Filename.Insert(i, "t_grass");
                thisEnc.Layer1Rotate.Insert(i, 0);
                thisEnc.Layer1Mirror.Insert(i, 0);
                thisEnc.Layer2Filename.Insert(i, "t_a_blank");
                thisEnc.Layer2Rotate.Insert(i, 0);
                thisEnc.Layer2Mirror.Insert(i, 0);
                thisEnc.Layer3Filename.Insert(i, "t_a_blank");
                thisEnc.Layer3Rotate.Insert(i, 0);
                thisEnc.Layer3Mirror.Insert(i, 0);
                thisEnc.Walkable.Insert(i, 1);
                thisEnc.LoSBlocked.Insert(i, 0);
            }
            foreach (CreatureRefs crtRef in thisEnc.encounterCreatureRefsList)
            {
                crtRef.creatureStartLocationX++;
            }
            foreach (Coordinate coor in thisEnc.encounterPcStartLocations)
            {
                coor.X++;
            }
            foreach (Prop prpRef in thisEnc.propsList)
            {
                prpRef.LocationX++;
            }
            foreach (Trigger t in thisEnc.Triggers)
            {
                foreach (Coordinate p in t.TriggerSquaresList)
                {
                    p.X++;
                }
            }
            thisEnc.MapSizeX++;
            mapSizeChangeStuff();
        }
        private void btnMinusLeftX_Click(object sender, EventArgs e)
        {
            //y * area.MapSizeX + x
            int oldX = thisEnc.MapSizeX;
            for (int i = thisEnc.Layer1Filename.Count - oldX; i >= 0; i -= oldX)
            {
                //thisEnc.encounterTiles.RemoveAt(i);
                thisEnc.Layer1Filename.RemoveAt(i);
                thisEnc.Layer1Rotate.RemoveAt(i);
                thisEnc.Layer1Mirror.RemoveAt(i);
                thisEnc.Layer2Filename.RemoveAt(i);
                thisEnc.Layer2Rotate.RemoveAt(i);
                thisEnc.Layer2Mirror.RemoveAt(i);
                thisEnc.Layer3Filename.RemoveAt(i);
                thisEnc.Layer3Rotate.RemoveAt(i);
                thisEnc.Layer3Mirror.RemoveAt(i);
                thisEnc.Walkable.RemoveAt(i);
                thisEnc.LoSBlocked.RemoveAt(i);
            }
            foreach (CreatureRefs crtRef in thisEnc.encounterCreatureRefsList)
            {
                crtRef.creatureStartLocationX--;
            }
            foreach (Coordinate coor in thisEnc.encounterPcStartLocations)
            {
                coor.X--;
            }
            foreach (Prop prpRef in thisEnc.propsList)
            {
                prpRef.LocationX--;
            }
            foreach (Trigger t in thisEnc.Triggers)
            {
                foreach (Coordinate p in t.TriggerSquaresList)
                {
                    p.X--;
                }
            }
            thisEnc.MapSizeX--;
            mapSizeChangeStuff();
        }
        private void btnPlusRightX_Click(object sender, EventArgs e)
        {
            //y * area.MapSizeX + x
            int oldX = thisEnc.MapSizeX;
            for (int i = thisEnc.Layer1Filename.Count - 1; i >= 0; i -= oldX)
            {
                //TileEnc newTile = new TileEnc();
                //thisEnc.encounterTiles.Insert(i + 1, newTile);
                thisEnc.Layer1Filename.Insert(i + 1, "t_grass");
                thisEnc.Layer1Rotate.Insert(i + 1, 0);
                thisEnc.Layer1Mirror.Insert(i + 1, 0);
                thisEnc.Layer2Filename.Insert(i + 1, "t_a_blank");
                thisEnc.Layer2Rotate.Insert(i + 1, 0);
                thisEnc.Layer2Mirror.Insert(i + 1, 0);
                thisEnc.Layer3Filename.Insert(i + 1, "t_a_blank");
                thisEnc.Layer3Rotate.Insert(i + 1, 0);
                thisEnc.Layer3Mirror.Insert(i + 1, 0);
                thisEnc.Walkable.Insert(i + 1, 1);
                thisEnc.LoSBlocked.Insert(i + 1, 0);
            }
            thisEnc.MapSizeX++;
            mapSizeChangeStuff();
        }
        private void btnMinusRightX_Click(object sender, EventArgs e)
        {
            //y * area.MapSizeX + x
            int oldX = thisEnc.MapSizeX;
            for (int i = thisEnc.Layer1Filename.Count - 1; i >= 0; i -= oldX)
            {
                //thisEnc.encounterTiles.RemoveAt(i);
                thisEnc.Layer1Filename.RemoveAt(i);
                thisEnc.Layer1Rotate.RemoveAt(i);
                thisEnc.Layer1Mirror.RemoveAt(i);
                thisEnc.Layer2Filename.RemoveAt(i);
                thisEnc.Layer2Rotate.RemoveAt(i);
                thisEnc.Layer2Mirror.RemoveAt(i);
                thisEnc.Layer3Filename.RemoveAt(i);
                thisEnc.Layer3Rotate.RemoveAt(i);
                thisEnc.Layer3Mirror.RemoveAt(i);
                thisEnc.Walkable.RemoveAt(i);
                thisEnc.LoSBlocked.RemoveAt(i);
            }
            thisEnc.MapSizeX--;
            mapSizeChangeStuff();
        }
        private void btnPlusTopY_Click(object sender, EventArgs e)
        {
            //y * area.MapSizeX + x
            for (int i = 0; i < thisEnc.MapSizeX; i++)
            {
                //TileEnc newTile = new TileEnc();
                //thisEnc.encounterTiles.Insert(0, newTile);
                thisEnc.Layer1Filename.Insert(0, "t_grass");
                thisEnc.Layer1Rotate.Insert(0, 0);
                thisEnc.Layer1Mirror.Insert(0, 0);
                thisEnc.Layer2Filename.Insert(0, "t_a_blank");
                thisEnc.Layer2Rotate.Insert(0, 0);
                thisEnc.Layer2Mirror.Insert(0, 0);
                thisEnc.Layer3Filename.Insert(0, "t_a_blank");
                thisEnc.Layer3Rotate.Insert(0, 0);
                thisEnc.Layer3Mirror.Insert(0, 0);
                thisEnc.Walkable.Insert(0, 1);
                thisEnc.LoSBlocked.Insert(0, 0);
            }
            foreach (CreatureRefs crtRef in thisEnc.encounterCreatureRefsList)
            {
                crtRef.creatureStartLocationY++;
            }
            foreach (Coordinate coor in thisEnc.encounterPcStartLocations)
            {
                coor.Y++;
            }
            foreach (Prop prpRef in thisEnc.propsList)
            {
                prpRef.LocationY++;
            }
            foreach (Trigger t in thisEnc.Triggers)
            {
                foreach (Coordinate p in t.TriggerSquaresList)
                {
                    p.Y++;
                }
            }
            thisEnc.MapSizeY++;
            mapSizeChangeStuff();
        }
        private void btnMinusTopY_Click(object sender, EventArgs e)
        {
            //y * area.MapSizeX + x
            for (int i = 0; i < thisEnc.MapSizeX; i++)
            {
                //thisEnc.encounterTiles.RemoveAt(0);
                thisEnc.Layer1Filename.RemoveAt(0);
                thisEnc.Layer1Rotate.RemoveAt(0);
                thisEnc.Layer1Mirror.RemoveAt(0);
                thisEnc.Layer2Filename.RemoveAt(0);
                thisEnc.Layer2Rotate.RemoveAt(0);
                thisEnc.Layer2Mirror.RemoveAt(0);
                thisEnc.Layer3Filename.RemoveAt(0);
                thisEnc.Layer3Rotate.RemoveAt(0);
                thisEnc.Layer3Mirror.RemoveAt(0);
                thisEnc.Walkable.RemoveAt(0);
                thisEnc.LoSBlocked.RemoveAt(0);
            }
            foreach (CreatureRefs crtRef in thisEnc.encounterCreatureRefsList)
            {
                crtRef.creatureStartLocationY--;
            }
            foreach (Coordinate coor in thisEnc.encounterPcStartLocations)
            {
                coor.Y--;
            }
            foreach (Prop prpRef in thisEnc.propsList)
            {
                prpRef.LocationY--;
            }
            foreach (Trigger t in thisEnc.Triggers)
            {
                foreach (Coordinate p in t.TriggerSquaresList)
                {
                    p.Y--;
                }
            }
            thisEnc.MapSizeY--;
            mapSizeChangeStuff();
        }
        private void btnPlusBottumY_Click(object sender, EventArgs e)
        {
            //y * area.MapSizeX + x
            for (int i = 0; i < thisEnc.MapSizeX; i++)
            {
                //TileEnc newTile = new TileEnc();
                //thisEnc.encounterTiles.Add(newTile);
                thisEnc.Layer1Filename.Add("t_grass");
                thisEnc.Layer1Rotate.Add(0);
                thisEnc.Layer1Mirror.Add(0);
                thisEnc.Layer2Filename.Add("t_a_blank");
                thisEnc.Layer2Rotate.Add(0);
                thisEnc.Layer2Mirror.Add(0);
                thisEnc.Layer3Filename.Add("t_a_blank");
                thisEnc.Layer3Rotate.Add(0);
                thisEnc.Layer3Mirror.Add(0);
                thisEnc.Walkable.Add(1);
                thisEnc.LoSBlocked.Add(0);
            }
            thisEnc.MapSizeY++;
            mapSizeChangeStuff();
        }
        private void btnMinusBottumY_Click(object sender, EventArgs e)
        {
            //y * area.MapSizeX + x
            for (int i = 0; i < thisEnc.MapSizeX; i++)
            {
                //thisEnc.encounterTiles.RemoveAt(thisEnc.encounterTiles.Count - 1);
                int total = thisEnc.LoSBlocked.Count;
                thisEnc.Layer1Filename.RemoveAt(total - 1);
                thisEnc.Layer1Rotate.RemoveAt(total - 1);
                thisEnc.Layer1Mirror.RemoveAt(total - 1);
                thisEnc.Layer2Filename.RemoveAt(total - 1);
                thisEnc.Layer2Rotate.RemoveAt(total - 1);
                thisEnc.Layer2Mirror.RemoveAt(total - 1);
                thisEnc.Layer3Filename.RemoveAt(total - 1);
                thisEnc.Layer3Rotate.RemoveAt(total - 1);
                thisEnc.Layer3Mirror.RemoveAt(total - 1);
                thisEnc.Walkable.RemoveAt(total - 1);
                thisEnc.LoSBlocked.RemoveAt(total - 1);
            }
            thisEnc.MapSizeY--;
            mapSizeChangeStuff();
        }
        private void btnProperties_Click(object sender, EventArgs e)
        {
            prntForm.frmIceBlinkProperties.propertyGrid1.SelectedObject = thisEnc;
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
                refreshMap(true);
                UpdatePB();
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
                foreach (CreatureRefs crt in thisEnc.encounterCreatureRefsList)
                {
                    if (crt.creatureTag == lastSelectedObjectTag)
                    {
                        // remove at index of matched location
                        thisEnc.encounterCreatureRefsList.RemoveAt(cnt);
                        return;
                    }
                    cnt++;
                }
                cnt = 0;
                foreach (Prop prp in thisEnc.propsList)
                {
                    if (prp.PropTag == lastSelectedObjectTag)
                    {
                        // remove at index of matched tag
                        thisEnc.propsList.RemoveAt(cnt);
                        return;
                    }
                    cnt++;
                }
                foreach (Trigger t in thisEnc.Triggers)
                {
                    if (t.TriggerTag == lastSelectedObjectTag)
                    {
                        // remove at index of matched tag
                        thisEnc.Triggers.Remove(t);
                        return;
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
