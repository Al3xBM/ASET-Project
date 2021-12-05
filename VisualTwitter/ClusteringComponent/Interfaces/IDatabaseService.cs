using ClusteringComponent.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClusteringComponent.Interfaces
{
    public interface IDatabaseService
    {
        public List<Tweet> GetTweetSample();

        public List<Tweet> GetWhitelistedTweets();
    }
}
