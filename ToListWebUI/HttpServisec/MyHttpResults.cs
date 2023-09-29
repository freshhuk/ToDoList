using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using ToDoListWebDomain.Domain.Entity;
using ToDoListWebDomain.Domain.Models;
namespace ToListWebUI.HttpServisec
{
    [Route("myhttpresults")]
    public class MyHttpResultsController : Controller
    {
        private readonly APIToDoListHttpServices _apiHttpServisec;
        private readonly ILogger<MyHttpResultsController> _logger;
        public MyHttpResultsController(APIToDoListHttpServices apiHttpServisec, ILogger<MyHttpResultsController> logger)
        {
            _apiHttpServisec = apiHttpServisec;
            _logger = logger;
        }
        [HttpPost]
        [Route("resultaddtask")]
        public async Task<IActionResult> ResultAddTaskDbAsync(string TaskName, string TaskDescription, DateTime TimeTask, string TaskStatus)
        {
            var model = new ToDoTask()
            {
                NameTask = TaskName,
                DescriptionTask = TaskDescription,
                TaskTime = TimeTask.Date,
                Status = TaskStatus
            };

            _logger.LogInformation($"Sending task to APIHttpServices: {JsonSerializer.Serialize(model)}");
            var result = await _apiHttpServisec.AddTaskDbAsync(model);
            if(result == "successful")
            {
                return Redirect("~/Home/Index");
            }
            
            else if (result == "nosuccessful")
            {
                //error
                return Redirect("~/Home/Index");
            }

            else
            {
                return Redirect("~/Home/StartPage");
            }


        }
        [HttpPost]
        [Route("resultchangetaskdb")]
        public async Task<IActionResult> ResultChangeTaskDbAsync([FromForm] int _Id, string _TaskName, string _TaskDescription, DateTime _TimeTask, string _TaskStatus)
        {
            var chamgemodel = new ChangeTaskModel()
            {
                Id = _Id,
                TaskName = _TaskName,
                TaskDescription = _TaskDescription,
                TaskStatus = _TaskStatus,
                TaskData = _TimeTask.Date,
            };
            _logger.LogInformation($"Sending task to APIHttpServices: {JsonSerializer.Serialize(chamgemodel)}");

            var result = await _apiHttpServisec.ChangeTaskDbAsync(chamgemodel);
            if (result == "successful")
            {
                return Redirect("~/Home/Index");
            }

            else if (result == "nosuccessful")
            {
                //error
                return Redirect("~/Home/Index");
            }
            else
            {
                return Redirect("~/Home/StartPage");
            }
        }
    }
}
