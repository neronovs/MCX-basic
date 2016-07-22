using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCX_Basic
{
    public class NSRange
    {
        public int location;
        public int length;

        public NSRange(int location, int length)
        {
            this.location = location;
            this.length = length;
        }

        public int getLocation()
        {
            return this.location;
        }

        public void setLocaion(int value)
        {
            location = value;
        }

        public int getLength()
        {
            return this.length;
        }

        public void setLength(int value)
        {
            length = value;
        }
    }
}
