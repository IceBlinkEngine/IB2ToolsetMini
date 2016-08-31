using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace IB2ToolsetMini
{
    public class FileNameSelectEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            Prop p = (Prop)context.Instance;
            using (var sel = new ImageSelector(p.prntForm.mod, p.prntForm, "prp_"))
            {                
                var result = sel.ShowDialog();
                if (result == DialogResult.OK) // Test result.
                {
                    return p.prntForm.returnImageFilenameFromImageSelector;
                }
            }
            /*using (FileDialog dlg = new OpenFileDialog())
            {
                //dlg.InitialDirectory = (string)value;
                dlg.FileName = "prp_*";
                dlg.Filter = "Image (*.png)|*.png|All Files (*.*)|*.*";
                dlg.FilterIndex = 1;
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    return Path.GetFileNameWithoutExtension(dlg.FileName);                    
                }
            }*/
            return base.EditValue(context, provider, value);
        }
    }
}
