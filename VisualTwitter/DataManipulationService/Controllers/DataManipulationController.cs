using DataManipulationService.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace DataManipulationService.Controllers
{
    [ApiController]
    [Route("DataManipulation")]
    public class DataManipulationController : ControllerBase
    {
        //private readonly ITweetsManipulationService _tweetsManipulationService;
        private readonly ITwitterApiService _twitterApiService;
        public DataManipulationController(ITwitterApiService twitterApiService)
        {
            //_tweetsManipulationService = tweetsManipulationService;
            _twitterApiService = twitterApiService;
        }

        [HttpGet("trending/{woeid}")]
        public async Task<IActionResult> GetTrending(string woeid)
        {
            var responseBody = await _twitterApiService.GetTrendingAsync(woeid);

            try
            {
                if(responseBody.Contains("errors"))
                    return BadRequest(new { message = "Bad Request" });

                return Ok(responseBody);
            }
            catch (JsonException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{topic}")]
        public IActionResult SearchTopic(string topic)
        {
            throw new NotImplementedException();
        }
    }
}
