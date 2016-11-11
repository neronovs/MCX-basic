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
        private readonly int NSNotFound = -1;
        private readonly bool NO = false;
        private readonly bool YES = true;
        private readonly String TAG = MethodBase.GetCurrentMethod().DeclaringType.Name + ": ";
        private DigitalFunc digitalFunc = new DigitalFunc();
        private NormalizeString normaStr = new NormalizeString();
        private Variables variables = new Variables();
        private StringFunc stringFunc = new StringFunc();

        MainForm mainForm;
        FormDelegate formDelegate;


        public RunCommand(MainForm reference)
        {
            mainForm = reference;
            
            formDelegate += mainForm.Cls;
        }

        public RunCommand() { }

        public bool set(String string_val)
        {
            Debug.WriteLine(TAG + string_val);
            //if (string_val.Length <= 0) return false;
            string_val = normaStr.lowcaseWithText(string_val);
            bool result = false;
            if (string_val.Substring(0, 1).Equals(" "))
                string_val = normaStr.removeSpaceInBegin(string_val).ToLower();
            String separator = "\"";
            String base_val = returnBaseCommand(string_val);
            Debug.WriteLine(TAG + string_val);
            GlobalVars.getInstance().Error = "";
            GlobalVars.getInstance().ListOfStrings.Clear();

            if (base_val.ToLower().Equals("ver"))
            {
                //App version
                string versionApp = Assembly.GetEntryAssembly().GetName().Version.ToString();
                //File version
                string versionFile = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion;
                Debug.WriteLine("Ver..." + GlobalVars.getInstance().Version1);
                Debug.WriteLine(TAG + "± " + GlobalVars.getInstance().Version1 + versionApp + Environment.NewLine);


                GlobalVars.getInstance().ListOfStrings.Add(GlobalVars.getInstance().Version1 + versionApp + Environment.NewLine);
                GlobalVars.getInstance().ListOfStrings.Add(GlobalVars.getInstance().Version2 + Environment.NewLine);
                GlobalVars.getInstance().ListOfStrings.Add(GlobalVars.getInstance().Version3 + Environment.NewLine);
                foreach (string element in GlobalVars.getInstance().ListOfStrings) Debug.WriteLine(element);
                result = true;
            }
            else if (base_val.ToLower().Equals("auto"))
            {
                Debug.WriteLine(TAG + "± autoSet = YES");
                GlobalVars.getInstance().ListOfStrings.Add("Press \"ctrl + c\" to stop the auto-numeration" + Environment.NewLine);
                GlobalVars.getInstance().AutoSet = (true);
                GlobalVars.getInstance().IsOkSet = (false);
                GlobalVars.getInstance().ListOfStrings.Add(("\n" + GlobalVars.getInstance().ProgramCounter.ToString()) + " ");
                result = true;
            }
            else if (base_val.ToLower().Equals("list"))
            {
                GlobalVars.getInstance().IsOkSet = (true);
                String listNumber;
                String currentLineNumber;
                if (string_val.Length > 4)
                {
                    //string_val = string_val.Substring(4, string_val.Length);
                    string_val = string_val.Substring(4);
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
                                        GlobalVars.getInstance().ListOfStrings.Add((GlobalVars.getInstance().ListOfProgram[i].ToString() + Environment.NewLine).ToString());
                                    }
                                }
                                result = true;
                            }
                            else {
                                GlobalVars.getInstance().Error = "Syntax error" + Environment.NewLine;
                            }
                        }
                    }
                    else {
                        /*listNumber = string_val;
                        if (digitalFunc.isOnlyDigits(listNumber))
                        {*/
                        for (int i = 0; i < GlobalVars.getInstance().ListOfProgram.Count; i++)
                        {
                            currentLineNumber = GlobalVars.getInstance().ListOfProgram[i].ToString();
                            currentLineNumber = currentLineNumber.Split(' ')[0];
                            //if (currentLineNumber.ToLower().Equals(listNumber))
                            {
                                GlobalVars.getInstance().ListOfStrings.Add((GlobalVars.getInstance().ListOfProgram[i].ToString() + Environment.NewLine).ToString());
                            }
                        }
                        result = true;
                        /* } else {
                             GlobalVars.getInstance().Error = "Syntax error" + Environment.NewLine;
                         }*/
                    }
                if (String.IsNullOrEmpty(string_val))
                    for (int i = 0; i < GlobalVars.getInstance().ListOfProgram.Count; i++)
                    {
                        GlobalVars.getInstance().ListOfStrings.Add((GlobalVars.getInstance().ListOfProgram[i].ToString() + Environment.NewLine).ToString());
                    }
                result = true;
            }
            else if (base_val.ToLower().Equals("help"))
            {
                Debug.WriteLine(TAG + "± help");
                for (int i = 0; i < GlobalVars.getInstance().ListOfCommands.Count; i++)
                {
                    GlobalVars.getInstance().ListOfStrings.Add((GlobalVars.getInstance().ListOfCommands[i].ToString() + Environment.NewLine).ToString());
                }
                GlobalVars.getInstance().ListOfStrings.Add((Environment.NewLine).ToString());
                result = true;
            }
            else if (base_val.ToLower().Equals("clear"))
            {
                GlobalVars.getInstance().Variables.Clear();
                result = YES;
            }
            else if (base_val.ToLower().Equals("cls"))
            {
                try
                {
                    formDelegate();
                }
                catch { Debug.WriteLine(TAG + "± cls: Did NOT Initialized the DELEGATE"); }

                //mainForm.Cls();
                
                result = YES;
            }

            else if (base_val.ToLower().Equals("exit"))
            {
                Debug.WriteLine(TAG + "± exit");
                Environment.Exit(0);
            }

            else if (base_val.ToLower().Equals("end"))
            {
                Debug.WriteLine(TAG + "± end");
                GlobalVars.getInstance().Run = false;
                GlobalVars.getInstance().IsOkSet = true;
                result = true;
            }
            else if (base_val.ToLower().Equals("beep"))
            {
                Debug.WriteLine(TAG + "± Beep begin ...");
                result = NO;
            }
            else if (base_val.ToLower().Equals("color"))
            {
                result = NO;
            }
            else if (base_val.ToLower().Equals("input"))
            {
                GlobalVars.getInstance().Input = input(string_val);
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
                    Debug.WriteLine(TAG + "± READING ... '" + string_val + "'");
                    if (GlobalVars.getInstance().DataReadIndex < GlobalVars.getInstance().Data.Count)
                    {
                        if (string_val.Contains(","))
                        {
                            String[] arr = string_val.Substring(5).Split(',');
                            for (int i = 0; i < arr.Length; i++)
                            {
                                String stmp = arr[i];
                                if (stmp.Contains(separator))
                                {
                                    GlobalVars.getInstance().Error = "Syntax error" + Environment.NewLine;
                                }
                                else if (digitalFunc.isOnlyDigits(stmp))
                                { // если только цифры
                                    GlobalVars.getInstance().Error = "Syntax error" + Environment.NewLine;
                                }
                                else if (GlobalVars.getInstance().DataReadIndex >= GlobalVars.getInstance().Data.Count)
                                { // Out of DATA
                                    GlobalVars.getInstance().Error = "Out of DATA" + Environment.NewLine;
                                }
                                else {
                                    String dataValue = GlobalVars.getInstance().Data[GlobalVars.getInstance().DataReadIndex].ToString();
                                    if (variables.variableIsString(stmp)) dataValue = "\"" + dataValue + "\"";
                                    if (variables.isArrayPresent(stmp))
                                    {
                                        setDim(stmp + "=" + dataValue);
                                        GlobalVars.getInstance().DataReadIndex++;
                                    }
                                    else {
                                        int index = variables.makeVariableIndex(stmp);
                                        if (!variables.forbiddenVariable(stmp) && stmp.Length > 0 && String.IsNullOrEmpty(GlobalVars.getInstance().Error))
                                        {
                                            if (index == GlobalVars.getInstance().Variables.Count())
                                            {
                                                GlobalVars.getInstance().Variables.Add(addVariable(variables.returnVarValue(dataValue), stmp));
                                            }
                                            else {
                                                GlobalVars.getInstance().Variables[index] = addVariable(variables.returnVarValue(dataValue), stmp);
                                            }
                                        }
                                        GlobalVars.getInstance().DataReadIndex++;
                                    }
                                }
                            }
                        }
                        else {
                            String stmp = string_val.Substring(5);
                            Debug.WriteLine(TAG + "± READ to ... variable " + stmp);
                            if (stmp.Contains(separator))
                            {
                                GlobalVars.getInstance().Error = "Syntax error" + Environment.NewLine;
                            }
                            else if (digitalFunc.isOnlyDigits(stmp))
                            { // если только цифры
                                GlobalVars.getInstance().Error = "Syntax error" + Environment.NewLine;
                            }
                            else {
                                String dataValue = GlobalVars.getInstance().Data[GlobalVars.getInstance().DataReadIndex].ToString();
                                if (variables.variableIsString(stmp)) dataValue = "\"" + dataValue + "\"";
                                if (variables.isArrayPresent(stmp))
                                {
                                    setDim(stmp + "=" + dataValue);
                                    GlobalVars.getInstance().DataReadIndex++;
                                }
                                else {
                                    int index = variables.makeVariableIndex(stmp);
                                    if (!variables.forbiddenVariable(stmp) && stmp.Length > 0 && String.IsNullOrEmpty(GlobalVars.getInstance().Error))
                                    {
                                        if (index == GlobalVars.getInstance().Variables.Count())
                                        {
                                            GlobalVars.getInstance().Variables.Add(addVariable(variables.returnVarValue(dataValue), stmp));
                                        }
                                        else {
                                            GlobalVars.getInstance().Variables[index] = addVariable(variables.returnVarValue(dataValue), stmp);
                                        }
                                    }
                                    GlobalVars.getInstance().DataReadIndex++;
                                }
                            }
                        }
                        if (String.IsNullOrEmpty(GlobalVars.getInstance().Error)) result = YES;
                    }
                    else {
                        GlobalVars.getInstance().Error = "Out of DATA" + Environment.NewLine;
                    }
                }
                else {
                    GlobalVars.getInstance().Error = "Missing operand" + Environment.NewLine;
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
                            Debug.WriteLine(TAG + "± Wrong number format in delete(RunCommand)!");
                        }
                        for (int li = begin; li <= end; li++)
                        {
                            listNimber = (li).ToString();
                            if (digitalFunc.isOnlyDigits(listNimber))
                            {
                                for (int i = 0; i < GlobalVars.getInstance().ListOfProgram.Count; i++)
                                {
                                    currentLineNimber = GlobalVars.getInstance().ListOfProgram[i].ToString().Split(' ')[0];
                                    if (currentLineNimber.Equals(listNimber))
                                        toDelete.Add(GlobalVars.getInstance().ListOfProgram[i].ToString());
                                }
                                for (int i = 0; i < toDelete.Count; i++)
                                {
                                    GlobalVars.getInstance().ListOfProgram.Remove(toDelete[i]);
                                }
                                result = YES;
                            }
                            else {
                                GlobalVars.getInstance().Error = "Syntax error" + Environment.NewLine;
                            }
                        }
                    }
                    else {
                        listNimber = string_val;
                        if (digitalFunc.isOnlyDigits(listNimber))
                        {
                            for (int i = 0; i < GlobalVars.getInstance().ListOfProgram.Count; i++)
                            {
                                currentLineNimber = GlobalVars.getInstance().ListOfProgram[i].ToString().Split(' ')[0];
                                if (currentLineNimber.Equals(listNimber))
                                {
                                    toDelete.Add(GlobalVars.getInstance().ListOfProgram[i]);
                                }
                            }
                            for (int i = 0; i < toDelete.Count; i++)
                            {
                                GlobalVars.getInstance().ListOfProgram.Remove(toDelete[i]);
                            }
                            result = YES;
                        }
                        else {
                            GlobalVars.getInstance().Error = "Syntax error" + Environment.NewLine;
                        }
                    }
                }
                else {
                    GlobalVars.getInstance().Error = "Missing operand" + Environment.NewLine;
                }
            }
            else if (base_val.ToLower().Equals("restore"))
            {
                GlobalVars.getInstance().IsOkSet = YES;
                String listNumber = string_val.Substring(0, 7).Replace(" ", "");
                listNumber = resultFromString(listNumber);
                if (String.IsNullOrEmpty(listNumber))
                {
                    GlobalVars.getInstance().DataReadIndex = 0;
                    result = YES;
                }
                else {
                    if (digitalFunc.isOnlyDigits(listNumber))
                    {
                        try
                        {
                            GlobalVars.getInstance().DataReadIndex = Int32.Parse(GlobalVars.getInstance().DataIndex[Int32.Parse(listNumber)].ToString());
                        }
                        catch //(NumberFormatException e)
                        {
                            Debug.WriteLine(TAG + "± Wrong number format in restore!");
                        }
                        result = YES;
                    }
                    else {
                        GlobalVars.getInstance().Error = "Syntax error" + Environment.NewLine;
                    }
                    Debug.WriteLine(TAG + "restore GlobalVars.getInstance().DataReadIndex=" + GlobalVars.getInstance().DataReadIndex);
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
                    GlobalVars.getInstance().CommandIf = "";
                    // NSCharacterSet * equalSet = [NSCharacterSet characterSetWithCharactersInString:"=<>"];.matches("[a-zA-Z]+")) { //если только буквы
                    String ifValue = normaStr.removeSpaceInBegin(string_val.Substring(3));
                    String thenValue;
                    String elseValue = "";
                    NSRange rangeThen = new NSRange(ifValue.IndexOf("then"), 4);
                    //Debug.WriteLine(TAG + "± !!! IF operator contain [=<>] " + ifValue.Substring(0, rangeThen.location) + " " + ifValue.Substring(0, rangeThen.location).Contains(".*[=<>]+.*"));
                    if (rangeThen.location == NSNotFound || Regex.IsMatch(ifValue.Substring(0, rangeThen.location), (".*[=<>]+.*")) == false)
                    //ifValue.Substring(0, rangeThen.location).matches(".*[=<>]+.*"))
                    {
                        Debug.WriteLine(TAG + "± Syntax error in IF operator");
                        GlobalVars.getInstance().Error = "Syntax error in IF operator" + Environment.NewLine;
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
                            thenValue = normaStr.removeSpaceInBeginAndEnd(string_val.Substring(rangeThen.location + 4, rangeElse.location - rangeThen.location - 4));
                        }

                        if (ifThen(ifValue))
                        {  //  Вызываем метод проверки IF
                            GlobalVars.getInstance().CommandIf = thenValue;
                        }
                        else {
                            GlobalVars.getInstance().CommandIf = elseValue;
                        }
                    }
                }
                else {
                    GlobalVars.getInstance().Error = "Missing operand" + Environment.NewLine;
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
                        Debug.WriteLine(TAG + "± for without to");
                        GlobalVars.getInstance().Error = "Syntax error - FOR without TO" + Environment.NewLine;
                    }
                    else {
                        forValue = forValue.Substring(0, rangeTo.location - 1).Replace(" ", "");
                    }
                    rangeTo = new NSRange(string_val.IndexOf("to"), 2);
                    NSRange rangeStep = new NSRange(string_val.IndexOf("step"), 4);
                    if (rangeStep.location == NSNotFound)
                    {
                        Debug.WriteLine(TAG + "± for without step ");
                        toValue = string_val.Substring(rangeTo.location + 2);
                        toValue = toValue.Replace(" ", "");
                        stepValue = "1";
                    }
                    else {
                        stepValue = string_val.Substring(rangeStep.location + 4).Replace(" ", "");
                        toValue = string_val.Substring(rangeTo.location + 2, rangeTo.length + 1).Replace(" ", "");
                    }
                    Debug.WriteLine(TAG + "± !!!! forValue='" + forValue + "' toValue='" + toValue + "' stepValue='" + stepValue + "'");
                    String tmpFor = resultFromString(variables.returnVarValue(forValue));
                    String tmpTo = resultFromString(variables.returnVarValue(toValue));
                    String tmpStep = resultFromString(variables.returnVarValue(stepValue));
                    Debug.WriteLine(TAG + "±     forValue=" + forValue + " toValue=" + toValue + " stepValue=" + stepValue);
                    Debug.WriteLine(TAG + "± TMP forValue=" + tmpFor + " toValue=" + tmpTo + " stepValue=" + tmpStep);
                    String varName = variables.returnVarName(forValue);
                    forSet.ForLine = GlobalVars.getInstance().RunnedLine;
                    forSet.ForName = varName;
                    forSet.ForStep = tmpStep;
                    forSet.ForTo = tmpTo;
                    GlobalVars.getInstance().ForArray.Add(forSet);
                    int index = variables.makeVariableIndex(varName);
                    String value = tmpFor;
                    if (!variables.forbiddenVariable(varName) && varName.Length > 0 && String.IsNullOrEmpty(GlobalVars.getInstance().Error))
                    {
                        if (index == GlobalVars.getInstance().Variables.Count)
                        {
                            GlobalVars.getInstance().Variables.Add(addVariable(value, varName));
                        }
                        else {
                            GlobalVars.getInstance().Variables[index] = addVariable(value, varName);
                        }
                        result = YES;
                    }
                    else {
                        GlobalVars.getInstance().Error = "Syntax error";
                        GlobalVars.getInstance().Command = "";
                        Debug.WriteLine(TAG + "± let empty");
                    }
                    result = YES;
                }
                else {
                    GlobalVars.getInstance().Error = "Missing operand" + Environment.NewLine;
                }
            }
            else if (base_val.ToLower().Equals("next"))
            {
                if (GlobalVars.getInstance().ForArray.Count > 0)
                {
                    ForSet forSet = new ForSet();
                    forSet = (ForSet)GlobalVars.getInstance().ForArray[GlobalVars.getInstance().ForArray.Count - 1];

                    int forI = 0, ff = 0;
                    try
                    {
                        Debug.WriteLine(TAG + "± NEXT try int variables.returnContainOfVariable(forSet.forName)=" + variables.returnContainOfVariable(forSet.ForName));
                        Debug.WriteLine(TAG + "± NEXT try int forSet.forStep=" + forSet.ForStep);
                        forI = (int)(float.Parse(variables.returnContainOfVariable(forSet.ForName)) + float.Parse(forSet.ForStep));
                        ff = (int)float.Parse(forSet.ForTo);
                    }
                    catch //(NumberFormatException e)
                    {
                        Debug.WriteLine(TAG + "± Wrong number format in NEXT operator!");
                    }
                    if (forI > ff)
                    {
                        GlobalVars.getInstance().ForArray.RemoveAt(GlobalVars.getInstance().ForArray.Count - 1);
                    }
                    else {
                        GlobalVars.getInstance().Variables[variables.makeVariableIndex(forSet.ForName)] = addVariable((forI).ToString(), forSet.ForName);
                        GlobalVars.getInstance().RunIndex = returnIndexFromLine(forSet.ForLine) + 1;
                    }
                    result = YES;
                }
                else {
                    GlobalVars.getInstance().Error = "NEXT without FOR" + Environment.NewLine;
                }
            }
            else if (base_val.ToLower().Equals("goto"))
            {
                if (string_val.Length > base_val.Length + 1)
                {
                    string_val = (string_val.Substring(5).Replace(" ", ""));
                    GlobalVars.getInstance().RunIndex = returnIndexFromLine(string_val);
                    result = YES;
                    if (GlobalVars.getInstance().RunIndex < -1)
                    {
                        GlobalVars.getInstance().RunIndex = 0;
                        GlobalVars.getInstance().Error = "Undefined line number" + Environment.NewLine;
                        result = NO;
                    }
                }
                else {
                    GlobalVars.getInstance().Error = "Missing operand" + Environment.NewLine;
                }
            }
            else if (base_val.ToLower().Equals("gosub"))
            {
                if (string_val.Length > base_val.Length + 1)
                {
                    string_val = (string_val.Substring(6).Replace(" ", ""));
                    GlobalVars.getInstance().GosubArray.Add(GlobalVars.getInstance().RunnedLine);
                    GlobalVars.getInstance().RunIndex = returnIndexFromLine(string_val);
                    result = YES;
                    if (GlobalVars.getInstance().RunIndex < -1)
                    {
                        GlobalVars.getInstance().RunIndex = 0;
                        GlobalVars.getInstance().Error = "Undefined line number" + Environment.NewLine;
                        result = NO;
                    }
                }
                else {
                    GlobalVars.getInstance().Error = "Missing operand" + Environment.NewLine;
                }
            }
            else if (base_val.ToLower().Equals("return"))
            {
                if (GlobalVars.getInstance().GosubArray.Count > 0)
                {
                    string_val = GlobalVars.getInstance().GosubArray[GlobalVars.getInstance().GosubArray.Count - 1].ToString();
                    GlobalVars.getInstance().GosubArray.RemoveAt(GlobalVars.getInstance().GosubArray.Count - 1);
                    Debug.WriteLine(TAG + "± return - " + string_val);
                    GlobalVars.getInstance().RunIndex = returnIndexFromLine(string_val) + 1;
                    result = YES;
                }
                else {
                    GlobalVars.getInstance().Error = "RETURN without GOSUB" + Environment.NewLine;
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
                    GlobalVars.getInstance().Error = "Missing operand" + Environment.NewLine;
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
                    GlobalVars.getInstance().Error = "Missing operand" + Environment.NewLine;
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
                    GlobalVars.getInstance().Error = "Missing operand" + Environment.NewLine;
                }
            }
            else if (base_val.ToLower().Equals("print"))
            {
                if (string_val.Length > base_val.Length + 1)
                {
                    result = print(string_val);
                }
                else {
                    GlobalVars.getInstance().Error = "Missing operand" + Environment.NewLine;
                }
            }
            else if (base_val.ToLower().Equals("varl"))
            {
                for (int i = 0; i < GlobalVars.getInstance().Variables.Count; i++)
                {
                    VariableSet varSet = GlobalVars.getInstance().Variables[i];
                    GlobalVars.getInstance().ListOfStrings.Add(varSet.name + " = " + varSet.var + Environment.NewLine);
                }
                result = YES;
            }
            else if (base_val.ToLower().Equals("csrlin"))
            {
                /*if (string_val.Length > base_val.Length + 1)
                {*/
                    GlobalVars.getInstance().ListOfStrings.Add(GlobalVars.getInstance().LineNumber + Environment.NewLine);
                    result = YES;
               /* }
                else {
                    GlobalVars.getInstance().Error = "Missing operand" + Environment.NewLine;
                }*/
            }
            else if (base_val.ToLower().Equals("pos"))
            {
                GlobalVars.getInstance().ListOfStrings.Add(GlobalVars.getInstance().Command.Length.ToString() + Environment.NewLine);
                result = YES;

            }
            else if (base_val.ToLower().Equals("let"))
            {
                if (string_val.Length > base_val.Length + 1)
                {
                    String clearString = string_val.Substring(4);
                    result = let(clearString);
                }
                else {
                    GlobalVars.getInstance().Error = "Missing operand" + Environment.NewLine;
                }
            }
            else if (base_val.ToLower().Equals("run"))
            {
                
                /*if (string_val.Length > 4) {
                    load(string_val.Substring(4));
                }*/ 
                GlobalVars.getInstance().IsOkSet = NO;
                result = NO;
                scanData();
            }
            else if (base_val.ToLower().Equals("files"))
            {
                DirectoryInfo directory = new DirectoryInfo(GlobalVars.getInstance().CurrentFolder);
                //FileInfo[] list = directory.GetFiles(directory.FullName, SearchOption.AllDirectories);
                string[] list = Directory.GetFiles(directory.FullName);
                if (list == null)
                    //list = new FileInfo[] { };
                    list = new string[] { };
                List<string> fileList = list.ToList();
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
                for (int i = 0; i < fileList.Count; i++)
                    if (fileList[i].ToString().Contains(".bas"))
                        GlobalVars.getInstance().ListOfStrings.Add(fileList[i].ToString().Substring(fileList[i].ToString().LastIndexOf("\\") + 1) + Environment.NewLine);
                Debug.WriteLine(TAG + "± files-" + GlobalVars.getInstance().ListOfStrings);
                result = YES;
            }
            else if (base_val.ToLower().Equals("share"))
            {
                String fn;
                if (string_val.Length > base_val.Length + 1) {
                string_val = string_val.Substring(6);
                string_val = normaStr.removeSpaceInBegin(string_val);
                string_val = string_val.Replace("\"","");
                string_val = (string_val.Replace(".bas",""));
                String documentsDirectory = GlobalVars.getInstance().CurrentFolder; // Get documents directory
                String arrayText = "";
                for (int i = 0; i < GlobalVars.getInstance().ListOfProgram.Count; i++)
                    arrayText = arrayText + GlobalVars.getInstance().ListOfProgram[i].ToString() + Environment.NewLine;
                fn = documentsDirectory + string_val;
                bool succeed = false;

                try
                {
                    DirectoryInfo f = new DirectoryInfo(GlobalVars.getInstance().CurrentFolder); 
                    //Check if folder not excist - make new one
                    if (!Directory.Exists(GlobalVars.getInstance().CurrentFolder))
                    {
                        Debug.WriteLine(TAG + "± Make dir " + GlobalVars.getInstance().CurrentFolder);
                        f.Create();
                    }
                    String filepath = Path.Combine(f.ToString(), string_val) + ".bas";  // file path to save
                    using (FileStream fs = new FileStream(filepath, FileMode.OpenOrCreate, FileAccess.Write))
                    {
                        fs.Close();
                        if (File.Exists(fs.Name)) File.Delete(fs.Name);
                    }
                    using (FileStream fs = new FileStream(filepath, FileMode.OpenOrCreate, FileAccess.Write))
                    {
                        using (StreamWriter sw = new StreamWriter(fs))
                        {
                            sw.WriteLine(arrayText);
                            sw.Close();
                        }
                    }
                    GlobalVars.getInstance().FileName = documentsDirectory + "\\" + string_val + ".bas";
                    succeed = YES;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(TAG + "± Exception: " + ex.ToString());
                    succeed = NO;
                }

                Debug.WriteLine(TAG + "± saved - '" + GlobalVars.getInstance().FileName + "'");

                if (!succeed){
                    GlobalVars.getInstance().Error = "Bad file name";
                    // Handle error here
                } else {
                        string file = GlobalVars.getInstance().FileName.Replace('/', '\\');
                        Process.Start(new ProcessStartInfo("explorer.exe", @"/select, " + file));

                        /*Process proc = new Process();
                        proc.StartInfo.FileName = "mailto:?attach=" + @"C:\temp\test.bas";
                        proc.Start();*/

                    }
                    result=NO;
                } else {
                    GlobalVars.getInstance().Error = "Missing operand" + Environment.NewLine;
                } 
            }
            else if (base_val.ToLower().Equals("save"))
            {
                if (string_val.Length > base_val.Length + 1)
                {
                    save(string_val.Substring(5));
                    result = YES;
                }
                else {
                    GlobalVars.getInstance().Error = "Missing operand" + Environment.NewLine;
                }
            }
            else if (base_val.ToLower().Equals("csave"))
            {
                //csave:GlobalVars.getInstance().FileName];
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
            else if (base_val.ToLower().Equals("kill"))
            {
                kill(string_val);
            }
            else if (!digitalFunc.isOnlyDigits(base_val) && variables.isArrayPresent(string_val))
            { // let dim array
                Debug.WriteLine(TAG + "± let to dim '" + string_val + "'");
                result = setDim(string_val);
            }
            else if (!digitalFunc.isOnlyDigits(base_val) && !String.IsNullOrEmpty(base_val))
            { // let var without LET operator
                Debug.WriteLine(TAG + "± let to variable (no let)");
                NSRange range = new NSRange(string_val.IndexOf("="), 1);
                if (range.location != NSNotFound)
                {
                    String afterBase = string_val.Substring(range.location + 1);
                    String before = string_val.Substring(0, range.location);
                    if (GlobalVars.getInstance().KeyScan.Length == 0) GlobalVars.getInstance().KeyScan = "";
                    afterBase = normaStr.removeSpaceInBeginAndEnd(afterBase);
                    if (afterBase.ToLower().Equals("inkey$"))
                    {
                        if (GlobalVars.getInstance().Run) GlobalVars.getInstance().ScanKeyOn = YES;
                        //string_val = before + "=\"" + GlobalVars.getInstance().KeyScan + "=\"";
                        //string_val = inkey().ToString();
                        GlobalVars.getInstance().KeyScan = "";
                    }
                }
                result = let(string_val);
            }
            else if (digitalFunc.isOnlyDigits(base_val) && !base_val.ToLower().Equals(""))
            { // manual program string_val set
                Debug.WriteLine(TAG + "± manual program string_val set at line '" + base_val + "'");
                GlobalVars.getInstance().Variables.Clear();
                programString(string_val, base_val);
                GlobalVars.getInstance().IsOkSet = NO;
            }
            else {
                Debug.WriteLine(TAG + "± command error");
                GlobalVars.getInstance().Command = "";
                if (!base_val.Equals(""))
                {
                    Debug.WriteLine(TAG + "± Syntax error");
                    GlobalVars.getInstance().Error = "Syntax error" + Environment.NewLine;
                }
            }
            Debug.WriteLine(TAG + "± runCommand set over!");
            return result;
        }

        public String inkey()
        {
            String res = "";
            
            do
            {
                res = GlobalVars.getInstance().KeyScan;
            } while (res == "");

            //res = cki.KeyChar.ToString();
            return res;
        }

        public void autoProgramSet(String string_val)
        {
            GlobalVars.getInstance().ListOfStrings.Clear();
            GlobalVars.getInstance().ListOfProgram.Add(string_val);
            GlobalVars.getInstance().ProgramCounter = GlobalVars.getInstance().ProgramCounter + GlobalVars.getInstance().AutoStep;
            GlobalVars.getInstance().ListOfStrings.Add(Environment.NewLine + GlobalVars.getInstance().ProgramCounter + " ");
        }

        public void autoProgramStop()
        {
            GlobalVars.getInstance().ListOfStrings.Clear();
            //GlobalVars.getInstance().ListOfStrings.Add(GlobalVars.getInstance().ProgramCounter + " ");
            GlobalVars.getInstance().ListOfStrings.Add(Environment.NewLine);
            //GlobalVars.getInstance().ListOfProgram.Add(Environment.NewLine);
            GlobalVars.getInstance().AutoSet = NO;
            Debug.WriteLine(TAG + "± autoProgramStop");
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
        GlobalVars.getInstance().Error="Type mismatch" + Environment.NewLine;
        }
        }else{
        if  (var rangeOfCharacterFromSet:numberSet].location != NSNotFound) {
        result=YES;
        GlobalVars.getInstance().Error="Type mismatch" + Environment.NewLine;
        }
        }
        return result;
    }
