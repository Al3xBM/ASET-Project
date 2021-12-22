using MongoDB.Driver;

namespace DataManipulationService.Interfaces
{
    public interface IDatabaseConnection
    {
        public IMongoDatabase getDatabaseConnection(string databaseName);
    }
}
