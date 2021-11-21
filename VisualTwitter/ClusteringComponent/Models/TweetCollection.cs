using ClusteringComponent.Models;
using System.Collections.Generic;

namespace UserService.Models
{
    public class TweetCollection
    {
        // represtnts all the tweets to be clustered
        private List<Tweet> _tweetList;
        private List<TweetVector> _tweetVectors;

        public void SetTweetList(List<Tweet> tweetList)
        {
            _tweetList = tweetList;
        }

        public void SetTweetVector(List<TweetVector> tweetVector)
        {
            _tweetVectors = tweetVector;
        }

        public List<TweetVector> GetTweetVectors()
        {
            return new List<TweetVector>(_tweetVectors);
        }

        public List<Tweet> GetTweetsContent()
        {
            return new List<Tweet>(_tweetList);
        }
    }
}