*/


        public bool setDim(String string_val)
        {
            Debug.WriteLine(TAG + "± set dim..." + string_val);
            bool result = YES;
            if (string_val.Contains("="))
            {
                ArraySet arrayDim = new ArraySet();
                NSRange rangeFirst = new NSRange(string_val.IndexOf("("), 1);
                NSRange rangeSecond = new NSRange(string_val.IndexOf(")"), 1);
                String name = string_val.Substring(0, rangeFirst.location);
                String indexString = string_val.Substring(rangeFirst.location + 1, rangeFirst.length);
                if (variables.variableIsPresent(indexString))
                    indexString = variables.returnContainOfVariable(indexString);
                int index = 0;
                try
                {
                    index = Int32.Parse(indexString);
                }
                catch //(NumberFormatException e)
                {
                    Debug.WriteLine(TAG + "± Wrong number format in setDim!");
                }
                Debug.WriteLine(TAG + "± set dim index='" + string_val.Substring(rangeFirst.location + 1, rangeFirst.length) + "' name='" + name + "'");
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
                        Debug.WriteLine(TAG + "± dim variable is string_val " + value);
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
                        Debug.WriteLine(TAG + "± dim variable is digits " + value);
                        if (String.IsNullOrEmpty(value) || variables.variableIsString(value))
                        {
                            value = "";
                            GlobalVars.getInstance().Error = "Type mismatch" + Environment.NewLine;
                            result = NO;
                        }
                        value = (digitalFunc.returnMathResult(value)).ToString();
                    }
                    for (int i = 0; i < GlobalVars.getInstance().Array.Count; i++)
                    {
                        arrayDim = (ArraySet)GlobalVars.getInstance().Array[i];
                        String str = arrayDim.name;
                        if (str.Equals(name))
                        {
                            arrayDim.value[index] = value;
                            GlobalVars.getInstance().Array[i] = arrayDim;
                        }
                        Debug.WriteLine(TAG + "± dim name='" + name + "' index='" + index + "' value='" + value + "' error='" + GlobalVars.getInstance().Error + "' dim='" + arrayDim.value + "'");
                    }
                }
                else {
                    result = NO;
                    GlobalVars.getInstance().Error = "Syntax error" + Environment.NewLine;
                }
            }
            else {
                result = NO;
                GlobalVars.getInstance().Error = "Syntax error" + Environment.NewLine;
            }
            return result;
        }

        public void loadGlobal()
        {
            /*
        NSString *filePath = GlobalVars.getInstance().FileName;
        //    Debug.WriteLine(TAG + "± load global for - ",filePath);
        if (filePath) {
        reset];
        NSString *arrayText = [NSString stringWithContentsOfFile:filePath encoding:NSUTF8StringEncoding error:nil];
        if (arrayText) {
        GlobalVars.getInstance().ListOfProgram = [NSMutableArray arrayWithArray(arrayText componentsSeparatedByString: Environment.NewLine));
        GlobalVars.getInstance().ProgramCounter = ((self returnBaseCommand:GlobalVars.getInstance().ListOfProgram lastObject))intValue] + GlobalVars.getInstance().AutoStep;
        }
        } else {
        GlobalVars.getInstance().Error = "File not found" + Environment.NewLine;
        }
        */
        }

        public void csave(String string_val)
        {
            /*
        Debug.WriteLine(TAG + "± saving-''",string_val);
        NSError *error;
        String arrayText = GlobalVars.getInstance().ListOfProgram componentsJoinedByString: Environment.NewLine];
        bool succeed = [arrayText writeToFile:string_val atomically:YES encoding:NSUTF8StringEncoding error:&error];
        if (!succeed){
        GlobalVars.getInstance().Error = "Bad file name";
        // Handle error here
        }
        */
        }

        public bool kill(String string_val)
        {
            bool result = NO;

            NormalizeString normaStr = new NormalizeString();
            string_val = normaStr.removeSpaceInBegin(string_val);
            string_val.Replace("\"", "");
            NSRange rangeVal = new NSRange(string_val.IndexOf(' ') + 1, string_val.Length - string_val.IndexOf(' ') - 1);
            string_val = string_val.Substring(rangeVal.location, rangeVal.length);
            string fullPath = GlobalVars.getInstance().CurrentFolder + "\\" + string_val;
            bool success = File.Exists(fullPath);
            if (success)
            {
                File.Delete(fullPath);
                result = YES;
            }
            else
            {
                GlobalVars.getInstance().Error = "File not found" + Environment.NewLine;
                result = NO;
            }

            /*string[] txtList = Directory.GetFiles(GlobalVars.getInstance().CurrentFolder, "*.bas");
            foreach (string f in txtList)
            {
                File.Delete(f);
            }*/



            /*
        normaStr.= ((NormalizeString alloc]init];
        string_val = [normaStr.removeSpaceInBegin:string_val];
        string_val = [string_val.Replace("\"",""];
        string_val = [string_val.Replace(".bas",""];
        string_val = [NSString stringWithFormat:".bas",string_val];
        List<String> paths = NSSearchPathForDirectoriesInDomains(NSDocumentDirectory, NSUserDomainMask, YES);
        NSString *documentsDirectory = [paths[0];
        NSString *filePath = [documentsDirectory stringByAppendingPathComponent:string_val];
        Debug.WriteLine(TAG + "± kill for - ",filePath);

        NSFileManager *fileManager = [NSFileManager defaultManager];
        NSError *error;
        bool success = [fileManager removeItemAtPath:filePath error:&error];
        if (!success) {
        GlobalVars.getInstance().Error = "File not found" + Environment.NewLine;
        result=NO;
        }
        */
            return result;
        }

        public void scanKeyOff()
        {
            GlobalVars.getInstance().ScanKeyOn = NO;
        }

        public void programString(String string_val, String number)
        {
            bool isReplace = NO;
            int indexForReplace = 0;
            int indexForInsert = 0;
            int append = 0;
            GlobalVars.getInstance().ListOfStrings.Clear();
            for (int i = 0; i < GlobalVars.getInstance().ListOfProgram.Count; i++)
            {
                String currentLineNimber = GlobalVars.getInstance().ListOfProgram[i].ToString().Split(' ')[0];
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
                    Debug.WriteLine(TAG + "± Wrong number format in programString!");
                }
            }
            if (string_val.Length > number.Length + 1)
            {
                if (isReplace)
                {
                    GlobalVars.getInstance().ListOfProgram[indexForReplace] = string_val;
                }
                else {
                    GlobalVars.getInstance().ListOfProgram.Insert(indexForInsert + append, string_val);
                }
                GlobalVars.getInstance().ProgramCounter = GlobalVars.getInstance().ProgramCounter + GlobalVars.getInstance().AutoStep;
                GlobalVars.getInstance().ListOfStrings.Add("\n" + GlobalVars.getInstance().ProgramCounter + " ");
            }
            else {
                GlobalVars.getInstance().Error = "Undefined line number" + Environment.NewLine;
            }
        }

        public void reset()
        {
            GlobalVars.getInstance().LineNumber = 0;
            GlobalVars.getInstance().Data.Clear();
            GlobalVars.getInstance().Array.Clear();
            GlobalVars.getInstance().Variables.Clear();
            GlobalVars.getInstance().ListOfProgram.Clear();
            GlobalVars.getInstance().ListOfStrings.Clear();
            GlobalVars.getInstance().GosubArray.Clear();
            GlobalVars.getInstance().ForArray.Clear();

            GlobalVars.getInstance().AutoSet = NO;
            GlobalVars.getInstance().AutoStep = 10;
            GlobalVars.getInstance().ProgramCounter = 10;
            GlobalVars.getInstance().CommandIf = "";
            GlobalVars.getInstance().Error = "";
            GlobalVars.getInstance().Input = "";
            GlobalVars.getInstance().RunnedLine = "";
            GlobalVars.getInstance().DataReadIndex = 0;
            GlobalVars.getInstance().IsOkSet = YES;
            GlobalVars.getInstance().Run = NO;
            GlobalVars.getInstance().ShowError = NO;
            GlobalVars.getInstance().ScanKeyOn = NO;
            GlobalVars.getInstance().KeyScan = "";
        }

        public void save(String string_val)
        {
            Debug.WriteLine(TAG + "± saving-" + string_val);
            
            string_val = normaStr.removeSpaceInBegin(string_val);
            string_val = string_val.Replace("\"", "");
            string_val = string_val.Replace(".bas", "");
            string_val = string_val + ".bas";
            String documentsDirectory = GlobalVars.getInstance().CurrentFolder; // Get documents directory
            String arrayText = "";
            for (int i = 0; i < GlobalVars.getInstance().ListOfProgram.Count; i++)
                arrayText = arrayText + GlobalVars.getInstance().ListOfProgram[i].ToString() + Environment.NewLine;
            try
            {
                DirectoryInfo f = new DirectoryInfo(GlobalVars.getInstance().CurrentFolder); //Check if folder not excist - make new one

                if (!Directory.Exists(GlobalVars.getInstance().CurrentFolder))
                {
                    Debug.WriteLine(TAG + "± Make dir " + GlobalVars.getInstance().CurrentFolder);
                    f.Create();
                }

                String filepath = Path.Combine(f.ToString(), string_val);  // file path to save

                using (FileStream fs = new FileStream(filepath, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    fs.Close();
                    if (File.Exists(fs.Name)) File.Delete(fs.Name);
                }

                using (FileStream fs = new FileStream(filepath, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        sw.WriteLine(arrayText);
                        sw.Close();
                    }
                }

                GlobalVars.getInstance().FileName = documentsDirectory + "/" + string_val;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(TAG + "± Exception: " + ex.ToString());
            }
            Debug.WriteLine(TAG + "± saved - '" + GlobalVars.getInstance().FileName + "'");
        }

        public void scanData()
        {
            GlobalVars.getInstance().DataReadIndex = 0;
            GlobalVars.getInstance().Data.Clear();
            GlobalVars.getInstance().DataIndex.Clear();
            //    Debug.WriteLine(TAG + "± SCANNING...");
            String separator = "\"";
            for (int n = 0; n < GlobalVars.getInstance().ListOfProgram.Count; n++)
            {
                String currentLine = GlobalVars.getInstance().ListOfProgram[n].ToString();
                if (String.IsNullOrEmpty(currentLine))
                {
                    GlobalVars.getInstance().ListOfProgram.RemoveAt(n);
                    Debug.WriteLine(TAG + "± REMOVING..." + n + " " + currentLine);
                }
            }
            for (int n = 0; n < GlobalVars.getInstance().ListOfProgram.Count; n++)
            {
                String currentLine = GlobalVars.getInstance().ListOfProgram[n].ToString();
                Debug.WriteLine(TAG + "± SCANNING..." + n + " " + currentLine);
                String untilSpace = currentLine.Split(' ')[0];
                GlobalVars.getInstance().RunnedLine = untilSpace;
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
                            for (int i = 0; i < arr.Length; i++)
                            {
                                String stmp = arr[i];
                                GlobalVars.getInstance().DataIndex.Add(untilSpace); //записываем в индекс номер строки data
                                if (stmp.Contains(separator))
                                {
                                    stmp = stmp.Split(Convert.ToChar(separator))[1];
                                    GlobalVars.getInstance().Data.Add(stmp);
                                }
                                else {
                                    GlobalVars.getInstance().Data.Add(stmp);
                                }
                                Debug.WriteLine(TAG + "± SET DATA ''" + stmp);
                            }
                        }
                        else {
                            String stmp = string_val.Substring(5);
                            GlobalVars.getInstance().DataIndex.Add(untilSpace); //записываем в индекс номер строки data
                            if (stmp.Contains(separator))
                            {
                                GlobalVars.getInstance().Data.Add(stmp.Split(Convert.ToChar(separator))[1]);
                            }
                            else {
                                GlobalVars.getInstance().Data.Add(stmp);
                            }
                            Debug.WriteLine(TAG + "± SET DATA ''" + stmp);
                        }
                    }
                    else {
                        GlobalVars.getInstance().Error = "Missing operand" + Environment.NewLine;
                    }
                }
            }
            Debug.WriteLine(TAG + "± SCANNING OVER " + GlobalVars.getInstance().DataIndex);
        }

        public bool let(String string_val)
        {
            bool result = NO;
            if (string_val.Contains("="))
            {
                String value = resultFromString(variables.returnVarValue(string_val));//присваиваем результат рассчетов к переменной
                String varName = variables.returnVarName(string_val);
                int index = variables.makeVariableIndex(varName);
                if (String.IsNullOrEmpty(GlobalVars.getInstance().Error))
                {
                    if (!variables.forbiddenVariable(varName) && varName.Length > 0)
                    {
                        if (index == GlobalVars.getInstance().Variables.Count)
                        {
                            GlobalVars.getInstance().Variables.Add(addVariable(value, varName));
                            Debug.WriteLine(TAG + "± let new var - " + varName + " value=" + value);
                            Debug.WriteLine(TAG + "± let new var - " + GlobalVars.getInstance().Variables[index].getName().ToString() + " value=" + GlobalVars.getInstance().Variables[index].getVar().ToString());
                        }
                        else {
                            GlobalVars.getInstance().Variables[index] = addVariable(value, varName);
                            Debug.WriteLine(TAG + "± let exist var - " + varName + " value=" + value);
                        }
                        if (String.IsNullOrEmpty(GlobalVars.getInstance().Error)) result = YES;
                    }
                    else {
                        GlobalVars.getInstance().Command = "";
                        GlobalVars.getInstance().Error = "Forbidden variable" + Environment.NewLine;
                        result = NO;
                        Debug.WriteLine(TAG + "± let empty Forbidden variable");
                    }
                }
            }
            else {
                GlobalVars.getInstance().Error = "Syntax error" + Environment.NewLine;
                Debug.WriteLine(TAG + "± let Syntax error");
            }
            // for (int i = 0; i < GlobalVars.getInstance().Variables.Count; i++)
            /*Debug.WriteLine(TAG + "± let varName - " + GlobalVars.getInstance().Variables[i].getName()
                    + " value=" + GlobalVars.getInstance().Variables[i].getVar() + " is String type=" + GlobalVars.getInstance().Variables[i].getStringType());
            */
            return result;
        }

        public bool print(String string_val)
        {
            Debug.WriteLine(TAG + "± PRINT init ......''" + string_val);
            String cr = Environment.NewLine;
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
            if (string_val == "pos")
            {
                GlobalVars.getInstance().ListOfStrings.Add(GlobalVars.getInstance().Command.Length.ToString() + cr);
                result = YES;
            } else printResult = string_val + cr;
            GlobalVars.getInstance().ListOfStrings.Add(printResult);
            if (String.IsNullOrEmpty(GlobalVars.getInstance().Error)) result = YES;
            Debug.WriteLine(TAG + "± PRINT result '" + printResult + "' error-" + GlobalVars.getInstance().Error);
            return result;
        }

        public bool initDim(String string_val)
        {
            bool result = YES;
            Debug.WriteLine(TAG + "± initDim... " + string_val);
            List<String> arr = new List<String>(string_val.Split(',').ToList());
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
                    size = string_val.Substring(rangeFirst.location + 1, rangeSecond.location-(rangeFirst.location + 1));
                    size = resultFromString(size);
                    if (!variables.forbiddenVariable(name) && name.Length > 0 && String.IsNullOrEmpty(GlobalVars.getInstance().Error))
                    {
                        int tmp = 0;
                        try
                        {
                            tmp = (int)float.Parse(size);
                        }
                        catch //(NumberFormatException e)
                        {
                            Debug.WriteLine(TAG + "± Wrong number format in initDim!");
                        }
                        variables.initArrayNameWithSize(name, tmp);
                        Debug.WriteLine(TAG + "± dim name - '" + name + "' size = " + size);
                    }
                    else {
                        GlobalVars.getInstance().Command = "";
                        GlobalVars.getInstance().Error = "Wrong dim variable name" + Environment.NewLine;
                        result = NO;
                    }
                }
                else {
                    GlobalVars.getInstance().Command = "";
                    GlobalVars.getInstance().Error = "Wrong dim variable name" + Environment.NewLine;
                    result = NO;
                }
                String testValue = name.Replace(" ", "");
                if (testValue.Equals(""))
                {
                    GlobalVars.getInstance().Command = "";
                    GlobalVars.getInstance().Error = "Wrong dim variable name" + Environment.NewLine;
                    result = NO;
                }
            }
            Debug.WriteLine(TAG + "± initDim result - '" + result + "'");
            return result;
        }

        public int returnIndexFromLine(String line)
        {
            int result = -2;
            for (int i = 0; i < GlobalVars.getInstance().ListOfProgram.Count; i++)
            {
                String currentLine = GlobalVars.getInstance().ListOfProgram[i].ToString().Split(' ')[0];
                if (currentLine.Equals(line))
                {
                    result = i - 1;
                    Debug.WriteLine(TAG + "± returnIndexFromLine currentLine=" + currentLine + "   String line=" + line + " result=" + result);
                }
            }
            Debug.WriteLine(TAG + "± returnIndexFromLine - " + result);
            return result;
        }

        public VariableSet addVariable(String var, String name)
        {
            bool strType = NO;
            //    Debug.WriteLine(TAG + "± addVariable+ is string_val ",var);
            var = normaStr.removeSpaceInBeginAndEnd(var);
            if (var.Equals("+")) var = "\"+\"";
            String value = var;
            if (name.Contains("$"))
            {
                Debug.WriteLine(TAG + "± addVariable+variable is string_val " + var);
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
                Debug.WriteLine(TAG + "± variable is digits " + var);
                if (String.IsNullOrEmpty(var) || variables.variableIsString(var))
                {
                    value = "0";
                    GlobalVars.getInstance().Error = "Type mismatch" + Environment.NewLine;
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

            Debug.WriteLine(TAG + "± IF operator '=' " + ifValue);

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
                    Debug.WriteLine(TAG + "± Syntax error in IF operator");
                    GlobalVars.getInstance().Error = "Type mismatch in IF operator" + Environment.NewLine;
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
                    Debug.WriteLine(TAG + "± Syntax error in IF operator");
                    GlobalVars.getInstance().Error = "Type mismatch in IF operator" + Environment.NewLine;
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
                    Debug.WriteLine(TAG + "± Syntax error in IF operator");
                    GlobalVars.getInstance().Error = "Type mismatch in IF operator" + Environment.NewLine;
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
                    Debug.WriteLine(TAG + "± Syntax error in IF operator");
                    GlobalVars.getInstance().Error = "Type mismatch in IF operator" + Environment.NewLine;
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
                    Debug.WriteLine(TAG + "± Syntax error in IF operator");
                    GlobalVars.getInstance().Error = "Type mismatch in IF operator" + Environment.NewLine;
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
                    Debug.WriteLine(TAG + "± Syntax error in IF operator");
                    GlobalVars.getInstance().Error = "Type mismatch in IF operator" + Environment.NewLine;
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
                        Debug.WriteLine(TAG + "± (digit)IF " + ifFirst + " == " + ifSecond);
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
                    Debug.WriteLine(TAG + "± (digit)IF " + ifFirst + " EQUAL " + ifSecond);
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
                Debug.WriteLine(TAG + "± Syntax error in IF operator for all!");
                GlobalVars.getInstance().Error = "Syntax error in IF operator" + Environment.NewLine;
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
            Debug.WriteLine(TAG + "± set dim..." + string_val);
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
                    Debug.WriteLine(TAG + "± Wrong number format in setDim!");
                }
                Debug.WriteLine(TAG + "± set dim index='" + string_val.Substring(rangeFirst.location + 1, rangeSecond.location) + "' name='" + name + "'");
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
                        Debug.WriteLine(TAG + "± dim variable is string_val " + value);
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
                        Debug.WriteLine(TAG + "± dim variable is digits " + value);
                        if (String.IsNullOrEmpty(value) || variables.variableIsString(value))
                        {
                            value = "";
                            GlobalVars.getInstance().Error = "Type mismatch" + Environment.NewLine;
                            result = NO;
                        }
                        value = (digitalFunc.returnMathResult(value)).ToString();
                    }

                    for (int i = 0; i < GlobalVars.getInstance().Array.Count; i++)
                    {
                        arrayDim = (ArraySet)GlobalVars.getInstance().Array[i];
                        String str = arrayDim.name;
                        if (str.Equals(name))
                        {
                            arrayDim.value[index] = value;
                    GlobalVars.getInstance().Array[i] = arrayDim;
                }
                Debug.WriteLine(TAG + "± dim name='" + name + "' index='" + index + "' value='" + value + "' error='" + GlobalVars.getInstance().Error + "' dim='" + arrayDim.value + "'");
            }
        }
        else {
            result = NO;
            GlobalVars.getInstance().Error = "Syntax error" + Environment.NewLine;
        }
}
    else {
        result = NO;
        GlobalVars.getInstance().Error = "Syntax error" + Environment.NewLine;
    }
    return result;
}*/


        public String returnBaseCommand(String string_val)
        {
            String result = string_val.Split(' ')[0];
            for (int i = 0; i < GlobalVars.getInstance().ListOfAll.Count; i++)
            {
                int index = string_val.ToLower().IndexOf(GlobalVars.getInstance().ListOfAll[i].ToString());
                if (index != NSNotFound && index == 0)
                    result = GlobalVars.getInstance().ListOfAll[i].ToString();
            }
            return result;
        }

        public String input(String string_val)
        {
            String result = "";
            Debug.WriteLine(TAG + "± input -'" + string_val + "'");

            GlobalVars.getInstance().ScanKeyOn = NO;
            string_val = string_val.Replace(";", ",");
            String alphaSet = "[^a-zA-Z]+";//если только НЕ буквы
            String separator = "\"";
            string_val = string_val.Substring(5);
            String until = " ";
            if (string_val.Contains(separator))
                until = separator + normaStr.extractTextToArray(string_val)[0].ToString() + separator;
            GlobalVars.getInstance().Error = "";
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
                GlobalVars.getInstance().Error = "Syntax error" + Environment.NewLine;
                Debug.WriteLine(TAG + "± input Syntax error");
                GlobalVars.getInstance().IsOkSet = NO;
            }
            else if (strings && !stringsVariable && !digits && !digitsVariable && string_val.Contains(","))
            {
                result = string_val.Substring(index);
                result = result.Replace(" ", "");
                if (!result.Contains(",")) result = "," + result;
                until = until.Replace(separator, "");
                GlobalVars.getInstance().ListOfStrings.Add(until);
                variables.forbiddenVariable(result.Substring(1));
                Debug.WriteLine(TAG + "± input string_val result - " + result);
            }
            else {
                Debug.WriteLine(TAG + "± input Str-'" + string_val + "'");
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
                Debug.WriteLine(TAG + "± input result -'" + result + "'");
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
            Debug.WriteLine(TAG + "± ========resultFromString begin-'" + string_val + "'");
            //очищаем от пробелов
            string_val = normaStr.removeSpacesWithText(string_val);
            //заменяем ; на ,
            string_val = normaStr.replaceCharWithCharInText(';', ',', string_val);
            Debug.WriteLine(TAG + "± ========resultFromString replaceChar:';' withChar:',' ---- '" + string_val + "'");
            List<String> arr = new List<String>(normaStr.stringSeparateAllToArray(string_val)); //делим строку на компоненты
            Debug.WriteLine(TAG + "± ========stringSeparateAllToArray " + arr);
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
            for (int i = 0; i < arr.Count; i++)
            {
                string_val = string_val + arr[i].ToString(); //склеиваем строку из массива
            }
            arr.Clear();
            Debug.WriteLine(TAG + "± ========resultFromString after Replace vars - '" + string_val + "'");
            //Находим массивы и подменяем
            String forDim = "";
            bool addMode = NO;
            List<String> arrTemp = new List<String>(normaStr.stringSeparateAllToArray(string_val)); //делим строку на компоненты
            if (arrTemp.Count > 3)
            {
                for (int i = 1; i < arrTemp.Count; i++)
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
            for (int i = 0; i < arr.Count; i++)
            {
                string_val = string_val + arr[i].ToString(); //склеиваем строку из массива
            }
            arr.Clear();
            arrTemp.Clear();
            Debug.WriteLine(TAG + "± ========resultFromString after Replace dims - '" + string_val + "'");
            //склеиваем все строки стоящие рядом
            String fStr = "";
            String tStr = "";
            bool firstEl = NO;
            bool secondEl = NO;
            int index = 0;
            arr = new List<String>(normaStr.stringSeparateAllToArray(string_val)); //делим строку на компоненты
                                                                                   //    Debug.WriteLine(TAG + "± resultFromString arr-''",arr);
            if (arr.Count > 3 && !string_val.Contains("instr"))
            {
                string_val = "";
                for (int i = 0; i < arr.Count; i++)
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
            for (int i = 0; i < arrTemp.Count; i++)
            {
                string_val = string_val + arrTemp[i].ToString(); //склеиваем строку из массива
            }
            arr.Clear();
            arrTemp.Clear();
            Debug.WriteLine(TAG + "± ========resultFromString after strings-'" + string_val + "'");

            //Находим все мат функции и внутри них анализируем на наличие арифметики затем рассчитываем и подменяем
            String forMFunc = "";
            int addCount = 0;
            bool first = NO;
            addMode = NO;
            arrTemp = new List<String>(normaStr.stringSeparateAllToArray(string_val)); //делим строку на компоненты

            if (arrTemp.Count > 3)
            {
                for (int i = 1; i < arrTemp.Count; i++)
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
                                        String temp = digitalFunc.mathFunctionInMixedString(forMFunc.Substring(4, forMFunc.Length - 5));
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
                Debug.WriteLine(TAG + "± ========!!else++++++++++++" + string_val + "'");
                arr = new List<String>(arrTemp);
            }
            string_val = "";

            for (int i = 0; i < arr.Count; i++)
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
            Debug.WriteLine(TAG + "± ========resultFromString after mathematics & string_val func's-'" + string_val + "'");

            //вычисляем каждый компонент математики
            if (string_val.Length > 0)
                string_val = digitalFunc.mathFunctionInMixedString(string_val); //1. mathFunctionInMixedString
            Debug.WriteLine(TAG + "± ========resultFromString mathFunctionInMixedString-'" + string_val + "'");
            //избавляемся от запятых и с плюсов, склеиваем строку из компонентов
            arr = new List<String>(normaStr.stringSeparateAllToArray(string_val)); //делим строку на компоненты
            bool quotes = NO;
            for (int i = 0; i < arr.Count; i++)
            {
                if (normaStr.isText(arr[i].ToString())) quotes = YES;
                if (!arr[i].ToString().Equals(","))
                {
                    if (!arr[i].ToString().Equals("+"))
                        result = result + arr[i].ToString(); //склеиваем строку из массива
                } else {
                    if (!string_val.Contains("\"")) result = result + arr[i].ToString(); //for float numbers
                }
            }
            if (quotes)
            {
                result = result.Replace("\"", "");
                result = "\"" + result + "\"";
            }
            Debug.WriteLine(TAG + "± ========resultFromString FINAL RESULT-'" + result + "'");
            return result;
        }

        public void renumGotoGosub()
        {
            List<String> oldList = new List<String>(GlobalVars.getInstance().ListOfProgram);
            String replaceString;
            String number;
            if (GlobalVars.getInstance().ListOfProgram.Count > 0)
            {
                String gotoGosub = "goto";
                int c = 1;
                for (int i = 0; i < GlobalVars.getInstance().ListOfProgram.Count; i++)
                {
                    number = GlobalVars.getInstance().ListOfProgram[i].ToString().Split(' ')[0];
                    replaceString = GlobalVars.getInstance().ListOfProgram[i].ToString();
                    replaceString = c * 10 + " " + replaceString.Substring(number.Length + 1);
                    GlobalVars.getInstance().ListOfProgram[i] = replaceString;
                    c++;
                }
                for (int i = 0; i < GlobalVars.getInstance().ListOfProgram.Count; i++)
                {
                    replaceString = GlobalVars.getInstance().ListOfProgram[i].ToString();
                    Debug.WriteLine(TAG + "± before number=" + i + " replaceString='" + replaceString + "'");
                    NSRange range = new NSRange(replaceString.IndexOf(gotoGosub), gotoGosub.Length);
                    String[] tempArr = Regex.Split(replaceString, gotoGosub);
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
                                    Debug.WriteLine(TAG + "± '" + replaceString.ElementAt(t) + "'");
                                    index = t;
                                    found = YES;
                                }
                            String first = GlobalVars.getInstance().ListOfProgram[i].ToString().Substring(0, locat);
                            String whatRepl = replaceString.Substring(locat, index);
                            String second = GlobalVars.getInstance().ListOfProgram[i].ToString().Substring(index);
                            int renIndex = returnIndexOf(oldList, whatRepl);
                            String withRepl = GlobalVars.getInstance().ListOfProgram[renIndex].ToString().Split(' ')[0];
                            String repStr = first + withRepl + second;
                            GlobalVars.getInstance().ListOfProgram[i] = repStr;
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
                for (int i = 0; i < GlobalVars.getInstance().ListOfProgram.Count; i++)
                {
                    replaceString = GlobalVars.getInstance().ListOfProgram[i].ToString();
                    NSRange range = new NSRange(replaceString.IndexOf(gotoGosub), gotoGosub.Length);
                    String[] tempArr = Regex.Split(replaceString, gotoGosub);
                    c = 0;
                    int locat = 0;
                    try { locat = (int)(range.location + range.length + 1); } catch { }
                    bool found = NO;
                    int index = 0;
                    for (int cc = 1; cc < tempArr.Length; cc++)
                        if (range.location != NSNotFound && !normaStr.insideText(replaceString, locat))
                        {
                            index = (int)replaceString.Length;
                            for (int t = locat; t < replaceString.Length; t++)
                                if (replaceString.ElementAt(t) == ' ' && !found)
                                {
                                    Debug.WriteLine(TAG + "± '" + replaceString.ElementAt(t) + "'");
                                    index = t;
                                    found = YES;
                                }
                            String first = GlobalVars.getInstance().ListOfProgram[i].ToString().Substring(0, locat);
                            String whatRepl = replaceString.Substring(locat, index);
                            String second = GlobalVars.getInstance().ListOfProgram[i].ToString().Substring(index);
                            int renIndex = returnIndexOf(oldList, whatRepl);
                            String withRepl = GlobalVars.getInstance().ListOfProgram[renIndex].ToString().Split(' ')[0];
                            String repStr = first + withRepl + second;
                            GlobalVars.getInstance().ListOfProgram[i] = repStr;
                            if (!String.IsNullOrEmpty(whatRepl)) GlobalVars.getInstance().ListOfProgram[i] = repStr;
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
                for (int i = 0; i < GlobalVars.getInstance().ListOfProgram.Count; i++)
                {
                    replaceString = GlobalVars.getInstance().ListOfProgram[i].ToString();
                    NSRange range = new NSRange(replaceString.IndexOf(gotoGosub), gotoGosub.Length);
                    String[] tempArr = Regex.Split(replaceString, gotoGosub);
                    c = 0;
                    int locat = (int)(range.location + range.length + 1);
                    bool found = NO;
                    int index = 0;
                    for (int cc = 1; cc < tempArr.Length; cc++)
                        if (range.location != NSNotFound && !normaStr.insideText(replaceString, locat))
                        {
                            index = (int)replaceString.Length;
                            for (int t = locat; t < replaceString.Length; t++)
                                if (replaceString.ElementAt(t) == ' ' && !found)
                                {
                                    Debug.WriteLine(TAG + "± '" + replaceString.ElementAt(t) + "'");
                                    index = t;
                                    found = YES;
                                }
                            String first = GlobalVars.getInstance().ListOfProgram[i].ToString().Substring(0, locat);
                            String whatRepl = replaceString.Substring(locat, index);
                            String second = GlobalVars.getInstance().ListOfProgram[i].ToString().Substring(index);
                            int renIndex = returnIndexOf(oldList, whatRepl);
                            String withRepl = GlobalVars.getInstance().ListOfProgram[renIndex].ToString().Split(' ')[0];
                            String repStr = first + withRepl + second;
                            GlobalVars.getInstance().ListOfProgram[i] = repStr;

                            if (!String.IsNullOrEmpty(whatRepl)) GlobalVars.getInstance().ListOfProgram[i] = repStr;
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
