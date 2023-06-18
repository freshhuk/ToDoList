using Microsoft.AspNetCore.Mvc;
using ToDoListWeb.Entity;
using ToDoListWeb.Models;
using System.Threading.Tasks;

namespace ToDoListWeb.Controllers
{
    public class TaskController : Controller
    {
        private readonly ILogger<TaskController> _logger;

        public TaskController(ILogger<TaskController> logger)
        {
            _logger = logger;
        }
        //Принимаем данные из формы , и заносим их в бд
        [HttpPost]
        public async Task<IActionResult> GetTaskDb(string TaskName, string TaskDescription, DateTime TaskData)
        {
            if (TaskName != null && TaskDescription != null)
            {
                using (var TaskDb = new TaskDbContex())
                {

                    await TaskDb.AddAsync(new ToDoTask()
                    {
                        NameTask = TaskName,
                        DescriptionTask = TaskDescription,
                        TaskTime = TaskData.Date,
                        Status = "In progress"

                    });
                    await TaskDb.SaveChangesAsync();
                }
                
            }
            else
            {
                TempData["ErrorMessage"] = "You have not completed all fields";
                return RedirectToAction("CreateTask", "Home");
           
            }
            //в поле где статус мы автоматом пишим в процесе так как
            //ее только создали и она в процесе выполнения
            return Redirect("~/");
        }

        //удалаяем нашу задачу из бд
        [HttpPost]
        public async Task<IActionResult> DeleteTaskDBb(int Id)
        {
            using (var TaskDb = new TaskDbContex())
            {
                var Task = await TaskDb.ToDoTask.FindAsync(Id);
                if (Task != null)
                {
                    TaskDb.ToDoTask.Remove(Task);
                    await TaskDb.SaveChangesAsync();
                    return Redirect("~/");
                }
                else
                {
                    //потом можно вывести какую то ошибку но покачто просто кидает на главную
                    return Redirect("~/");
                }
            }
        }

        //метод для изминения нашей задачи в бд
        [HttpPost]
        public IActionResult ChangeTaskDb(int Id, string TaskName, string TaskDescription, DateTime TaskData, string TaskStatus)
        {
            using (var TaskDb = new TaskDbContex())
            {
                var Task = TaskDb.ToDoTask.Find(Id);
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
                        
                        TaskDb.SaveChanges();
                        _logger.LogInformation(message: "Данные записались");
                        return Redirect("~/");
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
                    return Redirect("~/");

                }
            }

        }
    }
}
