package com.neronov.aleksei.mcxbasic;

import android.support.v7.app.AppCompatActivity;
import android.util.Log;

import java.io.File;
import java.io.FileWriter;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.Collections;
import java.util.Comparator;
import java.util.List;

import static com.neronov.aleksei.mcxbasic.GlobalVars.getInstance;

/**
 * Created by Aleksei Neronov on 12.03.16.
 */
public class RunCommand extends AppCompatActivity {
    private static final String TAG = MainActivity.class.getSimpleName();
    private static final int NSNotFound = -1;
    private static final boolean NO = false;
    private static final boolean YES = true;
    private static DigitalFunc digitalFunc = new DigitalFunc();
    private static NormalizeString normaStr = new NormalizeString();
    private static Variables variables = new Variables();
    private static StringFunc stringFunc = new StringFunc();

    public boolean set(String string) {
        boolean result = false;
        if (string.substring(0, 1).equals(" "))
            string = normaStr.removeSpaceInBegin(string).toLowerCase();
        String separator = "\"";
        String base = returnBaseCommand(string);
        Log.d(TAG, "± RunCommand..." + string);
        getInstance().setError("");
        getInstance().listOfStrings.clear();

        if (base.equalsIgnoreCase("ver")) {
            Log.d(TAG, "± " + GlobalVars.version1 + BuildConfig.VERSION_NAME + "\n");
            getInstance().listOfStrings.add(GlobalVars.version1 + BuildConfig.VERSION_NAME + "\n");
            getInstance().listOfStrings.add(GlobalVars.version2);
            getInstance().listOfStrings.add("\n");
            getInstance().listOfStrings.add(GlobalVars.version3);
            result = true;
        } else if (base.equalsIgnoreCase("auto")) {
            // Log.d(TAG, "± autoSet = YES");
            getInstance().setAutoSet(true);
            getInstance().setIsOkSet(false);
            getInstance().listOfStrings.add(String.valueOf(getInstance().getProgramCounter()) + " ");
            result = true;
        } else if (base.equalsIgnoreCase("list")) {
            getInstance().setIsOkSet(true);
            String listNumber;
            String currentLineNumber;
            string = string.substring(4, string.length());
            string = string.trim();
            if (!string.isEmpty())
                if (string.contains("-")) {
                    String[] arr = string.split("-");
                    int begin = Integer.parseInt(arr[0]);
                    int end = Integer.parseInt(arr[1]);
                    for (int li = begin; li <= end; li++) {
                        listNumber = String.valueOf(li);
                        if (digitalFunc.isOnlyDigits(listNumber)) {
                            for (int i = 0; i < getInstance().getListOfProgram().size(); i++) {
                                currentLineNumber = getInstance().getListOfProgram().get(i).toString();
                                currentLineNumber = currentLineNumber.split(" ")[0];
                                if (currentLineNumber.equalsIgnoreCase(listNumber)) {
                                    getInstance().listOfStrings.add(String.valueOf(getInstance().getListOfProgram().get(i).toString() + "\n"));
                                }
                            }
                            result = true;
                        } else {
                            getInstance().error = "Syntax error\n";
                        }
                    }
                } else {
                    listNumber = string;
                    if (digitalFunc.isOnlyDigits(listNumber)) {
                        for (int i = 0; i < getInstance().getListOfProgram().size(); i++) {
                            currentLineNumber = getInstance().getListOfProgram().get(i).toString();
                            currentLineNumber = currentLineNumber.split(" ")[0];
                            if (currentLineNumber.equalsIgnoreCase(listNumber)) {
                                getInstance().listOfStrings.add(String.valueOf(getInstance().getListOfProgram().get(i).toString() + "\n"));
                            }
                        }
                        result = true;
                    } else {
                        getInstance().error = "Syntax error\n";
                    }
                }
            if (string.isEmpty())
                for (int i = 0; i < getInstance().getListOfProgram().size(); i++) {
                    getInstance().listOfStrings.add(String.valueOf(getInstance().getListOfProgram().get(i).toString() + "\n"));
                }
            result = true;
        } else if (base.equalsIgnoreCase("help")) {
            Log.d(TAG, "± help");
            for (int i = 0; i < getInstance().getListOfCommands().size(); i++) {
                getInstance().listOfStrings.add(String.valueOf(getInstance().getListOfCommands().get(i).toString() + "\n"));
            }
            getInstance().listOfStrings.add(String.valueOf("\n"));
            result = true;
        } else if (base.equalsIgnoreCase("clear")) {
            getInstance().variables.clear();
            result = YES;
        } else if (base.equalsIgnoreCase("cls")) {
            result = NO;
        } else if (base.equalsIgnoreCase("end")) {
            Log.d(TAG, "± end");
            getInstance().run = false;
            getInstance().isOkSet = true;
            result = true;
        } else if (base.equalsIgnoreCase("beep")) {
            Log.d(TAG, "± Beep begin ...");
            result = NO;
        } else if (base.equalsIgnoreCase("color")) {
            result = NO;
        } else if (base.equalsIgnoreCase("input")) {
            getInstance().input = input(string);
            result = NO;
        } else if (base.equalsIgnoreCase("data")) {
            result = YES;
        } else if (base.equalsIgnoreCase("delete")) {
            if (string.length() > base.length() + 1) {
                String listNimber;
                String currentLineNimber;
                ArrayList toDelete = new ArrayList();
                string = (string.substring(7).replace(" ", ""));
                if (string.contains("-")) {
                    int begin = 0;
                    int end = 0;
                    String[] arr = string.split("-");
                    try {
                        begin = Integer.parseInt(arr[0]);
                        end = Integer.parseInt(arr[1]);
                    } catch (NumberFormatException e) {
                        Log.d(TAG, "± Wrong number format in delete(RunCommand)!");
                    }
                    for (int li = begin; li <= end; li++) {
                        listNimber = String.valueOf(li);
                        if (digitalFunc.isOnlyDigits(listNimber)) {
                            for (int i = 0; i < getInstance().listOfProgram.size(); i++) {
                                currentLineNimber = getInstance().listOfProgram.get(i).toString().split(" ")[0];
                                if (currentLineNimber.equals(listNimber))
                                    toDelete.add(getInstance().listOfProgram.get(i).toString());
                            }
                            for (int i = 0; i < toDelete.size(); i++) {
                                getInstance().listOfProgram.remove(toDelete.get(i));
                            }
                            result = YES;
                        } else {
                            getInstance().error = "Syntax error\n";
                        }
                    }
                } else {
                    listNimber = string;
                    if (digitalFunc.isOnlyDigits(listNimber)) {
                        for (int i = 0; i < getInstance().listOfProgram.size(); i++) {
                            currentLineNimber = getInstance().listOfProgram.get(i).toString().split(" ")[0];
                            if (currentLineNimber.equals(listNimber)) {
                                toDelete.add(getInstance().listOfProgram.get(i));
                            }
                        }
                        for (int i = 0; i < toDelete.size(); i++) {
                            getInstance().listOfProgram.remove(toDelete.get(i));
                        }
                        result = YES;
                    } else {
                        getInstance().error = "Syntax error\n";
                    }
                }
            } else {
                getInstance().error = "Missing operand\n";
            }
        } else if (base.equalsIgnoreCase("restore")) {
            getInstance().isOkSet = YES;
            String listNumber = string.substring(0, 7).replace(" ", "");
            listNumber = resultFromString(listNumber);
            if (listNumber.isEmpty()) {
                getInstance().dataReadIndex = 0;
                result = YES;
            } else {
                if (digitalFunc.isOnlyDigits(listNumber)) {
                    try {
                        getInstance().dataReadIndex = Integer.parseInt(getInstance().dataIndex.get(Integer.parseInt(listNumber)).toString());
                    } catch (NumberFormatException e) {
                        Log.d(TAG, "± Wrong number format in restore!");
                    }
                    result = YES;
                } else {
                    getInstance().error = "Syntax error\n";
                }
                Log.d(TAG, "restore getInstance().dataReadIndex=" + getInstance().dataReadIndex);
            }
        } else if (base.equalsIgnoreCase("renum")) {
            renumGotoGosub();
            result = YES;
        } else if (base.equalsIgnoreCase("if")) {
            if (string.length() > base.length() + 1) {
                getInstance().commandIf = "";
                // NSCharacterSet * equalSet = [NSCharacterSet characterSetWithCharactersInString:"=<>"];.matches("[a-zA-Z]+")) { //если только буквы
                String ifValue = normaStr.removeSpaceInBegin(string.substring(3));
                String thenValue;
                String elseValue = "";
                NSRange rangeThen = new NSRange(ifValue.indexOf("then"), 4);
                //Log.d(TAG, "± !!! IF operator contain [=<>] "+ifValue.substring(0, rangeThen.location)+" "+ifValue.substring(0, rangeThen.location).matches(".*[=<>]+.*"));
                if (rangeThen.location == NSNotFound || !ifValue.substring(0, rangeThen.location).matches(".*[=<>]+.*")) {
                    Log.d(TAG, "± Syntax error in IF operator");
                    getInstance().error = "Syntax error in IF operator\n";
                } else {
                    ifValue = normaStr.removeSpaceInBeginAndEnd(ifValue.substring(0, rangeThen.location));
                    rangeThen = new NSRange(string.indexOf("then"), 4);
                    NSRange rangeElse = new NSRange(string.indexOf("else"), 4);
                    if (rangeElse.location == NSNotFound) {
                        thenValue = normaStr.removeSpaceInBeginAndEnd(string.substring(rangeThen.location + 4));
                    } else {
                        elseValue = normaStr.removeSpaceInBeginAndEnd(string.substring(rangeElse.location + 4));
                        thenValue = normaStr.removeSpaceInBeginAndEnd(string.substring(rangeThen.location + 4, rangeElse.location));
                    }

                    if (ifThen(ifValue)) {  //  Вызываем метод проверки IF
                        getInstance().commandIf = thenValue;
                    } else {
                        getInstance().commandIf = elseValue;
                    }
                }
            } else {
                getInstance().error = "Missing operand\n";
            }
        } else if (base.equalsIgnoreCase("for")) {
            if (string.length() > base.length() + 1) {
                ForSet forSet = new ForSet();
                String forValue = normaStr.removeSpaceInBegin(string.substring(4));
                String toValue = "";
                String stepValue = "";
                NSRange rangeTo = new NSRange(forValue.indexOf("to"), 2);
                if (rangeTo.location == NSNotFound) {
                    Log.d(TAG, "± for without to");
                    getInstance().error = "Syntax error - FOR without TO\n";
                } else {
                    forValue = forValue.substring(0, rangeTo.location).replace(" ", "");
                }
                rangeTo = new NSRange(string.indexOf("to"), 2);
                NSRange rangeStep = new NSRange(string.indexOf("step"), 4);
                if (rangeStep.location == NSNotFound) {
                    Log.d(TAG, "± for without step ");
                    toValue = string.substring(rangeTo.location + 2);
                    toValue = toValue.replace(" ", "");
                    stepValue = "1";
                } else {
                    stepValue = string.substring(rangeStep.location + 4).replace(" ", "");
                    toValue = string.substring(rangeTo.location + 2, rangeStep.location).replace(" ", "");
                }
                Log.d(TAG, "± !!!! forValue='" + forValue + "' toValue='" + toValue + "' stepValue='" + stepValue + "'");
                String tmpFor = resultFromString(variables.returnVarValue(forValue));
                String tmpTo = resultFromString(variables.returnVarValue(toValue));
                String tmpStep = resultFromString(variables.returnVarValue(stepValue));
                Log.d(TAG, "±     forValue=" + forValue + " toValue=" + toValue + " stepValue=" + stepValue);
                Log.d(TAG, "± TMP forValue=" + tmpFor + " toValue=" + tmpTo + " stepValue=" + tmpStep);
                String varName = variables.returnVarName(forValue);
                forSet.forLine = getInstance().runnedLine;
                forSet.forName = varName;
                forSet.forStep = tmpStep;
                forSet.forTo = tmpTo;
                getInstance().forArray.add(forSet);
                int index = variables.makeVariableIndex(varName);
                String value = tmpFor;
                if (!variables.forbiddenVariable(varName) && varName.length() > 0 && getInstance().error.isEmpty()) {
                    if (index == getInstance().variables.size()) {
                        getInstance().variables.add(addVariable(value, varName));
                    } else {
                        getInstance().variables.set(index, addVariable(value, varName));
                    }
                    result = YES;
                } else {
                    getInstance().error = "Syntax error";
                    getInstance().command = "";
                    Log.d(TAG, "± let empty");
                }
                result = YES;
            } else {
                getInstance().error = "Missing operand\n";
            }
        } else if (base.equalsIgnoreCase("next")) {
            if (getInstance().forArray.size() > 0) {
                ForSet forSet = new ForSet();
                forSet = (ForSet) getInstance().forArray.get(getInstance().forArray.size() - 1);
                int forI = 0, ff = 0;
                try {
                    //Log.d(TAG, "± NEXT try int variables.returnContainOfVariable(forSet.forName)=" + variables.returnContainOfVariable(forSet.forName));
                    //Log.d(TAG, "± NEXT try int forSet.forStep=" + forSet.forStep);
                    forI = (int) (Float.parseFloat(variables.returnContainOfVariable(forSet.forName)) + Float.parseFloat(forSet.forStep));
                    ff = (int) Float.parseFloat(forSet.forTo);
                } catch (NumberFormatException e) {
                    Log.d(TAG, "± Wrong number format in NEXT operator!");
                }
                if (forI > ff) {
                    getInstance().forArray.remove(getInstance().forArray.size() - 1);
                } else {
                    getInstance().variables.set(variables.makeVariableIndex(forSet.forName), addVariable(String.valueOf(forI), forSet.forName));
                    getInstance().runIndex = returnIndexFromLine(forSet.forLine) + 1;
                }
                result = YES;
            } else {
                getInstance().error = "NEXT without FOR\n";
            }
        } else if (base.equalsIgnoreCase("goto")) {
            if (string.length() > base.length() + 1) {
                string = (string.substring(5).replace(" ", ""));
                getInstance().runIndex = returnIndexFromLine(string);
                result = YES;
                if (getInstance().runIndex < -1) {
                    getInstance().runIndex = 0;
                    getInstance().error = "Undefined line number\n";
                    result = NO;
                }
            } else {
                getInstance().error = "Missing operand\n";
            }
        } else if (base.equalsIgnoreCase("gosub")) {
            if (string.length() > base.length() + 1) {
                string = (string.substring(6).replace(" ", ""));
                getInstance().gosubArray.add(getInstance().runnedLine);
                getInstance().runIndex = returnIndexFromLine(string);
                result = YES;
                if (getInstance().runIndex < -1) {
                    getInstance().runIndex = 0;
                    getInstance().error = "Undefined line number\n";
                    result = NO;
                }
            } else {
                getInstance().error = "Missing operand\n";
            }
        } else if (base.equalsIgnoreCase("return")) {
            if (getInstance().gosubArray.size() > 0) {
                string = getInstance().gosubArray.get(getInstance().gosubArray.size() - 1).toString();
                getInstance().gosubArray.remove(getInstance().gosubArray.size() - 1);
                Log.d(TAG, "± return - " + string);
                getInstance().runIndex = returnIndexFromLine(string) + 1;
                result = YES;
            } else {
                getInstance().error = "RETURN without GOSUB\n";
            }
        } else if (base.equalsIgnoreCase("rem")) {
            result = YES;
        } else if (base.equalsIgnoreCase("dim")) {
            if (string.length() > base.length() + 1) {
                String clearString = string.substring(4);
                result = initDim(clearString);
            } else {
                getInstance().error = "Missing operand\n";
            }
        } else if (base.equalsIgnoreCase("date")) {
            if (string.length() > base.length() + 1) {
                String clearString = string.substring(5);
                result = variables.getDateToVariable(clearString);
            } else {
                getInstance().error = "Missing operand\n";
            }
        } else if (base.equalsIgnoreCase("time")) {
            if (string.length() > base.length() + 1) {
                String clearString = string.substring(5);
                result = variables.getTimeToVariable(clearString);
            } else {
                getInstance().error = "Missing operand\n";
            }
        } else if (base.equalsIgnoreCase("print")) {
            if (string.length() > base.length() + 1) {
                result = print(string);
            } else {
                getInstance().error = "Missing operand\n";
            }
        } else if (base.equalsIgnoreCase("varl")) {
            for (int i = 0; i < getInstance().variables.size(); i++) {
                VariableSet varSet = getInstance().variables.get(i);
                getInstance().listOfStrings.add(varSet.name + " = " + varSet.var + "\n");
            }
            result = YES;
        } else if (base.equalsIgnoreCase("csrlin")) {
            if (string.length() > base.length() + 1) {
                getInstance().listOfStrings.add(getInstance().lineNumber + "\n");
                result = YES;
            } else {
                getInstance().error = "Missing operand\n";
            }
        } else if (base.equalsIgnoreCase("pos")) {
            if (string.length() > base.length() + 1) {
                getInstance().listOfStrings.add(getInstance().command.length());
                result = YES;
            } else {
                getInstance().error = "Missing operand\n";
            }
        } else if (base.equalsIgnoreCase("let")) {
            if (string.length() > base.length() + 1) {
                String clearString = string.substring(4);
                result = let(clearString);
            } else {
                getInstance().error = "Missing operand\n";
            }
        } else if (base.equalsIgnoreCase("run")) {
            /*
            if (string.length() > 4) {
                load(string.substring(4));
            } */
            getInstance().isOkSet = NO;
            result = NO;
            scanData();
        } else if (base.equalsIgnoreCase("files")) {
            File directory = new File(getInstance().currentFolder);
            File[] list = directory.listFiles();
            if (list == null)
                list = new File[]{};
            List<File> fileList = Arrays.asList(list);
            Collections.sort(fileList, new Comparator<File>() {
                @Override
                public int compare(File file, File file2) {
                    if (file.isDirectory() && file2.isFile())
                        return -1;
                    else if (file.isFile() && file2.isDirectory())
                        return 1;
                    else
                        return file.getPath().compareTo(file2.getPath());
                }
            });
            for (int i = 0; i < fileList.size(); i++)
                if (fileList.get(i).toString().contains(".bas"))
                    getInstance().listOfStrings.add(fileList.get(i).toString().substring(fileList.get(i).toString().lastIndexOf("/") + 1) + "\n");
            // Log.d(TAG, "± files-"+getInstance().listOfStrings);
            result = YES;
        } else if (base.equalsIgnoreCase("share")) {

            /*
                String fn;
                if (string.length()>base.length()+1) {
                    string = [string.substring(6];
                    normaStr.= ((NormalizeString alloc]init];
                    string = [normaStr.removeSpaceInBegin:string];
                    string = [string.replace("\"",""];
                    string = [string.replace(".bas",""];
                    string = [NSString stringWithFormat:".bas",string];
                    ArrayList paths = NSSearchPathForDirectoriesInDomains(NSDocumentDirectory, NSUserDomainMask, YES);
                    NSString *documentsDirectory = [paths.get(0]; // Get documents directory
                    NSError *error;
                    String arrayText = getInstance().listOfProgram componentsJoinedByString: "\n"];
                    fn= documentsDirectory stringByAppendingPathComponent:string];
                    boolean succeed = [arrayText writeToFile(documentsDirectory stringByAppendingPathComponent:string]
                    atomically:YES encoding:NSUTF8StringEncoding error:&error];
                    getInstance().fileName= documentsDirectory stringByAppendingPathComponent:string];
                    if (!succeed){
                        getInstance().error = "Bad file name";
                        // Handle error here
                    } else {
                        getInstance().fileName=string;
                    }
                    result=NO;
                } else {
                    getInstance().error = "Missing operand\n";
                } */
        } else if (base.equalsIgnoreCase("save")) {
            if (string.length() > base.length() + 1) {
                save(string.substring(5));
                result = YES;
            } else {
                getInstance().error = "Missing operand\n";
            }
        } else if (base.equalsIgnoreCase("csave")) {
            //csave:getInstance().fileName];
            result = YES;
        } else if (base.equalsIgnoreCase("new")) {
            reset();
            result = NO;
        } else if (base.equalsIgnoreCase("reset")) {
            reset();
            result = NO;
        } else if (base.equalsIgnoreCase("load")) {
            result = NO;
        } else if (!digitalFunc.isOnlyDigits(base) && variables.isArrayPresent(string)) { // let dim array
            Log.d(TAG, "± let to dim '" + string + "'");
            result = setDim(string);
        } else if (!digitalFunc.isOnlyDigits(base) && !base.isEmpty()) { // let var without LET operator
            Log.d(TAG, "± let to variable (no let)");
            NSRange range = new NSRange(string.indexOf("="), 1);
            if (range.location != NSNotFound) {
                String afterBase = string.substring(range.location + 1);
                String before = string.substring(0, range.location);
                if (getInstance().keyScan.length() == 0) getInstance().keyScan = "";
                if (afterBase.equalsIgnoreCase("inkey$")) {
                    if (getInstance().run) getInstance().scanKeyOn = YES;
                    string = before + "=\"" + getInstance().keyScan + "=\"";
                    getInstance().keyScan = "";
                }
            }
            result = let(string);
        } else if (digitalFunc.isOnlyDigits(base) && !base.equalsIgnoreCase("")) { // manual program string set
            Log.d(TAG, "± manual program string set at line '" + base + "'");
            getInstance().variables.clear();
            programString(string, base);
            getInstance().isOkSet = NO;
        } else {
            Log.d(TAG, "± command error");
            getInstance().setCommand("");
            if (!base.equals("")) {
                Log.d(TAG, "± Syntax error");
                getInstance().setError("Syntax error\n");
            }
        }
        Log.d(TAG, "± runCommand set over!");
        return result;
    }

