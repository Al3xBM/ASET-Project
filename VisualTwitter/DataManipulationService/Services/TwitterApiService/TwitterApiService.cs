using DataManipulationService.Interfaces;
using DataManipulationService.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace DataManipulationService.Services.TwitterApiService
{
    public class TwitterApiService : ITwitterApiService
    {
        private readonly ITwitterConnection _twitterConnection;
        public TwitterApiService(ITwitterConnection twitterConnection)
        {
            _twitterConnection = twitterConnection;
        }
        public async Task<List<Tweet>> GetTweetsSample()
        {
            throw new NotImplementedException();
        }
        public async Task<string> GetTrendingAsync(string id)
        {
            HttpClient client = _twitterConnection.GetTwitterClient();
            string url = $"1.1/trends/place.json?id={id}";
            HttpResponseMessage response = await client.GetAsync(url);
            return await response.Content.ReadAsStringAsync();
            
        }

        
    }
}
