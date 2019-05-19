using JG.FinTechTest.Calculator;
using JG.FinTechTest.Controllers;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;
using System;

namespace JG.FinTechTest.Tests.Controllers
{
    public class GiftAidControllerTest
    {
        [Test]
        public void CalledWithNullAmount()
        {
            var calculator = Substitute.For<IGiftAidCalculator>();
            var controller = new GiftAidController(calculator);
            var result = controller.Get(null);

            var badRequestResult = result as BadRequestResult;

            Assert.IsNotNull(badRequestResult);
            calculator.DidNotReceive();
        }

        [Test]
        public void CalledWithAmount()
        {
            var calculator = Substitute.For<IGiftAidCalculator>();

            var amount = 100;
            var giftAidAmount = 25;

            calculator.CalculateGiftAid(amount).Returns(giftAidAmount);
            var controller = new GiftAidController(calculator);
            var result = controller.Get(amount);

            var okResult = result as OkObjectResult;

            Assert.IsNotNull(okResult);
            calculator.Received().CalculateGiftAid(amount);
        }

        [Test]
        [TestCase(100000.01)]
        [TestCase(1.99)]
        [TestCase(10000000)]
        [TestCase(-5)]
        public void CalledWithInvalidAmount(decimal amount)
        {
            var calculator = Substitute.For<IGiftAidCalculator>();
            var controller = new GiftAidController(calculator);
            var result = controller.Get(amount);

            var badRequestResult = result as BadRequestObjectResult;

            Assert.IsNotNull(badRequestResult);
            Assert.AreSame("The input must be more the 2 but less that 100000", badRequestResult.Value);
            calculator.DidNotReceive();
        }
    }
}