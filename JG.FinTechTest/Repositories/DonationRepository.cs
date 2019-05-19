using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JG.FinTechTest.Models;
using LiteDB;

namespace JG.FinTechTest.Repositories
{
    public class DonationRepository : IDonationRepository, IDisposable
    {
        private readonly LiteDatabase _database;
        
        public DonationRepository(LiteDatabase database)
        {
            _database = database;
        }

        public GiftAidDonation RecordDonation(GiftAidDonation donation)
        {
            var donations = _database.GetCollection<GiftAidDonation>("donations");
            donations.Insert(donation);

            return donation;
         }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _database.Dispose();
                }
                
                disposedValue = true;
            }
        }
        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
