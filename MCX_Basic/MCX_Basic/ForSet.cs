using System;
using System.Runtime.Serialization;

namespace MCX_Basic
{
    public class ForSet
    {
        private String forLine;
        private String forName;
        private String forStep;
        private String forTo;

        public ForSet()
        {
            forLine = "";
            forName = "";
            forStep = "";
            forTo = "";
        }

        public String ForLine
        {
            get { return this.forLine; }
            set { forLine = value; }
        }

        public String ForName
        {
            get { return this.forName; }
            set { forName = value; }
        }

        public String ForStep
        {
            get { return this.forStep; }
            set { forStep = value; }
        }

        public String ForTo
        {
            get { return this.forTo; }
            set { forTo = value; }
        }
        
    }
}