using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDoListWebDomain.Domain.Models;
using ToDoListWebInfrastructure.Context;
using Microsoft.EntityFrameworkCore;
using ToListWebUI.HttpServisec;
using ToDoListWebInfrastructure.Interfaces;

namespace ToListWebUI.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserDbContext _userdbContext;
        private readonly AuthorizationHttpServisec _authorizationHttpServisec;
        private readonly ILogger<AccountController> _logger;
        public AccountController(UserDbContext userdbContext, AuthorizationHttpServisec authorizationHttpServisec, ILogger<AccountController> logger)
        {
            _userdbContext = userdbContext;
            _authorizationHttpServisec = authorizationHttpServisec;
            _logger = logger;
        }
        #region AuthorizationPages
        //Для открытия формочки изминения данных
        [HttpGet]
        public IActionResult EditUserData()
        {
            return View();
        }
        //открывает форму для регестрации

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            await _userdbContext.Database.MigrateAsync();
            return View(new UserRegistration());
        }
        //метод пост который использует наш сервис http для отправки пост запроса на сервер регестрации
        [HttpPost]
        public async Task<IActionResult> Register(UserRegistration model)
        {
            if (ModelState.IsValid)
            {
                var result = await _authorizationHttpServisec.RegisterUserAsync(model);

                if (result == "Регистрация успешна.")
                {
                    _logger.LogInformation(message: "Регистрация успешна.");
                    // Регистрация успешна, выполните необходимые действия (например, перенаправление пользователя)
                    return View("~/Views/Home/Index.cshtml");
                }
                else
                {
                    _logger.LogError(message: "Регистрация не удалась");
                    // Регистрация не удалась, отобразите ошибку
                    ModelState.AddModelError("", result);
                }
            }

            // Если ModelState не валидно, верните форму с ошибками
            return View(model);
        }
        //метод открывает форму для логина
        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            return View(new UserLogin()
            {
                ReturnUrl = !string.IsNullOrEmpty(returnUrl) ? returnUrl : "/"
            });
        }

        //метод пост который использует наш сервис http для отправки пост запроса на сервер регестрации
        [HttpPost]
        public async Task<IActionResult> Login(UserLogin model)
        {
            if (ModelState.IsValid)
            {
                var result = await _authorizationHttpServisec.LoginUserAsync(model);

                if (result == "successful")
                {
                    // Регистрация успешна, выполните необходимые действия (например, перенаправление пользователя)
                    return View("~/Views/Home/Index.cshtml");
                    
                }
                else
                {
                    // Регистрация не удалась, отобразите ошибку
                    ModelState.AddModelError("", result);
                }
            }

            // Если ModelState не валидно, верните форму с ошибками
            return View(model);
        }
        #endregion
    }
}