    public String returnBaseCommand(String string) {
        String result = string.split(" ")[0];
        for (int i = 0; i < getInstance().getListOfAll().size(); i++) {
            int index = string.toLowerCase().indexOf(getInstance().listOfAll.get(i).toString());
            if (index != NSNotFound && index == 0)
                result = getInstance().listOfAll.get(i).toString();
        }
        return result;
    }

    public String resultFromString(String string) {
        //очищаем от пробелов
        //заменяем ; на ,
        //Находим все переменные и подменяем на значения
        //Находим массивы и подменяем
        //Находим все функции и внутри них анализируем на наличие арифметики затем рассчитываем и подменяем
        //вычисляем каждый компонент математики
        //строковые функции

        String result = "";
        //Log.d(TAG, "± ========resultFromString begin-'" + string + "'");
        //очищаем от пробелов
        string = normaStr.removeSpacesWithText(string);
        //заменяем ; на ,
        string = normaStr.replaceCharWithCharInText(';', ',', string);
        //Log.d(TAG, "± ========resultFromString replaceChar:';' withChar:',' ---- '" + string + "'");
        ArrayList arr = new ArrayList(normaStr.stringSeparateAllToArray(string)); //делим строку на компоненты
        //Log.d(TAG, "± ========stringSeparateAllToArray " + arr);
        //Находим все переменные и подменяем на значения
        for (int i = 0; i < arr.size(); i++) {
            if (variables.variableIsPresent(arr.get(i).toString())) {
                String tmp = variables.returnContainOfVariable(arr.get(i).toString());
                if (variables.variableIsString(arr.get(i).toString()) && !normaStr.isText(tmp))
                    tmp = "\"" + tmp + "\"";//если переменная строковая добавим кавычки но при этом содержимое не содержит кавычки
                arr.set(i, tmp);
            }
        }
        string = "";
        for (int i = 0; i < arr.size(); i++) {
            string = string + arr.get(i).toString(); //склеиваем строку из массива
        }
        arr.clear();
        //Log.d(TAG, "± ========resultFromString after replace vars - '" + string + "'");
        //Находим массивы и подменяем
        String forDim = "";
        boolean addMode = NO;
        ArrayList arrTemp = new ArrayList(normaStr.stringSeparateAllToArray(string)); //делим строку на компоненты
        if (arrTemp.size() > 3) {
            for (int i = 1; i < arrTemp.size(); i++) {
                boolean addArr = YES;
                String curr = arrTemp.get(i).toString();
                String prev = arrTemp.get(i - 1).toString();
                if (addMode) {
                    addArr = NO;
                    if (!curr.equals(")")) {
                        forDim = forDim + curr;
                    } else {
                        forDim = forDim + curr;
                        addMode = NO;
                        if (variables.isArrayPresent(forDim)) {
                            String tmp = variables.returnContainOfArray(forDim);
                            forDim = tmp;
                        }
                        if (arr.size() == 0) {
                            arr.add(forDim);
                        } else {
                            arr.set(arr.size() - 1, forDim);
                        }
                    }
                }
                if (curr.equals("(")) {
                    if (!digitalFunc.mathFunction(prev) && !stringFunc.stringFunction(prev)) {
                        forDim = prev + curr;
                        addMode = YES;
                    } else {
                        if (i == 1) arr.add(prev);
                        arr.add(curr);
                    }
                } else {
                    if (addArr) {
                        if (i == 1) arr.add(prev);
                        arr.add(curr);
                    }
                }
            }
        } else {
            arr = new ArrayList(arrTemp);
        }
        string = "";
        for (int i = 0; i < arr.size(); i++) {
            string = string + arr.get(i).toString(); //склеиваем строку из массива
        }
        arr.clear();
        arrTemp.clear();
        //Log.d(TAG, "± ========resultFromString after replace dims - '" + string + "'");
        //склеиваем все строки стоящие рядом
        String fStr = "";
        String tStr = "";
        boolean firstEl = NO;
        boolean secondEl = NO;
        int index = 0;
        arr = new ArrayList(normaStr.stringSeparateAllToArray(string)); //делим строку на компоненты
        //    Log.d(TAG, "± resultFromString arr-''",arr);
        if (arr.size() > 3 && !string.contains("instr")) {
            string = "";
            for (int i = 0; i < arr.size(); i++) {
                arrTemp.add(arr.get(i));
                if (normaStr.isText(arr.get(i).toString()) && !firstEl && !secondEl) { //находим первый элемент текст но еще нет второго и задает индекс первого
                    fStr = arr.get(i).toString().replace("\"", "");
                    firstEl = YES;
                    index = i;
                }
                if (arr.get(i).toString().equals(",") && firstEl && !secondEl && i == index + 1) {//находим 2 элемент запятую и уже есть первый
                    secondEl = YES;
                }
                if (arr.get(i).toString().equals("+") && firstEl && !secondEl && i == index + 1) {//находим 2 элемент + и уже есть первый
                    secondEl = YES;
                }
                if (i == index + 1 && !secondEl)// если индекс первого эл-та предидущий но нет второго то все сбрасываем
                {
                    firstEl = NO;
                    fStr = "";
                    tStr = "";
                }
                if (i == index + 2 && secondEl && !normaStr.isText(arr.get(i).toString()))// если индекс первого эл-та -2 но третий эл-т не текст то все сбрасываем
                {
                    firstEl = NO;
                    secondEl = NO;
                    fStr = "";
                    tStr = "";
                }
                if (normaStr.isText(arr.get(i).toString()) && firstEl && secondEl) { //находим третий элемент текст имея первый и второй
                    tStr = arr.get(i).toString().replace("\"", "");
                    firstEl = YES;
                    secondEl = NO;
                    index = i;
                    if (arrTemp.size() > 2) {
                        arrTemp.remove(arrTemp.size() - 1);
                        arrTemp.remove(arrTemp.size() - 1);
                        arrTemp.remove(arrTemp.size() - 1);
                    }
                    fStr = "\"" + fStr + tStr + "\"";
                    arrTemp.add(fStr);
                    fStr = fStr.replace("\"", "");
                }
            }
        }
        for (int i = 0; i < arrTemp.size(); i++) {
            string = string + arrTemp.get(i).toString(); //склеиваем строку из массива
        }
        arr.clear();
        arrTemp.clear();
        //Log.d(TAG, "± ========resultFromString after strings-'" + string + "'");
        //Находим все мат функции и внутри них анализируем на наличие арифметики затем рассчитываем и подменяем
        String forMFunc = "";
        int addCount = 0;
        boolean first = NO;
        addMode = NO;
        arrTemp = new ArrayList(normaStr.stringSeparateAllToArray(string)); //делим строку на компоненты
        if (arrTemp.size() > 3) {
            for (int i = 1; i < arrTemp.size(); i++) {
                String curr = arrTemp.get(i).toString();
                String prev = arrTemp.get(i - 1).toString();
                if (curr.equals("(") && digitalFunc.mathFunction(prev) && !addMode) { //определяем когда началась мат функция
                    addCount++;
                    addMode = YES;
                    first = YES;
                    forMFunc = prev + curr;
                }
                if (curr.equals("(") && stringFunc.stringFunction(prev) && !addMode) { //определяем когда началась строковая функция
                    addCount++;
                    addMode = YES;
                    first = YES;
                    forMFunc = prev + curr;
                }
                if (addMode) { // режим наращивания функции
                    if (!first) {
                        forMFunc = forMFunc + curr;
                        if (curr.equals("(")) {
                            addCount++;
                        }
                        if (curr.equals(")")) {
                            addCount--;
                        }
                        if (addCount == 0) {
                            if (forMFunc.length() > 3)
                                if (forMFunc.substring(0, 4).equals("abs(") || forMFunc.substring(0, 4).equals("fix(")) {
                                    String temp = digitalFunc.mathFunctionInMixedString(forMFunc.substring(4, forMFunc.length() - 1));
                                    forMFunc = forMFunc.substring(0, 4) + temp + ")";
                                }
                            arr.add(forMFunc);
                            forMFunc = "";
                            addMode = NO;
                        }
                    }
                    first = NO;
                } else { // режим просто добавления без мат функции
                    if (i == 1) arr.add(prev);
                    if (!digitalFunc.mathFunction(curr) && !stringFunc.stringFunction(curr))
                        arr.add(curr);
                }
            }
        } else {
            arr = new ArrayList(arrTemp);
        }
        string = "";
        for (int i = 0; i < arr.size(); i++) {
            String tmp = arr.get(i).toString();
            if (digitalFunc.mathFunction(tmp)) {
                tmp = digitalFunc.mathFunctionInMixedString(tmp);
                tmp = String.valueOf(digitalFunc.returnMathResult(tmp));
            }
            if (stringFunc.stringFunction(tmp))
                tmp = stringFunc.returnStringResult(tmp); // проверяем на строковые функции и возвращаем результаты
            string = string + tmp; //склеиваем строку из массива
        }
        arr.clear();
        //Log.d(TAG, "± ========resultFromString after mathematics & string func's-'" + string + "'");

        //вычисляем каждый компонент математики
        if (string.length() > 0)
            string = digitalFunc.mathFunctionInMixedString(string); //1. mathFunctionInMixedString
        //Log.d(TAG, "± ========resultFromString mathFunctionInMixedString-'" + string + "'");
        //избавляемся от запятых и с плюсов, склеиваем строку из компонентов
        arr = new ArrayList(normaStr.stringSeparateAllToArray(string)); //делим строку на компоненты
        boolean quotes = NO;
        for (int i = 0; i < arr.size(); i++) {
            if (normaStr.isText(arr.get(i).toString())) quotes = YES;
            if (!arr.get(i).toString().equals(","))
                if (!arr.get(i).toString().equals("+"))
                    result = result + arr.get(i).toString(); //склеиваем строку из массива
        }
        if (quotes) {
            result = result.replace("\"", "");
            result = "\"" + result + "\"";
        }
        //Log.d(TAG, "± ========resultFromString FINAL RESULT-'" + result + "'");
        return result;
    }

