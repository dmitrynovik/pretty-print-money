using NUnit.Framework;
using System;

namespace PrettyPrintMoney.UnitTest
{
    [TestFixture]
    public class MoneyFormatterTest
    {
        [Test]
        public void When_Negative_Then_Error()
        {
            Assert.Throws<ArgumentException>(() => (-1.0).FormatMoney());
        }

        [Test]
        public void When_TooLarge_Then_Error()
        {
            Assert.Throws<ArgumentException>(() => (2000000000.01).FormatMoney());
        }

        [Test]
        public void When_0_Returns_Zero_Dollars()
        {
            var x = 0.0.FormatMoney();
            Assert.AreEqual("zero dollars", x);
        }

        [Test]
        public void When_1000000_Returns_One_Million_Dollars()
        {
            var x = 1000000.0.FormatMoney();
            Assert.AreEqual("one million dollars", x);
        }

        [Test]
        public void When_7000000_Returns_Seven_Million_Dollars()
        {
            var x = 7000000.0.FormatMoney();
            Assert.AreEqual("seven million dollars", x);
        }

        [Test]
        public void When_7_Returns_Seven_Dollars()
        {
            var x = 7.0.FormatMoney();
            Assert.AreEqual("seven dollars", x);
        }

        [Test]
        public void When_0_64_Returns_Seven_Dollars_and_Sity_Four_Cents()
        {
            var x = 0.64.FormatMoney();
            Assert.AreEqual("sixty four cents", x);
        }

        [Test]
        public void When_0_07_Returns_Seven_Cents()
        {
            var x = 0.07.FormatMoney();
            Assert.AreEqual("seven cents", x);
        }

        [Test]
        public void When_7_64_Returns_Seven_Dollars_and_Sity_Four_Cents()
        {
            var x = 7.64.FormatMoney();
            Assert.AreEqual("seven dollars and sixty four cents", x);
        }

        [Test]
        public void When_137_64_Returns_Correct()
        {
            var x = 137.64.FormatMoney();
            Assert.AreEqual("one hundred and thirty seven dollars and sixty four cents", x);
        }

        [Test]
        public void When_1357256_And_32_Returns_Correct()
        {
            var x = 1357256.32.FormatMoney();
            Assert.AreEqual(
                "one million, three hundred and fifty seven thousand, two hundred and fifty six dollars and thirty two cents", 
                x);
        }

    }
}
