using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
namespace ReminderService.Models
{
    public class ReminderContext
    {
        MongoClient mc;
        IMongoDatabase _db;
        //declare variables to connect to MongoDB database
        public ReminderContext(IConfiguration configuration)
        {
            var s = configuration.GetSection("MongoDB:ConnectionString").Value;
            var db = configuration.GetSection("MongoDB:ReminderDatabase").Value;
            mc = new MongoClient(s);
            _db = mc.GetDatabase(db);
            //Initialize MongoClient and Database using connection string and database name from configuration
        }
        public IMongoCollection<Reminder> Reminders => _db.GetCollection<Reminder>("Reminders");
        //Define a MongoCollection to represent the Reminders collection of MongoDB based on Reminder type
    }
}