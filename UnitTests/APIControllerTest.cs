using Castle.Core.Logging;
using Microsoft.Extensions.Logging;
using Moq;
using ToDoListWebAPI.Controllers;
using UnitTests.MockEntities;

namespace UnitTests
{
    public class APIControllerTest
    {
        [Fact]
        public void AddTaskDbTest()
        {
            //Arrange
            var mockILogger = new Mock<ILogger<TaskController>>();
            var mockDbContext = new MockToDoTask();

            //var controller = new TaskController(mockILogger.Object, mockDbContext.);



        }
    }
}