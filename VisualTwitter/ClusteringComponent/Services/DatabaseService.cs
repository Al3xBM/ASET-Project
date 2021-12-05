using ClusteringComponent.Interfaces;
using ClusteringComponent.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClusteringComponent.Services
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

        public List<Tweet> GetTweetSample()
        {
            return _database.GetCollection<Tweet>("Tweets").Find(_ => true).Limit(1000).ToList();
        }
        
        public List<Tweet> GetWhitelistedTweets()
        {
            return _database.GetCollection<Tweet>("WhitelistedTweets").Find(_ => true).ToList();
        }
    }
}
