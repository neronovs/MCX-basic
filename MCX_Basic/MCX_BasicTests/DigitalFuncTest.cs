using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MCX_Basic;

namespace MCX_BasicTests
{
    [TestClass]
    public class DigitalFuncTest
    {
        DigitalFunc df = new DigitalFunc();
        string str = "", res = "";

        [TestMethod]
        public void TestMethod1()
        {
            //for isMath
            Assert.IsTrue(df.isMath("abs"));
            //for mathFunction
            str = @"atn";
            Assert.IsTrue(df.mathFunction(str));
            //for isOnlyDigitWithMath
            str = @"-123-345+678/329*0.32";
            Assert.IsTrue(df.isOnlyDigitsWithMath(str));
            //for isOnlyDigit
            str = @"-1230.32";
            Assert.IsTrue(df.isOnlyDigits(str));
            str = @"";
            //The method needes to be added to the class
            /*Assert.IsTrue(df.isOnlyDigitsAndNotEmpty(str));
            XCTAssertFalse([digitalFunc isOnlyDigitsAndNotEmpty: str]);*/

            //for toBinary
            /*res = @"10011010010";
            Assert.AreEqual(res, df.ToBinary(1234));
            XCTAssertTrue([res isEqualToString:[digitalFunc toBinary: 1234]]);*/

            //for returnMathResult
            str = @"123+90.209-32/347*746";
            res = "144,413610951009";
            Assert.AreEqual(res, df.returnMathResult(str).ToString());
            str = "val(\" - 91.44\")";
            res = @"-91,44";
            //XCTAssertTrue([res isEqualToString:[digitalFunc returnMathResult: str].stringValue]);
            Assert.AreEqual(res, df.returnMathResult(str).ToString());

        }
    }
}
