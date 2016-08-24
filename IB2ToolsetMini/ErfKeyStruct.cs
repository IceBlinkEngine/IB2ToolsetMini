using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IB2ToolsetMini
{
    public class ErfKeyStruct
    {
        public string ResRef = "";
        public int ResID = 0;
        public ResourceType ResType = ResourceType.are;
        public ushort Unused = 0;

        public ErfKeyStruct()
        {

        }
    }
}
