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
            if (TempData.ContainsKey("ErrorMessage"))
            {
                ViewBag.ErrorMessage = TempData["ErrorMessage"];
            }
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

                var changedTask = TaskDb.ToDoTask.Find(Id);
                if (changedTask == null)
                {
                    // В случае, если задача с указанным Id не найдена, перенаправляем на другую страницу или выводим сообщение об ошибке
                    return RedirectToAction("Index", "Home");
                }
                ViewBag.ChangedTask = changedTask;
                _logger.LogInformation(message: "Отправили данные о задаче которую изменяем");
            }
            if (TempData.ContainsKey("ErrorMessage"))
            {
                ViewBag.ErrorMessage = TempData["ErrorMessage"];
            }
            else
            {
                ViewBag.ErrorMessage = null;  
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