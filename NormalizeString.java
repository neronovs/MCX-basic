package com.neronov.aleksei.mcxbasic;

import android.util.Log;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;

import static com.neronov.aleksei.mcxbasic.GlobalVars.getInstance;

/**
 * Created by newuser on 15.03.16.
 */
public class NormalizeString {
    private static final int NSNotFound = -1;
    private static final boolean NO = false;
    private static final boolean YES = true;
    private static final String TAG = MainActivity.class.getSimpleName();

    DigitalFunc digitalFunc;

    public String replaceCharWithCharInText(char sign, char witbSign, String string) {
        String result = "";
        int indexFirst = 0;
        boolean foundFirst = NO;
        if (string.length() > 0) {
            for (int i = 0; i < string.length(); i++) {
                if (string.substring(i, i + 1).equals("\"") && !foundFirst) {
                    foundFirst = YES;
                    indexFirst = i;
                }
                if (string.substring(i, i + 1).equals("\"") && foundFirst && indexFirst != i) {
                    foundFirst = NO;
                }
                if (!foundFirst) {
                    result = result + string.substring(i, i + 1);
                } else {
                    if (string.substring(i, i + 1).equals(String.valueOf(sign))) {
                        result = result + witbSign;
                        Log.d(TAG, "± " + result);
                    } else {
                        result = result + string.substring(i, i + 1);
                    }
                }
            }
        }
        return result;
    }

    public String lowcaseWithText(String string) {
        String result = "";
        int indexFirst = 0;
        boolean foundFirst = NO;
        if (string.length() > 0) {
            for (int i = 0; i < string.length(); i++) {
                if (string.substring(i, i + 1).equals("\"") && !foundFirst) {
                    foundFirst = YES;
                    indexFirst = i;
                }
                if (string.substring(i, i + 1).equals("\"") && foundFirst && indexFirst != i) {
                    foundFirst = NO;
                }
                if (foundFirst) {
                    result = result + string.substring(i, i + 1);
                }
                if (!foundFirst) {
                    result = result + string.substring(i, i + 1).toLowerCase();
                }
            }
        }
        return result;
    }

    public String removeSpacesWithText(String string) {
        String result = "";
        int indexFirst = 0;
        boolean foundFirst = NO;
        if (string.length() > 0) {
            for (int i = 0; i < string.length(); i++) {

                if (string.substring(i, i + 1).equals("\"") && !foundFirst) {
                    foundFirst = YES;
                    indexFirst = i;
                }
                if (string.substring(i, i + 1).equals("\"") && foundFirst && indexFirst != i) {
                    foundFirst = NO;
                }
                if (foundFirst) {
                    result = result + string.substring(i, i + 1);
                }
                if (!foundFirst && !string.substring(i, i + 1).equals(" ")) {
                    result = result + string.substring(i, i + 1);
                }
            }
        }
        return result;
    }

    public String removeSpaceInBegin(String string) {
        int i = 0;
        while (i < string.length() && string.substring(i, i + 1).equals(" ")) i++;
        return string.substring(i);
    }

    public String removeSpaceInBeginAndEnd(String string) {
        return string.trim();
    }

    public String removeText(String string) {
        ArrayList arr = new ArrayList();
        int indexFirst = 0;
        boolean foundFirst = NO;
        if (string.length() > 0) {
            for (int i = 0; i < string.length(); i++) {
                if (string.substring(i, i + 1).equals("\"") && !foundFirst) {
                    foundFirst = YES;
                    indexFirst = i;
                }
                if (string.substring(i, i + 1).equals("\"") && foundFirst && indexFirst != i) {
                    foundFirst = NO;
                    NSRange range = new NSRange(indexFirst, i - indexFirst + 1);
                    arr.add(string.substring(range.location, range.location + range.length));
                }
            }
        } else {
            getInstance().error = "Syntax error\n";
        }
        if (foundFirst) {
            getInstance().error = "Syntax error\n";
        } else {
            for (int i = 0; i < arr.size(); i++) {
                //Log.d(TAG, "± removeText->" + arr.get(i).toString());
                string = string.replaceAll(arr.get(i).toString(), "");
            }
        }
        return string;
    }

