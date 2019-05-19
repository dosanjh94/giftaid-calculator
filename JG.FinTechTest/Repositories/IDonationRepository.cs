using JG.FinTechTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JG.FinTechTest.Repositories
{
    public interface IDonationRepository
    {
        GiftAidDonation RecordDonation(GiftAidDonation donation);
    }
}
