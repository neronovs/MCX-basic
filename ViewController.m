//
//  ViewController.m
//  Basic
//
//  Created by Алексей Неронов on 14.10.15.
//  Copyright © 2015 Алексей Неронов. All rights reserved.
//
//

#import "ViewController.h"
#import "GlobalVars.h"
#import "RunCommand.h"
#import "NormalizeString.h"
#import "Variables.h"
#import "AppDelegate.h"
#import "DigitalFunc.h"
#import "Files.h"

@implementation ViewController
{
    GlobalVars *globals;
    Variables *variables;
    RunCommand *runCommand;
    NSInteger position;
    NormalizeString *normaStr;
    BOOL inputMode;
    int inputCount;
    BOOL nextCommand;
    NSTimer *runTimer;
    AppDelegate *appDelegate;
    DigitalFunc *digitalFunc;
    int textIndex;
    Files *files;
}

@synthesize commandWindow;

- (void)viewDidLoad {
    [super viewDidLoad];
    NSLog(@"init");
    textIndex=0;
    //    NSLog(@"viewDidLoad screen size %@",NSStringFromRect(commandWindow.frame));
    
    appDelegate = [[NSApplication sharedApplication] delegate];
    appDelegate.viewController = self;
    
    inputMode = NO;
    inputCount = 0;
    globals = [GlobalVars sharedInstance];
    runCommand = [[RunCommand alloc] init];
    [self setBackground:[self detectColor:@"4"]];
    [self setColor:[self detectColor:@"15"]];
    commandWindow.automaticQuoteSubstitutionEnabled = NO;
    commandWindow.enabledTextCheckingTypes = 0;
    commandWindow.delegate=self;
    [commandWindow setTextColor:[NSColor whiteColor]];
    [commandWindow setFont:[NSFont fontWithName:@"Menlo" size:16]];
    [self version];
    [self printOk];
}

- (void)setRepresentedObject:(id)representedObject {
    [super setRepresentedObject:representedObject];
    
    // Update the view, if already loaded
}

-(NSString*) returnBaseCommand:(NSString*)string
{
    NSString* result = [string componentsSeparatedByString:@" "][0];
    globals = [GlobalVars sharedInstance];
    for (int i=0; i<[globals.listOfAll count]; i++) {
        NSRange range=[[string lowercaseString] rangeOfString:[globals.listOfAll objectAtIndex:i]];
        if (range.location != NSNotFound && range.location==0) result=[globals.listOfAll objectAtIndex:i];
    }
    return result;
}

- (NSRange)getViewableRange:(NSTextView *)tv{
    NSScrollView *sv = [tv enclosingScrollView];
    if(!sv) return NSMakeRange(0,0);
    NSLayoutManager *lm = [tv layoutManager];
    NSRect visRect = [tv visibleRect];
    
    NSPoint tco = [tv textContainerOrigin];
    visRect.origin.x -= tco.x;
    visRect.origin.y -= tco.y;
    
    NSRange glyphRange = [lm glyphRangeForBoundingRect:visRect
                          
                                       inTextContainer:[tv textContainer]];
    NSRange charRange = [lm characterRangeForGlyphRange:glyphRange
                         
                                       actualGlyphRange:nil];
    return charRange;
}