    public String input(String string) {
        String result = "";
        Log.d(TAG, "± input -'" + string + "'");

        getInstance().scanKeyOn = NO;
        string = string.replace(";", ",");
        String alphaSet = "[^a-zA-Z]+";//если только НЕ буквы
        String separator = "\"";
        string = string.substring(5);
        String until = " ";
        if (string.contains(separator))
            until = separator + normaStr.extractTextToArray(string).get(0).toString() + separator;
        getInstance().error = "";
        int index = until.length() + 1;
        boolean strings = NO;
        boolean stringsVariable = NO;
        boolean digits = NO;
        boolean digitsVariable = NO;
        if (variables.variableIsString(until)) stringsVariable = YES;
        if (variables.variableIsDigit(until)) digitsVariable = YES;
        if (digitalFunc.isOnlyDigitsWithMath(until)) digits = YES;
        if (until.substring(0, 1).equals(separator) && until.substring(until.length() - 1).equals(separator))
            strings = YES;
        if (string.isEmpty()) {
            getInstance().error = "Syntax error\n";
            Log.d(TAG, "± input Syntax error");
            getInstance().isOkSet = NO;
        } else if (strings && !stringsVariable && !digits && !digitsVariable && string.contains(",")) {
            result = string.substring(index);
            result = result.replace(" ", "");
            if (!result.contains(",")) result = "," + result;
            until = until.replace(separator, "");
            getInstance().listOfStrings.add(until);
            variables.forbiddenVariable(result.substring(1));
            Log.d(TAG, "± input string result - " + result);
        } else {
            Log.d(TAG, "± input Str-'" + string + "'");
            string = string.replace(" ", "");
            if (string.contains(",") && !string.substring(0, 1).matches(alphaSet)) {
                result = "," + string;
            } else if (!string.matches(alphaSet)) {
                result = "," + string;
            } else {
                result = "," + string;
            }
            Log.d(TAG, "± input result -'" + result + "'");
        }
        return result;

    }

