package com.neronov.aleksei.mcxbasic;

import android.text.TextUtils;
import android.util.Log;

import org.mariuszgromada.math.mxparser.Expression;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.Random;

import static com.neronov.aleksei.mcxbasic.GlobalVars.getInstance;

/**
 * Created by Aleksei Neronov on 13.03.16.
 */
public class DigitalFunc {
    private static final int NSNotFound = -1;
    private static final boolean NO = false;
    private static final boolean YES = true;
    private static final String TAG = MainActivity.class.getSimpleName();
    NormalizeString normaStr = new NormalizeString();
    Variables variables = new Variables();

    String numberSet = "^-?[0-9]\\d*(\\.\\d+)?$";
    String invNumberSet = "[^0-9.-]+$";
    String alphaSet = "(?:[^a-zA-Z]+$)";
    String mathSet = "(?:[-+*/^])";
    String numberMathSet = "^[^0-9./^+*-]+$";
//    NSCharacterSet * numberSet = NSCharacterSet characterSetWithCharactersInString:".0123456789+-/*^" invertedSet;

    public boolean isOnlyDigits(String var) {
        boolean result = YES;
        //Log.d(TAG, "± isOnlyDigits !!!" + var + "!!!");
        if (!var.matches(numberSet)) result = NO;
        return result;
    }

    public boolean isMath(String string) {
        boolean result = NO;
        if (mathFunction(string) || isOnlyDigits(string) || variables.variableIsDigit(string))
            result = YES;
        return result;
    }

    public final boolean isOnlyDigitsWithMath(String s) {
        String nmSet = ".0123456789+-/*^";
        boolean containsDigit = YES;
        if (s != null && !s.isEmpty()) {
            for (char c : s.toCharArray()) {
                if (!nmSet.contains(String.valueOf(c))) {
                    containsDigit = NO;
                    break;
                }
            }
        }
        return containsDigit;
    }

    public boolean mathFunction(String string) {
        boolean result = NO;
        for (int i = 0; i < getInstance().listMathFunc.size(); i++) {
            NSRange range = new NSRange(string.toLowerCase().indexOf(getInstance().listMathFunc.get(i).toString()), getInstance().listMathFunc.get(i).toString().length());
            if (range.location != NSNotFound && !normaStr.insideText(string.toLowerCase(), range.location))
                result = YES;
        }
        return result;
    }

