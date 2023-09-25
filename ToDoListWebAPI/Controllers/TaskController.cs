using Microsoft.AspNetCore.Mvc;
using ToDoListWebInfrastructure.Interfaces;
using ToDoListWebDomain.Domain.Entity;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

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

            if (!ModelState.IsValid)
            {
                // Если модель данных недействительна, вернуть BadRequest с информацией об ошибках
                return BadRequest(ModelState);
            }
            else
            {
                if (TaskModel.NameTask != null && TaskModel.DescriptionTask != null)
                {
                    await _dbContext.AddAsync(new ToDoTask()
                    {
                        NameTask = TaskModel.NameTask,
                        DescriptionTask = TaskModel.DescriptionTask,
                        TaskTime = TaskModel.TaskTime,
                        Status = "In progress"

                    });
                    await _dbContext.SaveChangesAsync();
                    return Ok();

                }
                else
                {
                    return Ok();
                }
            }  
        }

        //удалаяем нашу задачу из бд
        [HttpPost("delete/{id}")]
        public async Task<IActionResult> DeleteTaskDBb(int Id)
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
        public async Task<IActionResult> ChangeTaskDb(int Id, string TaskName, string TaskDescription, DateTime TaskData, string TaskStatus)
        {
            var Task = _dbContext.Get(Id);
            if (Task != null)
            {
                 _logger.LogInformation(message: "мы попали в метод записи ");
                 if (!string.IsNullOrEmpty(TaskName) && !string.IsNullOrEmpty(TaskDescription) && !string.IsNullOrEmpty(TaskStatus))
                 {
                     Task.Id = Id;
                     Task.NameTask = TaskName;
                     Task.DescriptionTask = TaskDescription;
                     Task.TaskTime = TaskData;
                     Task.Status = TaskStatus;
                        
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



