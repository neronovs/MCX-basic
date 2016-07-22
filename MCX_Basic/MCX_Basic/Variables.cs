using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MCX_Basic
{
    public class Variables
    {
        private readonly int NSNotFound = -1;
        private readonly bool NO = false;
        private readonly bool YES = true;
        private readonly String TAG = MethodBase.GetCurrentMethod().DeclaringType.Name + ": ";

        DigitalFunc digitalFunc;
        NormalizeString normaStr = new NormalizeString();
        Variables variables;
        StringFunc stringFunc;

        public int makeVariableIndex(String name)
        {
            int result = GlobalVars.getInstance().variables.Count;
            if (result > 0)
            {
                for (int i = 0; i < GlobalVars.getInstance().variables.Count; i++)
                {
                    VariableSet varSet = GlobalVars.getInstance().variables[i];
                    if (varSet.name.Equals(name))
                    {
                        result = i;
                    }
                }
            }
            return result;
        }

        public bool variableIsDigit(String name)
        {
            bool result = NO;
            if (GlobalVars.getInstance().variables.Count > 0 || variableIsPresent(name))
            {
                for (int i = 0; i < GlobalVars.getInstance().variables.Count; i++)
                {
                    VariableSet varSet = GlobalVars.getInstance().variables[i];
                    if (varSet.name.Equals(name))
                    {
                        if (!varSet.stringType) result = YES;
                    }
                }
            }
            return result;
        }

        public bool variableIsPresent(String name)
        {
            bool result = NO;
            if (GlobalVars.getInstance().variables.Count > 0)
            {
                for (int i = 0; i < GlobalVars.getInstance().variables.Count; i++)
                {
                    VariableSet varSet = GlobalVars.getInstance().variables[i];
                    if (varSet.name.Equals(name))
                    {
                        result = YES;
                    }
                }
            }
            return result;
        }

        public String returnContainOfVariable(String name)
        {
            String result = "";
            if (GlobalVars.getInstance().variables.Count > 0 || variableIsPresent(name))
            {
                for (int i = 0; i < GlobalVars.getInstance().variables.Count; i++)
                {
                    VariableSet varSet = GlobalVars.getInstance().variables[i];
                    if (varSet.name.Equals(name))
                    {
                        result = varSet.var;
                    }
                }
            }
            return result;
        }

        public void initArrayNameWithSize(String name, int size)
        {
            ArraySet array = new ArraySet();
            for (int i = 0; i < GlobalVars.getInstance().array.Count; i++)
            {
                array = (ArraySet)GlobalVars.getInstance().array[i];
                String str = array.name;
                if (str.Equals(name))
                {
                    GlobalVars.getInstance().array.RemoveAt(i);
                }
            }
            array = new ArraySet();
            for (int i = 0; i <= size; i++)
            {
                array.value.Add("");
            }
            array.name = name;
            array.size = size;
            GlobalVars.getInstance().array.Add(array);
        }

        public bool isArrayPresent(String name)
        {
            bool result = NO;
            NSRange rangeFirst = new NSRange(name.IndexOf("("), 1);
            NSRange rangeSecond = new NSRange(name.IndexOf(")"), 1);
            ArraySet array = new ArraySet();
            String string_var = name;
            Debug.WriteLine(TAG + "± ========resultFromString NAME - "+ name);

            if (!normaStr.insideText(name, rangeFirst.location) && !normaStr.insideText(name, rangeSecond.location))
                while (rangeFirst.location != NSNotFound)
                {
                    if (rangeFirst.location != NSNotFound)
                    {
                        name = name.Substring(0, rangeFirst.location);
                        for (int i = 0; i < GlobalVars.getInstance().array.Count; i++)
                        {
                            array = (ArraySet)GlobalVars.getInstance().array[i];
                            String str = array.name;
                            if (str.Equals(name))
                            {
                                result = YES;
                            }
                        }
                    }
                    string_var = string_var.Substring(rangeSecond.location + 1);
                    name = string_var;
                    rangeFirst = new NSRange(name.IndexOf("("), 1);
                    rangeSecond = new NSRange(name.IndexOf(")"), 1);
                }
            Debug.WriteLine(TAG + "± ========resultFromString result - "+ result);
            return result;
        }

        public bool variableIsString(String name)
        {
            bool result = NO;
            if (GlobalVars.getInstance().variables.Count > 0 || variableIsPresent(name))
            {
                for (int i = 0; i < GlobalVars.getInstance().variables.Count; i++)
                {
                    VariableSet varSet = (VariableSet)GlobalVars.getInstance().variables[i];
                    if (varSet.name.Equals(name))
                    {
                        result = varSet.stringType;
                    }
                }
            }
            return result;
        }

        public String returnVarValue(String string_var)
        {
            int index = string_var.IndexOf("=");
            String result = string_var.Substring(index + 1);
            if (string_var.Equals("\"=\"")) result = "\"=\"";
            return result;
        }

        public bool forbiddenVariable(String string_var)
        {
            bool result = NO;
            for (int i = 0; i < GlobalVars.getInstance().listOfAll.Count; i++)
            {
                int index = string_var.ToLower().IndexOf(GlobalVars.getInstance().listOfAll[i].ToString());
                if (index != NSNotFound && index == 0)
                {
                    result = YES;
                    Debug.WriteLine(TAG + "± forbidden var found!! " + string_var);
                }
            }
            if (result)
            {
                GlobalVars.getInstance().error = "Vrong variable name\r\n";
            }
            return result;
        }

        public VariableSet addDateToVariable(String variable)
        {
            //Calendar c;// = Calendar.GlobalVars.getInstance();
            //String format = "yyyy-MM-dd";
            /*
            // US culture
            var usCulture = new CultureInfo("en-US");
            // Get current UTC time.   
            var utcDate = DateTime.UtcNow;
            // Change time to match GMT + 1.
            var gmt1Date = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(utcDate, "W. Europe Standard Time");
            // Output the GMT+1 time in our specified format using the US-culture. 
            var str = gmt1Date.ToString("ddd, dd MMM yyyy HH:mm:ss z", usCulture);
            */
            //SimpleDateFormat sdf = new SimpleDateFormat(format, Locale.US);
            //var sdf = new DateTime();

            VariableSet result = new VariableSet();
            //result.var = sdf.format(c.getTime());
            result.name = variable;
            result.stringType = YES;
            Debug.WriteLine(TAG + "± addDateToVariable " + result);
            return result;
        }

        public VariableSet addTimeToVariable(String variable)
        {
            VariableSet result = new VariableSet();
            /*Calendar c = Calendar.GlobalVars.getInstance();
            String format = "HH:mm:ss";
            SimpleDateFormat sdf = new SimpleDateFormat(format, Locale.US);
            
            result.var = sdf.format(c.getTime());
            result.name = variable;
            result.stringType = YES;
            Debug.WriteLine(TAG + "± addTimeToVariable " + result);*/
            return result;
        }

        public String returnVarName(String string_var)
        {
            String untilEqual = string_var.Split('=')[0];
            int indexforAfterEqual = untilEqual.Length;
            untilEqual = untilEqual.Replace(" ", "");
            String result = untilEqual;
            String afterEqual = string_var.Substring(indexforAfterEqual);
            if (result.Substring(result.Length - 1).Equals("$"))
            {
                if (!normaStr.isPairedQuotes(afterEqual))
                {
                    Debug.WriteLine(TAG + "± Miss \" in string_var variable " + result + afterEqual);
                    GlobalVars.getInstance().error = "Miss \" in string_var variable\r\n";
                }
            }
            int l = 0;
            if (afterEqual.Length > 3) l = 4;
            if (!result.Contains("$") && afterEqual.Contains("\"") && !afterEqual.Substring(0, l).Equals("=asc")
                    && !afterEqual.Substring(0, l).Equals("=ins") && !afterEqual.Substring(0, l).Equals("=val"))
            {
                GlobalVars.getInstance().error = "Type mismatch\r\n";
            }
            String set = "(?:[^a-zA-Z]+$)";
            if (Regex.IsMatch(result, set))
            {
                result = "";
                GlobalVars.getInstance().error = "Variable contains illegal characters\r\n";
            }
            return result;
        }

        public bool getDateToVariable(String string_var)
        {
            bool result = NO;
            String varName = returnVarName(string_var);
            int index = makeVariableIndex(varName);
            if (!forbiddenVariable(varName) && varName.Length > 0 && String.IsNullOrEmpty(GlobalVars.getInstance().error) && varName.Contains("$"))
            {
                if (index == GlobalVars.getInstance().variables.Count)
                {
                    GlobalVars.getInstance().variables.Add(addDateToVariable(varName));
                }
                else {
                    GlobalVars.getInstance().variables[index] = addDateToVariable(varName);
                }
                result = YES;
            }
            else {
                GlobalVars.getInstance().command = "";
                GlobalVars.getInstance().error = "Syntax error\r\n";
                Debug.WriteLine(TAG + "± date let empty, error-" + GlobalVars.getInstance().error);
                GlobalVars.getInstance().isOkSet = NO;
            }
            return result;
        }

        public bool getTimeToVariable(String string_var)
        {
            bool result = NO;
            String varName = returnVarName(string_var);
            int index = makeVariableIndex(varName);
            if (!forbiddenVariable(varName) && varName.Length > 0 && String.IsNullOrEmpty(GlobalVars.getInstance().error) && varName.Contains("$"))
            {
                if (index == GlobalVars.getInstance().variables.Count)
                {
                    GlobalVars.getInstance().variables.Add(addTimeToVariable(varName));
                }
                else {
                    GlobalVars.getInstance().variables[index] = addTimeToVariable(varName);
                }
                result = YES;
            }
            else {
                GlobalVars.getInstance().command = "";
                GlobalVars.getInstance().error = "Syntax error\r\n";
                Debug.WriteLine(TAG + "± time let empty, error-" + GlobalVars.getInstance().error);
                GlobalVars.getInstance().isOkSet = NO;
            }
            return result;
        }

        public String returnContainOfArray(String string_var)
        {
            String totalResult = string_var;
            String stringArr;
            List<String> arr = new List<String>(normaStr.stringAndDigitsSeparateToArray(string_var));
            if (arr.Count > 0) for (int i = 0; i < arr.Count; i++)
                {
                    string_var = arr[i].ToString();
                    if (isArrayPresent(string_var))
                    {
                        string_var = string_var.Replace(" ", "");
                        string_var = string_var.Replace(",", "");
                        string_var = string_var.Replace("+", "");
                        stringArr = string_var;
                        ArraySet arrayDim = new ArraySet();
                        NSRange rangeFirst = new NSRange(string_var.IndexOf("("), 1);
                        NSRange rangeSecond = new NSRange(string_var.IndexOf(")"), 1);
                        String name = string_var.Substring(0, rangeFirst.location);
                        String indexS = string_var.Substring(rangeFirst.location + 1, rangeSecond.location);
                        int index = 0;
                        try
                        {
                            index = Int32.Parse(indexS);
                            if (variableIsPresent(indexS)) index = (int)digitalFunc.returnMathResult(indexS);
                        }
                        catch //(NumberFormatException e)
                        {
                            Debug.WriteLine(TAG + "± indexS=" + indexS + "Wrong number format in returnContainOfArray!");
                        }
                        String result = "";
                        while (rangeFirst.location != NSNotFound)
                        {
                            if (isArrayPresent(string_var))
                            {
                                for (int ii = 0; ii < GlobalVars.getInstance().array.Count; ii++)
                                {
                                    arrayDim = (ArraySet)GlobalVars.getInstance().array[ii];
                                    String str = arrayDim.name;
                                    if (str.Equals(name))
                                    {
                                        if (index < arrayDim.value.Count)
                                        {
                                            result = arrayDim.value[index].ToString();
                                            if (!name.Contains("$") && String.IsNullOrEmpty(result)) result = "0";
                                            if (name.Contains("$") && String.IsNullOrEmpty(result)) result = " ";
                                        }
                                        else {
                                            GlobalVars.getInstance().error = "Subscript out of range\r\n";
                                            result = "error";
                                        }
                                    }
                                }
                            }
                            string_var = string_var.Substring(rangeSecond.location + 1);
                            rangeFirst = new NSRange(name.IndexOf("("), 1);
                            rangeSecond = new NSRange(name.IndexOf(")"), 1);
                            if (string_var.Length > 2)
                            {
                                name = string_var.Substring(0, rangeFirst.location);
                                try
                                {
                                    index = Int32.Parse(string_var.Substring(rangeFirst.location + 1, rangeSecond.location));
                                }
                                catch //(NumberFormatException e)
                                {
                                    Debug.WriteLine(TAG + "± indexS=" + indexS + "Wrong number format in returnContainOfArray!");
                                }
                            }
                        }
                        if (name.Contains("$"))
                        {
                            result = "\"" + result + "\"";
                        }
                        totalResult = totalResult.Replace(stringArr, result);
                    }
                }
            return totalResult;
        }
    }
}