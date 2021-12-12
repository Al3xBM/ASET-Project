using DataManipulationService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataManipulationService.Interfaces
{
    public interface IDatabaseService
    {
        public void insertTweet(Tweet tweet);

        public void insertWhitelistedTweets(List<Tweet> tweets);

        public void insertBasketballTweets(Tweet tweets);
    }
}
