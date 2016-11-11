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
using System.Reflection;

namespace MCX_Basic
{
    //public delegate void FormDelegate();

    public partial class MainForm : Form
    {
        private readonly int NSNotFound = -1;
        private readonly bool NO = false;
        private readonly bool YES = true;
        private readonly String TAG = MethodBase.GetCurrentMethod().DeclaringType.Name + ": ";
        //String currectLine = ""; //text in the current cursor line
        System.Windows.Forms.Timer timer;

        RunCommand runCommand;

        private static NormalizeString normaStr = new NormalizeString();
        //    DigitalFunc digitalFunc = new DigitalFunc();
        //    Variables variables = new Variables();
        //    StringFunc stringFunc = new StringFunc();

        //int position;
        bool inputMode;
        int inputCount;
        bool nextCommand;
        //int keyOffset;
        int textIndex;

        Timer handler = new Timer();

        FormDelegate formDelegate;
        //public static Form formMainUrl;

        /*
        // DLL libraries used to manage hotkeys
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vlc);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);
        const int MYACTION_HOTKEY_ID = 1;
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x0312 && m.WParam.ToInt32() == MYACTION_HOTKEY_ID)
            {
                // My hotkey has been typed

                // Do what you want here
                GlobalVars.getInstance().KeyScan = m.ToString();
            }
            base.WndProc(ref m);
        }
        */

        public MainForm()
        {
            InitializeComponent();
            timer = new System.Windows.Forms.Timer();
            timer.Tick += new EventHandler(ThreadMethod);
            runCommand = new RunCommand(this);

            formDelegate += Cls;
            //formMainUrl = this;

            // Modifier keys codes: Alt = 1, Ctrl = 2, Shift = 4, Win = 8
            // Compute the addition of each combination of the keys you want to be pressed
            // ALT+CTRL = 1 + 2 = 3 , CTRL+SHIFT = 2 + 4 = 6...
            //RegisterHotKey(this.Handle, MYACTION_HOTKEY_ID, 6, (int)Keys.F12);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Maximized; //Makes the fullscreen window maximized
            FormBorderStyle = FormBorderStyle.None; //Hides the border of the fullscreen window
            commandWindow.Focus(); //Sets focus on the textBoxMain element

            // if (getInstance().FirstStart)
            {
                Debug.WriteLine("± init in " + GlobalVars.getInstance().CurrentFolder);

                DirectoryInfo f = new DirectoryInfo(GlobalVars.getInstance().CurrentFolder); //Check if folder not excist - make new one
                if (!Directory.Exists(GlobalVars.getInstance().CurrentFolder))
                {
                    Debug.WriteLine("± Make dir " + GlobalVars.getInstance().CurrentFolder);
                    f.Create();
                }

                GlobalVars.getInstance().FirstStart = false;
                GlobalVars.getInstance().ScanKeyOn = NO;

                reset();
            }
        }

        public void onTextChanged(String s, int start, int before, int count)
        {
            Debug.WriteLine(TAG + "± onTextChanged 1..." + count);
            if (count > 0)// && GlobalVars.getInstance().ScanKeyOn)
            {
                Debug.WriteLine(TAG + "± onTextChanged 2..." + s.Substring(start, 1));
                if (s.ToCharArray().ElementAt(start) == 10)
                {
                    //GlobalVars.getInstance().LineNumber = getCurrentCursorLine(commandWindow.Text);
                    GlobalVars.getInstance().LineNumber = getCurrentCursorLine(commandWindow);
                    try
                    {
                        onEnterPress((s.ToCharArray().ElementAt(start)).ToString());
                    }
                    catch (IOException e)
                    {
                        //e.printStackTrace();
                        Debug.WriteLine(e.StackTrace);
                    }
                }
            }
        }

        public void beepPlay()
        {
            Debug.WriteLine(TAG + "± Beep play begin ...");
            System.Media.SystemSounds.Beep.Play();
        }

        public int getCurrentCursorLine(TextBox editText) //Returns the current Y cursor position
        {
            /*int selectionStart = editText.SelectionStart;
            //Layout layout = editText.getLayout();

            if (!(selectionStart == -1))
            {
                return editText.GetLineFromCharIndex(editText.GetFirstCharIndexOfCurrentLine());
            }
            return -1;*/

            return editText.GetLineFromCharIndex(editText.GetFirstCharIndexOfCurrentLine());
        }

