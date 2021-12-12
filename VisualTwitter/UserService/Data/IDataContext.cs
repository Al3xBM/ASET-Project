using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserService.Data
{
    public interface IDataContext
    {
        public IMongoDatabase getDatabaseConnection(string databaseName);
    }
}
