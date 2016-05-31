package com.neronov.aleksei.mcxbasic;

import java.util.ArrayList;
import java.util.Arrays;

/**
 * Created by newuser on 12.03.16.
 */
public class GlobalVars {

    public static final String version1 = "MCX Basic version ";
    public static final String version2 = "Copyright 2015-16 by Neronov Aleksey\n";
    public static final String version3 = "Type 'help' for list of command\n";

    public String error;
    public String runnedLine;
    public String command;
    public String commandIf;
    public String input;
    public String fileName;
    public String keyScan;

    public ArrayList listOfStrings;
    public ArrayList data;
    public ArrayList dataIndex;
    public ArrayList array;
    public ArrayList<VariableSet> variables;
    public ArrayList listOfProgram;
    public ArrayList gosubArray;
    public ArrayList forArray;
    public ArrayList listOfCommands;
    public ArrayList listStrFunc;
    public ArrayList listMathFunc;
    public ArrayList listOfAll;

    public boolean run;
    public boolean autoSet;
    public boolean isOkSet;
    public boolean showError;
    public boolean scanKeyOn;

    public int dataReadIndex;
    public int autoStep;
    public int programCounter;
    public int runIndex;
    public int lineNumber;

    private GlobalVars() {
        String[] str = {"abs(x) - Returns the absolute value of a variable.",
                "asc(x$) - Returns the ASCII value of the first character in a string.",
                "atn(x) - Returns the arctangent of a variable.",
                "auto - Starts automatic line numbering. The line number will start with 10, incrementing in steps of 10. To stop swipe LEFT.",
                "beep - Simple 'beep' sound.",
                "bin$(x) - Returns a string with the binary representation of a (decimal) number.",
                "chr$(x) - Returns a string containing the ASCII character with the ASCII code of a variable.",
                "cint(x) - Returns the passed parameter rounded up.",
                "clear - Sets all numeric variables to zero, all strings to null.",
                "cls - Clears the screen.",
                "color <foreground>[,<background>] - Changes the color(s) to be used for the foreground and background.",
                "cos(x) - Returns the cosine of a variable in radians.",
                "csrlin - Returns the vertical coordinate of the cursor.",
                "data <data>,\"<data>\" - Store data in a program, to be read with the READ command.",
                "date x$ - Reads the date to string variable.",
                "delete <line number>[-<end line number>] - Erase part of an MCX-BASIC listing from memory.",
                "dim <variable>(<size>) - Specify the dimension of an array.",
                "end - Close all open files (if any) and end program execution.",
                "exp(x) - Returns e (mathematical constant) to the power of x.",
                "fix(x) - Returns the integer part of a variable by truncating the numbers after the decimal point.",
                "files - Displays saved BASIC program.",
                "for <variable>=<start value> to <end value> [step <increment>] ... next - Sets up a loop in order to repeat a block of commands until the counter variable has reached (or exceeded) the end value.",
                "goto <line number> - Jump to the specified line number and execute the commands from there.",
                "gosub <line number> ... return - Execute a subroutine located on the specified line number and continue with the next statement after the subroutine has completed.",
                "hex$(x) - Returns a string with the hexadecimal representation of a (decimal) number.",
                "help - Displays this help.",
                "if <condition expression> then <statement> [else <statement>] - Checks if a condition has been met and executes the statement specified after THEN. Optionally, if the condition has not been met the statement after ELSE will be run.",
                "inkey$ - Returns either a single character read from the keyboard or (if no key is pressed) an empty string.",
                "input [\"<prompt>\",]<x>[,<y>,<z>,...] - Shows an optional prompt followed by a question mark, storing the input into variables.",
                "instr(a$,b$) - Returns the position of the first occurrence of a B$ substring in A$ string.",
                "int(x) - Returns the largest integer equal to or smaller than a variable.",
                "kill \"name\" - Delete saved BASIC program.",
                "list [[<linenumber>] - [<linenumber>]] - Displays the program, or a part of it, in memory on screen.",
                "let variable=x - Assign data to the variable.",
                "left$(a$,x) - Returns a string composed of the leftmost x characters of the string a$.",
                "len(a$) - Returns the length of a string, including all non-printable characters.",
                "load \"name\" - Load saved BASIC program.",
                "log(a) - Returns the natural logarithm of a variable.",
                "mid$(a$,x[,y]) - Returns a substring of variable length (y) starting at a variable position (x) in an input string a$.",
                "new - Begin new program. Clear memory and variables.",
                "oct$(x) - Returns a string with the octal representation of a (decimal) number.",
                "pos - Returns the horizontal coordinate of the cursor.",
                "print <expression> - Display whatever comes after it on the screen.",
                "read x,x$ - Preparing data for variable values read-data.",
                "rem - REM statements are not executed, but are output exactly as entered when the program is listed.",
                "reset - Clear memory and variables. Restart system.",
                "restore [<linenumber>] - To allow DATA statements to be reread from a specified line.",
                "renum - Renumber the lines of a program, including all references to those lines by GOSUB, GOTO etc.",
                "right$(a$,x) - Returns a string composed of the right x characters of the string a$.",
                "run - Execute program. To run swipe RIGHT. To stop swipe LEFT.",
                "rnd(x) - Returns a random value between 0 and x.",
                "save \"name\" - Saves a BASIC program.",
                "share \"name\" - Saves a BASIC program and share.",
                "sin(x) - Returns the sine of a variable in radians.",
                "spc$(x) - Inserts a variable amount of spaces.",
                "sqr(x) - Returns the square root of a variable.",
                "string$(x,y) - Returns a string with a variable length (x), all containing either the same character, which is defined as an ASCII code (y).",
                "str$(x) - Returns a string representation of a numeric variable.",
                "time x$ - Reads the time to string variable.",
                "tan(x) - Returns the tangent of a variable.",
                "val(x$) - Returns the numerical value of the contents of a string, omitting leading spaces.",
                "varl - Displays all variables and values on screen.",
                "ver - Version of MCX Basic."};
        listOfCommands = new ArrayList();
        listOfCommands.addAll(Arrays.asList(str));

        String[] str1 = {"bin", "chr", "hex", "left", "right", "mid", "spc", "oct", "string", "str"};
        listStrFunc = new ArrayList();
        listStrFunc.addAll(Arrays.asList(str1));

        String[] str2 = {"atn", "cos", "abs", "asc", "exp", "fix", "instr"
                , "cint", "int", "len", "log", "rnd", "sin", "sqr", "tan", "val"};
        listMathFunc = new ArrayList();
        listMathFunc.addAll(Arrays.asList(str2));

        String[] str3 = {"auto", "beep", "list", "help", "print", "cls", "clear", "color", "csrlin", "data", "dim", "date", "time", "delete", "end",
                "files", "for", "to", "step", "next", "goto", "gosub", "input", "let", "load", "cload", "oct", "pos", "renum", "rem", "run",
                "reset", "read", "return", "save", "varl", "ver", "if", "then", "else", "cload", "csave", "kill", "inkey", "new", "restore",
                "share"};
        listOfAll = new ArrayList();
        listOfAll.addAll(Arrays.asList(str));
        listOfAll.addAll(Arrays.asList(str1));
        listOfAll.addAll(Arrays.asList(str2));
        listOfAll.addAll(Arrays.asList(str3));

        listOfStrings = new ArrayList();
        data = new ArrayList();
        dataIndex = new ArrayList();
        array = new ArrayList();
        variables = new ArrayList<VariableSet>();
        listOfProgram = new ArrayList();
        gosubArray = new ArrayList();
        forArray = new ArrayList();


        error = "";
        fileName = "";
        command = "";
        commandIf = "";
        input = "";
        runnedLine = "";
        keyScan = "";

        autoStep = 10;
        programCounter = 10;
        lineNumber = 0;
        dataReadIndex = 0;
        runIndex = 0;

        autoSet = false;
        isOkSet = true;
        run = false;
        showError = false;
        scanKeyOn = false;

    }

