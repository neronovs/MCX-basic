package com.neronov.aleksei.mcxbasic;

import android.util.Log;

import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Calendar;
import java.util.Locale;

import static com.neronov.aleksei.mcxbasic.GlobalVars.getInstance;

/**
 * Created by Aleksei Neronov on 15.03.16.
 */
public class Variables {
    private static final int NSNotFound = -1;
    private static final boolean NO = false;
    private static final boolean YES = true;
    private static final String TAG = MainActivity.class.getSimpleName();

    DigitalFunc digitalFunc;
    NormalizeString normaStr = new NormalizeString();
    Variables variables;
    StringFunc stringFunc;

    public int makeVariableIndex(String name) {
        int result = getInstance().variables.size();
        if (result > 0) {
            for (int i = 0; i < getInstance().variables.size(); i++) {
                VariableSet varSet = getInstance().variables.get(i);
                if (varSet.name.equals(name)) {
                    result = i;
                }
            }
        }
        return result;
    }

    public boolean variableIsDigit(String name) {
        boolean result = NO;
        if (getInstance().variables.size() > 0 || variableIsPresent(name)) {
            for (int i = 0; i < getInstance().variables.size(); i++) {
                VariableSet varSet = getInstance().variables.get(i);
                if (varSet.name.equals(name)) {
                    if (!varSet.stringType) result = YES;
                }
            }
        }
        return result;
    }

    public boolean variableIsPresent(String name) {
        boolean result = NO;
        if (getInstance().variables.size() > 0) {
            for (int i = 0; i < getInstance().variables.size(); i++) {
                VariableSet varSet = getInstance().variables.get(i);
                if (varSet.name.equals(name)) {
                    result = YES;
                }
            }
        }
        return result;
    }

    public String returnContainOfVariable(String name) {
        String result = "";
        if (getInstance().variables.size() > 0 || variableIsPresent(name)) {
            for (int i = 0; i < getInstance().variables.size(); i++) {
                VariableSet varSet = getInstance().variables.get(i);
                if (varSet.name.equals(name)) {
                    result = varSet.var;
                }
            }
        }
        return result;
    }

    public void initArrayNameWithSize(String name, int size) {
        ArraySet array = new ArraySet();
        for (int i = 0; i < getInstance().array.size(); i++) {
            array = (ArraySet) getInstance().array.get(i);
            String str = array.name;
            if (str.equals(name)) {
                getInstance().array.remove(i);
            }
        }
        array = new ArraySet();
        for (int i = 0; i <= size; i++) {
            array.value.add("");
        }
        array.name = name;
        array.size = size;
        getInstance().array.add(array);
    }

    public boolean isArrayPresent(String name) {
        boolean result = NO;
        NSRange rangeFirst = new NSRange(name.indexOf("("), 1);
        NSRange rangeSecond = new NSRange(name.indexOf(")"), 1);
        ArraySet array = new ArraySet();
        String string = name;
        //Log.d(TAG, "± ========resultFromString NAME - "+ name);

        if (!normaStr.insideText(name, rangeFirst.location) && !normaStr.insideText(name, rangeSecond.location))
            while (rangeFirst.location != NSNotFound) {
                if (rangeFirst.location != NSNotFound) {
                    name = name.substring(0, rangeFirst.location);
                    for (int i = 0; i < getInstance().array.size(); i++) {
                        array = (ArraySet) getInstance().array.get(i);
                        String str = array.name;
                        if (str.equals(name)) {
                            result = YES;
                        }
                    }
                }
                string = string.substring(rangeSecond.location + 1, string.length());
                name = string;
                rangeFirst = new NSRange(name.indexOf("("), 1);
                rangeSecond = new NSRange(name.indexOf(")"), 1);
            }
        //Log.d(TAG, "± ========resultFromString result - "+ result);
        return result;
    }

    public boolean variableIsString(String name) {
        boolean result = NO;
        if (getInstance().variables.size() > 0 || variableIsPresent(name)) {
            for (int i = 0; i < getInstance().variables.size(); i++) {
                VariableSet varSet = (VariableSet) getInstance().variables.get(i);
                if (varSet.name.equals(name)) {
                    result = varSet.stringType;
                }
            }
        }
        return result;
    }

    public String returnVarValue(String string) {
        int index = string.indexOf("=");
        String result = string.substring(index + 1);
        if (string.equals("\"=\"")) result = "\"=\"";
        return result;
    }

    public boolean forbiddenVariable(String string) {
        boolean result = NO;
        for (int i = 0; i < getInstance().listOfAll.size(); i++) {
            int index = string.toLowerCase().indexOf(getInstance().listOfAll.get(i).toString());
            if (index != NSNotFound && index == 0) {
                result = YES;
                Log.d(TAG, "± forbidden var found!! " + string);
            }
        }
        if (result) {
            getInstance().error = "Vrong variable name\n";
        }
        return result;
    }

    public VariableSet addDateToVariable(String variable) {
        Calendar c = Calendar.getInstance();
        String format = "yyyy-MM-dd";
        SimpleDateFormat sdf = new SimpleDateFormat(format, Locale.US);
        VariableSet result = new VariableSet();
        result.var = sdf.format(c.getTime());
        result.name = variable;
        result.stringType = YES;
        Log.d(TAG, "± addDateToVariable " + result);
        return result;
    }

