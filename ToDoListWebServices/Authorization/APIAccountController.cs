using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using ToDoListWebDomain.Domain.Models;
using ToDoListWebInfrastructure.Context;

namespace ToDoListWebServices.Authorization
{
    [ApiController]
    [Route("api/[controller]")]
    public class APIAccountController : Controller
    {
        private readonly TaskDbContex _dbContext;
        private readonly UserDbContext _userdbContext;
        private readonly ILogger<APIAccountController> _logger;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public APIAccountController(TaskDbContex dbContext, UserDbContext userdbContext, SignInManager<User> signInManager, UserManager<User> userManager, ILogger<APIAccountController> logger)
        {
            _dbContext = dbContext;
            _userdbContext = userdbContext;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }
        
        [HttpPost("LoginAccount")]
        [ValidateAntiForgeryToken]
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
                        return Ok(model.ReturnUrl);
                    }
                    return Ok();
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
                    return BadRequest();
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
                return BadRequest();
            }
            ModelState.AddModelError("", "Пользователь не найден");
            return Ok(model);
        }
        

         
        [HttpPost("Register")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(UserRegistration model)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _logger.LogInformation("Модель валидна");

            var user = new User { UserName = model.LoginProp, Email = model.EmailProp };

            try
            {
                var createResult = await _userManager.CreateAsync(user, model.Password);

                if (createResult.Succeeded)
                {
                    _logger.LogInformation("Успешно создан пользователь");
                    await _dbContext.Database.MigrateAsync();
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return Ok();
                }
                else
                {
                    _logger.LogInformation("Регистрация прошла с ошибкой");

                    foreach (var error in createResult.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }

                    return BadRequest(ModelState);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при создании пользователя");
                ModelState.AddModelError("", "Произошла ошибка при создании пользователя.");
                return BadRequest(ModelState);
            }
        }

        [HttpPost("LogoutAccount")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            
            await _signInManager.SignOutAsync();
            return Ok();
        }

    }
}
