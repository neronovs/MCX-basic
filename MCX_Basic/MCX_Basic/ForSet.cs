using System;

namespace MCX_Basic
{
    public class ForSet //implements Serializable
    {
        public String forLine;
        public String forName;
        public String forStep;
        public String forTo;

        public ForSet()
        {
            forLine = "";
            forName = "";
            forStep = "";
            forTo = "";
        }

        public String getForLine()
        {
            return this.forLine;
        }

        public void setForLine(String value)
        {
            forLine = value;
        }

        public String getForName()
        {
            return this.forName;
        }

        public void setForName(String value)
        {
            forName = value;
        }

        public String getForStep()
        {
            return this.forStep;
        }

        public void setForStep(String value)
        {
            forStep = value;
        }

        public String getForTo()
        {
            return this.forTo;
        }

        public void setForTo(String value)
        {
            forTo = value;
        }
    }
}