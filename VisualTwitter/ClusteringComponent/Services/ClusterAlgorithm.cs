using ClusteringComponent.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Models;

namespace ClusteringComponent.Services
{
    // K-Means Algorithm Implementation
    public class ClusterAlgorithm
    {
        public static List<Centroid> PrepareTweetCluster(int k, List<TweetVector> tweetCollection, ref int _counter)
        {
            /**
             * initialize the k means
             * use means to initialize clustering       => InitializeClusterCentroid()
             * loop at most maxIter times
             * use new clustering to update means       => FindClosestClusterCenter()
             *   use new means to update clustering     => CalculateMeanPoints()
             *   exit loop if no change to clustering or
             *   clustering would create an empty cluster
             * end-loop
             * return clustering
             */
            throw new NotImplementedException();
        }

        // Initializing cluster center
        private static void InitializeClusterCentroid(out List<Centroid> centroid, int count)
        {
            throw new NotImplementedException();
        }

        // Finding closest cluster center
        private static int FindClosestClusterCenter(List<Centroid> clusterCenter, TweetVector obj)
        {
            throw new NotImplementedException();
        }

        // Identifying the new position of the cluster center
        private static List<Centroid> CalculateMeanPoints(List<Centroid> _clusterCenter)
        {
            throw new NotImplementedException();
        }
    }
}
