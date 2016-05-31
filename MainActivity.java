package com.neronov.aleksei.mcxbasic;

import android.graphics.Color;
import android.media.MediaPlayer;
import android.os.Bundle;
import android.os.Handler;
import android.support.v7.app.AppCompatActivity;
import android.text.Editable;
import android.text.Layout;
import android.text.Selection;
import android.text.TextWatcher;
import android.util.Log;
import android.view.KeyEvent;
import android.view.View;
import android.widget.EditText;
import android.widget.TextView;
import android.widget.Toast;

import java.io.DataInputStream;
import java.io.File;
import java.io.FileInputStream;
import java.io.IOException;
import java.util.ArrayList;
import java.util.Arrays;

import static com.neronov.aleksei.mcxbasic.GlobalVars.getInstance;


public class MainActivity extends AppCompatActivity {

    private static final String TAG = MainActivity.class.getSimpleName();
    private EditText commandWindow;
    private static final int NSNotFound = -1;
    private static final boolean NO = false;
    private static final boolean YES = true;

    RunCommand runCommand = new RunCommand();
    private static NormalizeString normaStr = new NormalizeString();
    //    DigitalFunc digitalFunc = new DigitalFunc();
//    Variables variables = new Variables();
//    StringFunc stringFunc = new StringFunc();

    int position;
    boolean inputMode;
    int inputCount;
    boolean nextCommand;
    int keyOffset;
    int textIndex;

