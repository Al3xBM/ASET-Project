using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace UserService.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/users")]
    public class UserController : ControllerBase
    {
        private IUserService _userService;

        public UserController(IUserService userService)
        {

        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody] object dto)
        {
            throw new NotImplementedException();
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] object dto)
        {
            throw new NotImplementedException();
        }
    }
}