        public bool onEnterPress(String text) //throws IOException
        {
            //commandWindow.setSelection(commandWindow.Text.Length);
            commandWindow.Select(commandWindow.Text.Length, 0);
            //        Debug.WriteLine(TAG + "± onEnterPress index="+(GlobalVars.getInstance().GlobalVars.getInstance().LineNumber - 1)+" total lines="+commandWindow.Text.ToString().Split('" + Environment.NewLine).Length);
            if (GlobalVars.getInstance().LineNumber - 1 >= commandWindow.Text.ToString().Split('\n').Length)
                GlobalVars.getInstance().LineNumber--;
            else GlobalVars.getInstance().LineNumber = commandWindow.Text.ToString().Split('\n').Length;

            String textLine = commandWindow.Text.ToString().Split('\n')[GlobalVars.getInstance().LineNumber - 1];
            GlobalVars.getInstance().Command = textLine;
            bool shouldChangeText = YES;
            //GlobalVars.getInstance().KeyScan = text;
            textIndex = commandWindow.Text.ToString().Length;
            String[] lines = commandWindow.Text.ToString().Split('\n');

            if (inputMode)
            {
                if (!string.IsNullOrEmpty(GlobalVars.getInstance().Input))
                {
                    int extrAdd = 2;
                    if (GlobalVars.getInstance().ListOfStrings.Count() == 0)
                    {
                        GlobalVars.getInstance().ListOfStrings.Add("t");
                        extrAdd = 1;
                    }
                    Debug.WriteLine(TAG + "± input mode ='" + GlobalVars.getInstance().Input + "' listofstring0='" 
                        + GlobalVars.getInstance().ListOfStrings[0].ToString() + "'");
                    String[] arr = GlobalVars.getInstance().Input.Substring(1).Split(',');
                    String entered;
                    int indexInput = GlobalVars.getInstance().ListOfStrings[0].ToString().Length + extrAdd;
                    Debug.WriteLine(TAG + "± input-- '" + lines[lines.Length - 1] + "' indexed-'" 
                        + GlobalVars.getInstance().ListOfStrings[0].ToString());
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
            // NOT inputMode
            else {
                if (!GlobalVars.getInstance().Command.Equals(""))
                {
                    Debug.WriteLine(TAG + "!!!isOkSet: " + GlobalVars.getInstance().IsOkSet);
                    if (GlobalVars.getInstance().IsOkSet) returnCR();
                    if (!GlobalVars.getInstance().AutoSet)
                    {
                        Debug.WriteLine(TAG + "± autoSet=" + GlobalVars.getInstance().AutoSet);
                        if (runCommand.set(GlobalVars.getInstance().Command))
                        {
                            //[commandWindow resignFirstResponder];
                            for (int i = 0; i < GlobalVars.getInstance().ListOfStrings.Count(); i++)
                               addStringToCommandWindow(GlobalVars.getInstance().ListOfStrings[i].ToString());
                               //prints "OK" on the screen
                               if (!GlobalVars.getInstance().AutoSet || !GlobalVars.getInstance().Run) printOk();
                        }
                        else {
                            otherCommands(GlobalVars.getInstance().Command);

                            //if length of the current line bigger than 0 and it's not run, then insert NEW LINE in the commandWindow 
                            try {
                                if (commandWindow.Lines.GetValue(GlobalVars.getInstance().LineNumber).ToString().Length > 0
                                    && !GlobalVars.getInstance().Run)
                                    returnCR();
                            } catch (Exception ex) { Debug.WriteLine(TAG + " Exception: " + ex); }

                            printOk();
                        }
                    }
                    else {
                        Debug.WriteLine(TAG + "± !!!!!autoSet=" + GlobalVars.getInstance().AutoSet);
                        runCommand.autoProgramSet(GlobalVars.getInstance().Command);
                        for (int i = 0; i < GlobalVars.getInstance().ListOfStrings.Count(); i++)
                        {
                            addStringToCommandWindow(GlobalVars.getInstance().ListOfStrings[i].ToString());
                        }
                    }
                    if (GlobalVars.getInstance().Command.Count() > 3)
                        if (GlobalVars.getInstance().Command.Substring(0, 4).ToLower().Equals("list")) //EqualsIgnoreCase
                            listColor();
                    //  return YES;
                }
            }
            shouldChangeText = string.IsNullOrEmpty(GlobalVars.getInstance().Command);
            
            if (GlobalVars.getInstance().ScanKeyOn)
            {
                GlobalVars.getInstance().KeyScan = inkey();
            }

            return shouldChangeText;
        }

        public void reset()
        {
            GlobalVars.getInstance().Error = "";
            Debug.WriteLine(TAG + "± reset");
            //commandWindow = (EditText)findViewById(R.id.CommandWindow);
            commandWindow.Text = (null);
            commandWindow.BackColor = (detectColor(4));
            commandWindow.ForeColor = (detectColor(15));
            version();
            printOk();

            /*String str = "123\"qwe\",WER\"DSFds\"AAA+\"  \"";
            List<String> array1 = new List<String>() { "123\"qwe\"", "WER\"DSFds\"AAA", "\"  \"" };
            List<string> array2 = new List<String>();
            //CollectionAssert.AreEqual(array1, normalizeString.extractTextToArray(str));
            NormalizeString normalizeString = new NormalizeString();
            array2 = normalizeString.stringSeparateToArray(str);
            Debug.WriteLine("array2 Count -> " + array2.Count);
            Debug.WriteLine("outcome info array2[0] -> " + array2[0]);
            Debug.WriteLine("outcome info array2[1] -> " + array2[1]);
            Debug.WriteLine("outcome info array2[2] -> " + array2[2]);*/
            //Debug.WriteLine("outcome info [3] -> " + array2[3]);
            //print runCommand.set("a=2");
            //runCommand.set("print abs(1)");
            //runCommand.set("print a,\"---\",7");
            inputMode = NO;
            inputCount = 0;
            //keyOffset = 0;
            //commandWindow.setSelection(commandWindow.Text.Length);
            commandWindow.Select(commandWindow.Text.Length, 0);
        }

        public Color detectColor(int colorNumber)
        {
            Debug.WriteLine(TAG + "± Color # " + colorNumber);
            Color result = Color.White;
            if (colorNumber == 1) result = Color.Black;
            if (colorNumber == 2) result = System.Drawing.ColorTranslator.FromHtml("#00E100");
            if (colorNumber == 3) result = System.Drawing.ColorTranslator.FromHtml("#27FF62");
            if (colorNumber == 4) result = System.Drawing.ColorTranslator.FromHtml("#0836A0");
            if (colorNumber == 5) result = System.Drawing.ColorTranslator.FromHtml("#5B60FF");
            if (colorNumber == 6) result = System.Drawing.ColorTranslator.FromHtml("#C50023");
            if (colorNumber == 7) result = System.Drawing.ColorTranslator.FromHtml("#0EDFFF");
            if (colorNumber == 8) result = System.Drawing.ColorTranslator.FromHtml("#FF0019");
            if (colorNumber == 9) result = System.Drawing.ColorTranslator.FromHtml("#FF6372");
            if (colorNumber == 10) result = System.Drawing.ColorTranslator.FromHtml("#D7E300");
            if (colorNumber == 11) result = System.Drawing.ColorTranslator.FromHtml("#CAD186");
            if (colorNumber == 12) result = System.Drawing.ColorTranslator.FromHtml("#009507");
            if (colorNumber == 13) result = System.Drawing.ColorTranslator.FromHtml("#DA0FB1");
            if (colorNumber == 14) result = System.Drawing.ColorTranslator.FromHtml("#ADADAD");
            if (colorNumber == 15) result = System.Drawing.ColorTranslator.FromHtml("#FFFFFF");
            if (colorNumber > 15)
            {

                GlobalVars.getInstance().Error = ("Incorrect color");
                Debug.WriteLine(TAG + "± Incorrect color" + Environment.NewLine);
            }
            return result;
        }

        public void listColor()
        {
            /*
            GlobalVars.getInstance() =[GlobalVars sharedInstance];
            digitalFunc =[[DigitalFunc alloc]init];
            NSCharacterSet * alphaSet =[[NSCharacterSet characterSetWithCharactersInString:@
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLKMNOPQRSTUVWXYZ"]invertedSet];
            NSString * string =[commandWindow text].lowercaseString;
            NSMutableArray * arrayOfRanges =[[NSMutableArray alloc]init];
            NSMutableArray * arrayOfRangesText =[[NSMutableArray alloc]init];
            NSMutableArray * arrayOfRangesNumber =[[NSMutableArray alloc]init];

            for (int i = 0; i <[GlobalVars.getInstance().ListOfAll count];
            i++){
            //NSRange searchRange = NSMakeRange(0,string.length);
            //NSRange searchRange = getViewableRange:commandWindow];
            NSRange searchRange = NSMakeRange(textIndex, string.length - textIndex);
            NSRange foundRange;
            while (searchRange.location < string.length) {
                searchRange.length = string.length - searchRange.location;
                foundRange =[string rangeOfString(GlobalVars.getInstance().ListOfAll.get(i]options:
                0 range:
                searchRange];
                if (foundRange.location != NSNotFound) {
                    // found an occurrence of the substring! do stuff here
                    searchRange.location = foundRange.location + foundRange.length;
                    bool prevEmpty = YES;
                    bool afterEmpty = YES;
                    if (foundRange.location + foundRange.length < string.length - 1) {
                        NSString * tmp =[string substringWithRange:
                        NSMakeRange(foundRange.location + foundRange.length, 1)];
                        //                    NSLog(@"colorWord ->%@",tmp);
                        if ([tmp rangeOfCharacterFromSet:alphaSet].location == NSNotFound)
                        prevEmpty = NO;
                    }
                    if (foundRange.location > 0) {
                        NSString * tmp =[string substringWithRange:
                        NSMakeRange(foundRange.location - 1, 1)];
                        if ([tmp rangeOfCharacterFromSet:alphaSet].location == NSNotFound)
                        prevEmpty = NO;
                    }
                    if (prevEmpty && afterEmpty) {
                        [arrayOfRanges addObject(NSValue valueWithRange:foundRange]];
                    }
                } else {
                    // no more substring to find
                    break;
                }
            }
        }

            NSRange searchRange = NSMakeRange(textIndex, string.length - textIndex);
            NSLog( @ "searchRange = %@", NSStringFromRange(searchRange));
            int counter = textIndex;
            NSArray * lines =[[string substringWithRange:searchRange]componentsSeparatedByString:@ "" + Environment.NewLine];
            for (int i = 0; i <[lines count];
            i++){

            NSString * base =returnBaseCommand(lines.get(i]];
            if ([digitalFunc isOnlyDigits:base]&&![base isEqual:@ ""]){ // manual program string set
                [arrayOfRangesNumber addObject(NSValue valueWithRange:
                NSMakeRange(counter, base.length)]];
            }

            int indexFirst = 0;
            BOOL foundFirst = NO;
            NSString * lineString =[lines.get(i];
            if (lineString.length > 0) {
                for (int i = 0; i <[lineString length];
                i++){
                    if ([lineString characterAtIndex:i]=='"' && !foundFirst){
                        foundFirst = YES;
                        indexFirst = i;
                    }
                    if ([lineString characterAtIndex:i]=='"' && foundFirst && indexFirst != i){
                        foundFirst = NO;
                        NSRange range = NSMakeRange(counter + indexFirst, i - indexFirst + 1);
                        [arrayOfRangesText addObject(NSValue valueWithRange:range]];
                    }
                }
            }

            counter = counter + (int)[[lines.get(i]length]+1;
        }

            if ([arrayOfRanges count]>0){

            NSMutableAttributedString * stringM =[[NSMutableAttributedString alloc]initWithString:
            commandWindow.text];
            [stringM addAttribute:NSForegroundColorAttributeName value:colorWithHexString:@
            "FFFFFF"]range:
            NSMakeRange(0, string.length)];
            [stringM addAttribute:NSFontAttributeName value(UIFont fontWithName:@ "Menlo" size:
            16.0]range:
            NSMakeRange(0, string.length)];

            for (int i = 0; i <[arrayOfRanges count];
            i++)
            [stringM addAttribute:NSForegroundColorAttributeName value:colorWithHexString:@
            "8EE67C"]range([arrayOfRanges.get(i]rangeValue]];

            for (int i = 0; i <[arrayOfRangesText count];
            i++)
            [stringM addAttribute:NSForegroundColorAttributeName value:colorWithHexString:@
            "DED27C"]range([arrayOfRangesText.get(i]rangeValue]];

            for (int i = 0; i <[arrayOfRangesNumber count];
            i++)
            [stringM addAttribute:NSForegroundColorAttributeName value:colorWithHexString:@
            "ADADAD"]range([arrayOfRangesNumber.get(i]rangeValue]];

            [commandWindow setAttributedText:stringM];
        }
        */
        }

        public void syntaxError()
        {
            GlobalVars.getInstance().IsOkSet = NO;
            if (GlobalVars.getInstance().Run)
            {
                GlobalVars.getInstance().Error = GlobalVars.getInstance().Error + " at line " + GlobalVars.getInstance().RunnedLine + Environment.NewLine;
            }
            stopRunning();
            addStringToCommandWindow(GlobalVars.getInstance().Error);
        }

        public void stopRunning()
        {
            //printOk();
            //handler.removeCallbacks(null);
            handler.Stop();
            GlobalVars.getInstance().RunIndex = 0;
            GlobalVars.getInstance().Run = NO;
            GlobalVars.getInstance().IsOkSet = YES;
            inputMode = NO;
            GlobalVars.getInstance().ScanKeyOn = NO;
            Debug.WriteLine(TAG + "Program is OVER!!!!! " + GlobalVars.getInstance().Error);
        }

        public void version()
        {
            runCommand.set("ver");
            printListOfStrings();
        }

        public void printListOfStrings()
        {
            for (int i = 0; i < GlobalVars.getInstance().ListOfStrings.Count(); i++)
            {
                String temp = GlobalVars.getInstance().ListOfStrings[i].ToString();
                addStringToCommandWindow(temp);
            }
            GlobalVars.getInstance().ListOfStrings.Clear();
        }

        public void printOk()
        {
            String[] lines = commandWindow.Text.ToString().Split('\n');
            if (lines.Length > 2)
                if (GlobalVars.getInstance().IsOkSet && !GlobalVars.getInstance().Run)
                {
                    addStringToCommandWindow("Ok" + Environment.NewLine);
                }
            //GlobalVars.getInstance().IsOkSet = (true);
        }

        public void addStringToCommandWindow(String string_val)
        {
            String temp = commandWindow.Text.ToString() + string_val;
            commandWindow.AppendText(string_val);
            commandWindow.Select(commandWindow.Text.Length, 0);
        }

        public void returnCR()
        {
            addStringToCommandWindow(Environment.NewLine);
        }
        
        private void commandWindow_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (e.KeyChar == 13)
                e.Handled = true;
        }

        public String inkey()
        {
            String res = "";

            //do {

                res = GlobalVars.getInstance().KeyScan;
                //System.Threading.Thread.Sleep(100);
            //} while (res == "");


            return res;
        }

        private void commandWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (GlobalVars.getInstance().ScanKeyOn)
            {
                var task = Task.Factory.StartNew(inkey);
                task.Wait();
                task = Task.Factory.StartNew(inkey);
                
                GlobalVars.getInstance().KeyScan = e.KeyData.ToString();
            }

            if (e.KeyCode == Keys.Enter)
            {
                /*List<String> listOfTextBox = new List<String>(commandWindow.Lines); //takes all lines in the List massive
                int firstCharIndexInCurrentLine = commandWindow.GetFirstCharIndexOfCurrentLine();
                int nextLineNumber = commandWindow.GetLineFromCharIndex(firstCharIndexInCurrentLine) + 1;
                int currentLineNumber = commandWindow.GetLineFromCharIndex(firstCharIndexInCurrentLine);
                int firstCharIndexInNextLine = commandWindow.GetFirstCharIndexFromLine(nextLineNumber);
                int lastCharInCurrentLine = firstCharIndexInNextLine - 1;
                if (lastCharInCurrentLine > 0) e.SuppressKeyPress = true; //rejects of Enter movement
                if (lastCharInCurrentLine <= 0) lastCharInCurrentLine = commandWindow.Text.Length; else lastCharInCurrentLine = firstCharIndexInNextLine - 1;
                currectLine = listOfTextBox[currentLineNumber];*/

                GlobalVars.getInstance().LineNumber = getCurrentCursorLine(commandWindow);
                onEnterPress(Environment.NewLine);//send symbol of "Enter" - "\n"

                /*if (runCommand.set(currectLine))
                {
                    for (int i = 0; i < GlobalVars.getInstance().ListOfStrings.Count(); i++)
                        addStringToCommandWindow(GlobalVars.getInstance().ListOfStrings[i]);
                }*/

            } else if (e.Control && e.KeyCode == Keys.C)
            {
                runCommand.autoProgramStop();
            }
        }

