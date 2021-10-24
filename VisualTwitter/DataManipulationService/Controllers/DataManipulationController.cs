using DataManipulationService.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataManipulationService.Controllers
{
    [ApiController]
    [Route("DataManipulation")]
    public class DataManipulationController : ControllerBase
    {
        private readonly ITweetsManipulationService _tweetsManipulationService;
        public DataManipulationController(ITweetsManipulationService tweetsManipulationService)
        {
            _tweetsManipulationService = tweetsManipulationService;

        }

        [HttpGet]
        public IActionResult GetTrending()
        {
            throw new NotImplementedException();
        }
        [HttpGet("{topic}")]
        public IActionResult SearchTopic(string topic)
        {
            throw new NotImplementedException();
        }
    }
}
