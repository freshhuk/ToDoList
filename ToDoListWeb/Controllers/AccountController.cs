using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using ToDoListWeb.Entity;
using ToDoListWeb.Models;

namespace ToDoListWeb.Controllers
{
    
    public class AccountController : Controller
    {
        private readonly TaskDbContex _dbContext;
        private readonly UserDbContext _userdbContext;
        private readonly ILogger<TaskController> _logger;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(TaskDbContex dbContext, UserDbContext userdbContext, SignInManager<User> signInManager, UserManager<User> userManager, ILogger<TaskController> logger)
        {
            _dbContext = dbContext;
            _userdbContext = userdbContext;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }
        //Для открытия формочки изминения данных
        [HttpGet]
        public IActionResult EditUserData()
        {
            return View();
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
        //сам метод для логина
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserLogin model)
        {

            if (ModelState.IsValid)
            {
                _logger.LogWarning(message: "Медель валидна");
                var loginResult = await _signInManager.PasswordSignInAsync(model.LoginProp,
                    model.Password,
                    false,
                    lockoutOnFailure: false);
                if (loginResult.Succeeded)
                {
                    await _dbContext.Database.EnsureCreatedAsync();
                    if (Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    return RedirectToAction("Index", "Home");
                }
                else if (loginResult.IsLockedOut)
                {
                    // Логика при заблокированной учетной записи
                    _logger.LogWarning("Учетная запись заблокирована.");
                    ModelState.AddModelError("", "Учетная запись заблокирована.");
                }
                else if (loginResult.IsNotAllowed)
                {
                    // Логика, если пользователь не разрешен для входа
                    _logger.LogWarning("Пользователь не разрешен для входа.");
                    ModelState.AddModelError("", "Пользователь не разрешен для входа.");
                }
                else
                {
                    // Логика при неудачной аутентификации
                    _logger.LogError("Ошибка аутентификации.");
                    ModelState.AddModelError("", "Ошибка аутентификации.");
                }
            }
            else
            {
                // Код при невалидной модели
                

                foreach (var modelState in ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        _logger.LogWarning(error.ErrorMessage);
                    }
                }

                ModelState.AddModelError("", "Пользователь не найден");
            }

            ModelState.AddModelError("", "Пользователь не найден");
            return View(model);

        }
        //открывает форму для регестрации
        
        [HttpGet]
        public async Task<IActionResult> Register()
        {
            await _userdbContext.Database.EnsureCreatedAsync();
            return View(new UserRegistration());
        }

         
        //сам метод регестрации
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(UserRegistration model)
        {
           

            //создаем екземпляр класа для отправки смс на почту
            if (ModelState.IsValid)
            {
                _logger.LogInformation(message: "Успешно1");
                var user = new User { UserName = model.LoginProp, Email = model.EmailProp };

                try
                {
                    var createResult = await _userManager.CreateAsync(user, model.Password);
                    if (createResult.Succeeded)
                    {                      
                        _logger.LogInformation(message: "Успешно 2x");
                        await _dbContext.Database.MigrateAsync();
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return RedirectToAction("Index", "Home");
                    }
                    if (!createResult.Succeeded)
                    {
                        foreach (var error in createResult.Errors)
                        {
                            _logger.LogError(error.Description);
                        }
                    }
                    else
                    {
                        _logger.LogInformation(message: "Регистрация прошла с ошибкой");
                        foreach (var identityError in createResult.Errors)
                        {
                            ModelState.AddModelError("", identityError.Description);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Ошибка при создании пользователя");
                    // Обработка исключения или возврат пользователю сообщения об ошибке
                    ModelState.AddModelError("", "Произошла ошибка при создании пользователя.");
                }
            }

            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            
            await _signInManager.SignOutAsync();
            return RedirectToAction("StartPage", "Home");
        }

    }
}
