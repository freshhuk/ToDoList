using Microsoft.AspNetCore.Mvc;
using ToDoListWeb.Entity;

namespace ToDoListWeb.Controllers
{
    
    public class AccountController : Controller
    {
        private readonly TaskDbContex _dbContext;

        public AccountController(TaskDbContex dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IActionResult> Register()
        {
           
            // Создание базы данных, если она не существует
            
            await _dbContext.Database.EnsureCreatedAsync();
            

            return View();
        }
    }
}
