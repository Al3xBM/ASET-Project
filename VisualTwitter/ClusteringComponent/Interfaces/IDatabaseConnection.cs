using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClusteringComponent.Interfaces
{
    public interface IDatabaseConnection
    {
        public IMongoDatabase getDatabaseConnection(string databaseName);
    }
}
