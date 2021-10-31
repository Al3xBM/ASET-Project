using System.Collections.Generic;
using UserService.Models;

namespace ClusteringComponent.Models
{
    public class Centroid
    {
        public List<TweetVector> GroupedTweet { get; set; }
    }
}
