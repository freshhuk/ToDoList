using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoListWebDomain.Domain.Entity;
using ToDoListWebDomain.Domain.Models;
using ToDoListWebInfrastructure.Context;
using ToDoListWebInfrastructure.Interfaces;

namespace ToListWebUI.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDataContext<ToDoTask> _dbContext;


        public HomeController([FromServices] IDataContext<ToDoTask> dbContext, ILogger<HomeController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        


        [AllowAnonymous]
        [HttpGet]
        public IActionResult StartPage()
        {
            return View();
        }

        //отсылаем наши данные на страницу
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            await CheckDateTask();
            List<ToDoTask> sortedTasks = _dbContext.GetAll().ToList();
            ViewBag.NoSortTask = sortedTasks;
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
        [HttpGet]
        public IActionResult GeneralTasks()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Profile()
        {

            return View();
        }
        [HttpGet]
        public IActionResult Settings()
        {
            return View();
        }
        [HttpGet]
        public IActionResult ChangeTaskPage(int Id)
        {

            var changedTask = _dbContext.Get(Id);
            if (changedTask == null)
            {
                // В случае, если задача с указанным Id не найдена, перенаправляем на другую страницу или выводим сообщение об ошибке
                return RedirectToAction("Index", "Home");
            }
            ViewBag.ChangedTask = changedTask;
            _logger.LogInformation(message: "Отправили данные о задаче которую изменяем");

            //for error
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

        //метод если текущая дата совпадает с датой в задаче то статус не выполнено
        private async Task CheckDateTask()
        {
            if (_dbContext.GetAll() != null)
            {
                foreach (var task in _dbContext.GetAll())
                {
                    if (task.TaskTime <= DateTime.Now)
                    {
                        task.Status = "Not done";
                    }
                }
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                _logger.LogWarning(message: "_dbContext.ToDoTask is null");
            }

        }
    }
}
