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

        public List<Tweet> GetBasketballTweets() => _database.GetCollection<Tweet>("BasketballSampleV3").Find(_ => true).Limit(3000).ToList();

        public List<Tweet> GetTweetSample() => _database.GetCollection<Tweet>("Tweets").Find(_ => true).Limit(3000).ToList();

        public List<Tweet> GetWhitelistedTweets() => _database.GetCollection<Tweet>("WhitelistedTweetsV4").Find(_ => true).ToList();

        public List<Player> GetPlayers() => _database.GetCollection<Player>("NbaPlayers").Find(_ => true).ToList();

        public List<Team> GetTeams() => _database.GetCollection<Team>("NbaTeams").Find(_ => true).ToList();
    }
}
