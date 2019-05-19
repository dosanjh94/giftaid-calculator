using JG.FinTechTest.Calculator;
using NUnit.Framework;
using System;

namespace JG.FinTechTest.Tests
{
    public class GiftAidControllerTest
    {
        [Test]
        [TestCase(0,0)]
        [TestCase(100,25)]
        [TestCase(100.01,25)]
        [TestCase(171.98,42.99)]
        public void GiftAidCalculationTest(decimal donation, decimal expected)
        {
            var calculator = new GiftAidCalculator();
            var result = calculator.CalculateGiftAid(donation);
            Assert.AreEqual(expected, result);
        }

        [Test]
        [TestCase(-1)]
        [TestCase(-50)]
        [TestCase(-0.5)]
        public void GiftAidNegativeAmountCalculationTest(decimal donation)
        {
            var calculator = new GiftAidCalculator();
            Assert.Throws<ArgumentException>(() => calculator.CalculateGiftAid(donation));
        }
    }
}