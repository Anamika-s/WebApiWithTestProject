using ReminderService.Exceptions;
using ReminderService.Models;
using ReminderService.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace ReminderService.Services
{
    public class ReminderService:IReminderService
    {
        private IReminderRepository rp; //Inherit the respective interface and implement the methods in 
        // this class i.e ReminderService by inheriting IReminderService

        /*
      * ReminderRepository should  be injected through constructor injection. 
      * Please note that we should not create ReminderRepository object using the new keyword
      */
        public ReminderService(IReminderRepository reminderRepository)
        {
            rp = reminderRepository;
        }

        public async Task<bool> CreateReminder(string userId, string email, ReminderSchedule schedule)
        {
            var rem = rp.IsReminderExists(userId, schedule.NewsId).Result;
            if (rem)
            {
                throw new ReminderAlreadyExistsException($"This News already have a reminder");
            }
            await rp.CreateReminder(userId, email, schedule);
            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteReminder(string userId, int newsId)
        {
            var r = rp.DeleteReminder(userId, newsId);
            if (!r.Result)
            {
                throw new NoReminderFoundException("No reminder found for this news");
            }
            return await r;
        }

        public async Task<List<ReminderSchedule>> GetReminders(string userId)
        {
            var r = rp.GetReminders(userId);
            if (r.Result == null)
            {
                throw new NoReminderFoundException("No reminders found for this user");
            }
            return await r;
        }

        public async Task<bool> UpdateReminder(string userId, ReminderSchedule reminder)
        {
            var r = rp.UpdateReminder(userId, reminder);
            if (!r.Result)
            {
                throw new NoReminderFoundException("No reminder found for this news");
            }
            return await r;
        }

        /* Implement all the methods of respective interface asynchronously*/


        // Implement GetReminders method which should be used to get all reminders by userId.

        // Implement CreateReminder method which should be used to create a new reminder.   

        // Implement DeleteReminder method which should be used to delete a reminder by userId and newsId

        // Implement a UpdateReminder method which should be used to update an existing reminder by using
        // userId and reminder details  


        // Throw your own custom Exception whereever its required in  GetReminders, CreateReminder, DeleteReminder, 
        // and UpdateReminder functionalities
    }
}
