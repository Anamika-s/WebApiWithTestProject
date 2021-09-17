using System.Threading.Tasks;
using UserService.Exceptions;
using UserService.Models;
using UserService.Repository;
namespace UserService.Services
{
    //Inherit the respective interface and implement the methods in 
    // this class i.e UserService by inheriting IUserService
    public class UserService:IUserService
    {
        private IUserRepository ur;  /*
         * UserRepository should  be injected through constructor injection. 
         * Please note that we should not create USerRepository object using the new keyword
         */
        
        public UserService(IUserRepository userRepository)
        {
            ur = userRepository;
        }

        public async Task<bool> AddUser(UserProfile user)
        {
            var u = ur.GetUser(user.UserId).Result;
            if (u != null)
            {
                throw new UserAlreadyExistsException($"{user.UserId} is already in use");
            }
            return await ur.AddUser(user);
        }

        public async Task<bool> DeleteUser(string userId)
        {
            var u = ur.GetUser(userId).Result;
            if (u == null)
            {
                throw new UserNotFoundException($"This user id doesn't exist");
            }
            return await ur.DeleteUser(userId);
        }
    

        public async Task<UserProfile> GetUser(string userId)
        {
           var r = ur.GetUser(userId);
           if (r.Result == null)
           {
             throw new UserNotFoundException($"This user id doesn't exist");
           }
         return await r;
        }

        public async Task<bool> UpdateUser(string userId, UserProfile user)
        {
            var r = ur.UpdateUser(user);
            if (!r.Result)
            {
                throw new UserNotFoundException($"This user id doesn't exist");
            }
            return await r;
        }
        //Implement the methods of interface Asynchronously.

        // Implement AddUser method which should be used to add  a new user Profile.  

        // Implement DeleteUser method which should be used to delete an existing user by userId.


        // Implement GetUser method which should be used to get a user by userId.

        // Implement UpdateUser method which should be used to update an existing user by using
        // UserProfile details.
    }
}
