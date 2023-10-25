using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using ToDoListWebInfrastructure.Context;
using ToDoListWebDomain.Domain.Models;
using ToDoListWebInfrastructure.Interfaces;

namespace ToDoListWebServices.Authorization
{
    [Route("dataaccount")]
    public class DataAccountController : Controller
    {
        private readonly IDataContext<User> _userdbContext;
        private readonly ILogger<DataAccountController> _logger;
        private readonly UserManager<User> _userManager;

        public DataAccountController(IDataContext<User> userdbContext, ILogger<DataAccountController> logger, UserManager<User> userManager)
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
                            return Ok();
                        }
                        
                        else
                        {
                            // Обработка ошибок изменений
                            _logger.LogInformation(message: "Данные аккаунта были he изменены");
                            return BadRequest();
                        }
                        
                    }
                    else 
                    {
                        _logger.LogError(message: "Error data null");
                        TempData["ErrorMessage"] = "You have not completed all fields";
                        return BadRequest();
                    }
                }
                else
                {
                    return BadRequest();
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
