using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IB2ToolsetMini
{
    public class IBScript
    {
        private string _scriptName = "newScript";
        public List<string> codeLines = new List<string>();

        public string scriptName
        {
            get { return _scriptName; }
            set { _scriptName = value; }
        }
        public IBScript()
        {

        }
    }
}