-(void)listColor
{
    globals = [GlobalVars sharedInstance];
    digitalFunc=[[DigitalFunc alloc]init];
    NSCharacterSet * alphaSet = [[NSCharacterSet characterSetWithCharactersInString:@"abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLKMNOPQRSTUVWXYZ"] invertedSet];
    NSString *text = [commandWindow string];
    NSString *string = [commandWindow string].lowercaseString;
    NSMutableArray* arrayOfRanges=[[NSMutableArray alloc]init];
    NSMutableArray* arrayOfRangesText=[[NSMutableArray alloc]init];
    NSMutableArray* arrayOfRangesNumber=[[NSMutableArray alloc]init];
    
    for (int i=0; i<[globals.listOfAll count]; i++){
        //NSRange searchRange = NSMakeRange(0,string.length);
        //NSRange searchRange = [self getViewableRange:commandWindow];
        NSRange searchRange = NSMakeRange(textIndex,string.length-textIndex);
        NSRange foundRange;
        while (searchRange.location < string.length)
        {
            searchRange.length = string.length-searchRange.location;
            foundRange = [string rangeOfString:[globals.listOfAll objectAtIndex:i] options:0 range:searchRange];
            if (foundRange.location != NSNotFound) {
                // found an occurrence of the substring! do stuff here
                searchRange.location = foundRange.location+foundRange.length;
                bool prevEmpty=YES;
                bool afterEmpty=YES;
                if (foundRange.location+foundRange.length<string.length-1){
                    NSString* tmp=[string substringWithRange:NSMakeRange(foundRange.location+foundRange.length,1)];
                    //                    NSLog(@"colorWord ->%@",tmp);
                    if ([tmp rangeOfCharacterFromSet:alphaSet].location == NSNotFound) prevEmpty=NO;
                }
                if (foundRange.location>0){
                    NSString* tmp=[string substringWithRange:NSMakeRange(foundRange.location-1,1)];
                    if ([tmp rangeOfCharacterFromSet:alphaSet].location == NSNotFound) prevEmpty=NO;
                }
                if (prevEmpty && afterEmpty)
                {
                    [arrayOfRanges addObject:[NSValue valueWithRange:foundRange]];
                }
            } else {
                // no more substring to find
                break;
            }
        }
    }
    
    NSRange searchRange = NSMakeRange(textIndex,string.length-textIndex);
    int counter=textIndex;
    NSArray *lines = [[string substringWithRange:searchRange] componentsSeparatedByString:@"\n"];
    for (int i=0; i<[lines count]; i++) {
        
        NSString* base = [self returnBaseCommand:[lines objectAtIndex:i]];
        if ([digitalFunc isOnlyDigits:base]&&![base isEqual:@""]) { // manual program string set
            [arrayOfRangesNumber addObject:[NSValue valueWithRange:NSMakeRange(counter,base.length)]];
        }
        
        int indexFirst = 0;
        BOOL foundFirst=NO;
        NSString* lineString=[lines objectAtIndex:i];
        if (lineString.length>0) {
            for (int i=0; i < [lineString length]; i++) {
                if ([lineString characterAtIndex:i] == '"' && !foundFirst) {
                    foundFirst=YES;
                    indexFirst=i;
                }
                if ([lineString characterAtIndex:i] == '"' && foundFirst && indexFirst!=i) {
                    foundFirst=NO;
                    NSRange range = NSMakeRange(counter+indexFirst,i-indexFirst+1);
                    [arrayOfRangesText addObject:[NSValue valueWithRange:range]];
                }
            }
        }
        
        counter=counter+(int)[[lines objectAtIndex:i] length]+1;
    }
    
    if ([arrayOfRanges count]>0){
        [commandWindow setRichText:YES];
        [commandWindow setString:text];
        if ([arrayOfRanges count]>0)
            for (int i=0; i<[arrayOfRanges count]; i++) [commandWindow setTextColor:[self colorWithHexString:@"8EE67C"] range:[[arrayOfRanges objectAtIndex:i] rangeValue]];
        if ([arrayOfRangesText count]>0)
            for (int i=0; i<[arrayOfRangesText count]; i++) [commandWindow setTextColor:[self colorWithHexString:@"DED27C"] range:[[arrayOfRangesText objectAtIndex:i] rangeValue]];
        if ([arrayOfRangesNumber count]>0)
            for (int i=0; i<[arrayOfRangesNumber count]; i++) [commandWindow setTextColor:[self colorWithHexString:@"ADADAD"]  range:[[arrayOfRangesNumber objectAtIndex:i] rangeValue]];
    }
}


