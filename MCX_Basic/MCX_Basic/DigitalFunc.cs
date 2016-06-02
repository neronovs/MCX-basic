using System;
using System.Collections.Generic;
using System.Linq;
//using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Reflection;
using org.mariuszgromada.math.mxparser;

namespace MCX_Basic
{
    class DigitalFunc
    {
        private static int NSNotFound = -1;
        private static bool NO = false;
        private static bool YES = true;
        private static String TAG;// = MainActivity.class.getSimpleName();
        NormalizeString normaStr = new NormalizeString();
        Variables variables = new Variables();

        String numberSet = "^-?[0-9]\\d*(\\.\\d+)?$";
        String invNumberSet = "[^0-9.-]+$";
        String alphaSet = "^[A-Za-z]+$";
        String mathSet = "(?:[-+*/^])";
        String numberMathSet = "^[^0-9./^+*-]+$";
        //    NSCharacterSet * numberSet = NSCharacterSet characterSetWithCharactersInString:".0123456789+-/*^" invertedSet;

        public bool isOnlyDigits(String var)
        {
            bool result = YES;
            //if (!var.matches(numberSet)) result = NO;
            if (Regex.IsMatch(var, numberSet, RegexOptions.IgnoreCase) == false)
                result = NO;

            return result;
        }

        public bool isMath(String string_var)
        {
            bool result = NO;
            if (mathFunction(string_var) || isOnlyDigits(string_var) || variables.variableIsDigit(string_var))
                result = YES;
            return result;
        }

        public bool isOnlyDigitsWithMath(String s)
        {
            String nmSet = ".0123456789+-/*^";
            bool containsDigit = YES;
            if (s != null && s != "")
            {
                foreach (char c in s.ToCharArray())
                {
                    if (!nmSet.Contains(c.ToString()))
                    {
                        containsDigit = NO;
                        break;
                    }
                }
            }
            return containsDigit;
        }
        /*
            public bool isOnlyDigitsWithMath(String var)
            {
                bool result = YES;
                if (var.matches(numberMathSet)) result = NO;
                return result;
            }
        */
        public bool mathFunction(String string_var)
        {
            bool result = NO;
            for (int i = 0; i < GlobalVars.getInstance().listMathFunc.Count; i++)
            {
                NSRange range = new NSRange(string_var.ToLower().IndexOf(GlobalVars.getInstance().listMathFunc[i].ToString()), GlobalVars.getInstance().listMathFunc[i].ToString().Length);
                if (range.location != NSNotFound && !normaStr.insideText(string_var.ToLower(), range.location))
                    result = YES;
            }
            return result;
        }

