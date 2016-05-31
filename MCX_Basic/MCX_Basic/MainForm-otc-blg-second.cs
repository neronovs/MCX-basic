using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

namespace MCX_Basic
{
    public partial class MainForm : Form
    {
        private static String TAG;// = MainActivity.class.getSimpleName();
        //private String commandWindow;
        private static int NSNotFound = -1;
        private static bool NO = false;
        private static bool YES = true;
        String currectLine = ""; //text in the current cursor line

        RunCommand runCommand = new RunCommand();

        private static NormalizeString normaStr = new NormalizeString();
        //    DigitalFunc digitalFunc = new DigitalFunc();
        //    Variables variables = new Variables();
        //    StringFunc stringFunc = new StringFunc();

        int position;
        bool inputMode;
        int inputCount;
        bool nextCommand;
        int keyOffset;
        int textIndex;

        //CGSize screenSize;
        Timer handler = new Timer();

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Maximized; //Makes the fullscreen window maximized
            FormBorderStyle = FormBorderStyle.None; //Hides the border of the fullscreen window
            commandWindow.Focus(); //Sets focus on the textBoxMain element

            // if (getInstance().firstStart)
            {
                Debug.WriteLine("± init in " + GlobalVars.getInstance().currentFolder);

                DirectoryInfo f = new DirectoryInfo(GlobalVars.getInstance().currentFolder); //Check if folder not excist - make new one
                if (!Directory.Exists(GlobalVars.getInstance().currentFolder))
                {
                    Debug.WriteLine("± Make dir " + GlobalVars.getInstance().currentFolder);
                    f.Create();
                }

                GlobalVars.getInstance().firstStart = false;
                GlobalVars.getInstance().scanKeyOn = NO;
            }
        }
        /*commandWindow = (String)findViewById(R.id.CommandWindow);
        commandWindow.addTextChangedListener(inputTextWatcher);
        commandWindow.setOnKeyListener(new View.OnKeyListener()

        {
        public bool onKey(View v, int keyCode, KeyEvent event) {

    if ((event.getAction() == KeyEvent.ACTION_DOWN) &&
            (keyCode == KeyEvent.KEYCODE_ENTER)) {
        //Debug.WriteLine("± Key pressed->" + keyCode);
        return false;//onEnterPress();
    }
     return false;
 }
 });

 }

 private void 
 (object sender, EventArgs e)
 {

 }*/

        /*
    commandWindow.setOnTouchListener(new OnSwipeTouchListener(MainActivity.this) {
       public void onSwipeTop() {
            String command="let a=89";
            Toast.makeText(MainActivity.this, command, Toast.Length_SHORT).show();
            runCommand.set(command);
            for (int i = 0; i < GlobalVars.getInstance().getListOfStrings().Count(); i++)
                addStringToCommandWindow(getInstance().getListOfStrings().get(i).ToString());
            Toast.makeText(MainActivity.this,command, Toast.Length_SHORT).show();
        }
        public void onSwipeRight() {
            Toast.makeText(MainActivity.this, "right", Toast.Length_SHORT).show();
        }
        public void onSwipeLeft() {
            Toast.makeText(MainActivity.this, "stop", Toast.Length_SHORT).show();
            returnCR();
            stopRunning();
            runCommand.autoProgramStop();
        }
        public void onSwipeBottom() {
            runCommand.set("list");
            for (int i = 0; i < GlobalVars.getInstance().getListOfStrings().Count(); i++)
                addStringToCommandWindow(getInstance().getListOfStrings().get(i).ToString());
            Toast.makeText(MainActivity.this, "list", Toast.Length_SHORT).show();
        }

    });
*//*

            reset();
            GlobalVars.getInstance().setScanKeyOn(true);
    }
}*//*

private TextWatcher inputTextWatcher = new TextWatcher()
{
        public void afterTextChanged(Editable s)
{
}*/


        public void beforeTextChanged(String s, int start, int count, int after)
        {
        }

        public void addStringToCommandWindow(String string_val)
        {
            //            commandWindow.Text += string_val;
            commandWindow.AppendText(string_val);
        }

