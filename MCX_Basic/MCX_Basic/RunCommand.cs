using MCX_Basic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MCX_Basic
{
    public class RunCommand
    {
        private static String TAG;// = MainActivity.class.getSimpleName();
        private static int NSNotFound = -1;
        private static bool NO = false;
        private static bool YES = true;
        private static DigitalFunc digitalFunc = new DigitalFunc();
        private static NormalizeString normaStr = new NormalizeString();
        private static Variables variables = new Variables();
        private static StringFunc stringFunc = new StringFunc();

        public bool set(String string_val)
        {
            Debug.WriteLine("->" + string_val);
            string_val = normaStr.lowcaseWithText(string_val);
            bool result = false;
            if (string_val.Substring(0, 1).Equals(" "))
                string_val = normaStr.removeSpaceInBegin(string_val).ToLower();
            String separator = "\"";
            String base_val = returnBaseCommand(string_val);
            //Log.d(TAG, "± RunCommand..." + string_val);
            GlobalVars.getInstance().Error="";
            GlobalVars.getInstance().listOfStrings.Clear();

            if (base_val.ToLower().Equals("ver"))
            {
                Debug.WriteLine("Ver..." + GlobalVars.version1);
                //Log.d(TAG, "± " + GlobalVars.version1 + BuildConfig.VERSION_NAME + "\n");
                GlobalVars.getInstance().listOfStrings.Add(GlobalVars.version1 + Assembly.GetAssembly(typeof(String)).GetName().Version + "\n");
                GlobalVars.getInstance().listOfStrings.Add(GlobalVars.version2);
                GlobalVars.getInstance().listOfStrings.Add("\n");
                GlobalVars.getInstance().listOfStrings.Add(GlobalVars.version3);
                //foreach (string element in GlobalVars.getInstance().listOfStrings) Debug.WriteLine(element);
                result = true;
            }
            else if (base_val.ToLower().Equals("auto"))
            {
                //Log.d(TAG, "± autoSet = YES");
                GlobalVars.getInstance().AutoSet=(true);
                GlobalVars.getInstance().IsOkSet=(false);
                GlobalVars.getInstance().listOfStrings.Add((GlobalVars.getInstance().ProgramCounter.ToString()) + " ");
                result = true;
            }
            else if (base_val.ToLower().Equals("list"))
            {
                GlobalVars.getInstance().IsOkSet=(true);
                String listNumber;
                String currentLineNumber;
                if (string_val.Length > 4)
                {
                    string_val = string_val.Substring(4, string_val.Length);
                    string_val = string_val.Trim();
                }
                if (!String.IsNullOrEmpty(string_val))
                    if (string_val.Contains("-"))
                    {
                        String[] arr = string_val.Split('-');
                        int begin = Int32.Parse(arr[0]);
                        int end = Int32.Parse(arr[1]);
                        for (int li = begin; li <= end; li++)
                        {
                            listNumber = li.ToString();
                            if (digitalFunc.isOnlyDigits(listNumber))
                            {
                                for (int i = 0; i < GlobalVars.getInstance().ListOfProgram.Count; i++)
                                {
                                    currentLineNumber = GlobalVars.getInstance().ListOfProgram[i].ToString();
                                    currentLineNumber = currentLineNumber.Split(' ')[0];
                                    if (currentLineNumber.ToLower().Equals(listNumber))
                                    {
                                        GlobalVars.getInstance().listOfStrings.Add((GlobalVars.getInstance().ListOfProgram[i].ToString() + "\n").ToString());
                                    }
                                }
                                result = true;
                            }
                            else {
                                GlobalVars.getInstance().error = "Syntax error\n";
                            }
                        }
                    }
                    else {
                        listNumber = string_val;
                        if (digitalFunc.isOnlyDigits(listNumber))
                        {
                            for (int i = 0; i < GlobalVars.getInstance().ListOfProgram.Count; i++)
                            {
                                currentLineNumber = GlobalVars.getInstance().ListOfProgram[i].ToString();
                                currentLineNumber = currentLineNumber.Split(' ')[0];
                                if (currentLineNumber.ToLower().Equals(listNumber))
                                {
                                    GlobalVars.getInstance().listOfStrings.Add((GlobalVars.getInstance().ListOfProgram[i].ToString() + "\n").ToString());
                                }
                            }
                            result = true;
                        }
                        else {
                            GlobalVars.getInstance().error = "Syntax error\n";
                        }
                    }
                if (String.IsNullOrEmpty(string_val))
                    for (int i = 0; i < GlobalVars.getInstance().ListOfProgram.Count; i++)
                    {
                        GlobalVars.getInstance().listOfStrings.Add((GlobalVars.getInstance().ListOfProgram[i].ToString() + "\n").ToString());
                    }
                result = true;
            }
            else if (base_val.ToLower().Equals("help"))
            {
                //Log.d(TAG, "± help");
                for (int i = 0; i < GlobalVars.getInstance().ListOfCommands.Count; i++)
                {
                    GlobalVars.getInstance().listOfStrings.Add((GlobalVars.getInstance().ListOfCommands[i].ToString() + "\n").ToString());
                }
                GlobalVars.getInstance().listOfStrings.Add(("\n").ToString());
                result = true;
            }
            else if (base_val.ToLower().Equals("Clear"))
            {
                GlobalVars.getInstance().variables.Clear();
                result = YES;
            }
            else if (base_val.ToLower().Equals("cls"))
            {
                result = NO;
            }
            else if (base_val.ToLower().Equals("end"))
            {
                //Log.d(TAG, "± end");
                GlobalVars.getInstance().run = false;
                GlobalVars.getInstance().isOkSet = true;
                result = true;
            }
            else if (base_val.ToLower().Equals("beep"))
            {
                //Log.d(TAG, "± Beep begin ...");
                result = NO;
            }
            else if (base_val.ToLower().Equals("color"))
            {
                result = NO;
            }
            else if (base_val.ToLower().Equals("input"))
            {
                GlobalVars.getInstance().input = input(string_val);
                result = NO;
            }
            else if (base_val.ToLower().Equals("data"))
            {
                result = YES;
            }

            #region READ treatment
            else if (base_val.ToLower().Equals("read"))
            {
                if (string_val.Length > base_val.Length + 1)
                {
                    //Log.d(TAG, "± READING ... '" + string + "'");
                    if (GlobalVars.getInstance().dataReadIndex < GlobalVars.getInstance().data.Count)
                    {
                        if (string_val.Contains(","))
                        {
                            String[] arr = string_val.Substring(5).Split(',');
                            for (int i = 0; i < arr.Length; i++)
                            {
                                String stmp = arr[i];
                                if (stmp.Contains(separator))
                                {
                                    GlobalVars.getInstance().error = "Syntax error\n";
                                }
                                else if (digitalFunc.isOnlyDigits(stmp))
                                { // если только цифры
                                    GlobalVars.getInstance().error = "Syntax error\n";
                                }
                                else if (GlobalVars.getInstance().dataReadIndex >= GlobalVars.getInstance().data.Count)
                                { // Out of DATA
                                    GlobalVars.getInstance().error = "Out of DATA\n";
                                }
                                else {
                                    String dataValue = GlobalVars.getInstance().data[GlobalVars.getInstance().dataReadIndex].ToString();
                                    if (variables.variableIsString(stmp)) dataValue = "\"" + dataValue + "\"";
                                    if (variables.isArrayPresent(stmp))
                                    {
                                        setDim(stmp + "=" + dataValue);
                                        GlobalVars.getInstance().dataReadIndex++;
                                    }
                                    else {
                                        int index = variables.makeVariableIndex(stmp);
                                        if (!variables.forbiddenVariable(stmp) && stmp.Length > 0 && String.IsNullOrEmpty(GlobalVars.getInstance().error))
                                        {
                                            if (index == GlobalVars.getInstance().variables.Count())
                                            {
                                                GlobalVars.getInstance().variables.Add(addVariable(variables.returnVarValue(dataValue), stmp));
                                            }
                                            else {
                                                GlobalVars.getInstance().variables[index] = addVariable(variables.returnVarValue(dataValue), stmp);
                                            }
                                        }
                                        GlobalVars.getInstance().dataReadIndex++;
                                    }
                                }
                            }
                        }
                        else {
                            String stmp = string_val.Substring(5);
                            //Log.d(TAG, "± READ to ... variable " + stmp);
                            if (stmp.Contains(separator))
                            {
                                GlobalVars.getInstance().error = "Syntax error\n";
                            }
                            else if (digitalFunc.isOnlyDigits(stmp))
                            { // если только цифры
                                GlobalVars.getInstance().error = "Syntax error\n";
                            }
                            else {
                                String dataValue = GlobalVars.getInstance().data[GlobalVars.getInstance().dataReadIndex].ToString();
                                if (variables.variableIsString(stmp)) dataValue = "\"" + dataValue + "\"";
                                if (variables.isArrayPresent(stmp))
                                {
                                    setDim(stmp + "=" + dataValue);
                                    GlobalVars.getInstance().dataReadIndex++;
                                }
                                else {
                                    int index = variables.makeVariableIndex(stmp);
                                    if (!variables.forbiddenVariable(stmp) && stmp.Length > 0 && String.IsNullOrEmpty(GlobalVars.getInstance().error))
                                    {
                                        if (index == GlobalVars.getInstance().variables.Count())
                                        {
                                            GlobalVars.getInstance().variables.Add(addVariable(variables.returnVarValue(dataValue), stmp));
                                        }
                                        else {
                                            GlobalVars.getInstance().variables[index] = addVariable(variables.returnVarValue(dataValue), stmp);
                                        }
                                    }
                                    GlobalVars.getInstance().dataReadIndex++;
                                }
                            }
                        }
                        if (String.IsNullOrEmpty(GlobalVars.getInstance().error)) result = YES;
                    }
                    else {
                        GlobalVars.getInstance().error = "Out of DATA\n";
                    }
                }
                else {
                    GlobalVars.getInstance().error = "Missing operand\n";
                }
            }
            #endregion 

            else if (base_val.ToLower().Equals("delete"))
            {
                if (string_val.Length > base_val.Length + 1)
                {
                    String listNimber;
                    String currentLineNimber;
                    List<String> toDelete = new List<String>();
                    string_val = (string_val.Substring(7).Replace(" ", ""));
                    if (string_val.Contains("-"))
                    {
                        int begin = 0;
                        int end = 0;
                        String[] arr = string_val.Split('-');
                        try
                        {
                            begin = Int32.Parse(arr[0]);
                            end = Int32.Parse(arr[1]);
                        }
                        catch //(NumberFormatException e)
                        {
                            //Log.d(TAG, "± Wrong number format in delete(RunCommand)!");
                        }
                        for (int li = begin; li <= end; li++)
                        {
                            listNimber = (li).ToString();
                            if (digitalFunc.isOnlyDigits(listNimber))
                            {
                                for (int i = 0; i < GlobalVars.getInstance().listOfProgram.Count; i++)
                                {
                                    currentLineNimber = GlobalVars.getInstance().listOfProgram[i].ToString().Split(' ')[0];
                                    if (currentLineNimber.Equals(listNimber))
                                        toDelete.Add(GlobalVars.getInstance().listOfProgram[i].ToString());
                                }
                                for (int i = 0; i < toDelete.Count; i++)
                                {
                                    GlobalVars.getInstance().listOfProgram.Remove(toDelete[i]);
                                }
                                result = YES;
                            }
                            else {
                                GlobalVars.getInstance().error = "Syntax error\n";
                            }
                        }
                    }
                    else {
                        listNimber = string_val;
                        if (digitalFunc.isOnlyDigits(listNimber))
                        {
                            for (int i = 0; i < GlobalVars.getInstance().listOfProgram.Count; i++)
                            {
                                currentLineNimber = GlobalVars.getInstance().listOfProgram[i].ToString().Split(' ')[0];
                                if (currentLineNimber.Equals(listNimber))
                                {
                                    toDelete.Add(GlobalVars.getInstance().listOfProgram[i]);
                                }
                            }
                            for (int i = 0; i < toDelete.Count; i++)
                            {
                                GlobalVars.getInstance().listOfProgram.Remove(toDelete[i]);
                            }
                            result = YES;
                        }
                        else {
                            GlobalVars.getInstance().error = "Syntax error\n";
                        }
                    }
                }
                else {
                    GlobalVars.getInstance().error = "Missing operand\n";
                }
            }
            else if (base_val.ToLower().Equals("restore"))
            {
                GlobalVars.getInstance().isOkSet = YES;
                String listNumber = string_val.Substring(0, 7).Replace(" ", "");
                listNumber = resultFromString(listNumber);
                if (String.IsNullOrEmpty(listNumber))
                {
                    GlobalVars.getInstance().dataReadIndex = 0;
                    result = YES;
                }
                else {
                    if (digitalFunc.isOnlyDigits(listNumber))
                    {
                        try
                        {
                            GlobalVars.getInstance().dataReadIndex = Int32.Parse(GlobalVars.getInstance().dataIndex[Int32.Parse(listNumber)].ToString());
                        }
                        catch //(NumberFormatException e)
                        {
                            //Log.d(TAG, "± Wrong number format in restore!");
                        }
                        result = YES;
                    }
                    else {
                        GlobalVars.getInstance().error = "Syntax error\n";
                    }
                    //Log.d(TAG, "restore GlobalVars.getInstance().dataReadIndex=" + GlobalVars.getInstance().dataReadIndex);
                }
            }
            else if (base_val.ToLower().Equals("renum"))
            {
                renumGotoGosub();
                result = YES;
            }
            else if (base_val.ToLower().Equals("if"))
            {
                if (string_val.Length > base_val.Length + 1)
                {
                    GlobalVars.getInstance().commandIf = "";
                    // NSCharacterSet * equalSet = [NSCharacterSet characterSetWithCharactersInString:"=<>"];.matches("[a-zA-Z]+")) { //если только буквы
                    String ifValue = normaStr.removeSpaceInBegin(string_val.Substring(3));
                    String thenValue;
                    String elseValue = "";
                    NSRange rangeThen = new NSRange(ifValue.IndexOf("then"), 4);
                    //Log.d(TAG, "± !!! IF operator contain [=<>] "+ifValue.Substring(0, rangeThen.location)+" "+ifValue.Substring(0, rangeThen.location).matches(".*[=<>]+.*"));
                    if (rangeThen.location == NSNotFound || Regex.IsMatch(ifValue.Substring(0, rangeThen.location), (".*[=<>]+.*")) == false)
                    //ifValue.Substring(0, rangeThen.location).matches(".*[=<>]+.*"))
                    {
                        //Log.d(TAG, "± Syntax error in IF operator");
                        GlobalVars.getInstance().error = "Syntax error in IF operator\n";
                    }
                    else {
                        ifValue = normaStr.removeSpaceInBeginAndEnd(ifValue.Substring(0, rangeThen.location));
                        rangeThen = new NSRange(string_val.IndexOf("then"), 4);
                        NSRange rangeElse = new NSRange(string_val.IndexOf("else"), 4);
                        if (rangeElse.location == NSNotFound)
                        {
                            thenValue = normaStr.removeSpaceInBeginAndEnd(string_val.Substring(rangeThen.location + 4));
                        }
                        else {
                            elseValue = normaStr.removeSpaceInBeginAndEnd(string_val.Substring(rangeElse.location + 4));
                            thenValue = normaStr.removeSpaceInBeginAndEnd(string_val.Substring(rangeThen.location + 4, rangeElse.location));
                        }

                        if (ifThen(ifValue))
                        {  //  Вызываем метод проверки IF
                            GlobalVars.getInstance().commandIf = thenValue;
                        }
                        else {
                            GlobalVars.getInstance().commandIf = elseValue;
                        }
                    }
                }
                else {
                    GlobalVars.getInstance().error = "Missing operand\n";
                }
            }

            else if (base_val.ToLower().Equals("for"))
            {
                if (string_val.Length > base_val.Length + 1)
                {
                    ForSet forSet = new ForSet();
                    String forValue = normaStr.removeSpaceInBegin(string_val.Substring(4));
                    String toValue = "";
                    String stepValue = "";
                    NSRange rangeTo = new NSRange(forValue.IndexOf("to"), 2);
                    if (rangeTo.location == NSNotFound)
                    {
                        //Log.d(TAG, "± for without to");
                        GlobalVars.getInstance().error = "Syntax error - FOR without TO\n";
                    }
                    else {
                        forValue = forValue.Substring(0, rangeTo.location).Replace(" ", "");
                    }
                    rangeTo = new NSRange(string_val.IndexOf("to"), 2);
                    NSRange rangeStep = new NSRange(string_val.IndexOf("step"), 4);
                    if (rangeStep.location == NSNotFound)
                    {
                        //Log.d(TAG, "± for without step ");
                        toValue = string_val.Substring(rangeTo.location + 2);
                        toValue = toValue.Replace(" ", "");
                        stepValue = "1";
                    }
                    else {
                        stepValue = string_val.Substring(rangeStep.location + 4).Replace(" ", "");
                        toValue = string_val.Substring(rangeTo.location + 2, rangeStep.location).Replace(" ", "");
                    }
                    //Log.d(TAG, "± !!!! forValue='" + forValue + "' toValue='" + toValue + "' stepValue='" + stepValue + "'");
                    String tmpFor = resultFromString(variables.returnVarValue(forValue));
                    String tmpTo = resultFromString(variables.returnVarValue(toValue));
                    String tmpStep = resultFromString(variables.returnVarValue(stepValue));
                    //Log.d(TAG, "±     forValue=" + forValue + " toValue=" + toValue + " stepValue=" + stepValue);
                    //Log.d(TAG, "± TMP forValue=" + tmpFor + " toValue=" + tmpTo + " stepValue=" + tmpStep);
                    String varName = variables.returnVarName(forValue);
                    forSet.forLine = GlobalVars.getInstance().runnedLine;
                    forSet.forName = varName;
                    forSet.forStep = tmpStep;
                    forSet.forTo = tmpTo;
                    GlobalVars.getInstance().forArray.Add(forSet);
                    int index = variables.makeVariableIndex(varName);
                    String value = tmpFor;
                    if (!variables.forbiddenVariable(varName) && varName.Length > 0 && String.IsNullOrEmpty(GlobalVars.getInstance().error))
                    {
                        if (index == GlobalVars.getInstance().variables.Count)
                        {
                            GlobalVars.getInstance().variables.Add(addVariable(value, varName));
                        }
                        else {
                            GlobalVars.getInstance().variables[index] = addVariable(value, varName);
                        }
                        result = YES;
                    }
                    else {
                        GlobalVars.getInstance().error = "Syntax error";
                        GlobalVars.getInstance().command = "";
                        //Log.d(TAG, "± let empty");
                    }
                    result = YES;
                }
                else {
                    GlobalVars.getInstance().error = "Missing operand\n";
                }
            }
            else if (base_val.ToLower().Equals("next"))
            {
                if (GlobalVars.getInstance().forArray.Count > 0)
                {
                    ForSet forSet = new ForSet();
                    forSet = (ForSet)GlobalVars.getInstance().forArray[GlobalVars.getInstance().forArray.Count - 1];

                    int forI = 0, ff = 0;
                    try
                    {
                        //Log.d(TAG, "± NEXT try int variables.returnContainOfVariable(forSet.forName)=" + variables.returnContainOfVariable(forSet.forName));
                        //Log.d(TAG, "± NEXT try int forSet.forStep=" + forSet.forStep);
                        forI = (int)(float.Parse(variables.returnContainOfVariable(forSet.forName)) + float.Parse(forSet.forStep));
                        ff = (int)float.Parse(forSet.forTo);
                    }
                    catch //(NumberFormatException e)
                    {
                        //Log.d(TAG, "± Wrong number format in NEXT operator!");
                    }
                    if (forI > ff)
                    {
                        GlobalVars.getInstance().forArray.RemoveAt(GlobalVars.getInstance().forArray.Count - 1);
                    }
                    else {
                        GlobalVars.getInstance().variables[variables.makeVariableIndex(forSet.forName)] = addVariable((forI).ToString(), forSet.forName);
                        GlobalVars.getInstance().runIndex = returnIndexFromLine(forSet.forLine) + 1;
                    }
                    result = YES;
                }
                else {
                    GlobalVars.getInstance().error = "NEXT without FOR\n";
                }
            }
            else if (base_val.ToLower().Equals("goto"))
            {
                if (string_val.Length > base_val.Length + 1)
                {
                    string_val = (string_val.Substring(5).Replace(" ", ""));
                    GlobalVars.getInstance().runIndex = returnIndexFromLine(string_val);
                    result = YES;
                    if (GlobalVars.getInstance().runIndex < -1)
                    {
                        GlobalVars.getInstance().runIndex = 0;
                        GlobalVars.getInstance().error = "Undefined line number\n";
                        result = NO;
                    }
                }
                else {
                    GlobalVars.getInstance().error = "Missing operand\n";
                }
            }
            else if (base_val.ToLower().Equals("gosub"))
            {
                if (string_val.Length > base_val.Length + 1)
                {
                    string_val = (string_val.Substring(6).Replace(" ", ""));
                    GlobalVars.getInstance().gosubArray.Add(GlobalVars.getInstance().runnedLine);
                    GlobalVars.getInstance().runIndex = returnIndexFromLine(string_val);
                    result = YES;
                    if (GlobalVars.getInstance().runIndex < -1)
                    {
                        GlobalVars.getInstance().runIndex = 0;
                        GlobalVars.getInstance().error = "Undefined line number\n";
                        result = NO;
                    }
                }
                else {
                    GlobalVars.getInstance().error = "Missing operand\n";
                }
            }
            else if (base_val.ToLower().Equals("return"))
            {
                if (GlobalVars.getInstance().gosubArray.Count > 0)
                {
                    string_val = GlobalVars.getInstance().gosubArray[GlobalVars.getInstance().gosubArray.Count - 1].ToString();
                    GlobalVars.getInstance().gosubArray.RemoveAt(GlobalVars.getInstance().gosubArray.Count - 1);
                    //Log.d(TAG, "± return - " + string_val);
                    GlobalVars.getInstance().runIndex = returnIndexFromLine(string_val) + 1;
                    result = YES;
                }
                else {
                    GlobalVars.getInstance().error = "RETURN without GOSUB\n";
                }
            }
            else if (base_val.ToLower().Equals("rem"))
            {
                result = YES;
            }
            else if (base_val.ToLower().Equals("dim"))
            {
                if (string_val.Length > base_val.Length + 1)
                {
                    String clearString = string_val.Substring(4);
                    result = initDim(clearString);
                }
                else {
                    GlobalVars.getInstance().error = "Missing operand\n";
                }
            }
            else if (base_val.ToLower().Equals("date"))
            {
                if (string_val.Length > base_val.Length + 1)
                {
                    String clearString = string_val.Substring(5);
                    result = variables.getDateToVariable(clearString);
                }
                else {
                    GlobalVars.getInstance().error = "Missing operand\n";
                }
            }
            else if (base_val.ToLower().Equals("time"))
            {
                if (string_val.Length > base_val.Length + 1)
                {
                    String clearString = string_val.Substring(5);
                    result = variables.getTimeToVariable(clearString);
                }
                else {
                    GlobalVars.getInstance().error = "Missing operand\n";
                }
            }
            else if (base_val.ToLower().Equals("print"))
            {
                if (string_val.Length > base_val.Length + 1)
                {
                    result = print(string_val);
                }
                else {
                    GlobalVars.getInstance().error = "Missing operand\n";
                }
            }
            else if (base_val.ToLower().Equals("varl"))
            {
                for (int i = 0; i < GlobalVars.getInstance().variables.Count; i++)
                {
                    VariableSet varSet = GlobalVars.getInstance().variables[i];
                    GlobalVars.getInstance().listOfStrings.Add(varSet.name + " = " + varSet.var + "\n");
                }
                result = YES;
            }
            else if (base_val.ToLower().Equals("csrlin"))
            {
                if (string_val.Length > base_val.Length + 1)
                {
                    GlobalVars.getInstance().listOfStrings.Add(GlobalVars.getInstance().lineNumber + "\n");
                    result = YES;
                }
                else {
                    GlobalVars.getInstance().error = "Missing operand\n";
                }
            }
            else if (base_val.ToLower().Equals("pos"))
            {
                if (string_val.Length > base_val.Length + 1)
                {
                    GlobalVars.getInstance().listOfStrings.Add(GlobalVars.getInstance().command.Length.ToString());
                    result = YES;
                }
                else {
                    GlobalVars.getInstance().error = "Missing operand\n";
                }
            }
            else if (base_val.ToLower().Equals("let"))
            {
                if (string_val.Length > base_val.Length + 1)
                {
                    String clearString = string_val.Substring(4);
                    result = let(clearString);
                }
                else {
                    GlobalVars.getInstance().error = "Missing operand\n";
                }
            }
            else if (base_val.ToLower().Equals("run"))
            {
                /*
                if (string_val.Length > 4) {
                    load(string_val.Substring(4));
                } */
                GlobalVars.getInstance().isOkSet = NO;
                result = NO;
                scanData();
            }
            else if (base_val.ToLower().Equals("files"))
            {
                DirectoryInfo directory = new DirectoryInfo(GlobalVars.getInstance().currentFolder);
                FileInfo[] list = directory.GetFiles(directory.FullName);
                if (list == null)
                    list = new FileInfo[] { };
                List<FileInfo> fileList = list.ToList();
                /*Collections.sort(fileList, new Comparator<File>() {
                @Override
                public int compare(File file, File file2)
        {
            if (file.isDirectory() && file2.isFile())
                return -1;
            else if (file.isFile() && file2.isDirectory())
                return 1;
            else
                return file.getPath().compareTo(file2.getPath());
        }
    });*/
            for (int i = 0; i<fileList.Count; i++)
                if (fileList[i].ToString().Contains(".bas"))
                    GlobalVars.getInstance().listOfStrings.Add(fileList[i].ToString().Substring(fileList[i].ToString().LastIndexOf("/") + 1) + "\n");
            //Log.d(TAG, "± files-"+GlobalVars.getInstance().listOfStrings);
            result = YES;
        }
            else if (base_val.ToLower().Equals("share"))
            {
                /*
                    String fn;
                    if (string_val.Length>base_val.Length+1) {
                        string_val = [string_val.Substring(6];
                        normaStr.= ((NormalizeString alloc]init];
                        string_val = [normaStr.removeSpaceInBegin:string_val];
                        string_val = [string_val.Replace("\"",""];
                        string_val = [string_val.Replace(".bas",""];
                        string_val = [NSString stringWithFormat:".bas",string_val];
                        List<String> paths = NSSearchPathForDirectoriesInDomains(NSDocumentDirectory, NSUserDomainMask, YES);
                        NSString *documentsDirectory = [paths[0]; // Get documents directory
                        NSError *error;
                        String arrayText = GlobalVars.getInstance().listOfProgram componentsJoinedByString: "\n"];
                        fn= documentsDirectory stringByAppendingPathComponent:string_val];
                        bool succeed = [arrayText writeToFile(documentsDirectory stringByAppendingPathComponent:string_val]
                        atomically:YES encoding:NSUTF8StringEncoding error:&error];
                        GlobalVars.getInstance().fileName= documentsDirectory stringByAppendingPathComponent:string_val];
                        if (!succeed){
                            GlobalVars.getInstance().error = "Bad file name";
                            // Handle error here
                        } else {
                            GlobalVars.getInstance().fileName=string_val;
                        }
                        result=NO;
                    } else {
                        GlobalVars.getInstance().error = "Missing operand\n";
                    } */
            }
            else if (base_val.ToLower().Equals("save"))
            {
                if (string_val.Length > base_val.Length + 1)
                {
                    save(string_val.Substring(5));
                    result = YES;
                }
                else {
                    GlobalVars.getInstance().error = "Missing operand\n";
                }
            }
            else if (base_val.ToLower().Equals("csave"))
            {
                //csave:GlobalVars.getInstance().fileName];
                result = YES;
            }
            else if (base_val.ToLower().Equals("new"))
            {
                reset();
                result = NO;
            }
            else if (base_val.ToLower().Equals("reset"))
            {
                reset();
                result = NO;
            }
            else if (base_val.ToLower().Equals("load"))
            {
                result = NO;
            }
            else if (!digitalFunc.isOnlyDigits(base_val) && variables.isArrayPresent(string_val))
            { // let dim array
              //Log.d(TAG, "± let to dim '" + string_val + "'");
                result = setDim(string_val);
            }
            else if (!digitalFunc.isOnlyDigits(base_val) && !String.IsNullOrEmpty(base_val))
            { // let var without LET operator
              //Log.d(TAG, "± let to variable (no let)");
                NSRange range = new NSRange(string_val.IndexOf("="), 1);
                if (range.location != NSNotFound)
                {
                    String afterBase = string_val.Substring(range.location + 1);
                    String before = string_val.Substring(0, range.location);
                    if (GlobalVars.getInstance().keyScan.Length == 0) GlobalVars.getInstance().keyScan = "";
                    if (afterBase.ToLower().Equals("inkey$"))
                    {
                        if (GlobalVars.getInstance().run) GlobalVars.getInstance().scanKeyOn = YES;
                        string_val = before + "=\"" + GlobalVars.getInstance().keyScan + "=\"";
                        GlobalVars.getInstance().keyScan = "";
                    }
                }
                result = let(string_val);
            }
            else if (digitalFunc.isOnlyDigits(base_val) && !base_val.ToLower().Equals(""))
            { // manual program string_val set
              //Log.d(TAG, "± manual program string_val set at line '" + base_val + "'");
                GlobalVars.getInstance().variables.Clear();
                programString(string_val, base_val);
                GlobalVars.getInstance().isOkSet = NO;
            }
            else {
                //Log.d(TAG, "± command error");
                GlobalVars.getInstance().Command="";
                if (!base_val.Equals(""))
                {
                    //Log.d(TAG, "± Syntax error");
                    GlobalVars.getInstance().Error="Syntax error\n";
                }
            }
            //Log.d(TAG, "± runCommand set over!");
            return result;
        }

        public void autoProgramSet(String string_val)
        {
            GlobalVars.getInstance().listOfStrings.Clear();
            GlobalVars.getInstance().listOfProgram.Add(string_val);
            GlobalVars.getInstance().programCounter = GlobalVars.getInstance().programCounter + GlobalVars.getInstance().autoStep;
            GlobalVars.getInstance().listOfStrings.Add(GlobalVars.getInstance().programCounter + " ");
        }

        public void autoProgramStop()
        {
            GlobalVars.getInstance().listOfStrings.Clear();
            GlobalVars.getInstance().listOfStrings.Add(GlobalVars.getInstance().programCounter + " ");
            GlobalVars.getInstance().autoSet = NO;
            //Log.d(TAG, "± autoProgramStop");
        }


        /*
    public bool typeMismath(String var, bool isStr) {
        bool result = NO;
        NSCharacterSet * numberSet = ((NSCharacterSet characterSetWithCharactersInString:".0123456789"] invertedSet];
        String separator="\"";
        if (isStr)
        {
        if (!((var Substring(1].Equals(separator] || !((var.Substring (var length]-1].Equals(separator]) {
        result=YES;
        GlobalVars.getInstance().error="Type mismatch\n";
        }
        }else{
        if  (var rangeOfCharacterFromSet:numberSet].location != NSNotFound) {
        result=YES;
        GlobalVars.getInstance().error="Type mismatch\n";
        }
        }
        return result;
    }
*/


        public bool setDim(String string_val)
        {
            //Log.d(TAG, "± set dim..." + string_val);
            bool result = YES;
            if (string_val.Contains("="))
            {
                ArraySet arrayDim = new ArraySet();
                NSRange rangeFirst = new NSRange(string_val.IndexOf("("), 1);
                NSRange rangeSecond = new NSRange(string_val.IndexOf(")"), 1);
                String name = string_val.Substring(0, rangeFirst.location);
                String indexString = string_val.Substring(rangeFirst.location + 1, rangeSecond.location);
                if (variables.variableIsPresent(indexString))
                    indexString = variables.returnContainOfVariable(indexString);
                int index = 0;
                try
                {
                    index = Int32.Parse(indexString);
                }
                catch //(NumberFormatException e)
                {
                    //Log.d(TAG, "± Wrong number format in setDim!");
                }
                //Log.d(TAG, "± set dim index='" + string_val.Substring(rangeFirst.location + 1, rangeSecond.location) + "' name='" + name + "'");
                rangeFirst = new NSRange(string_val.IndexOf("="), 1);
                String value = string_val.Substring(rangeFirst.location + 1);
                //если переменная строковая добавим кавычки но при этом содержимое не содержит кавычки
                if (name.Contains("$") && !normaStr.isText(value)) value = "\"" + value + "\"";
                value = resultFromString(value); //присваиваем результат рассчетов к массиву
                String testValue = value.Replace(" ", "");
                if (!String.IsNullOrEmpty(testValue))
                {
                    if (name.Contains("$"))
                    {
                        //Log.d(TAG, "± dim variable is string_val " + value);
                        if (value.Equals("")) value = "\"\"";
                        if (normaStr.isText(value))
                        {
                            value = value.Replace("\"", "");
                        }
                        else {
                            value = stringFunc.returnStringResult(value);
                        }
                    }
                    else {
                        //Log.d(TAG, "± dim variable is digits " + value);
                        if (String.IsNullOrEmpty(value) || variables.variableIsString(value))
                        {
                            value = "";
                            GlobalVars.getInstance().error = "Type mismatch\n";
                            result = NO;
                        }
                        value = (digitalFunc.returnMathResult(value)).ToString();
                    }
                    for (int i = 0; i < GlobalVars.getInstance().array.Count; i++)
                    {
                        arrayDim = (ArraySet)GlobalVars.getInstance().array[i];
                        String str = arrayDim.name;
                        if (str.Equals(name))
                        {
                            arrayDim.value[index] = value;
                    GlobalVars.getInstance().array[i] = arrayDim;
                }
                //Log.d(TAG, "± dim name='" + name + "' index='" + index + "' value='" + value + "' error='" + GlobalVars.getInstance().error + "' dim='" + arrayDim.value + "'");
            }
        }
                else {
                    result = NO;
                    GlobalVars.getInstance().error = "Syntax error\n";
                }
            }
            else {
                result = NO;
                GlobalVars.getInstance().error = "Syntax error\n";
            }
            return result;
        }

    public void loadGlobal()
        {
            /*
        NSString *filePath = GlobalVars.getInstance().fileName;
        //    //Log.d(TAG, "± load global for - ",filePath);
        if (filePath) {
        reset];
        NSString *arrayText = [NSString stringWithContentsOfFile:filePath encoding:NSUTF8StringEncoding error:nil];
        if (arrayText) {
        GlobalVars.getInstance().listOfProgram = [NSMutableArray arrayWithArray(arrayText componentsSeparatedByString: "\n"));
        GlobalVars.getInstance().programCounter = ((self returnBaseCommand:GlobalVars.getInstance().listOfProgram lastObject))intValue] + GlobalVars.getInstance().autoStep;
        }
        } else {
        GlobalVars.getInstance().error = "File not found\n";
        }
        */
        }

        public void csave(String string_val)
        {
            /*
        //Log.d(TAG, "± saving-''",string_val);
        NSError *error;
        String arrayText = GlobalVars.getInstance().listOfProgram componentsJoinedByString: "\n"];
        bool succeed = [arrayText writeToFile:string_val atomically:YES encoding:NSUTF8StringEncoding error:&error];
        if (!succeed){
        GlobalVars.getInstance().error = "Bad file name";
        // Handle error here
        }
        */
        }

        public bool kill(String string_val)
        {
            bool result = YES;
            /*
        normaStr.= ((NormalizeString alloc]init];
        string_val = [normaStr.removeSpaceInBegin:string_val];
        string_val = [string_val.Replace("\"",""];
        string_val = [string_val.Replace(".bas",""];
        string_val = [NSString stringWithFormat:".bas",string_val];
        List<String> paths = NSSearchPathForDirectoriesInDomains(NSDocumentDirectory, NSUserDomainMask, YES);
        NSString *documentsDirectory = [paths[0];
        NSString *filePath = [documentsDirectory stringByAppendingPathComponent:string_val];
        //Log.d(TAG, "± kill for - ",filePath);

        NSFileManager *fileManager = [NSFileManager defaultManager];
        NSError *error;
        bool success = [fileManager removeItemAtPath:filePath error:&error];
        if (!success) {
        GlobalVars.getInstance().error = "File not found\n";
        result=NO;
        }
        */
            return result;
        }

        public void scanKeyOff()
        {
            GlobalVars.getInstance().scanKeyOn = NO;
        }

        public void programString(String string_val, String number)
        {
            bool isReplace = NO;
            int indexForReplace = 0;
            int indexForInsert = 0;
            int append = 0;
            GlobalVars.getInstance().listOfStrings.Clear();
            for (int i = 0; i < GlobalVars.getInstance().listOfProgram.Count; i++)
            {
                String currentLineNimber = GlobalVars.getInstance().listOfProgram[i].ToString().Split(' ')[0];
                if (currentLineNimber.Equals(number))
                {
                    isReplace = YES;
                    indexForReplace = i;
                }
                try
                {
                    if (Int32.Parse(currentLineNimber) < Int32.Parse(number))
                    {
                        indexForInsert = i;
                        append = 1;
                    }
                }
                catch //(NumberFormatException e)
                {
                    //Log.d(TAG, "± Wrong number format in programString!");
                }
            }
            if (string_val.Length > number.Length + 1)
            {
                if (isReplace)
                {
                    GlobalVars.getInstance().listOfProgram[indexForReplace] = string_val;
                }
                else {
                    GlobalVars.getInstance().listOfProgram.Insert(indexForInsert + append, string_val);
                }
                GlobalVars.getInstance().programCounter = GlobalVars.getInstance().programCounter + GlobalVars.getInstance().autoStep;
                GlobalVars.getInstance().listOfStrings.Add(GlobalVars.getInstance().programCounter + " ");
            }
            else {
                GlobalVars.getInstance().error = "Undefined line number\n";
            }
        }

        public void reset()
        {
            GlobalVars.getInstance().lineNumber = 0;
            GlobalVars.getInstance().data.Clear();
            GlobalVars.getInstance().array.Clear();
            GlobalVars.getInstance().variables.Clear();
            GlobalVars.getInstance().listOfProgram.Clear();
            GlobalVars.getInstance().listOfStrings.Clear();
            GlobalVars.getInstance().gosubArray.Clear();
            GlobalVars.getInstance().forArray.Clear();

            GlobalVars.getInstance().autoSet = NO;
            GlobalVars.getInstance().autoStep = 10;
            GlobalVars.getInstance().programCounter = 10;
            GlobalVars.getInstance().commandIf = "";
            GlobalVars.getInstance().error = "";
            GlobalVars.getInstance().input = "";
            GlobalVars.getInstance().runnedLine = "";
            GlobalVars.getInstance().dataReadIndex = 0;
            GlobalVars.getInstance().isOkSet = YES;
            GlobalVars.getInstance().run = NO;
            GlobalVars.getInstance().showError = NO;
            GlobalVars.getInstance().scanKeyOn = NO;
        }

        public void save(String string_val)
        {
            //Log.d(TAG, "± saving-" + string_val);
            string_val = normaStr.removeSpaceInBegin(string_val);
            string_val = string_val.Replace("\"", "");
            string_val = string_val.Replace(".bas", "");
            string_val = string_val + ".bas";
            String documentsDirectory = GlobalVars.getInstance().currentFolder; // Get documents directory
            String arrayText = "";
            for (int i = 0; i < GlobalVars.getInstance().listOfProgram.Count; i++)
                arrayText = arrayText + GlobalVars.getInstance().listOfProgram[i].ToString() + "\n";
            try
            {

                DirectoryInfo f = new DirectoryInfo(GlobalVars.getInstance().currentFolder); //Check if folder not excist - make new one
                //if (!f.isDirectory())
                if (!Directory.Exists(GlobalVars.getInstance().currentFolder))
                {
                    //Log.d(TAG, "± Make dir " + GlobalVars.getInstance().currentFolder);
                    f.Create();
                }

                String filepath = Path.Combine(f.ToString(), string_val);  // file path to save
                StreamWriter writer = new StreamWriter(filepath);
                writer = File.AppendText(arrayText);
                writer.Flush();
                writer.Close();
                GlobalVars.getInstance().fileName = documentsDirectory + "/" + string_val;
            }
            catch //(Throwable t)
            {
                //Log.d(TAG, "± Exception: " + t.ToString());
            }
            //Log.d(TAG, "± saved - '" + GlobalVars.getInstance().fileName + "'");
        }

        public void scanData()
        {
            GlobalVars.getInstance().dataReadIndex = 0;
            GlobalVars.getInstance().data.Clear();
            GlobalVars.getInstance().dataIndex.Clear();
            //    //Log.d(TAG, "± SCANNING...");
            String separator = "\"";
            for (int n = 0; n < GlobalVars.getInstance().listOfProgram.Count; n++)
            {
                String currentLine = GlobalVars.getInstance().listOfProgram[n].ToString();
            if (String.IsNullOrEmpty(currentLine))
            {
                GlobalVars.getInstance().listOfProgram.RemoveAt(n);
                //Log.d(TAG, "± REMOVING..." + n + " " + currentLine);
            }
        }
    for (int n = 0; n < GlobalVars.getInstance().listOfProgram.Count; n++)
    {
        String currentLine = GlobalVars.getInstance().listOfProgram[n].ToString();
        //Log.d(TAG, "± SCANNING..." + n + " " + currentLine);
        String untilSpace = currentLine.Split(' ')[0];
        GlobalVars.getInstance().runnedLine = untilSpace;
        int indexforAfterSpace = untilSpace.Length;
        String string_val = currentLine.Substring(indexforAfterSpace + 1);
        String base_val = returnBaseCommand(string_val);
        if (base_val.ToLower().Equals("data"))
        {
            if (string_val.Length > base_val.Length + 1)
            {
                if (string_val.Contains(","))
                {
                    String[] arr = string_val.Substring(5).Split(',');
                    for (int i = 0; i<arr.Length; i++)
                    {
                        String stmp = arr[i];
        GlobalVars.getInstance().dataIndex.Add(untilSpace); //записываем в индекс номер строки data
                        if (stmp.Contains(separator))
                        {
                            stmp = stmp.Split(Convert.ToChar(separator))[1];
                            GlobalVars.getInstance().data.Add(stmp);
                        }
                        else {
                            GlobalVars.getInstance().data.Add(stmp);
                        }
                        //Log.d(TAG, "± SET DATA ''" + stmp);
                    }
                }
                else {
                    String stmp = string_val.Substring(5);
GlobalVars.getInstance().dataIndex.Add(untilSpace); //записываем в индекс номер строки data
                    if (stmp.Contains(separator))
                    {
                        GlobalVars.getInstance().data.Add(stmp.Split(Convert.ToChar(separator))[1]);
                    }
                    else {
                        GlobalVars.getInstance().data.Add(stmp);
                    }
                    //Log.d(TAG, "± SET DATA ''" + stmp);
                }
            }
            else {
                GlobalVars.getInstance().error = "Missing operand\n";
            }
        }
    }
    //Log.d(TAG, "± SCANNING OVER " + GlobalVars.getInstance().dataIndex);
}

        public bool let(String string_val)
        {
            bool result = NO;
            if (string_val.Contains("="))
            {
                String value = resultFromString(variables.returnVarValue(string_val));//присваиваем результат рассчетов к переменной
                String varName = variables.returnVarName(string_val);
                int index = variables.makeVariableIndex(varName);
                if (String.IsNullOrEmpty(GlobalVars.getInstance().error))
                {
                    if (!variables.forbiddenVariable(varName) && varName.Length > 0)
                    {
                        if (index == GlobalVars.getInstance().variables.Count)
                        {
                            GlobalVars.getInstance().variables.Add(addVariable(value, varName));
                            //Log.d(TAG, "± let new var - " + varName + " value=" + value);
                            //Log.d(TAG, "± let new var - " + GlobalVars.getInstance().variables[index).getName().ToString() + " value=" + GlobalVars.getInstance().variables[index).getVar().ToString());
                        }
                        else {
                            GlobalVars.getInstance().variables[index] = addVariable(value, varName);
                            //Log.d(TAG, "± let exist var - " + varName + " value=" + value);
                        }
                        if (String.IsNullOrEmpty(GlobalVars.getInstance().error)) result = YES;
                    }
                    else {
                        GlobalVars.getInstance().command = "";
                        GlobalVars.getInstance().error = "Forbidden variable\n";
                        result = NO;
                        //Log.d(TAG, "± let empty Forbidden variable");
                    }
                }
            }
            else {
                GlobalVars.getInstance().error = "Syntax error\n";
                //Log.d(TAG, "± let Syntax error");
            }
            // for (int i = 0; i < GlobalVars.getInstance().variables.Count; i++)
            //Log.d(TAG, "± let varName - " + GlobalVars.getInstance().variables[i].getName()
            //        + " value=" + GlobalVars.getInstance().variables[i].getVar() + " is String type=" + GlobalVars.getInstance().variables[i].getStringType());
            return result;
        }

        public bool print(String string_val)
        {
            //Log.d(TAG, "± PRINT init ......''" + string_val);
            String cr = "\n";
            if (string_val.Substring(string_val.Length - 1).Equals(";"))
            {
                cr = "";
                string_val = string_val.Substring(0, string_val.Length - 1);
            }
            bool result = NO;
            string_val = string_val.Substring(5);
            String printResult = "";
            string_val = resultFromString(string_val);//присваиваем результат рассчетов
            if (normaStr.isText(string_val))
            {
                string_val = string_val.Replace("\"", "");
            }
            printResult = string_val + cr;
            GlobalVars.getInstance().listOfStrings.Add(printResult);
            if (String.IsNullOrEmpty(GlobalVars.getInstance().error)) result = YES;
            //Log.d(TAG, "± PRINT result '" + printResult + "' error-" + GlobalVars.getInstance().error);
            return result;
        }

        public bool initDim(String string_val)
        {
            bool result = YES;
            //Log.d(TAG, "± initDim... " + string_val);
            List<String> arr = new List<String>((string_val.Split(',')).ToList());
            for (int i = 0; i < arr.Count; i++)
            {
                string_val = arr[i].ToString();
                String name = null;
                String size;
                NSRange rangeFirst = new NSRange(string_val.IndexOf("("), 1);
                if (rangeFirst.location != NSNotFound)
                {
                    NSRange rangeSecond = new NSRange(string_val.IndexOf(")"), 1);
                    name = string_val.Substring(0, rangeFirst.location);
                    size = string_val.Substring(rangeFirst.location + 1, rangeSecond.location);
                    size = resultFromString(size);
                    if (!variables.forbiddenVariable(name) && name.Length > 0 && String.IsNullOrEmpty(GlobalVars.getInstance().error))
                    {
                        int tmp = 0;
                        try
                        {
                            tmp = (int)float.Parse(size);
                        }
                        catch //(NumberFormatException e)
                        {
                            //Log.d(TAG, "± Wrong number format in initDim!");
                        }
                        variables.initArrayNameWithSize(name, tmp);
                        //Log.d(TAG, "± dim name - '" + name + "' size = " + size);
                    }
                    else {
                        GlobalVars.getInstance().command = "";
                        GlobalVars.getInstance().error = "Wrong dim variable name\n";
                        result = NO;
                    }
                }
                else {
                    GlobalVars.getInstance().command = "";
                    GlobalVars.getInstance().error = "Wrong dim variable name\n";
                    result = NO;
                }
                String testValue = name.Replace(" ", "");
                if (testValue.Equals(""))
                {
                    GlobalVars.getInstance().command = "";
                    GlobalVars.getInstance().error = "Wrong dim variable name\n";
                    result = NO;
                }
            }
            //Log.d(TAG, "± initDim result - '" + result+"'");
            return result;
        }

        public int returnIndexFromLine(String line)
        {
            int result = -2;
            for (int i = 0; i < GlobalVars.getInstance().listOfProgram.Count; i++)
            {
                String currentLine = GlobalVars.getInstance().listOfProgram[i].ToString().Split(' ')[0];
                if (currentLine.Equals(line))
                {
                    result = i - 1;
                    //Log.d(TAG, "± returnIndexFromLine currentLine=" + currentLine + "   String line=" + line + " result=" + result);
                }
            }
            //Log.d(TAG, "± returnIndexFromLine - " + result);
            return result;
        }

        public VariableSet addVariable(String var, String name)
        {
            bool strType = NO;
            //    //Log.d(TAG, "± addVariable+ is string_val ",var);
            var = normaStr.removeSpaceInBeginAndEnd(var);
            if (var.Equals("+")) var = "\"+\"";
            String value = var;
            if (name.Contains("$"))
            {
                //Log.d(TAG, "± addVariable+variable is string_val " + var);
                if (String.IsNullOrEmpty(var)) var = "\"\"";
                strType = YES;
                if (normaStr.isText(value))
                {
                    value = value.Replace("\"", "");
                }
                else {
                    value = stringFunc.returnStringResult(var);
                }
            }
            else {
                //Log.d(TAG, "± variable is digits " + var);
                if (String.IsNullOrEmpty(var) || variables.variableIsString(var))
                {
                    value = "0";
                    GlobalVars.getInstance().error = "Type mismatch\n";
                }
                value = (digitalFunc.returnMathResult(value)).ToString();
            }
            VariableSet result = new VariableSet();
            result.var = value;
            result.name = name;
            result.stringType = strType;
            return result;
        }

        public bool isStringValue(String string_val)
        {
            bool result = YES;
            bool strings = NO;
            bool stringsVariable = NO;
            bool digits = NO;
            bool digitsVariable = NO;
            if (variables.variableIsString(string_val)) stringsVariable = YES;
            if (variables.variableIsDigit(string_val)) digitsVariable = YES;
            if (digitalFunc.isOnlyDigitsWithMath(string_val)) digits = YES;
            if (string_val.Length > 0)
                if (string_val.Substring(0, 1).Equals("\"") && string_val.Substring(string_val.Length - 1).Equals("\""))
                    strings = YES;
            if ((!strings && !stringsVariable && digits && digitsVariable)
                    || (!strings && !stringsVariable && !digits && digitsVariable)
                    || (!strings && !stringsVariable && digits && !digitsVariable))
            { //if variable and value is only digits
                result = NO;
            }
            return result;
        }

        public String checkResult(String value)
        {
            return resultFromString(value);//присваиваем результат рассчетов
        }

        public bool ifThen(String ifValue)
        {
            bool result = true;

            String ifFirst;
            String ifEqual;
            String ifSecond;
            NSRange rangeThen;

            //Log.d(TAG, "± IF operator '=' "+ifValue);

            if (ifValue.Contains("=>"))
            {
                ifEqual = "=>";
                rangeThen = new NSRange(ifValue.IndexOf(ifEqual), ifEqual.Length);
                ifFirst = ifValue.Substring(0, rangeThen.location);
                ifSecond = ifValue.Substring(rangeThen.location + ifEqual.Length);
                ifFirst = checkResult(ifFirst);
                ifSecond = checkResult(ifSecond);
                if (!isStringValue(ifFirst) && !isStringValue(ifSecond))
                {
                    if (variables.variableIsPresent(ifFirst))
                        ifFirst = variables.returnContainOfVariable(ifFirst);
                    if (variables.variableIsPresent(ifSecond))
                        ifSecond = variables.returnContainOfVariable(ifSecond);
                    if (float.Parse(ifFirst) >= float.Parse(ifSecond))
                    {
                        result = YES;
                    }
                    else {
                        result = NO;
                    }
                }
                else {
                    //Log.d(TAG, "± Syntax error in IF operator");
                    GlobalVars.getInstance().error = "Type mismatch in IF operator\n";
                }
            }
            else if (ifValue.Contains("<>"))
            {
                ifEqual = "<>";
                rangeThen = new NSRange(ifValue.IndexOf(ifEqual), ifEqual.Length);
                ifFirst = ifValue.Substring(0, rangeThen.location);
                ifSecond = ifValue.Substring(rangeThen.location + ifEqual.Length);
                ifFirst = checkResult(ifFirst);
                ifSecond = checkResult(ifSecond);
                if (!isStringValue(ifFirst) && !isStringValue(ifSecond))
                {
                    if (variables.variableIsPresent(ifFirst))
                        ifFirst = variables.returnContainOfVariable(ifFirst);
                    if (variables.variableIsPresent(ifSecond))
                        ifSecond = variables.returnContainOfVariable(ifSecond);
                    if (float.Parse(ifFirst) != float.Parse(ifSecond))
                    {
                        result = YES;
                    }
                    else {
                        result = NO;
                    }
                }
                else {
                    ifFirst = normaStr.removeSpacesWithText(ifFirst);
                    ifSecond = normaStr.removeSpacesWithText(ifSecond);
                    if (variables.variableIsPresent(ifFirst))
                        ifFirst = "\"" + variables.returnContainOfVariable(ifFirst) + "\"";
                    if (variables.variableIsPresent(ifSecond))
                        ifSecond = "\"" + variables.returnContainOfVariable(ifSecond) + "\"";
                    if (!ifFirst.Equals(ifSecond))
                    {
                        result = YES;
                    }
                    else {
                        result = NO;
                    }
                }
            }
            else if (ifValue.Contains("><"))
            {
                ifEqual = "><";
                rangeThen = new NSRange(ifValue.IndexOf(ifEqual), ifEqual.Length);
                ifFirst = ifValue.Substring(0, rangeThen.location);
                ifSecond = ifValue.Substring(rangeThen.location + ifEqual.Length);
                ifFirst = checkResult(ifFirst);
                ifSecond = checkResult(ifSecond);
                if (!isStringValue(ifFirst) && !isStringValue(ifSecond))
                {
                    if (variables.variableIsPresent(ifFirst))
                        ifFirst = variables.returnContainOfVariable(ifFirst);
                    if (variables.variableIsPresent(ifSecond))
                        ifSecond = variables.returnContainOfVariable(ifSecond);
                    if (float.Parse(ifFirst) != float.Parse(ifSecond))
                    {
                        result = YES;
                    }
                    else {
                        result = NO;
                    }
                }
                else {
                    ifFirst = normaStr.removeSpacesWithText(ifFirst);
                    ifSecond = normaStr.removeSpacesWithText(ifSecond);
                    if (variables.variableIsPresent(ifFirst))
                        ifFirst = "\"" + variables.returnContainOfVariable(ifFirst) + "\"";
                    if (variables.variableIsPresent(ifSecond))
                        ifSecond = "\"" + variables.returnContainOfVariable(ifSecond) + "\"";
                    if (!ifFirst.Equals(ifSecond))
                    {
                        result = YES;
                    }
                    else {
                        result = NO;
                    }
                }
            }
            else if (ifValue.Contains(">="))
            {
                ifEqual = ">=";
                rangeThen = new NSRange(ifValue.IndexOf(ifEqual), ifEqual.Length);
                ifFirst = ifValue.Substring(0, rangeThen.location);
                ifSecond = ifValue.Substring(rangeThen.location + ifEqual.Length);
                ifFirst = checkResult(ifFirst);
                ifSecond = checkResult(ifSecond);
                if (!isStringValue(ifFirst) && !isStringValue(ifSecond))
                {
                    if (variables.variableIsPresent(ifFirst))
                        ifFirst = variables.returnContainOfVariable(ifFirst);
                    if (variables.variableIsPresent(ifSecond))
                        ifSecond = variables.returnContainOfVariable(ifSecond);
                    if (float.Parse(ifFirst) >= float.Parse(ifSecond))
                    {
                        result = YES;
                    }
                    else {
                        result = NO;
                    }
                }
                else {
                    //Log.d(TAG, "± Syntax error in IF operator");
                    GlobalVars.getInstance().error = "Type mismatch in IF operator\n";
                }
            }
            else if (ifValue.Contains("=<"))
            {
                ifEqual = "=<";
                rangeThen = new NSRange(ifValue.IndexOf(ifEqual), ifEqual.Length);
                ifFirst = ifValue.Substring(0, rangeThen.location);
                ifSecond = ifValue.Substring(rangeThen.location + ifEqual.Length);
                ifFirst = checkResult(ifFirst);
                ifSecond = checkResult(ifSecond);
                if (!isStringValue(ifFirst) && !isStringValue(ifSecond))
                {
                    if (variables.variableIsPresent(ifFirst))
                        ifFirst = variables.returnContainOfVariable(ifFirst);
                    if (variables.variableIsPresent(ifSecond))
                        ifSecond = variables.returnContainOfVariable(ifSecond);
                    if (float.Parse(ifFirst) <= float.Parse(ifSecond))
                    {
                        result = YES;
                    }
                    else {
                        result = NO;
                    }
                }
                else {
                    //Log.d(TAG, "± Syntax error in IF operator");
                    GlobalVars.getInstance().error = "Type mismatch in IF operator\n";
                }
            }
            else if (ifValue.Contains("<="))
            {
                ifEqual = "<=";
                rangeThen = new NSRange(ifValue.IndexOf(ifEqual), ifEqual.Length);
                ifFirst = ifValue.Substring(0, rangeThen.location);
                ifSecond = ifValue.Substring(rangeThen.location + ifEqual.Length);
                ifFirst = checkResult(ifFirst);
                ifSecond = checkResult(ifSecond);
                if (!isStringValue(ifFirst) && !isStringValue(ifSecond))
                {
                    if (variables.variableIsPresent(ifFirst))
                        ifFirst = variables.returnContainOfVariable(ifFirst);
                    if (variables.variableIsPresent(ifSecond))
                        ifSecond = variables.returnContainOfVariable(ifSecond);
                    if (float.Parse(ifFirst) <= float.Parse(ifSecond))
                    {
                        result = YES;
                    }
                    else {
                        result = NO;
                    }
                }
                else {
                    //Log.d(TAG, "± Syntax error in IF operator");
                    GlobalVars.getInstance().error = "Type mismatch in IF operator\n";
                }
            }
            else if (ifValue.Contains("<"))
            {
                ifEqual = "<";
                rangeThen = new NSRange(ifValue.IndexOf(ifEqual), ifEqual.Length);
                ifFirst = ifValue.Substring(0, rangeThen.location);
                ifSecond = ifValue.Substring(rangeThen.location + ifEqual.Length);
                ifFirst = checkResult(ifFirst);
                ifSecond = checkResult(ifSecond);
                if (!isStringValue(ifFirst) && !isStringValue(ifSecond))
                {
                    if (variables.variableIsPresent(ifFirst))
                        ifFirst = variables.returnContainOfVariable(ifFirst);
                    if (variables.variableIsPresent(ifSecond))
                        ifSecond = variables.returnContainOfVariable(ifSecond);
                    if (float.Parse(ifFirst) < float.Parse(ifSecond))
                    {
                        result = YES;
                    }
                    else {
                        result = NO;
                    }
                }
                else {
                    //Log.d(TAG, "± Syntax error in IF operator");
                    GlobalVars.getInstance().error = "Type mismatch in IF operator\n";
                }
            }
            else if (ifValue.Contains(">"))
            {
                ifEqual = ">";
                rangeThen = new NSRange(ifValue.IndexOf(ifEqual), ifEqual.Length);
                ifFirst = ifValue.Substring(0, rangeThen.location);
                ifSecond = ifValue.Substring(rangeThen.location + ifEqual.Length);
                ifFirst = checkResult(ifFirst);
                ifSecond = checkResult(ifSecond);
                if (!isStringValue(ifFirst) && !isStringValue(ifSecond))
                {
                    if (variables.variableIsPresent(ifFirst))
                        ifFirst = variables.returnContainOfVariable(ifFirst);
                    if (variables.variableIsPresent(ifSecond))
                        ifSecond = variables.returnContainOfVariable(ifSecond);
                    if (float.Parse(ifFirst) > float.Parse(ifSecond))
                    {
                        result = YES;
                    }
                    else {
                        result = NO;
                    }
                }
                else {
                    //Log.d(TAG, "± Syntax error in IF operator");
                    GlobalVars.getInstance().error = "Type mismatch in IF operator\n";
                }
            }
            else if (ifValue.Contains("="))
            {
                ifEqual = "=";
                rangeThen = new NSRange(ifValue.IndexOf(ifEqual), ifEqual.Length);
                ifFirst = ifValue.Substring(0, rangeThen.location);
                ifSecond = ifValue.Substring(rangeThen.location + ifEqual.Length);
                ifFirst = checkResult(ifFirst);
                ifSecond = checkResult(ifSecond);
                if (!isStringValue(ifFirst) && !isStringValue(ifSecond))
                {
                    if (variables.variableIsPresent(ifFirst))
                        ifFirst = variables.returnContainOfVariable(ifFirst);
                    if (variables.variableIsPresent(ifSecond))
                        ifSecond = variables.returnContainOfVariable(ifSecond);
                    if (float.Parse(ifFirst) == float.Parse(ifSecond))
                    {
                        //Log.d(TAG, "± (digit)IF " + ifFirst + " == " + ifSecond);
                        result = YES;
                    }
                    else {
                        result = NO;
                    }
                }
                else {
                    ifFirst = normaStr.removeSpacesWithText(ifFirst);
                    ifSecond = normaStr.removeSpacesWithText(ifSecond);
                    if (variables.variableIsPresent(ifFirst))
                        ifFirst = "\"" + variables.returnContainOfVariable(ifFirst) + "\"";
                    if (variables.variableIsPresent(ifSecond))
                        ifSecond = "\"" + variables.returnContainOfVariable(ifSecond) + "\"";
                    //Log.d(TAG, "± (digit)IF " + ifFirst + " EQUAL " + ifSecond);
                    if (ifFirst.Equals(ifSecond))
                    {
                        result = YES;
                    }
                    else {
                        result = NO;
                    }
                }
            }
            else {
                //Log.d(TAG, "± Syntax error in IF operator for all!");
                GlobalVars.getInstance().error = "Syntax error in IF operator\n";
            }
            return result;
        }

        public int returnIndexOf(List<String> arr, String line)
        {
            int result = 0;
            for (int i = 0; i < arr.Count; i++)
            {
                String currentLine = arr[i].ToString().Split(' ')[0];
                if (currentLine.Equals(line))
                {
                    result = i;
                }
            }
            return result;
        }


        /*public bool setDim(String string_val)
        {
            //Log.d(TAG, "± set dim..." + string_val);
            bool result = YES;
            if (string_val.Contains("="))
            {
                ArraySet arrayDim = new ArraySet();
                NSRange rangeFirst = new NSRange(string_val.IndexOf("("), 1);
                NSRange rangeSecond = new NSRange(string_val.IndexOf(")"), 1);
                String name = string_val.Substring(0, rangeFirst.location);
                String indexString = string_val.Substring(rangeFirst.location + 1, rangeSecond.location);
                if (variables.variableIsPresent(indexString))
                    indexString = variables.returnContainOfVariable(indexString);
                int index = 0;
                try
                {
                    index = Int32.Parse(indexString);
                }
                catch //(NumberFormatException e)
                {
                    //Log.d(TAG, "± Wrong number format in setDim!");
                }
                //Log.d(TAG, "± set dim index='" + string_val.Substring(rangeFirst.location + 1, rangeSecond.location) + "' name='" + name + "'");
                rangeFirst = new NSRange(string_val.IndexOf("="), 1);
                String value = string_val.Substring(rangeFirst.location + 1);
                //если переменная строковая добавим кавычки но при этом содержимое не содержит кавычки
                if (name.Contains("$") && !normaStr.isText(value)) value = "\"" + value + "\"";
                value = resultFromString(value); //присваиваем результат рассчетов к массиву
                String testValue = value.Replace(" ", "");
                if (!String.IsNullOrEmpty(testValue))
                {
                    if (name.Contains("$"))
                    {
                        //Log.d(TAG, "± dim variable is string_val " + value);
                        if (value.Equals("")) value = "\"\"";
                        if (normaStr.isText(value))
                        {
                            value = value.Replace("\"", "");
                        }
                        else {
                            value = stringFunc.returnStringResult(value);
                        }
                    }
                    else {
                        //Log.d(TAG, "± dim variable is digits " + value);
                        if (String.IsNullOrEmpty(value) || variables.variableIsString(value))
                        {
                            value = "";
                            GlobalVars.getInstance().error = "Type mismatch\n";
                            result = NO;
                        }
                        value = (digitalFunc.returnMathResult(value)).ToString();
                    }

                    for (int i = 0; i < GlobalVars.getInstance().array.Count; i++)
                    {
                        arrayDim = (ArraySet)GlobalVars.getInstance().array[i];
                        String str = arrayDim.name;
                        if (str.Equals(name))
                        {
                            arrayDim.value[index] = value;
                    GlobalVars.getInstance().array[i] = arrayDim;
                }
                //Log.d(TAG, "± dim name='" + name + "' index='" + index + "' value='" + value + "' error='" + GlobalVars.getInstance().error + "' dim='" + arrayDim.value + "'");
            }
        }
        else {
            result = NO;
            GlobalVars.getInstance().error = "Syntax error\n";
        }
}
    else {
        result = NO;
        GlobalVars.getInstance().error = "Syntax error\n";
    }
    return result;
}*/


        public String returnBaseCommand(String string_val)
        {
            String result = string_val.Split(' ')[0];
            for (int i = 0; i < GlobalVars.getInstance().ListOfAll.Count; i++)
            {
                int index = string_val.ToLower().IndexOf(GlobalVars.getInstance().listOfAll[i].ToString());
                if (index != NSNotFound && index == 0)
                    result = GlobalVars.getInstance().listOfAll[i].ToString();
            }
            return result;
        }

        public String input(String string_val)
        {
            String result = "";
            //Log.d(TAG, "± input -'" + string_val + "'");

            GlobalVars.getInstance().scanKeyOn = NO;
            string_val = string_val.Replace(";", ",");
            String alphaSet = "[^a-zA-Z]+";//если только НЕ буквы
            String separator = "\"";
            string_val = string_val.Substring(5);
            String until = " ";
            if (string_val.Contains(separator))
                until = separator + normaStr.extractTextToArray(string_val)[0].ToString() + separator;
            GlobalVars.getInstance().error = "";
            int index = until.Length + 1;
            bool strings = NO;
            bool stringsVariable = NO;
            bool digits = NO;
            bool digitsVariable = NO;
            if (variables.variableIsString(until)) stringsVariable = YES;
            if (variables.variableIsDigit(until)) digitsVariable = YES;
            if (digitalFunc.isOnlyDigitsWithMath(until)) digits = YES;
            if (until.Substring(0, 1).Equals(separator) && until.Substring(until.Length - 1).Equals(separator))
                strings = YES;
            if (String.IsNullOrEmpty(string_val))
            {
                GlobalVars.getInstance().error = "Syntax error\n";
                //Log.d(TAG, "± input Syntax error");
                GlobalVars.getInstance().isOkSet = NO;
            }
            else if (strings && !stringsVariable && !digits && !digitsVariable && string_val.Contains(","))
            {
                result = string_val.Substring(index);
                result = result.Replace(" ", "");
                if (!result.Contains(",")) result = "," + result;
                until = until.Replace(separator, "");
                GlobalVars.getInstance().listOfStrings.Add(until);
                variables.forbiddenVariable(result.Substring(1));
                //Log.d(TAG, "± input string_val result - " + result);
            }
            else {
                //Log.d(TAG, "± input Str-'" + string_val + "'");
                string_val = string_val.Replace(" ", "");
                if (string_val.Contains(",") && !Regex.IsMatch(string_val.Substring(0, 1), alphaSet))
                {
                    result = "," + string_val;
                }
                else if (!Regex.IsMatch(string_val, alphaSet))
                {
                    result = "," + string_val;
                }
                else {
                    result = "," + string_val;
                }
                //Log.d(TAG, "± input result -'" + result + "'");
            }
            return result;

        }

        public String resultFromString(String string_val)
        {
            //очищаем от пробелов
            //заменяем ; на ,
            //Находим все переменные и подменяем на значения
            //Находим массивы и подменяем
            //Находим все функции и внутри них анализируем на наличие арифметики затем рассчитываем и подменяем
            //вычисляем каждый компонент математики
            //строковые функции

            String result = "";
            //Log.d(TAG, "± ========resultFromString begin-'" + string_val + "'");
            //очищаем от пробелов
            string_val = normaStr.removeSpacesWithText(string_val);
            //заменяем ; на ,
            string_val = normaStr.replaceCharWithCharInText(';', ',', string_val);
            //Log.d(TAG, "± ========resultFromString replaceChar:';' withChar:',' ---- '" + string_val + "'");
            List<String> arr = new List<String>(normaStr.stringSeparateAllToArray(string_val)); //делим строку на компоненты
                                                                                                        //Log.d(TAG, "± ========stringSeparateAllToArray " + arr);
                                                                                                        //Находим все переменные и подменяем на значения
            for (int i = 0; i < arr.Count; i++)
            {
                if (variables.variableIsPresent(arr[i].ToString()))
                {
                    String tmp = variables.returnContainOfVariable(arr[i].ToString());
                    if (variables.variableIsString(arr[i].ToString()) && !normaStr.isText(tmp))
                        tmp = "\"" + tmp + "\"";//если переменная строковая добавим кавычки но при этом содержимое не содержит кавычки
                    arr[i] = tmp;
        }
    }
    string_val = "";
    for (int i = 0; i<arr.Count; i++)
    {
        string_val = string_val + arr[i].ToString(); //склеиваем строку из массива
}
arr.Clear();
    //Log.d(TAG, "± ========resultFromString after Replace vars - '" + string_val + "'");
    //Находим массивы и подменяем
    String forDim = "";
bool addMode = NO;
List<String> arrTemp = new List<String>(normaStr.stringSeparateAllToArray(string_val)); //делим строку на компоненты
    if (arrTemp.Count > 3)
    {
        for (int i = 1; i<arrTemp.Count; i++)
        {
            bool addArr = YES;
String curr = arrTemp[i].ToString();
String prev = arrTemp[i - 1].ToString();
            if (addMode)
            {
                addArr = NO;
                if (!curr.Equals(")"))
                {
                    forDim = forDim + curr;
                }
                else {
                    forDim = forDim + curr;
                    addMode = NO;
                    if (variables.isArrayPresent(forDim))
                    {
                        String tmp = variables.returnContainOfArray(forDim);
forDim = tmp;
                    }
                    if (arr.Count == 0)
                    {
                        arr.Add(forDim);
                    }
                    else {
                        arr[arr.Count - 1] = forDim;
                    }
                }
            }
            if (curr.Equals("("))
            {
                if (!digitalFunc.mathFunction(prev) && !stringFunc.stringFunction(prev))
                {
                    forDim = prev + curr;
                    addMode = YES;
                }
                else {
                    if (i == 1) arr.Add(prev);
                    arr.Add(curr);
                }
            }
            else {
                if (addArr)
                {
                    if (i == 1) arr.Add(prev);
                    arr.Add(curr);
                }
            }
        }
    }
    else {
        arr = new List<String>(arrTemp);
    }
    string_val = "";
    for (int i = 0; i<arr.Count; i++)
    {
        string_val = string_val + arr[i].ToString(); //склеиваем строку из массива
    }
    arr.Clear();
    arrTemp.Clear();
    //Log.d(TAG, "± ========resultFromString after Replace dims - '" + string_val + "'");
    //склеиваем все строки стоящие рядом
    String fStr = "";
String tStr = "";
bool firstEl = NO;
bool secondEl = NO;
int index = 0;
arr = new List<String>(normaStr.stringSeparateAllToArray(string_val)); //делим строку на компоненты
                                                                    //    //Log.d(TAG, "± resultFromString arr-''",arr);
    if (arr.Count > 3 && !string_val.Contains("instr"))
    {
        string_val = "";
        for (int i = 0; i<arr.Count; i++)
        {
            arrTemp.Add(arr[i]);
            if (normaStr.isText(arr[i].ToString()) && !firstEl && !secondEl)
            { //находим первый элемент текст но еще нет второго и задает индекс первого
                fStr = arr[i].ToString().Replace("\"", "");
firstEl = YES;
                index = i;
            }
            if (arr[i].ToString().Equals(",") && firstEl && !secondEl && i == index + 1)
            {//находим 2 элемент запятую и уже есть первый
                secondEl = YES;
            }
            if (arr[i].ToString().Equals("+") && firstEl && !secondEl && i == index + 1)
            {//находим 2 элемент + и уже есть первый
                secondEl = YES;
            }
            if (i == index + 1 && !secondEl)// если индекс первого эл-та предидущий но нет второго то все сбрасываем
            {
                firstEl = NO;
                fStr = "";
                tStr = "";
            }
            if (i == index + 2 && secondEl && !normaStr.isText(arr[i].ToString()))// если индекс первого эл-та -2 но третий эл-т не текст то все сбрасываем
            {
                firstEl = NO;
                secondEl = NO;
                fStr = "";
                tStr = "";
            }
            if (normaStr.isText(arr[i].ToString()) && firstEl && secondEl)
            { //находим третий элемент текст имея первый и второй
                tStr = arr[i].ToString().Replace("\"", "");
firstEl = YES;
                secondEl = NO;
                index = i;
                if (arrTemp.Count > 2)
                {
                    arrTemp.RemoveAt(arrTemp.Count - 1);
                    arrTemp.RemoveAt(arrTemp.Count - 1);
                    arrTemp.RemoveAt(arrTemp.Count - 1);
                }
                fStr = "\"" + fStr + tStr + "\"";
                arrTemp.Add(fStr);
                fStr = fStr.Replace("\"", "");
            }
        }
    }
    for (int i = 0; i<arrTemp.Count; i++)
    {
        string_val = string_val + arrTemp[i].ToString(); //склеиваем строку из массива
    }
    arr.Clear();
    arrTemp.Clear();
    //Log.d(TAG, "± ========resultFromString after strings-'" + string_val + "'");
    //Находим все мат функции и внутри них анализируем на наличие арифметики затем рассчитываем и подменяем
    String forMFunc = "";
int addCount = 0;
bool first = NO;
addMode = NO;
    arrTemp = new List<String>(normaStr.stringSeparateAllToArray(string_val)); //делим строку на компоненты
    if (arrTemp.Count > 3)
    {
        for (int i = 1; i<arrTemp.Count; i++)
        {
            String curr = arrTemp[i].ToString();
String prev = arrTemp[i - 1].ToString();
            if (curr.Equals("(") && digitalFunc.mathFunction(prev) && !addMode)
            { //определяем когда началась мат функция
                addCount++;
                addMode = YES;
                first = YES;
                forMFunc = prev + curr;
            }
            if (curr.Equals("(") && stringFunc.stringFunction(prev) && !addMode)
            { //определяем когда началась строковая функция
                addCount++;
                addMode = YES;
                first = YES;
                forMFunc = prev + curr;
            }
            if (addMode)
            { // режим наращивания функции
                if (!first)
                {
                    forMFunc = forMFunc + curr;
                    if (curr.Equals("("))
                    {
                        addCount++;
                    }
                    if (curr.Equals(")"))
                    {
                        addCount--;
                    }
                    if (addCount == 0)
                    {
                        if (forMFunc.Length > 3)
                            if (forMFunc.Substring(0, 4).Equals("abs(") || forMFunc.Substring(0, 4).Equals("fix("))
                            {
                                String temp = digitalFunc.mathFunctionInMixedString(forMFunc.Substring(4, forMFunc.Length - 1));
forMFunc = forMFunc.Substring(0, 4) + temp + ")";
                            }
                        arr.Add(forMFunc);
                        forMFunc = "";
                        addMode = NO;
                    }
                }
                first = NO;
            }
            else { // режим просто добавления без мат функции
                if (i == 1) arr.Add(prev);
                if (!digitalFunc.mathFunction(curr) && !stringFunc.stringFunction(curr))
                    arr.Add(curr);
            }
        }
    }
    else {
        arr = new List<String>(arrTemp);
    }
    string_val = "";
    for (int i = 0; i<arr.Count; i++)
    {
        String tmp = arr[i].ToString();
        if (digitalFunc.mathFunction(tmp))
        {
            tmp = digitalFunc.mathFunctionInMixedString(tmp);
            tmp = (digitalFunc.returnMathResult(tmp)).ToString();
        }
        if (stringFunc.stringFunction(tmp))
            tmp = stringFunc.returnStringResult(tmp); // проверяем на строковые функции и возвращаем результаты
        string_val = string_val + tmp; //склеиваем строку из массива
    }
    arr.Clear();
    //Log.d(TAG, "± ========resultFromString after mathematics & string_val func's-'" + string_val + "'");

    //вычисляем каждый компонент математики
    if (string_val.Length > 0)
        string_val = digitalFunc.mathFunctionInMixedString(string_val); //1. mathFunctionInMixedString
                                                                //Log.d(TAG, "± ========resultFromString mathFunctionInMixedString-'" + string_val + "'");
                                                                //избавляемся от запятых и с плюсов, склеиваем строку из компонентов
    arr = new List<String>(normaStr.stringSeparateAllToArray(string_val)); //делим строку на компоненты
    bool quotes = NO;
    for (int i = 0; i<arr.Count; i++)
    {
        if (normaStr.isText(arr[i].ToString())) quotes = YES;
        if (!arr[i].ToString().Equals(","))
            if (!arr[i].ToString().Equals("+"))
                result = result + arr[i].ToString(); //склеиваем строку из массива
    }
    if (quotes)
    {
        result = result.Replace("\"", "");
        result = "\"" + result + "\"";
    }
    //Log.d(TAG, "± ========resultFromString FINAL RESULT-'" + result + "'");
    return result;
}

        public void renumGotoGosub()
        {
            List<String> oldList = new List<String>(GlobalVars.getInstance().listOfProgram);
            String replaceString;
            String number;
            if (GlobalVars.getInstance().listOfProgram.Count > 0)
            {
                String gotoGosub = "goto";
                int c = 1;
                for (int i = 0; i < GlobalVars.getInstance().listOfProgram.Count; i++)
                {
                    number = GlobalVars.getInstance().listOfProgram[i].ToString().Split(' ')[0];
                    replaceString = GlobalVars.getInstance().listOfProgram[i].ToString();
                    replaceString = c * 10 + " " + replaceString.Substring(number.Length + 1, replaceString.Length);
                    GlobalVars.getInstance().listOfProgram[i] = replaceString;
                c++;
            }
            for (int i = 0; i < GlobalVars.getInstance().listOfProgram.Count; i++)
            {
                replaceString = GlobalVars.getInstance().listOfProgram[i].ToString();
                //Log.d(TAG, "± before number=" + i + " replaceString='" + replaceString + "'");
                NSRange range = new NSRange(replaceString.IndexOf(gotoGosub), gotoGosub.Length);
                String[] tempArr = replaceString.Split(Convert.ToChar(gotoGosub));
                c = 0;
                int locat = (int)(range.location + range.length + 1);
                bool found = NO;
                int index = 0;
                for (int cc = 1; cc < tempArr.Length; cc++)
                    if (range.location != NSNotFound && !normaStr.insideText(replaceString, locat))
                    {
                        index = replaceString.Length;
                        for (int t = locat; t < replaceString.Length; t++)
                            if (replaceString.ElementAt(t) == ' ' && !found)
                            {
                                //Log.d(TAG, "± '" + replaceString.ElementAt(t) + "'");
                                index = t;
                                found = YES;
                            }
                        String first = GlobalVars.getInstance().listOfProgram[i].ToString().Substring(0, locat);
                        String whatRepl = replaceString.Substring(locat, index);
                        String second = GlobalVars.getInstance().listOfProgram[i].ToString().Substring(index);
                        int renIndex = returnIndexOf(oldList, whatRepl);
                        String withRepl = GlobalVars.getInstance().listOfProgram[renIndex].ToString().Split(' ')[0];
            String repStr = first + withRepl + second;
            GlobalVars.getInstance().listOfProgram[i] = repStr;
            replaceString = repStr;
            range = new NSRange(replaceString.Substring(locat).IndexOf(gotoGosub) + locat, gotoGosub.Length);
            locat = range.location + range.length + 1;
            found = NO;
            index = 0;
            first = "";
            second = "";
            whatRepl = "";
            withRepl = "";
        }
    }

    gotoGosub = "gosub";
        for (int i = 0; i<GlobalVars.getInstance().listOfProgram.Count; i++)
        {
            replaceString = GlobalVars.getInstance().listOfProgram[i].ToString();
    NSRange range = new NSRange(replaceString.IndexOf(gotoGosub), gotoGosub.Length);
    String[] tempArr = replaceString.Split(Convert.ToChar(gotoGosub));
    c = 0;
            int locat = (int)(range.location + range.length + 1);
    bool found = NO;
    int index = 0;
            for (int cc = 1; cc<tempArr.Length; cc++)
                if (range.location != NSNotFound && !normaStr.insideText(replaceString, locat))
                {
                    index = (int)replaceString.Length;
                    for (int t = locat; t<replaceString.Length; t++)
                        if (replaceString.ElementAt(t) == ' ' && !found)
                        {
                            //Log.d(TAG, "± '" + replaceString.ElementAt(t) + "'");
                            index = t;
                            found = YES;
                        }
String first = GlobalVars.getInstance().listOfProgram[i].ToString().Substring(0, locat);
String whatRepl = replaceString.Substring(locat, index);
String second = GlobalVars.getInstance().listOfProgram[i].ToString().Substring(index);
int renIndex = returnIndexOf(oldList, whatRepl);
String withRepl = GlobalVars.getInstance().listOfProgram[renIndex].ToString().Split(' ')[0];
                    String repStr = first + withRepl + second;
GlobalVars.getInstance().listOfProgram[i] = repStr;
                    if (!String.IsNullOrEmpty(whatRepl)) GlobalVars.getInstance().listOfProgram[i] = repStr;
                    replaceString = repStr;
                    range = new NSRange(replaceString.Substring(locat).IndexOf(gotoGosub) + locat, gotoGosub.Length);
                    range.location = range.location + locat;
                    locat = (int)(range.location + range.length + 1);
                    found = NO;
                    index = 0;
                    first = "";
                    second = "";
                    whatRepl = "";
                    withRepl = "";
                }
        }
        gotoGosub = "restore";
        for (int i = 0; i<GlobalVars.getInstance().listOfProgram.Count; i++)
        {
            replaceString = GlobalVars.getInstance().listOfProgram[i].ToString();
NSRange range = new NSRange(replaceString.IndexOf(gotoGosub), gotoGosub.Length);
String[] tempArr = replaceString.Split(Convert.ToChar(gotoGosub));
c = 0;
            int locat = (int)(range.location + range.length + 1);
bool found = NO;
int index = 0;
            for (int cc = 1; cc<tempArr.Length; cc++)
                if (range.location != NSNotFound && !normaStr.insideText(replaceString, locat))
                {
                    index = (int)replaceString.Length;
                    for (int t = locat; t<replaceString.Length; t++)
                        if (replaceString.ElementAt(t) == ' ' && !found)
                        {
                            //Log.d(TAG, "± '" + replaceString.ElementAt(t) + "'");
                            index = t;
                            found = YES;
                        }
                    String first = GlobalVars.getInstance().listOfProgram[i].ToString().Substring(0, locat);
String whatRepl = replaceString.Substring(locat, index);
String second = GlobalVars.getInstance().listOfProgram[i].ToString().Substring(index);
int renIndex = returnIndexOf(oldList, whatRepl);
String withRepl = GlobalVars.getInstance().listOfProgram[renIndex].ToString().Split(' ')[0];
                    String repStr = first + withRepl + second;
GlobalVars.getInstance().listOfProgram[i] = repStr;

                    if (!String.IsNullOrEmpty(whatRepl)) GlobalVars.getInstance().listOfProgram[i] = repStr;
                    replaceString = repStr;
                    range = new NSRange(replaceString.Substring(locat).IndexOf(gotoGosub) + locat, gotoGosub.Length);
                    range.location = range.location + locat;
                    locat = (int)(range.location + range.length + 1);
                    found = NO;
                    index = 0;
                    first = "";
                    second = "";
                    whatRepl = "";
                    withRepl = "";
                }
        }
    }
}




    }
}
