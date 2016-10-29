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
            int result = GlobalVars.getInstance().Variables.Count;
            if (result > 0)
            {
                for (int i = 0; i < GlobalVars.getInstance().Variables.Count; i++)
                {
                    VariableSet varSet = GlobalVars.getInstance().Variables[i];
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
            if (GlobalVars.getInstance().Variables.Count > 0 || variableIsPresent(name))
            {
                for (int i = 0; i < GlobalVars.getInstance().Variables.Count; i++)
                {
                    VariableSet varSet = GlobalVars.getInstance().Variables[i];
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
            if (GlobalVars.getInstance().Variables.Count > 0)
            {
                for (int i = 0; i < GlobalVars.getInstance().Variables.Count; i++)
                {
                    VariableSet varSet = GlobalVars.getInstance().Variables[i];
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
            if (GlobalVars.getInstance().Variables.Count > 0 || variableIsPresent(name))
            {
                for (int i = 0; i < GlobalVars.getInstance().Variables.Count; i++)
                {
                    VariableSet varSet = GlobalVars.getInstance().Variables[i];
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
            for (int i = 0; i < GlobalVars.getInstance().Array.Count; i++)
            {
                array = (ArraySet)GlobalVars.getInstance().Array[i];
                String str = array.name;
                if (str.Equals(name))
                {
                    GlobalVars.getInstance().Array.RemoveAt(i);
                }
            }
            array = new ArraySet();
            for (int i = 0; i <= size; i++)
            {
                array.value.Add("");
            }
            array.name = name;
            array.size = size;
            GlobalVars.getInstance().Array.Add(array);
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
                        for (int i = 0; i < GlobalVars.getInstance().Array.Count; i++)
                        {
                            array = (ArraySet)GlobalVars.getInstance().Array[i];
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
            if (GlobalVars.getInstance().Variables.Count > 0 || variableIsPresent(name))
            {
                for (int i = 0; i < GlobalVars.getInstance().Variables.Count; i++)
                {
                    VariableSet varSet = (VariableSet)GlobalVars.getInstance().Variables[i];
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
            for (int i = 0; i < GlobalVars.getInstance().ListOfAll.Count; i++)
            {
                int index = string_var.ToLower().IndexOf(GlobalVars.getInstance().ListOfAll[i].ToString());
                if (index != NSNotFound && index == 0)
                {
                    result = YES;
                    Debug.WriteLine(TAG + "± forbidden var found!! " + string_var);
                }
            }
            if (result)
            {
                GlobalVars.getInstance().Error = "Vrong variable name" + Environment.NewLine;
            }
            return result;
        }

        public VariableSet addDateToVariable(String variable)
        {
            VariableSet result = new VariableSet();

            // culture
            var usCulture = new CultureInfo("en-US");
            // Get current UTC time.   
            var curDate = DateTime.Now;
            // Output the date in our specified format using the US-culture. 
            var str = curDate.ToString("dd.MM.yyyy", usCulture);

            result.var = str.ToString();

            result.name = variable;
            result.stringType = YES;
            Debug.WriteLine(TAG + "± addDateToVariable " + result);
            return result;
        }

        public VariableSet addTimeToVariable(String variable)
        {
            VariableSet result = new VariableSet();
            String format = "HH:mm:ss";

            // culture
            var usCulture = new CultureInfo("en-US");
            // Get current UTC time.   
            var curDate = DateTime.Now;
            // Output the date in our specified format using the US-culture. 
            var str = curDate.ToString(format, usCulture);

            result.var = str.ToString();
            result.name = variable;
            result.stringType = YES;
            Debug.WriteLine(TAG + "± addTimeToVariable " + result);
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
                    GlobalVars.getInstance().Error = "Miss \" in string_var variable" + Environment.NewLine;
                }
            }
            int l = 0;
            if (afterEqual.Length > 3) l = 4;
            if (!result.Contains("$") && afterEqual.Contains("\"") && !afterEqual.Substring(0, l).Equals("=asc")
                    && !afterEqual.Substring(0, l).Equals("=ins") && !afterEqual.Substring(0, l).Equals("=val"))
            {
                GlobalVars.getInstance().Error = "Type mismatch" + Environment.NewLine;
            }
            String set = "(?:[^a-zA-Z]+^$)";
            if (Regex.IsMatch(result, set))
            {
                result = "";
                GlobalVars.getInstance().Error = "Variable contains illegal characters" + Environment.NewLine;
            }
            return result;
        }

        public bool getDateToVariable(String string_var)
        {
            bool result = NO;
            String varName = returnVarName(string_var);
            int index = makeVariableIndex(varName);
            if (!forbiddenVariable(varName) && varName.Length > 0 && String.IsNullOrEmpty(GlobalVars.getInstance().Error) && varName.Contains("$"))
            {
                if (index == GlobalVars.getInstance().Variables.Count)
                {
                    GlobalVars.getInstance().Variables.Add(addDateToVariable(varName));
                }
                else {
                    GlobalVars.getInstance().Variables[index] = addDateToVariable(varName);
                }
                result = YES;
            }
            else {
                GlobalVars.getInstance().Command = "";
                GlobalVars.getInstance().Error = "Syntax error" + Environment.NewLine;
                Debug.WriteLine(TAG + "± date let empty, error-" + GlobalVars.getInstance().Error);
                GlobalVars.getInstance().IsOkSet = NO;
            }
            return result;
        }

        public bool getTimeToVariable(String string_var)
        {
            bool result = NO;
            String varName = returnVarName(string_var);
            int index = makeVariableIndex(varName);
            if (!forbiddenVariable(varName) && varName.Length > 0 && String.IsNullOrEmpty(GlobalVars.getInstance().Error) && varName.Contains("$"))
            {
                if (index == GlobalVars.getInstance().Variables.Count)
                {
                    GlobalVars.getInstance().Variables.Add(addTimeToVariable(varName));
                }
                else {
                    GlobalVars.getInstance().Variables[index] = addTimeToVariable(varName);
                }
                result = YES;
            }
            else {
                GlobalVars.getInstance().Command = "";
                GlobalVars.getInstance().Error = "Syntax error" + Environment.NewLine;
                Debug.WriteLine(TAG + "± time let empty, error-" + GlobalVars.getInstance().Error);
                GlobalVars.getInstance().IsOkSet = NO;
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
                        String indexS = string_var.Substring(rangeFirst.location + 1, rangeSecond.length);
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
                                for (int ii = 0; ii < GlobalVars.getInstance().Array.Count; ii++)
                                {
                                    arrayDim = (ArraySet)GlobalVars.getInstance().Array[ii];
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
                                            GlobalVars.getInstance().Error = "Subscript out of range" + Environment.NewLine;
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