        public void onTextChanged(String s, int start, int before, int count)
        {
            Debug.WriteLine(TAG, "± onTextChanged 1..." + count);
            if (count > 0)// && GlobalVars.getInstance().scanKeyOn)
            {
                Debug.WriteLine(TAG, "± onTextChanged 2..." + s.Substring(start,1));
                if (s.ToCharArray().ElementAt(start) == 10)
                {
                    GlobalVars.getInstance().lineNumber = getCurrentCursorLine(commandWindow.Text);
                    try
                    {
                        onEnterPress((s.ToCharArray().ElementAt(start)).ToString());
                    }
                    catch (IOException e)
                    {
                        //e.printStackTrace();
                    }
                }
            }
        }

        public void beepPlay()
        {
            Debug.WriteLine(TAG, "± Beep play begin ...");
            //MediaPlayer mp = MediaPlayer.create(this, getResources().getIdentifier("beep", "raw", getPackageName()));
            //mp.start();
        }

        public int getCurrentCursorLine(String editText) //Returns the current Y cursor position
        {
            int selectionStart = Selection.getSelectionStart(editText);
            Layout layout = editText.getLayout();

            if (!(selectionStart == -1))
            {
                return layout.getLineForOffset(selectionStart);
            }
            return -1;
        }

        public bool onEnterPress(String text) //throws IOException
        {
            commandWindow.setSelection(commandWindow.Text.Length);
            //        Debug.WriteLine(TAG, "± onEnterPress index="+(GlobalVars.getInstance().GlobalVars.getInstance().lineNumber - 1)+" total lines="+commandWindow.Text.ToString().Split('\n").Length);
            if (GlobalVars.getInstance().lineNumber - 1 >= commandWindow.Text.ToString().Split('\n').Length)
                GlobalVars.getInstance().lineNumber--;
            String textLine = commandWindow.Text.ToString().Split('\n')[GlobalVars.getInstance().lineNumber - 1];
            GlobalVars.getInstance().Command=textLine;
            boolshouldChangeText = true;
            GlobalVars.getInstance().KeyScan=text;
            textIndex = commandWindow.Text.ToString().Length;
            String[]
            lines = commandWindow.Text.ToString().Split('\n');
            if (inputMode)
            {
                if (!GlobalVars.getInstance().input.isEmpty())
                {
                    int extrAdd = 2;
                    if (GlobalVars.getInstance().listOfStrings.Count() == 0)
                    {
                        GlobalVars.getInstance().listOfStrings.Add("t");
                        extrAdd = 1;
                    }
                    Debug.WriteLine(TAG, "± input mode ='" + GlobalVars.getInstance().input + "' listofstring0='" + GlobalVars.getInstance().listOfStrings[0].ToString() + "'");
                    String[] arr = GlobalVars.getInstance().input.Substring(1).Split(',');
                    String entered;
                    int indexInput = GlobalVars.getInstance().listOfStrings[0].ToString().Length + extrAdd;
                    Debug.WriteLine(TAG, "± input-- '" + lines[lines.Length - 1] + "' indexed-'" + GlobalVars.getInstance().listOfStrings[0].ToString());
                    entered = lines[lines.Length - 1].Substring(indexInput);
                    String str = arr[inputCount] + "=" + entered;
                    if (arr[inputCount].Substring(arr[inputCount].Length - 1).Equals("$"))
                        str = arr[inputCount] + "=\"" + entered + "\"";
                    runCommand.set(str);
                    inputCount++;
                    if (inputCount >= arr.Length)
                    {
                        returnCR();
                        inputMode = NO;
                        inputCount = 0;
                        nextCommand = YES;
                    }
                    else {
                        returnCR();
                        addStringToCommandWindow("? ");
                    }
                }
                else {
                    returnCR();
                    inputMode = NO;
                    inputCount = 0;
                    syntaxError();
                    nextCommand = YES;
                }
            }
            else {
                if (!GlobalVars.getInstance().Command.Equals(""))
                {
                    if (GlobalVars.getInstance().isOkSet) returnCR();
                    if (!GlobalVars.getInstance().autoSet)
                    {
                        // Debug.WriteLine(TAG, "± autoSet=" + GlobalVars.getInstance().autoSet);
                        if (runCommand.set(GlobalVars.getInstance().Command))
                        {
                            //[commandWindow resignFirstResponder];
                            for (int i = 0; i < GlobalVars.getInstance().ListOfStrings.Count(); i++)
                                addStringToCommandWindow(GlobalVars.getInstance().ListOfStrings[i].ToString());
                            //[commandWindow becomeFirstResponder];
                            //   if (!GlobalVars.getInstance().autoSet || !GlobalVars.getInstance().run) printOk();
                        }
                        else {
                            otherCommands(GlobalVars.getInstance().command);
                            printOk();
                        }
                    }
                    else {
                        // Debug.WriteLine(TAG, "± !!!!!autoSet=" + GlobalVars.getInstance().autoSet);
                        runCommand.autoProgramSet(GlobalVars.getInstance().command);
                        for (int i = 0; i < GlobalVars.getInstance().listOfStrings.Count(); i++)
                        {
                            addStringToCommandWindow(GlobalVars.getInstance().listOfStrings[i].ToString());
                        }
                    }
                    if (GlobalVars.getInstance().Command.Count() > 3)
                        if (GlobalVars.getInstance().Command.Substring(0, 4).EqualsIgnoreCase("list"))
                            listColor();
                    //  return YES;
                }
            }
            shouldChangeText = GlobalVars.getInstance().Command.isEmpty();
            return shouldChangeText;
        }

