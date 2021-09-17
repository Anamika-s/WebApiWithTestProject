using MongoDB.Driver;
using System.Linq;
using System.Threading.Tasks;
using UserService.Models;
namespace UserService.Repository
{
    //Inherit the respective interface and implement the methods in 
    // this class i.e UserRepository by inheriting IUserRepository class 
    //which is used to implement all methods in the classs
    public class UserRepository:IUserRepository
    {
        private UserContext uc;  //define a private variable to represent Reminder Database Context
        public UserRepository(UserContext userContext)
        {
            uc = userContext;
        }

        public async Task<bool> AddUser(UserProfile user)
        {
            uc.Users.InsertOne(user);
            var inserted = GetUser(user.UserId).Result;
            return await Task.FromResult(inserted != null);
        }

        public async Task<bool> DeleteUser(string userId)
        {
            var deleted = uc.Users.DeleteOne(x => x.UserId == userId);
            return await Task.FromResult(deleted.DeletedCount > 0);
        }

        public async Task<UserProfile> GetUser(string userId)
        {
            var u = uc.Users.Find(x => x.UserId == userId).FirstOrDefault();
            return await Task.FromResult(u);
        }

        public async Task<bool> UpdateUser(UserProfile user)
        {
            var f = Builders<UserProfile>.Filter.Where(x => x.UserId == user.UserId);
            var up = Builders<UserProfile>.Update.Set(x => x.Email, user.Email)
                                                    .Set(x => x.Contact, user.Contact)
                                                    .Set(x => x.FirstName, user.FirstName)
                                                    .Set(x => x.LastName, user.LastName)
                                                    .Set(x => x.CreatedAt, user.CreatedAt);
            return await Task.FromResult(uc.Users.UpdateOne(f, up).ModifiedCount > 0);
        }

        //Implement the methods of interface Asynchronously.

        // Implement AddUser method which should be used to add  a new user Profile.  

        // Implement DeleteUser method which should be used to delete an existing user by userId.


        // Implement GetUser method which should be used to get a user by userId.



        // Implement UpdateUser method which should be used to update an existing user by using
        // UserProfile details.
    }
}
