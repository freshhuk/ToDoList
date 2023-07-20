using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Moq;
using ToDoListWeb.Controllers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ToDoListWeb.Entity;
using Microsoft.AspNetCore.Mvc;

namespace UnitTests
{
    public class TaskControllerTests
    {
        [Fact]
        public async Task DeleteTaskTest()
        {
            // Arrange создаем скажем так обьект нашего класа

            var configurationMock = new Mock<IConfiguration>();
            var dbContextMock = new Mock<TaskDbContex>(configurationMock.Object); var loggerMock = new Mock<ILogger<TaskController>>();

            var controller = new TaskController(dbContextMock.Object, loggerMock.Object);

            //Act выполняет метод

            var result = await controller.DeleteTaskDBb(3) as RedirectResult;

            //Assert верифицирует результат теста, тоесть мы ставим некие условия для нашего теста 

            Assert.NotNull(result);
            
            
        }
    }
}
