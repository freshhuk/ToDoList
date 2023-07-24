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
using ToDoListWeb.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace UnitTests
{
    class TestDbTasks : IDataContext
    {
        public DbSet<ToDoTask> ToDoTask { get; set; }
        public List<ToDoTask> TestData = new List<ToDoTask>()
        {
           new ToDoTask() { Id = 0, },
           new ToDoTask() { Id = 1, },
           new ToDoTask() { Id = 2, },
           new ToDoTask() { Id = 3, },
        };

        public async Task AddAsync<TEntity>(TEntity entity) where TEntity : class
        {
            TestData.Add(entity as ToDoTask);
            await Task.CompletedTask; // Имитируем асинхронное выполнение
        }

        public async Task SaveChangesAsync()
        {
            await Task.CompletedTask; // Имитируем асинхронное выполнение
        }

        // Метод для поиска задачи по Id
        public async Task<ToDoTask> FindAsync(int Id)
        {
            return await Task.FromResult(TestData.FirstOrDefault(task => task.Id == Id));
        }

        // Метод для удаления задачи по Id
        public async Task RemoveAsync(int Id)
        {
            var taskToRemove = await FindAsync(Id);
            if (taskToRemove != null)
            {
                TestData.Remove(taskToRemove);
            }
        }
        public void ClearTestData()
        {
            TestData.Clear();
        }

        // Метод для получения всех тестовых задач из имитации базы данных
        public List<ToDoTask> GetToDoTasks()
        {
            return TestData;
        }
    }
    public class TaskControllerTests
    {
        [Fact]
        public async Task GetTaskDb()
        {
            // Arrange создаем скажем так обьект нашего класа

            var dbcontextMock = new TestDbTasks();
            var loggerMock = new Mock<ILogger<TaskController>>();
            dbcontextMock.ClearTestData();
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
    }
}