    public boolean insideText(String string, int index) {
        int indexFirst = 0;
        boolean foundFirst = NO;
        boolean result = NO;
        if (string.length() > 0) {
            for (int i = 0; i < string.length(); i++) {
                if (string.substring(i, i + 1).equals("\"") && !foundFirst) {
                    foundFirst = YES;
                    indexFirst = i;
                }
                if (string.substring(i, i + 1).equals("\"") && foundFirst && indexFirst != i) {
                    foundFirst = NO;
                }
                if (foundFirst && i == index) {
                    result = YES;
                }
            }
        }
        return result;
    }

    public boolean isPairedQuotes(String string) {
        int indexFirst = 0;
        boolean foundFirst = NO;
        boolean result = YES;
        if (string.length() > 0) {
            for (int i = 0; i < string.length(); i++) {
                if (string.substring(i, i + 1).equals("\"") && !foundFirst) {
                    foundFirst = YES;
                    result = NO;
                    indexFirst = i;
                }
                if (string.substring(i, i + 1).equals("\"") && foundFirst && indexFirst != i) {
                    foundFirst = NO;
                    result = YES;
                }
            }
        }
        return result;
    }

    public boolean isText(String string) {
        boolean foundFirst = NO;
        boolean result = NO;
        if (string.length() > 0) {
            if (string.substring(0, 1).equals("\"")) foundFirst = YES;
            if (string.substring(string.length() - 1).equals("\"") && foundFirst) result = YES;
        }
        return result;
    }

    public boolean isPairedBracket(String string) {
        int indexFirst = 0;
        boolean foundFirst = NO;
        boolean result = YES;
        if (string.length() > 0) {
            for (int i = 0; i < string.length(); i++) {
                if (string.substring(i, i + 1).equals("(") && !foundFirst) {
                    foundFirst = YES;
                    result = NO;
                    indexFirst = i;
                }
                if (string.substring(i, i + 1).equals(")") && foundFirst && indexFirst != i) {
                    foundFirst = NO;
                    result = YES;
                }
            }
        }
        return result;
    }

    public List extractNumToArray(String string) {
        List<String> arr = Arrays.asList(string.split(","));
        return arr;
    }

    public ArrayList extractTextToArray(String string) {
        ArrayList arr = new ArrayList();
        int indexFirst = 0;
        boolean foundFirst = NO;
        if (string.length() > 0) {
            for (int i = 0; i < string.length(); i++) {
                if (string.substring(i, i + 1).equals("\"") && !foundFirst) {
                    foundFirst = YES;
                    indexFirst = i + 1;
                }
                if (string.substring(i, i + 1).equals("\"") && foundFirst && indexFirst != i + 1) {
                    foundFirst = NO;
                    NSRange range = new NSRange(indexFirst, i - indexFirst);
                    arr.add(string.substring(range.location, range.location + range.length));
                }
            }
        } else {
            getInstance().error = "Syntax error\n";
        }

        if (foundFirst || arr.size() != 2) {
            getInstance().error = "Syntax error\n";
        } else {
            Log.d(TAG, "± extracted Text->" + arr);
        }
        return arr;
    }

    public ArrayList extractTextAndOtherToArray(String string) {
        ArrayList arr = new ArrayList();
        int index = 0;
        int indexFirst = 0;
        boolean foundFirst = NO;
        boolean haveText = NO;
        if (string.length() > 0) {
            for (int i = 0; i < string.length(); i++) {
                if (string.substring(i, i + 1).equals("\"") && !foundFirst) {
                    foundFirst = YES;
                    indexFirst = i;
                    NSRange range = new NSRange(index, i - index);
                    arr.add(string.substring(range.location, range.location + range.length));
                }
                if (string.substring(i, i + 1).equals("\"") && foundFirst && indexFirst != i) {
                    foundFirst = NO;
                    index = i + 1;
                    haveText = YES;
                    NSRange range = new NSRange(indexFirst, i - indexFirst + 1);
                    arr.add(string.substring(range.location, range.location + range.length));
                }
            }
            if (!haveText) {
                arr.add(string);
            } else {
                NSRange range = new NSRange(index, string.length() - index);
                arr.add(string.substring(range.location, range.location + range.length));
            }
        } else {
            getInstance().error = "Syntax error\n";
        }
        Log.d(TAG, "± extract TextAndOther To Array ->" + arr);
        return arr;
    }

