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
            var _randomNumbers = Enumerable.Range(1, k).OrderBy(g => Guid.NewGuid()).Take(tweetCollection.Count).ToArray();
            HashSet<int> uniqRand = new(_randomNumbers);

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
            List<Centroid> prevClusterCenter;

            InitializeClusterCentroid(out List<Centroid> resultSet, centroidCollection.Count);

            do
            {
                prevClusterCenter = centroidCollection;

                // TODO

            } while (stoppingCriteria == false);

            return resultSet;
        }

        // Initializing cluster center
        private static void InitializeClusterCentroid(out List<Centroid> centroidList, int count)
        {
            Centroid centroid;
            centroidList = new List<Centroid>();
            for (int i = 0; i < count; i++)
            {
                centroid = new Centroid();
                centroid.GroupedTweets = new List<TweetVector>();
                centroidList.Add(centroid);
            }
        }

        // Finding closest cluster center
        private static int FindClosestClusterCenter(List<Centroid> clusterCenter, TweetVector obj)
        {
            Func<double[], double[], double> computeSimilarityFunc = TweetsProcessing.ComputeCosineSimilarity;
            List<double> similarityMeasureList = clusterCenter
                .Select(c => computeSimilarityFunc(c.GroupedTweets[0].VectorSpace.Values.ToArray(), 
                    obj.VectorSpace.Values.ToArray()))
                .ToList();

            return clusterCenter.IndexOf(clusterCenter.Max());
        }

        // Identifying the new position of the cluster center
        private static List<Centroid> CalculateMeanPoints(List<Centroid> clusterCenter)
        {
            for (int i = 0; i < clusterCenter.Count; i++)
            {
                if (clusterCenter[i].GroupedTweets.Count > 0)
                {
                    for (int j = 0; j < clusterCenter[i].GroupedTweets[0].VectorSpace.Count; j++)
                    {
                        double total = 0;

                        total = clusterCenter[i].GroupedTweets.Select(vector => vector.VectorSpace.Values.ToList()[j]).Sum();
                        //reassign new calculated mean on each cluster center,
                        //It indicates the reposition of centroid


                        // in clusterCenter[i] la cheia J in GroupedTweets[0].VectorSpace adaugam media


                        //clusterCenter[i].GroupedTweets[0].VectorSpace.Keys.ToArray()[j] = 
                        //                    total / clusterCenter[i].GroupedTweets.Count;
                    }
                }
            }
            return clusterCenter;
        }
    }
}
