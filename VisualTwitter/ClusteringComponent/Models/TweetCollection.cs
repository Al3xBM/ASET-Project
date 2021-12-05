using ClusteringComponent.Models;
using System.Collections.Generic;

namespace UserService.Models
{
    public class TweetCollection
    {
        // represtnts all the tweets to be clustered
        public List<Tweet> TweetList { get; set; }

        public List<TweetVector> TweetVectors { get; set; }
    }
}