using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using UserService.Models;

namespace UserService.Data
{
    //public class DataContext : DbContext
    public class DataContext : IDataContext
    {
        /*public DataContext()
        {

        }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<SearchResult> SearchResults { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<SearchResult>().ToTable("SearchResults");
        }*/
        public IMongoDatabase getDatabaseConnection(string databaseName)
        {
            var client = new MongoClient("mongodb+srv://sebastian:parola@cluster0.55w40.mongodb.net/myFirstDatabase?retryWrites=true&w=majority");
            var database = client.GetDatabase(databaseName);
            return database;
        }
    }
}
