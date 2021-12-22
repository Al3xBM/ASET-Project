using DataManipulationService.Models;
using System.Collections.Generic;

namespace DataManipulationService.Interfaces
{
    public interface IDatabaseService
    {
        public void insertTweet(Tweet tweet);

        public void insertWhitelistedTweets(List<Tweet> tweets);

        public void insertBasketballTweets(Tweet tweets);
    }
}
