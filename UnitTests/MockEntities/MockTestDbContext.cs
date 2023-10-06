using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoListWebDomain.Domain.Entity;
using ToDoListWebInfrastructure.Interfaces;

namespace UnitTests.MockEntities
{
    internal class MockTestDbContext : IDataContext<ToDoTask>
    {
        private List<ToDoTask> dataStore = new List<ToDoTask>() 
        {
            new ToDoTask(){Id = 0, NameTask ="test", DescriptionTask="test", Status="in progress", TaskTime = DateTime.Now} 
        };

        public async Task AddAsync(ToDoTask item)
        {
            dataStore.Add(item);
        }

        public void Delete(int id)
        {
            var item = dataStore.SingleOrDefault(entity => GetId(entity) == id);
            if (item != null)
            {
                dataStore.Remove(item);
            }
        }

        public ToDoTask Get(int id)
        {
            var item = dataStore.SingleOrDefault(entity => GetId(entity) == id);
            if (item == null)
            {
                throw new InvalidOperationException("Item not found.");
            }
            return item;
        }

        public IEnumerable<ToDoTask> GetAll()
        {
            return dataStore;
        }

        public async Task SaveChangesAsync()
        {
            // В данном случае, для мока базы данных, сохранение изменений может быть пустым методом,
            // так как нет реальной базы данных.
            await Task.CompletedTask;
        }

        public async Task Update(ToDoTask item)
        {
            var id = GetId(item);
            var existingItem = dataStore.SingleOrDefault(entity => GetId(entity) == id);
            if (existingItem != null)
            {
                // Обновляем поля существующего элемента
                UpdateItem(existingItem, item);
            }
        }

        // Метод для получения уникального идентификатора элемента
        private int GetId(ToDoTask item)
        {
            return item.Id;
        }

        // Метод для обновления полей существующего элемента
        private void UpdateItem(ToDoTask existingItem, ToDoTask newItem)
        {

            existingItem.Id = newItem.Id;
            existingItem.NameTask = newItem.NameTask;
            existingItem.DescriptionTask = newItem.DescriptionTask;
            existingItem.Status = newItem.Status;
            existingItem.TaskTime = newItem.TaskTime;
        }
    }
}