    public ArrayList extractTextAndNumToArray(String string) {
        ArrayList arr = new ArrayList();
        int indexFirst = 0;
        boolean foundFirst = NO;
        if (string.length() > 0) {
            for (int i = 0; i < string.length(); i++) {
                if (string.substring(i, i + 1).equals("\"") && !foundFirst) {
                    foundFirst = YES;
                    indexFirst = i + 1;
                }
                if (string.substring(i, i + 1).equals("\"") && foundFirst && indexFirst != i + 1) {
                    foundFirst = NO;
                    NSRange range = new NSRange(indexFirst, i - indexFirst);
                    arr.add(string.substring(range.location, range.location + range.length));
                }
                if (string.substring(i, i + 1).equals("+") || string.substring(i, i + 1).equals(",")) {
                    foundFirst = NO;
                    NSRange range = new NSRange(i + 1, string.length() - i - 1);
                    String str = string.substring(range.location, range.location + range.length);
                    if (str.indexOf(',') != NSNotFound) str = str.split(",")[0];
                    arr.add(str);
                }
            }
        } else {
            getInstance().error = "Syntax error\n";
        }
        if (foundFirst || arr.size() < 2) {
            getInstance().error = "Syntax error\n";
        } else {
            Log.d(TAG, "± extract TextAndNum To Array ->" + arr);
        }

        return arr;
    }

