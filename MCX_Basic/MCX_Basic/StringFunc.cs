using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Reflection;

namespace MCX_Basic
{
    public class StringFunc
    {
        private static DigitalFunc digitalFunc = new DigitalFunc();
        private static NormalizeString normaStr = new NormalizeString();
        private static RunCommand runCommand = new RunCommand();
        private static Variables variables = new Variables();

        private readonly int NSNotFound = -1;
        private readonly bool NO = false;
        private readonly bool YES = true;
        private readonly String TAG = MethodBase.GetCurrentMethod().DeclaringType.Name + ": ";

        public StringFunc() { }

        public bool stringFunction(String string_val)
        {
            bool result = NO;
            for (int i = 0; i < GlobalVars.getInstance().ListStrFunc.Count; i++)
            {
                NSRange range = new NSRange(string_val.ToLower().IndexOf(GlobalVars.getInstance().ListStrFunc[i].ToString()), GlobalVars.getInstance().ListStrFunc[i].ToString().Length);
                if (range.location != NSNotFound && !normaStr.insideText(string_val.ToLower(), range.location))
                    result = YES;
            }
            return result;
        }

        public List<String> returnCaseInsensFromString(String str, String search)
        {
            //return new List<String>(str.Split(Convert.ToChar(search))).ToList();
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(search);
            return new List<String>(regex.Split(str));
        }

        public String returnAfterStrFuncParse(String string_val)
        {
            String result;
            String tmpStr = string_val;
            for (int t = 0; t < GlobalVars.getInstance().ListStrFunc.Count; t++)
                if (tmpStr.ToLower().Contains(GlobalVars.getInstance().ListStrFunc[t].ToString()))
                {
                    List<String> arr = new List<String>(returnCaseInsensFromString(tmpStr, GlobalVars.getInstance().ListStrFunc[t].ToString()));
                    int index = arr[0].ToString().Length + GlobalVars.getInstance().ListStrFunc[t].ToString().Length;
                    for (int i = 1; i < arr.Count; i++)
                    {
                        if (normaStr.insideText(tmpStr, index))
                        {
                            arr[i - 1] = arr[i - 1] + GlobalVars.getInstance().ListStrFunc[t].ToString() + arr[i];
                            arr.RemoveAt(i);
                        }
                        if (i < arr.Count)
                            index = index + arr[i].ToString().Length + GlobalVars.getInstance().ListStrFunc[t].ToString().Length;
                    }
                    for (int i = 1; i < arr.Count; i++)
                    {
                        NSRange range = new NSRange(arr[i].ToString().IndexOf(")"), 1);
                        if (range.location != NSNotFound)
                        {
                            String forReplace = arr[i].ToString().Substring(0, range.location + 1);
                            forReplace = GlobalVars.getInstance().ListStrFunc[t].ToString() + forReplace;
                            result = GlobalVars.getInstance().ListStrFunc[t].ToString() + arr[i].ToString();
                            String replacer;
                            if (range.location != NSNotFound)
                            {
                                replacer = "\"" + parseStringFunc(forReplace) + "\"";
                                result = result.Replace(forReplace, replacer);
                                arr[i] = result;
                            }
                            else {
                                arr[i] = "";
                            }
                        }
                    }
                    tmpStr = "";
                    for (int i = 0; i < arr.Count; i++) tmpStr = tmpStr + arr[i].ToString();
                }
            return tmpStr;
        }