    public boolean print(String string) {
        //Log.d(TAG, "± PRINT init ......''" + string);
        String cr = "\n";
        if (string.substring(string.length() - 1).equals(";")) {
            cr = "";
            string = string.substring(0,string.length() - 1);
        }
        boolean result = NO;
        string = string.substring(5);
        String printResult = "";
        string = resultFromString(string);//присваиваем результат рассчетов
        if (normaStr.isText(string)) {
            string = string.replace("\"", "");
        }
        printResult = string + cr;
        getInstance().listOfStrings.add(printResult);
        if (getInstance().error.isEmpty()) result = YES;
        //Log.d(TAG, "± PRINT result '" + printResult + "' error-" + getInstance().error);
        return result;
    }


    public boolean ifThen(String ifValue) {
        boolean result = YES;

        String ifFirst;
        String ifEqual;
        String ifSecond;
        NSRange rangeThen;

        //Log.d(TAG, "± IF operator '=' "+ifValue);

        if (ifValue.contains("=>")) {
            ifEqual = "=>";
            rangeThen = new NSRange(ifValue.indexOf(ifEqual), ifEqual.length());
            ifFirst = ifValue.substring(0, rangeThen.location);
            ifSecond = ifValue.substring(rangeThen.location + ifEqual.length());
            ifFirst = checkResult(ifFirst);
            ifSecond = checkResult(ifSecond);
            if (!isStringValue(ifFirst) && !isStringValue(ifSecond)) {
                if (variables.variableIsPresent(ifFirst))
                    ifFirst = variables.returnContainOfVariable(ifFirst);
                if (variables.variableIsPresent(ifSecond))
                    ifSecond = variables.returnContainOfVariable(ifSecond);
                if (Float.parseFloat(ifFirst) >= Float.parseFloat(ifSecond)) {
                    result = YES;
                } else {
                    result = NO;
                }
            } else {
                Log.d(TAG, "± Syntax error in IF operator");
                getInstance().error = "Type mismatch in IF operator\n";
            }
        } else if (ifValue.contains("<>")) {
            ifEqual = "<>";
            rangeThen = new NSRange(ifValue.indexOf(ifEqual), ifEqual.length());
            ifFirst = ifValue.substring(0, rangeThen.location);
            ifSecond = ifValue.substring(rangeThen.location + ifEqual.length());
            ifFirst = checkResult(ifFirst);
            ifSecond = checkResult(ifSecond);
            if (!isStringValue(ifFirst) && !isStringValue(ifSecond)) {
                if (variables.variableIsPresent(ifFirst))
                    ifFirst = variables.returnContainOfVariable(ifFirst);
                if (variables.variableIsPresent(ifSecond))
                    ifSecond = variables.returnContainOfVariable(ifSecond);
                if (Float.parseFloat(ifFirst) != Float.parseFloat(ifSecond)) {
                    result = YES;
                } else {
                    result = NO;
                }
            } else {
                ifFirst = normaStr.removeSpacesWithText(ifFirst);
                ifSecond = normaStr.removeSpacesWithText(ifSecond);
                if (variables.variableIsPresent(ifFirst))
                    ifFirst = "\"" + variables.returnContainOfVariable(ifFirst) + "\"";
                if (variables.variableIsPresent(ifSecond))
                    ifSecond = "\"" + variables.returnContainOfVariable(ifSecond) + "\"";
                if (!ifFirst.equals(ifSecond)) {
                    result = YES;
                } else {
                    result = NO;
                }
            }
        } else if (ifValue.contains("><")) {
            ifEqual = "><";
            rangeThen = new NSRange(ifValue.indexOf(ifEqual), ifEqual.length());
            ifFirst = ifValue.substring(0, rangeThen.location);
            ifSecond = ifValue.substring(rangeThen.location + ifEqual.length());
            ifFirst = checkResult(ifFirst);
            ifSecond = checkResult(ifSecond);
            if (!isStringValue(ifFirst) && !isStringValue(ifSecond)) {
                if (variables.variableIsPresent(ifFirst))
                    ifFirst = variables.returnContainOfVariable(ifFirst);
                if (variables.variableIsPresent(ifSecond))
                    ifSecond = variables.returnContainOfVariable(ifSecond);
                if (Float.parseFloat(ifFirst) != Float.parseFloat(ifSecond)) {
                    result = YES;
                } else {
                    result = NO;
                }
            } else {
                ifFirst = normaStr.removeSpacesWithText(ifFirst);
                ifSecond = normaStr.removeSpacesWithText(ifSecond);
                if (variables.variableIsPresent(ifFirst))
                    ifFirst = "\"" + variables.returnContainOfVariable(ifFirst) + "\"";
                if (variables.variableIsPresent(ifSecond))
                    ifSecond = "\"" + variables.returnContainOfVariable(ifSecond) + "\"";
                if (!ifFirst.equals(ifSecond)) {
                    result = YES;
                } else {
                    result = NO;
                }
            }
        } else if (ifValue.contains(">=")) {
            ifEqual = ">=";
            rangeThen = new NSRange(ifValue.indexOf(ifEqual), ifEqual.length());
            ifFirst = ifValue.substring(0, rangeThen.location);
            ifSecond = ifValue.substring(rangeThen.location + ifEqual.length());
            ifFirst = checkResult(ifFirst);
            ifSecond = checkResult(ifSecond);
            if (!isStringValue(ifFirst) && !isStringValue(ifSecond)) {
                if (variables.variableIsPresent(ifFirst))
                    ifFirst = variables.returnContainOfVariable(ifFirst);
                if (variables.variableIsPresent(ifSecond))
                    ifSecond = variables.returnContainOfVariable(ifSecond);
                if (Float.parseFloat(ifFirst) >= Float.parseFloat(ifSecond)) {
                    result = YES;
                } else {
                    result = NO;
                }
            } else {
                Log.d(TAG, "± Syntax error in IF operator");
                getInstance().error = "Type mismatch in IF operator\n";
            }
        } else if (ifValue.contains("=<")) {
            ifEqual = "=<";
            rangeThen = new NSRange(ifValue.indexOf(ifEqual), ifEqual.length());
            ifFirst = ifValue.substring(0, rangeThen.location);
            ifSecond = ifValue.substring(rangeThen.location + ifEqual.length());
            ifFirst = checkResult(ifFirst);
            ifSecond = checkResult(ifSecond);
            if (!isStringValue(ifFirst) && !isStringValue(ifSecond)) {
                if (variables.variableIsPresent(ifFirst))
                    ifFirst = variables.returnContainOfVariable(ifFirst);
                if (variables.variableIsPresent(ifSecond))
                    ifSecond = variables.returnContainOfVariable(ifSecond);
                if (Float.parseFloat(ifFirst) <= Float.parseFloat(ifSecond)) {
                    result = YES;
                } else {
                    result = NO;
                }
            } else {
                Log.d(TAG, "± Syntax error in IF operator");
                getInstance().error = "Type mismatch in IF operator\n";
            }
        } else if (ifValue.contains("<=")) {
            ifEqual = "<=";
            rangeThen = new NSRange(ifValue.indexOf(ifEqual), ifEqual.length());
            ifFirst = ifValue.substring(0, rangeThen.location);
            ifSecond = ifValue.substring(rangeThen.location + ifEqual.length());
            ifFirst = checkResult(ifFirst);
            ifSecond = checkResult(ifSecond);
            if (!isStringValue(ifFirst) && !isStringValue(ifSecond)) {
                if (variables.variableIsPresent(ifFirst))
                    ifFirst = variables.returnContainOfVariable(ifFirst);
                if (variables.variableIsPresent(ifSecond))
                    ifSecond = variables.returnContainOfVariable(ifSecond);
                if (Float.parseFloat(ifFirst) <= Float.parseFloat(ifSecond)) {
                    result = YES;
                } else {
                    result = NO;
                }
            } else {
                Log.d(TAG, "± Syntax error in IF operator");
                getInstance().error = "Type mismatch in IF operator\n";
            }
        } else if (ifValue.contains("<")) {
            ifEqual = "<";
            rangeThen = new NSRange(ifValue.indexOf(ifEqual), ifEqual.length());
            ifFirst = ifValue.substring(0, rangeThen.location);
            ifSecond = ifValue.substring(rangeThen.location + ifEqual.length());
            ifFirst = checkResult(ifFirst);
            ifSecond = checkResult(ifSecond);
            if (!isStringValue(ifFirst) && !isStringValue(ifSecond)) {
                if (variables.variableIsPresent(ifFirst))
                    ifFirst = variables.returnContainOfVariable(ifFirst);
                if (variables.variableIsPresent(ifSecond))
                    ifSecond = variables.returnContainOfVariable(ifSecond);
                if (Float.parseFloat(ifFirst) < Float.parseFloat(ifSecond)) {
                    result = YES;
                } else {
                    result = NO;
                }
            } else {
                Log.d(TAG, "± Syntax error in IF operator");
                getInstance().error = "Type mismatch in IF operator\n";
            }
        } else if (ifValue.contains(">")) {
            ifEqual = ">";
            rangeThen = new NSRange(ifValue.indexOf(ifEqual), ifEqual.length());
            ifFirst = ifValue.substring(0, rangeThen.location);
            ifSecond = ifValue.substring(rangeThen.location + ifEqual.length());
            ifFirst = checkResult(ifFirst);
            ifSecond = checkResult(ifSecond);
            if (!isStringValue(ifFirst) && !isStringValue(ifSecond)) {
                if (variables.variableIsPresent(ifFirst))
                    ifFirst = variables.returnContainOfVariable(ifFirst);
                if (variables.variableIsPresent(ifSecond))
                    ifSecond = variables.returnContainOfVariable(ifSecond);
                if (Float.parseFloat(ifFirst) > Float.parseFloat(ifSecond)) {
                    result = YES;
                } else {
                    result = NO;
                }
            } else {
                Log.d(TAG, "± Syntax error in IF operator");
                getInstance().error = "Type mismatch in IF operator\n";
            }
        } else if (ifValue.contains("=")) {
            ifEqual = "=";
            rangeThen = new NSRange(ifValue.indexOf(ifEqual), ifEqual.length());
            ifFirst = ifValue.substring(0, rangeThen.location);
            ifSecond = ifValue.substring(rangeThen.location + ifEqual.length());
            ifFirst = checkResult(ifFirst);
            ifSecond = checkResult(ifSecond);
            if (!isStringValue(ifFirst) && !isStringValue(ifSecond)) {
                if (variables.variableIsPresent(ifFirst))
                    ifFirst = variables.returnContainOfVariable(ifFirst);
                if (variables.variableIsPresent(ifSecond))
                    ifSecond = variables.returnContainOfVariable(ifSecond);
                if (Float.parseFloat(ifFirst) == Float.parseFloat(ifSecond)) {
                    // Log.d(TAG, "± (digit)IF " + ifFirst + " == " + ifSecond);
                    result = YES;
                } else {
                    result = NO;
                }
            } else {
                ifFirst = normaStr.removeSpacesWithText(ifFirst);
                ifSecond = normaStr.removeSpacesWithText(ifSecond);
                if (variables.variableIsPresent(ifFirst))
                    ifFirst = "\"" + variables.returnContainOfVariable(ifFirst) + "\"";
                if (variables.variableIsPresent(ifSecond))
                    ifSecond = "\"" + variables.returnContainOfVariable(ifSecond) + "\"";
                Log.d(TAG, "± (digit)IF " + ifFirst + " EQUAL " + ifSecond);
                if (ifFirst.equals(ifSecond)) {
                    result = YES;
                } else {
                    result = NO;
                }
            }
        } else {
            Log.d(TAG, "± Syntax error in IF operator for all!");
            getInstance().error = "Syntax error in IF operator\n";
        }
        return result;
    }


