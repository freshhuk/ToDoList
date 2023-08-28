using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToDoListWebInfrastructure.Interfaces;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ToDoListWebDomain.Domain.Entity;

namespace UnitTests
{
    internal class MockToDoTask
    {
        public int Id { get; set; }
        public string NameTask { get; set; }
        public string DescriptionTask { get; set; }
        public string Status { get; set; }
        [DataType(DataType.Date)]
        [Column(TypeName = "date")]
        public DateTime TaskTime { get; set; }
    }

    internal class TestDbContext : DbContext, IDataContext<ToDoTask>
    {
        
        public List<ToDoTask> _testData = new List<ToDoTask>()
        {
           new ToDoTask() { Id = 0, NameTask ="trtrt,", DescriptionTask="dfdfefe", TaskTime = DateTime.Now, Status ="penis" },
           new ToDoTask() { Id = 1, NameTask ="trtrt,", DescriptionTask="dfdfefe", TaskTime = DateTime.Now, Status ="penis"},
           new ToDoTask() { Id = 2, NameTask ="trtrt,", DescriptionTask="dfdfefe", TaskTime = DateTime.Now, Status ="penis"},
           new ToDoTask() { Id = 3, NameTask ="trtrt,", DescriptionTask="dfdfefe", TaskTime = DateTime.Now, Status ="penis"},
        };

        public async Task AddAsync(ToDoTask item)
        {
            _testData.Add(item);
        }

        public void Delete(int id)
        {
            var task = _testData.Find(t => t.Id == id);
            if (task != null)
                _testData.Remove(task);
        }

        public ToDoTask Get(int id)
        {
            var task = _testData.SingleOrDefault(t => t.Id == id);
            if (task == null)
            {
                throw new InvalidOperationException("Task not found.");
            }
            return task;
        }

        public IEnumerable<ToDoTask> GetAll()
        {
            return _testData.OrderBy(t => t.Id);
        }

        public Task SaveChangesAsync()
        {
            // Здесь можно выполнить дополнительные действия, если необходимо
            return Task.CompletedTask;
        }

        public async Task Update(ToDoTask item)
        {
            var existingTask = _testData.FirstOrDefault(t => t.Id == item.Id);
            if (existingTask != null)
            {
                // Производим обновление полей существующей задачи
                existingTask.NameTask = item.NameTask;
                existingTask.DescriptionTask = item.DescriptionTask;
                existingTask.Status = item.Status;
                existingTask.TaskTime = item.TaskTime;
            }
        }
        public void ClearTestData()
        {
            _testData.Clear();
        }

    }
}
