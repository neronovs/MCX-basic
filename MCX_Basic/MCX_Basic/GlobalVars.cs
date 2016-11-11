using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCX_Basic
{
    //The delegate
    public delegate void FormDelegate();

    public class GlobalVars
    {
        private String version1 = "MCX Basic version ";
        private String version2 = "Copyright 2015-16 by Aleksey & Sergey Neronov";
        private String version3 = "Type 'help' for list of command\r\n";

        private String error;
        private String runnedLine;
        private String command;
        private String commandIf;
        private String input;
        private String fileName;
        private String currentFolder;
        private String keyScan;

        private List<string> listOfStrings;
        private List<string> data;
        private List<string> dataIndex;
        private List<ArraySet> array;
        private List<VariableSet> variables;
        private List<string> listOfProgram;
        private List<string> gosubArray;
        private List<ForSet> forArray;
        private List<string> listOfCommands;
        private List<string> listStrFunc;
        private List<string> listMathFunc;
        private List<string> listOfAll;

        private bool run;
        private bool autoSet;
        private bool isOkSet;
        private bool showError;
        private bool scanKeyOn;
        private bool firstStart;

        private int dataReadIndex;
        private int autoStep;
        private int programCounter;
        private int runIndex;
        private int lineNumber;

        public GlobalVars()
        {
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
                "instr(a$,b$) - Returns the position of the first occurrence of a B$ Substring in A$ string.",
                "int(x) - Returns the largest integer equal to or smaller than a variable.",
                "kill \"name\" - Delete saved BASIC program.",
                "list [[<linenumber>] - [<linenumber>]] - Displays the program, or a part of it, in memory on screen.",
                "let variable=x - Assign data to the variable.",
                "left$(a$,x) - Returns a string composed of the leftmost x characters of the string a$.",
                "len(a$) - Returns the length of a string, including all non-printable characters.",
                "load - Load saved BASIC program by choosing a file from a dialog window. Later - load \"name\" ",
                "log(a) - Returns the natural logarithm of a variable.",
                "mid$(a$,x[,y]) - Returns a Substring of variable length (y) starting at a variable position (x) in an input string a$.",
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
                "share \"name\" - Saves a BASIC program and share (open the selected file in a folder).",
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
            listOfCommands = new List<string>();
            listOfCommands.AddRange(str.ToList());

            String[] str1 = { "bin", "chr", "hex", "left", "right", "mid", "spc", "oct", "string", "str" };
            listStrFunc = new List<string>();
            listStrFunc.AddRange(str1.ToList());

            String[] str2 = {"atn", "cos", "abs", "asc", "exp", "fix", "instr"
                , "cint", "int", "len", "log", "rnd", "sin", "sqr", "tan", "val"};
            listMathFunc = new List<string>();
            listMathFunc.AddRange(str2.ToList());

            String[] str3 = {"auto", "beep", "list", "help", "print", "cls", "clear", "color", "csrlin", "data", "dim", "date", "time", "delete", "end",
                "files", "for", "to", "step", "next", "goto", "gosub", "input", "let", "load", "cload", "oct", "pos", "renum", "rem", "run",
                "reset", "read", "return", "save", "varl", "ver", "if", "then", "else", "cload", "csave", "kill", "inkey", "new", "restore",
                "share"};
            listOfAll = new List<string>();
            listOfAll.AddRange(str.ToList());
            listOfAll.AddRange(str1.ToList());
            listOfAll.AddRange(str2.ToList());
            listOfAll.AddRange(str3.ToList());

            listOfStrings = new List<string>();
            data = new List<string>();
            dataIndex = new List<string>();
            array = new List<ArraySet>();
            variables = new List<VariableSet>();
            listOfProgram = new List<string>();
            gosubArray = new List<string>();
            forArray = new List<ForSet>();


            error = "";
            fileName = "";
            currentFolder = Environment.CurrentDirectory + "/bas";
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
            firstStart = true;
        }

        private static GlobalVars mInstance = null;
        //public static synchronized GlobalVars.getInstance()
        public static GlobalVars getInstance()
        {
            if (null == mInstance)
            {
                mInstance = new GlobalVars();
            }
            return mInstance;
        }

        public List<string> ListOfStrings
        {
            get { return this.listOfStrings; }
            set { listOfStrings = value; }
        }

        public List<string> Data
        {
            get { return this.data; }
            set { data = value; }
        }

        public List<string> DataIndex
        {
            get { return this.dataIndex; }
            set { dataIndex = value; }
        }

        public List<ArraySet> Array
        {
            get { return this.array; }
            set { array = value; }
        }

        public List<VariableSet> Variables
        {
            get { return this.variables; }
            set { variables = value; }
        }

        public List<string> ListOfProgram
        {
            get { return this.listOfProgram; }
            set { listOfProgram = value; }
        }

        public List<string> GosubArray
        {
            get { return this.gosubArray; }
            set { gosubArray = value; }
        }

        public List<ForSet> ForArray
        {
            get { return this.forArray; }
            set { forArray = value; }
        }

        public List<string> ListOfCommands
        {
            get { return this.listOfCommands; }
            set { listOfCommands = value; }
        }

        public List<string> ListStrFunc
        {
            get { return this.listStrFunc; }
            set { listStrFunc = value; }
        }

        public List<string> ListMathFunc
        {
            get { return this.listMathFunc; }
            set { listMathFunc = value; }
        }

        public List<string> ListOfAll
        {
            get { return this.listOfAll; }
            set { listOfAll = value; }
        }

        public String FileName
        {
            get { return this.fileName; }
            set { fileName = value; }
        }

        public String KeyScan
        {
            get { return this.keyScan; }
            set { keyScan = value; }
        }

        public String Command
        {
            get { return this.command; }
            set { command = value; }
        }

        public String CommandIf
        {
            get { return this.commandIf; }
            set { commandIf = value; }
        }

        public String Input
        {
            get { return this.input; }
            set { input = value; }
        }

        public String RunnedLine
        {
            get { return this.runnedLine; }
            set { runnedLine = value; }
        }

        public String Error
        {
            get { return this.error; }
            set { error = value; }
        }

        public int DataReadIndex
        {
            get { return this.dataReadIndex; }
            set { dataReadIndex = value; }
        }

        public int AutoStep
        {
            get { return this.autoStep; }
            set { autoStep = value; }
        }

        public int ProgramCounter
        {
            get { return this.programCounter; }
            set { programCounter = value; }
        }

        public int RunIndex
        {
            get { return this.runIndex; }
            set { runIndex = value; }
        }

        public int LineNumber
        {
            get { return this.lineNumber; }
            set { lineNumber = value; }
        }

        public bool AutoSet
        {
            get { return this.autoSet; }
            set { autoSet = value; }
        }

        public bool IsOkSet
        {
            get { return this.isOkSet; }
            set { isOkSet = value; }
        }

        public bool Run
        {
            get { return this.run; }
            set { run = value; }
        }

        public bool ShowError
        {
            get { return this.showError; }
            set { showError = value; }
        }

        public bool ScanKeyOn
        {
            get { return this.scanKeyOn; }
            set { scanKeyOn = value; }
        }

        public string CurrentFolder
        {
            get { return this.currentFolder; }
            set { currentFolder = value; }
        }

        public bool FirstStart
        {
            get { return this.firstStart; }
            set { firstStart = value; }
        }

        public string Version1
        {
            get { return this.version1; }
        }
        public string Version2
        {
            get { return this.version2; }
        }
        public string Version3
        {
            get { return this.version3; }
        }

    }
}
