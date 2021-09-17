using MongoDB.Driver;
using ReminderService.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace ReminderService.Repository
{
    //Inherit the respective interface and implement the methods in 
    // this class i.e ReminderRepository by inheriting IReminderRepository class 
    //which is used to implement all Data access operations
    public class ReminderRepository:IReminderRepository
    {
        private ReminderContext rc;   //define a private variable to represent Reminder Database Context
      

        public ReminderRepository(ReminderContext reminderContext)
        {
            rc = reminderContext;
        }

        public Task CreateReminder(string userId, string email, ReminderSchedule schedule)
        {
            var list = rc.Reminders.Find(x => x.UserId == userId).FirstOrDefault();
            var n = list == null ? 100 : list.NewsReminders.Max(x => x.NewsId);
            var f = Builders<Reminder>.Filter.And(
                Builders<Reminder>.Filter.Where(x => x.UserId == userId && x.Email == email)
            );
            if (schedule.NewsId == 0)
            {
                schedule.NewsId = n;
            }
            if (list == null)
            {
                var reminder = new Reminder()
                {
                    UserId = userId,
                    Email = email,
                    NewsReminders = new List<ReminderSchedule>()
                    {
                       schedule
                    }
                };
                rc.Reminders.InsertOne(reminder);
            }
            else
            {
                list.NewsReminders.Add(schedule);
                var rem = new Reminder()
                {
                    UserId = userId,
                    Email = email,
                    NewsReminders = list.NewsReminders
                };
                var up = Builders<Reminder>.Update.Set(x => x.NewsReminders, list.NewsReminders);
                rc.Reminders.UpdateOne(f, up);
            }
            var c = rc.Reminders.Find(f).FirstOrDefault();
            return Task.FromResult(c);
        }

        public async Task<bool> DeleteReminder(string userId, int newsId)
        {
            var r = rc.Reminders.Find(x => x.UserId == userId).FirstOrDefault();

            if (r == null)
            {
                return await Task.FromResult(false);
            }
            var value = r.NewsReminders.Where(x => x.NewsId == newsId).FirstOrDefault();
            if (value == null)
            {
                return await Task.FromResult(false);
            }
            var list = r.NewsReminders.Where(x => x.NewsId != newsId).ToList();
            r.NewsReminders = list;
            var up = Builders<Reminder>.Update.Set(x => x.NewsReminders, list);
            var f = Builders<Reminder>.Filter.Where(x => x.UserId == userId);
            var del = rc.Reminders.UpdateOne(f, up);
            var res = rc.Reminders.Find(x => x.UserId == userId).FirstOrDefault().NewsReminders.Where(x => x.NewsId == newsId).FirstOrDefault();
            return await Task.FromResult(res == null);
        }

        public async Task<List<ReminderSchedule>> GetReminders(string userId)
        {
            List<ReminderSchedule> t = null;
            var rem = rc.Reminders.Find(x => x.UserId == userId).FirstOrDefault();
            if (rem == null)
            {
                return await Task.FromResult(t);
            }
            return await Task.FromResult(rem.NewsReminders);
        }

        public async Task<bool> IsReminderExists(string userId, int newsId)
        {
            var exist = rc.Reminders.Find(x => x.UserId == userId).FirstOrDefault();
            if (exist == null)
            {
                return await Task.FromResult(false);
            }
            var reminderExist = exist.NewsReminders.Where(x => x.NewsId == newsId).FirstOrDefault();
            return await Task.FromResult(reminderExist != null);
        }

        public async Task<bool> UpdateReminder(string userId, ReminderSchedule reminder)
        {
            var rem = rc.Reminders.Find(x => x.UserId == userId).FirstOrDefault();
            if (rem == null)
            {
                return await Task.FromResult(false);
            }
            var rl = rem.NewsReminders;
            if (rl == null || rl.Count == 0)
            {
                return await Task.FromResult(false);
            }
            var rexist = rl.Where(x => x.NewsId == reminder.NewsId).FirstOrDefault();
            if (rexist == null)
            {
                return await Task.FromResult(false);
            }
            var f = Builders<Reminder>.Filter.And(
                Builders<Reminder>.Filter.Where(x => x.UserId == userId),
                Builders<Reminder>.Filter.ElemMatch(x => x.NewsReminders, c => c.NewsId == reminder.NewsId)
                );
            var e = rc.Reminders.Find(x => x.UserId == userId).FirstOrDefault();
            var eres = rc.Reminders.Find(f).FirstOrDefault();
            if (eres == null)
            {
                return await Task.FromResult(false);
            }
            var update = Builders<Reminder>.Update.Set(x => x.NewsReminders[-1], reminder);
            rc.Reminders.FindOneAndUpdate(f, update);
            var result = rc.Reminders.Find(f).FirstOrDefault();
            var m = result.NewsReminders.Where(x => x.NewsId == reminder.NewsId).FirstOrDefault();
            var incomingReminderDate = reminder.Schedule.ToString("MM/dd/yyyy");
            var updatedReminderDate = m.Schedule.ToString("MM/dd/yyyy");
            return await Task.FromResult(incomingReminderDate == updatedReminderDate);
        }
        //Implement the methods of interface Asynchronously.

        // Implement CreateReminder method which should be used to save a new reminder.  

        // Implement DeleteReminder method which should be used to delete an existing reminder.

        // Implement GetReminders method which should be used to get a reminder by userId.

        // Implement IsReminderExists method which should be used to check an existing reminder by userId

        // Implement UpdateReminder method which should be used to update an existing reminder using  userId and 
        // reminder Schedule

    }
}
