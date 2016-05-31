using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCX_Basic
{
    public class VariableSet
    {
        public String var;
        public String name;
        public bool stringType;

        public VariableSet()
        {
            var = "";
            name = "";
            stringType = false;
        }

        public String getVar()
        {
            return this.var;
        }

        public void setVar(String value)
        {
            var = value;
        }

        public String getName()
        {
            return this.name;
        }

        public void setName(String value)
        {
            name = value;
        }

        public bool getStringType()
        {
            return this.stringType;
        }

        public void setStringType(bool value)
        {
            stringType = value;
        }

    }
}