- (BOOL)textView:(NSTextView *)textView doCommandBySelector:(SEL)commandSelector
{
    variables=[[Variables alloc]init];
    globals = [GlobalVars sharedInstance];
    NSLog(@"%@",NSStringFromSelector(commandSelector));
    if(commandSelector == @selector(insertNewline:))
    {
        // return key
        globals.showError=NO;
        NSString *allTheText = [commandWindow string];
        NSArray *lines = [allTheText componentsSeparatedByString:@"\n"];
        position = [[[commandWindow selectedRanges] objectAtIndex:0] rangeValue].location;
        NSArray *arrLines = [[allTheText substringToIndex:position] componentsSeparatedByString:@"\n"];
        globals.lineNumber = [arrLines count]-1;
        globals.command = [lines objectAtIndex:globals.lineNumber];
        
        NSLog(@"command %@",globals.command);
        //        NSLog(@"listOfStrings %@",globals.listOfStrings);
        textIndex=(int)commandWindow.string.length;
        
        if (inputMode){
            if (![globals.input isEqual:@""])
            {
                
                int extrAdd=2;
                if ([globals.listOfStrings count]==0) {
                    [globals.listOfStrings addObject:@"t"];
                    extrAdd=1;
                }
                //                NSLog(@"input mode ='%@' listofstring[0]='%@'",globals.input,[globals.listOfStrings objectAtIndex:0]);
                NSArray *arr = [[globals.input substringFromIndex:1] componentsSeparatedByCharactersInSet:[NSCharacterSet characterSetWithCharactersInString:@","]];
                NSString* entered;
                NSInteger indexInput=[[globals.listOfStrings objectAtIndex:0]length]+extrAdd;
                //                NSLog(@"input-- '%@' indexed-'%@'",[lines lastObject],[globals.listOfStrings objectAtIndex:0]);
                entered=[[lines lastObject] substringFromIndex:indexInput];
                NSString* str=[NSString stringWithFormat:@"%@=%@",[arr objectAtIndex:inputCount],entered];
                if ([[[arr objectAtIndex:inputCount] substringFromIndex:[[arr objectAtIndex:inputCount] length] - 1] isEqual:@"$"])
                    str=[NSString stringWithFormat:@"%@=\"%@\"",[arr objectAtIndex:inputCount],entered];
                //                NSLog(@"input--runCommand %@",str);
                [runCommand set:str];
                inputCount++;
                if (inputCount>=[arr count]) {
                    [self returnCR];
                    inputMode=NO;
                    inputCount=0;
                    nextCommand=YES;
                } else {
                    [self returnCR];
                    [commandWindow insertText:@"? " replacementRange:NSMakeRange([[self.commandWindow string] length],0)];
                }
            } else {
                [self returnCR];
                inputMode=NO;
                inputCount=0;
                [self syntaxError];
                nextCommand=YES;
            }
            return YES;
        } else {
            if (![globals.command isEqual:@""]){
                int l=0;
                if (globals.command.length>3) l=4;
                if (globals.isOkSet) [self returnCR];
                if (!globals.autoSet) {
                    if ([runCommand set:globals.command]) {
                        for (int i=0; i<[globals.listOfStrings count]; i++) {
                            [commandWindow insertText:[globals.listOfStrings objectAtIndex:i] replacementRange:NSMakeRange([[self.commandWindow string] length],0)];
                        }
                        
                        if (!globals.autoSet || !globals.run) [self printOk];
                    }else{
                        [self otherCommands:globals.command];
                        [self printOk];
                    }
                }else{
                    [runCommand autoProgramSet:globals.command];
                    for (int i=0; i<[globals.listOfStrings count]; i++) {
                        [commandWindow insertText:[globals.listOfStrings objectAtIndex:i] replacementRange:NSMakeRange([[self.commandWindow string] length],0)];
                    }
                }
                
                if (globals.command.length>3){
                    if ([[globals.command substringToIndex:4] isEqual:@"list"]) [self listColor];
                    if ([[globals.command substringToIndex:4] isEqual:@"help"]) [self listColor];
                }
                
                return YES;
            }
        }
    }
    else if(commandSelector == @selector(cancelOperation:))
    {
        [self stopRunning];
        [runCommand autoProgramStop];
        [self returnCR];
        return YES;
    }
    
    return NO;
}

