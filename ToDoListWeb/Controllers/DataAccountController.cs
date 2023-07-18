using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ToDoListWeb.Entity;
using ToDoListWeb.Models;

namespace ToDoListWeb.Controllers
{
    public class DataAccountController : Controller
    {
        private readonly TaskDbContex _dbContext;
        private readonly ILogger<TaskController> _logger;


        public DataAccountController(TaskDbContex dbContext, ILogger<TaskController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> ChangeAccount(string LoginProp, string EmailProp,string PassWord)
        {
            
            try
            {
                if (!string.IsNullOrEmpty(LoginProp) && !string.IsNullOrEmpty(EmailProp) && !string.IsNullOrEmpty(PassWord))
                {

                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при создании пользователя");
                
            }
            return View();
        }

    }
}
