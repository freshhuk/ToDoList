using Microsoft.AspNetCore.Mvc;
using ToDoListWeb.Entity;
using ToDoListWeb.Interfaces;
using ToDoListWeb.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace ToDoListWeb.Controllers
{
    [Authorize]
    public class TaskController : Controller, ITaskController
    {
        
        private readonly ILogger<TaskController> _logger;
        private readonly IDataContext _dbContext;


        public TaskController([FromServices] IDataContext dbContext, ILogger<TaskController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        



        //Принимаем данные из формы , и заносим их в бд
        [HttpPost]
        public async Task<IActionResult> GetTaskDb(string TaskName, string TaskDescription, DateTime TaskData)
        {
            
            if (TaskName != null && TaskDescription != null)
            {
                

                await _dbContext.AddAsync(new ToDoTask()
                {
                    NameTask = TaskName,
                    DescriptionTask = TaskDescription,
                    TaskTime = TaskData.Date,
                    Status = "In progress"

                });
                await _dbContext.SaveChangesAsync();
                
                
            }
            else
            {
                TempData["ErrorMessage"] = "Fill the rest of fields!";
                return RedirectToAction("CreateTask", "Home");
           
            }
            //в поле где статус мы автоматом пишим в процесе так как
            //ее только создали и она в процесе выполнения
            return Redirect("~/Home/Index");
        }

        //удалаяем нашу задачу из бд
        [HttpPost]
        public async Task<IActionResult> DeleteTaskDBb(int Id)
        {
            
            
            var Task = await _dbContext.ToDoTask.FindAsync(Id);
            if (Task != null)
            {
                _dbContext.ToDoTask.Remove(Task);
                await _dbContext.SaveChangesAsync();
                return Redirect("~/Home/Index");
            }
            else
            {
                //потом можно вывести какую то ошибку но покачто просто кидает на главную
                return Redirect("~/Home/Index");
            
            }
        }

        //метод для изминения нашей задачи в бд
        [HttpPost]
        public async Task<IActionResult> ChangeTaskDb(int Id, string TaskName, string TaskDescription, DateTime TaskData, string TaskStatus)
        {
            var Task = _dbContext.ToDoTask.Find(Id);
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
                     return Redirect("~/Home/Index");
                 }
                 else
                 {
                     _logger.LogError(message:"Error data null");
                     TempData["ErrorMessage"] = "You have not completed all fields";
                     return RedirectToAction("ChangeTaskPage", "Home");



                 }
            }
            else
            {
                //потом можно вывести какую то ошибку но покачто просто кидает на главную
                _logger.LogError(message: "Error задача не найдена");
                return Redirect("~/Home/Index");

            }
                
        }

      
    }
}
