﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDoListWebDomain.Domain.Enums;
using ToDoListWebDomain.Domain.Entity;
using ToDoListWebInfrastructure.Interfaces;

namespace ToListWebUI.Controllers

{
    //[Authorize]
    public class SortTaskController : Controller
    {
        private readonly IDataContext<ToDoTask> _dbContext;
        public SortTaskEnum.SortTaskType taskEnum;
        private readonly ILogger<SortTaskController> _logger;


        public SortTaskController([FromServices] IDataContext<ToDoTask> dbContext, ILogger<SortTaskController> logger)
        {   
            _logger = logger;
            _dbContext = dbContext; 
        }
        //метод для получения какой тип сортировки мы выбрали
        [HttpPost]
        public IActionResult GetSortEnum(string SortType)
        {
            _logger.LogInformation(message: "Мы попали в метод выбора енама");
            SortTaskEnum.SortTaskType taskEnum = SortTaskEnum.SortTaskType.NoSort;
            switch (SortType)
            {
                case "No Sort":
                    _logger.LogInformation(message: "Выбран енам");
                    taskEnum = SortTaskEnum.SortTaskType.NoSort;
                    NoSort();
                    break;
                case "Date(Descending)":
                    taskEnum = SortTaskEnum.SortTaskType.SortTaskDateFromMinToMax;
                    DateDescending();
                    break;
                case "Date(ascending)":
                    taskEnum = SortTaskEnum.SortTaskType.SortTaskDateFromMaxToMin;
                    DateAascending();
                    break;
                case "Recently added":
                    taskEnum = SortTaskEnum.SortTaskType.SortRecentlyAdded;
                    RecentlyAdded();
                    break;
                case "Added long ago":
                    taskEnum = SortTaskEnum.SortTaskType.SortOldAdded;
                    AddedLongAgo();
                    break;
                default:
                    break;
            }
            ViewBag.SortType = taskEnum;
            return View("~/Views/Home/Index.cshtml");
        }
        //No Sort
        
        private IActionResult NoSort()
        {
            List<ToDoTask> sortedTasks = _dbContext.GetAll().ToList();
            _logger.LogInformation(message: "Метод сортировки ");
            ViewBag.NoSortTask = sortedTasks;
            return View("~/Views/Home/Index.cshtml");
        }
        //метод для филтрации заданий по дате min - max

        private IActionResult DateDescending()
        {
            List<ToDoTask> sortedTasks = _dbContext.GetAll().OrderBy(t => t.TaskTime).ToList();
            ViewBag.SortTasksMinToMax = sortedTasks;
            return View("~/Views/Home/Index.cshtml");
        }
        //метод для сортировки по дате max - min

        private IActionResult DateAascending()
        {
            List<ToDoTask> sortedTasks = _dbContext.GetAll().OrderByDescending(t => t.TaskTime).ToList();
            ViewBag.SortTasksMaxToMin = sortedTasks;
            return View("~/Views/Home/Index.cshtml");
        }
        //метод сортировики по индексу недавно добавленые

        private IActionResult RecentlyAdded()
        {
            List<ToDoTask> sortedTasks = _dbContext.GetAll().OrderByDescending(t => t.Id).ToList();
            ViewBag.SortTaskRecentlyAdded = sortedTasks;
            return View("~/Views/Home/Index.cshtml");
        }
        //метод сортировики по индексу давно добавленые
        private IActionResult AddedLongAgo()
        {
            List<ToDoTask> sortedTasks = _dbContext.GetAll().OrderBy(t => t.Id).ToList();
            ViewBag.SortTaskOldAdded = sortedTasks;
            return View("~/Views/Home/Index.cshtml");
        }
    }
}
