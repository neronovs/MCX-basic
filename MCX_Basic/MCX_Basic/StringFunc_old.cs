using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace MCX_Basic
{
    class StringFunc
    {
        DigitalFunc digitalFunc = new DigitalFunc();
        NormalizeString normaStr = new NormalizeString();
        RunCommand runCommand = new RunCommand();
        Variables variables = new Variables();

        //private static final String TAG = MainActivity.class.getSimpleName();
        private static int NSNotFound = -1;
        private static bool NO = false;
        private static bool YES = true;

    public bool stringFunction(String string_var)
        {
            bool result = NO;
            for (int i = 0; i < GlobalVars.getInstance().listStrFunc.Count(); i++)
            {
                NSRange range = new NSRange(string_var.ToLower().IndexOf(GlobalVars.getInstance().listStrFunc[i].ToString()), GlobalVars.getInstance().listStrFunc[i].ToString().Length);
                if (range.location != NSNotFound && !normaStr.insideText(string_var.ToLower(), range.location))
                    result = YES;
            }
            return result;
        }

        
        public List<string_var> returnCaseInsensFromString(String str, String search)
        {
            /*
            NSRange r = new NSRange(0, str.Length);
            for (; ; ) {
                r = new NSRange(str.IndexOf(search), search.Length);
                if (r.location == NSNotFound) {
                    String ss = str.Substring(0, str.Length);
                    result.Add(ss);
                    break;
                } else {
                    String ss = str.Substring(0, r.location);
                    str = str.Substring(r.length + search.Length + 1);
                    result.Add(ss);
                    //////Log.d(TAG, "± ss=" + ss);
                }
                r.location++;
                r.length = str.Length - r.location;
            }
            */
            return new List<string_var>(str.Split(search.ToCharArray()).ToList());
        }

        public String returnAfterStrFuncParse(String string_var)
        {
            String result;
            String tmpStr = string_var;
            for (int t = 0; t < GlobalVars.getInstance().listStrFunc.Count(); t++)
                if (tmpStr.ToLower().Contains(GlobalVars.getInstance().listStrFunc[t].ToString()))
                {
                    List<string_var> arr = new List<string_var>(returnCaseInsensFromString(tmpStr, GlobalVars.getInstance().listStrFunc[t].ToString()));
                    int index = arr[0].ToString().Length + GlobalVars.getInstance().listStrFunc[t].ToString().Length;
                    for (int i = 1; i < arr.Count(); i++)
                    {
                        if (normaStr.insideText(tmpStr, index))
                        {
                            arr[i - 1] = arr[i - 1] + GlobalVars.getInstance().listStrFunc[t].ToString() + arr[i];
                            arr.RemoveAt(i);
                        }
                        if (i < arr.Count())
                            index = index + arr[i].ToString().Length + GlobalVars.getInstance().listStrFunc[t].ToString().Length;
                    }
                    for (int i = 1; i < arr.Count(); i++)
                    {
                        NSRange range = new NSRange(arr[i].ToString().IndexOf(")"), 1);
                        if (range.location != NSNotFound)
                        {
                            String forReplace = arr[i].ToString().Substring(0, range.location + 1);
                            forReplace = GlobalVars.getInstance().listStrFunc[t].ToString() + forReplace;
                            result = GlobalVars.getInstance().listStrFunc[t].ToString() + arr[i].ToString();
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
                    for (int i = 0; i < arr.Count(); i++) tmpStr = tmpStr + arr[i].ToString();
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
                String prefix = "bin$("; // string_var prefix, not needle prefix!
                String suffix = ")"; // string_var suffix, not needle suffix!
                NSRange range = new NSRange(prefix.Length, str.Length - prefix.Length - suffix.Length);
                String tmpStr = str.Substring(range.location, range.location + range.length);
                tmpStr = runCommand.resultFromString(tmpStr);
                if (digitalFunc.isOnlyDigits(tmpStr))
                { // если только цифры
                    if (Convert.ToInt32(tmpStr) >= 0)
                        result = (Convert.ToInt32(tmpStr)).ToString();
                }
                else {  // если переменные
                    int index = variables.makeVariableIndex(tmpStr);
                    VariableSet varSet = GlobalVars.getInstance().variables[index];
                    String stringNumber = varSet.var;
                    if (Convert.ToInt32(stringNumber) >= 0)
                        result = (Convert.ToInt32(stringNumber)).ToString();
                }
            }
            else if (funcString.Equals("chr$"))
            { // возвращает код ASCII символа
                String prefix = "chr$("; // string_var prefix, not needle prefix!
                String suffix = ")"; // string_var suffix, not needle suffix!
                NSRange range = new NSRange(prefix.Length, str.Length - prefix.Length - suffix.Length);
                String tmpStr = str.Substring(range.location, range.location + range.length);
                tmpStr = runCommand.resultFromString(tmpStr);
                if (digitalFunc.isOnlyDigits(tmpStr))
                { // если только цифры
                    if (Convert.ToInt32(tmpStr) > 0 && Convert.ToInt32(tmpStr) < 255)
                        result = ((char)Convert.ToInt32(tmpStr)).ToString();
                }
                else {  // если переменные
                    int index = variables.makeVariableIndex(tmpStr);
                    VariableSet varSet = GlobalVars.getInstance().variables[index];
                    String stringNumber = varSet.var;
                    if (Convert.ToInt32(stringNumber) > 0 && Convert.ToInt32(stringNumber) < 255)
                        result = ((char)Convert.ToInt32(stringNumber)).ToString();
                }
            }
            else if (funcString.Equals("spc$"))
            { //возвращает заданное количество пробелов
                String prefix = "spc$("; // string_var prefix, not needle prefix!
                String suffix = ")"; // string_var suffix, not needle suffix!
                NSRange range = new NSRange(prefix.Length, str.Length - prefix.Length - suffix.Length);
                int spaces = 0;
                String tmpStr = str.Substring(range.location, range.location + range.length);
                tmpStr = runCommand.resultFromString(tmpStr);
                if (digitalFunc.isOnlyDigits(tmpStr))
                { // если только цифры
                    spaces = Convert.ToInt32(tmpStr);
                }
                else {  // если переменные
                    int index = variables.makeVariableIndex(tmpStr);
                    VariableSet varSet = GlobalVars.getInstance().variables[index];
                    tmpStr = varSet.var;
                    spaces = Convert.ToInt32(tmpStr);
                }
                tmpStr = "";
                for (int i = 0; i < spaces; i++) tmpStr = tmpStr + " ";
                result = tmpStr;
            }
            else if (funcString.Equals("str$"))
            { //преобразовывает число в строку
                String prefix = "str$("; // string_var prefix, not needle prefix!
                String suffix = ")"; // string_var suffix, not needle suffix!
                NSRange range = new NSRange(prefix.Length, str.Length - prefix.Length - suffix.Length);
                String tmpStr = str.Substring(range.location, range.location + range.length);
                tmpStr = runCommand.resultFromString(tmpStr);
                if (digitalFunc.isMath(tmpStr))
                {
                    tmpStr = (digitalFunc.returnMathResult(tmpStr)).ToString();
                    //////Log.d(TAG, "± str=" + str + "tmpStr=" + tmpStr);
                }
                else {
                    tmpStr = "";
                }
                result = tmpStr;
            }
            else if (funcString.Equals("stri"))
            { //Возвращает количество заданных кодом ASCII символов
                String prefix = "string_var$("; // string_var prefix, not needle prefix!
                String suffix = ")"; // string_var suffix, not needle suffix!
                if (str.Contains(suffix) && str.Contains(prefix))
                {
                    NSRange range = new NSRange(prefix.Length, str.Length - prefix.Length - suffix.Length);
                    String normStr = str.Substring(range.location, range.location + range.length);
                    normStr = normStr.Replace("\\+", ",");
                    String varNm = normStr.Replace(" ", "").Split(',')[0];
                    if (variables.variableIsPresent(varNm))
                    {
                        normStr = normStr.Replace(varNm, variables.returnContainOfVariable(varNm));
                    }
                    List<string_var> arr = normaStr.extractNumToArray(normStr);
                    String tmpStr = arr[1].ToString();
                    result = "";
                    int arr0 = 0;
                    try
                    {
                        arr0 = Convert.ToInt32(arr[0].ToString());
                    }
                    catch
                    {
                        //////Log.d(TAG, "± str=" + str + "Wrang number format in string_var$!");
                    }
                    for (int i = 0; i < arr0; i++)
                    {
                        result = result + (char)Convert.ToInt32(tmpStr);
                    }
                }
                else {
                    GlobalVars.getInstance().error = "Syntax error at - " + str + "\n";
                }
            }
            else if (funcString.Equals("hex$"))
            { //функция переводит десятичные в шестнадцатиричные
                String prefix = "hex$("; // string_var prefix, not needle prefix!
                String suffix = ")"; // string_var suffix, not needle suffix!
                NSRange range = new NSRange(prefix.Length, str.Length - prefix.Length - suffix.Length);
                String tmpStr = str.Substring(range.location, range.location + range.length);
                tmpStr = runCommand.resultFromString(tmpStr);
                int num = 0;
                if (digitalFunc.isOnlyDigits(tmpStr))
                { // если только цифры
                    num = Convert.ToInt32(tmpStr);
                }
                else {  // если переменные
                    int index = variables.makeVariableIndex(tmpStr);
                    VariableSet varSet = GlobalVars.getInstance().variables[index];
                    try
                    {
                        num = Convert.ToInt32(varSet.var);
                    }
                    catch 
                    {
                        ////Log.d(TAG, "± str=" + str + "Wrong number format in hex$!");
                    }
                }
                result = Convert.ToInt16(num).ToString();
            }
            else if (funcString.Equals("left"))
            { // подстрока из строки x знаков с лева
                String prefix = "left$("; // string_var prefix, not needle prefix!
                String suffix = ")"; // string_var suffix, not needle suffix!
                if (str.Contains(suffix) && str.Contains(prefix))
                {
                    NSRange range = new NSRange(prefix.Length, str.Length - prefix.Length - suffix.Length);
                    String normStr = str.Substring(range.location, range.location + range.length);
                    normStr = normStr.Replace("\\+", ",");
                    String varNm = normStr.Replace(" ", "").Split(',')[0];
                    if (variables.variableIsPresent(varNm))
                    {
                        normStr = normStr.Replace(varNm, "\"" + variables.returnContainOfVariable(varNm) + "\"");
                    }
                    List<string_var> arr = new List<string_var>();
                    arr = normaStr.extractTextAndNumToArray(normStr);
                    if (GlobalVars.getInstance().error == null)
                        range = new NSRange(arr[0].ToString().IndexOf(arr[1].ToString()), arr[1].ToString().Length);
                    int ind = 0;
                    try
                    {
                        ind = Convert.ToInt32(arr[1].ToString());
                    }
                    catch 
                    {
                        ////Log.d(TAG, "± str=" + str + "Wrang number format in left$!");
                    }
                    if (arr[0].ToString().Length < ind || arr[0].ToString().Length < 1)
                    {
                        ////Log.d(TAG, "string_var was not extracted");
                        result = "";
                    }
                    else {
                        result = arr[0].ToString().Substring(0, ind);
                    }
                }
                else {
                    GlobalVars.getInstance().error = "Syntax error at - " + str + "\n";
                }
            }
            else if (funcString.Equals("righ"))
            { // подстрока из строки x знаков с права
                String prefix = "right$("; // string_var prefix, not needle prefix!
                String suffix = ")"; // string_var suffix, not needle suffix!
                if (str.Contains(suffix) && str.Contains(prefix))
                {
                    NSRange range = new NSRange(prefix.Length, str.Length - prefix.Length - suffix.Length);
                    String normStr = str.Substring(range.location, range.location + range.length);
                    normStr = normStr.Replace("\\+", ",");
                    String varNm = normStr.Replace(" ", "").Split(',')[0];
                    if (variables.variableIsPresent(varNm))
                    {
                        normStr = normStr.Replace(varNm, "\"" + variables.returnContainOfVariable(varNm) + "\"");
                    }
                    List<string_var> arr = new List<string_var>();
                    arr = normaStr.extractTextAndNumToArray(normStr);
                    if (GlobalVars.getInstance().error == null)
                        range = new NSRange(arr[0].ToString().IndexOf(arr[1].ToString()), arr[1].ToString().Length);
                    int ind = 0;
                    try
                    {
                        ind = Convert.ToInt32(arr[1].ToString());
                    }
                    catch ////(NumberFormatException e)
                    {
                        ////Log.d(TAG, "± str=" + str + "Wrang number format in left$!");
                    }
                    if (arr[0].ToString().Length < ind || arr[0].ToString().Length < 1)
                    {
                        ////Log.d(TAG, "string_var was not extracted");
                        result = "";
                    }
                    else {
                        result = arr[0].ToString().Substring(arr[0].ToString().Length - ind, arr[0].ToString().Length);
                    }
                }
                else {
                    GlobalVars.getInstance().error = "Syntax error at - " + str + "\n";
                }
            }
            else if (funcString.Equals("mid$"))
            { // подстрока из строки с налальной позиции x и длинной y
                String prefix = "mid$("; // string_var prefix, not needle prefix!
                String suffix = ")"; // string_var suffix, not needle suffix!
                if (str.Contains(suffix) && str.Contains(prefix))
                {
                    NSRange range = new NSRange(prefix.Length, str.Length - prefix.Length - suffix.Length);
                    String normStr = str.Substring(range.location, range.location + range.length);
                    normStr = normStr.Replace("\\+", ",");
                    String varNm = normStr.Replace(" ", "").Split(',')[0];
                    if (variables.variableIsPresent(varNm))
                    {
                        normStr = normStr.Replace(varNm, "\"" + variables.returnContainOfVariable(varNm) + "\"");
                    }
                    List<string_var> arr = new List<string_var>();
                    arr = normaStr.extractTextAndNumToArray(normStr);
                    int ind1 = 0, ind2 = 0;
                    if (GlobalVars.getInstance().error == null)
                    {
                        if (arr.Count() == 3)
                        {
                            try
                            {
                                ind1 = Convert.ToInt32(arr[1].ToString());
                                ind2 = Convert.ToInt32(arr[2].ToString());
                            }
                            catch 
                            {
                                ////Log.d(TAG, "± str=" + str + "Wrang number format in left$!");
                            }
                            range = new NSRange(ind1 - 1, ind2);
                        }
                        if (arr.Count() == 2)
                        {
                            try
                            {
                                ind1 = Convert.ToInt32(arr[1].ToString());
                            }
                            catch 
                            {
                                ////Log.d(TAG, "± str=" + str + "Wrang number format in left$!");
                            }
                            range = new NSRange(ind1 - 1, arr[0].ToString().Length - ind1 + 1);
                        }
                    }
                    if (arr[0].ToString().Length < ind1 || arr[0].ToString().Length < 1)
                    {
                        ////Log.d(TAG, "string_var was not extracted");
                        result = "";
                    }
                    else {
                        result = arr[0].ToString().Substring(range.location, range.location + range.length);
                    }
                }
                else {
                    GlobalVars.getInstance().error = "Syntax error at - " + str + "\n";
                }
            }
            else if (funcString.Equals("oct$"))
            { //функция переводит десятичные в восьмеричные
                String prefix = "oct$("; // string_var prefix, not needle prefix!
                String suffix = ")"; // string_var suffix, not needle suffix!
                NSRange range = new NSRange(prefix.Length, str.Length - prefix.Length - suffix.Length);
                String tmpStr = str.Substring(range.location, range.location + range.length);
                tmpStr = runCommand.resultFromString(tmpStr);
                int num = 0;
                if (digitalFunc.isOnlyDigits(tmpStr))
                { // если только цифры
                    num = Convert.ToInt32(tmpStr);
                }
                else {  // если переменные
                    int index = variables.makeVariableIndex(tmpStr);
                    VariableSet varSet = GlobalVars.getInstance().variables[index];
                    try
                    {
                        num = Convert.ToInt32(varSet.var);
                    }
                    catch ////(NumberFormatException e)
                    {
                        ////Log.d(TAG, "± str=" + str + "Wrong number format in hex$!");
                    }
                }
                result = Convert.ToInt16(num).ToString();
            }
            ////Log.d(TAG, "± STRINGFUNC " + str + "='" + result + "'");
            return result;
        }
        /*
            public String returnStringResult(String str)
            {
                String result=str;
                if (!normaStr.isText(str)){
                str=returnAfterStrFuncParse(str);
                result=str;
                String tempStr="";
                String separator="\"";
                if (str.Contains("\\+") || str.Contains(",")) {
                    List<string_var> arr = new List<string_var>();
                    arr=normaStr. [[NSMutableArray alloc]initWithArray:[normaStr stringSeparateToArray:str]];

                    int index=(int)[[arr objectAtIndex:0] length];
                    for (int i=1; i<[arr count]; i++) {
                        if ([normaStr insideText:str atIndex:index]) {
                            [arr replaceObjectAtIndex:i-1 withObject:[NSString stringWithFormat:@"%@%@%@",[arr objectAtIndex:i-1],
                            [result substringWithRange:NSMakeRange([[arr objectAtIndex:i-1]length], 1)],[arr objectAtIndex:i]]];
                            [arr removeObjectAtIndex:i];
                        }
                        if ([arr count]>1) index=index+(int)[[arr objectAtIndex:i]length]+1;
                    }
                    for (int i=0; i<[arr count]; i++) {
                        if ([[[arr objectAtIndex:i] substringToIndex:1] isEqual:separator] && [[[arr objectAtIndex:i] substringFromIndex:[[arr objectAtIndex:i] length]-1]isEqual:separator]) {
                            tempStr=[NSString stringWithFormat:@"%@%@",tempStr,[[arr objectAtIndex:i] componentsSeparatedByString:separator][1]];
                        } else {
                            if ([digitalFunc isOnlyDigits:[arr objectAtIndex:i]])//если это только цифры ![self typeMismath:[arr objectAtIndex:i] varIsString:NO]
                            {
                                tempStr=[NSString stringWithFormat:@"%@%@",tempStr,[arr objectAtIndex:i]] ;
                            }else{
                                if ([variables variableIsPresent:[arr objectAtIndex:i]]) {
                                    VariableSet* varSet=[globals.variables objectAtIndex:[variables makeVariableIndex:[arr objectAtIndex:i]]];
                                    tempStr=[NSString stringWithFormat:@"%@%@",tempStr,varSet.var];
                                } else {
                                    tempStr=@"";
                                    globals.error=@"Variable not excist\n";
                                    NSLog(@"Variable not excist");
                                }
                            }
                        }
                    }
                    result=tempStr;
                } else {
                    if ([digitalFunc isOnlyDigits:str]) {
                        tempStr=str;
                    }else {
                        if ([[str substringToIndex:1] isEqual:separator] && [str substringFromIndex:[str length]-1]) {
                            tempStr=[NSString stringWithFormat:@"%@%@",tempStr,[str componentsSeparatedByString:separator][1]];
                        } else {
                            if ([variables variableIsPresent:str]){
                                VariableSet* varSet=[globals.variables objectAtIndex: [variables makeVariableIndex:str]];
                                tempStr=varSet.var;
                            }else{
                                tempStr=str;
                            }
                        }
                        result=tempStr;
                    }
                }
            }
                result = [NSString stringWithFormat:@"\"%@\"",result];
                return result;
            }*/
    }
}
