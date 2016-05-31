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

                reset();
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
                addStringToCommandWindow(getInstance().getListOfStrings()[i].ToString());
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
                addStringToCommandWindow(getInstance().getListOfStrings()[i].ToString());
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

        /* public void addStringToCommandWindow(String string_val)
         {
             //            commandWindow.Text += string_val;
             commandWindow.AppendText(string_val);
         }*/

        public void onTextChanged(String s, int start, int before, int count)
        {
            Debug.WriteLine(TAG, "± onTextChanged 1..." + count);
            if (count > 0)// && GlobalVars.getInstance().scanKeyOn)
            {
                Debug.WriteLine(TAG, "± onTextChanged 2..." + s.Substring(start, 1));
                if (s.ToCharArray().ElementAt(start) == 10)
                {
                    //GlobalVars.getInstance().lineNumber = getCurrentCursorLine(commandWindow.Text);
                    GlobalVars.getInstance().lineNumber = getCurrentCursorLine(commandWindow);
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

        public int getCurrentCursorLine(TextBox editText) //Returns the current Y cursor position
        {
            int selectionStart = editText.SelectionStart;
            //Layout layout = editText.getLayout();

            if (!(selectionStart == -1))
            {
                //return layout.getLineForOffset(selectionStart);
                getCurrentCursorLine(editText);
            }
            return -1;
        }

        public bool onEnterPress(String text) //throws IOException
        {
            //commandWindow.setSelection(commandWindow.Text.Length);
            commandWindow.Select(commandWindow.Text.Length, 0);
            //        Debug.WriteLine(TAG, "± onEnterPress index="+(GlobalVars.getInstance().GlobalVars.getInstance().lineNumber - 1)+" total lines="+commandWindow.Text.ToString().Split('\n").Length);
            if (GlobalVars.getInstance().lineNumber - 1 >= commandWindow.Text.ToString().Split('\n').Length)
                GlobalVars.getInstance().lineNumber--;
            String textLine = commandWindow.Text.ToString().Split('\n')[GlobalVars.getInstance().lineNumber - 1];
            GlobalVars.getInstance().Command = textLine;
            bool shouldChangeText = true;
            GlobalVars.getInstance().KeyScan = text;
            textIndex = commandWindow.Text.ToString().Length;
            String[]
            lines = commandWindow.Text.ToString().Split('\n');
            if (inputMode)
            {
                if (!string.IsNullOrEmpty(GlobalVars.getInstance().input))
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
                        if (GlobalVars.getInstance().Command.Substring(0, 4).ToLower().Equals("list")) //EqualsIgnoreCase
                            listColor();
                    //  return YES;
                }
            }
            shouldChangeText = string.IsNullOrEmpty(GlobalVars.getInstance().Command);
            return shouldChangeText;
        }

        public void reset()
        {
            GlobalVars.getInstance().Error = "";
            Debug.WriteLine(TAG, "± reset");
            //commandWindow = (EditText)findViewById(R.id.CommandWindow);
            commandWindow.Text = (null);
            commandWindow.BackColor = (detectColor(4));
            commandWindow.ForeColor = (detectColor(15));
            version();
            printOk();
            //print runCommand.set("a=2");
            //runCommand.set("a(1)=199");
            runCommand.set("print a,\"---\",7");
            inputMode = false;
            inputCount = 0;
            keyOffset = 0;
            //commandWindow.setSelection(commandWindow.Text.Length);
            commandWindow.Select(commandWindow.Text.Length, 0);
        }

        public Color detectColor(int colorNumber)
        {
            // Log.d(TAG, "± Color # " + colorNumber);
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
                //Log.d(TAG, "± Incorrect color\n");
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

            for (int i = 0; i <[GlobalVars.getInstance().listOfAll count];
            i++){
            //NSRange searchRange = NSMakeRange(0,string.length);
            //NSRange searchRange = getViewableRange:commandWindow];
            NSRange searchRange = NSMakeRange(textIndex, string.length - textIndex);
            NSRange foundRange;
            while (searchRange.location < string.length) {
                searchRange.length = string.length - searchRange.location;
                foundRange =[string rangeOfString(GlobalVars.getInstance().listOfAll.get(i]options:
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
            NSArray * lines =[[string substringWithRange:searchRange]componentsSeparatedByString:@ "\n"];
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
            GlobalVars.getInstance().isOkSet = NO;
            if (GlobalVars.getInstance().run)
            {
                GlobalVars.getInstance().error = GlobalVars.getInstance().error + " at line " + GlobalVars.getInstance().runnedLine + "\n";
            }
            stopRunning();
            addStringToCommandWindow(GlobalVars.getInstance().error);
        }

        public void stopRunning()
        {
            printOk();
            //handler.removeCallbacks(null);
            handler.Stop();
            GlobalVars.getInstance().runIndex = 0;
            GlobalVars.getInstance().run = NO;
            GlobalVars.getInstance().isOkSet = YES;
            inputMode = NO;
            //GlobalVars.getInstance().scanKeyOn = NO;
            Debug.WriteLine(TAG, "Program is OVER!!!!! " + GlobalVars.getInstance().error);
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
            GlobalVars.getInstance().listOfStrings.Clear();
        }

        public void printOk()
        {
            String[] lines = commandWindow.Text.ToString().Split('\n');
            if (lines.Length > 2)
                if (GlobalVars.getInstance().IsOkSet && !GlobalVars.getInstance().Run)
                {
                    addStringToCommandWindow("Ok\n");
                }
            GlobalVars.getInstance().IsOkSet = (true);
        }

        public void addStringToCommandWindow(String string_val)
        {
            String temp = commandWindow.Text.ToString() + string_val;
            commandWindow.AppendText(string_val);
            commandWindow.Select(commandWindow.Text.Length, 0);
        }

        public void returnCR()
        {
            addStringToCommandWindow("Ok\n");
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

        public void setColor(String color)
        {
            int col = 1;
            try
            {
                col = int.Parse(color);
            }
            catch //(NumberFormatException e)
            {
                Debug.WriteLine(TAG, "± Background color=" + color + "Wrong number format in string$!");
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
                Debug.WriteLine(TAG, "± color=" + color + "Wrong number format in string$!");
            }
            commandWindow.BackColor = (detectColor(col));
        }

        public void runProgram()
        {
            if (nextCommand)
            {
                String untilSpace = GlobalVars.getInstance().listOfProgram[GlobalVars.getInstance().runIndex].ToString().Split(' ')[0];
                GlobalVars.getInstance().runnedLine = untilSpace;
                int indexforAfterSpace = untilSpace.Length;
                String commandRun = GlobalVars.getInstance().listOfProgram[GlobalVars.getInstance().runIndex].ToString().Substring(indexforAfterSpace + 1);
                if (commandRun.Substring(0, 1).Equals(" "))
                {
                    commandRun = normaStr.removeSpaceInBegin(commandRun);
                }
                Debug.WriteLine(TAG, "± " + GlobalVars.getInstance().runIndex + " Command '" + commandRun + "'");
                if (!commandRun.Equals(""))
                {
                    if (runCommand.set(commandRun))
                    {
                        for (int n = 0; n < GlobalVars.getInstance().listOfStrings.Count(); n++)
                        {
                            addStringToCommandWindow(GlobalVars.getInstance().listOfStrings[n].ToString());
                        }
                    }
                    else {
                        otherCommands(commandRun);
                    }
                }
                GlobalVars.getInstance().runIndex++;
                if (GlobalVars.getInstance().runIndex >= GlobalVars.getInstance().listOfProgram.Count()) stopRunning();
                if (!GlobalVars.getInstance().error.Equals("")) stopRunning();
                if (!GlobalVars.getInstance().run) stopRunning();
            }
            if (GlobalVars.getInstance().run)
            {
                //Do something after 100ms
                try
                {
                    runProgram();
                }
                catch (IOException e)
                {
                    //e.printStackTrace();
                }
            }
        }

        public void share()
        {
            /*
                    NSURL* fn=fileToURL:GlobalVars.getInstance().fileName];
                    NSLog(@"shareCommand for '%@'",GlobalVars.getInstance().fileName);
                    NSString *texttoshare = @"MCX Basic file. Sent by share command.";
                    //    String listToShare = [GlobalVars.getInstance().listOfProgram componentsJoinedByString: @"\n"];
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


        public void otherCommands(String commandRun)
        {
            String str = commandRun;
            commandRun = commandRun.Split(' ')[0];
            for (int i = 0; i < GlobalVars.getInstance().listOfAll.Count(); i++)
            {
                //Debug.WriteLine(TAG, "± listOfAll" + GlobalVars.getInstance().listOfAll[i]);
                NSRange range = new NSRange(commandRun.IndexOf(GlobalVars.getInstance().listOfAll[i].ToString()), commandRun.Length);
                if (range.location != NSNotFound && range.location == 0)
                {
                    commandRun = GlobalVars.getInstance().listOfAll[i].ToString();
                    Debug.WriteLine(TAG, "± Command found! '%@'" + commandRun);
                }
            }
            if (commandRun.ToLower().Equals("cls"))
            {
                commandWindow.Text = (null);
            }
            else if (commandRun.ToLower().Equals("load"))
            {
                /*OpenFileDialog fileDialog = new OpenFileDialog(this)
                        .setFilter(".*\\.bas")
                        .setOpenDialogListener(new OpenFileDialog.OpenDialogListener() {
                                @Override
                                public void OnSelectedFile(String fileName) throws IOException
        {
            Toast.makeText(getApplicationContext(), fileName, Toast.LENGTH_LONG).show();
            GlobalVars.getInstance().setFileName(fileName);
            reset();
            String arrayText = readFileAsString(fileName);
            Debug.WriteLine(TAG, "± readFileAsString " + arrayText);
            GlobalVars.getInstance().listOfProgram = new ArrayList<String>(Arrays.asList(arrayText.Split("\n")));
                                    GlobalVars.getInstance().programCounter = Integer.parseInt(runCommand.returnBaseCommand(GlobalVars.getInstance().listOfProgram.get(GlobalVars.getInstance().listOfProgram.size() - 1).ToString()) + GlobalVars.getInstance().autoStep);

                                }
                            });
                    fileDialog.show();*/
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
                if (GlobalVars.getInstance().listOfProgram.Count() > 0)
                {
                    Debug.WriteLine(TAG, "± start running program");
                    nextCommand = YES;
                    GlobalVars.getInstance().runIndex = 0;
                    GlobalVars.getInstance().run = YES;
                    GlobalVars.getInstance().error = "";
                    runProgram();
                }
            }
            else if (commandRun.ToLower().Equals("share") && !GlobalVars.getInstance().run)
            {
                share();
            }
            else if (commandRun.ToLower().Equals("if") && string.IsNullOrEmpty(GlobalVars.getInstance().error))
            {
                Debug.WriteLine(TAG, "± if in progress on ViewController. " + GlobalVars.getInstance().commandIf);
                if (!string.IsNullOrEmpty(GlobalVars.getInstance().commandIf))
                {
                    if (runCommand.set(GlobalVars.getInstance().commandIf))
                    {
                        for (int n = 0; n < GlobalVars.getInstance().listOfStrings.Count(); n++)
                        {
                            addStringToCommandWindow(GlobalVars.getInstance().listOfStrings[n].ToString());
                        }
                    }
                    else {
                        otherCommands(GlobalVars.getInstance().commandIf);
                    }
                }
            }
            else if (commandRun.ToLower().Equals("input") && string.IsNullOrEmpty(GlobalVars.getInstance().error))
            {
                nextCommand = NO;
                if (GlobalVars.getInstance().listOfStrings.Count() > 0)
                {
                    addStringToCommandWindow(GlobalVars.getInstance().listOfStrings[0].ToString());
                    // returnCR];
                }
                addStringToCommandWindow("? ");
                GlobalVars.getInstance().isOkSet = NO;
                inputMode = YES;
            }
            else {
                syntaxError();
            }
            Debug.WriteLine(TAG, "± commandRun=" + commandRun);
        }

        private void commandWindow_TextChanged(object sender, EventArgs e)
        {

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
    }

}

