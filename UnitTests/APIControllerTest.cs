using Castle.Core.Logging;
using Microsoft.Extensions.Logging;
using Moq;
using ToDoListWebAPI.Controllers;
using ToDoListWebInfrastructure.Interfaces;
using UnitTests.MockEntities;
using ToDoListWebDomain.Domain.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;

namespace UnitTests
{
    public class APIControllerTest
    {
        [Fact]
        public void AddTaskDbTest()
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
            var result = controller.AddTaskDb(model);

            //Assert

            Assert.IsType<Ok>(result);
        }
    }
}