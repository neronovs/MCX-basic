package com.neronov.aleksei.mcxbasic;

import android.util.Log;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;

import static com.neronov.aleksei.mcxbasic.GlobalVars.getInstance;

/**
 * Created by Aleksei Neronov on 15.03.16.
 */
public class StringFunc {
    private static DigitalFunc digitalFunc = new DigitalFunc();
    private static NormalizeString normaStr = new NormalizeString();
    private static RunCommand runCommand = new RunCommand();
    private static Variables variables = new Variables();

    private static final String TAG = MainActivity.class.getSimpleName();
    private static final int NSNotFound = -1;
    private static final boolean NO = false;
    private static final boolean YES = true;

    public boolean stringFunction(String string) {
        boolean result = NO;
        for (int i = 0; i < getInstance().listStrFunc.size(); i++) {
            NSRange range = new NSRange(string.toLowerCase().indexOf(getInstance().listStrFunc.get(i).toString()), getInstance().listStrFunc.get(i).toString().length());
            if (range.location != NSNotFound && !normaStr.insideText(string.toLowerCase(), range.location))
                result = YES;
        }
        return result;
    }

    public ArrayList returnCaseInsensFromString(String str, String search) {
        return new ArrayList<String>(Arrays.asList(str.split(search)));
    }

    public String returnAfterStrFuncParse(String string) {
        String result;
        String tmpStr = string;
        for (int t = 0; t < getInstance().listStrFunc.size(); t++)
            if (tmpStr.toLowerCase().contains(getInstance().listStrFunc.get(t).toString())) {
                ArrayList arr = new ArrayList(returnCaseInsensFromString(tmpStr, getInstance().listStrFunc.get(t).toString()));
                int index = arr.get(0).toString().length() + getInstance().listStrFunc.get(t).toString().length();
                for (int i = 1; i < arr.size(); i++) {
                    if (normaStr.insideText(tmpStr, index)) {
                        arr.set(i - 1, arr.get(i - 1) + getInstance().listStrFunc.get(t).toString() + arr.get(i));
                        arr.remove(i);
                    }
                    if (i < arr.size())
                        index = index + arr.get(i).toString().length() + getInstance().listStrFunc.get(t).toString().length();
                }
                for (int i = 1; i < arr.size(); i++) {
                    NSRange range = new NSRange(arr.get(i).toString().indexOf(")"), 1);
                    if (range.location != NSNotFound) {
                        String forReplace = arr.get(i).toString().substring(0, range.location + 1);
                        forReplace = getInstance().listStrFunc.get(t).toString() + forReplace;
                        result = getInstance().listStrFunc.get(t).toString() + arr.get(i).toString();
                        String replacer;
                        if (range.location != NSNotFound) {
                            replacer = "\"" + parseStringFunc(forReplace) + "\"";
                            result = result.replace(forReplace, replacer);
                            arr.set(i, result);
                        } else {
                            arr.set(i, "");
                        }
                    }
                }
                tmpStr = "";
                for (int i = 0; i < arr.size(); i++) tmpStr = tmpStr + arr.get(i).toString();
            }
        return tmpStr;
    }


