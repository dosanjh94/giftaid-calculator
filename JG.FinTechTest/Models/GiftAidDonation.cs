using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JG.FinTechTest.Models
{
    public class GiftAidDonation
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string PostCode { get; set; }
        public decimal DonationAmount { get; set; }
    }
}
