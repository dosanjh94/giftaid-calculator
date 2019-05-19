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
    }
}