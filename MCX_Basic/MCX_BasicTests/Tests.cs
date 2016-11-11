using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MCX_Basic;
using System.Collections.Generic;
using System.Diagnostics;

namespace MCX_BasicTests
{
    [TestClass]
    public class Tests
    {
        string str = "", res = "";

        [TestMethod]
        public void testNormalizeString()
        {
            //lowcaseWithText
            NormalizeString normalizeString = new NormalizeString();
            str = "123 qweWER\"DSFds\"AAA";
            res = "123 qwewer\"DSFds\"aaa";
            Assert.AreEqual(res, normalizeString.lowcaseWithText(str));
            //replaceCharWithCharInText
            str = "123 qweWER\"DSFds\"AAA";
            res = "123 qwEWER\"DSFds\"AAA";
            Assert.AreEqual(res, normalizeString.replaceCharWithCharInText('e', 'E', str));
            //removeSpaceInBeginAndEnd
            str = "  123 qweWER\"DSFds\"AAA  ";
            res = "123 qweWER\"DSFds\"AAA";
            Assert.AreEqual(res, normalizeString.removeSpaceInBeginAndEnd(str));
            //removeSpaceInBegin
            str = "   123 qweWER\"DSFds\"AAA   ";
            res = "123 qweWER\"DSFds\"AAA   ";
            Assert.AreEqual(res, normalizeString.removeSpaceInBegin(str));
            //removeSpacesWithText
            str = "   123 qwe  WER\"  DSF ds\"AAA   ";
            res = "123qweWER\"  DSF ds\"AAA";
            Assert.AreEqual(res, normalizeString.removeSpacesWithText(str));
            //removeText
            str = "   123 qweWER\"DSFds\"AAA   ";
            res = "   123 qweWERAAA   ";
            Assert.AreEqual(res, normalizeString.removeText(str));
            str = "123 \"qwe\" WER\"DSFds\"AAA \"  \"";
            List<String> array1 = new List<String>() { "qwe", "DSFds", "  " };
            CollectionAssert.AreEqual(array1, normalizeString.extractTextToArray(str));
            //removeSpaceInBeginAndEnd
            str = "   123 qweWER\"DSFds\"AAA   ";
            res = "123 qweWER\"DSFds\"AAA";
            Assert.AreEqual(res, normalizeString.removeSpaceInBeginAndEnd(str));
            //extractTextAndOtherToArray
            str = "123 \"qwe\" WER\"DSFds\"AAA \"  \"";
            array1 = new List<String>() { "123 ", "\"qwe\"", " WER", "\"DSFds\"", "AAA ", "\"  \"", "" };
            CollectionAssert.AreEqual(array1, normalizeString.extractTextAndOtherToArray(str));
            //extractTextAndNumToArray
            str = "123 \"qwe\" WER\"DSFds\"AAA \"  \"";
            array1 = new List<String>() { "qwe", "DSFds", "  " };
            CollectionAssert.AreEqual(array1, normalizeString.extractTextAndNumToArray(str));
            //extractNumToArray
            str = "123,11,22,33,44,900";
            array1 = new List<String>() { "123", "11", "22", "33", "44", "900" };
            CollectionAssert.AreEqual(array1, normalizeString.extractNumToArray(str));
            //stringSeparateToArray
            str = "123\"qwe\",WER\"DSFds\"AAA+\"  \"";
            array1 = new List<String>() { "123\"qwe\"", "WER\"DSFds\"AAA", "\"  \"" };
            CollectionAssert.AreEqual(array1, normalizeString.stringSeparateToArray(str));
            //stringSeparateAllToArray
            str = "123-11+22/(33*44)^900";
            array1 = new List<String>() { "123", "-", "11", "+", "22", "/", "", "(", "33", "*", "44", ")", "", "^", "900" };
            CollectionAssert.AreEqual(array1, normalizeString.stringSeparateAllToArray(str));
            //isPairedBracket
            str = "\"123 - 11 + 22 / (33 * 44) ^ 900\"";
            Assert.IsTrue(normalizeString.isPairedBracket(str));
            //isPairedBracket
            str = "123-11+22/(33*44^900";
            Assert.IsFalse(normalizeString.isPairedBracket(str));
            //isText
            str = "\"123 - 11 + 22 / (33 * 44) ^ 900\"";
            Assert.IsTrue(normalizeString.isText(str));
            //insideText
            str = "\"123 - 11 + 22 /\"(33*44)^900";
            Assert.IsTrue(normalizeString.insideText(str, 5));
            Assert.IsFalse(normalizeString.insideText(str, 18));
            //isPairedQuotes
            str = "\"123 - 11 + 22 / (33 * 44) ^ 900\"\"";
            Assert.IsFalse(normalizeString.isPairedQuotes(str));
        }

