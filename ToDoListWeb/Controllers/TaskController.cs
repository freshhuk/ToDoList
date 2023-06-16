using Microsoft.AspNetCore.Mvc;
using ToDoListWeb.Entity;
using ToDoListWeb.Models;

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
        public IActionResult GetTaskDb(string TaskName, string TaskDescription, DateTime TaskData)
        {
            if (TaskName != null && TaskDescription != null)
            {
                using (var TaskDb = new TaskDbContex())
                {
                    TaskDb.Add(new ToDoTask()
                    {
                        NameTask = TaskName,
                        DescriptionTask = TaskDescription,
                        TaskTime = TaskData,
                        Status = "В процессе"

                    });
                    TaskDb.SaveChanges();
                }
            }
            //в поле где статус мы автоматом пишим в процесе так как
            //ее только создали и она в процесе выполнения
            return Redirect("~/");
        }

        //удалаяем нашу задачу из бд
        [HttpPost]
        public IActionResult DeleteTaskDBb(int Id)
        {
            using (var TaskDb = new TaskDbContex())
            {
                var Task = TaskDb.ToDoTask.Find(Id);
                if (Task != null)
                {
                    TaskDb.ToDoTask.Remove(Task);
                    TaskDb.SaveChanges();
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
                        //потом можно вывести какую то ошибку но покачто просто кидает на главную
                        _logger.LogError(message:"Error data null");
                        return Redirect("~/");
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
