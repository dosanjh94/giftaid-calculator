using System;

namespace JG.FinTechTest.Calculator
{
    /// <summary>
    /// Calculates GiftAid amount using donation and a using 20% tax rate. 
    /// </summary>
    public class GiftAidCalculator : IGiftAidCalculator
    {
        private const decimal TAX_RATE = 20m;

        /// <summary>
        /// Calculates GiftAid amount using donation and a using 20% tax rate. 
        /// </summary>
        /// <param name="donation">Positive dot=nation amount</param>
        /// <returns>GiftAid amount rounded down to 2 decimal places</returns>
        public decimal CalculateGiftAid(decimal donation)
        {
            if (donation < 0)
            {
                throw new ArgumentException("Donation amount must be positive");
            }

            var unroundedGiftAid = donation * (TAX_RATE / (100m - TAX_RATE));

            return RoundDownGiftAid(unroundedGiftAid);
        }

        private decimal RoundDownGiftAid(decimal amount)
        {
            return Math.Floor(amount * 100) / 100;
        }
    }
}