-(void) command:(NSString*)commandRun
{
    globals = [GlobalVars sharedInstance];
    globals.command = commandRun;
    textIndex=(int)commandWindow.string.length;
    
    NSLog(@"command %@",globals.command);
    
    if (![globals.command isEqual:@""]){
        int l=0;
        if (globals.command.length>3) l=4;
        if (globals.isOkSet) [self returnCR];
        if (!globals.autoSet) {
            if ([runCommand set:globals.command]) {
                for (int i=0; i<[globals.listOfStrings count]; i++) {
                    [commandWindow insertText:[globals.listOfStrings objectAtIndex:i] replacementRange:NSMakeRange([[self.commandWindow string] length],0)];
                }
                
                if (!globals.autoSet || !globals.run) [self printOk];
            }else{
                [self otherCommands:globals.command];
                [self printOk];
            }
        }else{
            [runCommand autoProgramSet:globals.command];
            for (int i=0; i<[globals.listOfStrings count]; i++) {
                [commandWindow insertText:[globals.listOfStrings objectAtIndex:i] replacementRange:NSMakeRange([[self.commandWindow string] length],0)];
            }
        }
    }
    if ([[globals.command substringToIndex:4] isEqual:@"list"]) {
        [self listColor];
    }
}

-(void)runProgram
{
    if (nextCommand)
    {
        globals = [GlobalVars sharedInstance];
        NSString* currentLine=[globals.listOfProgram objectAtIndex:globals.runIndex];
        NSString* untilSpace = [currentLine componentsSeparatedByString:@" "][0];
        globals.runnedLine=untilSpace;
        NSInteger indexforAfterSpace = untilSpace.length;
        NSString* commandRun = [currentLine substringFromIndex:indexforAfterSpace+1];
        
        if ([[commandRun substringToIndex:1] isEqual:@" "]) {
            normaStr=[[NormalizeString alloc]init];
            commandRun=[normaStr removeSpaceInBegin:commandRun];
        }
        NSLog(@"%d Command '%@'",globals.runIndex,commandRun);
        if (![commandRun isEqual:@""]){
            if ([runCommand set:commandRun]){
                for (int n=0; n<[globals.listOfStrings count]; n++) {
                    [commandWindow insertText:[globals.listOfStrings objectAtIndex:n] replacementRange:NSMakeRange([[self.commandWindow string] length],0)];
                }
            }else{
                [self otherCommands:commandRun];
            }
        }
        globals.runIndex++;
        if (globals.runIndex>=[globals.listOfProgram count]) [self stopRunning];
        if (![globals.error isEqual:@""]) [self stopRunning];
        if (!globals.run) [self stopRunning];
    }
    if (globals.run) runTimer = [NSTimer scheduledTimerWithTimeInterval:0.01 target:self selector:@selector(runProgram) userInfo:nil repeats:NO];
}

-(void) stopRunning
{
    [self printOk];
    [runTimer invalidate];
    runTimer = nil;
    globals = [GlobalVars sharedInstance];
    globals.runIndex=0;
    globals.run=NO;
    globals.isOkSet=YES;
    inputMode=NO;
    globals.scanKeyOn=NO;
    files = [[Files alloc]init];
    [files clear];
    NSLog(@"Program is OVER!!!!! %@",globals.error);
}

- (NSURL *) fileToURL:(NSString*)filename
{
    NSArray *paths = NSSearchPathForDirectoriesInDomains(NSDocumentDirectory, NSUserDomainMask, YES);
    NSString *documentsDirectory = [paths objectAtIndex:0]; // Get documents directory
    
    NSLog(@"filename='%@'",filename);
    NSString *filePath = [documentsDirectory stringByAppendingPathComponent:filename];
    NSLog(@"filePath='%@'",filePath);
    NSLog(@"fileURLWithPath='%@'",[NSURL fileURLWithPath:filePath]);
    
    return [NSURL fileURLWithPath:[filePath substringFromIndex:1]];
}

-(void)share
{
    NSLog(@"shareCommand for '%@'",globals.fileName);
    NSString *texttoshare = @"MCX Basic file. Sent by share command.";
    NSString *fileName = globals.fileName;
    NSURL *saveUrl = [NSURL URLWithString:[NSString stringWithFormat:@"file://%@", NSTemporaryDirectory()]];
    saveUrl = [saveUrl URLByAppendingPathComponent:fileName];
    // Write image to temporary directory
    NSString* arrayText = [globals.listOfProgram componentsJoinedByString: @"\n"];
    NSError *error;
    [arrayText writeToURL:saveUrl atomically:YES encoding:NSUTF8StringEncoding error:&error];
    // Attach the raw NSURL pointing to the local file
    NSArray *shareItems = [NSArray arrayWithObjects:saveUrl, texttoshare, nil];
    // Open share prompt
    NSSharingService *service = [NSSharingService sharingServiceNamed:NSSharingServiceNameComposeMessage];
    service.delegate = self;
    [service performWithItems:shareItems];
}

