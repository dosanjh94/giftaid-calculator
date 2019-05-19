using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JG.FinTechTest.Calculator;
using JG.FinTechTest.Models;
using Microsoft.AspNetCore.Mvc;

namespace JG.FinTechTest.Controllers
{
    [Route("api/giftaid")]
    [ApiController]
    public class GiftAidController : ControllerBase
    {
        private readonly IGiftAidCalculator _calculator;

        public GiftAidController(IGiftAidCalculator calculator)
        {
            _calculator = calculator;
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

            var response = new GiftAidResponse {
                DonationAmount= amount.Value,
                GiftAidAmount = _calculator.CalculateGiftAid(amount.Value)
            };
            
            return Ok(response);
        }
    }
}