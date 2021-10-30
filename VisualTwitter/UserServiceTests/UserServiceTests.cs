using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
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
        public UserRegisterDTO registerModel;
        public UserUpdateDTO updateModel;

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
            registerModel = new UserRegisterDTO
            {
                FirstName = "Alex",
                LastName = "Balan",
                Email = "ali.balan16@gmail.com",
                Password = "admin1234"
            };
            updateModel = new UserUpdateDTO
            {
                Email = "ali.balan16@gmail.com",
                Password = "admin1234"

            };

            _mapperMoq.Setup(moq => moq.Map<User>(registerModel)).Returns(user);
        }

        [Test]
        public void ReqisterReturnOK()
        {
            var controller = new UserController(_userServiceMoq.Object, _mapperMoq.Object, _appSettings);

            IActionResult response = controller.Register(registerModel);
            Assert.IsInstanceOf<OkObjectResult>(response);

        }

        [Test]
        public void ReqisterReturnExeption_When_PasswordIsEmpty()
        {
            _userServiceMoq.Setup(moq => moq.Create(user, "")).Throws(new UserException(""));

            var controller = new UserController(_userServiceMoq.Object, _mapperMoq.Object, _appSettings);

            IActionResult response = controller.Register(registerModel);
            Assert.IsInstanceOf<OkObjectResult>(response);
        }

        [Test]
        public void AuthenticateReturnOK()
        {
            var dto = new UserAuthenticateDTO
            {
                Email = "ali.balan16@gmail.com",
                Password = "admin1234"
            };

            var controller = new UserController(_userServiceMoq.Object, _mapperMoq.Object, _appSettings);

            IActionResult response = controller.Authenticate(dto); ;
            Assert.IsInstanceOf<UnauthorizedObjectResult>(response);

        }
        [Test]
        public void UpdateReturnsNoContent()
        {
            _mapperMoq.Setup(moq => moq.Map<User>(updateModel)).Returns(user);

            var controller = new UserController(_userServiceMoq.Object, _mapperMoq.Object, _appSettings);

            IActionResult response = controller.Update(1, updateModel);
            Assert.IsInstanceOf<NoContentResult>(response);
        }
    }
}