-(void) otherCommands:(NSString*)commandRun
{
    //    NSLog(@"otherCommands -'%@'",commandRun);
    commandRun=[commandRun lowercaseString];
    NSString* str=commandRun;
    commandRun=[commandRun componentsSeparatedByString:@" "][0];
    globals = [GlobalVars sharedInstance];
    for (int i=0; i<[globals.listOfAll count]; i++) {
        NSRange range=[[commandRun lowercaseString] rangeOfString:[globals.listOfAll objectAtIndex:i]];
        if (range.location != NSNotFound && range.location==0) {
            commandRun=[globals.listOfAll objectAtIndex:i];
        }
    }
    if ([commandRun isEqual:@"cls"]) {
        [commandWindow setString:@""];
    }else if ([commandRun isEqual:@"reset"]) {
        [self reset];
    }else if ([commandRun isEqual:@"new"]) {
        [self new];
    }else if ([commandRun isEqual:@"color"]) {
        str=[str stringByReplacingOccurrencesOfString:@"color" withString:@""];
        str=[str stringByReplacingOccurrencesOfString:@" " withString:@""];
        NSString* foreground=[str componentsSeparatedByString:@","][0];
        if (![foreground isEqual:@""]) [self setColor:[self detectColor:foreground]];
        if ([str rangeOfString:@","].location != NSNotFound ) {
            NSString* background=[str componentsSeparatedByString:@","][1];
            [self setBackground:[self detectColor:background]];
        }
    }else if ([commandRun isEqual:@"share"]&&!globals.run) {
        [self share];
    }else if ([commandRun isEqual:@"run"]) {
        if ([globals.listOfProgram count]>0){
            //            NSLog(@"start running program");
            nextCommand=YES;
            globals.runIndex=0;
            globals.error=@"";
            globals.run=YES;
            [self runProgram];
        }
    }else if ([commandRun isEqual:@"if"] && [globals.error isEqual:@""]) {
        NSLog(@"if in progress on ViewController. %@",globals.commandIf);
        if (![globals.commandIf isEqual:@""]){
            if ([runCommand set:globals.commandIf]){
                for (int n=0; n<[globals.listOfStrings count]; n++) {
                    [commandWindow insertText:[globals.listOfStrings objectAtIndex:n] replacementRange:NSMakeRange([[self.commandWindow string] length],0)];
                }
            }else{
                [self otherCommands:globals.commandIf];
            }
        }
        
    }else if ([commandRun isEqual:@"input"] && [globals.error isEqual:@""]) {
        // NSLog(@"input - error=%@",globals.error);
        nextCommand=NO;
        if ([globals.listOfStrings count]>0){
            [commandWindow insertText:[globals.listOfStrings objectAtIndex:0] replacementRange:NSMakeRange([[self.commandWindow string] length],0)];
            //[self returnCR];
        }
        [commandWindow insertText:@"? " replacementRange:NSMakeRange([[self.commandWindow string] length],0)];
        globals.isOkSet=NO;
        inputMode=YES;
    }
    /*
     else{
     [self syntaxError];
     }
     */
    if (![globals.error isEqual:@""]) {
        [self syntaxError];
    }
    
}

-(void)setBackground:(NSColor*)back
{
    [commandWindow setBackgroundColor:back];
}

-(void)setColor:(NSColor*)color
{
    [commandWindow setTextColor:color];
}

