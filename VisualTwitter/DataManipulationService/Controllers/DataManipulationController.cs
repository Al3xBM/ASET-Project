using DataManipulationService.Interfaces;
using DataManipulationService.MOP;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
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
        [TwitterApiServiceMonitor]
        public async Task<IActionResult> GetTrending(string region)
        {
            try
            {
                var availableTrendsResponse = await _twitterApiService.GetAvailableTrendsAsync();

                if (availableTrendsResponse == null || availableTrendsResponse.Contains("errors"))
                    return BadRequest(new { message = "Bad Request" });

                if (!availableTrendsResponse.Contains($"{region}"))
                    return NotFound(new { message = "No trends available for the specified region" });

                string woeid = "";
                dynamic availableTrends = JsonConvert.DeserializeObject(availableTrendsResponse);

                foreach (var trend in availableTrends)
                {
                    if (trend["country"] == region)
                    {
                        woeid = trend["woeid"];
                        break;
                    }
                }

                var trendsList = await _twitterApiService.GetTrendingAsync(woeid);

                if (trendsList.Contains("errors"))
                    return BadRequest(new { message = " Bad Request on GetTrendingAsync" });

                return Ok(trendsList);
            }
            catch (JsonException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{topic}")]
        public async Task<IActionResult> SearchTopic()
        {
            string responseBody = await _twitterApiService.GetTweetsSample();

            if (responseBody.Contains("early"))
                return StatusCode(206);

            return Ok(responseBody);
        }

        [HttpGet("whitelisted")]
        public async Task<IActionResult> SearchWhitelistedUsersTweets()
        {
            var responseBody = await _twitterApiService.SearchWhitelistedUsersTweets();

            if (responseBody != null && responseBody.Any())
                return Ok(responseBody);

            return BadRequest(new { message = "No tweets found." });
        }
    }
}
