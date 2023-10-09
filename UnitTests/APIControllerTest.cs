using Castle.Core.Logging;
using Microsoft.Extensions.Logging;
using Moq;
using ToDoListWebAPI.Controllers;
using ToDoListWebInfrastructure.Interfaces;
using UnitTests.MockEntities;
using ToDoListWebDomain.Domain.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;
using ToDoListWebDomain.Domain.Models;

namespace UnitTests
{
    public class APIControllerTest
    {
        #region TrueTests(OkResult)
        [Fact]
        public async Task AddTaskDbTest()
        {
            //Arrange
            var loggerMock = new Mock<ILogger<TaskController>>();

            var controller = new TaskController(new MockTestDbContext(), loggerMock.Object);
            var model = new ToDoTask()
            {
                Id = 1,
                NameTask = "Test",
                DescriptionTask = "Test",
                Status = "In progress",
                TaskTime = DateTime.Now,
            };
            //Act
            var result = await controller.AddTaskDb(model);

            //Assert

            Assert.IsType<OkResult>(result);
        }
        [Fact]
        public async Task DeleteTaskDbTest()
        {
            //Arrange
            var loggerMock = new Mock<ILogger<TaskController>>();

            var controller = new TaskController(new MockTestDbContext(), loggerMock.Object);

            //Act
            var result = await controller.DeleteTaskDb(2);

            //Assert
            Assert.IsType<OkResult>(result);
        }
        [Fact]
        public async Task ChangeTaskTest()
        {
            //Arrange
            var loggerMock = new Mock<ILogger<TaskController>>();

            var controller = new TaskController(new MockTestDbContext(), loggerMock.Object);
            var model = new ChangeTaskModel()
            {
                Id = 1,
                TaskName = "Test",
                TaskDescription = "Test",
                TaskStatus = "In progress",
                TaskData = DateTime.Now,
            };
            //Act
            var result = await controller.ChangeTaskDb(model);

            //Assert
            Assert.IsType<OkResult>(result);
        }
        #endregion



    }
}