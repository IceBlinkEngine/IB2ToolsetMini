using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IB2ToolsetMini
{
    public class StringForDropDownList
    {
        private string _stringValue = "none"; //item unique tag name

        [CategoryAttribute("01 - String value"), DescriptionAttribute("string value for this entry")]
        public string stringValue
        {
            get { return _stringValue; }
            set { _stringValue = value; }
        }
        public StringForDropDownList()
        {

        }
        public override string ToString()
        {
            return stringValue;
        }
    }
}
