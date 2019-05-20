using Microsoft.AspNetCore.Hosting;
using NUnit.Framework;
using System.Threading.Tasks;
using Microsoft.AspNetCore.TestHost;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using System.IO;
using LiteDB;
using JG.FinTechTest.Repositories;
using JG.FinTechTest.Models;

namespace JG.FinTechTest.Tests.Repositories
{
    public class DonationRepositoryTest
    {
        private MemoryStream _memoryStream;
        private LiteDatabase _db;
        private DonationRepository _donationRepository;

        [SetUp]
        public void Setup()
        {
            _memoryStream = new MemoryStream();
            _db = new LiteDatabase(_memoryStream);
            _donationRepository = new DonationRepository(_db);
        }

        [Test]
        public void InsertsDonationIntoDb()
        {
            var donation = new GiftAidDonation() {
                DonationAmount = 17.81m,
                Name = "TestName",
                PostCode = "TestPostCode"
            };
            var result = _donationRepository.RecordDonation(donation);

            var collectionExists = _db.CollectionExists("donations");
            Assert.IsTrue(collectionExists);

            var collection = _db.GetCollection<GiftAidDonation>("donations");
            var dbResult = collection.FindById(result.Id);

            Assert.AreEqual(donation.DonationAmount, dbResult.DonationAmount);
            Assert.AreEqual(donation.Name, dbResult.Name);
            Assert.AreEqual(donation.PostCode, dbResult.PostCode);
        }

        [TearDown]
        public void TearDown()
        {
            _memoryStream.Dispose();
            _db.Dispose();
        }
    }
}
