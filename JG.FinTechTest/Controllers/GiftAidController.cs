using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JG.FinTechTest.Calculator;
using JG.FinTechTest.Models;
using JG.FinTechTest.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace JG.FinTechTest.Controllers
{
    [Route("api/giftaid")]
    [ApiController]
    public class GiftAidController : ControllerBase
    {
        private readonly IGiftAidCalculator _calculator;
        private readonly IDonationRepository _donationRepositroy;

        public GiftAidController(IGiftAidCalculator calculator, IDonationRepository donationRepository)
        {
            _calculator = calculator;
            _donationRepositroy = donationRepository;
        }

        /// <summary>
        /// Get the amount of gift aid reclaimable for donation amount
        /// </summary>
        /// <param name="amount">The value of the donation made</param>
        /// <returns>The amount of Gift Aid for the donation</returns>
        /// <response code="200">Returns the Git Aid amount</response>
        /// <response code="400">If no or invalid amount is recieved</response>  
        [HttpGet]
        public IActionResult Get([FromQuery] decimal? amount)
        {
            if (!amount.HasValue)
            {
                return BadRequest();
            }
            if(amount < 2 || amount > 100000)
            {
                return BadRequest("The input must be more the 2 but less that 100000");
            }

            var response = new GiftAidResponse {
                DonationAmount= amount.Value,
                GiftAidAmount = _calculator.CalculateGiftAid(amount.Value)
            };
            
            return Ok(response);
        }

        /// <summary>
        /// Post a donation eligible for a gift aid reclaim
        /// </summary>
        /// <param name="donation"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Post([FromBody] GiftAidDonation donation)
        {
            if (donation.DonationAmount < 2 || donation.DonationAmount > 100000)
            {
                return BadRequest();
            }

            if (string.IsNullOrWhiteSpace(donation.Name))
            {
                return BadRequest();
            }

            if (string.IsNullOrWhiteSpace(donation.PostCode))
            {
                return BadRequest();
            }

            var result =_donationRepositroy.RecordDonation(donation);
            return Created(result.Id.ToString(), result);
        }
    }
}