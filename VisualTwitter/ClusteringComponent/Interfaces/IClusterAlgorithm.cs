using ClusteringComponent.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Models;

namespace ClusteringComponent.Interfaces
{
    public interface IClusterAlgorithm
    {
        public TweetCollection Collection { get; set; }

        public List<Cluster> PrepareTweetCluster(string topic);

        public void InitializeClusterCentroid(out List<Cluster> centroidList, int count);

        public int FindClosestClusterCenter(List<Cluster> clusterCenter, TweetVector obj, int tweetsInCentroid);

        public List<Cluster> CalculateMeanPoints(List<Cluster> clusterCenter);

        public void LoadCollection();
    }
}
