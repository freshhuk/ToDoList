using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoListWeb.Enums;
using ToDoListWeb.Controllers;
using ToDoListWebDomain.Domain.Entity;
using ToDoListWebInfrastructure.Interfaces;
using Xunit;

namespace UnitTests
{
    public class SortTaskControllerTests
    {
        [Fact]
        public void GetSortEnum()
        {
            //Arrange
            var mockDbContext = new Mock<IDataContext<ToDoTask>>();
            mockDbContext.Setup(m => m.GetAll()).Returns(GetAll());
            var mockLogger = new Mock<ILogger<SortTaskController>>();

            var controller = new SortTaskController(mockDbContext.Object, mockLogger.Object);

            //Act 
            var result = controller.GetSortEnum("No Sort") as ViewResult;

            //Assert
            var actualSortType = result.ViewData["SortType"] as SortTaskEnum.SortTaskType?;
            Assert.NotNull(result);
            Assert.Equal(SortTaskEnum.SortTaskType.NoSort, actualSortType);

        }
        [Fact]
        public void NoSortTest()
        {
            //Arrange
            var mockDbContext = new Mock<IDataContext<ToDoTask>>();
            mockDbContext.Setup(m => m.GetAll()).Returns(GetAll());
            var mockLogger = new Mock<ILogger<SortTaskController>>();

            var controller = new SortTaskController(mockDbContext.Object, mockLogger.Object);

            //Act 
            var result = controller.NoSort() as ViewResult;

            //Assert

            Assert.NotNull(result);
            var sortedTask = result.ViewData["NoSortTask"] as List<ToDoTask>;
            Assert.NotNull(sortedTask);
            Assert.NotEmpty(sortedTask);
        }
        [Fact]
        public void DateDescendingTest()
        {
            //Arrange
            var mockDbContext = new Mock<IDataContext<ToDoTask>>();
            mockDbContext.Setup(m => m.GetAll()).Returns(GetAll());
            var mockLogger = new Mock<ILogger<SortTaskController>>();

            var controller = new SortTaskController(mockDbContext.Object, mockLogger.Object);

            //Act 
            var result = controller.DateDescending() as ViewResult;

            //Assert

            Assert.NotNull(result);
            var sortedTask = result.ViewData["SortTasksMinToMax"] as List<ToDoTask>;
            Assert.NotNull(sortedTask);
            Assert.NotEmpty(sortedTask);
        }
        [Fact]
        public void DateAascendingTest()
        {
            //Arrange
            var mockDbContext = new Mock<IDataContext<ToDoTask>>();
            mockDbContext.Setup(m => m.GetAll()).Returns(GetAll());
            var mockLogger = new Mock<ILogger<SortTaskController>>();
            
            var controller = new SortTaskController(mockDbContext.Object, mockLogger.Object);

            //Act 
            var result = controller.DateAascending() as ViewResult;

            //Assert

            Assert.NotNull(result);
            var sortedTask = result.ViewData["SortTasksMaxToMin"] as List<ToDoTask>;
            Assert.NotNull(sortedTask);
            Assert.NotEmpty(sortedTask);
        }
        [Fact]
        public void RecentlyAddedTest()
        {
            //Arrange
            var mockDbContext = new Mock<IDataContext<ToDoTask>>();
            mockDbContext.Setup(m => m.GetAll()).Returns(GetAll());
            var mockLogger = new Mock<ILogger<SortTaskController>>();

            var controller = new SortTaskController(mockDbContext.Object, mockLogger.Object);

            //Act 
            var result = controller.RecentlyAdded() as ViewResult;

            //Assert
            Assert.NotNull(result);
            var sortedTask = result.ViewData["SortTaskRecentlyAdded"] as List<ToDoTask>;
            Assert.NotNull(sortedTask);
            Assert.NotEmpty(sortedTask);

        }
        [Fact]
        public void AddedLongAgo_ReturnsViewResult()
        {
            //Arraange
            var mockDbContext = new Mock<IDataContext<ToDoTask>>();
            mockDbContext.Setup(m => m.GetAll()).Returns(GetAll());

            var mockLogger = new Mock<ILogger<SortTaskController>>();

            var _controller = new SortTaskController(mockDbContext.Object, mockLogger.Object);

            // Act
            var result = _controller.AddedLongAgo() as ViewResult;

            // Assert
            Assert.NotNull(result);
            var sortedTasks = result.ViewData["SortTaskOldAdded"] as List<ToDoTask>;
            Assert.NotNull(sortedTasks);
            Assert.NotEmpty(sortedTasks);
        }


        // Вспомогательный метод для создания тестовых данных
        private List<ToDoTask> GetAll()
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