    public double returnMathResult(String str) {
        //Log.d(TAG, "± returnMathResult for " + str);
        double value = 0;
        if (str.length() > 0) {
            // подготовка
            String originalString = str;
            str = str.replace(" ", "");
            int index = 0;
            // замена переменных
            ArrayList arrAll = new ArrayList();
            arrAll = normaStr.stringSeparateAllToArray(str);
            for (int i = 0; i < arrAll.size(); i++)
                if (variables.variableIsPresent(arrAll.get(i).toString()))
                    arrAll.set(i, variables.returnContainOfVariable(arrAll.get(i).toString()));
            str = TextUtils.join("", arrAll);
            ArrayList<String> arr = new ArrayList<String>(Arrays.asList(str.split(mathSet)));
            ArrayList arrValue = new ArrayList();
            arrValue = arr;
            if (str.substring(0, 1).equals("-")) {
                arrValue.remove(0);
                arrValue.set(0, "-" + arrValue.get(0).toString());
            }
            ArrayList arrSign = new ArrayList();
            for (int i = 0; i < arrValue.size() - 1; i++) {
                index = index + arrValue.get(i).toString().length() + 1;
                NSRange range = new NSRange(index - 1, 1);
                arrSign.add(str.substring(range.location, range.location + range.length));
            }
            //Log.d(TAG, "± !returnMathResult for " + str);
            for (int i = 0; i < arrValue.size(); i++) {
                if (arrValue.get(i).toString().length() > 0)
                    if (arrValue.get(i).toString().substring(arrValue.get(i).toString().length() - 1).equals("("))
                        if (arrSign.get(i).toString().equals("-")) {
                            arrSign.remove(i);
                            String tmpstr = arrValue.get(i).toString() + "-" + arrValue.get(i + 1).toString();
                            arrValue.remove(i);
                            arrValue.set(i, tmpstr);
                        }
            }
            for (int i = 0; i < arrValue.size(); i++) {
                int l = 0;
                if (arrValue.get(i).toString().length() > 3) l = 4;
                String funcString = arrValue.get(i).toString().substring(0, l).toLowerCase();
                if (funcString.equals("val(")) {
                    if (arrValue.get(i).toString().contains(")")) {
                        String prefix = "val("; // string prefix, not needle prefix!
                        String suffix = ")"; // string suffix, not needle suffix!
                        NSRange range = new NSRange(prefix.length(), arrValue.get(i).toString().length() - prefix.length() - suffix.length());
                        String numStr = arrValue.get(i).toString().substring(range.location, range.location + range.length);
                        numStr = numStr.replace("\"", "");
                        if (variables.variableIsPresent(numStr))
                            numStr = variables.returnContainOfVariable(numStr);
                        numStr = TextUtils.join("", numStr.split(invNumberSet));
                        try {
                            arrValue.set(i, Double.parseDouble(numStr));
                        } catch (NumberFormatException e) {
                            Log.d(TAG, "± numStr=" + numStr + "Wrong number format in VAL");
                        }
                    } else {
                        getInstance().error = "Syntax error at - \n" + arrValue.get(i).toString() + "\n";
                    }
                } else if (funcString.equals("asc(")) {
                    if (arrValue.get(i).toString().contains(")")) {
                        String prefix = "asc(\""; // string prefix, not needle prefix!
                        String suffix = "\")"; // string suffix, not needle suffix!
                        NSRange range = new NSRange(prefix.length(), arrValue.get(i).toString().length() - prefix.length() - suffix.length());
                        if (!arrValue.get(i).toString().contains("\"")) {
                            prefix = "asc(";
                            suffix = ")";
                            range = new NSRange(prefix.length(), arrValue.get(i).toString().length() - prefix.length() - suffix.length());
                            String nameString = arrValue.get(i).toString().substring(range.location, range.location + range.length);
                            int index1 = variables.makeVariableIndex(nameString);
                            if (index1 > 0 && !nameString.isEmpty()) {
                                VariableSet varSet = (VariableSet) getInstance().variables.get(index1);
                                String asciiString = varSet.var.toString().substring(0, 1);
                                arrValue.set(i, asciiString);
                            } else {
                                arrValue.set(i, "0");
                            }
                        } else {
                            String asciiString = arrValue.get(i).toString().substring(range.location, range.location + range.length);
                            asciiString = String.valueOf((int) asciiString.charAt(0));
                            arrValue.set(i, asciiString);
                        }
                    } else {
                        getInstance().error = "Syntax error at - " + arrValue.get(i).toString() + "\n";
                    }
                } else if (funcString.equals("abs(")) {
                    NSRange close = new NSRange(arrValue.get(i).toString().indexOf(")"), 1);
                    if (close.location != NSNotFound) {
                        String prefix = "abs("; // string prefix, not needle prefix!
                        String suffix = ")"; // string suffix, not needle suffix!
                        NSRange range = new NSRange(prefix.length(), arrValue.get(i).toString().length() - prefix.length() - suffix.length());
                        String numStr = arrValue.get(i).toString().substring(range.location, range.location + range.length);
                        double pre = 0;
                        try {
                            pre = Math.abs(Double.parseDouble(numStr));
                        } catch (NumberFormatException e) {
                            Log.d(TAG, "± str=" + str + "Wrong number format in ABS");
                        }
                        arrValue.set(i, String.valueOf(pre));
                    } else {
                        getInstance().error = "Syntax error at - " + arrValue.get(i).toString() + "\n";
                    }
                } else if (funcString.equals("fix(")) {
                    NSRange close = new NSRange(arrValue.get(i).toString().indexOf(")"), 1);
                    if (close.location != NSNotFound) {
                        String prefix = "fix("; // string prefix, not needle prefix!
                        String suffix = ")"; // string suffix, not needle suffix!
                        NSRange range = new NSRange(prefix.length(), arrValue.get(i).toString().length() - prefix.length() - suffix.length());
                        String numStr = arrValue.get(i).toString().substring(range.location, range.location + range.length);
                        try {
                            arrValue.set(i, String.valueOf(Math.round(Double.parseDouble(numStr))));
                        } catch (NumberFormatException e) {
                            Log.d(TAG, "± numStr=" + numStr + "Wrong number format in VAL");
                        }
                    } else {
                        getInstance().error = "Syntax error at - " + arrValue.get(i).toString() + "\n";
                    }
                } else if (funcString.equals("rnd(")) {
                    NSRange close = new NSRange(arrValue.get(i).toString().indexOf(")"), 1);
                    if (close.location != NSNotFound) {
                        String prefix = "rnd("; // string prefix, not needle prefix!
                        String suffix = ")"; // string suffix, not needle suffix!
                        NSRange range = new NSRange(prefix.length(), arrValue.get(i).toString().length() - prefix.length() - suffix.length());
                        String numStr = arrValue.get(i).toString().substring(range.location, range.location + range.length);
                        try {
                            Random r = new Random();
                            int rint = r.nextInt(Integer.parseInt(numStr));
                            arrValue.set(i, String.valueOf(rint));
                        } catch (NumberFormatException e) {
                            Log.d(TAG, "± numStr=" + numStr + "Wrong number format in VAL");
                        }
                    } else {
                        getInstance().error = "Syntax error at - " + arrValue.get(i).toString() + "\n";
                    }
                } else if (funcString.equals("inst")) {
                    if (arrValue.get(i).toString().contains(")")) {
                        String prefix = "instr("; // string prefix, not needle prefix!
                        String suffix = ")"; // string suffix, not needle suffix!
                        NSRange range = new NSRange(prefix.length(), originalString.length() - prefix.length() - suffix.length());
                        String normStr = originalString.substring(range.location, range.location + range.length);
                        normStr = normStr.replace("+", ",");
                        String varNm = normStr.replace(" ", "").split(",")[0];
                        if (variables.variableIsPresent(varNm)) {
                            normStr = normStr.replace(varNm, "\"" + variables.returnContainOfVariable(varNm) + "\"");
                        }
                        varNm = normStr.replace(" ", "").split(",")[1];
                        if (variables.variableIsPresent(varNm)) {
                            normStr = normStr.replace(varNm, "\"" + variables.returnContainOfVariable(varNm) + "\"");
                        }
                        int indLoc = 0;
                        ArrayList arr1 = new ArrayList();
                        arr1 = normaStr.extractTextToArray(normStr);
                        if (getInstance().error.isEmpty())
                            indLoc = arr1.get(0).toString().indexOf(arr1.get(1).toString());
                        if (range.location == NSNotFound) {
                            Log.d(TAG, "± string was not found");
                            arrValue.set(i, "0");
                        } else {
                            arrValue.set(i, String.valueOf(indLoc + 1));
                        }
                    } else {
                        getInstance().error = "Syntax error at - " + arrValue.get(i).toString() + "\n";
                    }
                } else if (funcString.equals("len(")) {
                    if (arrValue.get(i).toString().contains(")")) {
                        String prefix = "len("; // string prefix, not needle prefix!
                        String suffix = ")"; // string suffix, not needle suffix!
                        NSRange range = new NSRange(prefix.length(), originalString.length() - prefix.length() - suffix.length());
                        String normStr = originalString.substring(range.location, range.location + range.length);
                        normStr = normStr.replace("+", ",");
                        String varNm = normStr.replace(" ", "").split(",")[0];
                        if (variables.variableIsPresent(varNm)) {
                            normStr = normStr.replace(varNm, variables.returnContainOfVariable(varNm));
                        }
                        normStr = normStr.replace("\"", "");
                        if (getInstance().error.length() != 0) {
                            Log.d(TAG, "± string was not found");
                            arrValue.set(i, "0");
                        } else {
                            arrValue.set(i, String.valueOf(normStr.length()));
                        }
                    } else {
                        getInstance().error = "Syntax error at - " + arrValue.get(i).toString() + "\n";
                    }
                } else if (arrValue.get(i).toString().matches("[a-zA-Z]+")) { //если только буквы
                    index = variables.makeVariableIndex(arrValue.get(i).toString());
                    if (variables.variableIsPresent(arrValue.get(i).toString())) {
                        VariableSet varSet = getInstance().variables.get(index);
                        arrValue.set(i, varSet.var);
                    } else {
                        //getInstance().error = "Syntax error\n";
                        arrValue.set(i, "0");
                        Log.d(TAG, "± !!!Variable not excist arrValue=" + arrValue);
                    }
                }
            }
            //Log.d(TAG, "± !!!!!!!!! arrValue="+arrValue);
            str = arrValue.get(0).toString();
            for (int i = 1; i < arrValue.size(); i++)
                str = str + arrSign.get(i - 1).toString() + arrValue.get(i).toString();

            str = str.replace("log(", "ln(");
            str = str.replace("sqr(", "sqrt(");
            str = str.replace("atn(", "atan(");
            str = str.replace("cint(", "ceil(");
            str = str.replace("int(", "floor(");
            /*
            str=str stringByReplacingOccurrencesOfString:"rnd(" withString:"random(0,";
            */
            if (getInstance().error.equals("")) {
                Expression expression = new Expression(str);
                if (expression.checkSyntax()) {
                    //Log.d(TAG, "± !!!!!!!!! RESULTING TEST " + expression.getExpressionString() + "=" + expression.calculate()+" str="+str);
                    value = expression.calculate();
                } else {
                    Log.d(TAG, "± in expression " + expression.getErrorMessage());
                    getInstance().error = "Error " + expression.getErrorMessage() + "\n";
                }
            }
        }
        return value;
    }

