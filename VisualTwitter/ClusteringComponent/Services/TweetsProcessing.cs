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
            string tweet = Regex.Replace(_tweet, @"[^\'\#\w\s]", "");
            return tweet.ToLower().Split(" ").ToList();
        }

        public void CreateTweetVectors()
        {
            List<TweetVector> tweetVectorWords = new();
            foreach (Tweet tweet in tweetCollection.TweetList)
            {
                Dictionary<string, double> wordFreq = new();
                List<string> tweetWords = GetTweetWords(tweet.text);

                foreach (string word in tweetWords)
                {
                    if (word == "rt")
                        continue;

                    wordFreq[word] = ComputeTFIDF(tweetWords, word);
                    if(double.IsInfinity(wordFreq[word]))
                        wordFreq[word] = 0;

                    wordFreq[word] = Math.Round(wordFreq[word], 4);
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
            int count = tweetWords.Where(word => word == term).Count();
            return (double)count / tweetWords.Count;
        }

        // idf(t) = log(N/(df + 1))
        // df(t) = occurrence of t in documents
        public double ComputeInverseDocumentFrequency(string term)
        {
            int count = 0;
            foreach (Tweet tweet in tweetCollection.TweetList)
            {
                if(tweet.text.ToLower().Contains(term))
                    ++count;
                 // GetTweetWords(tweet.text).FindAll(word => word.ToLower() == term.ToLower()).Count;
            }

            return (double)Math.Log(tweetCollection.TweetList.Count / (double)count);
        }

        // Finding Similarity Score
        public static double ComputeCosineSimilarity(IEnumerable<double> vecA, IEnumerable<double> vecB)
        {
            /*
                cos(alpha) = (a * b) / (||a|| * ||b||)
                a = (a1, a2, ..., an);
                b = (b1, b2, ..., bn);

                K(X, Y) = <X, Y> / (||X||*||Y||)
            */


            double result = ComputeVectorsProduct(vecA, vecB) / (ComputeVectorNorm(vecA) * ComputeVectorNorm(vecB));
            return result + 0.000001;
        }

        public static double ComputeVectorNorm(IEnumerable<double> array)
        {
            return Math.Sqrt(array.Select(value => value * value).Sum());
        }

        public static double ComputeVectorsProduct(IEnumerable<double> vecA, IEnumerable<double> vecB)
        {
            // Debug.WriteLine("vecA = " + vecA.Count + "; vecB = " + vecB.Count);
            return vecA.Zip(vecB, (a, b) => a * b).Sum();
        }
    }
}