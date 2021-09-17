using NewsService.Models;
using NewsService.Repository;
using NewsService.Exceptions;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace NewsService.Services
{
    //Inherit the respective interface and implement the methods in 
    // this class i.e NewsService by inheriting INewsService

    public class NewsService : INewsService
    {
        private INewsRepository nr;
        public NewsService(INewsRepository newsRepository)
        {
            nr = newsRepository;
        }/*
        * NewsRepository should  be injected through constructor injection. 
        * Please note that we should not create NewsRepository object using the new keyword
        */
        /* Implement all the methods of respective interface asynchronously*/

        /* Implement CreateNews method to add the new news details*/

        /* Implement AddOrUpdateReminder using userId and newsId*/

        /* Implement DeleteNews method to remove the existing news*/

        /* Implement DeleteReminder method to delte the Reminder using userId*/

        /* Implement FindAllNewsByUserId to get the News Details by userId*/
        public async Task<bool> AddOrUpdateReminder(string userId, int newsId, Reminder reminder)
        {
            var e = nr.GetNewsById(userId, newsId).Result;
            if (e == null)
            {
                throw new NoNewsFoundException($"NewsId {newsId} for {userId} doesn't exist");
            }
            return await nr.AddOrUpdateReminder(userId, newsId, reminder);
        }

        public async Task<int> CreateNews(string userId, News news)
        {
            var r = nr.IsNewsExist(userId, news.Title).Result;
            if (r)
            {
                throw new NewsAlreadyExistsException($"{userId} have already added this news");
            }
            return await nr.CreateNews(userId, news);
        }

        public async Task<bool> DeleteNews(string userId, int newsId)
        {
            var r = nr.DeleteNews(userId, newsId);
            if (!r.Result)
            {
                throw new NoNewsFoundException($"NewsId {newsId} for {userId} doesn't exist");
            }
            return await r;
        }

        public async Task<bool> DeleteReminder(string userId, int newsId)
        {
            var e = nr.IsReminderExists(userId, newsId).Result;
            if (!e)
            {
                throw new NoReminderFoundException("No reminder found for this news");
            }
            return await nr.DeleteReminder(userId, newsId);
        }

        public async Task<List<News>> FindAllNewsByUserId(string userId)
        {
            var r = nr.FindAllNewsByUserId(userId);
            if (r.Result == null)
            {
                throw new NoNewsFoundException($"No news found for {userId}");
            }
            return await r;
        }
    }
}
