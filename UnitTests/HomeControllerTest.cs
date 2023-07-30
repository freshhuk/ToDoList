using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moq;
using ToDoListWeb.Controllers;
using ToDoListWeb.Interfaces;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;

namespace UnitTests
{
    public class HomeControllerTest
    {
        [Fact]
        public void ProfileTest()
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
    }
}
