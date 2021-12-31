using DataManipulationService.Models;
using System.Collections.Generic;

namespace DataManipulationService.Interfaces
{
    public interface IDatabaseService
    {
        public void InsertTweet(Tweet tweet);

        public void InsertWhitelistedTweets(List<Tweet> tweets);

        public void InsertBasketballTweets(Tweet tweets);

        public List<WhitelistedUser> GetWhitelistedUsers();
    }
}
