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
        private static readonly double EPSILON = 0.00001;
        private static readonly int MAX_ITERATIONS = 10;
        /**
         * Step 1: select k
         * Step 2: randomly select k distinct data points 
         * Step 3: measure the distance between the first tweet vector
            and the k initial clusters 
         * Step 4: assign the first point to the nearest cluster. And so on..
         * Step 5: calculate the mean of each cluster 
         
         * Stop: repet steps until the cluster centroids don't change their position*/

        public static List<Cluster> PrepareTweetCluster(int k, List<TweetVector> tweetCollection)
        {
            /*
             * **** STEP 1 ****
             * 
             * prepares k initial centroid and assign one object randomly to each centroid
             */
            List<Cluster> centroidCollection = new(k);     // k clusters
            Cluster centroid;
            int iterationNumber = 0;


            /*
             * **** STEP 2 ****
             * 
             * Avoid repeation of random number => HashSet collection
             */
            int[] _randomNumbers = Enumerable.Range(1, k).OrderBy(g => Guid.NewGuid()).Take(tweetCollection.Count).ToArray();
            HashSet<int> uniqRand = new(_randomNumbers);

            // in each cluster, the centroid will be on first position in GroupedTweets list
            centroidCollection = uniqRand.Select(i =>
            {
                centroid = new Cluster
                {
                    GroupedTweets = new List<TweetVector>()
                };
                centroid.GroupedTweets.Add(tweetCollection[i]);
                return centroid;
            }).ToList();

            bool stoppingCriteria = true;
            List<Cluster> prevClusterCendroid;

            InitializeClusterCentroid(out List<Cluster> resultSet, centroidCollection.Count);

            do
            {
                iterationNumber++;

                prevClusterCendroid = centroidCollection;

                /*
                 * **** STEP 3 & STEP 4 ****
                 */
                foreach (TweetVector tweet in tweetCollection)
                {
                    int index = FindClosestClusterCenter(centroidCollection, tweet);
                    resultSet[index].GroupedTweets.Add(tweet);
                }

                InitializeClusterCentroid(out centroidCollection, centroidCollection.Count);

                /*
                 * **** STEP 5 ****
                 */
                centroidCollection = CalculateMeanPoints(resultSet); 
                
                stoppingCriteria = CheckStoppingCriteria(prevClusterCendroid, centroidCollection, iterationNumber);
                if (!stoppingCriteria)
                {
                    // initialize the result set for next iteration
                    InitializeClusterCentroid(out resultSet, centroidCollection.Count);
                }
            } while (stoppingCriteria == false);

            return resultSet;
        }

        /*
         * The algorithm will stops if and only if a number of iterations reached up a maximum
         * or if and only if current cluster centroids have the same positions as previous cluster centroids
         * at a certain iteration.
         */
        private static bool CheckStoppingCriteria(List<Cluster> previous, List<Cluster> current, int iteration)
        {
            // TODO
            return iteration == MAX_ITERATIONS;
        }

        /*
         * Initializing cluster center
         */
        private static void InitializeClusterCentroid(out List<Cluster> centroidList, int count)
        {
            centroidList = Enumerable.Range(0, count).Select(i =>
            {
                Cluster centroid = new();
                centroid.GroupedTweets = new List<TweetVector>();
                return centroid;
            }).ToList();
        }

        /*
         * Finding closest cluster center
         */
        private static int FindClosestClusterCenter(List<Cluster> clusterCenter, TweetVector obj)
        {
            Func<double[], double[], double> computeSimilarityFunc = TweetsProcessing.ComputeCosineSimilarity;
            List<double> similarityMeasureList = clusterCenter
                .Select(centroid => computeSimilarityFunc(centroid.GroupedTweets[0].VectorSpace.Values.ToArray(), 
                    obj.VectorSpace.Values.ToArray()))
                .ToList();

            return clusterCenter.IndexOf(clusterCenter.Max());
        }

        /*
         * Identifying the new position of the cluster center
         * 
         * The center is always found at first position of GroupedTweets collecion
         */
        private static List<Cluster> CalculateMeanPoints(List<Cluster> clusterCenter)
        {
            string key;
            double total;
            int j;

            // iterate through each cluster centroids
            clusterCenter = clusterCenter.Select(centroid => {
                if (centroid.GroupedTweets.Count > 0)
                {
                    // loop through values of first tweet vector space
                    for (j = 0; j < centroid.GroupedTweets[0].VectorSpace.Count; j++)
                    {
                        // compute sum over column j values of each tweet vector space
                        total = centroid.GroupedTweets.Select(vector => vector.VectorSpace.ElementAt(j).Value).Sum();

                        // identify vector space key of first tweet vector in this cluster
                        key = centroid.GroupedTweets[0].VectorSpace.Keys.ElementAt(j);

                        // compute mean and update vector space values (key position = j)
                        centroid.GroupedTweets[0].VectorSpace[key] = total / centroid.GroupedTweets.Count;
                    }
                }
                return centroid;
            }).ToList();
            return clusterCenter;
        }
    }
}