        public double returnMathResult(String str)
        {
            ////Log.d(TAG, "± returnMathResult for " + str);
            double value = 0;
            if (str.Length > 0)
            {
                // подготовка
                //            NSCharacterSet * alphaSet = NSCharacterSet characterSetWithCharactersInString:"abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLKMNOPQRSTUVWXYZ" invertedSet;
                //            NSCharacterSet * numberSet = NSCharacterSet characterSetWithCharactersInString:".0123456789-" invertedSet;
                //            String numberSet="?![.0123456789-]";
                String originalString = str;
                str = str.Replace(" ", "");
                int index = 0;
                // замена переменных
                List<String> arrAll = new List<String>();
                arrAll = normaStr.stringSeparateAllToArray(str);
                for (int i = 0; i < arrAll.Count; i++)
                    if (variables.variableIsPresent(arrAll[i].ToString()))
                        arrAll[i] = variables.returnContainOfVariable(arrAll[i].ToString());
                str = String.Join("", arrAll);
                List<String> arr = new List<String>((str.Split(Convert.ToChar(mathSet)).ToList()));
                List<String> arrValue = new List<String>();
                arrValue = arr;
                if (str.Substring(0, 1).Equals("-"))
                {
                    arrValue.RemoveAt(0);
                    arrValue[0] = "-" + arrValue[0].ToString();
                }
                List<String> arrSign = new List<String>();
                for (int i = 0; i < arrValue.Count - 1; i++)
                {
                    index = index + arrValue[i].ToString().Length + 1;
                    NSRange range = new NSRange(index - 1, 1);
                    arrSign.Add(str.Substring(range.location, range.length));
                }
                ////Log.d(TAG, "± !returnMathResult for " + str);
                for (int i = 0; i < arrValue.Count; i++)
                {
                    if (arrValue[i].ToString().Length > 0)
                        if (arrValue[i].ToString().Substring(arrValue[i].ToString().Length - 1).Equals("("))
                            if (arrSign[i].ToString().Equals("-"))
                            {
                                arrSign.RemoveAt(i);
                                String tmpstr = arrValue[i].ToString() + "-" + arrValue[i + 1].ToString();
                                arrValue.RemoveAt(i);
                                arrValue[i] = tmpstr;
                            }
                }

                ////Log.d(TAG, "± returnMathResult arrValue " + arrValue);

                for (int i = 0; i < arrValue.Count; i++)
                {
                    int l = 0;
                    if (arrValue[i].ToString().Length > 3) l = 4;
                    String funcString = arrValue[i].ToString().Substring(0, l).ToLower();
                    if (funcString.Equals("val("))
                    {
                        if (arrValue[i].ToString().Contains(")"))
                        {
                            String prefix = "val("; // string_var prefix, not needle prefix!
                            String suffix = ")"; // string_var suffix, not needle suffix!
                            NSRange range = new NSRange(prefix.Length, arrValue[i].ToString().Length - prefix.Length - suffix.Length);
                            String numStr = arrValue[i].ToString().Substring(range.location, range.length);
                            numStr = numStr.Replace("\"", "");
                            if (variables.variableIsPresent(numStr))
                                numStr = variables.returnContainOfVariable(numStr);
                            numStr = String.Join("", numStr.Split(Convert.ToChar(invNumberSet)));
                            try
                            {
                                arrValue[i]= Double.Parse(numStr).ToString();
                            }
                            catch //////(NumberFormatException e)
                            {
                                ////Log.d(TAG, "± numStr=" + numStr + "Wrong number format in VAL");
                            }
                        }
                        else {
                            GlobalVars.getInstance().error = "Syntax error at - \n" + arrValue[i].ToString() + "\n";
                        }
                    }
                    else if (funcString.Equals("asc("))
                    {
                        if (arrValue[i].ToString().Contains(")"))
                        {
                            String prefix = "asc(\""; // string_var prefix, not needle prefix!
                            String suffix = "\")"; // string_var suffix, not needle suffix!
                            NSRange range = new NSRange(prefix.Length, arrValue[i].ToString().Length - prefix.Length - suffix.Length);
                            if (!arrValue[i].ToString().Contains("\""))
                            {
                                prefix = "asc(";
                                suffix = ")";
                                range = new NSRange(prefix.Length, arrValue[i].ToString().Length - prefix.Length - suffix.Length);
                                String nameString = arrValue[i].ToString().Substring(range.location, range.length);
                                int index1 = variables.makeVariableIndex(nameString);
                                if (index1 > 0 && nameString != null)
                                {
                                    VariableSet varSet = (VariableSet)GlobalVars.getInstance().variables[index1];
                                    String asciiString = varSet.var.ToString().Substring(0, 1);
                                    arrValue[i] = asciiString;
                                }
                                else {
                                    arrValue[i] = "0";
                                }
                            }
                            else {
                                String asciiString = arrValue[i].ToString().Substring(range.location, range.length);
                                asciiString = ((int)asciiString.ElementAt(0)).ToString();
                                arrValue[i] = asciiString;
                            }
                        }
                        else {
                            GlobalVars.getInstance().error = "Syntax error at - " + arrValue[i].ToString() + "\n";
                        }
                    }
                    else if (funcString.Equals("abs("))
                    {
                        NSRange close = new NSRange(arrValue[i].ToString().IndexOf(")"), 1);
                        if (close.location != NSNotFound)
                        {
                            String prefix = "abs("; // string_var prefix, not needle prefix!
                            String suffix = ")"; // string_var suffix, not needle suffix!
                            NSRange range = new NSRange(prefix.Length, arrValue[i].ToString().Length - prefix.Length - suffix.Length);
                            String numStr = arrValue[i].ToString().Substring(range.location, range.length);
                            double pre = 0;
                            try
                            {
                                pre = Math.Abs(Double.Parse(numStr));
                            }
                            catch //////(NumberFormatException e)
                            {
                                ////Log.d(TAG, "± str=" + str + "Wrong number format in ABS");
                            }
                            arrValue[i] = pre.ToString();
                        }
                        else {
                            GlobalVars.getInstance().error = "Syntax error at - " + arrValue[i].ToString() + "\n";
                        }
                    }
                    else if (funcString.Equals("fix("))
                    {
                        NSRange close = new NSRange(arrValue[i].ToString().IndexOf(")"), 1);
                        if (close.location != NSNotFound)
                        {
                            String prefix = "fix("; // string_var prefix, not needle prefix!
                            String suffix = ")"; // string_var suffix, not needle suffix!
                            NSRange range = new NSRange(prefix.Length, arrValue[i].ToString().Length - prefix.Length - suffix.Length);
                            String numStr = arrValue[i].ToString().Substring(range.location, range.length);
                            try
                            {
                                arrValue[i] = (Math.Round(Double.Parse(numStr))).ToString();
                            }
                            catch ////(NumberFormatException e)
                            {
                                ////Log.d(TAG, "± numStr=" + numStr + "Wrong number format in VAL");
                            }
                        }
                        else {
                            GlobalVars.getInstance().error = "Syntax error at - " + arrValue[i].ToString() + "\n";
                        }
                    }
                    else if (funcString.Equals("rnd("))
                    {
                        NSRange close = new NSRange(arrValue[i].ToString().IndexOf(")"), 1);
                        if (close.location != NSNotFound)
                        {
                            String prefix = "rnd("; // string_var prefix, not needle prefix!
                            String suffix = ")"; // string_var suffix, not needle suffix!
                            NSRange range = new NSRange(prefix.Length, arrValue[i].ToString().Length - prefix.Length - suffix.Length);
                            String numStr = arrValue[i].ToString().Substring(range.location, range.length);
                            try
                            {
                                Random r = new Random();
                                int rint = r.Next(Int32.Parse(numStr));
                                arrValue[i] = rint.ToString();
                            }
                            catch ////(NumberFormatException e)
                            {
                                ////Log.d(TAG, "± numStr=" + numStr + "Wrong number format in VAL");
                            }
                        }
                        else {
                            GlobalVars.getInstance().error = "Syntax error at - " + arrValue[i].ToString() + "\n";
                        }
                    }
                    else if (funcString.Equals("inst"))
                    {
                        if (arrValue[i].ToString().Contains(")"))
                        {
                            String prefix = "instr("; // string_var prefix, not needle prefix!
                            String suffix = ")"; // string_var suffix, not needle suffix!
                            NSRange range = new NSRange(prefix.Length, originalString.Length - prefix.Length - suffix.Length);
                            String normStr = originalString.Substring(range.location, range.length);
                            normStr = normStr.Replace("+", ",");
                            String varNm = normStr.Replace(" ", "").Split(',')[0];
                            if (variables.variableIsPresent(varNm))
                            {
                                normStr = normStr.Replace(varNm, "\"" + variables.returnContainOfVariable(varNm) + "\"");
                            }
                            varNm = normStr.Replace(" ", "").Split(',')[1];
                            if (variables.variableIsPresent(varNm))
                            {
                                normStr = normStr.Replace(varNm, "\"" + variables.returnContainOfVariable(varNm) + "\"");
                            }
                            int indLoc = 0;
                            List<String> arr1 = new List<String>();
                            arr1 = normaStr.extractTextToArray(normStr);
                            if (GlobalVars.getInstance().error == null) indLoc = arr1[0].ToString().IndexOf(arr1[1].ToString());
                            if (range.location == NSNotFound)
                            {
                                ////Log.d(TAG, "± string_var was not found");
                                arrValue[i] = "0";
                            }
                            else {
                                arrValue[i] = (indLoc + 1).ToString();
                            }
                        }
                        else {
                            GlobalVars.getInstance().error = "Syntax error at - " + arrValue[i].ToString() + "\n";
                        }
                    }
                    else if (funcString.Equals("len("))
                    {
                        if (arrValue[i].ToString().Contains(")"))
                        {
                            String prefix = "len("; // string_var prefix, not needle prefix!
                            String suffix = ")"; // string_var suffix, not needle suffix!
                            NSRange range = new NSRange(prefix.Length, originalString.Length - prefix.Length - suffix.Length);
                            String normStr = originalString.Substring(range.location, range.length);
                            normStr = normStr.Replace("+", ",");
                            String varNm = normStr.Replace(" ", "").Split(',')[0];
                            if (variables.variableIsPresent(varNm))
                            {
                                normStr = normStr.Replace(varNm, variables.returnContainOfVariable(varNm));
                            }
                            normStr = normStr.Replace("\"", "");
                            if (GlobalVars.getInstance().error.Length != 0)
                            {
                                ////Log.d(TAG, "± string_var was not found");
                                arrValue[i] = "0";
                            }
                            else {
                                arrValue[i] = (normStr.Length).ToString();
                            }
                        }
                        else {
                            GlobalVars.getInstance().error = "Syntax error at - " + arrValue[i].ToString() + "\n";
                        }
                    }
                    else if (arrValue[i].ToString().Contains(alphaSet))
                    {
                        index = variables.makeVariableIndex(arrValue[i].ToString());
                        if (variables.variableIsPresent(arrValue[i].ToString()))
                        {
                            VariableSet varSet = GlobalVars.getInstance().variables[index];
                            arrValue[i] = varSet.var;
                        }
                        else {
                            ////Log.d(TAG, "± !!!Variable not excist");
                            //GlobalVars.getInstance().error = "Syntax error\n";
                            arrValue[i] = "0";
                        }
                    }

                }
                str = arrValue[0].ToString();
                for (int i = 1; i < arrValue.Count; i++)
                    str = str + arrSign[i - 1].ToString() + arrValue[i].ToString();

                str = str.Replace("log(", "ln(");
                str = str.Replace("sqr(", "sqrt(");
                str = str.Replace("atn(", "atan(");
                str = str.Replace("cint(", "ceil(");
                str = str.Replace("int(", "floor(");
                /*
                str=str stringByReplacingOccurrencesOfString:"rnd(" withString:"random(0,";
                */
                if (GlobalVars.getInstance().error.Equals(""))
                {
                    //[System.Reflection.Assembly]::LoadFile("d:\\OneDriveNod\\OneDrive\\Coding\\Examples\\MCX basic\\MCX_Basic\\MCX_Basic\\mxparser.dll")
                    //[org.mariuszgromada.math.mxparser.regressiontesting.PerformanceTests]::Start()


                    Expression expression = new Expression(str);
                    if (expression.checkSyntax())
                    {
                        ////Log.d(TAG, "± !!!!!!!!! RESULTING TEST " + expression.getExpressionString() + "=" + expression.calculate());
                        value = expression.calculate();
                    }
                    else {
                        ////Log.d(TAG, "± in expression " + expression.getErrorMessage());
                        GlobalVars.getInstance().error = "Error " + expression.getErrorMessage() + "\n";
                    }
                }
            }
            return value;
        }