        public String parseStringFunc(String str)
        {

            String result = str;
            int l = 0;
            if (str.Length > 3) l = 4;
            String funcString = str.Substring(0, l).ToLower();
            if (funcString.Equals("bin$"))
            { //функция переводит десятичные в двоичные
                String prefix = "bin$("; // string prefix, not needle prefix!
                String suffix = ")"; // string suffix, not needle suffix!
                NSRange range = new NSRange(prefix.Length, str.Length - prefix.Length - suffix.Length);
                String tmpStr = str.Substring(range.location, range.length);
                tmpStr = runCommand.resultFromString(tmpStr);
                if (digitalFunc.isOnlyDigits(tmpStr))
                { // если только цифры
                    if (Int32.Parse(tmpStr) >= 0)
                        result = Convert.ToString(Int32.Parse(tmpStr), 2);
                }
                else {  // если переменные
                    int index = variables.makeVariableIndex(tmpStr);
                    VariableSet varSet = GlobalVars.getInstance().Variables[index];
                    String stringNumber = varSet.var;
                    if (Int32.Parse(stringNumber) >= 0)
                        //result = (Int32.Parse(stringNumber)).ToString();
                        result = Convert.ToString(Int32.Parse(stringNumber), 2);
                }
            }
            else if (funcString.Equals("chr$"))
            { // возвращает код ASCII символа
                String prefix = "chr$("; // string prefix, not needle prefix!
                String suffix = ")"; // string suffix, not needle suffix!
                NSRange range = new NSRange(prefix.Length, str.Length - prefix.Length - suffix.Length);
                String tmpStr = str.Substring(range.location, range.length);
                tmpStr = runCommand.resultFromString(tmpStr);
                if (digitalFunc.isOnlyDigits(tmpStr))
                { // если только цифры
                    if (Int32.Parse(tmpStr) > 0 && Int32.Parse(tmpStr) < 255)
                        result = ((char)Int32.Parse(tmpStr)).ToString();
                }
                else {  // если переменные
                    int index = variables.makeVariableIndex(tmpStr);
                    VariableSet varSet = GlobalVars.getInstance().Variables[index];
                    String stringNumber = varSet.var;
                    if (Int32.Parse(stringNumber) > 0 && Int32.Parse(stringNumber) < 255)
                        result = ((char)Int32.Parse(stringNumber)).ToString();
                }
            }
            else if (funcString.Equals("spc$"))
            { //возвращает заданное количество пробелов
                String prefix = "spc$("; // string prefix, not needle prefix!
                String suffix = ")"; // string suffix, not needle suffix!
                NSRange range = new NSRange(prefix.Length, str.Length - prefix.Length - suffix.Length);
                int spaces = 0;
                String tmpStr = str.Substring(range.location, range.length);
                tmpStr = runCommand.resultFromString(tmpStr);
                if (digitalFunc.isOnlyDigits(tmpStr))
                { // если только цифры
                    spaces = Int32.Parse(tmpStr);
                }
                else {  // если переменные
                    int index = variables.makeVariableIndex(tmpStr);
                    VariableSet varSet = GlobalVars.getInstance().Variables[index];
                    tmpStr = varSet.var;
                    spaces = Int32.Parse(tmpStr);
                }
                tmpStr = "";
                for (int i = 0; i < spaces; i++) tmpStr = tmpStr + " ";
                result = tmpStr;
            }
            else if (funcString.Equals("str$"))
            { //преобразовывает число в строку
                String prefix = "str$("; // string prefix, not needle prefix!
                String suffix = ")"; // string suffix, not needle suffix!
                NSRange range = new NSRange(prefix.Length, str.Length - prefix.Length - suffix.Length);
                String tmpStr = str.Substring(range.location, range.length);
                tmpStr = runCommand.resultFromString(tmpStr);
                if (digitalFunc.isMath(tmpStr))
                {
                    tmpStr = (digitalFunc.returnMathResult(tmpStr)).ToString();
                    Debug.WriteLine(TAG + "± str=" + str + "tmpStr=" + tmpStr);
                }
                else {
                    tmpStr = "";
                }
                result = tmpStr;
            }
            else if (funcString.Equals("stri"))
            { //Возвращает количество заданных кодом ASCII символов
                String prefix = "string$("; // string prefix, not needle prefix!
                String suffix = ")"; // string suffix, not needle suffix!
                if (str.Contains(suffix) && str.Contains(prefix))
                {
                    NSRange range = new NSRange(prefix.Length, str.Length - prefix.Length - suffix.Length);
                    String normStr = str.Substring(range.location, range.length);
                    normStr = normStr.Replace("\\+", ",");
                    String varNm = normStr.Replace(" ", "").Split(',')[0];
                    if (variables.variableIsPresent(varNm))
                    {
                        normStr = normStr.Replace(varNm, variables.returnContainOfVariable(varNm));
                    }
                    List<String> arr = normaStr.extractNumToArray(normStr);
                    String tmpStr = (arr[1]).ToString();
                    result = "";
                    int arr0 = 0;
                    try
                    {
                        arr0 = Int32.Parse(arr[0].ToString());
                    }
                    catch //(NumberFormatException e)
                    {
                        Debug.WriteLine(TAG + "± str=" + str + "Wrong number format in string$!");
                    }
                    for (int i = 0; i < arr0; i++)
                    {
                        result = result + (char)Int32.Parse(tmpStr);
                    }
                }
                else {
                    GlobalVars.getInstance().Error = "Syntax error at - " + str + Environment.NewLine;
                }
            }
            else if (funcString.Equals("hex$"))
            { //функция переводит десятичные в шестнадцатиричные
                String prefix = "hex$("; // string prefix, not needle prefix!
                String suffix = ")"; // string suffix, not needle suffix!
                NSRange range = new NSRange(prefix.Length, str.Length - prefix.Length - suffix.Length);
                String tmpStr = str.Substring(range.location, range.length);
                tmpStr = runCommand.resultFromString(tmpStr);
                int num = 0;
                if (digitalFunc.isOnlyDigits(tmpStr))
                { // если только цифры
                    num = int.Parse(tmpStr);
                }
                else {  // если переменные
                    int index = variables.makeVariableIndex(tmpStr);
                    VariableSet varSet = GlobalVars.getInstance().Variables[index];
                    try
                    {
                        num = int.Parse(varSet.var);
                    }
                    catch //(NumberFormatException e)
                    {
                        Debug.WriteLine(TAG + "± str=" + str + "Wrong number format in hex$!");
                    }
                }
                result = num.ToString("X");
            }
            else if (funcString.Equals("left"))
            { // подстрока из строки x знаков с лева
                String prefix = "left$("; // string prefix, not needle prefix!
                String suffix = ")"; // string suffix, not needle suffix!
                if (str.Contains(suffix) && str.Contains(prefix))
                {
                    NSRange range = new NSRange(prefix.Length, str.Length - prefix.Length - suffix.Length);
                    String normStr = str.Substring(range.location, range.length);
                    normStr = normStr.Replace("\\+", ",");
                    String varNm = normStr.Replace(" ", "").Split(',')[0];
                    if (variables.variableIsPresent(varNm))
                    {
                        normStr = normStr.Replace(varNm, "\"" + variables.returnContainOfVariable(varNm) + "\"");
                    }
                    List<String> arr = new List<String>();
                    arr = normaStr.extractTextAndNumToArray(normStr);
                    if (String.IsNullOrEmpty(GlobalVars.getInstance().Error))
                        range = new NSRange(arr[0].ToString().IndexOf(arr[1].ToString()), arr[1].ToString().Length);
                    int ind = 0;
                    try
                    {
                        ind = Int32.Parse(arr[1].ToString());
                    }
                    catch //(NumberFormatException e)
                    {
                        Debug.WriteLine(TAG + "± str=" + str + "Wrong number format in left$!");
                    }
                    if (arr[0].ToString().Length < ind || arr[0].ToString().Length < 1)
                    {
                        Debug.WriteLine(TAG + "string was not extracted");
                        result = "";
                    }
                    else {
                        result = arr[0].ToString().Substring(0, ind);
                    }
                }
                else {
                    GlobalVars.getInstance().Error = "Syntax error at - " + str + Environment.NewLine;
                }
            }
            else if (funcString.Equals("righ"))
            { // подстрока из строки x знаков с права
                String prefix = "right$("; // string prefix, not needle prefix!
                String suffix = ")"; // string suffix, not needle suffix!
                if (str.Contains(suffix) && str.Contains(prefix))
                {
                    NSRange range = new NSRange(prefix.Length, str.Length - prefix.Length - suffix.Length);
                    String normStr = str.Substring(range.location, range.length);
                    normStr = normStr.Replace("\\+", ",");
                    String varNm = normStr.Replace(" ", "").Split(',')[0];
                    if (variables.variableIsPresent(varNm))
                    {
                        normStr = normStr.Replace(varNm, "\"" + variables.returnContainOfVariable(varNm) + "\"");
                    }
                    List<String> arr = new List<String>();
                    arr = normaStr.extractTextAndNumToArray(normStr);
                    if (String.IsNullOrEmpty(GlobalVars.getInstance().Error))
                        range = new NSRange(arr[0].ToString().IndexOf(arr[1].ToString()), arr[1].ToString().Length);
                    int ind = 0;
                    try
                    {
                        ind = Int32.Parse(arr[1].ToString());
                    }
                    catch //(NumberFormatException e)
                    {
                        Debug.WriteLine(TAG + "± str=" + str + "Wrong number format in left$!");
                    }
                    if (arr[0].ToString().Length < ind || arr[0].ToString().Length < 1)
                    {
                        Debug.WriteLine(TAG + "string was not extracted");
                        result = "";
                    }
                    else {
                        result = arr[0].ToString().Substring(arr[0].ToString().Length - ind);
                    }
                }
                else {
                    GlobalVars.getInstance().Error = "Syntax error at - " + str + Environment.NewLine;
                }
            }
            else if (funcString.Equals("mid$"))
            { // подстрока из строки с налальной позиции x и длинной y
                String prefix = "mid$("; // string prefix, not needle prefix!
                String suffix = ")"; // string suffix, not needle suffix!
                if (str.Contains(suffix) && str.Contains(prefix))
                {
                    NSRange range = new NSRange(prefix.Length, str.Length - prefix.Length - suffix.Length);
                    String normStr = str.Substring(range.location, range.length);
                    normStr = normStr.Replace("\\+", ",");
                    String varNm = normStr.Replace(" ", "").Split(',')[0];
                    if (variables.variableIsPresent(varNm))
                    {
                        normStr = normStr.Replace(varNm, "\"" + variables.returnContainOfVariable(varNm) + "\"");
                    }
                    List<String> arr = new List<String>();
                    arr = normaStr.extractTextAndNumToArray(normStr);
                    int ind1 = 0, ind2 = 0;
                    if (String.IsNullOrEmpty(GlobalVars.getInstance().Error))
                    {
                        if (arr.Count == 3)
                        {
                            try
                            {
                                ind1 = Int32.Parse(arr[1].ToString());
                                ind2 = Int32.Parse(arr[2].ToString());
                            }
                            catch //(NumberFormatException e)
                            {
                                Debug.WriteLine(TAG + "± str=" + str + "Wrong number format in left$!");
                            }
                            range = new NSRange(ind1 - 1, ind2);
                        }
                        if (arr.Count == 2)
                        {
                            try
                            {
                                ind1 = Int32.Parse(arr[1].ToString());
                            }
                            catch //(NumberFormatException e)
                            {
                                Debug.WriteLine(TAG + "± str=" + str + "Wrong number format in left$!");
                            }
                            range = new NSRange(ind1 - 1, arr[0].ToString().Length - ind1 + 1);
                        }
                    }
                    if (arr[0].ToString().Length < ind1 || arr[0].ToString().Length < 1)
                    {
                        Debug.WriteLine(TAG + "string was not extracted");
                        result = "";
                    }
                    else {
                        result = arr[0].ToString().Substring(range.location, range.length);
                    }
                }
                else {
                    GlobalVars.getInstance().Error = "Syntax error at - " + str + Environment.NewLine;
                }
            }
            else if (funcString.Equals("oct$"))
            { //функция переводит десятичные в восьмеричные
                String prefix = "oct$("; // string prefix, not needle prefix!
                String suffix = ")"; // string suffix, not needle suffix!
                NSRange range = new NSRange(prefix.Length, str.Length - prefix.Length - suffix.Length);
                String tmpStr = str.Substring(range.location, range.length);
                tmpStr = runCommand.resultFromString(tmpStr);
                int num = 0;
                if (digitalFunc.isOnlyDigits(tmpStr))
                { // если только цифры
                    num = Int32.Parse(tmpStr);
                }
                else {  // если переменные
                    int index = variables.makeVariableIndex(tmpStr);
                    VariableSet varSet = GlobalVars.getInstance().Variables[index];
                    try
                    {
                        num = Int32.Parse(varSet.var);
                    }
                    catch //(NumberFormatException e)
                    {
                        Debug.WriteLine(TAG + "± str=" + str + "Wrong number format in hex$!");
                    }
                }
                result = Convert.ToString(num, 8);
            }
            Debug.WriteLine(TAG + "± STRINGFUNC " + str + "='" + result + "'");
            return result;
        }