    /*
    public ArrayList stringAndDigitsSeparateToArray(String string) {
        digitalFunc = new DigitalFunc();
        variables =[[Variables alloc]init];
        NSMutableArray * arr =[[NSMutableArray alloc]init];
        int arrIndex = 0;
        int indexFirst = 0;
        boolean foundFirst = NO;
        if (string.length > 0) {
            for (int i = 0; i <[string length];
            i++){
                if ([string characterAtIndex:i]=='"' && !foundFirst){
                    foundFirst = YES;
                    indexFirst = i + 1;
                }
                if ([string characterAtIndex:i]=='"' && foundFirst && indexFirst != i + 1){
                    foundFirst = NO;
                }
                if (([string characterAtIndex:
                i]=='+' ||[string characterAtIndex:i]=='-' ||[string characterAtIndex:i]=='/'
                        ||[string characterAtIndex:i]=='*' ||[string characterAtIndex:i]=='^' ||[
                string characterAtIndex:i]==',')&&!foundFirst){
                    NSRange range = NSMakeRange(arrIndex, i - arrIndex);
                    arrIndex = i + 1;
                    [arr addObject:[string substringWithRange:range]];
                }
                if (i ==[string length]-1){
                    NSRange range = NSMakeRange(arrIndex, i - arrIndex + 1);
                    [arr addObject:[string substringWithRange:range]];
                }

            }
            for (int i = 0; i <[arr count];
            i++){
                if ([[arr objectAtIndex:i]length]>4){
                    String tmp =[[self removeSpaceInBeginAndEnd:[arr objectAtIndex:i]]
                    substringToIndex:
                    5];
                    if ([tmp isEqual:@ "instr"]){
                        [arr replaceObjectAtIndex:i withObject:[NSString stringWithFormat:@
                        "%@,%@",[arr objectAtIndex:i],[arr objectAtIndex:i + 1]]];
                        [arr removeObjectAtIndex:i + 1];
                    }
                }
            }
            //if ([arr count]==0) [arr addObject:string];
            for (int i = 0; i <[arr count];
            i++)[arr replaceObjectAtIndex:i withObject:[self removeSpaceInBeginAndEnd:[
            arr objectAtIndex:i]]];
        }

        if (foundFirst) {
            globals.error = @ "Syntax error\n";
        } else {
            //        NSLog(@"Separated TEXT - %@",arr);
        }

        return arr;
    }

    -(NSArray*) stringSeparateToArray:(String)string
    {
        
        digitalFunc=[[DigitalFunc alloc]init];
        stringFunc=[[StringFunc alloc]init];
        variables=[[Variables alloc]init];
        NSMutableArray* arr=[[NSMutableArray alloc]init];
        int arrIndex = 0;
        int indexFirst = 0;
        boolean foundFirst=NO;
        if (string.length>0) {
            for (int i=0; i < [string length]; i++) {
                if ([string characterAtIndex:i] == '"' && !foundFirst) {
                    foundFirst=YES;
                    indexFirst=i+1;
                }
                if ([string characterAtIndex:i] == '"' && foundFirst && indexFirst!=i+1) {
                    foundFirst=NO;
                }
                if (([string characterAtIndex:i] == ',' && !foundFirst)) {
                    NSRange range = NSMakeRange(arrIndex,i-arrIndex);
                    arrIndex=i+1;
                    [arr addObject:[string substringWithRange:range]];
                }
                if (([string characterAtIndex:i] == '+' && !foundFirst)) {
                    NSRange range = NSMakeRange(arrIndex,i-arrIndex);
                    String tmp = [string substringWithRange:range];
                    {
                        arrIndex=i+1;
                        [arr addObject:tmp];
                    }
                }
                if (i==[string length]-1) {
                    NSRange range = NSMakeRange(arrIndex,i-arrIndex+1);
                    [arr addObject:[string substringWithRange:range]];
                }

            }

            //  склеиваем если функция
            //        NSLog(@"1...Separated TEXT - %@",arr);

            for (int i=0; i<[arr count]; i++) {
                if ([[arr objectAtIndex:i] length]>4){
                    String tmp=[[self removeSpaceInBeginAndEnd:[arr objectAtIndex:i]] substringToIndex:5];
                    if ([tmp isEqual:@"instr"]) {
                        [arr replaceObjectAtIndex:i withObject:[NSString stringWithFormat:@"%@,%@",[arr objectAtIndex:i],[arr objectAtIndex:i+1]]];
                        [arr removeObjectAtIndex:i+1];
                    }
                }
            }
            NSArray *arrStr = [self stringAndDigitsSeparateToArray:string];
            NSMutableArray *arrValue=[[NSMutableArray alloc]initWithArray:arrStr];
            if ([[string substringToIndex:1] isEqual:@"-"]) {
                [arrValue removeObjectAtIndex:0];
                [arrValue replaceObjectAtIndex:0 withObject:[NSString stringWithFormat:@"-%@",[arrValue objectAtIndex:0]]];
            }
            int index=0;
            NSMutableArray *arrSign = [[NSMutableArray alloc]init];
            for (int i=0; i<[arr count]-1; i++) {
                index=index+(int)[[arr objectAtIndex:i]length]+1;
                NSRange range = NSMakeRange(index-1,1);
                [arrSign addObject:[string substringWithRange:range]];
            }


            boolean adding=NO;
            int indAdd = 0;
            NSMutableArray *arrValueDelete = [[NSMutableArray alloc]init];
            if ([arrValue count]>2) for (int i=0; i<[arr count]-1; i++) {
                String notext=[self removeText:[arr objectAtIndex:i]];
                if ([digitalFunc mathFunction:notext]) {
                    adding=YES;
                    indAdd=i;
                }
                if (adding){
                    String addString=[NSString stringWithFormat:@"%@%@%@",[arr objectAtIndex:indAdd],[arrSign objectAtIndex:i],[arr objectAtIndex:i+1]];
                    [arrValueDelete addObject:[arr objectAtIndex:i+1]];
                    [arr replaceObjectAtIndex:indAdd withObject:addString];
                }
                if ([[arr objectAtIndex:i+1] rangeOfString:@")"].location != NSNotFound) {
                    adding=NO;
                }
            }
            [arr removeObjectsInArray:arrValueDelete];

            [arrValueDelete removeAllObjects];
            adding=NO;
            indAdd = 0;
            NSLog(@"ARR --- '%@'",arr);
            if ([arrValue count]>1) for (int i=0; i<[arr count]-1; i++) {
                String notext=[self removeText:[arr objectAtIndex:i]];
                if ([stringFunc stringFunction:notext]) {
                    adding=YES;
                    indAdd=i;
                    NSLog(@"adding '%@'",notext);
                }
                if (adding){
                    String addString=[NSString stringWithFormat:@"%@%@%@",[arr objectAtIndex:indAdd],[arrSign objectAtIndex:i],[arr objectAtIndex:i+1]];
                    [arrValueDelete addObject:[arr objectAtIndex:i+1]];
                    [arr replaceObjectAtIndex:indAdd withObject:addString];
                }
                if ([[arr objectAtIndex:i+1] rangeOfString:@")"].location != NSNotFound) {
                    adding=NO;
                }
            }
            [arr removeObjectsInArray:arrValueDelete];

            //if ([arr count]==0) [arr addObject:string];
            for (int i=0; i<[arr count]; i++)  [arr replaceObjectAtIndex:i withObject:[self removeSpaceInBeginAndEnd:[arr objectAtIndex:i]]];
        }

        if (foundFirst) {
            globals.error = @"Syntax error\n";
        }else{
            NSLog(@"Separated TEXT - %@",arr);
        }

        return arr;
    }

    -(NSArray*) stringSeparateAllToArray:(String)string
    {
        
        digitalFunc=[[DigitalFunc alloc]init];
        variables=[[Variables alloc]init];
        NSMutableArray* arr=[[NSMutableArray alloc]init];
        int arrIndex = 0;
        int indexFirst = 0;
        boolean foundFirst=NO;
        if (string.length>0) {
            for (int i=0; i < [string length]; i++) {
                if ([string characterAtIndex:i] == '"' && !foundFirst) {
                    foundFirst=YES;
                    indexFirst=i+1;
                }
                if ([string characterAtIndex:i] == '"' && foundFirst && indexFirst!=i+1) {
                    foundFirst=NO;
                }
                if (([string characterAtIndex:i] == ',' && !foundFirst)) {
                    NSRange range = NSMakeRange(arrIndex,i-arrIndex);
                    arrIndex=i+1;
                    [arr addObject:[string substringWithRange:range]];
                    [arr addObject:@","];
                }
                if (([string characterAtIndex:i] == '+' && !foundFirst)) {
                    NSRange range = NSMakeRange(arrIndex,i-arrIndex);
                    String tmp = [string substringWithRange:range];
                    {
                        arrIndex=i+1;
                        [arr addObject:tmp];
                        [arr addObject:@"+"];
                    }
                }
                if (([string characterAtIndex:i] == '-' && !foundFirst)) {
                    NSRange range = NSMakeRange(arrIndex,i-arrIndex);
                    String tmp = [string substringWithRange:range];
                    {
                        arrIndex=i+1;
                        [arr addObject:tmp];
                        [arr addObject:@"-"];
                    }
                }
                if (([string characterAtIndex:i] == '/' && !foundFirst)) {
                    NSRange range = NSMakeRange(arrIndex,i-arrIndex);
                    String tmp = [string substringWithRange:range];
                    {
                        arrIndex=i+1;
                        [arr addObject:tmp];
                        [arr addObject:@"/"];
                    }
                }
                if (([string characterAtIndex:i] == '*' && !foundFirst)) {
                    NSRange range = NSMakeRange(arrIndex,i-arrIndex);
                    String tmp = [string substringWithRange:range];
                    {
                        arrIndex=i+1;
                        [arr addObject:tmp];
                        [arr addObject:@"*"];
                    }
                }
                if (([string characterAtIndex:i] == '^' && !foundFirst)) {
                    NSRange range = NSMakeRange(arrIndex,i-arrIndex);
                    String tmp = [string substringWithRange:range];
                    {
                        arrIndex=i+1;
                        [arr addObject:tmp];
                        [arr addObject:@"^"];
                    }
                }
                if (([string characterAtIndex:i] == '(' && !foundFirst)) {
                    NSRange range = NSMakeRange(arrIndex,i-arrIndex);
                    String tmp = [string substringWithRange:range];
                    {
                        arrIndex=i+1;
                        [arr addObject:tmp];
                        [arr addObject:@"("];
                    }
                }
                if (([string characterAtIndex:i] == ')' && !foundFirst)) {
                    NSRange range = NSMakeRange(arrIndex,i-arrIndex);
                    String tmp = [string substringWithRange:range];
                    {
                        arrIndex=i+1;
                        [arr addObject:tmp];
                        [arr addObject:@")"];
                    }
                }
                if (i==[string length]-1) {
                    NSRange range = NSMakeRange(arrIndex,i-arrIndex+1);
                    [arr addObject:[string substringWithRange:range]];
                }

            }

        }

        if (foundFirst) {
            globals.error = @"Syntax error\n";
        }else{
            //        NSLog(@"Separated ALL TEXT - %@",arr);
        }

        return arr;
    }

    */
}
