using ClusteringComponent.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using UserService.Models;

namespace ClusteringComponent.Services
{
    public class TweetsProcessing
    {
        //Calculates TF-IDF weight for each term t in tweet d
        // tf - idf(t,d) = tf(d,t) * idf(t)

        private readonly TweetCollection tweetCollection;

        public TweetsProcessing(TweetCollection collection)
        {
            tweetCollection = collection;
        }

        public static List<string> GetTweetWords(string _tweet)
        {
            string tweet = Regex.Replace(_tweet, @"[^\'\#\w\s]", " ");
            return tweet.ToLower().Split(" ").ToList();
        }

        public void CreateTweetVectors()
        {
            List<TweetVector> tweetVectorWords = new List<TweetVector>();
            Dictionary<string, double> totalFreq = new Dictionary<string, double>();

            foreach (Tweet tweet in tweetCollection.TweetList)
            {
                Dictionary<string, double> wordFreq = new();
                List<string> tweetWords = GetTweetWords(tweet.text);

                foreach (string word in tweetWords)
                {
                    if (word == "rt")
                        continue;

                    if (totalFreq.ContainsKey(word))
                    {
                        wordFreq[word] = totalFreq[word];
                        continue;
                    }

                    wordFreq[word] = ComputeTFIDF(tweetWords, word);

                    if(double.IsInfinity(wordFreq[word]))
                        wordFreq[word] = 0;

                    wordFreq[word] = Math.Round(wordFreq[word], 5);
                    totalFreq[word] = wordFreq[word];
                }

                tweetVectorWords.Add(new()
                {
                    Content = tweet.text,
                    VectorSpace = wordFreq
                });
            }

            tweetCollection.TweetVectors = tweetVectorWords;
        }

        public double ComputeTFIDF(List<string> tweetWords, string term)
        {
            return ComputeTermFrequency(tweetWords, term) * ComputeInverseDocumentFrequency(term);
        }

        // tf(t,d) = count of term in d / number of words in d
        public static double ComputeTermFrequency(List<string> tweetWords, string term)
        {
            term = term.ToLower();
            int count = tweetWords.Where(word => GetDamerauLevenshteinDistance(word.ToLower(), term) <= 3).Count();
                // word == term).Count();
            return (double)count / tweetWords.Count;
        }

        public static int GetDamerauLevenshteinDistance(string s, string t)
        {
            var bounds = new { Height = s.Length + 1, Width = t.Length + 1 };

            int[,] matrix = new int[bounds.Height, bounds.Width];

            for (int height = 0; height < bounds.Height; height++) { matrix[height, 0] = height; };
            for (int width = 0; width < bounds.Width; width++) { matrix[0, width] = width; };

            for (int height = 1; height < bounds.Height; height++)
            {
                for (int width = 1; width < bounds.Width; width++)
                {
                    int cost = (s[height - 1] == t[width - 1]) ? 0 : 1;
                    int insertion = matrix[height, width - 1] + 1;
                    int deletion = matrix[height - 1, width] + 1;
                    int substitution = matrix[height - 1, width - 1] + cost;

                    int distance = Math.Min(insertion, Math.Min(deletion, substitution));

                    if (height > 1 && width > 1 && s[height - 1] == t[width - 2] && s[height - 2] == t[width - 1])
                    {
                        distance = Math.Min(distance, matrix[height - 2, width - 2] + cost);
                    }

                    matrix[height, width] = distance;
                }
            }

            return matrix[bounds.Height - 1, bounds.Width - 1];
        }

        // idf(t) = log(N/(df + 1))
        // df(t) = occurrence of t in documents
        public double ComputeInverseDocumentFrequency(string term)
        {
            term = term.ToLower();
            int count = tweetCollection.TweetList.Where(x => x.text.ToLower().Contains(term)).Count();
            // tweetCollection.TweetList.Where(x => x.text.ToLower().Split(" ").Any(y => GetDamerauLevenshteinDistance(term, y.ToLower()) <= 3)).Count();

            return (double)Math.Log(tweetCollection.TweetList.Count / (double)count);
        }

        // Finding Similarity Score
        public static double ComputeCosineSimilarity(IDictionary<string, double> dictA, IDictionary<string, double> dictB)
        {
            /*
                cos(alpha) = (a * b) / (||a|| * ||b||)
                a = (a1, a2, ..., an);
                b = (b1, b2, ..., bn);

                K(X, Y) = <X, Y> / (||X||*||Y||)
            */

/*            IDictionary<string, double> copyDictA = dictA.ToDictionary(x => x.Key, x => x.Value);
            IDictionary<string, double> copyDictB = dictB.ToDictionary(x => x.Key, x => x.Value);

            AddMissingWords(copyDictA, copyDictB);
            AddMissingWords(copyDictB, copyDictA);
            var temp1 = copyDictA.OrderBy(x => x.Key).Select(x => x.Value);
            var temp2 = copyDictB.OrderBy(x => x.Key).Select(x => x.Value);*/

            double result = ComputeVectorsProduct(dictA.Values, dictB.Values) / (ComputeVectorNorm(dictA.Values) * ComputeVectorNorm(dictB.Values));
            return Math.Round(result + 0.000001, 5);
        }

        public static void AddMissingWords(IDictionary<string, double> dictA, IDictionary<string, double> dictB)
        {
            foreach(var key in dictB.Keys)
                if (!dictA.ContainsKey(key))
                    dictA.Add(key, 0);
        }

        public static double ComputeVectorNorm(IEnumerable<double> array)
        {
            return Math.Sqrt(array.Select(value => value * value).Sum());
        }

        public static double ComputeVectorsProduct(IEnumerable<double> vecA, IEnumerable<double> vecB)
        {
            // Debug.WriteLine("vecA = " + vecA.Count + "; vecB = " + vecB.Count);
            return vecA.Zip(vecB, (a, b) => a * b).Sum();

/*            double totalSum = 0;
            foreach(var a in vecA)
                foreach (var b in vecB)
                    totalSum += a * b;

            return totalSum;*/
        }
    }
}