    private static GlobalVars mInstance = null;
    public static synchronized GlobalVars getInstance() {
        if (null == mInstance) {
            mInstance = new GlobalVars();
        }
        return mInstance;
    }

    public ArrayList getListOfStrings() {
        return this.listOfStrings;
    }

    public void setListOfStrings(ArrayList value) {
        listOfStrings = value;
    }

    public ArrayList getData() {
        return this.data;
    }

    public void setData(ArrayList value) {
        data = value;
    }

    public ArrayList getDataIndex() {
        return this.data;
    }

    public void setDataIndex(ArrayList value) {
        dataIndex = value;
    }

    public ArrayList getArray() {
        return this.array;
    }

    public void setArray(ArrayList value) {
        array = value;
    }

    public ArrayList<VariableSet> getVariables() {
        return this.variables;
    }

    public void setVariables(ArrayList<VariableSet> value) {
        variables = value;
    }

    public ArrayList getListOfProgram() {
        return this.listOfProgram;
    }

    public void setListOfProgram(ArrayList value) {
        listOfProgram = value;
    }

    public ArrayList getGosubArray() {
        return this.gosubArray;
    }

    public void setGosubArray(ArrayList value) {
        gosubArray = value;
    }

    public ArrayList getForArray() {
        return this.forArray;
    }

