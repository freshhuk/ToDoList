using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ToDoListWeb.Models;
using ToDoListWeb.Entity;

namespace ToDoListWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        //отсылаем наши данные на страницу
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.AllTask = new TaskDbContex().ToDoTask;
            return View();
        }
        public IActionResult CreateTask()
        {
            return View();
        }
        //Принимаем данные из формы , и заносим их в бд
        [HttpPost]
        public IActionResult GetTaskDb(string TaskName, string TaskDescription, DateTime TaskData)
        {
            if (TaskName != null && TaskDescription != null && TaskData != null)
            {
                using(var TaskDb = new TaskDbContex()) 
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
            using(var TaskDb = new TaskDbContex())
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}