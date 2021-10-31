using System.Collections.Generic;

namespace UserService.Models
{
    public class TweetCollection
    {
        // represtnts all the tweets to be clustered
        private List<string> _tweetList;
        private List<TweetVector> _tweetVectors;

        public TweetCollection SetTweetList(List<string> tweetList)
        {
            _tweetList = tweetList;
            return this;
        }

        public TweetCollection SetTweetVector(List<TweetVector> tweetVector)
        {
            _tweetVectors = tweetVector;
            return this;
        }

        public List<TweetVector> GetTweetVectors()
        {
            return new List<TweetVector>(_tweetVectors);
        }

        public List<string> GetTweetsContent()
        {
            return new List<string>(_tweetList);
        }
    }
}