    public String parseStringFunc(String str) {

        String result = str;
        int l = 0;
        if (str.length() > 3) l = 4;
        String funcString = str.substring(0, l).toLowerCase();
        if (funcString.equals("bin$")) { //функция переводит десятичные в двоичные
            String prefix = "bin$("; // string prefix, not needle prefix!
            String suffix = ")"; // string suffix, not needle suffix!
            NSRange range = new NSRange(prefix.length(), str.length() - prefix.length() - suffix.length());
            String tmpStr = str.substring(range.location, range.location + range.length);
            tmpStr = runCommand.resultFromString(tmpStr);
            if (digitalFunc.isOnlyDigits(tmpStr)) { // если только цифры
                if (Integer.parseInt(tmpStr) >= 0)
                    result = Integer.toBinaryString(Integer.parseInt(tmpStr));
            } else {  // если переменные
                int index = variables.makeVariableIndex(tmpStr);
                VariableSet varSet = getInstance().variables.get(index);
                String stringNumber = varSet.var;
                if (Integer.parseInt(stringNumber) >= 0)
                    result = Integer.toBinaryString(Integer.parseInt(stringNumber));
            }
        } else if (funcString.equals("chr$")) { // возвращает код ASCII символа
            String prefix = "chr$("; // string prefix, not needle prefix!
            String suffix = ")"; // string suffix, not needle suffix!
            NSRange range = new NSRange(prefix.length(), str.length() - prefix.length() - suffix.length());
            String tmpStr = str.substring(range.location, range.location + range.length);
            tmpStr = runCommand.resultFromString(tmpStr);
            if (digitalFunc.isOnlyDigits(tmpStr)) { // если только цифры
                if (Integer.parseInt(tmpStr) > 0 && Integer.parseInt(tmpStr) < 255)
                    result = Character.toString((char) Integer.parseInt(tmpStr));
            } else {  // если переменные
                int index = variables.makeVariableIndex(tmpStr);
                VariableSet varSet = getInstance().variables.get(index);
                String stringNumber = varSet.var;
                if (Integer.parseInt(stringNumber) > 0 && Integer.parseInt(stringNumber) < 255)
                    result = Character.toString((char) Integer.parseInt(stringNumber));
            }
        } else if (funcString.equals("spc$")) { //возвращает заданное количество пробелов
            String prefix = "spc$("; // string prefix, not needle prefix!
            String suffix = ")"; // string suffix, not needle suffix!
            NSRange range = new NSRange(prefix.length(), str.length() - prefix.length() - suffix.length());
            int spaces = 0;
            String tmpStr = str.substring(range.location, range.location + range.length);
            tmpStr = runCommand.resultFromString(tmpStr);
            if (digitalFunc.isOnlyDigits(tmpStr)) { // если только цифры
                spaces = Integer.parseInt(tmpStr);
            } else {  // если переменные
                int index = variables.makeVariableIndex(tmpStr);
                VariableSet varSet = getInstance().variables.get(index);
                tmpStr = varSet.var;
                spaces = Integer.parseInt(tmpStr);
            }
            tmpStr = "";
            for (int i = 0; i < spaces; i++) tmpStr = tmpStr + " ";
            result = tmpStr;
        } else if (funcString.equals("str$")) { //преобразовывает число в строку
            String prefix = "str$("; // string prefix, not needle prefix!
            String suffix = ")"; // string suffix, not needle suffix!
            NSRange range = new NSRange(prefix.length(), str.length() - prefix.length() - suffix.length());
            String tmpStr = str.substring(range.location, range.location + range.length);
            tmpStr = runCommand.resultFromString(tmpStr);
            if (digitalFunc.isMath(tmpStr)) {
                tmpStr = String.valueOf(digitalFunc.returnMathResult(tmpStr));
                Log.d(TAG, "± str=" + str + "tmpStr=" + tmpStr);
            } else {
                tmpStr = "";
            }
            result = tmpStr;
        } else if (funcString.equals("stri")) { //Возвращает количество заданных кодом ASCII символов
            String prefix = "string$("; // string prefix, not needle prefix!
            String suffix = ")"; // string suffix, not needle suffix!
            if (str.contains(suffix) && str.contains(prefix)) {
                NSRange range = new NSRange(prefix.length(), str.length() - prefix.length() - suffix.length());
                String normStr = str.substring(range.location, range.location + range.length);
                normStr = normStr.replaceAll("\\+", ",");
                String varNm = normStr.replace(" ", "").split(",")[0];
                if (variables.variableIsPresent(varNm)) {
                    normStr = normStr.replaceAll(varNm, variables.returnContainOfVariable(varNm));
                }
                List arr = normaStr.extractNumToArray(normStr);
                String tmpStr = String.valueOf(arr.get(1));
                result = "";
                int arr0 = 0;
                try {
                    arr0 = Integer.parseInt(arr.get(0).toString());
                } catch (NumberFormatException e) {
                    Log.d(TAG, "± str=" + str + "Wrong number format in string$!");
                }
                for (int i = 0; i < arr0; i++) {
                    result = result + (char) Integer.parseInt(tmpStr);
                }
            } else {
                getInstance().error = "Syntax error at - " + str + "\n";
            }
        } else if (funcString.equals("hex$")) { //функция переводит десятичные в шестнадцатиричные
            String prefix = "hex$("; // string prefix, not needle prefix!
            String suffix = ")"; // string suffix, not needle suffix!
            NSRange range = new NSRange(prefix.length(), str.length() - prefix.length() - suffix.length());
            String tmpStr = str.substring(range.location, range.location + range.length);
            tmpStr = runCommand.resultFromString(tmpStr);
            int num = 0;
            if (digitalFunc.isOnlyDigits(tmpStr)) { // если только цифры
                num = Integer.parseInt(tmpStr);
            } else {  // если переменные
                int index = variables.makeVariableIndex(tmpStr);
                VariableSet varSet = getInstance().variables.get(index);
                try {
                    num = Integer.parseInt(varSet.var);
                } catch (NumberFormatException e) {
                    Log.d(TAG, "± str=" + str + "Wrong number format in hex$!");
                }
            }
            result = Integer.toString(num, 16);
        } else if (funcString.equals("left")) { // подстрока из строки x знаков с лева
            String prefix = "left$("; // string prefix, not needle prefix!
            String suffix = ")"; // string suffix, not needle suffix!
            if (str.contains(suffix) && str.contains(prefix)) {
                NSRange range = new NSRange(prefix.length(), str.length() - prefix.length() - suffix.length());
                String normStr = str.substring(range.location, range.location + range.length);
                normStr = normStr.replaceAll("\\+", ",");
                String varNm = normStr.replaceAll(" ", "").split(",")[0];
                if (variables.variableIsPresent(varNm)) {
                    normStr = normStr.replaceAll(varNm, "\"" + variables.returnContainOfVariable(varNm) + "\"");
                }
                ArrayList arr = new ArrayList();
                arr = normaStr.extractTextAndNumToArray(normStr);
                if (getInstance().error.isEmpty())
                    range = new NSRange(arr.get(0).toString().indexOf(arr.get(1).toString()), arr.get(1).toString().length());
                int ind = 0;
                try {
                    ind = Integer.parseInt(arr.get(1).toString());
                } catch (NumberFormatException e) {
                    Log.d(TAG, "± str=" + str + "Wrong number format in left$!");
                }
                if (arr.get(0).toString().length() < ind || arr.get(0).toString().length() < 1) {
                    Log.d(TAG, "string was not extracted");
                    result = "";
                } else {
                    result = arr.get(0).toString().substring(0, ind);
                }
            } else {
                getInstance().error = "Syntax error at - " + str + "\n";
            }
        } else if (funcString.equals("righ")) { // подстрока из строки x знаков с права
            String prefix = "right$("; // string prefix, not needle prefix!
            String suffix = ")"; // string suffix, not needle suffix!
            if (str.contains(suffix) && str.contains(prefix)) {
                NSRange range = new NSRange(prefix.length(), str.length() - prefix.length() - suffix.length());
                String normStr = str.substring(range.location, range.location + range.length);
                normStr = normStr.replaceAll("\\+", ",");
                String varNm = normStr.replaceAll(" ", "").split(",")[0];
                if (variables.variableIsPresent(varNm)) {
                    normStr = normStr.replaceAll(varNm, "\"" + variables.returnContainOfVariable(varNm) + "\"");
                }
                ArrayList arr = new ArrayList();
                arr = normaStr.extractTextAndNumToArray(normStr);
                if (getInstance().error.isEmpty())
                    range = new NSRange(arr.get(0).toString().indexOf(arr.get(1).toString()), arr.get(1).toString().length());
                int ind = 0;
                try {
                    ind = Integer.parseInt(arr.get(1).toString());
                } catch (NumberFormatException e) {
                    Log.d(TAG, "± str=" + str + "Wrong number format in left$!");
                }
                if (arr.get(0).toString().length() < ind || arr.get(0).toString().length() < 1) {
                    Log.d(TAG, "string was not extracted");
                    result = "";
                } else {
                    result = arr.get(0).toString().substring(arr.get(0).toString().length() - ind, arr.get(0).toString().length());
                }
            } else {
                getInstance().error = "Syntax error at - " + str + "\n";
            }
        } else if (funcString.equals("mid$")) { // подстрока из строки с налальной позиции x и длинной y
            String prefix = "mid$("; // string prefix, not needle prefix!
            String suffix = ")"; // string suffix, not needle suffix!
            if (str.contains(suffix) && str.contains(prefix)) {
                NSRange range = new NSRange(prefix.length(), str.length() - prefix.length() - suffix.length());
                String normStr = str.substring(range.location, range.location + range.length);
                normStr = normStr.replaceAll("\\+", ",");
                String varNm = normStr.replaceAll(" ", "").split(",")[0];
                if (variables.variableIsPresent(varNm)) {
                    normStr = normStr.replaceAll(varNm, "\"" + variables.returnContainOfVariable(varNm) + "\"");
                }
                ArrayList arr = new ArrayList();
                arr = normaStr.extractTextAndNumToArray(normStr);
                int ind1 = 0, ind2 = 0;
                if (getInstance().error.isEmpty()) {
                    if (arr.size() == 3) {
                        try {
                            ind1 = Integer.parseInt(arr.get(1).toString());
                            ind2 = Integer.parseInt(arr.get(2).toString());
                        } catch (NumberFormatException e) {
                            Log.d(TAG, "± str=" + str + "Wrong number format in left$!");
                        }
                        range = new NSRange(ind1 - 1, ind2);
                    }
                    if (arr.size() == 2) {
                        try {
                            ind1 = Integer.parseInt(arr.get(1).toString());
                        } catch (NumberFormatException e) {
                            Log.d(TAG, "± str=" + str + "Wrong number format in left$!");
                        }
                        range = new NSRange(ind1 - 1, arr.get(0).toString().length() - ind1 + 1);
                    }
                }
                if (arr.get(0).toString().length() < ind1 || arr.get(0).toString().length() < 1) {
                    Log.d(TAG, "string was not extracted");
                    result = "";
                } else {
                    result = arr.get(0).toString().substring(range.location, range.location + range.length);
                }
            } else {
                getInstance().error = "Syntax error at - " + str + "\n";
            }
        } else if (funcString.equals("oct$")) { //функция переводит десятичные в восьмеричные
            String prefix = "oct$("; // string prefix, not needle prefix!
            String suffix = ")"; // string suffix, not needle suffix!
            NSRange range = new NSRange(prefix.length(), str.length() - prefix.length() - suffix.length());
            String tmpStr = str.substring(range.location, range.location + range.length);
            tmpStr = runCommand.resultFromString(tmpStr);
            int num = 0;
            if (digitalFunc.isOnlyDigits(tmpStr)) { // если только цифры
                num = Integer.parseInt(tmpStr);
            } else {  // если переменные
                int index = variables.makeVariableIndex(tmpStr);
                VariableSet varSet = getInstance().variables.get(index);
                try {
                    num = Integer.parseInt(varSet.var);
                } catch (NumberFormatException e) {
                    Log.d(TAG, "± str=" + str + "Wrong number format in hex$!");
                }
            }
            result = Integer.toString(num, 8);
        }
        Log.d(TAG, "± STRINGFUNC " + str + "='" + result + "'");
        return result;
    }