        public String mathFunctionInMixedString(String string_val)
        {
            String result = "";
            int index = 0;
            // делаем замену -- и +- исключая текст
            List<String> arrTemp = new List<String>(normaStr.extractTextAndOtherToArray(string_val));
            for (int i = 0; i < arrTemp.Count; i++)
            {
                String stemp = arrTemp[i].ToString();
                if (!normaStr.isText(stemp))
                {
                    stemp = stemp.Replace("\\+-", "-");
                    stemp = stemp.Replace("--", "+");
                }
                arrTemp[i] = stemp;
            }
            string_val = "";
            for (int i = 0; i < arrTemp.Count; i++)
            {
                string_val = string_val + arrTemp[i].ToString(); //склеиваем строку из массива
            }
            List<String> arrValue = new List<String>(normaStr.stringAndDigitsSeparateToArray(string_val));
            if (string_val.Substring(0, 1).Equals("-"))
            {
                arrValue.RemoveAt(0);
                arrValue[0] = "-" + arrValue[0].ToString();
            }
            List<String> arrSign = new List<String>();
            for (int i = 0; i < arrValue.Count - 1; i++)
            {
                index = index + arrValue[i].ToString().Length + 1;
                NSRange range = new NSRange(index - 1, 1);
                arrSign.Add(string_val.Substring(range.location, range.length));
            }
            if (arrSign.Count > 1)
                for (int i = 1; i < arrSign.Count; i++)
                {
                    if (arrSign[i - 1].ToString().Equals(",") && arrSign[i].ToString().Equals("-"))
                    {
                        arrSign.RemoveAt(i);
                        arrValue.RemoveAt(i);
                        arrValue[i] = "-" + arrValue[i].ToString();
                    }
                }
            bool adding = NO;
            int indAdd = 0;
            List<String> arrValueDelete = new List<String>();
            List<String> arrSignDelete = new List<String>();
            if (arrValue.Count > 2) for (int i = 0; i < arrValue.Count - 1; i++)
                {
                    String notext = normaStr.removeText(arrValue[i].ToString());
                    if (mathFunction(notext))
                    {
                        adding = YES;
                        indAdd = i;
                    }
                    if (adding)
                    {
                        String addString = arrValue[indAdd].ToString() + arrSign[i].ToString() + arrValue[i + 1].ToString();
                        arrValueDelete.Add(arrValue[i + 1].ToString());
                        arrSignDelete.Add(arrSign[i].ToString());
                        arrValue[indAdd] = addString;
                    }
                    if (arrValue[i + 1].ToString().Contains(")"))
                    {
                        adding = NO;
                    }
                }
            //deleting array from array
            arrValue = arrValue.Except(arrValueDelete).ToList();
            arrSign = arrSign.Except(arrValueDelete).ToList();
            String str = "";
            String old = "";
            if (arrValue.Count > 1)
            {
                for (int i = 1; i < arrValue.Count(); i++)
                {
                    if (arrValue[i].ToString().Length > 0)
                    {
                        if (isMath(arrValue[i - 1].ToString()) && isMath(arrValue[i].ToString())
                                && !arrSign[i - 1].ToString().Equals(",") && !arrValue[i].ToString().Substring(0, 1).Equals("\""))
                        {
                            String first;
                            if (normaStr.isText(arrValue[i - 1].ToString()))
                            {
                                first = arrValue[i - 1].ToString();
                            }
                            else {
                                first = (returnMathResult(arrValue[i - 1].ToString())).ToString();
                            }
                            if (!String.IsNullOrEmpty(str)) first = "";
                            if (!String.IsNullOrEmpty(old)) result = old;
                            String second;
                            if (normaStr.isText(arrValue[i].ToString()))
                            {
                                second = arrValue[i].ToString();
                            }
                            else {
                                second = (returnMathResult(arrValue[i].ToString())).ToString();
                            }
                            str = str + first + arrSign[i - 1].ToString() + second;
                            str = str.Replace("--", "+");
                        }
                        else {
                            if (!String.IsNullOrEmpty(old)) result = old;
                            if (!String.IsNullOrEmpty(str))
                            {
                                if (!normaStr.isText(str))
                                {
                                    str = (returnMathResult(str)).ToString();
                                }
                            }
                            else {
                                str = arrValue[i - 1].ToString();
                            }
                            old = result + str + arrSign[i - 1].ToString();
                            result = result + str + arrSign[i - 1].ToString() + arrValue[i].ToString();
                            str = "";
                        }
                    }
                }
            }
            else {
                result = string_val;
                if (arrValue.Count > 0)
                    if (isMath(arrValue[0].ToString()) && isMath(arrValue[0].ToString()))
                    {
                        if (normaStr.isText(string_val))
                        {
                            result = string_val;
                        }
                        else {
                            result = (returnMathResult(string_val)).ToString();
                        }
                    }
            }
            if (!str.Equals(""))
            {
                str = (returnMathResult(str)).ToString();
                result = result + str;
                str = "";
            }
            return result;
        }

        /*  Этот метод не используется!!! ОН НЕ НУЖЕН НО ОСТАВЛЕН НА ВСЯКИЙ СЛУЧАЙ
     public String toMathFuncWithPrefix(String string_val, String prefix)
        {
            String suffix = ")"; // string_val suffix, not needle suffix!
            NSRange range = new NSRange(prefix.Length,string_val.Length - prefix.Length - suffix.Length);
            String result = string_val.Substring(range.location,range.location+range.length);
            Log.d(TAG, "± !!!!!returnMathResult string_val="+string_val+"result="+result);
            if (!result.matches(alphaSet)) rangeOfCharacterFromSet:alphaSet.location == NSNotFound) {
            int index = variables makeVariableIndex:result;
            VariableSet* varSet=globals.variables objectAtIndex:index;
            result = varSet.var;
        }
            return result;
        } */
    }
}