        public void setColor(String color)
        {
            int col = 1;
            try
            {
                col = int.Parse(color);
            }
            catch //(NumberFormatException e)
            {
                Debug.WriteLine(TAG + "± Background color=" + color + "Wrong number format in string$!");
            }
            commandWindow.ForeColor = (detectColor(col));
        }

        public void setBackground(String color)
        {
            int col = 1;
            try
            {
                col = int.Parse(color);
            }
            catch //(NumberFormatException e)
            {
                Debug.WriteLine(TAG + "± color=" + color + "Wrong number format in string$!");
            }
            commandWindow.BackColor = (detectColor(col));
        }

        public void runProgram()
        {

            if (nextCommand)
            {
                String untilSpace = GlobalVars.getInstance().ListOfProgram
                    [GlobalVars.getInstance().RunIndex].ToString().Split(' ')[0];
                GlobalVars.getInstance().RunnedLine = untilSpace;
                int indexforAfterSpace = untilSpace.Length;
                
                String commandRun = GlobalVars.getInstance().ListOfProgram[GlobalVars.getInstance().RunIndex].ToString().Substring(indexforAfterSpace + 1);
                if (commandRun.Substring(0, 1).Equals(" "))
                {
                    commandRun = normaStr.removeSpaceInBegin(commandRun);
                }
                Debug.WriteLine(TAG + "± " + GlobalVars.getInstance().RunIndex + " Command '" + commandRun + "'");
                if (!commandRun.Equals(""))
                {
                    if (runCommand.set(commandRun))
                    {
                        for (int n = 0; n < GlobalVars.getInstance().ListOfStrings.Count(); n++)
                        {
                            addStringToCommandWindow(GlobalVars.getInstance().ListOfStrings[n].ToString());
                        }
                    }
                    else {
                        otherCommands(commandRun);
                    }
                }
                GlobalVars.getInstance().RunIndex++;
                if (GlobalVars.getInstance().RunIndex >= GlobalVars.getInstance().ListOfProgram.Count()) stopRunning();
                if (!GlobalVars.getInstance().Error.Equals("")) stopRunning();
                if (!GlobalVars.getInstance().Run) stopRunning();
            }
            if (GlobalVars.getInstance().Run)
            {
                //Do something after 100ms
                timer.Interval = 100;
                //timer.Tick += new EventHandler(ThreadMethod);
                timer.Start();
            } else {
                timer.Stop();
                timer.Dispose();
            }
        }

