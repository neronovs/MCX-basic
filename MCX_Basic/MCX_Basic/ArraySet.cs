using System;
using System.Collections.Generic;

namespace MCX_Basic
{
    public class ArraySet
    {
        public String name;
        public List<String> value;
        public int size;

        public ArraySet()
        {
            name = "";
            value = new List<String>();
            size = 0;
        }

        public String getName()
        {
            return this.name;
        }

        public void setName(String value)
        {
            name = value;
        }

        public List<String> getValue()
        {
            return this.value;
        }

        public void setValue(List<String> value1)
        {
            value = value1;
        }

        public int getSize()
        {
            return this.size;
        }

        public void setSize(int value)
        {
            size = value;
        }

    }
}