using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IB2ToolsetMini
{
    [Serializable]
    public class DlgSyncStruct
    {
        public uint Index = 0;
        public bool ShowOnce = false;
        public bool IsChild = false;
        public List<Condition> conditions = new List<Condition>();
        public string scriptInfoForEndOfText = "";

        public DlgSyncStruct()
        {

        }
        public DlgSyncStruct DeepCopy()
        {
            DlgSyncStruct copy = new DlgSyncStruct();
            copy.Index = this.Index;
            copy.ShowOnce = this.ShowOnce;
            copy.IsChild = this.IsChild;
            copy.scriptInfoForEndOfText = this.scriptInfoForEndOfText;
            copy.conditions.Clear();
            foreach (Condition c in this.conditions)
            {
                copy.conditions.Add(c.DeepCopy());
            }
            return copy;
        }
    }
}
