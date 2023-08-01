﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moq;
using ToDoListWeb.Controllers;
using ToDoListWeb.Interfaces;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using ToDoListWeb.Entity;

namespace UnitTests
{
    public class HomeControllerTest
    {
        [Fact]
        public void StartPageTest()
        {
            //Arrange
            var mockDbContext = new Mock<IDataContext>();
            var mockLogger = new Mock<ILogger<HomeController>>();

            var controller = new HomeController(mockDbContext.Object, mockLogger.Object);

            //Act
            var result = controller.StartPage() as ViewResult;

            //Assert
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
        }
        [Fact]
        public void SettingsPageTest()
        {
            //Arrange
            var mockDbContext = new Mock<IDataContext>();
            var mockLogger = new Mock<ILogger<HomeController>>();

            var controller = new HomeController(mockDbContext.Object, mockLogger.Object);

            //Act
            var result = controller.Settings() as ViewResult;

            //Assert
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void CreateTaskPageTest()
        {
            //дописатт тест

            //Arrange
            var mockDbContext = new Mock<IDataContext>();
            var mockLogger = new Mock<ILogger<HomeController>>();

            var controller = new HomeController(mockDbContext.Object, mockLogger.Object);

            //Act
            var result = controller.CreateTask() as ViewResult;

            //Assert
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void ProfilePageTest()
        {
            //Arrange
            var mockDbContext = new Mock<IDataContext>();
            var mockLogger = new Mock<ILogger<HomeController>>();

            var controller = new HomeController(mockDbContext.Object, mockLogger.Object);

            //Act
            var result = controller.Profile() as ViewResult;

            //Assert
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
        }
        


        // Вспомогательный метод для создания тестовых данных
        private List<ToDoTask> GetTestTasks()
        {
            return new List<ToDoTask>
            {
                new ToDoTask { Id = 1, NameTask = "Задача 1", DescriptionTask = "Описание 1", TaskTime = DateTime.Now, Status = "Открыто" },
                new ToDoTask { Id = 2, NameTask = "Задача 2", DescriptionTask = "Описание 2", TaskTime = DateTime.Now, Status = "Открыто" },
                new ToDoTask { Id = 3, NameTask = "Задача 3", DescriptionTask = "Описание 3", TaskTime = DateTime.Now, Status = "Открыто" },
                new ToDoTask { Id = 4, NameTask = "Задача 4", DescriptionTask = "Описание 4", TaskTime = DateTime.Now, Status = "Открыто" }
            };
        }
    }
}
