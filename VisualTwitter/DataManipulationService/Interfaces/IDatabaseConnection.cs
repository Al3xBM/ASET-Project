using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataManipulationService.Interfaces
{
    public interface IDatabaseConnection
    {
        public IMongoDatabase getDatabaseConnection(string databaseName);
    }
}
