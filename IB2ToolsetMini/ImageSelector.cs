using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IB2ToolsetMini
{
    public partial class ImageSelector : Form
    {
        private Module mod;
        private ParentForm prntForm;
        public Dictionary<string, Bitmap> imageList = new Dictionary<string, Bitmap>();
        public string prefix = "";

        public ImageSelector(Module m, ParentForm pf, string Prefix)
        {
            InitializeComponent();
            mod = m;
            prntForm = pf;
            prefix = Prefix;
            loadImageList();
            createButtons();
        }
        private void setFormSize()
        {
            //this.flowLayoutPanel1.Height = (this.flowLayoutPanel1.Controls.Count / 10) * 100;
        }
        private void loadImageList()
        {
            imageList.Clear();
            //first load from module ImageList
            foreach (KeyValuePair<string, Bitmap> kvp in prntForm.resourcesBitmapList)
            {
                if (kvp.Key.StartsWith(prefix))
                {
                    imageList.Add(kvp.Key, kvp.Value);
                }
            }
            //next load from default folder graphics
            string jobDir = "";
            jobDir = prntForm._mainDirectory + "\\default\\NewModule\\graphics";
            foreach (string f in Directory.GetFiles(jobDir, "*.*"))
            {
                string name = Path.GetFileNameWithoutExtension(f);
                if (name.StartsWith(prefix))
                {
                    if (!imageList.ContainsKey(name))
                    {
                        imageList.Add(name, new Bitmap(f));
                    }
                }                
            }
        }
        private void createButtons()
        {
            this.flowLayoutPanel1.Controls.Clear();
            foreach (KeyValuePair<string, Bitmap> kvp in imageList)
            {
                Button btnNew = new Button();
                // 
                // btnOne
                // 
                btnNew.BackgroundImage = kvp.Value;
                btnNew.BackgroundImageLayout = ImageLayout.Zoom;
                btnNew.FlatAppearance.BorderColor = System.Drawing.Color.Black;
                btnNew.FlatAppearance.BorderSize = 2;
                btnNew.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
                btnNew.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Green;
                btnNew.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                btnNew.Name = kvp.Key;
                btnNew.Size = new System.Drawing.Size(kvp.Value.Width * 2, kvp.Value.Height * 2);
                btnNew.TabIndex = 1;
                btnNew.Text = kvp.Key;
                btnNew.UseVisualStyleBackColor = true;
                btnNew.Click += new System.EventHandler(this.btnSelectedImage_Click);
                this.flowLayoutPanel1.Controls.Add(btnNew);
            }
            setFormSize();
        }
        private void btnSelectedImage_Click(object sender, EventArgs e)
        {
            Button selectBtn = (Button)sender;
            prntForm.returnImageFilenameFromImageSelector = selectBtn.Name;
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}
