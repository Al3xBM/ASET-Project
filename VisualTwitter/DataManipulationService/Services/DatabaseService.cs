using DataManipulationService.Interfaces;
using DataManipulationService.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

namespace DataManipulationService.Services
{
    public class DatabaseService : IDatabaseService
    {
        private protected IDatabaseConnection _databaseConnection;
        private protected IMongoDatabase _database;
        public DatabaseService(IDatabaseConnection databaseConnection)
        {
            _databaseConnection = databaseConnection;
            _database = databaseConnection.getDatabaseConnection("VisualTwitter");
        }
        public void InsertTweet(Tweet tweet) => _database.GetCollection<Tweet>("Tweets").InsertOne(tweet);

        public void InsertWhitelistedTweets(List<Tweet> tweets)
        {
            IMongoCollection<Tweet> collection = _database.GetCollection<Tweet>("WhitelistedTweetsV3");
            foreach (var tweet in tweets)
            {
                if (!collection.Find(x => x.id == tweet.id).Any())
                {
                    collection.InsertOne(tweet);
                }
            }
            //_database.GetCollection<Tweet>("WhitelistedTweets").InsertMany(tweets);
        }

        public void InsertBasketballTweets(Tweet tweet)
        {
            IMongoCollection<Tweet> collection = _database.GetCollection<Tweet>("BasketballSampleV3");

            if (!collection.Find(x => x.id == tweet.id || x.text.Equals(tweet.text)).Any())
            {
                collection.InsertOne(tweet);
            }
        }// .InsertMany(tweets);

        public List<WhitelistedUser> GetWhitelistedUsers() => _database.GetCollection<WhitelistedUser>("WhitelistedUsers").Find(_ => true).ToList();
    }
}
