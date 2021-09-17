using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
namespace UserService.Models
{
    public class UserContext
    {
        MongoClient mc;
        IMongoDatabase _db; //declare variables to connect to MongoDB database

        public UserContext(IConfiguration configuration)
        {
            var s = configuration.GetSection("MongoDB:ConnectionString").Value;
            var db = configuration.GetSection("MongoDB:UserDatabase").Value;
            mc = new MongoClient(s);
            _db = mc.GetDatabase(db);   //Initialize MongoClient and Database using connection string and database name from configuration
        }
        public IMongoCollection<UserProfile> Users => _db.GetCollection<UserProfile>("Users");
        //Define a MongoCollection to represent the Users collection of MongoDB based on UserProfile type
    }
}
