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
#pragma warning disable CS0414 // The field 'TwitterConnection.instance' is assigned but its value is never used
        private static TwitterConnection instance = null;
#pragma warning restore CS0414 // The field 'TwitterConnection.instance' is assigned but its value is never used
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
