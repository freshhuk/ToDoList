using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoListWebDomain.Domain.Entity;
using ToDoListWebInfrastructure.Interfaces;
using ToListWebUI.Controllers;

namespace UnitTests
{
    public class UIHomeControllerTest
    {
        [Fact]
        public void StartPageTest()
        {
            //Arrange
            var mockDbContext = new Mock<IDataContext<ToDoTask>>();
            var mockLogger = new Mock<ILogger<HomeController>>();

            var controller = new HomeController(mockDbContext.Object, mockLogger.Object);

            //Act
            var result = controller.StartPage() as ViewResult;

            //Assert
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
        }
        [Fact]
        public async Task IndexTest()
        {
            //Arrange
            var mockDbContext = new Mock<IDataContext<ToDoTask>>();
            var mockLogger = new Mock<ILogger<HomeController>>();
            var controller = new HomeController(mockDbContext.Object, mockLogger.Object);

            //Act
            var result = await controller.Index() as ViewResult;

            //Assert
            Assert.NotNull(result);
            var sortedTask = result.ViewData["NoSortTask"] as List<ToDoTask>;
            Assert.NotNull(sortedTask);
            

        }
        [Fact]
        public void SettingsPageTest()
        {
            //Arrange
            var mockDbContext = new Mock<IDataContext<ToDoTask>>();
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

            //Arrange
            var mockDbContext = new Mock<IDataContext<ToDoTask>>();
            var mockLogger = new Mock<ILogger<HomeController>>();

            var controller = new HomeController(mockDbContext.Object, mockLogger.Object);

            var expectedErrorMessage = "Error message to display in ViewBag";

            // Создаем TempData с ключом "ErrorMessage"
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            tempData["ErrorMessage"] = expectedErrorMessage;
            controller.TempData = tempData;
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            };

            //Act
            var result = controller.CreateTask() as ViewResult;

            //Assert
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);

            var actualErrorMessage = result.ViewData["ErrorMessage"] as string;
            Assert.Equal(expectedErrorMessage, actualErrorMessage);
        }
        [Fact]
        public void GeneralTasksPageTest()
        {
            //Arrange
            var mockDbContext = new Mock<IDataContext<ToDoTask>>();
            var mockLogger = new Mock<ILogger<HomeController>>();

            var controller = new HomeController(mockDbContext.Object, mockLogger.Object);

            //Act
            var result = controller.GeneralTasks() as ViewResult;

            //Assert
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
        }
        [Fact]
        public void ProfilePageTest()
        {
            //Arrange
            var mockDbContext = new Mock<IDataContext<ToDoTask>>();
            var mockLogger = new Mock<ILogger<HomeController>>();

            var controller = new HomeController(mockDbContext.Object, mockLogger.Object);

            //Act
            var result = controller.Profile() as ViewResult;

            //Assert
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
        }
    }
}