    public String mathFunctionInMixedString(String string) {
        String result = "";
        int index = 0;
        // делаем замену -- и +- исключая текст
        ArrayList arrTemp = new ArrayList(normaStr.extractTextAndOtherToArray(string));
        for (int i = 0; i < arrTemp.size(); i++) {
            String stemp = arrTemp.get(i).toString();
            if (!normaStr.isText(stemp)) {
                stemp = stemp.replaceAll("\\+-", "-");
                stemp = stemp.replaceAll("--", "+");
            }
            arrTemp.set(i, stemp);
        }
        string = "";
        for (int i = 0; i < arrTemp.size(); i++) {
            string = string + arrTemp.get(i).toString(); //склеиваем строку из массива
        }
        ArrayList arrValue = new ArrayList(normaStr.stringAndDigitsSeparateToArray(string));
        if (string.substring(0, 1).equals("-")) {
            arrValue.remove(0);
            arrValue.set(0, "-" + arrValue.get(0).toString());
        }
        ArrayList arrSign = new ArrayList();
        for (int i = 0; i < arrValue.size() - 1; i++) {
            index = index + arrValue.get(i).toString().length() + 1;
            NSRange range = new NSRange(index - 1, 1);
            arrSign.add(string.substring(range.location, range.location + range.length));
        }
        if (arrSign.size() > 1)
            for (int i = 1; i < arrSign.size(); i++) {
                if (arrSign.get(i - 1).toString().equals(",") && arrSign.get(i).toString().equals("-")) {
                    arrSign.remove(i);
                    arrValue.remove(i);
                    arrValue.set(i, "-" + arrValue.get(i).toString());
                }
            }
        boolean adding = NO;
        int indAdd = 0;
        ArrayList arrValueDelete = new ArrayList();
        ArrayList arrSignDelete = new ArrayList();
        if (arrValue.size() > 2) for (int i = 0; i < arrValue.size() - 1; i++) {
            String notext = normaStr.removeText(arrValue.get(i).toString());
            if (mathFunction(notext)) {
                adding = YES;
                indAdd = i;
            }
            if (adding) {
                String addString = arrValue.get(indAdd).toString() + arrSign.get(i).toString() + arrValue.get(i + 1).toString();
                arrValueDelete.add(arrValue.get(i + 1).toString());
                arrSignDelete.add(arrSign.get(i).toString());
                arrValue.set(indAdd, addString);
            }
            if (arrValue.get(i + 1).toString().contains(")")) {
                adding = NO;
            }
        }
        arrValue.remove(arrValueDelete);
        arrSign.remove(arrSignDelete);
        String str = "";
        String old = "";
        if (arrValue.size() > 1) {
            for (int i = 1; i < arrValue.size(); i++) {
                if (arrValue.get(i).toString().length() > 0) {
                    if (isMath(arrValue.get(i - 1).toString()) && isMath(arrValue.get(i).toString())
                            && !arrSign.get(i - 1).toString().equals(",") && !arrValue.get(i).toString().substring(0, 1).equals("\"")) {
                        String first;
                        if (normaStr.isText(arrValue.get(i - 1).toString())) {
                            first = arrValue.get(i - 1).toString();
                        } else {
                            first = String.valueOf(returnMathResult(arrValue.get(i - 1).toString()));
                        }
                        if (!str.isEmpty()) first = "";
                        if (!old.isEmpty()) result = old;
                        String second;
                        if (normaStr.isText(arrValue.get(i).toString())) {
                            second = arrValue.get(i).toString();
                        } else {
                            second = String.valueOf(returnMathResult(arrValue.get(i).toString()));
                        }
                        str = str + first + arrSign.get(i - 1).toString() + second;
                        str = str.replaceAll("--", "+");
                    } else {
                        if (!old.isEmpty()) result = old;
                        if (!str.isEmpty()) {
                            if (!normaStr.isText(str)) {
                                str = String.valueOf(returnMathResult(str));
                            }
                        } else {
                            str = arrValue.get(i - 1).toString();
                        }
                        old = result + str + arrSign.get(i - 1).toString();
                        result = result + str + arrSign.get(i - 1).toString() + arrValue.get(i).toString();
                        str = "";
                    }
                }
            }
        } else {
            result = string;
            if (arrValue.size() > 0)
                if (isMath(arrValue.get(0).toString()) && isMath(arrValue.get(0).toString())) {
                    if (normaStr.isText(string)) {
                        result = string;
                    } else {
                        result = String.valueOf(returnMathResult(string));
                    }
                }
        }
        if (!str.equals("")) {
            str = String.valueOf(returnMathResult(str));
            result = result + str;
            str = "";
        }
        return result;
    }

    /*  Этот метод не используется!!! ОН НЕ НУЖЕН НО ОСТАВЛЕН НА ВСЯКИЙ СЛУЧАЙ
 public String toMathFuncWithPrefix(String string, String prefix)
    {
        String suffix = ")"; // string suffix, not needle suffix!
        NSRange range = new NSRange(prefix.length(),string.length() - prefix.length() - suffix.length());
        String result = string.substring(range.location,range.location+range.length);
        Log.d(TAG, "± !!!!!returnMathResult string="+string+"result="+result);
        if (!result.matches(alphaSet)) rangeOfCharacterFromSet:alphaSet.location == NSNotFound) {
        int index = variables makeVariableIndex:result;
        VariableSet* varSet=globals.variables objectAtIndex:index;
        result = varSet.var;
    }
        return result;
    } */


}