    public void setForArray(ArrayList value) {
        forArray = value;
    }

    public ArrayList getListOfCommands() {
        return this.listOfCommands;
    }

    public void setListOfCommands(ArrayList value) {
        listOfCommands = value;
    }

    public ArrayList getListStrFunc() {
        return this.listStrFunc;
    }

    public void setListStrFunc(ArrayList value) {
        listStrFunc = value;
    }

    public ArrayList getListMathFunc() {
        return this.listMathFunc;
    }

    public void setListMathFunc(ArrayList value) {
        listMathFunc = value;
    }

    public ArrayList getListOfAll() {
        return this.listOfAll;
    }

    public void setListOfAll(ArrayList value) {
        listOfAll = value;
    }

    public String getFileName() {
        return this.fileName;
    }

    public void setKeyScan(String value) {
        keyScan = value;
    }

    public String getKeyScan() {
        return this.keyScan;
    }

    public void setFileName(String value) {
        fileName = value;
    }

    public String getCommand() {
        return this.command;
    }

    public void setCommand(String value) {
        command = value;
    }

    public String getCommandIf() {
        return this.commandIf;
    }

    public void setCommandIf(String value) {
        commandIf = value;
    }

    public String getInput() {
        return this.input;
    }

    public void setInput(String value) {
        input = value;
    }

    public String getRunnedLine() {
        return this.runnedLine;
    }

    public void setRunnedLine(String value) {
        runnedLine = value;
    }

    public String getError() {
        return this.error;
    }

    public void setError(String value) {
        error = value;
    }

    public int getDataReadIndex() {
        return this.dataReadIndex;
    }

    public void setDataReadIndex(int value) {
        dataReadIndex = value;
    }

    public int getAutoStep() {
        return this.autoStep;
    }

    public void setAutoStep(int value) {
        autoStep = value;
    }

    public int getProgramCounter() {
        return this.programCounter;
    }

    public void setProgramCounter(int value) {
        programCounter = value;
    }

    public int getRunIndex() {
        return this.runIndex;
    }

    public void setRunIndex(int value) {
        runIndex = value;
    }

    public int getLineNumber() {
        return this.lineNumber;
    }

    public void setLineNumber(int value) {
        lineNumber = value;
    }

    public boolean getAutoSet() {
        return this.autoSet;
    }

    public void setAutoSet(boolean value) {
        autoSet = value;
    }

    public boolean getIsOkSet() {
        return this.isOkSet;
    }

    public void setIsOkSet(boolean value) {
        isOkSet = value;
    }

    public boolean getRun() {
        return this.run;
    }

    public void setRun(boolean value) {
        run = value;
    }

    public boolean getShowError() {
        return this.showError;
    }

    public void setShowError(boolean value) {
        showError = value;
    }

    public boolean getScanKeyOn() {
        return this.scanKeyOn;
    }

    public void setScanKeyOn(boolean value) {
        scanKeyOn = value;
    }

}
