using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IB2ToolsetMini
{
    public class ErfFile
    {
        public byte[] fileBytes;
        public ErfHeader thisHeader = new ErfHeader();
        public string LocalizedString = "";
        public List<ErfKeyStruct> KeyList = new List<ErfKeyStruct>();
        public List<ErfResourceStruct> ResourceList = new List<ErfResourceStruct>();

        public ErfFile()
        {
        }
        public ErfFile(string filename)
        {
            fileBytes = File.ReadAllBytes(filename);
            ReadHeader();
            ReadLocalizedString();
            LoadKeyList();
            LoadResourceList();
            File.Delete("keylist.txt");
            File.AppendAllText("keylist.txt", "ResRef" + ":" + "ResID" + ":" + "ResType" + ":" + "Unused" + Environment.NewLine);
            foreach (ErfKeyStruct ks in KeyList)
            {
                File.AppendAllText("keylist.txt", ks.ResRef + ":" + ks.ResID.ToString() + ":" + ((ResourceType)ks.ResType).ToString() + ":" + ks.Unused.ToString() + Environment.NewLine);
            }
            for (int i = 0; i < thisHeader.EntryCount; i++)
            {
                byte[] newArray = new byte[ResourceList[i].ResourceSize];
                Array.Copy(fileBytes, ResourceList[i].OffsetToResource, newArray, 0, ResourceList[i].ResourceSize);
                //File.WriteAllBytes("data\\" + KeyList[i].ResRef + "." + ((ResourceType)KeyList[i].ResType).ToString(), newArray);
            }
        }

        public void ReadHeader()
        {
            int i = 0;
            thisHeader.FileType = readString(fileBytes, i, 4); //4 char
            thisHeader.Version = readString(fileBytes, i += 4, 4); //4 char
            thisHeader.LanguageCount = readInt(fileBytes, i += 4, 4); //32bits
            thisHeader.LocalizedStringSize = readInt(fileBytes, i += 4, 4); //32bits
            thisHeader.EntryCount = readInt(fileBytes, i += 4, 4); //32bits
            thisHeader.OffsetToLocalizedString = readInt(fileBytes, i += 4, 4); //32bits
            thisHeader.OffsetToKeyList = readInt(fileBytes, i += 4, 4); //32bits
            thisHeader.OffsetToResourceList = readInt(fileBytes, i += 4, 4); //32bits
            thisHeader.BuildYear = readInt(fileBytes, i += 4, 4); //4 bytes
            thisHeader.BuildDay = readInt(fileBytes, i += 4, 4); //4 bytes
            thisHeader.DescriptionStrRef = readInt(fileBytes, i += 4, 4); //4 bytes
        }
        public void ReadLocalizedString()
        {
            //can ignore LangId (32bits) and StringSize (32bits)
            int id = readInt(fileBytes, thisHeader.OffsetToLocalizedString, 4);
            int size = readInt(fileBytes, thisHeader.OffsetToLocalizedString + 4, 4);
            LocalizedString = readString(fileBytes, thisHeader.OffsetToLocalizedString + 8, size);
        }
        public void LoadKeyList()
        {
            int offset = thisHeader.OffsetToKeyList - 2;
            KeyList.Clear();
            for (int i = 0; i < thisHeader.EntryCount; i++)
            {
                ErfKeyStruct newKeyStruct = new ErfKeyStruct();
                newKeyStruct.ResRef = readString(fileBytes, offset += 2, 16);
                newKeyStruct.ResID = readInt(fileBytes, offset += 16, 4);
                newKeyStruct.ResType = (ResourceType)readShort(fileBytes, offset += 4, 2);
                newKeyStruct.Unused = readShort(fileBytes, offset += 2, 2);
                KeyList.Add(newKeyStruct);
            }
        }
        public void LoadResourceList()
        {
            int offset = thisHeader.OffsetToResourceList - 4;
            ResourceList.Clear();
            for (int i = 0; i < thisHeader.EntryCount; i++)
            {
                ErfResourceStruct newResourceStruct = new ErfResourceStruct();
                newResourceStruct.OffsetToResource = readInt(fileBytes, offset += 4, 4);
                newResourceStruct.ResourceSize = readInt(fileBytes, offset += 4, 4);
                ResourceList.Add(newResourceStruct);
            }
        }

        public string readString(byte[] array, int index, int length)
        {
            string val = "";
            for (int i = index; i < index + length; i++)
            {
                char c = Convert.ToChar(array[i]);
                if (c == '\0') { continue; }
                val += Convert.ToChar(array[i]).ToString();
            }
            return val;
        }
        public int readInt(byte[] array, int index, int length)
        {
            byte[] newArray = new byte[length];
            Array.Copy(array, index, newArray, 0, length);

            int i = BitConverter.ToInt32(newArray, 0);
            return i;
        }
        public ushort readShort(byte[] array, int index, int length)
        {
            byte[] newArray = new byte[length];
            Array.Copy(array, index, newArray, 0, length);
            ushort i = BitConverter.ToUInt16(newArray, 0);
            return i;
        }
    }
}
