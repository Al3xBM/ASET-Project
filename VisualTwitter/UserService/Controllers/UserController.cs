using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using UserService.Repositories;

namespace UserService.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/users")]
    public class UserController : ControllerBase
    {
        private IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {

        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody] object userDTO)
        {
            throw new NotImplementedException();
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] object userDTO)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult GetAll()
        {
            throw new NotImplementedException();
        }

        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult GetById(int id)
        {
            throw new NotImplementedException(); ;
        }

        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult Update(int id, [FromBody] object model)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
