using DataManipulationService.Controllers;
using DataManipulationService.Interfaces;
using DataManipulationService.Models;
using DataManipulationService.Services.TwitterApiService;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataManipulationServiceTests
{
    public class DataManipulationServiceTests
    {
        public Mock<ITwitterApiService> _twitterApiServiceMoq;
        public List<Dictionary<string, string>> validWoeid;
        public string validWoeidResponse;
        public string invalidWoeid;
        public string invalidWoeidResponse;
        public List<Tweet> whitelistResponse;
        public string canceledEarlyResponse;
        public string endedSuccessfulyResponse;

        [SetUp]
        public void Setup()
        {
            _twitterApiServiceMoq = new Mock<ITwitterApiService>();
            validWoeid = new List<Dictionary<string, string>>() { new Dictionary<string, string>(){ { "country", "test" }, {"woeid", "4118" } } };
            validWoeidResponse = "{tweet1,tweet2,tweet3}";
            invalidWoeid = "2";
            invalidWoeidResponse = "{errors:error1,error2}";
            whitelistResponse = new List<Tweet> { new Tweet(), new Tweet()};
            canceledEarlyResponse = "Stopped early";
            endedSuccessfulyResponse = "";
        }

        [Test]
        public async Task Return_Ok_When_WOEID_Is_Valid()
        {
            _twitterApiServiceMoq.Setup(moq => moq.GetAvailableTrendsAsync()).ReturnsAsync(JsonConvert.SerializeObject(validWoeid));
            _twitterApiServiceMoq.Setup(moq => moq.GetTrendingAsync("4118")).ReturnsAsync(validWoeidResponse);

            var controller = new DataManipulationController(_twitterApiServiceMoq.Object);
            var response = await controller.GetTrending("test");

            Assert.IsInstanceOf<OkObjectResult>(response);
        }

        [Test]
        public async Task Return_NotFound_When_NoTrendsForRegion()
        {
            _twitterApiServiceMoq.Setup(moq => moq.GetAvailableTrendsAsync()).ReturnsAsync("");

            var controller = new DataManipulationController(_twitterApiServiceMoq.Object);
            var response = await controller.GetTrending("test");

            Assert.IsInstanceOf<NotFoundObjectResult>(response);
        }

        [Test]
        public async Task Return_BadRequest_When_WOEID_Is_Not_Valid()
        {
            _twitterApiServiceMoq.Setup(moq => moq.GetTrendingAsync(invalidWoeid)).ReturnsAsync(invalidWoeidResponse);

            var controller = new DataManipulationController(_twitterApiServiceMoq.Object);
            var response = await controller.GetTrending(invalidWoeid);

            Assert.IsInstanceOf<BadRequestObjectResult>(response);
        }

        [Test]
        public async Task WhitelistedUserTweets_ReturnOk_When_PulledTweets()
        {
            _twitterApiServiceMoq.Setup(moq => moq.SearchWhitelistedUsersTweets()).ReturnsAsync(whitelistResponse);

            var controller = new DataManipulationController(_twitterApiServiceMoq.Object);
            var response = await controller.SearchWhitelistedUsersTweets();

            Assert.IsInstanceOf<OkObjectResult>(response);
        }

        [Test]
        public async Task WhitelistedUserTweets_ReturnBadRequest_When_DidNotPullTweets()
        {
            _twitterApiServiceMoq.Setup(moq => moq.SearchWhitelistedUsersTweets()).ReturnsAsync(new List<Tweet>());

            var controller = new DataManipulationController(_twitterApiServiceMoq.Object);
            var response = await controller.SearchWhitelistedUsersTweets();

            Assert.IsInstanceOf<BadRequestObjectResult>(response);
        }

        [Test]
        public async Task GetTweetSample_Return206_ManuallyCanceled()
        {
            _twitterApiServiceMoq.Setup(moq => moq.GetTweetsSample()).ReturnsAsync(canceledEarlyResponse);

            var controller = new DataManipulationController(_twitterApiServiceMoq.Object);
            var response = await controller.SearchTopic();

            Assert.IsInstanceOf<StatusCodeResult>(response);
        }

        [Test]
        public async Task GetTweetSample_ReturnOk_When_AddedSetNumberOfTweets()
        {
            _twitterApiServiceMoq.Setup(moq => moq.GetTweetsSample()).ReturnsAsync(canceledEarlyResponse);

            var controller = new DataManipulationController(_twitterApiServiceMoq.Object);
            var response = await controller.SearchTopic();

            Assert.IsInstanceOf<StatusCodeResult>(response);
        }
    }
}