using Microsoft.AspNetCore.Hosting;
using NUnit.Framework;
using System.Threading.Tasks;
using Microsoft.AspNetCore.TestHost;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net;

namespace JG.FinTechTest.Tests.Intergration
{
    public class GiftAidIntegrationTest
    {
        private TestServer server;
        private HttpClient client;

        [SetUp]
        public void Setup()
        {
            var b = new WebHostBuilder().UseStartup<Startup>();
            server = new TestServer(b);
            client = server.CreateClient();
        }

        [Test]
        [TestCase(100, 25)]
        public async Task GetGiftAidTest(decimal amount, decimal expected)
        {
            var respose = await client.GetAsync($"/api/giftaid?amount={amount}");
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
            var respose = await client.GetAsync($"/api/giftaid");

            Assert.AreEqual(HttpStatusCode.BadRequest, respose.StatusCode);
        }

        [TearDown]
        public void TareDown()
        {
            client.Dispose();
            server.Dispose();
        }
    }
}
