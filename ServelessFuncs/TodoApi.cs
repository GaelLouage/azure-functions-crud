using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.WebJobs.Host;
using System.Collections.Generic;
using System.Linq;

namespace ServelessFuncs
{
    public static class TodoApi
    {
        private static List<Todo> items = new List<Todo>();
        [FunctionName("CreateTodo")]
        public static async Task<IActionResult> CreateTodo(
            [HttpTrigger(AuthorizationLevel.Anonymous,
            nameof(HttpMethods.Post), Route = "todo")]HttpRequest req,
            ILogger log)
        {   
            log.LogInformation("Creating a new todo list item");
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var input = JsonConvert.DeserializeObject<TodoCreateModel>(requestBody);    
            var todo = new Todo() { TaskDescription = input.TaskDescription };
            items.Add(todo);
            return new OkObjectResult(todo);
        }
        [FunctionName("GetTodos")]
        public static async Task<IActionResult> GetTodos(
           [HttpTrigger(AuthorizationLevel.Anonymous,
            nameof(HttpMethods.Get), Route = "todo")]HttpRequest req,
           ILogger log)
        {
            log.LogInformation("Getting todo list items");
           
            return new OkObjectResult(items);
        }
        [FunctionName("GetTodosById")]
        public static async Task<IActionResult> GetTodosById(
          [HttpTrigger(AuthorizationLevel.Anonymous,
            nameof(HttpMethods.Get), Route = "todo/{id}")]HttpRequest req,
          ILogger log, string id)
        {
            log.LogInformation("Getting todo list item by id.");
            var todo = items.FirstOrDefault(i => i.Id == id);
            if(todo == null)
            {
                return new NotFoundResult();
            }
            return new OkObjectResult(todo);
        }

        [FunctionName("UpdateTodo")]
        public static async Task<IActionResult> UpdateTodo(
         [HttpTrigger(AuthorizationLevel.Anonymous,
            nameof(HttpMethods.Put), Route = "todo/{id}")]HttpRequest req,
         ILogger log, string id)
        {
            log.LogInformation("Getting todo list items");
            var todo = items.FirstOrDefault(i => i.Id == id);
            if (todo == null)
            {
                return new NotFoundResult();
            }
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var updated = JsonConvert.DeserializeObject<TodoCreateModel>(requestBody);

            todo.IsCompleted = updated.IsCompleted;
            if(!string.IsNullOrEmpty(updated.TaskDescription)) {
                todo.TaskDescription = updated.TaskDescription;
            }

            return new OkObjectResult(todo);
        }

        [FunctionName("DeleteTodo")]
        public static async Task<IActionResult> DeleteTodo(
     [HttpTrigger(AuthorizationLevel.Anonymous,
            nameof(HttpMethods.Delete), Route = "todo/{id}")]HttpRequest req,
     ILogger log, string id)
        {
            
            var todo = items.FirstOrDefault(i => i.Id == id);
            if (todo == null)
            {
                return new NotFoundResult();
            }
            items.Remove(todo);

            return new OkObjectResult(todo);
        }
    }
}
