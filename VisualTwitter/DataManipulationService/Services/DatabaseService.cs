using DataManipulationService.Interfaces;
using DataManipulationService.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public void insertTweet(Tweet tweet)
        { 
            _database.GetCollection<Tweet>("Tweets").InsertOne(tweet);
        }

        public void insertWhitelistedTweets(List<Tweet> tweets)
        {
            _database.GetCollection<Tweet>("WhitelistedTweets").InsertMany(tweets);
        }
    }
}
