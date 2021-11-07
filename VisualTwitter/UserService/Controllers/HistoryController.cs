using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using UserService.Repositories;

namespace UserService.Controllers
{
    public class HistoryController : ControllerBase
    {
        private ISearchHistoryRepository _historyRepository;

        public HistoryController(ISearchHistoryRepository historyRepository)
        {

        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult GetSearchHistory([FromBody] object userDTO)
        {
            throw new NotImplementedException();
        }

        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult GetSearchResult(int id, [FromBody] object userDTO)
        {
            throw new NotImplementedException();
        }

        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult Delete(int id, [FromBody] object userDTO)
        {
            throw new NotImplementedException();
        }
    }
}