        [TestMethod]
        public void testDigitalFunc()
        {
            DigitalFunc df = new DigitalFunc();
            //for mixedstring
            str = "sin(13+2*3)";
            res = "0,149877209662952";
            Assert.AreEqual(res, df.returnMathResult(str).ToString());
            //for isMath
            str = "sin(13+2*3)";
            Assert.IsTrue(df.isMath(str));
            //for mathFunction
            str = "atn";
            Assert.IsTrue(df.mathFunction(str));
            //for isOnlyDigitWithMath
            str = "-123-345+678/329*0.32";
            Assert.IsTrue(df.isOnlyDigitsWithMath(str));
            //for isOnlyDigit
            str = "-1230.32";
            Assert.IsTrue(df.isOnlyDigits(str));
            str = "";
            //The method needes to be added to the class
            /*Assert.IsTrue(df.isOnlyDigitsAndNotEmpty(str));
            XCTAssertFalse([digitalFunc isOnlyDigitsAndNotEmpty: str]);*/

            //for toBinary
            /*res = "10011010010";
            Assert.AreEqual(res, df.ToBinary(1234));
            XCTAssertTrue([res isEqualToString:[digitalFunc toBinary: 1234]]);*/

            //for returnMathResult
            str = "123+90.209-32/347*746";
            res = "144,413610951009";
            Assert.AreEqual(res, df.returnMathResult(str).ToString());
            //str = "val(\" - 91.44\")";
            str = "val(\" - 91.44\")";
            res = "-91,44";
            Assert.AreEqual(res, df.returnMathResult(str).ToString());
            str = "asc(\"B4\")";
            res = "66";
            Assert.AreEqual(res, df.returnMathResult(str).ToString());
            str = "abs(-91.44)";
            res = "91,44";
            Assert.AreEqual(res, df.returnMathResult(str).ToString());
            str = "fix(-1.99)";
            res = "-1";
            Assert.AreEqual(res, df.returnMathResult(str).ToString());
            str = "instr((\"I like MCX!\",\"MCX\")";
            res = "8";
            Assert.AreEqual(res, df.returnMathResult(str).ToString());
            str = "len(\"MCX\")";
            res = "3";
            Assert.AreEqual(res, df.returnMathResult(str).ToString());
            str = "log(2)";
            res = "0,693147180559945";
            Assert.AreEqual(res, df.returnMathResult(str).ToString());
            str = "3^3";
            res = "27";
            Assert.AreEqual(res, df.returnMathResult(str).ToString());
            str = "sqr(9)";
            res = "3";
            Assert.AreEqual(res, df.returnMathResult(str).ToString());
            str = "atn(2)";
            res = "1,10714871779409";
            Assert.AreEqual(res, df.returnMathResult(str).ToString());
        }