    public String checkResult(String value) {
        return resultFromString(value);//присваиваем результат рассчетов
    }


    public void scanKeyOff() {
        getInstance().scanKeyOn = NO;
    }

    public void scanData() {
        getInstance().dataReadIndex = 0;
        getInstance().data.clear();
        getInstance().dataIndex.clear();
        //    Log.d(TAG, "± SCANNING...");
        String separator = "\"";
        for (int n = 0; n < getInstance().listOfProgram.size(); n++) {
            String currentLine = getInstance().listOfProgram.get(n).toString();
            if (currentLine.isEmpty()) {
                getInstance().listOfProgram.remove(n);
                Log.d(TAG, "± REMOVING..." + n + " " + currentLine);
            }
        }
        for (int n = 0; n < getInstance().listOfProgram.size(); n++) {
            String currentLine = getInstance().listOfProgram.get(n).toString();
            Log.d(TAG, "± SCANNING..." + n + " " + currentLine);
            String untilSpace = currentLine.split(" ")[0];
            getInstance().runnedLine = untilSpace;
            int indexforAfterSpace = untilSpace.length();
            String string = currentLine.substring(indexforAfterSpace + 1);
            String base = returnBaseCommand(string);
            if (base.equalsIgnoreCase("data")) {
                if (string.length() > base.length() + 1) {
                    if (string.contains(",")) {
                        String[] arr = string.substring(5).split(",");
                        for (int i = 0; i < arr.length; i++) {
                            String stmp = arr[i];
                            getInstance().dataIndex.add(untilSpace); //записываем в индекс номер строки data
                            if (stmp.contains(separator)) {
                                stmp = stmp.split(separator)[1];
                                getInstance().data.add(stmp);
                            } else {
                                getInstance().data.add(stmp);
                            }
                            Log.d(TAG, "± SET DATA ''" + stmp);
                        }
                    } else {
                        String stmp = string.substring(5);
                        getInstance().dataIndex.add(untilSpace); //записываем в индекс номер строки data
                        if (stmp.contains(separator)) {
                            getInstance().data.add(stmp.split(separator)[1]);
                        } else {
                            getInstance().data.add(stmp);
                        }
                        Log.d(TAG, "± SET DATA ''" + stmp);
                    }
                } else {
                    getInstance().error = "Missing operand\n";
                }
            }
        }
        Log.d(TAG, "± SCANNING OVER " + getInstance().dataIndex);
    }

