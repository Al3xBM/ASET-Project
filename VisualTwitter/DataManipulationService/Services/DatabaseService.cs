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
        public void insertTweet(Tweet tweet) => _database.GetCollection<Tweet>("Tweets").InsertOne(tweet);

        public void insertWhitelistedTweets(List<Tweet> tweets)
        {
            IMongoCollection<Tweet> collection = _database.GetCollection<Tweet>("WhitelistedTweets");
            foreach (var tweet in tweets)
            {
                if (!collection.Find(x => x.id == tweet.id).Any())
                {
                    collection.InsertOne(tweet);
                }
            }
            //_database.GetCollection<Tweet>("WhitelistedTweets").InsertMany(tweets);
        }

        public void insertBasketballTweets(Tweet tweet) => _database.GetCollection<Tweet>("BasketballSample").InsertOne(tweet);// .InsertMany(tweets);

    }
}
