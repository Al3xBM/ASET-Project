using DataManipulationService.Interfaces;
using MongoDB.Driver;

namespace DataManipulationService.Services
{
    public class DatabaseConnection : IDatabaseConnection
    {
        public IMongoDatabase getDatabaseConnection(string databaseName)
        {
            var client = new MongoClient("mongodb+srv://sebastian:parola@cluster0.55w40.mongodb.net/myFirstDatabase?retryWrites=true&w=majority");
            var database = client.GetDatabase(databaseName);
            return database;
        }

    }
}
