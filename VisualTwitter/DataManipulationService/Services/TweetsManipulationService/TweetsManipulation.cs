using DataManipulationService.Interfaces;
using DataManipulationService.Models;
using System;
using System.Collections.Generic;

namespace TweetsManipulationService.Services.TweetsManipulationService
{
    public class TweetsManipulation : ITweetsManipulationService
    {
        //check for grammar mistakes
        //check if the given topic matches the tweet's hashtag(for search functionality)
        public List<Tweet> FilterDataForSearch(List<Tweet> tweets)
        {
            throw new NotImplementedException();
        }
        public List<Tweet> FilterDataForTrending(List<Tweet> tweets)
        {
            throw new NotImplementedException();
        }
    }
}
