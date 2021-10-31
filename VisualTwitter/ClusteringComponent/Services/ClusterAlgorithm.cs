using ClusteringComponent.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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

            //prepares k initial centroid and assign one object randomly to each centroid
            List<Centroid> centroidCollection = new();
            Centroid centroid;

            /*
             * Avoid repeation of random number, if same no is generated 
             * more than once same document is added to the next cluster 
             * so avoid it using HasSet collection
             */
            HashSet<int> uniqRand = new HashSet<int>();
            //GenerateRandomNumber(ref uniqRand, k, documentCollection.Count);

            foreach (int pos in uniqRand)
            {
                centroid = new Centroid
                {
                    GroupedTweets = new List<TweetVector>()
                };
                centroid.GroupedTweets.Add(tweetCollection[pos]);
                centroidCollection.Add(centroid);
            }

            bool stoppingCriteria = true;
            List<Centroid> resultSet;
            List<Centroid> prevClusterCenter;

            InitializeClusterCentroid(out resultSet, centroidCollection.Count);

            do
            {
                prevClusterCenter = centroidCollection;

                // TODO

            } while (stoppingCriteria == false);


            return resultSet;
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
