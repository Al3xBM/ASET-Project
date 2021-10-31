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
        private readonly ITweetsManipulationService _tweetsManipulationService;
        private readonly ITwitterConnection _twitterConnection;
        public DataManipulationController(ITweetsManipulationService tweetsManipulationService, ITwitterConnection twitterConnection)
        {
            _tweetsManipulationService = tweetsManipulationService;
            _twitterConnection = twitterConnection;
        }

        [HttpGet]
        public async Task<IActionResult> GetTrendingAsync()
        {
            HttpClient client = _twitterConnection.GetTwitterClient();
            string url = $"1.1/statuses/sample.json";
            HttpResponseMessage response = await client.GetAsync(url);
            var responseBody = await response.Content.ReadAsStringAsync();

            try
            {
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
