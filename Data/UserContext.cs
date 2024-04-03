using MongoDB.Driver;
using ScriveAPI.Models;

namespace ScriveAPI.Data
{
    public class UserContext
    {
        private readonly IMongoDatabase _database;
        public IMongoCollection<User> Users => _database.GetCollection<User>("users");

        public UserContext(IConfiguration configuration)
        {
            var connectionString = configuration["MongoDbUrl"];
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase("Scrive");
        }
    }
}