    //CGSize screenSize;
    // Timer runTimer = new Timer();
    final Handler handler = new Handler();

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        // if (getInstance().firstStart)
        {
            Log.d(TAG, "± init in " + getInstance().currentFolder);

            File f = new File(getInstance().currentFolder); //Check if folder not excist - make new one
            if (!f.isDirectory()) {
                Log.d(TAG, "± Make dir " + getInstance().currentFolder);
                f.mkdir();
            }

            getInstance().firstStart = false;
            getInstance().scanKeyOn = NO;
            commandWindow = (EditText) findViewById(R.id.CommandWindow);
            commandWindow.addTextChangedListener(inputTextWatcher);
            commandWindow.setOnKeyListener(new View.OnKeyListener() {
                public boolean onKey(View v, int keyCode, KeyEvent event) {
                /*
                if ((event.getAction() == KeyEvent.ACTION_DOWN) &&
                        (keyCode == KeyEvent.KEYCODE_ENTER)) {
                    //Log.d(TAG, "± Key pressed->" + keyCode);
                    return false;//onEnterPress();
                }
                */
                    return false;
                }
            });

/*
        commandWindow.setOnTouchListener(new OnSwipeTouchListener(MainActivity.this) {
           public void onSwipeTop() {
                String command="let a=89";
                Toast.makeText(MainActivity.this, command, Toast.LENGTH_SHORT).show();
                runCommand.set(command);
                for (int i = 0; i < getInstance().getListOfStrings().size(); i++)
                    addStringToCommandWindow(getInstance().getListOfStrings().get(i).toString());
                Toast.makeText(MainActivity.this,command, Toast.LENGTH_SHORT).show();
            }
            public void onSwipeRight() {
                Toast.makeText(MainActivity.this, "right", Toast.LENGTH_SHORT).show();
            }
            public void onSwipeLeft() {
                Toast.makeText(MainActivity.this, "stop", Toast.LENGTH_SHORT).show();
                returnCR();
                stopRunning();
                runCommand.autoProgramStop();
            }
            public void onSwipeBottom() {
                runCommand.set("list");
                for (int i = 0; i < getInstance().getListOfStrings().size(); i++)
                    addStringToCommandWindow(getInstance().getListOfStrings().get(i).toString());
                Toast.makeText(MainActivity.this, "list", Toast.LENGTH_SHORT).show();
            }

        });
*/

            reset();
            getInstance().setScanKeyOn(true);
        }
    }

    private TextWatcher inputTextWatcher = new TextWatcher() {
        public void afterTextChanged(Editable s) {
        }

        public void beforeTextChanged(CharSequence s, int start, int count, int after) {
        }

        public void onTextChanged(CharSequence s, int start, int before, int count) {
//            Log.d(TAG, "± onTextChanged 1..."+count);
            if (count > 0)// && getInstance().scanKeyOn)
            {
//                Log.d(TAG, "± onTextChanged 2..."+s.charAt(start));
                if (s.charAt(start) == 10) {
                    getInstance().lineNumber = getCurrentCursorLine(commandWindow);
                    try {
                        onEnterPress(String.valueOf(s.charAt(start)));
                    } catch (IOException e) {
                        e.printStackTrace();
                    }
                }
            }
        }
    };

    public void beepPlay() {
        Log.d(TAG, "± Beep play begin ...");
        MediaPlayer mp = MediaPlayer.create(this, getResources().getIdentifier("beep", "raw", getPackageName()));
        mp.start();
    }

    public int getCurrentCursorLine(EditText editText) //Возвращает текущую Y позицию курсора
    {
        int selectionStart = Selection.getSelectionStart(editText.getText());
        Layout layout = editText.getLayout();

        if (!(selectionStart == -1)) {
            return layout.getLineForOffset(selectionStart);
        }
        return -1;
    }

    public boolean onEnterPress(String text) throws IOException {
        commandWindow.setSelection(commandWindow.getText().length());
//        Log.d(TAG, "± onEnterPress index="+(getInstance().getInstance().lineNumber - 1)+" total lines="+commandWindow.getText().toString().split("\n").length);
        if (getInstance().lineNumber - 1 >= commandWindow.getText().toString().split("\n").length)
            getInstance().lineNumber--;
        String textLine = commandWindow.getText().toString().split("\n")[getInstance().lineNumber - 1];
        getInstance().setCommand(textLine);
        boolean shouldChangeText = true;
        getInstance().setKeyScan(text);
        textIndex = commandWindow.getText().toString().length();
        String[] lines = commandWindow.getText().toString().split("\n");
        if (inputMode) {
            if (!getInstance().input.isEmpty()) {
                int extrAdd = 2;
                if (getInstance().listOfStrings.size() == 0) {
                    getInstance().listOfStrings.add("t");
                    extrAdd = 1;
                }
                Log.d(TAG, "± input mode ='"+getInstance().input+"' listofstring0='"+getInstance().listOfStrings.get(0).toString()+"'");
                String[] arr = getInstance().input.substring(1).split(",");
                String entered;
                int indexInput = getInstance().listOfStrings.get(0).toString().length() + extrAdd;
                Log.d(TAG, "± input-- '" + lines[lines.length - 1] + "' indexed-'" + getInstance().listOfStrings.get(0).toString());
                entered = lines[lines.length - 1].substring(indexInput);
                String str = arr[inputCount] + "=" + entered;
                if (arr[inputCount].substring(arr[inputCount].length() - 1).equals("$"))
                    str = arr[inputCount] + "=\"" + entered + "\"";
                runCommand.set(str);
                inputCount++;
                if (inputCount >= arr.length) {
                    returnCR();
                    inputMode = NO;
                    inputCount = 0;
                    nextCommand = YES;
                } else {
                    returnCR();
                    addStringToCommandWindow("? ");
                }
            } else {
                returnCR();
                inputMode = NO;
                inputCount = 0;
                syntaxError();
                nextCommand = YES;
            }
        } else {
            if (!getInstance().getCommand().equals("")) {
                if (getInstance().isOkSet) returnCR();
                if (!getInstance().autoSet) {
                    // Log.d(TAG, "± autoSet=" + getInstance().autoSet);
                    if (runCommand.set(getInstance().getCommand())) {
                        //[commandWindow resignFirstResponder];
                        for (int i = 0; i < getInstance().getListOfStrings().size(); i++)
                            addStringToCommandWindow(getInstance().getListOfStrings().get(i).toString());
                        //[commandWindow becomeFirstResponder];
                        //   if (!getInstance().autoSet || !getInstance().run) printOk();
                    } else {
                        otherCommands(getInstance().command);
                        printOk();
                    }
                } else {
                    // Log.d(TAG, "± !!!!!autoSet=" + getInstance().autoSet);
                    runCommand.autoProgramSet(getInstance().command);
                    for (int i = 0; i < getInstance().listOfStrings.size(); i++) {
                        addStringToCommandWindow(getInstance().listOfStrings.get(i).toString());
                    }
                }
                if (getInstance().getCommand().length() > 3)
                    if (getInstance().getCommand().substring(0, 4).equalsIgnoreCase("list"))
                        listColor();
                //  return YES;
            }
        }
        shouldChangeText = getInstance().getCommand().isEmpty();
        return shouldChangeText;
    }

    public void reset() {
        getInstance().setError("");
        Log.d(TAG, "± reset");
        commandWindow = (EditText) findViewById(R.id.CommandWindow);
        commandWindow.setText(null);
        commandWindow.setBackgroundColor(detectColor(4));
        commandWindow.setTextColor(detectColor(15));
        version();
        printOk();
        //print runCommand.set("a=2");
        //runCommand.set("a(1)=199");
        runCommand.set("print a,\"---\",7");
        inputMode = false;
        inputCount = 0;
        keyOffset = 0;
        commandWindow.setSelection(commandWindow.getText().length());
    }

    public void version() {
        runCommand.set("ver");
        printListOfStrings();
    }

    public void printListOfStrings() {
        for (int i = 0; i < getInstance().getListOfStrings().size(); i++) {
            String temp = getInstance().getListOfStrings().get(i).toString();
            addStringToCommandWindow(temp);
        }
        getInstance().listOfStrings.clear();
    }

    public void printOk() {
        String[] lines = commandWindow.getText().toString().split("\n");
        if (lines.length > 2)
            if (getInstance().getIsOkSet() && !getInstance().getRun()) {
                addStringToCommandWindow("Ok\n");
            }
        getInstance().setIsOkSet(true);
    }

    public void addStringToCommandWindow(String string) {
        String temp = commandWindow.getText().toString() + string;
        commandWindow.setText(temp, TextView.BufferType.EDITABLE);
        commandWindow.setSelection(commandWindow.getText().length());
    }

    public int detectColor(int colorNumber) {
        // Log.d(TAG, "± Color # " + colorNumber);
        int result = Color.WHITE;
        if (colorNumber == 1) result = Color.BLACK;
        if (colorNumber == 2) result = Color.parseColor("#00E100");
        if (colorNumber == 3) result = Color.parseColor("#27FF62");
        if (colorNumber == 4) result = Color.parseColor("#0836A0");
        if (colorNumber == 5) result = Color.parseColor("#5B60FF");
        if (colorNumber == 6) result = Color.parseColor("#C50023");
        if (colorNumber == 7) result = Color.parseColor("#0EDFFF");
        if (colorNumber == 8) result = Color.parseColor("#FF0019");
        if (colorNumber == 9) result = Color.parseColor("#FF6372");
        if (colorNumber == 10) result = Color.parseColor("#D7E300");
        if (colorNumber == 11) result = Color.parseColor("#CAD186");
        if (colorNumber == 12) result = Color.parseColor("#009507");
        if (colorNumber == 13) result = Color.parseColor("#DA0FB1");
        if (colorNumber == 14) result = Color.parseColor("#ADADAD");
        if (colorNumber == 15) result = Color.parseColor("#FFFFFF");
        if (colorNumber > 15) {

            getInstance().setError("Incorrect color");
            //Log.d(TAG, "± Incorrect color\n");
        }
        return result;
    }

    public void returnCR() {
        addStringToCommandWindow("Ok\n");
    }

    public void otherCommands(String commandRun) throws IOException {
        String str = commandRun;
        commandRun = commandRun.split(" ")[0];
        for (int i = 0; i < getInstance().listOfAll.size(); i++) {
            //Log.d(TAG, "± listOfAll" + getInstance().listOfAll.get(i));
            NSRange range = new NSRange(commandRun.indexOf(getInstance().listOfAll.get(i).toString()), commandRun.length());
            if (range.location != NSNotFound && range.location == 0) {
                commandRun = getInstance().listOfAll.get(i).toString();
                Log.d(TAG, "± Command found! '%@'" + commandRun);
            }
        }
        if (commandRun.equalsIgnoreCase("cls")) {
            commandWindow.setText(null);
        } else if (commandRun.equalsIgnoreCase("load")) {
            OpenFileDialog fileDialog = new OpenFileDialog(this)
                    .setFilter(".*\\.bas")
                    .setOpenDialogListener(new OpenFileDialog.OpenDialogListener() {
                        @Override
                        public void OnSelectedFile(String fileName) throws IOException {
                            Toast.makeText(getApplicationContext(), fileName, Toast.LENGTH_LONG).show();
                            getInstance().setFileName(fileName);
                            reset();
                            String arrayText = readFileAsString(fileName);
                            Log.d(TAG, "± readFileAsString " + arrayText);
                            getInstance().listOfProgram = new ArrayList<String>(Arrays.asList(arrayText.split("\n")));
                            getInstance().programCounter = Integer.parseInt(runCommand.returnBaseCommand(getInstance().listOfProgram.get(getInstance().listOfProgram.size() - 1).toString()) + getInstance().autoStep);

                        }
                    });
            fileDialog.show();
        } else if (commandRun.equalsIgnoreCase("reset")) {
            reset();
        } else if (commandRun.equalsIgnoreCase("beep")) {
            beepPlay();
        } else if (commandRun.equalsIgnoreCase("color")) {
            str = str.replace("color", "");
            str = str.replace(" ", "");
            String foreground = str.split(",")[0];
            if (!foreground.isEmpty()) setColor(foreground);
            if (str.contains(",")) setBackground(str.split(",")[1]);
        } else if (commandRun.equalsIgnoreCase("run")) {
            if (getInstance().listOfProgram.size() > 0) {
                Log.d(TAG, "± start running program");
                nextCommand = YES;
                getInstance().runIndex = 0;
                getInstance().run = YES;
                getInstance().error = "";
                runProgram();
            }
        } else if (commandRun.equalsIgnoreCase("share") && !getInstance().run) {
            share();
        } else if (commandRun.equalsIgnoreCase("if") && getInstance().error.isEmpty()) {
            Log.d(TAG, "± if in progress on ViewController. " + getInstance().commandIf);
            if (!getInstance().commandIf.isEmpty()) {
                if (runCommand.set(getInstance().commandIf)) {
                    for (int n = 0; n < getInstance().listOfStrings.size(); n++) {
                        addStringToCommandWindow(getInstance().listOfStrings.get(n).toString());
                    }
                } else {
                    otherCommands(getInstance().commandIf);
                }
            }
        } else if (commandRun.equalsIgnoreCase("input") && getInstance().error.isEmpty()) {
            nextCommand = NO;
            if (getInstance().listOfStrings.size() > 0) {
                addStringToCommandWindow(getInstance().listOfStrings.get(0).toString());
                // returnCR];
            }
            addStringToCommandWindow("? ");
            getInstance().isOkSet = NO;
            inputMode = YES;
        } else {
            syntaxError();
        }
        Log.d(TAG, "± commandRun=" + commandRun);
    }

    private static String readFileAsString(String filePath) throws IOException {
        DataInputStream dis = new DataInputStream(new FileInputStream(filePath));
        try {
            long len = new File(filePath).length();
            if (len > Integer.MAX_VALUE)
                throw new IOException("File " + filePath + " too large, was " + len + " bytes.");
            byte[] bytes = new byte[(int) len];
            dis.readFully(bytes);
            return new String(bytes, "UTF-8");
        } finally {
            dis.close();
        }
    }

    public void syntaxError() {
        getInstance().isOkSet = NO;
        if (getInstance().run) {
            getInstance().error = getInstance().error + " at line " + getInstance().runnedLine + "\n";
        }
        stopRunning();
        addStringToCommandWindow(getInstance().error);
    }

    public void stopRunning() {
        printOk();
        handler.removeCallbacks(null);
        getInstance().runIndex = 0;
        getInstance().run = NO;
        getInstance().isOkSet = YES;
        inputMode = NO;
        //getInstance().scanKeyOn = NO;
        Log.d(TAG, "Program is OVER!!!!! " + getInstance().error);
    }

    public void setBackground(String color) {
        int col = 1;
        try {
            col = Integer.parseInt(color);
        } catch (NumberFormatException e) {
            Log.d(TAG, "± color=" + color + "Wrong number format in string$!");
        }
        commandWindow.setBackgroundColor(detectColor(col));
    }

    public void setColor(String color) {
        int col = 1;
        try {
            col = Integer.parseInt(color);
        } catch (NumberFormatException e) {
            Log.d(TAG, "± Background color=" + color + "Wrong number format in string$!");
        }
        commandWindow.setTextColor(detectColor(col));
    }

    public void runProgram() throws IOException {
        if (nextCommand) {
            String untilSpace = getInstance().listOfProgram.get(getInstance().runIndex).toString().split(" ")[0];
            getInstance().runnedLine = untilSpace;
            int indexforAfterSpace = untilSpace.length();
            String commandRun = getInstance().listOfProgram.get(getInstance().runIndex).toString().substring(indexforAfterSpace + 1);
            if (commandRun.substring(0, 1).equals(" ")) {
                commandRun = normaStr.removeSpaceInBegin(commandRun);
            }
            Log.d(TAG, "± " + getInstance().runIndex + " Command '" + commandRun + "'");
            if (!commandRun.equals("")) {
                if (runCommand.set(commandRun)) {
                    for (int n = 0; n < getInstance().listOfStrings.size(); n++) {
                        addStringToCommandWindow(getInstance().listOfStrings.get(n).toString());
                    }
                } else {
                    otherCommands(commandRun);
                }
            }
            getInstance().runIndex++;
            if (getInstance().runIndex >= getInstance().listOfProgram.size()) stopRunning();
            if (!getInstance().error.equals("")) stopRunning();
            if (!getInstance().run) stopRunning();
        }
        if (getInstance().run) handler.postDelayed(new Runnable() {
            @Override
            public void run() {
                //Do something after 100ms
                try {
                    runProgram();
                } catch (IOException e) {
                    e.printStackTrace();
                }
            }
        }, 10);
        //  runTimer = [NSTimer scheduledTimerWithTimeInterval:0.01 target:self selector:@selector(runProgram) userInfo:nil repeats:NO];
    }

    public void share() {
/*
        NSURL* fn=fileToURL:getInstance().fileName];
        NSLog(@"shareCommand for '%@'",getInstance().fileName);
        NSString *texttoshare = @"MCX Basic file. Sent by share command.";
        //    String listToShare = [getInstance().listOfProgram componentsJoinedByString: @"\n"];
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

    public void listColor()

    {
        /*
        getInstance() =[GlobalVars sharedInstance];
        digitalFunc =[[DigitalFunc alloc]init];
        NSCharacterSet * alphaSet =[[NSCharacterSet characterSetWithCharactersInString:@
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLKMNOPQRSTUVWXYZ"]invertedSet];
        NSString * string =[commandWindow text].lowercaseString;
        NSMutableArray * arrayOfRanges =[[NSMutableArray alloc]init];
        NSMutableArray * arrayOfRangesText =[[NSMutableArray alloc]init];
        NSMutableArray * arrayOfRangesNumber =[[NSMutableArray alloc]init];

        for (int i = 0; i <[getInstance().listOfAll count];
        i++){
        //NSRange searchRange = NSMakeRange(0,string.length);
        //NSRange searchRange = getViewableRange:commandWindow];
        NSRange searchRange = NSMakeRange(textIndex, string.length - textIndex);
        NSRange foundRange;
        while (searchRange.location < string.length) {
            searchRange.length = string.length - searchRange.location;
            foundRange =[string rangeOfString(getInstance().listOfAll.get(i]options:
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

/*

    - (void)keyboardDidShow: (NSNotification *) notif{
        // Do something here
        //    NSLog(@"keyboardDidShow!!!");
        moveTextViewForKeyboard:notif up:YES];
    }

    - (void)keyboardDidHide: (NSNotification *) notif{
        // Do something here
        //     NSLog(@"keyboardDidHide!!!");
    }

    - (void)viewWillDisappear:(BOOL)animated
    {
        [[NSNotificationCenter defaultCenter] removeObserver:self];
    }

    - (void)moveTextViewForKeyboard:(NSNotification*)aNotification up:(BOOL)up {

        NSDictionary* userInfo = [aNotification userInfo];
        NSTimeInterval animationDuration;
        UIViewAnimationCurve animationCurve;
        CGRect keyboardEndFrame;

        [[userInfo objectForKey:UIKeyboardAnimationCurveUserInfoKey] getValue:&animationCurve];
        [[userInfo objectForKey:UIKeyboardAnimationDurationUserInfoKey] getValue:&animationDuration];
        [[userInfo objectForKey:UIKeyboardFrameEndUserInfoKey] getValue:&keyboardEndFrame];

        [UIView beginAnimations:nil context:nil];
        [UIView setAnimationDuration:animationDuration];
        [UIView setAnimationCurve:animationCurve];

        CGRect newFrame = commandWindow.frame;
        CGRect keyboardFrame = [self.view convertRect:keyboardEndFrame toView:nil];
        keyboardFrame.size.height -= tabBarController.tabBar.frame.size.height;
        newFrame.size.height -= keyboardFrame.size.height * (up?1:-1);
        commandWindow.frame = newFrame;


        if (up) {
            keyOffset=keyboardFrame.size.height;
            //         NSLog(@"keyboard UP!!! %d",keyOffset);
        } else {
            keyOffset=0;
            //         NSLog(@"keyboard HIDE!!! %d",keyOffset);
        }
        initScreenSize];
        [UIView commitAnimations];
    }

    - (void)keyboardWillShown:(NSNotification*)aNotification
    {
        moveTextViewForKeyboard:aNotification up:YES];
    }

    - (void)keyboardWillHide:(NSNotification*)aNotification
    {
        moveTextViewForKeyboard:aNotification up:NO];
    }

    -(void)viewWillAppear:(BOOL)animated{
        [[NSNotificationCenter defaultCenter] addObserver:self  selector:@selector(orientationChanged:)    name:UIDeviceOrientationDidChangeNotification  object:nil];
    }

    - (void)orientationChanged:(NSNotification *)notification{
        adjustViewsForOrientation([UIApplication sharedApplication] statusBarOrientation]];
    }

    - (void) adjustViewsForOrientation:(UIInterfaceOrientation) orientation {

        switch (orientation)
        {
            case UIInterfaceOrientationPortrait:
            case UIInterfaceOrientationPortraitUpsideDown:
            {
                //load the portrait view
                //            NSLog(@"Portrait!!!");
                initScreenSize];
            }

            break;
            case UIInterfaceOrientationLandscapeLeft:
            case UIInterfaceOrientationLandscapeRight:
            {
                //load the landscape view
                //            NSLog(@"Lanscape!!!");
                initScreenSize];
            }
            break;
            case UIInterfaceOrientationUnknown:break;
        }
    }

    - (void) initScreenSize
    {
        screenSize=self.view.frame.size;
        CGRect newFrame = commandWindow.frame;
        newFrame.size = CGSizeMake(screenSize.width*0.98, screenSize.height*0.96-keyOffset);
        newFrame.origin = CGPointMake(screenSize.width*0.01, screenSize.height*0.02);
        commandWindow.frame=newFrame;
    }

    - (void)viewDidLoad {
        [super viewDidLoad];
        // Do any additional setup after loading the view, typically from a nib.

        [super viewDidLoad];
        NSLog(@"init");
        inputMode = NO;
        inputCount = 0;
        keyOffset=0;
        
        runCommand = [[RunCommand alloc] init];
        setBackground:detectColor:@"4"]];
        setColor:detectColor:@"15"]];
        commandWindow.delegate=self;

        // Create a swipe gesture recogniser
        UISwipeGestureRecognizer *recogniser = [[UISwipeGestureRecognizer alloc] initWithTarget:self action:@selector(swipe:)];
        recogniser.direction = UISwipeGestureRecognizerDirectionLeft | UISwipeGestureRecognizerDirectionRight; // add other directions if needed
        // Add the swipe gesture recogniser to the text view
        [commandWindow addGestureRecognizer:recogniser];

        UISwipeGestureRecognizer *swipeGestureDown = [[UISwipeGestureRecognizer alloc] initWithTarget:self action:@selector(downSwipeGesture:)];
        swipeGestureDown.direction = UISwipeGestureRecognizerDirectionDown;
        [commandWindow addGestureRecognizer:swipeGestureDown];

        UISwipeGestureRecognizer *swipeGestureUp = [[UISwipeGestureRecognizer alloc] initWithTarget:self action:@selector(upSwipeGesture:)];
        swipeGestureUp.direction = UISwipeGestureRecognizerDirectionUp;
        [commandWindow addGestureRecognizer:swipeGestureUp];

        UISwipeGestureRecognizer *swipeGestureRight = [[UISwipeGestureRecognizer alloc] initWithTarget:self action:@selector(rightSwipeGesture:)];
        swipeGestureRight.direction = UISwipeGestureRecognizerDirectionRight;
        [commandWindow addGestureRecognizer:swipeGestureRight];

        UISwipeGestureRecognizer *swipeGestureLeft = [[UISwipeGestureRecognizer alloc] initWithTarget:self action:@selector(leftSwipeGesture:)];
        swipeGestureLeft.direction = UISwipeGestureRecognizerDirectionLeft;
        [commandWindow addGestureRecognizer:swipeGestureLeft];

        [commandWindow setTextColor(UIColor whiteColor]];
        [commandWindow setFont(UIFont fontWithName:@"Menlo" size:16]];
        version];
        printOk];
        [commandWindow becomeFirstResponder];
    }


    - (BOOL)textView:(UITextView *)textView shouldChangeTextInRange:(NSRange)range replacementText:(NSString *)text {
        BOOL shouldChangeText = YES;

        getInstance().keyScan=text;
        if (!getInstance().scanKeyOn) {shouldChangeText = YES;}else{shouldChangeText = NO;}

        if ([text isEqualToString:@"\n"] && !getInstance().scanKeyOn) {
            NSLog(@"Return pressed");
            textIndex=(int)commandWindow.text.length;
            // return key
            NSArray *lines = [commandWindow.text componentsSeparatedByString:@"\n"];
            NSRange range = textView.selectedRange;
            NSString * firstHalfString = [commandWindow.text substringToIndex:range.location];
            NSArray *arrLines = [firstHalfString componentsSeparatedByString:@"\n"];
            getInstance().lineNumber = [arrLines count]-1;
            getInstance().command = [lines.get(getInstance().lineNumber];
            NSLog(@"command %@",getInstance().command);

            if (inputMode){
                if (![getInstance().input isEqual:@""])
                {
                    int extrAdd=2;
                    if ([getInstance().listOfStrings count]==0) {
                    [getInstance().listOfStrings addObject:@"t"];
                    extrAdd=1;
                }
                    NSLog(@"input mode ='%@' listofstring0='%@'",getInstance().input,[getInstance().listOfStrings.get(0]);
                    NSArray *arr = [[getInstance().input substringFromIndex:1] componentsSeparatedByCharactersInSet(NSCharacterSet characterSetWithCharactersInString:@","]];
                    String entered;
                    NSInteger indexInput=[[getInstance().listOfStrings.get(0]length]+extrAdd;
                    NSLog(@"input-- '%@' indexed-'%@'",[lines lastObject],[getInstance().listOfStrings.get(0]);
                    entered=[[lines lastObject] substringFromIndex:indexInput];
                    String str=[NSString stringWithFormat:@"%@=%@",[arr.get(inputCount],entered];
                    if ([[[arr.get(inputCount] substringFromIndex([arr.get(inputCount] length] - 1] isEqual:@"$"])
                    str=[NSString stringWithFormat:@"%@=\"%@\"",[arr.get(inputCount],entered];

                    [runCommand set:str];
                    inputCount++;
                    if (inputCount>=[arr count]) {
                    returnCR];
                    inputMode=NO;
                    inputCount=0;
                    nextCommand=YES;
                } else {
                    returnCR];
                    addStringToCommandWindow:@"? "];
                }
                } else {
                    returnCR];
                    inputMode=NO;
                    inputCount=0;
                    syntaxError];
                    nextCommand=YES;
                }
                // return YES;
            } else {
                if (![getInstance().command isEqual:@""]){

                    //int l=0;
                    //if (getInstance().command.length>3) l=4;
                    if (getInstance().isOkSet) returnCR];
                    if (!getInstance().autoSet) {
                        if ([runCommand set:getInstance().command])
                        {
                            [commandWindow resignFirstResponder];
                            for (int i=0; i<[getInstance().listOfStrings count]; i++) {
                            addStringToCommandWindow(getInstance().listOfStrings.get(i]];
                        }
                            [commandWindow becomeFirstResponder];
                            if (!getInstance().autoSet || !getInstance().run) printOk];
                        }else{
                            otherCommands:getInstance().command];
                            printOk];
                        }
                    }else{

                        [runCommand autoProgramSet:getInstance().command];
                        for (int i=0; i<[getInstance().listOfStrings count]; i++) {
                            addStringToCommandWindow(getInstance().listOfStrings.get(i]];
                        }

                    }
                    if ([getInstance().command length]>3)
                    if ([[getInstance().command substringToIndex:4] isEqual:@"list"]) listColor];

                    //  return YES;

                }
            }
            shouldChangeText = NO;
        } else {
            //        NSLog(@"Other pressed");
        }
        if ([getInstance().command isEqual:@""]) shouldChangeText = YES;
        return shouldChangeText;
    }

    - (void)textViewDidChange:(UITextView *)textView
    {
        CGRect caret = [commandWindow caretRectForPosition:commandWindow.selectedTextRange.end];
        [commandWindow scrollRectToVisible:caret animated:YES];
    }

    - (void)didReceiveMemoryWarning {
        [super didReceiveMemoryWarning];
        // Dispose of any resources that can be recreated.
    }





    - (NSURL *) fileToURL:(String)filename
    {
        NSArray *paths = NSSearchPathForDirectoriesInDomains(NSDocumentDirectory, NSUserDomainMask, YES);
        NSString *documentsDirectory = [paths.get(0]; // Get documents directory

        NSLog(@"filename='%@'",filename);
        NSString *filePath = [documentsDirectory stringByAppendingPathComponent:filename];
        NSLog(@"filePath='%@'",filePath);
        NSLog(@"fileURLWithPath='%@'",[NSURL fileURLWithPath:filePath]);

        return [NSURL fileURLWithPath(filePath substringFromIndex:1]];
    }


*/
}