        [TestMethod]
        public void testRunCommand()
        {
            RunCommand runCommand = new RunCommand();
            GlobalVars.getInstance().Error = "";

            string str = "ver";
            //Assert.IsTrue(runCommand.set(str));
            //Assert.AreEqual(4, GlobalVars.getInstance().ListOfStrings.Count);
            Assert.AreEqual(GlobalVars.getInstance().Error, "");
            
            str="auto";
            Assert.IsTrue(runCommand.set(str));
            Assert.IsFalse(GlobalVars.getInstance().IsOkSet);
            Assert.IsTrue(GlobalVars.getInstance().AutoSet);
            Assert.AreEqual(2, GlobalVars.getInstance().ListOfStrings.Count);
            Assert.IsTrue(GlobalVars.getInstance().Error == "");
            str="list";
            Assert.IsTrue(runCommand.set(str));
            Assert.IsTrue(GlobalVars.getInstance().IsOkSet);
            str="a=10";
            Assert.IsTrue(runCommand.set(str));
            Assert.IsTrue(GlobalVars.getInstance().Variables.Count > 0);
            str="clear";
            Assert.IsTrue(runCommand.set(str));
            Assert.AreEqual(0, GlobalVars.getInstance().Variables.Count);
            Assert.IsTrue(GlobalVars.getInstance().Error == ""); 
            str="10 cls";
            Assert.IsFalse(runCommand.set(str));
            str="20 ver";
            Assert.IsFalse(runCommand.set(str));
            str="30 print 10";
            Assert.IsFalse(runCommand.set(str));
            str="delete 10-20";
            Assert.IsTrue(runCommand.set(str));
            Assert.AreEqual(1, GlobalVars.getInstance().ListOfProgram.Count);
            str = "end";
            Assert.IsTrue(runCommand.set(str));
            Assert.IsTrue(GlobalVars.getInstance().IsOkSet);
            Assert.IsFalse(GlobalVars.getInstance().Run);
            
        }

        [TestMethod]
        public void testStringFunctions()
        {
            StringFunc stringFunc = new StringFunc();
            GlobalVars globals = new GlobalVars();
            string strTSF, resTSF;
            globals.Error = "";
            strTSF = "spc$(3)";
            resTSF = "\"   \"";

            //XCTAssertTrue([resTSF isEqual:[stringFunc returnStringResult:strTSF]],
            //"Strings are not equal %@ %", resTSF, [stringFunc returnStringResult:strTSF]);
            Assert.IsTrue(stringFunc.stringFunction(strTSF));
            strTSF="bin$(8)";
            resTSF="\"1000\"";
            Assert.AreEqual(resTSF, stringFunc.returnStringResult(strTSF));
            strTSF="chr$(77)";
            resTSF="\"M\"";
            Assert.AreEqual(resTSF, stringFunc.returnStringResult(strTSF));
            strTSF="str$(-3.791)";
            resTSF="\"-3,791\"";
            Assert.AreEqual(resTSF, stringFunc.returnStringResult(strTSF));
            strTSF="string$(8,65)";
            resTSF="\"AAAAAAAA\"";
            Assert.AreEqual(resTSF, stringFunc.returnStringResult(strTSF));
            strTSF="hex$(127)";
            resTSF="\"7F\"";
            Assert.AreEqual(resTSF, stringFunc.returnStringResult(strTSF));
            strTSF="left$(\"MCX Forever!\",3)";
            resTSF="\"MCX\"";
            Assert.AreEqual(resTSF, stringFunc.returnStringResult(strTSF));
            strTSF ="right$(\"MCX Forever!\",3)";
            resTSF="\"er!\"";
            Assert.AreEqual(resTSF, stringFunc.returnStringResult(strTSF));
            strTSF="mid$(\"www.msx.org\",5,3)";
            resTSF="\"msx\"";
            Assert.AreEqual(resTSF, stringFunc.returnStringResult(strTSF));
            strTSF ="mid$(\"www.msx.org\",5)";
            resTSF="\"msx.org\"";
            Assert.AreEqual(resTSF, stringFunc.returnStringResult(strTSF));
            strTSF ="oct$(127)";
            resTSF="\"177\"";
            Assert.AreEqual(resTSF, stringFunc.returnStringResult(strTSF));
        }
    }
}