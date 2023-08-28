using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Moq;
using ToDoListWeb.Controllers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ToDoListWebDomain.Domain.Entity;
using ToDoListWebInfrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace UnitTests
{
    
    public class TaskControllerTests
    {
        /*
        [Fact]
        public async Task GetTaskDb()
        {
            // Arrange создаем скажем так обьект нашего класа

            var mockDbContext = new Mock<IDataContext<ToDoTask>>();
            var loggerMock = new Mock<ILogger<TaskController>>();
            mockDbContext.ClearTestData();
            var controller = new TaskController(dbcontextMock, loggerMock.Object);

            //Act выполняет метод

            var result = await controller.GetTaskDb("Test", "Description", DateTime.Now);

            //Assert верифицирует результат теста, тоесть мы ставим некие условия для нашего теста 

            // Проверяем, что в базе данных была добавлена задача
            var tasksInDb = dbcontextMock.GetToDoTasks();
            Assert.Single(tasksInDb); // Проверяем, что в базе данных только одна задача

            // Добавьте дополнительные утверждения для проверки значений задачи, если необходимо
            var addedTask = tasksInDb.First();
            Assert.Equal("Test", addedTask.NameTask);
            Assert.Equal("Description", addedTask.DescriptionTask);
            Assert.Equal(DateTime.Now.Date, addedTask.TaskTime.Date);
            Assert.Equal("In progress", addedTask.Status);


        }
        */
    }
}
