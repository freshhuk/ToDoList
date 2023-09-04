using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDoListWebDomain.Domain.Models;
using ToDoListWebInfrastructure.Context;
using Microsoft.EntityFrameworkCore;

using ToDoListWebInfrastructure.Interfaces;

namespace ToListWebUI.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserDbContext _userdbContext;

        public AccountController(UserDbContext userdbContext)
        {
            _userdbContext = userdbContext;

        }
        #region AuthorizationPages
        //Для открытия формочки изминения данных
        [HttpGet]
        public IActionResult EditUserData()
        {
            return View();
        }
        //открывает форму для регестрации
        [AllowAnonymous]

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            await _userdbContext.Database.MigrateAsync();
            return View(new UserRegistration());
        }
        //метод открывает форму для логина
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            return View(new UserLogin()
            {
                ReturnUrl = !string.IsNullOrEmpty(returnUrl) ? returnUrl : "/"
            });
        }
        #endregion
    }
}
