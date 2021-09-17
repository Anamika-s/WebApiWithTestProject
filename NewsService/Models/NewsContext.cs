using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace NewsService.Models
{
    public class NewsContext
    {
        MongoClient mc; //declare variables to connect to MongoDB database
        IMongoDatabase _db;

        public NewsContext(IConfiguration configuration)
        {
            var server = configuration.GetSection("MongoDb:ConnectionString").Value;
            var db = configuration.GetSection("MongoDb:NewsDatabase").Value;
            mc = new MongoClient(server);
            _db = mc.GetDatabase(db); //Initialize MongoClient and Database using connection string and database name from configuration
        }
        public IMongoCollection<UserNews> News => _db.GetCollection<UserNews>("UserNews");
        //Define a MongoCollection to represent the News collection of MongoDB based on UserNews type
        
    }
}