        void ThreadMethod(object sender, EventArgs e)
        {
            try
            {
                //System.Threading.Thread.Sleep(1000);
                runProgram();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(TAG + ", Exception of Task_MethodAsync(): " + ex);
            }
        }

        public void share()
        {
            /*
                    NSURL* fn=fileToURL:GlobalVars.getInstance().FileName];
                    NSLog(@"shareCommand for '%@'",GlobalVars.getInstance().FileName);
                    NSString *texttoshare = @"MCX Basic file. Sent by share command.";
                    //    String listToShare = [GlobalVars.getInstance().ListOfProgram componentsJoinedByString: @"" + Environment.NewLine];
                    NSArray *activityItems = @[texttoshare,fn];
                    UIActivityViewController *activityVC = [[UIActivityViewController alloc] initWithActivityItems:activityItems applicationActivities:nil];
                    NSLog(@"share step 2");

                    //if iPhone
                    if (UI_USER_INTERFACE_IDIOM() == UIUserInterfaceIdiomPhone) {
                        presentViewController:activityVC animated:YES completion:nil];
                    }
                    //if iPad
                    else {
                        // Change Rect to position Popover
                        UIPopoverController *popup = [[UIPopoverController alloc] initWithContentViewController:activityVC];
                        [popup presentPopoverFromRect:CGRectMake(self.view.frame.size.width/2, self.view.frame.size.height/4, 0, 0)inView:self.view permittedArrowDirections:UIPopoverArrowDirectionAny animated:YES];
                    }
                    */
        }

#region LOAD file polymofphism

