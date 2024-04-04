using MongoDB.Driver;
using ScriveAPI.Models;

namespace ScriveAPI.Data
{
    public class BlogContext
    {
        private readonly IMongoDatabase _database;
        public IMongoCollection<Blog> Blogs => _database.GetCollection<Blog>("blogs");

        public BlogContext(IConfiguration configuration)
        {
            var connectionString = configuration["MongoDbUrl"];
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase("Scrive");
        }
    }
}
