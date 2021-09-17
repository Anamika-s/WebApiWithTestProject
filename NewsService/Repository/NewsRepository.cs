using MongoDB.Driver;
using NewsService.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace NewsService.Repository
{
    //Inherit the respective interface and implement the methods in
    // this class i.e NewsRepository by inheriting INewsRepository
    public class NewsRepository:INewsRepository
    {
        private  NewsContext nc;  //define a private variable to represent NewsDbContext
      
        public NewsRepository(NewsContext newsContext)
        {
            nc = newsContext;
        }

        public async Task<bool> AddOrUpdateReminder(string userId, int newsId, Reminder reminder)
        {
            var un = nc.News.Find(x => x.UserId == userId).FirstOrDefault();
            var n = new News()
            {
                Reminder = reminder,
                NewsId = newsId
            };
            var f = Builders<UserNews>.Filter.And(Builders<UserNews>.Filter.Where(x => x.UserId == userId),
                Builders<UserNews>.Filter.ElemMatch(x => x.NewsList, c => c.NewsId == newsId));
            if (un==null)
            {
                var nu = new UserNews()
                {
                    UserId = userId,
                    NewsList = new List<News>() { n }
                };
                nc.News.InsertOne(nu);
            }
            else
            {
                var news = un.NewsList.Where(x => x.NewsId == newsId).FirstOrDefault();
                UpdateResult u;
                if(news==null)
                {
                    u = nc.News.UpdateOne(f, Builders<UserNews>.Update.AddToSet(x => x.NewsList, n));
                }
                else
                {
                    u = nc.News.UpdateOne(f, Builders<UserNews>.Update.Set(x => x.NewsList[-1].Reminder, reminder).Set(x => x.NewsList[-1].NewsId, newsId));
                }
                var getCount = u.ModifiedCount;
            }
            var updated = nc.News.Find(f).FirstOrDefault();
            var a = updated != null;
            return await Task.FromResult(a);
        }

        public async Task<int> CreateNews(string userId, News news)
        {
            var u = nc.News.Find(x => x.UserId == userId).FirstOrDefault();
            var n = u == null ? 100 : u.NewsList.Max(x => x.NewsId);
            if(news.NewsId==0)
            {
                news.NewsId=n+1;
            }
            if(u==null)
            {
                u = new UserNews()
                {
                    UserId = userId,
                    NewsList = new List<News>() { news }
                };
                nc.News.InsertOne(u);
            }
            else
            {
                var f = Builders<UserNews>.Filter.Where(x => x.UserId == userId);
                var up= Builders<UserNews>.Update.Push(x => x.NewsList,news);
                var c = nc.News.FindOneAndUpdate(f, up);
            }
            var cId = nc.News.Find(x => x.UserId == userId).FirstOrDefault().NewsList.Where(x => x.NewsId == news.NewsId).FirstOrDefault().NewsId;
            return await Task.FromResult(cId);
        }

        public async Task<bool> DeleteNews(string userId, int newsId)
        {
            var f = Builders<UserNews>.Filter.And(Builders<UserNews>.Filter.Where(x => x.UserId == userId));
            var n = nc.News.Find(x => x.UserId == userId).FirstOrDefault();
            if(n==null)
            {
                return await Task.FromResult(false);
            }
            var up = nc.News.FindOneAndUpdate(f, Builders<UserNews>.Update.PullFilter(x => x.NewsList, c => c.NewsId == newsId));
            var del = nc.News.Find(x => x.UserId == userId).FirstOrDefault().NewsList.Where(x => x.NewsId == newsId).FirstOrDefault();
            return await Task.FromResult(del == null);
        }

        public async Task<bool> DeleteReminder(string userId, int newsId)
        {
            var n = nc.News.Find(x => x.UserId == userId).FirstOrDefault();
            if (n == null || n.NewsList == null || n.NewsList.Count == 0)
            {
                return await Task.FromResult(false);
            }
            var rem = n.NewsList.Where(x => x.NewsId == newsId).FirstOrDefault();
            if (rem == null)
            {
                return await Task.FromResult(false);
            }
            var list = n.NewsList.Where(x => x.NewsId != newsId);
            var f = Builders<UserNews>.Filter.And(
                Builders<UserNews>.Filter.Where(x => x.UserId == userId),
                Builders<UserNews>.Filter.ElemMatch(x => x.NewsList, c => c.NewsId == newsId)
                );
            var up = Builders<UserNews>.Update.Set(x => x.NewsList, list);
            var a = nc.News.FindOneAndUpdate(f, up);
            var b = nc.News.Find(f).FirstOrDefault();
            return await Task.FromResult(b == null);
        }

        public async Task<List<News>> FindAllNewsByUserId(string userId)
        {
            List<News> nl = null;
            var r = nc.News.Find(x => x.UserId == userId).FirstOrDefault();
            if (r == null)
            {
                return await Task.FromResult(nl);
            }
            return await Task.FromResult(r.NewsList);
        }
    public async Task<News> GetNewsById(string userId, int newsId)
        {
        var p = Builders<UserNews>.Projection.Expression(x => x.NewsList.Where(y => y.NewsId == newsId).FirstOrDefault());
        return await Task.FromResult(nc.News.Find(x => x.UserId == userId).Project(p).FirstOrDefault());
        }

        public async Task<bool> IsNewsExist(string userId, string title)
        {
            var f = Builders<UserNews>.Filter.And
                (
                Builders<UserNews>.Filter.Where(x => x.UserId == userId),
                Builders<UserNews>.Filter.ElemMatch(x => x.NewsList, c => c.Title == title)
                );
            var r = nc.News.Find(f).AnyAsync();
            return await r;
        }

        public async Task<bool> IsReminderExists(string userId, int newsId)
        { 
            var f = Builders<UserNews>.Filter.And
                (
                Builders<UserNews>.Filter.Where(x => x.UserId == userId)
                );
            if (!nc.News.Find(f).Any())
            {
                return await Task.FromResult(false);
            }
            var t = nc.News.Find(f).FirstOrDefault().NewsList.Where(x => x.NewsId == newsId).FirstOrDefault();
            if (t == null)
            {
                return await Task.FromResult(false);
            }
            var result = t.Reminder != null;
            return await Task.FromResult(result);
        }
    }

      
        /* Implement all the methods of respective interface asynchronously*/

        // CreateNews method should be used to create a new news. NewsId should be autogenerated and
        // must start with 101.This should create a new UserNews if not exists else should push 
        //new news entry into existing UserNews collection.


        //FindAllNewsByUserId method should be used to retreive all news for a user by userId

        //DeleteNews method should be used to delete a news for a specific user

        //IsNewsExist method is used to check news of individual userId exist or not

        // GetNewsById  method is used to get the news by userId

        //AddOrUpdateReminder method is used to Add and update the reminder by userId and newsId

        //Delete Reminder method is used to Delete the created Reminder by userId

        //IsReminderExists method is used to check the Reminder Exist or not by userId

  
}
