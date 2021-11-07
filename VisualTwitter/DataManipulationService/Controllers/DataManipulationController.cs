using DataManipulationService.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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

        [HttpGet("trending/{region}")]
        public async Task<IActionResult> GetTrending(string region)
        {
            

            try
            {
                var availableTrendsResponse = await _twitterApiService.GetAvailableTrendsAsync();
                if(availableTrendsResponse.Contains("errors"))
                    return BadRequest(new { message = "Bad Request" });
                if (!availableTrendsResponse.Contains($"{region}"))
                    return NotFound(new { message = "No trends available for the specified region" });
                string woeid = "";

                dynamic availableTrends = JsonConvert.DeserializeObject(availableTrendsResponse);
                foreach(var trend in availableTrends)
                {
                    if (trend["country"] == region)
                    {
                        woeid = trend["woeid"];
                        break;
                    }
                }
                var trendsList = await _twitterApiService.GetTrendingAsync(woeid);
                if (trendsList.Contains("errors"))
                {
                    return BadRequest(new { message = " Bad Request on GetTrendingAsync" });
                }
                return Ok(trendsList);
            }
            catch (JsonException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{topic}")]
        public async Task<IActionResult> SearchTopic(string topic)
        {
            var responseBody = await _twitterApiService.GetTweetsSample();

            return null;
        }
    }
}