        public String returnStringResult(String str)
        {
            String result = str;
            if (!normaStr.isText(str))
            {
                str = returnAfterStrFuncParse(str);
                result = str;
                String tempStr = "";
                String separator = "\"";
                if (str.Contains("\\+") || str.Contains(","))
                {
                    List<String> arr = new List<String>();
                    arr = normaStr.stringSeparateToArray(str);
                    int index = arr[0].ToString().Length;
                    for (int i = 1; i < arr.Count; i++)
                    {
                        if (normaStr.insideText(str, index))
                        {
                            arr[i - 1] = arr[i - 1].ToString() + result.Substring(arr[i - 1].ToString().Length, arr[i - 1].ToString().Length + 1) + arr[i].ToString();
                            arr.RemoveAt(i);
                        }
                        if (arr.Count > 1) index = index + arr[i].ToString().Length + 1;
                    }
                    for (int i = 0; i < arr.Count; i++)
                    {
                        if (arr[i].ToString().Substring(0, 1).Equals(separator) && arr[i].ToString().Substring(arr[i].ToString().Length - 1).Equals(separator))
                        {
                            tempStr = tempStr + arr[i].ToString().Split(Convert.ToChar(separator))[1];
                        }
                        else {
                            if (digitalFunc.isOnlyDigits(arr[i].ToString())) //если это только цифры !
                            {
                                tempStr = tempStr + arr[i].ToString();
                            }
                            else {
                                if (variables.variableIsPresent(arr[i].ToString()))
                                {
                                    VariableSet varSet = GlobalVars.getInstance().Variables[variables.makeVariableIndex(arr[i].ToString())];
                                    tempStr = tempStr + varSet.var;
                                }
                                else {
                                    tempStr = "";
                                    GlobalVars.getInstance().Error = "Variable not excist" + Environment.NewLine;
                                    Debug.WriteLine(TAG + "± Variable not excist");
                                }
                            }
                        }
                    }
                    result = tempStr;
                }
                else {
                    if (digitalFunc.isOnlyDigits(str))
                    {
                        tempStr = str;
                    }
                    else {
                        if (str.Substring(0, 1).Equals(separator) && str.Substring(str.Length - 1).Equals(separator))
                        {
                            tempStr = tempStr + str.Split(Convert.ToChar(separator))[1];
                        }
                        else {
                            if (variables.variableIsPresent(str))
                            {
                                VariableSet varSet = GlobalVars.getInstance().Variables[variables.makeVariableIndex(str)];
                                tempStr = varSet.var;
                            }
                            else {
                                tempStr = str;
                            }
                        }
                        result = tempStr;
                    }
                }
            }
            result = "\"" + result + "\"";
            return result;
        }
    }
}