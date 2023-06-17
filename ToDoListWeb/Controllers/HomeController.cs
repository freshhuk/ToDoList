using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ToDoListWeb.Entity;
using ToDoListWeb.Models;

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

        public IActionResult Profile()
        {
            return View();
        }
        public IActionResult Settings()
        {
            return View();
        }

        public IActionResult ChangeTaskPage(int Id)
        {

            using (var TaskDb = new TaskDbContex())
            {
                ViewBag.ChangedTask = TaskDb.ToDoTask.Find(Id);
                _logger.LogInformation(message: "Отправили данные о задаче которую изменяем");
            }
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}