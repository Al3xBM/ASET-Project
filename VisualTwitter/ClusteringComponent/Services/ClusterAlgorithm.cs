using ClusteringComponent.Interfaces;
using ClusteringComponent.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UserService.Models;

namespace ClusteringComponent.Services
{
    // K-Means Algorithm Implementation
    public class ClusterAlgorithm : IClusterAlgorithm
    {
        private readonly int MAX_ITERATIONS = 5000;
        private readonly int clusterCount = 5; 
        private readonly IDatabaseService _databaseService;
        public TweetCollection Collection { get; set; }

        public ClusterAlgorithm(IDatabaseService databaseService)
        {
            _databaseService = databaseService;

        }

        public void LoadCollection()
        {
            Collection = new TweetCollection();
            Collection.TweetList = _databaseService.GetBasketballTweets();
            Collection.TweetList.AddRange(_databaseService.GetWhitelistedTweets());

            TweetsProcessing tweetsProcessing = new TweetsProcessing(Collection);
            tweetsProcessing.CreateTweetVectors();
        }

        public List<TweetVector> FilterWhitelistedTweets(string topic)
        {
            List<Tweet> tweets = _databaseService.GetWhitelistedTweets();
            List<string> topicWords = topic.ToLower().Split(" ").ToList();
            Dictionary<Tweet, int> wordMatchCount = new Dictionary<Tweet, int>();

            foreach(Tweet tweet in tweets)
            {
                wordMatchCount.Add(tweet, 0);

                foreach(string word in topicWords)
                {
                    if (tweet.text.ToLower().Contains(word))
                      ++wordMatchCount[tweet];
                }
            }

            if (wordMatchCount.Values.Max() == 0)
                return new List<TweetVector>();

            List<Tweet> filteredTweets = new List<Tweet>();
            for(int i = 0; i < 3; ++i)
            {
                if (wordMatchCount.Values.Max() == 0)
                    break;

                filteredTweets.Add(wordMatchCount.Select(x => x.Key).Where(x => wordMatchCount[x] == wordMatchCount.Values.Max()).FirstOrDefault());
                wordMatchCount.Remove(filteredTweets[i]);
            }

            List<TweetVector> returnTweets = new List<TweetVector>();

            foreach(var tweet in filteredTweets)
            {
                returnTweets.Add(Collection.TweetVectors.FirstOrDefault(x => x.Content == tweet.text));
            }

            return returnTweets;// Collection.TweetVectors.Where(x => returnTweets.Select(y => y.text == x.Content).Any()).ToList();
        }

        public List<Cluster> PrepareTweetCluster(string topic)
        {
            List<Cluster> centroidCollection = new(clusterCount);   
            Cluster centroid;
            int iterationNumber = 0;

            HashSet<int> uniqRand = new HashSet<int>();
            Random rng = new Random();



            while (uniqRand.Count < clusterCount)
            {
                uniqRand.Add(rng.Next(0, Collection.TweetList.Count));
            }

            List<TweetVector> whitelistVectors = FilterWhitelistedTweets(topic);
            int whitelistCount = whitelistVectors.Count;

            centroidCollection = uniqRand.Select(i =>
            {
                centroid = new Cluster
                {
                    GroupedTweets = new List<TweetVector>()
                };
                for(int j = 0; j < whitelistVectors.Count; ++j)
                    centroid.GroupedTweets.Add(Collection.TweetVectors[i + j]);
                
                return centroid;
            }).ToList();

            centroidCollection[0].GroupedTweets = whitelistVectors;
            List<Cluster> whitelistCentroid = new List<Cluster>(centroidCollection);

            List<Cluster> resultSet = null;

            do
            {
                InitializeClusterCentroid(out resultSet, clusterCount);

                iterationNumber++;

                foreach (TweetVector tweet in Collection.TweetVectors)
                {
                    int index = FindClosestClusterCenter(centroidCollection, tweet, whitelistCount);
                    resultSet[index].GroupedTweets.Add(tweet);
                }

                InitializeClusterCentroid(out centroidCollection, clusterCount);

                centroidCollection = CalculateMeanPoints(resultSet);

                for(int i = 0; i < whitelistCount; ++i)
                    centroidCollection[0].GroupedTweets.Insert(0, whitelistCentroid[0].GroupedTweets[i]);


            } while (iterationNumber != MAX_ITERATIONS);

            resultSet[0].GroupedTweets.AddRange(whitelistVectors);

            return resultSet;
        }

        public void InitializeClusterCentroid(out List<Cluster> centroidList, int count)
        {
            centroidList = Enumerable.Range(0, count).Select(i =>
            {
                Cluster centroid = new();
                centroid.GroupedTweets = new List<TweetVector>();
                return centroid;
            }).ToList();
        }

        public int FindClosestClusterCenter(List<Cluster> clusterCenter, TweetVector obj, int tweetsInCentroid)
        {
            Func<IEnumerable<double>, IEnumerable<double>, double> computeSimilarityFunc = TweetsProcessing.ComputeCosineSimilarity;
            List<double> similarityMeasureList = clusterCenter
                .Select(centroid =>
                {
                    if (centroid.GroupedTweets.Count == 0)
                        return 0;

                    var firstExceptSecond = centroid.GroupedTweets[0].VectorSpace.Values.Except(obj.VectorSpace.Values);
                    var secondExceptFirst = obj.VectorSpace.Values.Except(centroid.GroupedTweets[0].VectorSpace.Values);

                    if (!firstExceptSecond.Any() && !secondExceptFirst.Any())
                        return 1;

                    double max = 0;
                    for (int i = 0; i < tweetsInCentroid; ++i)
                        max = Math.Max(computeSimilarityFunc(centroid.GroupedTweets[i].VectorSpace.Values, obj.VectorSpace.Values), max);

                    return max;
                }).ToList();

            return similarityMeasureList.IndexOf(similarityMeasureList.Max());
        }

        public List<Cluster> CalculateMeanPoints(List<Cluster> clusterCenter)
        {
            double total;

            foreach(var cluster in clusterCenter)
            {
                if (cluster.GroupedTweets.Count == 0)
                    continue;

                foreach (var word in cluster.GroupedTweets[0].VectorSpace.Keys)
                {
                    total = cluster.GroupedTweets.Select(vector => vector.VectorSpace.ContainsKey(word) ? vector.VectorSpace[word] : 0.0).Sum();

                    cluster.GroupedTweets[0].VectorSpace[word] = total / cluster.GroupedTweets.Count;
                }
            }

            return clusterCenter;
        }
    }
}
