using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IB2ToolsetMini
{
    public class ErfHeader
    {
        public string FileType = ""; //4 char
        public string Version = "";  //4 char
        public int LanguageCount = 0; //32bits
        public int LocalizedStringSize = 0; //32bits
        public int EntryCount = 0; //32bits
        public int OffsetToLocalizedString = 0; //32bits
        public int OffsetToKeyList = 0; //32bits
        public int OffsetToResourceList = 0; //32bits
        public int BuildYear = 0; //4 bytes
        public int BuildDay = 0; //4 bytes
        public int DescriptionStrRef = 0; //4 bytes
        public byte[] Reserved = new byte[116]; //NULL for future use

        public ErfHeader()
        {

        }
    }
}