-(NSColor*) detectColor:(NSString*)colorNumber
{
    variables=[[Variables alloc]init];
    if ([variables variableIsPresent:colorNumber]) {
        colorNumber=[variables returnContainOfVariable:colorNumber];
    }
    
    //    NSLog(@"Color # %@\n",colorNumber);
    NSColor* result;
    if ([colorNumber isEqual:@"1"]) result=[NSColor blackColor];
    if ([colorNumber isEqual:@"2"]) result=[self colorWithHexString:@"00E100"];
    if ([colorNumber isEqual:@"3"]) result=[self colorWithHexString:@"27FF62"];
    if ([colorNumber isEqual:@"4"]) result=[self colorWithHexString:@"0836A0"];
    if ([colorNumber isEqual:@"5"]) result=[self colorWithHexString:@"5B60FF"];
    if ([colorNumber isEqual:@"6"]) result=[self colorWithHexString:@"C50023"];
    if ([colorNumber isEqual:@"7"]) result=[self colorWithHexString:@"0EDFFF"];
    if ([colorNumber isEqual:@"8"]) result=[self colorWithHexString:@"FF0019"];
    if ([colorNumber isEqual:@"9"]) result=[self colorWithHexString:@"FF6372"];
    if ([colorNumber isEqual:@"10"]) result=[self colorWithHexString:@"D7E300"];
    if ([colorNumber isEqual:@"11"]) result=[self colorWithHexString:@"CAD186"];
    if ([colorNumber isEqual:@"12"]) result=[self colorWithHexString:@"009507"];
    if ([colorNumber isEqual:@"13"]) result=[self colorWithHexString:@"DA0FB1"];
    if ([colorNumber isEqual:@"14"]) result=[self colorWithHexString:@"ADADAD"];
    if ([colorNumber isEqual:@"15"]) result=[NSColor whiteColor];
    if ([colorNumber intValue]>15) {
        result=[NSColor whiteColor];
        globals.error = @"Incorrect color\n";
        NSLog(@"Incorrect color");
    }
    return result;
}

-(NSColor*)colorWithHexString:(NSString*)hex
{
    NSString *cString = [[hex stringByTrimmingCharactersInSet:[NSCharacterSet whitespaceAndNewlineCharacterSet]] uppercaseString];
    
    // String should be 6 or 8 characters
    if ([cString length] < 6) return [NSColor grayColor];
    
    // strip 0X if it appears
    if ([cString hasPrefix:@"0X"]) cString = [cString substringFromIndex:2];
    
    if ([cString length] != 6) return  [NSColor grayColor];
    
    // Separate into r, g, b substrings
    NSRange range;
    range.location = 0;
    range.length = 2;
    NSString *rString = [cString substringWithRange:range];
    
    range.location = 2;
    NSString *gString = [cString substringWithRange:range];
    
    range.location = 4;
    NSString *bString = [cString substringWithRange:range];
    
    // Scan values
    unsigned int r, g, b;
    [[NSScanner scannerWithString:rString] scanHexInt:&r];
    [[NSScanner scannerWithString:gString] scanHexInt:&g];
    [[NSScanner scannerWithString:bString] scanHexInt:&b];
    
    return [NSColor colorWithRed:((float) r / 255.0f)
                           green:((float) g / 255.0f)
                            blue:((float) b / 255.0f)
                           alpha:1.0f];
}

-(void) returnCR
{
    [commandWindow insertText:@"\n" replacementRange:NSMakeRange([[self.commandWindow string] length],0)];
    [commandWindow setSelectedRange:NSMakeRange([[self.commandWindow string] length],0)];
}

-(void) printOk
{
    _window = [[[NSApplication sharedApplication] windows] firstObject];
    _window.title = [NSString stringWithFormat:@"MCX Basic  %@",globals.fileName];
    NSString *allTheText = [commandWindow string];
    NSArray *lines = [allTheText componentsSeparatedByString:@"\n"];
    if ([lines count]>2)
        if (globals.isOkSet && !globals.run && ![[lines objectAtIndex:[lines count]-2] isEqual:@"Ok"]) {
            [commandWindow insertText:@"Ok\n" replacementRange:NSMakeRange([[self.commandWindow string] length],0)];
            [commandWindow setSelectedRange:NSMakeRange([[self.commandWindow string] length],0)];
        }
    globals.isOkSet=YES;
}

-(void) ok
{
    [self returnCR];
    [self printOk];
}

