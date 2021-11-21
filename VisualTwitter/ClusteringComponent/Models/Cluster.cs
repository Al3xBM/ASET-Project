using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Models;

namespace ClusteringComponent.Models
{
    public class Cluster
    {
        public List<TweetVector> GroupedTweets { get; set; }
    }
}
