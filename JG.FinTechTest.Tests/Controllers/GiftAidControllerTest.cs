using JG.FinTechTest.Calculator;
using JG.FinTechTest.Controllers;
using JG.FinTechTest.Repositories;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;
using LiteDB;
using System.IO;
using JG.FinTechTest.Models;
using System;

namespace JG.FinTechTest.Tests.Controllers
{
    public class GiftAidControllerTest
    {
        private IGiftAidCalculator _calculator;
        private IDonationRepository _donationRepository;
        private GiftAidController _controller;

        [SetUp]
        public void Setup()
        {
            _calculator = Substitute.For<IGiftAidCalculator>();
            _donationRepository = Substitute.For<IDonationRepository>();
            _controller = new GiftAidController(_calculator, _donationRepository);
        }

        [Test]
        public void CalledGetWithNullAmount()
        {
            var result = _controller.Get(null);

            var badRequestResult = result as BadRequestResult;

            Assert.IsNotNull(badRequestResult);
            _calculator.DidNotReceive();
        }

        [Test]
        public void CalledGetWithAmount()
        {
            var amount = 100;
            var giftAidAmount = 25;

            _calculator.CalculateGiftAid(amount).Returns(giftAidAmount);

            var result = _controller.Get(amount);

            var okResult = result as OkObjectResult;

            Assert.IsNotNull(okResult);
            _calculator.Received().CalculateGiftAid(amount);
        }

        [Test]
        [TestCase(100000.01)]
        [TestCase(1.99)]
        [TestCase(10000000)]
        [TestCase(-5)]
        public void CalledGetWithInvalidAmount(decimal amount)
        {
            var result = _controller.Get(amount);

            var badRequestResult = result as BadRequestObjectResult;

            Assert.IsNotNull(badRequestResult);
            Assert.AreSame("The input must be more the 2 but less that 100000", badRequestResult.Value);
            _calculator.DidNotReceive();
        }

        [Test]
        public void CalledPostWithValidData()
        {
            var donation = new GiftAidDonation()
            {
                DonationAmount = 10,
                Name = "name",
                PostCode = "postCode"
            };

            _donationRepository.RecordDonation(donation).Returns(new GiftAidDonation()
            {
                Id = Guid.NewGuid(),
            });

            var result = _controller.Post(donation);

            var createdRequestResult = result as CreatedResult;
            Assert.IsNotNull(createdRequestResult);
            _donationRepository.Received().RecordDonation(donation);
        }

        [Test]
        [TestCase(0, "name", "postCode")]
        [TestCase(10, null, "postCode")]
        [TestCase(10, "name", null)]
        [TestCase(10, "name", "")]
        [TestCase(10, "", "postCode")]
        public void CalledPostWithInvalidData(decimal donationAmount, string name, string postCode)
        {
            var donation = new GiftAidDonation()
            {
                DonationAmount = donationAmount,
                Name = name,
                PostCode = postCode
            };

            var result = _controller.Post(donation);

            var badRequestResult = result as BadRequestResult;
            Assert.IsNotNull(badRequestResult);
        }
    }
}