using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCX_Basic
{
    class NormalizeString
    {
        private static int NSNotFound = -1;
        private static bool NO = false;
        private static bool YES = true;
        //private static String TAG = MainActivity.class.getSimpleName();

        DigitalFunc digitalFunc;

        public String replaceCharWithCharInText(char sign, char witbSign, String string_val)
        {
            String result = "";
            int indexFirst = 0;
            bool foundFirst = NO;
            if (string_val.Length > 0)
            {
                for (int i = 0; i < string_val.Length; i++)
                {
                    if (string_val.Substring(i, 1).Equals("\"") && !foundFirst)
                    {
                        foundFirst = YES;
                        indexFirst = i;
                    }
                    if (string_val.Substring(i, 1).Equals("\"") && foundFirst && indexFirst != i)
                    {
                        foundFirst = NO;
                    }
                    if (!foundFirst)
                    {
                        result = result + string_val.Substring(i, 1);
                    }
                    else {
                        if (string_val.Substring(i, 1).Equals(sign.ToString()))
                        {
                            result = result + witbSign;
                            ////Log.d(TAG, "± " + result);
                        }
                        else {
                            result = result + string_val.Substring(i, 1);
                        }
                    }
                }
            }
            return result;
        }

        public String lowcaseWithText(String string_var)
        {
            String result = "";
            int indexFirst = 0;
            bool foundFirst = NO;
            if (string_var.Length > 0)
            {
                //Debug.WriteLine("NormalizeString -> " + string_var);

                for (int i = 0; i < string_var.Length; i++)
                {
                    if (string_var.Substring(i, 1).Equals("\"") && !foundFirst)
                    {
                        foundFirst = YES;
                        indexFirst = i;
                    }
                    if (string_var.Substring(i, 1).Equals("\"") && foundFirst && indexFirst != i)
                    {
                        foundFirst = NO;
                    }
                    if (foundFirst)
                    {
                        result = result + string_var.Substring(i, 1);
                    }
                    if (!foundFirst)
                    {
                        result = result + string_var.Substring(i, 1).ToLower();
                    }
                }
            }
            return result;
        }

        public String removeSpacesWithText(String string_var)
        {
            String result = "";
            int indexFirst = 0;
            bool foundFirst = NO;
            if (string_var.Length > 0)
            {
                for (int i = 0; i < string_var.Length; i++)
                {

                    if (string_var.Substring(i, 1).Equals("\"") && !foundFirst)
                    {
                        foundFirst = YES;
                        indexFirst = i;
                    }
                    if (string_var.Substring(i, 1).Equals("\"") && foundFirst && indexFirst != i)
                    {
                        foundFirst = NO;
                    }
                    if (foundFirst)
                    {
                        result = result + string_var.Substring(i, 1);
                    }
                    if (!foundFirst && !string_var.Substring(i, 1).Equals(" "))
                    {
                        result = result + string_var.Substring(i, 1);
                    }
                }
            }
            return result;
        }

        public String removeSpaceInBegin(String string_var)
        {
            int i = 0;
            while (i < string_var.Length && string_var.Substring(i, 1).Equals(" ")) i++;
            return string_var.Substring(i);
        }

        public String removeSpaceInBeginAndEnd(String string_var)
        {
            return string_var.Trim();
        }

        public String removeText(String string_var)
        {
            List<String>  arr = new List<String> ();
            int indexFirst = 0;
            bool foundFirst = NO;
            if (string_var.Length > 0)
            {
                for (int i = 0; i < string_var.Length; i++)
                {
                    if (string_var.Substring(i, 1).Equals("\"") && !foundFirst)
                    {
                        foundFirst = YES;
                        indexFirst = i;
                    }
                    if (string_var.Substring(i, 1).Equals("\"") && foundFirst && indexFirst != i)
                    {
                        foundFirst = NO;
                        NSRange range = new NSRange(indexFirst, i - indexFirst + 1);
                        arr.Add(string_var.Substring(range.location, range.length));
                    }
                }
            }
            else {
                GlobalVars.getInstance().error = "Syntax error\n";
            }
            if (foundFirst)
            {
                GlobalVars.getInstance().error = "Syntax error\n";
            }
            else {
                for (int i = 0; i < arr.Count(); i++)
                {
                    ////Log.d(TAG, "± removeText->" + arr[i].ToString());
                    string_var = string_var.Replace(arr[i].ToString(), "");
                }
            }
            return string_var;
        }

        public bool insideText(String string_var, int index)
        {
            int indexFirst = 0;
            bool foundFirst = NO;
            bool result = NO;
            if (string_var.Length > 0)
            {
                for (int i = 0; i < string_var.Length; i++)
                {
                    if (string_var.Substring(i, 1).Equals("\"") && !foundFirst)
                    {
                        foundFirst = YES;
                        indexFirst = i;
                    }
                    if (string_var.Substring(i, 1).Equals("\"") && foundFirst && indexFirst != i)
                    {
                        foundFirst = NO;
                    }
                    if (foundFirst && i == index)
                    {
                        result = YES;
                    }
                }
            }
            return result;
        }

        public bool isPairedQuotes(String string_var)
        {
            int indexFirst = 0;
            bool foundFirst = NO;
            bool result = YES;
            if (string_var.Length > 0)
            {
                for (int i = 0; i < string_var.Length; i++)
                {
                    if (string_var.Substring(i, 1).Equals("\"") && !foundFirst)
                    {
                        foundFirst = YES;
                        result = NO;
                        indexFirst = i;
                    }
                    if (string_var.Substring(i, 1).Equals("\"") && foundFirst && indexFirst != i)
                    {
                        foundFirst = NO;
                        result = YES;
                    }
                }
            }
            return result;
        }

        public bool isText(String string_var)
        {
            bool foundFirst = NO;
            bool result = NO;
            if (string_var.Length > 0)
            {
                if (string_var.Substring(0, 1).Equals("\"")) foundFirst = YES;
                if (string_var.Substring(string_var.Length - 1).Equals("\"") && foundFirst) result = YES;
            }
            return result;
        }

        public bool isPairedBracket(String string_var)
        {
            int indexFirst = 0;
            bool foundFirst = NO;
            bool result = YES;
            if (string_var.Length > 0)
            {
                for (int i = 0; i < string_var.Length; i++)
                {
                    if (string_var.Substring(i, 1).Equals("(") && !foundFirst)
                    {
                        foundFirst = YES;
                        result = NO;
                        indexFirst = i;
                    }
                    if (string_var.Substring(i, 1).Equals(")") && foundFirst && indexFirst != i)
                    {
                        foundFirst = NO;
                        result = YES;
                    }
                }
            }
            return result;
        }

        public List<String>  extractNumToArray(String string_var)
        {
            List<String>  arr = new List<String> (string_var.Split(','));
            return arr;
        }

        public List<String>  extractTextToArray(String string_var)
        {
            List<String>  arr = new List<String> ();
            int indexFirst = 0;
            bool foundFirst = NO;
            if (string_var.Length > 0)
            {
                for (int i = 0; i < string_var.Length; i++)
                {
                    if (string_var.Substring(i, 1).Equals("\"") && !foundFirst)
                    {
                        foundFirst = YES;
                        indexFirst = i + 1;
                    }
                    if (string_var.Substring(i, 1).Equals("\"") && foundFirst && indexFirst != i + 1)
                    {
                        foundFirst = NO;
                        NSRange range = new NSRange(indexFirst, i - indexFirst);
                        arr.Add(string_var.Substring(range.location, range.length));
                    }
                }
            }
            else {
                GlobalVars.getInstance().error = "Syntax error\n";
            }

            if (foundFirst || arr.Count() != 2)
            {
                GlobalVars.getInstance().error = "Syntax error\n";
            }
            else {
                ////Log.d(TAG, "± extracted Text->" + arr);
            }
            return arr;
        }

        public List<String>  extractTextAndOtherToArray(String string_var)
        {
            List<String>  arr = new List<String> ();
            int index = 0;
            int indexFirst = 0;
            bool foundFirst = NO;
            bool haveText = NO;
            if (string_var.Length > 0)
            {
                for (int i = 0; i < string_var.Length; i++)
                {
                    if (string_var.Substring(i, 1).Equals("\"") && !foundFirst)
                    {
                        foundFirst = YES;
                        indexFirst = i;
                        NSRange range = new NSRange(index, i - index);
                        arr.Add(string_var.Substring(range.location, range.length));
                    }
                    if (string_var.Substring(i, 1).Equals("\"") && foundFirst && indexFirst != i)
                    {
                        foundFirst = NO;
                        index = i + 1;
                        haveText = YES;
                        NSRange range = new NSRange(indexFirst, i - indexFirst + 1);
                        arr.Add(string_var.Substring(range.location, range.length));
                    }
                }
                if (!haveText)
                {
                    arr.Add(string_var);
                }
                else {
                    NSRange range = new NSRange(index, string_var.Length - index);
                    arr.Add(string_var.Substring(range.location, range.length));
                }
            }
            else {
                GlobalVars.getInstance().error = "Syntax error\n";
            }
            ////Log.d(TAG, "± extract TextAndOther To Array ->" + arr);
            return arr;
        }

        public List<String>  extractTextAndNumToArray(String string_var)
        {
            List<String>  arr = new List<String> ();
            int indexFirst = 0;
            bool foundFirst = NO;
            if (string_var.Length > 0)
            {
                for (int i = 0; i < string_var.Length; i++)
                {
                    if (string_var.Substring(i, 1).Equals("\"") && !foundFirst)
                    {
                        foundFirst = YES;
                        indexFirst = i + 1;
                    }
                    if (string_var.Substring(i, 1).Equals("\"") && foundFirst && indexFirst != i + 1)
                    {
                        foundFirst = NO;
                        NSRange range = new NSRange(indexFirst, i - indexFirst);
                        arr.Add(string_var.Substring(range.location, range.length));
                    }
                    if (string_var.Substring(i, 1).Equals("+") || string_var.Substring(i, 1).Equals(","))
                    {
                        foundFirst = NO;
                        NSRange range = new NSRange(i + 1, string_var.Length - i - 1);
                        String str = string_var.Substring(range.location, range.length);
                        if (str.IndexOf(',') != NSNotFound) str = str.Split(',')[0];
                        arr.Add(str);
                    }
                }
            }
            else {
                GlobalVars.getInstance().error = "Syntax error\n";
            }
            if (foundFirst || arr.Count() < 2)
            {
                GlobalVars.getInstance().error = "Syntax error\n";
            }
            else {
                ////Log.d(TAG, "± extract TextAndNum To Array ->" + arr);
            }

            return arr;
        }

        public List<String>  stringAndDigitsSeparateToArray(String string_var)
        {
            List<String>  arr = new List<String> ();
            int arrIndex = 0;
            int indexFirst = 0;
            bool foundFirst = NO;
            if (string_var.Length > 0)
            {
                for (int i = 0; i < string_var.Length; i++)
                {
                    if (string_var.Substring(i, 1).Equals("\"") && !foundFirst)
                    {
                        foundFirst = YES;
                        indexFirst = i + 1;
                    }
                    if (string_var.Substring(i, 1).Equals("\"") && foundFirst && indexFirst != i + 1)
                    {
                        foundFirst = NO;
                    }
                    if ((string_var.Substring(i, 1).Equals("+") || string_var.Substring(i, 1).Equals("-")
                            || string_var.Substring(i, 1).Equals("/") || string_var.Substring(i, 1).Equals("*")
                            || string_var.Substring(i, 1).Equals("^") || string_var.Substring(i, 1).Equals(",")) && !foundFirst)
                    {
                        NSRange range = new NSRange(arrIndex, i - arrIndex);
                        arrIndex = i + 1;
                        arr.Add(string_var.Substring(range.location, range.length));
                    }
                    if (i == string_var.Length - 1)
                    {
                        NSRange range = new NSRange(arrIndex, i - arrIndex + 1);
                        arr.Add(string_var.Substring(range.location, range.length));
                    }
                    ////Log.d(TAG, "±  -->'" + string_var.Substring(i, 1));
                }
                for (int i = 0; i < arr.Count; i++)
                {
                    if (arr[i].ToString().Length > 4)
                    {
                        String tmp = arr[i].ToString().Substring(0, 5).Trim();
                        if (tmp.Equals("instr"))
                        {
                            arr[i] = arr[i].ToString() + "," + arr[i + 1].ToString();
                            arr.RemoveAt(i + 1);
                        }
                    }
                }
                for (int i = 0; i < arr.Count; i++) arr[i] = arr[i].ToString().Trim();
            }
            if (foundFirst)
            {
                GlobalVars.getInstance().error = "Syntax error\n";
            }
            else {
                //        NSLog(@"Separated TEXT - %@",arr);
            }
            //for (int i = 0; i < arr.Count; i++)
                ////Log.d(TAG, "± extracted arr->'" + arr[i].ToString() + "'");

            return arr;
        }

        public List<String>  stringSeparateAllToArray(String string_var)
        {
            List<String>  arr = new List<String> ();
            int arrIndex = 0;
            int indexFirst = 0;
            bool foundFirst = NO;
            if (string_var.Length > 0)
            {
                for (int i = 0; i < string_var.Length; i++)
                {
                    if (string_var.Substring(i, 1).Equals("\"") && !foundFirst)
                    {
                        foundFirst = YES;
                        indexFirst = i + 1;
                    }
                    if (string_var.Substring(i, 1).Equals("\"") && foundFirst && indexFirst != i + 1)
                    {
                        foundFirst = NO;
                    }
                    if (string_var.Substring(i, 1).Equals(",") && !foundFirst)
                    {
                        NSRange range = new NSRange(arrIndex, i - arrIndex);
                        arrIndex = i + 1;
                        arr.Add(string_var.Substring(range.location, range.length));
                        arr.Add(",");
                    }
                    if (string_var.Substring(i, 1).Equals("+") && !foundFirst)
                    {
                        NSRange range = new NSRange(arrIndex, i - arrIndex);
                        String tmp = string_var.Substring(range.location, range.length);
                        {
                            arrIndex = i + 1;
                            arr.Add(tmp);
                            arr.Add("+");
                        }
                    }
                    if (string_var.Substring(i, 1).Equals("-") && !foundFirst)
                    {
                        NSRange range = new NSRange(arrIndex, i - arrIndex);
                        String tmp = string_var.Substring(range.location, range.length);
                        {
                            arrIndex = i + 1;
                            arr.Add(tmp);
                            arr.Add("-");
                        }
                    }
                    if (string_var.Substring(i, 1).Equals("/") && !foundFirst)
                    {
                        NSRange range = new NSRange(arrIndex, i - arrIndex);
                        String tmp = string_var.Substring(range.location, range.length);
                        {
                            arrIndex = i + 1;
                            arr.Add(tmp);
                            arr.Add("/");
                        }
                    }
                    if (string_var.Substring(i, 1).Equals("*") && !foundFirst)
                    {
                        NSRange range = new NSRange(arrIndex, i - arrIndex);
                        String tmp = string_var.Substring(range.location, range.length);
                        {
                            arrIndex = i + 1;
                            arr.Add(tmp);
                            arr.Add("*");
                        }
                    }
                    if (string_var.Substring(i, 1).Equals("^") && !foundFirst)
                    {
                        NSRange range = new NSRange(arrIndex, i - arrIndex);
                        String tmp = string_var.Substring(range.location, range.length);
                        {
                            arrIndex = i + 1;
                            arr.Add(tmp);
                            arr.Add("^");
                        }
                    }
                    if (string_var.Substring(i, 1).Equals("(") && !foundFirst)
                    {
                        NSRange range = new NSRange(arrIndex, i - arrIndex);
                        String tmp = string_var.Substring(range.location, range.length);
                        {
                            arrIndex = i + 1;
                            arr.Add(tmp);
                            arr.Add("(");
                        }
                    }
                    if (string_var.Substring(i, 1).Equals(")") && !foundFirst)
                    {
                        NSRange range = new NSRange(arrIndex, i - arrIndex);
                        String tmp = string_var.Substring(range.location, range.length);
                        {
                            arrIndex = i + 1;
                            arr.Add(tmp);
                            arr.Add(")");
                        }
                    }
                    if (i == string_var.Length - 1)
                    {
                        NSRange range = new NSRange(arrIndex, i - arrIndex + 1);
                        arr.Add(string_var.Substring(range.location, range.length));
                    }
                }
            }
            if (foundFirst)
            {
                GlobalVars.getInstance().error = "Syntax error\n";
            }
            else {
                //        NSLog(@"Separated ALL TEXT - %@",arr);
            }

            //        for (int i=0; i<arr.Count; i++) //Log.d(TAG, "± extracted arr->'" + arr[i].ToString()+"'");
            return arr;
        }

        public List<String> stringSeparateToArray(String string_val)
        {
            List<String> arr = new List<String>();
            int arrIndex = 0;
            int indexFirst = 0;
            bool foundFirst = NO;
            if (string_val.Length > 0)
            {
                for (int i = 0; i < string_val.Length; i++)
                {
                    if (string_val.Substring(i, 1).Equals("\"") && !foundFirst)
                    {
                        foundFirst = YES;
                        indexFirst = i + 1;
                    }
                    if (string_val.Substring(i, 1).Equals("\"") && foundFirst && indexFirst != i + 1)
                    {
                        foundFirst = NO;
                    }
                    if (string_val.Substring(i, 1).Equals(",") && !foundFirst)
                    {
                        NSRange range = new NSRange(arrIndex, i - arrIndex);
                        arrIndex = i + 1;
                        arr.Add(string_val.Substring(range.location, range.length));
                    }
                    if (string_val.Substring(i, 1).Equals("+") && !foundFirst)
                    {
                        NSRange range = new NSRange(arrIndex, i - arrIndex);
                        String tmp = string_val.Substring(range.location, range.length);
                        {
                            arrIndex = i + 1;
                            arr.Add(tmp);
                        }
                    }
                    if (i == string_val.Length - 1)
                    {
                        NSRange range = new NSRange(arrIndex, i - arrIndex + 1);
                        arr.Add(string_val.Substring(range.location, range.length));
                    }
                }
                //  склеиваем если функция
                //Log.d(TAG, "± 1...Separated TEXT - %@" + arr);

                for (int i = 0; i < arr.Count; i++)
                {
                    if (arr[i].ToString().Length > 4)
                    {
                        String tmp = arr[i].ToString().Substring(0, 5).Trim();
                        if (tmp.Equals("instr"))
                        {
                            arr[i] = arr[i].ToString() + "," + arr[i + 1].ToString();
                            arr.RemoveAt(i + 1);
                        }
                    }
                }
                List<String> arrStr = new List<String>();
                arrStr = stringAndDigitsSeparateToArray(string_val);
                List<String> arrValue = new List<String>(arrStr);
                if (string_val.Substring(0, 1).Equals("-"))
                {
                    arrValue.RemoveAt(0);
                    arrValue[0] = "-" + arrValue[0].ToString();
                }
                int index = 0;
                List<String> arrSign = new List<String>();
                for (int i = 0; i < arr.Count - 1; i++)
                {
                    try
                    {
                        index = Int32.Parse(arr[1].ToString());
                    }
                    catch //(NumberFormatException e)
                    {
                        //Log.d(TAG, "± Wrong number format in left$!");
                    }
                    index = index + arr[i].ToString().Length + 1;
                    NSRange range = new NSRange(index - 1, 1);
                    arrSign.Add(string_val.Substring(range.location, range.length));
                }
                bool adding = NO;
                int indAdd = 0;
                List<String> arrValueDelete = new List<String>();
                if (arrValue.Count > 2) for (int i = 0; i < arr.Count - 1; i++)
                    {
                        String notext = removeText(arr[i].ToString());
                        if (digitalFunc.mathFunction(notext))
                        {
                            adding = YES;
                            indAdd = i;
                        }
                        if (adding)
                        {
                            String addString = arr[indAdd].ToString() + arrSign[i].ToString() + arr[i + 1].ToString();
                            arrValueDelete.Add(arr[i + 1].ToString());
                            arr[indAdd] = addString;
                        }
                        if (arr[i + 1].ToString().Contains(")"))
                        {
                            adding = NO;
                        }
                    }
                for (int ii = 0; ii < arrValueDelete.Count; ii++) arr.Remove(arrValueDelete[ii]);
                arrValueDelete.Clear();
                adding = NO;
                indAdd = 0;
                for (int ii = 0; ii < arrValueDelete.Count; ii++)
                    //Log.d(TAG, "± ARR --- " + arr[ii).ToString());
                if (arrValue.Count > 1) for (int i = 0; i < arr.Count - 1; i++)
                    {
                        String notext = removeText(arr[i].ToString());
                        StringFunc stringFunc = new StringFunc();
                        if (stringFunc.stringFunction(notext))
                        {
                            adding = YES;
                            indAdd = i;
                            //Log.d(TAG, "± adding " + notext);
                        }
                        if (adding)
                        {
                            String addString = arr[indAdd].ToString() + arrSign[i].ToString() + arr[i + 1].ToString();
                            arrValueDelete.Add(arr[i + 1].ToString());
                            arr[indAdd] = addString;
                        }
                        if (arr[i + 1].ToString().Contains(")"))
                        {
                            adding = NO;
                        }
                    }
                for (int ii = 0; ii < arrValueDelete.Count; ii++) arr.Remove(arrValueDelete[ii]);
                for (int i = 0; i < arr.Count; i++) arr[i] = arr[i].ToString().Trim();
            }
            if (foundFirst)
            {
                GlobalVars.getInstance().error = "Syntax error\n";
            }
            else {
                for (int i = 0; i < arr.Count; i++)
                {
                    //Log.d(TAG, "± Separated TEXT->'" + arr[i].ToString() + "'");
                }
            }
            return arr;
        }

    }
}
