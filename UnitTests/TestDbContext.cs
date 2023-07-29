using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToDoListWeb.Interfaces;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ToDoListWeb.Entity;

namespace UnitTests
{
    internal class TestDbContext : IDataContext
    {
        public DbSet<ToDoTask> ToDoTask { get; set; }
        public List<ToDoTask> TestData = new List<ToDoTask>()
        {
           new ToDoTask() { Id = 0, NameTask ="trtrt,", DescriptionTask="dfdfefe", TaskTime = DateTime.Now, Status ="penis" },
           new ToDoTask() { Id = 1, NameTask ="trtrt,", DescriptionTask="dfdfefe", TaskTime = DateTime.Now, Status ="penis"},
           new ToDoTask() { Id = 2, NameTask ="trtrt,", DescriptionTask="dfdfefe", TaskTime = DateTime.Now, Status ="penis"},
           new ToDoTask() { Id = 3, NameTask ="trtrt,", DescriptionTask="dfdfefe", TaskTime = DateTime.Now, Status ="penis"},
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
        //метод для получения наших данных
        public List<ToDoTask> GetToDoTasks()
        {
            return TestData.OrderBy(t => t.Id).ToList();
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

    }
}
