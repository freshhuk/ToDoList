using Microsoft.AspNetCore.Mvc;
using ToDoListWebInfrastructure.Interfaces;
using ToDoListWebDomain.Domain.Entity;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;
using ToDoListWebDomain.Domain.Models;
using Microsoft.IdentityModel.Tokens;

namespace ToDoListWebAPI.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : Controller
    {
        
        private readonly ILogger<TaskController> _logger;
        private readonly IDataContext<ToDoTask> _dbContext;


        public TaskController([FromServices] IDataContext<ToDoTask> dbContext, ILogger<TaskController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        


        //возможно переписать на то что б я принимал обьект с полями а не поля по одному
        //Принимаем данные из формы , и заносим их в бд
        [HttpPost]
        [Route("AddTaskDb")]
        public async Task<IActionResult> AddTaskDb(ToDoTask TaskModel)
        {
            _logger.LogInformation($"Received task from UI: {JsonSerializer.Serialize(TaskModel)}");

            if (!ModelState.IsValid)
            {
                // Если модель данных недействительна, вернуть BadRequest с информацией об ошибках
                _logger.LogError(message: "модель данных недействительна");
                return BadRequest(ModelState);
                
            }
            if (string.IsNullOrWhiteSpace(TaskModel.NameTask) || string.IsNullOrWhiteSpace(TaskModel.DescriptionTask))
            {
                // Если обязательные поля не заполнены, вернуть BadRequest
                return BadRequest("NameTask and DescriptionTask are required fields.");
            }
            // Заполнить поле Status значением "In progress"
            TaskModel.Status = "In progress";

            // Добавить задачу в базу данных
            await _dbContext.AddAsync(TaskModel);
            await _dbContext.SaveChangesAsync();

            return Ok();

        }

        //удалаяем нашу задачу из бд
        [HttpPost("DeleteTaskDb")]
        public async Task<IActionResult> DeleteTaskDb([FromBody] int Id)
        {
            
            var Task =  _dbContext.Get(Id);
            if (Task != null)
            {
                _dbContext.Delete(Id);
                await _dbContext.SaveChangesAsync();
                return Ok();
                
            }
            else
            {
                //потом можно вывести какую то ошибку но покачто просто кидает на главную
                return BadRequest();
            
            }
        }

        //метод для изминения нашей задачи в бд
        [HttpPost]
        [Route("ChangeTaskDb")]
        public async Task<IActionResult> ChangeTaskDb([FromBody] ChangeTaskModel chamgemodel)
        {
            var Task = _dbContext.Get(chamgemodel.Id);
            if (Task != null)
            {
                 _logger.LogInformation(message: "мы попали в метод записи ");
                 if (!string.IsNullOrEmpty(chamgemodel.TaskName) && !string.IsNullOrEmpty(chamgemodel.TaskDescription) && !string.IsNullOrEmpty(chamgemodel.TaskStatus))
                 {
                     Task.Id = chamgemodel.Id;
                     Task.NameTask = chamgemodel.TaskName;
                     Task.DescriptionTask = chamgemodel.TaskDescription;
                     Task.TaskTime = chamgemodel.TaskData;
                     Task.Status = chamgemodel.TaskStatus;
                        
                     await _dbContext.SaveChangesAsync();
                     _logger.LogInformation(message: "Данные записались");
                     return Ok();
                 }
                 else
                 {
                     _logger.LogError(message:"Error data null");
                     TempData["ErrorMessage"] = "You have not completed all fields";
                     return BadRequest();
                 }
            }
            else
            {
                //потом можно вывести какую то ошибку но покачто просто кидает на главную
                _logger.LogError(message: "Error задача не найдена");
                return BadRequest();

            }               
        }
    }
}



