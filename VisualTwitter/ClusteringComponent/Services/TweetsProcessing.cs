﻿using ClusteringComponent.Models;
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

        public void SetTweetCollection()
        {
            List<TweetVector> tweetVectorWords = new();
            foreach (Tweet tweet in tweetCollection.GetTweetsContent())
            {
                Dictionary<string, double> wordFreq = new();
                foreach (string word in GetTweetWords(tweet.Content))
                {
                    wordFreq[word] = ComputeTFIDF(tweet.Content, word);
                }

                tweetVectorWords.Add(new()
                {
                    Content = tweet.Content,
                    VectorSpace = wordFreq
                });
            }

            tweetCollection.SetTweetVector(tweetVectorWords);
        }

        public float ComputeTFIDF(string tweet, string term)
        {
            return ComputeTermFrequency(tweet, term) * ComputeInverseDocumentFrequency(term);
        }

        // tf(t,d) = count of term in d / number of words in d
        public static float ComputeTermFrequency(string tweet, string term)
        {
            List<string> words = GetTweetWords(tweet);
            int count = words.FindAll(word => word == term).Count;
            return count / words.Count;
        }

        // idf(t) = log(N/(df + 1))
        // df(t) = occurrence of t in documents
        public float ComputeInverseDocumentFrequency(string term)
        {
            int count = 0;
            foreach (Tweet tweet in tweetCollection.GetTweetsContent())
            {
                count += GetTweetWords(tweet.Content).FindAll(word => word.ToLower() == term.ToLower()).Count;
            }

            return (float)Math.Log(tweetCollection.GetTweetsContent().Count / (float)count);
        }

        // Finding Similarity Score
        public static double ComputeCosineSimilarity(List<double> vecA, List<double> vecB)
        {
            /*
                cos(alpha) = (a * b) / (||a|| * ||b||)
                a = (a1, a2, ..., an);
                b = (b1, b2, ..., bn);

                K(X, Y) = <X, Y> / (||X||*||Y||)
            */


            double result = ComputeVectorsProduct(vecA, vecB) / (ComputeVectorNorm(vecA) * ComputeVectorNorm(vecB));
            return result + double.MinValue;
        }

        public static double ComputeVectorNorm(List<double> array)
        {
            return Math.Sqrt(array.Select(value => value * value).Sum());
        }

        public static double ComputeVectorsProduct(List<double> vecA, List<double> vecB)
        {
            Debug.WriteLine("vecA = " + vecA.Count + "; vecB = " + vecB.Count);
            return vecA.Zip(vecB, (a, b) => a * b).ToArray().Sum();
        }
    }
}