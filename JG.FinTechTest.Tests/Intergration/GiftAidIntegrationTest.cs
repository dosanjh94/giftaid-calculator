﻿using Microsoft.AspNetCore.Hosting;
using NUnit.Framework;
using System.Threading.Tasks;
using Microsoft.AspNetCore.TestHost;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using LiteDB;
using System.IO;
using JG.FinTechTest.Models;
using System.Linq;

namespace JG.FinTechTest.Tests.Integration
{
    public class GiftAidIntegrationTest
    {
        private TestServer _server;
        private HttpClient _client;
        private MemoryStream _memoryStream;
        private LiteDatabase _db;

        [SetUp]
        public void Setup()
        {
            _memoryStream = new MemoryStream();
            _db = new LiteDatabase(_memoryStream);

            var builder = new WebHostBuilder().UseStartup<Startup>();
            builder.ConfigureTestServices(services =>
            {
                services.AddTransient(_ => _db);
            });
            _server = new TestServer(builder);
            _client = _server.CreateClient();
        }

        [Test]
        [TestCase(100, 25)]
        public async Task GetGiftAidTest(decimal amount, decimal expected)
        {
            var respose = await _client.GetAsync($"/api/giftaid?amount={amount}");
            var body = await respose.Content.ReadAsStringAsync();

            var shape = new { donationAmount = 0m, giftAidAmount = 0m };
            var result = JsonConvert.DeserializeAnonymousType(body, shape);

            Assert.IsNotNull(body);
            Assert.AreEqual(expected, result.giftAidAmount);
            Assert.AreEqual(amount, result.donationAmount);
        }

        [Test]
        public async Task GetGiftAidNoAmountTest()
        {
            var respose = await _client.GetAsync($"/api/giftaid");

            Assert.AreEqual(HttpStatusCode.BadRequest, respose.StatusCode);
        }

        [Test]
        public async Task PostGiftAidDonationTest()
        {
            var donation = new {
                name = "FakeName",
                postCode = "POST CODE",
                donationAmount = 10,
            };
            var json = JsonConvert.SerializeObject(donation);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync($"/api/giftaid", content);

            var body = await response.Content.ReadAsStringAsync();
            var shape = new { Id = ""};
            var result = JsonConvert.DeserializeAnonymousType(body, shape);

            var collection = _db.GetCollection<GiftAidDonation>("donations");
            var dbResult = collection.Find(c => c.Id.ToString() == result.Id).Single();
            Assert.AreEqual(dbResult.Name, donation.name);
            Assert.AreEqual(dbResult.PostCode, donation.postCode);
            Assert.AreEqual(dbResult.DonationAmount, donation.donationAmount);
        }

        [TearDown]
        public void TareDown()
        {
            _client.Dispose();
            _server.Dispose();
            _memoryStream.Dispose();
            _db.Dispose();
        }
    }
}
