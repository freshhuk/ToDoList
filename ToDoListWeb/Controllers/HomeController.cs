﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Threading.Tasks;
using ToDoListWeb.Entity;
using ToDoListWeb.Models;

namespace ToDoListWeb.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<TaskController> _logger;
        private readonly TaskDbContex _dbContext;
        private readonly UserDbContext _userdbContext;


        public HomeController([FromServices] TaskDbContex dbContext, ILogger<TaskController> logger, UserDbContext userdbContext)
        {
            _dbContext = dbContext;
            _userdbContext = userdbContext;
            _logger = logger;
        }
        

        
 
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> StartPage()
        {
            await _dbContext.Database.EnsureCreatedAsync();
            await _userdbContext.Database.EnsureCreatedAsync();
            return View();
        }
        
        //отсылаем наши данные на страницу
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            await CheckDateTask();
            List<ToDoTask> sortedTasks = await _dbContext.ToDoTask.ToListAsync();
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

        public IActionResult Profile()
        {

            return View();
        }
        public IActionResult Settings()
        {
            return View();
        }
        [HttpGet]
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
        //метод если текущая дата совпадает с датой в задаче то статус не выполнено
        private async Task CheckDateTask()
        {
            //можно и использовать linq но покачто хай будет через условие 
            //var tasksToUpdate = await _dbContext.ToDoTask.Where(task => task.TaskTime <= DateTime.Now).ToListAsync();
            foreach (var task in _dbContext.ToDoTask)
            {
                if (task.TaskTime <= DateTime.Now)
                {
                    task.Status = "Not done";
                }           
            }
            await _dbContext.SaveChangesAsync();
        }
    }
}