    public void renumGotoGosub() {
        ArrayList oldList = new ArrayList(getInstance().listOfProgram);
        String replaceString;
        String number;
        if (getInstance().listOfProgram.size() > 0) {
            String gotoGosub = "goto";
            int c = 1;
            for (int i = 0; i < getInstance().listOfProgram.size(); i++) {
                number = getInstance().listOfProgram.get(i).toString().split(" ")[0];
                replaceString = getInstance().listOfProgram.get(i).toString();
                replaceString = c * 10 + " " + replaceString.substring(number.length() + 1, replaceString.length());
                getInstance().listOfProgram.set(i, replaceString);
                c++;
            }
            for (int i = 0; i < getInstance().listOfProgram.size(); i++) {
                replaceString = getInstance().listOfProgram.get(i).toString();
                Log.d(TAG, "± before number=" + i + " replaceString='" + replaceString + "'");
                NSRange range = new NSRange(replaceString.indexOf(gotoGosub), gotoGosub.length());
                String[] tempArr = replaceString.split(gotoGosub);
                c = 0;
                int locat = (int) (range.location + range.length + 1);
                boolean found = NO;
                int index = 0;
                for (int cc = 1; cc < tempArr.length; cc++)
                    if (range.location != NSNotFound && !normaStr.insideText(replaceString, locat)) {
                        index = replaceString.length();
                        for (int t = locat; t < replaceString.length(); t++)
                            if (replaceString.charAt(t) == ' ' && !found) {
                                Log.d(TAG, "± '" + replaceString.charAt(t) + "'");
                                index = t;
                                found = YES;
                            }
                        String first = getInstance().listOfProgram.get(i).toString().substring(0, locat);
                        String whatRepl = replaceString.substring(locat, index);
                        String second = getInstance().listOfProgram.get(i).toString().substring(index);
                        int renIndex = returnIndexOf(oldList, whatRepl);
                        String withRepl = getInstance().listOfProgram.get(renIndex).toString().split(" ")[0];
                        String repStr = first + withRepl + second;
                        getInstance().listOfProgram.set(i, repStr);
                        replaceString = repStr;
                        range = new NSRange(replaceString.substring(locat).indexOf(gotoGosub) + locat, gotoGosub.length());
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
            for (int i = 0; i < getInstance().listOfProgram.size(); i++) {
                replaceString = getInstance().listOfProgram.get(i).toString();
                NSRange range = new NSRange(replaceString.indexOf(gotoGosub), gotoGosub.length());
                String[] tempArr = replaceString.split(gotoGosub);
                c = 0;
                int locat = (int) (range.location + range.length + 1);
                boolean found = NO;
                int index = 0;
                for (int cc = 1; cc < tempArr.length; cc++)
                    if (range.location != NSNotFound && !normaStr.insideText(replaceString, locat)) {
                        index = (int) replaceString.length();
                        for (int t = locat; t < replaceString.length(); t++)
                            if (replaceString.charAt(t) == ' ' && !found) {
                                Log.d(TAG, "± '" + replaceString.charAt(t) + "'");
                                index = t;
                                found = YES;
                            }
                        String first = getInstance().listOfProgram.get(i).toString().substring(0, locat);
                        String whatRepl = replaceString.substring(locat, index);
                        String second = getInstance().listOfProgram.get(i).toString().substring(index);
                        int renIndex = returnIndexOf(oldList, whatRepl);
                        String withRepl = getInstance().listOfProgram.get(renIndex).toString().split(" ")[0];
                        String repStr = first + withRepl + second;
                        getInstance().listOfProgram.set(i, repStr);
                        if (!whatRepl.isEmpty()) getInstance().listOfProgram.set(i, repStr);
                        replaceString = repStr;
                        range = new NSRange(replaceString.substring(locat).indexOf(gotoGosub) + locat, gotoGosub.length());
                        range.location = range.location + locat;
                        locat = (int) (range.location + range.length + 1);
                        found = NO;
                        index = 0;
                        first = "";
                        second = "";
                        whatRepl = "";
                        withRepl = "";
                    }
            }
            gotoGosub = "restore";
            for (int i = 0; i < getInstance().listOfProgram.size(); i++) {
                replaceString = getInstance().listOfProgram.get(i).toString();
                NSRange range = new NSRange(replaceString.indexOf(gotoGosub), gotoGosub.length());
                String[] tempArr = replaceString.split(gotoGosub);
                c = 0;
                int locat = (int) (range.location + range.length + 1);
                boolean found = NO;
                int index = 0;
                for (int cc = 1; cc < tempArr.length; cc++)
                    if (range.location != NSNotFound && !normaStr.insideText(replaceString, locat)) {
                        index = (int) replaceString.length();
                        for (int t = locat; t < replaceString.length(); t++)
                            if (replaceString.charAt(t) == ' ' && !found) {
                                Log.d(TAG, "± '" + replaceString.charAt(t) + "'");
                                index = t;
                                found = YES;
                            }
                        String first = getInstance().listOfProgram.get(i).toString().substring(0, locat);
                        String whatRepl = replaceString.substring(locat, index);
                        String second = getInstance().listOfProgram.get(i).toString().substring(index);
                        int renIndex = returnIndexOf(oldList, whatRepl);
                        String withRepl = getInstance().listOfProgram.get(renIndex).toString().split(" ")[0];
                        String repStr = first + withRepl + second;
                        getInstance().listOfProgram.set(i, repStr);

                        if (!whatRepl.isEmpty()) getInstance().listOfProgram.set(i, repStr);
                        replaceString = repStr;
                        range = new NSRange(replaceString.substring(locat).indexOf(gotoGosub) + locat, gotoGosub.length());
                        range.location = range.location + locat;
                        locat = (int) (range.location + range.length + 1);
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

    public boolean isStringValue(String string) {
        boolean result = YES;
        boolean strings = NO;
        boolean stringsVariable = NO;
        boolean digits = NO;
        boolean digitsVariable = NO;
        if (variables.variableIsString(string)) stringsVariable = YES;
        if (variables.variableIsDigit(string)) digitsVariable = YES;
        if (digitalFunc.isOnlyDigitsWithMath(string)) digits = YES;
        if (string.length() > 0)
            if (string.substring(0, 1).equals("\"") && string.substring(string.length() - 1).equals("\""))
                strings = YES;
        if ((!strings && !stringsVariable && digits && digitsVariable)
                || (!strings && !stringsVariable && !digits && digitsVariable)
                || (!strings && !stringsVariable && digits && !digitsVariable)) { //if variable and value is only digits
            result = NO;
        }
        return result;
    }

    public void reset() {
        getInstance().lineNumber = 0;
        getInstance().data.clear();
        getInstance().array.clear();
        getInstance().variables.clear();
        getInstance().listOfProgram.clear();
        getInstance().listOfStrings.clear();
        getInstance().gosubArray.clear();
        getInstance().forArray.clear();

        getInstance().autoSet = NO;
        getInstance().autoStep = 10;
        getInstance().programCounter = 10;
        getInstance().commandIf = "";
        getInstance().error = "";
        getInstance().input = "";
        getInstance().runnedLine = "";
        getInstance().dataReadIndex = 0;
        getInstance().isOkSet = YES;
        getInstance().run = NO;
        getInstance().showError = NO;
        getInstance().scanKeyOn = NO;
    }

    public void save(String string) {
        Log.d(TAG, "± saving-" + string);
        string = normaStr.removeSpaceInBegin(string);
        string = string.replace("\"", "");
        string = string.replace(".bas", "");
        string = string + ".bas";
        String documentsDirectory = getInstance().currentFolder; // Get documents directory
        String arrayText = "";
        for (int i = 0; i < getInstance().listOfProgram.size(); i++)
            arrayText = arrayText + getInstance().listOfProgram.get(i).toString() + "\n";
        try {

            File f = new File(getInstance().currentFolder); //Check if folder not excist - make new one
            if (!f.isDirectory()) {
                Log.d(TAG, "± Make dir " + getInstance().currentFolder);
                f.mkdir();
            }

            File filepath = new File(f, string);  // file path to save
            FileWriter writer = new FileWriter(filepath);
            writer.append(arrayText);
            writer.flush();
            writer.close();
            getInstance().fileName = documentsDirectory + "/" + string;
        } catch (Throwable t) {
            Log.d(TAG, "± Exception: " + t.toString());
        }
        Log.d(TAG, "± saved - '" + getInstance().fileName + "'");
    }

    public void csave(String string) {
            /*
        Log.d(TAG, "± saving-''",string);
        NSError *error;
        String arrayText = getInstance().listOfProgram componentsJoinedByString: "\n"];
        boolean succeed = [arrayText writeToFile:string atomically:YES encoding:NSUTF8StringEncoding error:&error];
        if (!succeed){
        getInstance().error = "Bad file name";
        // Handle error here
        }
        */
    }

    public boolean kill(String string) {
        boolean result = YES;
            /*
        normaStr.= ((NormalizeString alloc]init];
        string = [normaStr.removeSpaceInBegin:string];
        string = [string.replace("\"",""];
        string = [string.replace(".bas",""];
        string = [NSString stringWithFormat:".bas",string];
        ArrayList paths = NSSearchPathForDirectoriesInDomains(NSDocumentDirectory, NSUserDomainMask, YES);
        NSString *documentsDirectory = [paths.get(0];
        NSString *filePath = [documentsDirectory stringByAppendingPathComponent:string];
        Log.d(TAG, "± kill for - ",filePath);

        NSFileManager *fileManager = [NSFileManager defaultManager];
        NSError *error;
        boolean success = [fileManager removeItemAtPath:filePath error:&error];
        if (!success) {
        getInstance().error = "File not found\n";
        result=NO;
        }
        */
        return result;
    }

    public void loadGlobal() {
            /*
        NSString *filePath = getInstance().fileName;
        //    Log.d(TAG, "± load global for - ",filePath);
        if (filePath) {
        reset];
        NSString *arrayText = [NSString stringWithContentsOfFile:filePath encoding:NSUTF8StringEncoding error:nil];
        if (arrayText) {
        getInstance().listOfProgram = [NSMutableArray arrayWithArray(arrayText componentsSeparatedByString: "\n"));
        getInstance().programCounter = ((self returnBaseCommand:getInstance().listOfProgram lastObject))intValue] + getInstance().autoStep;
        }
        } else {
        getInstance().error = "File not found\n";
        }
        */
    }

    public int returnIndexFromLine(String line) {
        int result = -2;
        for (int i = 0; i < getInstance().listOfProgram.size(); i++) {
            String currentLine = getInstance().listOfProgram.get(i).toString().split(" ")[0];
            if (currentLine.equals(line)) {
                result = i - 1;
                Log.d(TAG, "± returnIndexFromLine currentLine=" + currentLine + "   String line=" + line + " result=" + result);
            }
        }
        Log.d(TAG, "± returnIndexFromLine - " + result);
        return result;
    }

    public int returnIndexOf(ArrayList arr, String line) {
        int result = 0;
        for (int i = 0; i < arr.size(); i++) {
            String currentLine = arr.get(i).toString().split(" ")[0];
            if (currentLine.equals(line)) {
                result = i;
            }
        }
        return result;
    }

    public boolean let(String string) {
        boolean result = NO;
        if (string.contains("=")) {
            String value = resultFromString(variables.returnVarValue(string));//присваиваем результат рассчетов к переменной
            String varName = variables.returnVarName(string);
            int index = variables.makeVariableIndex(varName);
            if (getInstance().error.isEmpty()) {
                if (!variables.forbiddenVariable(varName) && varName.length() > 0) {
                    if (index == getInstance().variables.size()) {
                        getInstance().variables.add(addVariable(value, varName));
                        // Log.d(TAG, "± let new var - " + varName + " value=" + value);
                        //Log.d(TAG, "± let new var - " + getInstance().variables.get(index).getName().toString() + " value=" + getInstance().variables.get(index).getVar().toString());
                    } else {
                        getInstance().variables.set(index, addVariable(value, varName));
                        //Log.d(TAG, "± let exist var - " + varName + " value=" + value);
                    }
                    if (getInstance().error.isEmpty()) result = YES;
                } else {
                    getInstance().command = "";
                    getInstance().error = "Forbidden variable\n";
                    result = NO;
                    Log.d(TAG, "± let empty Forbidden variable");
                }
            }
        } else {
            getInstance().error = "Syntax error\n";
            Log.d(TAG, "± let Syntax error");
        }
        // for (int i = 0; i < getInstance().variables.size(); i++)
        //Log.d(TAG, "± let varName - " + getInstance().variables.get(i).getName()
        //        + " value=" + getInstance().variables.get(i).getVar() + " is String type=" + getInstance().variables.get(i).getStringType());
        return result;
    }

    public boolean setDim(String string) {
        Log.d(TAG, "± set dim..." + string);
        boolean result = YES;
        if (string.contains("=")) {
            ArraySet arrayDim = new ArraySet();
            NSRange rangeFirst = new NSRange(string.indexOf("("), 1);
            NSRange rangeSecond = new NSRange(string.indexOf(")"), 1);
            String name = string.substring(0, rangeFirst.location);
            String indexString = string.substring(rangeFirst.location + 1, rangeSecond.location);
            if (variables.variableIsPresent(indexString))
                indexString = variables.returnContainOfVariable(indexString);
            int index = 0;
            try {
                index = Integer.parseInt(indexString);
            } catch (NumberFormatException e) {
                Log.d(TAG, "± Wrong number format in setDim!");
            }
            Log.d(TAG, "± set dim index='" + string.substring(rangeFirst.location + 1, rangeSecond.location) + "' name='" + name + "'");
            rangeFirst = new NSRange(string.indexOf("="), 1);
            String value = string.substring(rangeFirst.location + 1);
            //если переменная строковая добавим кавычки но при этом содержимое не содержит кавычки
            if (name.contains("$") && !normaStr.isText(value)) value = "\"" + value + "\"";
            value = resultFromString(value); //присваиваем результат рассчетов к массиву
            String testValue = value.replace(" ", "");
            if (!testValue.isEmpty()) {
                if (name.contains("$")) {
                    Log.d(TAG, "± dim variable is string " + value);
                    if (value.equals("")) value = "\"\"";
                    if (normaStr.isText(value)) {
                        value = value.replace("\"", "");
                    } else {
                        value = stringFunc.returnStringResult(value);
                    }
                } else {
                    Log.d(TAG, "± dim variable is digits " + value);
                    if (value.isEmpty() || variables.variableIsString(value)) {
                        value = "";
                        getInstance().error = "Type mismatch\n";
                        result = NO;
                    }
                    value = String.valueOf(digitalFunc.returnMathResult(value));
                }

                for (int i = 0; i < getInstance().array.size(); i++) {
                    arrayDim = (ArraySet) getInstance().array.get(i);
                    String str = arrayDim.name;
                    if (str.equals(name)) {
                        arrayDim.value.set(index, value);
                        getInstance().array.set(i, arrayDim);
                    }
                    Log.d(TAG, "± dim name='" + name + "' index='" + index + "' value='" + value + "' error='" + getInstance().error + "' dim='" + arrayDim.value + "'");
                }
            } else {
                result = NO;
                getInstance().error = "Syntax error\n";
            }
        } else {
            result = NO;
            getInstance().error = "Syntax error\n";
        }
        return result;
    }

    public boolean initDim(String string) {
        boolean result = YES;
        //Log.d(TAG, "± initDim... " + string);
        ArrayList arr = new ArrayList<String>(Arrays.asList(string.split(",")));
        for (int i = 0; i < arr.size(); i++) {
            string = arr.get(i).toString();
            String name = null;
            String size;
            NSRange rangeFirst = new NSRange(string.indexOf("("), 1);
            if (rangeFirst.location != NSNotFound) {
                NSRange rangeSecond = new NSRange(string.indexOf(")"), 1);
                name = string.substring(0, rangeFirst.location);
                size = string.substring(rangeFirst.location + 1, rangeSecond.location);
                size = resultFromString(size);
                if (!variables.forbiddenVariable(name) && name.length() > 0 && getInstance().error.isEmpty()) {
                    int tmp = 0;
                    try {
                        tmp = (int) Float.parseFloat(size);
                    } catch (NumberFormatException e) {
                        Log.d(TAG, "± Wrong number format in initDim!");
                    }
                    variables.initArrayNameWithSize(name, tmp);
                    //Log.d(TAG, "± dim name - '" + name + "' size = " + size);
                } else {
                    getInstance().command = "";
                    getInstance().error = "Wrong dim variable name\n";
                    result = NO;
                }
            } else {
                getInstance().command = "";
                getInstance().error = "Wrong dim variable name\n";
                result = NO;
            }
            String testValue = name.replace(" ", "");
            if (testValue.equals("")) {
                getInstance().command = "";
                getInstance().error = "Wrong dim variable name\n";
                result = NO;
            }
        }
        //Log.d(TAG, "± initDim result - '" + result+"'");
        return result;
    }

    public VariableSet addVariable(String var, String name) {
        boolean strType = NO;
        //    Log.d(TAG, "± addVariable+ is string ",var);
        var = normaStr.removeSpaceInBeginAndEnd(var);
        if (var.equals("+")) var = "\"+\"";
        String value = var;
        if (name.contains("$")) {
            Log.d(TAG, "± addVariable+variable is string " + var);
            if (var.isEmpty()) var = "\"\"";
            strType = YES;
            if (normaStr.isText(value)) {
                value = value.replace("\"", "");
            } else {
                value = stringFunc.returnStringResult(var);
            }
        } else {
            Log.d(TAG, "± variable is digits " + var);
            if (var.isEmpty() || variables.variableIsString(var)) {
                value = "0";
                getInstance().error = "Type mismatch\n";
            }
            value = String.valueOf(digitalFunc.returnMathResult(value));
        }
        VariableSet result = new VariableSet();
        result.var = value;
        result.name = name;
        result.stringType = strType;
        return result;
    }

/*
    public boolean typeMismath(String var, boolean isStr) {
        boolean result = NO;
        NSCharacterSet * numberSet = ((NSCharacterSet characterSetWithCharactersInString:".0123456789"] invertedSet];
        String separator="\"";
        if (isStr)
        {
        if (!((var substring(1].equals(separator] || !((var.substring (var length]-1].equals(separator]) {
        result=YES;
        getInstance().error="Type mismatch\n";
        }
        }else{
        if  (var rangeOfCharacterFromSet:numberSet].location != NSNotFound) {
        result=YES;
        getInstance().error="Type mismatch\n";
        }
        }
        return result;
    }
*/

    public void programString(String string, String number) {
        boolean isReplace = NO;
        int indexForReplace = 0;
        int indexForInsert = 0;
        int append = 0;
        getInstance().listOfStrings.clear();
        for (int i = 0; i < getInstance().listOfProgram.size(); i++) {
            String currentLineNimber = getInstance().listOfProgram.get(i).toString().split(" ")[0];
            if (currentLineNimber.equals(number)) {
                isReplace = YES;
                indexForReplace = i;
            }
            try {
                if (Integer.parseInt(currentLineNimber) < Integer.parseInt(number)) {
                    indexForInsert = i;
                    append = 1;
                }
            } catch (NumberFormatException e) {
                Log.d(TAG, "± Wrong number format in programString!");
            }
        }
        if (string.length() > number.length() + 1) {
            if (isReplace) {
                getInstance().listOfProgram.set(indexForReplace, string);
            } else {
                getInstance().listOfProgram.add(indexForInsert + append, string);
            }
            getInstance().programCounter = getInstance().programCounter + getInstance().autoStep;
            getInstance().listOfStrings.add(getInstance().programCounter + " ");
        } else {
            getInstance().error = "Undefined line number\n";
        }
    }

    public void autoProgramSet(String string) {
        getInstance().listOfStrings.clear();
        getInstance().listOfProgram.add(string);
        getInstance().programCounter = getInstance().programCounter + getInstance().autoStep;
        getInstance().listOfStrings.add(getInstance().programCounter + " ");
    }

    public void autoProgramStop() {
        getInstance().listOfStrings.clear();
        getInstance().listOfStrings.add(getInstance().programCounter + " ");
        getInstance().autoSet = NO;
        Log.d(TAG, "± autoProgramStop");
    }
}
