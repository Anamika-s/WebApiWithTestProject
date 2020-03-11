﻿using MongoDB.Driver;
using NewsService.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace NewsService.Repository
{
    //Inherit the respective interface and implement the methods in
    // this class i.e NewsRepository by inheriting INewsRepository
    public class NewsRepository
    {
        //define a private variable to represent NewsDbContext
      
        public NewsRepository(NewsContext newsContext)
        {
          
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
}
