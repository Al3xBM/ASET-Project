using DataManipulationService.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace DataManipulationService.Services.TwitterApiService
{
    public sealed class TwitterConnection : ITwitterConnection
    {
        private readonly IConfiguration _configuration;
        private static TwitterConnection instance = null;
        private static readonly object padlock = new object();

        public TwitterConnection()
        {

        }

        public TwitterConnection(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        //public static TwitterConnection Instance
        //{
        //    get
        //    {
        //        lock(padlock)
        //        {
        //            if(instance==null)
        //            {
        //                instance = new TwitterConnection();
        //            }
        //            return instance;
        //        }
        //    }
        //}
        public HttpClient GetTwitterClient()
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri(_configuration.GetSection("Twitter")["BaseUrl"])
            };

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _configuration.GetSection("Twitter")["BearerToken"]);

            return client;
        }
    }
}
