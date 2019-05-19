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