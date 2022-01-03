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
        private readonly int MAX_ITERATIONS = 1000;
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
                returnTweets.Add(Collection.TweetVectors.FirstOrDefault(x => x != null && x.Content == tweet.text));
            }

            return returnTweets;
                // Collection.TweetVectors.Where(x => returnTweets.Select(y => y.text == x.Content).Any()).ToList();
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
            List<(Tweet, TweetVector)> holdWhitelistCentroid = new List<(Tweet, TweetVector)>();

            foreach(var whitelistVector in whitelistVectors)
            {
                int tempIdx = Collection.TweetVectors.IndexOf(whitelistVector);

                if (tempIdx == -1)
                    break;

                holdWhitelistCentroid.Add((Collection.TweetList.ElementAt(tempIdx), Collection.TweetVectors.ElementAt(tempIdx)));
                Collection.TweetVectors.RemoveAt(tempIdx);
                Collection.TweetList.RemoveAt(tempIdx);
            }

            List<Team> teams = _databaseService.GetTeams();
            Team team1 = null, team2 = null;
            foreach(var word in topic.Split(" "))
            {
                if (team1 != null && team2 != null)
                    break;

                Team temp = teams.FirstOrDefault(x => x.Aliases.Contains(word));

                if(temp != null && team1 == null)
                    team1 = temp;

                if (temp != null && temp.Name != team1.Name && team2 == null)
                    team2 = temp;
            }

            List<string> topicWords = topic.Split(" ").ToList();
            if(team1 != null && team2 != null)
            {
                topicWords.AddRange(team1.Aliases);
                topicWords.AddRange(team2.Aliases);
            }
            topicWords = topicWords.Distinct().ToList();

            whitelistVectors.Insert(0, new TweetVector() { Content = topic, VectorSpace = topicWords.ToDictionary(x => x, x => 0.3 )});
            ++whitelistCount;

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

            // resultSet[0].GroupedTweets.AddRange(whitelistVectors);
            Collection.TweetList.AddRange(holdWhitelistCentroid.Select(x => x.Item1));
            Collection.TweetVectors.AddRange(holdWhitelistCentroid.Select(x => x.Item2));

            foreach(var res in resultSet)
            {
                if (res.Equals(resultSet[0]))
                    continue;

                foreach (var tweet in res.GroupedTweets)
                {
                    if (team1.Aliases.Any(x => tweet.Content.Contains(x) || team2.Aliases.Any(x => tweet.Content.Contains(x))))
                        resultSet[0].GroupedTweets.Add(tweet);

                    if (tweet.Content.Contains("dallasma"))
                        continue;
                }
            }

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
            // Func<IEnumerable<double>, IEnumerable<double>, double> computeSimilarityFunc = TweetsProcessing.ComputeCosineSimilarity;
            Func<IDictionary<string, double>, IDictionary<string, double>, double> computeSimilarityFunc = TweetsProcessing.ComputeCosineSimilarity;
            List<double> similarityMeasureList = clusterCenter
                .Select(centroid =>
                {
                    if (centroid.GroupedTweets.Count < tweetsInCentroid)
                        return 1;

                    var firstExceptSecond = centroid.GroupedTweets[0].VectorSpace.Values.Except(obj.VectorSpace.Values);
                    var secondExceptFirst = obj.VectorSpace.Values.Except(centroid.GroupedTweets[0].VectorSpace.Values);

                    if (!firstExceptSecond.Any() && !secondExceptFirst.Any())
                        return 0;

                    double max = 0;
                    for (int i = 0; i < tweetsInCentroid; ++i)
                        max = Math.Max(computeSimilarityFunc(centroid.GroupedTweets[i].VectorSpace, obj.VectorSpace), max);
                        // Math.Max(SimilarityCos(centroid.GroupedTweets[i].Content, obj.Content), max);


                    return max;
                }).ToList();

            return similarityMeasureList.IndexOf(similarityMeasureList.Min());
        }

        public static double SimilarityCos(string str1, string str2)
        {
            str1 = str1.Trim();
            str2 = str2.Trim();
            if (string.IsNullOrEmpty(str1) || string.IsNullOrEmpty(str2))
                return 0;

            List<string> lstr1 = SimpParticiple(str1);
            List<string> lstr2 = SimpParticiple(str2);
            //Find the union
            var strUnion = lstr1.Union(lstr2);
            //Find the vector
            List<int> int1 = new List<int>();
            List<int> int2 = new List<int>();
            foreach (var item in strUnion)
            {
                int1.Add(lstr1.Count(o => o == item));
                int2.Add(lstr2.Count(o => o == item));
            }

            double s = 0;
            double den1 = 0;
            double den2 = 0;
            for (int i = 0; i < int1.Count(); i++)
            {
                //Seeking Molecule
                s += int1[i] * int2[i];
                //Find the denominator (1)
                den1 += Math.Pow(int1[i], 2);
                //Find the denominator (2)
                den2 += Math.Pow(int2[i], 2);
            }

            return s / (Math.Sqrt(den1) * Math.Sqrt(den2));
        }

        public static List<string> SimpParticiple(string str)
        {
            List<string> vs = new List<string>();
            foreach (var item in str)
            {
                vs.Add(item.ToString());
            }
            return vs;
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
                    total = cluster.GroupedTweets.Select(vector => vector.VectorSpace.ContainsKey(word) ? vector.VectorSpace[word] : 0.0).Sum(); // .Count()

                    cluster.GroupedTweets[0].VectorSpace[word] = total / cluster.GroupedTweets.Select(x => x.VectorSpace.Values.Sum()).Sum();// .Count;
                }
            }

            return clusterCenter;
        }
    }
}