-(void) syntaxError
{
    if (!globals.showError)
    {
        globals.isOkSet=NO;
        globals = [GlobalVars sharedInstance];
        if (globals.run) {
            globals.error=[NSString stringWithFormat:@"%@at line %@\n",globals.error,globals.runnedLine];
        }
        [self stopRunning];
        [commandWindow insertText:globals.error replacementRange:NSMakeRange([[self.commandWindow string] length],0)];
        [commandWindow setSelectedRange:NSMakeRange([[self.commandWindow string] length],0)];
        globals.showError=YES;
    }
}

-(void) version
{
    [runCommand set:@"ver"];
    for (int i=0; i<[globals.listOfStrings count]; i++) {
        [commandWindow insertText:[globals.listOfStrings objectAtIndex:i] replacementRange:NSMakeRange([[self.commandWindow string] length],0)];
    }
}

-(void) reset
{
    globals.fileName=@"";
    [commandWindow setString:@""];
    NSLog(@"init");
    inputMode = NO;
    inputCount = 0;
    globals = [GlobalVars sharedInstance];
    runCommand = [[RunCommand alloc] init];
    [self setBackground:[self detectColor:@"4"]];
    [self setColor:[self detectColor:@"15"]];
    commandWindow.automaticQuoteSubstitutionEnabled = NO;
    commandWindow.enabledTextCheckingTypes = 0;
    commandWindow.delegate=self;
    [commandWindow setTextColor:[NSColor whiteColor]];
    [commandWindow setFont:[NSFont fontWithName:@"Menlo" size:16]];
    [self version];
    [self printOk];
}

-(void) new
{
    globals.fileName=@"";
    NSLog(@"new");
    inputMode = NO;
    inputCount = 0;
    globals = [GlobalVars sharedInstance];
    runCommand = [[RunCommand alloc] init];
    commandWindow.automaticQuoteSubstitutionEnabled = NO;
    commandWindow.enabledTextCheckingTypes = 0;
    commandWindow.delegate=self;
}

-(NSString*) selectFile
{
    NSString*result;
    // Loop counter.
    int i;
    // Create a File Open Dialog class.
    NSOpenPanel* openDlg = [NSOpenPanel openPanel];
    // Set array of file types
    NSArray *fileTypesArray;
    fileTypesArray = [NSArray arrayWithObjects:@"bas", nil];
    // Enable options in the dialog.
    [openDlg setCanChooseFiles:YES];
    [openDlg setAllowedFileTypes:fileTypesArray];
    [openDlg setAllowsMultipleSelection:TRUE];
    // Display the dialog box.  If the OK pressed,
    // process the files.
    if ( [openDlg runModal] == NSModalResponseOK ) {
        // Gets list of all files selected
        NSArray *filess = [openDlg URLs];
        // Loop through the files and process them.
        for( i = 0; i < [filess count]; i++ ) {
            // Do something with the filename.
            //NSLog(@"Loading file at path: %@", [[filess objectAtIndex:i] path]);
            result=[NSString stringWithFormat:@"%@",[[filess objectAtIndex:i] path]];
        }
    }
    return result;
}

- (NSString*)saveFile_New:(BOOL)isNew
{
    NSString* resultSave;
    NSSavePanel *panel = [NSSavePanel savePanel];
    NSString *fileName=[[globals.fileName lastPathComponent] stringByDeletingPathExtension];
    
    if ([fileName length] == 0 || isNew) {
        fileName=@"untitle.bas";
    }
    
    [panel setMessage:@"Please select a path where to save MCX Basic file."]; // Message inside modal window
    [panel setAllowsOtherFileTypes:YES];
    [panel setExtensionHidden:YES];
    [panel setCanCreateDirectories:YES];
    [panel setNameFieldStringValue:fileName];
    [panel setTitle:@"Saving MCX Basic file..."]; // Window title
    [panel setAllowedFileTypes:[NSArray arrayWithObjects:@"bas", nil]];
    
    NSInteger result = [panel runModal];
    NSError *error = nil;
    
    if (result == NSModalResponseOK) {
        resultSave = [[panel URL] path];
        if (error) {
            [NSApp presentError:error];
            resultSave=@"";
        }
    }
    return resultSave;
}

@end