        public void load(String string_val)
        {
            NormalizeString normaStr = new NormalizeString();
            string_val = normaStr.removeSpaceInBegin(string_val);

            Stream myStream = null;
            string_val = string_val.Replace(".bas", "");
            string_val = string_val + ".bas";
            String filePath = (GlobalVars.getInstance().CurrentFolder + "\\" + string_val).Replace('/', '\\');

            try
            {
                if (File.Exists(filePath))
                {
                    myStream = File.OpenRead(filePath);
                    using (myStream)
                    {
                        // Insert code to read the stream here.
                        GlobalVars.getInstance().FileName = filePath;
                        reset();

                        myStream.Position = 0;
                        using (StreamReader reader = new StreamReader(myStream, Encoding.UTF8))
                        {
                            //RunCommand runCommand = new RunCommand(this);
                            string arrayText = reader.ReadToEnd();
                            Debug.WriteLine(TAG + "± reader.ReadToEnd() " + arrayText);
                            GlobalVars.getInstance().ListOfProgram = new List<string>(arrayText.Split('\n'));
                            GlobalVars.getInstance().ProgramCounter =
                                int.Parse(runCommand.returnBaseCommand(GlobalVars.getInstance().ListOfProgram
                                [GlobalVars.getInstance().ListOfProgram.Count - 1].ToString()) +
                                GlobalVars.getInstance().AutoStep);

                            //find empty strings and delete them
                            List<string> tempList = new List<string>();
                            foreach (string col in GlobalVars.getInstance().ListOfProgram)
                            {
                                if (col.Length > 2)
                                {
                                    tempList.Add(col);
                                }
                            }
                            GlobalVars.getInstance().ListOfProgram.Clear();
                            GlobalVars.getInstance().ListOfProgram.AddRange(tempList);
                        }
                    }
                }
                else
                {
                    GlobalVars.getInstance().Error = "Bad file name";
                    addStringToCommandWindow(GlobalVars.getInstance().Error);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(TAG + "± An error: " + ex);
            }
        }

        public void load()
        {
            Stream myStream = null;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            //openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "bas files (*.bas)|*.bas|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((myStream = openFileDialog1.OpenFile()) != null)
                    {
                        using (myStream)
                        {
                            // Insert code to read the stream here.
                            string fileName = openFileDialog1.FileName;
                            GlobalVars.getInstance().FileName = fileName;
                            reset();

                            myStream.Position = 0;
                            using (StreamReader reader = new StreamReader(myStream, Encoding.UTF8))
                            {
                                string arrayText = reader.ReadToEnd();
                                Debug.WriteLine(TAG + "± reader.ReadToEnd() " + arrayText);
                                GlobalVars.getInstance().ListOfProgram = new List<string>(arrayText.Split('\n'));
                                GlobalVars.getInstance().ProgramCounter =
                                    int.Parse(runCommand.returnBaseCommand(GlobalVars.getInstance().ListOfProgram
                                    [GlobalVars.getInstance().ListOfProgram.Count - 1].ToString()) +
                                    GlobalVars.getInstance().AutoStep);

                                //find empty strings and delete them
                                List<string> tempList = new List<string>();
                                foreach (string col in GlobalVars.getInstance().ListOfProgram)
                                {
                                    if (col.Length > 2)
                                    {
                                        tempList.Add(col);
                                    }
                                }
                                GlobalVars.getInstance().ListOfProgram.Clear();
                                GlobalVars.getInstance().ListOfProgram.AddRange(tempList);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }
#endregion

        public void otherCommands(String commandRun)
        {
            String str = commandRun;
            commandRun = commandRun.Split(' ')[0];
            for (int i = 0; i < GlobalVars.getInstance().ListOfAll.Count(); i++)
            {
                //Debug.WriteLine(TAG + "± listOfAll" + GlobalVars.getInstance().ListOfAll[i]);
                NSRange range = new NSRange(commandRun.IndexOf(GlobalVars.getInstance().ListOfAll[i].ToString()), commandRun.Length);
                if (range.location != NSNotFound && range.location == 0)
                {
                    commandRun = GlobalVars.getInstance().ListOfAll[i].ToString();
                    Debug.WriteLine(TAG + "± Command found! '%@'" + commandRun);
                }
            }
            if (commandRun.ToLower().Equals("cls"))
            {
                //commandWindow.Text = null;
                //Cls();
                }
            else if (commandRun.ToLower().Equals("load"))
            {
                if (str.Length > 5)
                {
                    load(str.Substring(5));
                }
                else
                {
                    load();
                    /*
                    Stream myStream = null;
                    OpenFileDialog openFileDialog1 = new OpenFileDialog();

                    //openFileDialog1.InitialDirectory = "c:\\";
                    openFileDialog1.Filter = "bas files (*.bas)|*.bas|All files (*.*)|*.*";
                    openFileDialog1.FilterIndex = 1;
                    openFileDialog1.RestoreDirectory = true;

                    if (openFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            if ((myStream = openFileDialog1.OpenFile()) != null)
                            {
                                using (myStream)
                                {
                                    // Insert code to read the stream here.
                                    string fileName = openFileDialog1.FileName;
                                    GlobalVars.getInstance().FileName = fileName;
                                    reset();

                                    myStream.Position = 0;
                                    using (StreamReader reader = new StreamReader(myStream, Encoding.UTF8))
                                    {
                                        string arrayText = reader.ReadToEnd();
                                        Debug.WriteLine(TAG + "± reader.ReadToEnd() " + arrayText);
                                        GlobalVars.getInstance().ListOfProgram = new List<string>(arrayText.Split('\n'));
                                        GlobalVars.getInstance().ProgramCounter =
                                            int.Parse(runCommand.returnBaseCommand(GlobalVars.getInstance().ListOfProgram
                                            [GlobalVars.getInstance().ListOfProgram.Count - 1].ToString()) +
                                            GlobalVars.getInstance().AutoStep);

                                        //find empty strings and delete them
                                        List<string> tempList = new List<string>();
                                        foreach (string col in GlobalVars.getInstance().ListOfProgram)
                                        {
                                            if (col.Length > 2)
                                            {
                                                tempList.Add(col);
                                            }
                                        }
                                        GlobalVars.getInstance().ListOfProgram.Clear();
                                        GlobalVars.getInstance().ListOfProgram.AddRange(tempList);
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                        }
                    }*/
                }
            }
            else if (commandRun.ToLower().Equals("reset"))
            {
                reset();
            }
            else if (commandRun.ToLower().Equals("beep"))
            {
                beepPlay();
            }
            else if (commandRun.ToLower().Equals("color"))
            {
                str = str.Replace("color", "");
                str = str.Replace(" ", "");
                String foreground = str.Split(',')[0];
                if (!string.IsNullOrEmpty(foreground)) setColor(foreground);
                if (str.Contains(",")) setBackground(str.Split(',')[1]);
            }
            else if (commandRun.ToLower().Equals("run"))
            {
                if (GlobalVars.getInstance().ListOfProgram.Count() > 0)
                {
                    Debug.WriteLine(TAG + "± start running program");
                    nextCommand = YES;
                    GlobalVars.getInstance().RunIndex = 0;
                    GlobalVars.getInstance().Run = YES;
                    GlobalVars.getInstance().Error = "";
                    runProgram();
                }
            }
            else if (commandRun.ToLower().Equals("share") && !GlobalVars.getInstance().Run)
            {
                share();
            }
            else if (commandRun.ToLower().Equals("if") && string.IsNullOrEmpty(GlobalVars.getInstance().Error))
            {
                Debug.WriteLine(TAG + "± if in progress on ViewController. " + GlobalVars.getInstance().CommandIf);
                if (!string.IsNullOrEmpty(GlobalVars.getInstance().CommandIf))
                {
                    if (runCommand.set(GlobalVars.getInstance().CommandIf))
                    {
                        for (int n = 0; n < GlobalVars.getInstance().ListOfStrings.Count(); n++)
                        {
                            addStringToCommandWindow(GlobalVars.getInstance().ListOfStrings[n].ToString());
                        }
                    }
                    else {
                        otherCommands(GlobalVars.getInstance().CommandIf);
                    }
                }
            }
            else if (commandRun.ToLower().Equals("input") && string.IsNullOrEmpty(GlobalVars.getInstance().Error))
            {
                nextCommand = NO;
                if (GlobalVars.getInstance().ListOfStrings.Count() > 0)
                {
                    addStringToCommandWindow(GlobalVars.getInstance().ListOfStrings[0].ToString());
                    // returnCR];
                }
                addStringToCommandWindow("? ");
                GlobalVars.getInstance().IsOkSet = NO;
                inputMode = YES;
            }
            else {
                syntaxError();
            }
            Debug.WriteLine(TAG + "± commandRun=" + commandRun);
        }


        /*private static String readFileAsString(String filePath) 
    {
        DataInputStream dis = new DataInputStream(new FileInputStream(filePath));
            try {
                long len = new File(filePath).Length();
                if (len > Integer.MAX_VALUE)
                    throw new IOException("File " + filePath + " too large, was " + len + " bytes.");
    byte[] bytes = new byte[(int)len];
    dis.readFully(bytes);
                return new String(bytes, "UTF-8");
            } finally {
                dis.close();
            }*/

        internal void Cls()
        {
            GlobalVars.getInstance().ListOfStrings.Clear();
            commandWindow.Text = "";
        }

        public string CommandWindowText
        {
            get { return commandWindow.Text; }
        }
    }
}