    public String returnStringResult(String str) {
        String result = str;
        if (!normaStr.isText(str)) {
            str = returnAfterStrFuncParse(str);
            result = str;
            String tempStr = "";
            String separator = "\"";
            if (str.contains("\\+") || str.contains(",")) {
                ArrayList arr = new ArrayList();
                arr = normaStr.stringSeparateToArray(str);
                int index = arr.get(0).toString().length();
                for (int i = 1; i < arr.size(); i++) {
                    if (normaStr.insideText(str, index)) ;
                    {
                        arr.set(i - 1, arr.get(i - 1).toString() + result.substring(arr.get(i - 1).toString().length(), arr.get(i - 1).toString().length() + 1) + arr.get(i).toString());
                        arr.remove(i);
                    }
                    if (arr.size() > 1) index = index + arr.get(i).toString().length() + 1;
                }
                for (int i = 0; i < arr.size(); i++) {
                    if (arr.get(i).toString().substring(0, 1).equals(separator) && arr.get(i).toString().substring(arr.get(i).toString().length() - 1).equals(separator)) {
                        tempStr = tempStr + arr.get(i).toString().split(separator)[1];
                    } else {
                        if (digitalFunc.isOnlyDigits(arr.get(i).toString())) //если это только цифры !
                        {
                            tempStr = tempStr + arr.get(i).toString();
                        } else {
                            if (variables.variableIsPresent(arr.get(i).toString())) {
                                VariableSet varSet = getInstance().variables.get(variables.makeVariableIndex(arr.get(i).toString()));
                                tempStr = tempStr + varSet.var;
                            } else {
                                tempStr = "";
                                getInstance().error = "Variable not excist\n";
                                Log.d(TAG, "± Variable not excist");
                            }
                        }
                    }
                }
                result = tempStr;
            } else {
                if (digitalFunc.isOnlyDigits(str)) {
                    tempStr = str;
                } else {
                    if (str.substring(0, 1).equals(separator) && str.substring(str.length() - 1).equals(separator)) {
                        tempStr = tempStr + str.split(separator)[1];
                    } else {
                        if (variables.variableIsPresent(str)) {
                            VariableSet varSet = getInstance().variables.get(variables.makeVariableIndex(str));
                            tempStr = varSet.var;
                        } else {
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
