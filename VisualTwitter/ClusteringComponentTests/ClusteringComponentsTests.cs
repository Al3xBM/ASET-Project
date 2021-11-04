using ClusteringComponent.Services;
using NUnit.Framework;
using System.Collections.Generic;
using UserService.Models;

namespace TestClusteringComp
{
    public class Tests
    {
        string tweet1, tweet2;
        List<string> tweets;
        TweetsProcessing _tweetsProcessing;

        TweetCollection collection;

        [SetUp]
        public void Setup()
        {
            tweet1 = "Happy #FanartFriday! Thanks to everyone who shared your spooky seasonal art " +
                "for our last #lolartprompts! Check out the thread below for some of our faves! " +
                "Next week we'll share a fresh new prompt for November!"; // 34

            tweet2 = "The #LeagueofLegends universe turned immersive experience. Roam the life-sized " +
                "streets of the Undercity Come face-to-face with iconic champions Take on secret " +
                "missions Unlock hidden rooms Witness never-before-seen storylines Your #Arcane adventure awaits";   //32

            tweets = new();
            tweets.Add(tweet1);
            tweets.Add(tweet2);

            collection = new TweetCollection();
            collection = collection.SetTweetList(tweets);

            _tweetsProcessing = new TweetsProcessing(collection);
        }
        /**
         * Testele au fost efectuate pe metode ajutatoare (private) din clasa TweetsProcessing
         **/


        [Test]
        public void GetTweetWords()
        {
            List<string> words = TweetsProcessing.GetTweetWords(tweet1);
            Assert.IsTrue(words.Count == 35);
        }

        [Test]
        public void ShowWord()
        {
            List<string> words = TweetsProcessing.GetTweetWords(tweet1);
            Assert.AreEqual(words[27], "we'll");
        }

        [Test]
        public void GetTermFreq_for()
        {
            double count = TweetsProcessing.ComputeTermFrequency(tweet1, "for");
            Assert.AreEqual(count, 0.0d);
        }

        [Test]
        public void GetInverseDocFreq_for()
        {
            double count = _tweetsProcessing.ComputeInverseDocumentFrequency("for");
            Assert.AreEqual(count, -0.40546506643295288d);
        }

    }
}