using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using ToDoListWeb.Entity;
using ToDoListWeb.Models;

namespace ToDoListWeb.Controllers
{
    public class DataAccountController : Controller
    {
        private readonly UserDbContext _userdbContext;
        private readonly ILogger<TaskController> _logger;
        private readonly UserManager<User> _userManager;

        public DataAccountController(UserDbContext userdbContext, ILogger<TaskController> logger, UserManager<User> userManager)
        {
            _userManager = userManager;
            _userdbContext = userdbContext;
            _logger = logger;
        }
        
        [HttpPost]
        public async Task<IActionResult> ChangeDataAccount(string NewLoginProp, string NewEmailProp,string PassWord, string NewPassword)
        {
            var user = await _userManager.GetUserAsync(User);
            try
            {
                
                if(user != null)
                {
                    if (!string.IsNullOrEmpty(NewLoginProp) && !string.IsNullOrEmpty(NewEmailProp) && !string.IsNullOrEmpty(PassWord) && !string.IsNullOrEmpty(NewPassword))
                    {
                        user.UserName = NewLoginProp;
                        user.Email = NewEmailProp;
                        user.NormalizedEmail = _userManager.NormalizeEmail(NewEmailProp);

                        var changePasswordResult = await _userManager.ChangePasswordAsync(user, PassWord, NewPassword);

                        // Обновление пользователя
                        var updateUserResult = await _userManager.UpdateAsync(user);
                        if (changePasswordResult.Succeeded && updateUserResult.Succeeded)
                        {
                            // Изменения успешно применены

                            // Обновление идентификационных данных пользователя с новым логином
                            (User.Identity as ClaimsIdentity)?.RemoveClaim((User.Identity as ClaimsIdentity)?.FindFirst(ClaimTypes.Name));
                            (User.Identity as ClaimsIdentity)?.AddClaim(new Claim(ClaimTypes.Name, NewLoginProp));
                            TempData["CurrentUserEmail"] = user.Email;

                            // Перезапись идентификационных данных в текущем контексте аутентификации
                            await HttpContext.SignOutAsync();
                            await HttpContext.SignInAsync(User);

                            // Обновление данных пользователя в базе данных
                            await _userManager.UpdateAsync(user);

                            _logger.LogInformation(message: "Данные аккаунта были изменены");
                            await _userdbContext.SaveChangesAsync();
                            return Redirect("~/Home/Profile");
                        }
                        
                        else
                        {
                            // Обработка ошибок изменений
                            _logger.LogInformation(message: "Данные аккаунта были he изменены");
                            return Redirect("~/Home/Index");
                        }
                        
                    }
                    else 
                    {
                        _logger.LogError(message: "Error data null");
                        TempData["ErrorMessage"] = "You have not completed all fields";
                        return RedirectToAction("ChangeTaskPage", "Home");
                    }
                }
                else
                {
                    return Redirect("~/Home/Index");
                }
                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при создании пользователя");
                return Redirect("~/Home/Index");
            }
        }

    }
}
