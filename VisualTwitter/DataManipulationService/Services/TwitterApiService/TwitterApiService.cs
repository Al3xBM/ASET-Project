using DataManipulationService.Interfaces;
using DataManipulationService.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataManipulationService.Services.TwitterApiService
{
    public class TwitterApiService
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
    }
}