        public void reset()
        {
            GlobalVars.getInstance().Error="";
            Debug.WriteLine(TAG, "± reset");
            commandWindow = (EditText)findViewById(R.id.CommandWindow);
            commandWindow.Text=(null);
            commandWindow.BackColor=(detectColor(4));
            commandWindow.setTextColor(detectColor(15));
            version();
            printOk();
            //print runCommand.set("a=2");
            //runCommand.set("a(1)=199");
            runCommand.set("print a,\"---\",7");
            inputMode = false;
            inputCount = 0;
            keyOffset = 0;
            commandWindow.setSelection(commandWindow.Text.Length);
        }








        private void commandWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                List<String> listOfTextBox = new List<String>(commandWindow.Lines); //takes all lines in the List massive
                int firstCharIndexInCurrentLine = commandWindow.GetFirstCharIndexOfCurrentLine();
                int nextLineNumber = commandWindow.GetLineFromCharIndex(firstCharIndexInCurrentLine) + 1;
                int currentLineNumber = commandWindow.GetLineFromCharIndex(firstCharIndexInCurrentLine);
                int firstCharIndexInNextLine = commandWindow.GetFirstCharIndexFromLine(nextLineNumber);
                int lastCharInCurrentLine = firstCharIndexInNextLine - 1;
                if (lastCharInCurrentLine > 0) e.SuppressKeyPress = true; //rejects of Enter movement
                if (lastCharInCurrentLine <= 0) lastCharInCurrentLine = commandWindow.Text.Length; else lastCharInCurrentLine = firstCharIndexInNextLine - 1;

                currectLine = listOfTextBox[currentLineNumber];
                if (runCommand.set(currectLine))
                {
                    for (int i = 0; i < GlobalVars.getInstance().listOfStrings.Count(); i++)
                        addStringToCommandWindow(GlobalVars.getInstance().listOfStrings[i]);
                }
                /*Debug.Print("______________________________");
                Debug.Print("current line num: {0}, next line num: {1}, first char of next line: {2}, Last char of current line: {3}", nextLineNumber - 1, nextLineNumber, firstCharIndexInNextLine, lastCharInCurrentLine);
                if (firstCharIndexInNextLine > 0)
                Debug.Print("First symbol of next line is: {0}", textBoxMain.Text.Substring(firstCharIndexInNextLine,1));
                Debug.Print("Current line is: {0}", currectLine);*/
            }
        }

        private void commandWindow_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
