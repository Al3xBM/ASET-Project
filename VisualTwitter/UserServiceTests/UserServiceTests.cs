using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using NSubstitute;
using NUnit.Framework;
using RestSharp;
using UserService.Controllers;
using UserService.DataTransferObjects;
using UserService.Helpers;
using UserService.Models;
using UserService.Repositories;

namespace UserServiceTests
{
    public class Tests
    {
        public Mock<IUserRepository> _userServiceMoq;
        public Mock<IMapper> _mapperMoq;
        public IOptions<AppSettings> _appSettings;
        public User user; 
        public User invalidUser;
        public UserRegisterDTO registerDTO;
        public UserRegisterDTO registerDTONoPass;
        public UserUpdateDTO updateDTO;
        public UserAuthenticateDTO authenticateDTO;

        [SetUp]
        public void Setup()
        {
            _userServiceMoq = new Mock<IUserRepository>();
            _mapperMoq = new Mock<IMapper>();

            _appSettings = Options.Create(new AppSettings
            {
                Secret = "Hai la magazin"
            });


            user = new User
            {
                Id = 1,
                FirstName = "Alex",
                LastName = "Balan",
                Email = "ali.balan16@gmail.com",
                PasswordHash = null,
                PasswordSalt = null
            };
            invalidUser = new User
            {
                Id = 1,
                FirstName = "Alex",
                LastName = "Balan",
                Email = "ali.balan16@gmail.com",
                PasswordHash = null,
                PasswordSalt = null
            };
            registerDTO = new UserRegisterDTO
            {
                FirstName = "Alex",
                LastName = "Balan",
                Email = "ali.balan16@gmail.com",
                Password = "admin1234"
            };
            registerDTONoPass = new UserRegisterDTO
            {
                FirstName = "Alex",
                LastName = "Balan",
                Email = "ali.balan16@gmail.com",
                Password = ""
            };
            updateDTO = new UserUpdateDTO
            {
                Email = "ali.balan16@gmail.com",
                Password = "admin1234"

            }; 
            authenticateDTO = new UserAuthenticateDTO
            {
                Email = "ali.balan16@gmail.com",
                Password = "admin1234"
            };
        }

        [Test]
        public void RegisterReturnOK()
        {
            var controller = new UserController(_userServiceMoq.Object, _mapperMoq.Object);

            IActionResult response = controller.Register(registerDTO);
            Assert.IsInstanceOf<OkObjectResult>(response);

        }

        [Test]
        public void RegisterReturnExeption_When_PasswordIsEmpty()
        {
            _mapperMoq.Setup(moq => moq.Map<User>(registerDTONoPass)).Returns(user);
            _userServiceMoq.Setup(moq => moq.Create(user, "")).Throws(new UserException(""));

            var controller = new UserController(_userServiceMoq.Object, _mapperMoq.Object);

            IActionResult response = controller.Register(registerDTONoPass);
            Assert.IsInstanceOf<BadRequestObjectResult>(response);
        }

        [Test]
        public void Authenticate_ReturnUnauthorized()
        {
            var controller = new UserController(_userServiceMoq.Object, _mapperMoq.Object);

            IActionResult response = controller.Authenticate(authenticateDTO); ;
            Assert.IsInstanceOf<UnauthorizedObjectResult>(response);

        }
        [Test]
        public void Update_ReturnsNoContent()
        {
            _mapperMoq.Setup(moq => moq.Map<User>(updateDTO)).Returns(user);

            var controller = new UserController(_userServiceMoq.Object, _mapperMoq.Object);

            IActionResult response = controller.Update(1, updateDTO);
            Assert.IsInstanceOf<NoContentResult>(response);
        }

        [Test]
        public void Authenticate_ReturnOk()
        {
            _userServiceMoq.Setup(moq => moq.Authenticate(authenticateDTO.Email, authenticateDTO.Password)).Returns(user);
            _userServiceMoq.Setup(moq => moq.GetJWTToken(user)).Returns("token");

            var mockHttpContext = Substitute.For<HttpContext>();
            var controller = new UserController(_userServiceMoq.Object, _mapperMoq.Object);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = mockHttpContext
            };

            IActionResult response = controller.Authenticate(authenticateDTO);
            Assert.IsInstanceOf<OkObjectResult>(response);
        }
    }
}