    public VariableSet addTimeToVariable(String variable) {
        Calendar c = Calendar.getInstance();
        String format = "HH:mm:ss";
        SimpleDateFormat sdf = new SimpleDateFormat(format, Locale.US);
        VariableSet result = new VariableSet();
        result.var = sdf.format(c.getTime());
        result.name = variable;
        result.stringType = YES;
        Log.d(TAG, "± addTimeToVariable " + result);
        return result;
    }

    public String returnVarName(String string) {
        String untilEqual = string.split("=")[0];
        int indexforAfterEqual = untilEqual.length();
        untilEqual = untilEqual.replace(" ", "");
        String result = untilEqual;
        String afterEqual = string.substring(indexforAfterEqual);
        if (result.substring(result.length() - 1).equals("$")) {
            if (!normaStr.isPairedQuotes(afterEqual)) {
                Log.d(TAG, "± Miss \" in string variable " + result + afterEqual);
                getInstance().error = "Miss \" in string variable\n";
            }
        }
        int l = 0;
        if (afterEqual.length() > 3) l = 4;
        if (!result.contains("$") && afterEqual.contains("\"") && !afterEqual.substring(0, l).equals("=asc")
                && !afterEqual.substring(0, l).equals("=ins") && !afterEqual.substring(0, l).equals("=val")) {
            getInstance().error = "Type mismatch\n";
        }
        String set = "(?:[^a-zA-Z]+$)";
        if (result.matches(set)) {
            result = "";
            getInstance().error = "Variable contains illegal characters\n";
        }
        return result;
    }

    public boolean getDateToVariable(String string) {
        boolean result = NO;
        String varName = returnVarName(string);
        int index = makeVariableIndex(varName);
        if (!forbiddenVariable(varName) && varName.length() > 0 && getInstance().error.isEmpty() && varName.contains("$")) {
            if (index == getInstance().variables.size()) {
                getInstance().variables.add(addDateToVariable(varName));
            } else {
                getInstance().variables.set(index, addDateToVariable(varName));
            }
            result = YES;
        } else {
            getInstance().command = "";
            getInstance().error = "Syntax error\n";
            Log.d(TAG, "± date let empty, error-" + getInstance().error);
            getInstance().isOkSet = NO;
        }
        return result;
    }

    public boolean getTimeToVariable(String string) {
        boolean result = NO;
        String varName = returnVarName(string);
        int index = makeVariableIndex(varName);
        if (!forbiddenVariable(varName) && varName.length() > 0 && getInstance().error.isEmpty() && varName.contains("$")) {
            if (index == getInstance().variables.size()) {
                getInstance().variables.add(addTimeToVariable(varName));
            } else {
                getInstance().variables.set(index, addTimeToVariable(varName));
            }
            result = YES;
        } else {
            getInstance().command = "";
            getInstance().error = "Syntax error\n";
            Log.d(TAG, "± time let empty, error-" + getInstance().error);
            getInstance().isOkSet = NO;
        }
        return result;
    }

    public String returnContainOfArray(String string)
    {
        String totalResult=string;
        String stringArr;
        ArrayList arr =new ArrayList(normaStr.stringAndDigitsSeparateToArray(string));
        if (arr.size()>0) for (int i=0; i<arr.size(); i++) {
        string=arr.get(i).toString();
        if (isArrayPresent(string)){
            string = string.replace(" ","");
            string = string.replace(",","");
            string = string.replace("+","");
            stringArr=string;
            ArraySet arrayDim= new ArraySet();
            NSRange rangeFirst = new NSRange(string.indexOf("("), 1);
            NSRange rangeSecond = new NSRange(string.indexOf(")"), 1);
            String name=string.substring(0,rangeFirst.location);
            String indexS=string.substring(rangeFirst.location+1,rangeSecond.location);
            int index=0;
            try {
                index = Integer.parseInt(indexS);
                if (variableIsPresent(indexS)) index = (int) digitalFunc.returnMathResult(indexS);
            } catch (NumberFormatException e) {
                Log.d(TAG, "± indexS=" + indexS + "Wrong number format in returnContainOfArray!");
            }
            String result="";
            while (rangeFirst.location!=NSNotFound){
                if (isArrayPresent(string)){
                    for (int ii=0; ii<getInstance().array.size(); ii++) {
                        arrayDim=(ArraySet)getInstance().array.get(ii);
                        String str=arrayDim.name;
                        if (str.equals(name)) {
                            if (index<arrayDim.value.size()){
                                result=arrayDim.value.get(index).toString();
                                if (!name.contains("$") && result.isEmpty()) result="0";
                                if (name.contains("$") && result.isEmpty()) result=" ";
                            } else {
                                getInstance().error="Subscript out of range\n";
                                result="error";
                            }
                        }
                    }
                }
                string=string.substring(rangeSecond.location+1);
                rangeFirst = new NSRange(name.indexOf("("), 1);
                rangeSecond = new NSRange(name.indexOf(")"), 1);
                if (string.length()>2) {
                    name=string.substring(0,rangeFirst.location);
                    try {
                        index = Integer.parseInt(string.substring(rangeFirst.location+1, rangeSecond.location));
                    } catch (NumberFormatException e) {
                        Log.d(TAG, "± indexS=" + indexS + "Wrong number format in returnContainOfArray!");
                    }
                }
            }
            if (name.contains("$")) {
                result="\""+result+"\"";
            }
            totalResult = totalResult.replace(stringArr,result);
        }
    }
        return totalResult;
    }
}
