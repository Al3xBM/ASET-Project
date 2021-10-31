using DataManipulationService.Controllers;
using DataManipulationService.Interfaces;
using DataManipulationService.Services.TwitterApiService;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace DataManipulationServiceTests
{
    public class DataManipulationServiceTests
    {
        public Mock<ITwitterApiService> _twitterApiServiceMoq;
        [SetUp]
        public void Setup()
        {
            
        }

        [Test]
        public async Task Return_Ok_When_WOEID_Is_Valid()
        {
            string woeid = "4118";
            var fakeResponse = "{tweet1,tweet2,tweet3}";
            _twitterApiServiceMoq = new Mock<ITwitterApiService>();
            _twitterApiServiceMoq.Setup(moq => moq.GetTrendingAsync(woeid)).ReturnsAsync(fakeResponse);
            var controller = new DataManipulationController(_twitterApiServiceMoq.Object);
            var response = await controller.GetTrending(woeid);
            Assert.IsInstanceOf<OkObjectResult>(response);
        }

        [Test]
        public async Task Return_BadRequest_When_WOEID_Is_Not_Valid()
        {
            string woeid = "2";
            var fakeResponse = "{errors:error1,error2}";
            _twitterApiServiceMoq = new Mock<ITwitterApiService>();
            _twitterApiServiceMoq.Setup(moq => moq.GetTrendingAsync(woeid)).ReturnsAsync(fakeResponse);
            var controller = new DataManipulationController(_twitterApiServiceMoq.Object);
            var response = await controller.GetTrending(woeid);
            Assert.IsInstanceOf<BadRequestObjectResult>(response);
        }
    }
}