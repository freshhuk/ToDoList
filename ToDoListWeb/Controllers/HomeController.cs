using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using ToDoListWeb.Entity;
using ToDoListWeb.Models;

namespace ToDoListWeb.Controllers
{
    //[Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<TaskController> _logger;
        private readonly TaskDbContex _dbContext;


        public HomeController([FromServices] TaskDbContex dbContext, ILogger<TaskController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult StartPage()
        {
            return View();
        }
        //отсылаем наши данные на страницу
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            await _dbContext.Database.EnsureCreatedAsync();
            ViewBag.AllTask = _dbContext.ToDoTask;
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

            using (var TaskDb = _dbContext)
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