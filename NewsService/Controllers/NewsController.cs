using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NewsService.Exceptions;
using NewsService.Models;
using NewsService.Services;
using System;
namespace NewsService.Controllers
{
    /*
    * As in this assignment, we are working with creating RESTful web service, hence annotate
    * the class with [ApiController] annotation and define the controller level route as per REST Api standard.
    */
    public class NewsController : ControllerBase
    {
        private INewsService ns; /*
        * NewsService should  be injected through constructor injection. 
        * Please note that we should not create service object using the new keyword
        */

        public NewsController(INewsService newsService)
        {
            ns = newsService;
        }

        /* Implement HttpVerbs and Functionalities asynchronously*/

        [HttpGet]
        [Route("api/news/{userId}")]
        public async Task<IActionResult> Get(string userId)
        {
            try
            {
                var result = await ns.FindAllNewsByUserId(userId);
                return Ok(result);
            }
            catch (NoNewsFoundException n)
            { 
                return NotFound(n.Message);
            }

            catch (NewsAlreadyExistsException na)
            {
                return Conflict(na.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        } /*
         * Define a handler method which will get us the news by a userId.
         * 
         * This handler method should return any one of the status messages basis on
         * different situations: 
         * 1. 200(OK) - If the news found successfully.
         * This handler method should map to the URL "/api/news/{userId}" using HTTP GET method
         */

        [HttpPut]
        [Route("/api/news/{userId}/{newsId}/reminder")]
        public async Task<IActionResult> Post(string userId, int newsId, [FromBody] Reminder reminder)
        {
            try
            {
                return Created("", await ns.AddOrUpdateReminder(userId, newsId, reminder));
            }
            catch (NoNewsFoundException n)
            {
                return NotFound(n.Message);
            }

            catch (NewsAlreadyExistsException na)
            {
                return Conflict(na.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        } /*
        * Define a handler method which will create a specific news by reading the
        * Serialized object from request body and save the news details in a News table
        * in the database.
        * 
        * Please note that CreateNews method should add a news and also handle the exception.
        * This handler method should return any one of the status messages basis on different situations: 
        * 1. 201(CREATED) - If the news created successfully. 
        * 2. 409(CONFLICT) - If the userId conflicts with any existing newsid
        * 
        * This handler method should map to the URL "/api/news" using HTTP POST method
        */

        [HttpDelete]
        [Route("/api/news/{userId}/{newsId}")]
        public async Task<IActionResult> Delete(string userId, int newsId)
        {
            try
            {
                return Ok(await ns.DeleteNews(userId, newsId));
            }
            catch (NoNewsFoundException n)
            {
                return NotFound(n.Message);
            }

            catch (NewsAlreadyExistsException na)
            {
                return Conflict(na.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        } /*
         * Define a handler method which will delete a news from a database.
         * 
         * This handler method should return any one of the status messages basis on
         * different situations: 
         * 1. 200(OK) - If the news deleted successfully from database. 
         * 2. 404(NOT FOUND) - If the news with specified newsId is not found.
         * 
         * This handler method should map to the URL "/api/news/userId/newsId" using HTTP Delete
         * method" where "id" should be replaced by a valid newsId without {}
         */

        [HttpDelete]
        [Route("/api/news/{userId}/{newsId:int}/reminder")]
        public async Task<IActionResult> DeleteReminder(string userId, int newsId)
        {
            try
            {
                return Ok(await ns.DeleteReminder(userId, newsId));
            }
            catch (NoNewsFoundException n)
            {
                return NotFound(n.Message);
            }
            catch (NoReminderFoundException nr)
            {
                return NotFound(nr.Message);
            }
            catch (NewsAlreadyExistsException na)
            {
                return Conflict(na.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        } /*
         * Define a handler method (DeleteReminder) which will delete a news from a database.
         * 
         * This handler method should return any one of the status messages basis on
         * different situations: 
         * 1. 200(OK) - If the news deleted successfully from database using userId with newsId
         * 2. 404(NOT FOUND) - If the news with specified newsId is not found.
         * 
         * This handler method should map to the URL "/api/news/userId/newsId/reminder" using HTTP Delete
         * method" where "id" should be replaced by a valid newsId without {}
         */


        [HttpPost]
        [Route("/api/news/{userId}")]

        public async Task<IActionResult> Post(string userId, [FromBody] News news)
        {
            try
            {
                return Created("", await ns.CreateNews(userId, news));
            }
            catch (NoNewsFoundException n)
            {
                return NotFound(n.Message);
            }

            catch (NewsAlreadyExistsException na)
            {
                return Conflict(na.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }/*
         * Define a handler method (Put) which will update a news by userId,newsId and with Reminder Details
         * 
         * This handler method should return any one of the status messages basis on
         * different situations: 
         * 1. 200(OK) - If the news updated successfully to the database using userId with newsId
         * 2. 404(NOT FOUND) - If the news with specified newsId is not found.
         * 
         * This handler method should map to the URL "/api/news/userId/newsId/reminder" using HTTP PUT
         * method" where "id" should be replaced by a valid newsId without {}
         */

    }
}
