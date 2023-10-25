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
        
        [HttpPost("ChangeDataAccount")]
        public async Task<IActionResult> ChangeDataAccount(ChangeDataAccountModel _changemodel)
        {
            var user = await _userManager.GetUserAsync(User);
            try
            {
                
                if(user != null)
                {
                    if (!string.IsNullOrEmpty(_changemodel.NewLoginProp) && !string.IsNullOrEmpty(_changemodel.NewEmailProp) && !string.IsNullOrEmpty(_changemodel.PassWord) && !string.IsNullOrEmpty(_changemodel.NewPassword))
                    {
                        user.UserName = _changemodel.NewLoginProp;
                        user.Email = _changemodel.NewEmailProp;
                        user.NormalizedEmail = _userManager.NormalizeEmail(_changemodel.NewEmailProp);

                        var changePasswordResult = await _userManager.ChangePasswordAsync(user, _changemodel.PassWord, _changemodel.NewPassword);

                        // Обновление пользователя
                        var updateUserResult = await _userManager.UpdateAsync(user);
                        if (changePasswordResult.Succeeded && updateUserResult.Succeeded)
                        {
                            // Изменения успешно применены

                            // Обновление идентификационных данных пользователя с новым логином
                            (User.Identity as ClaimsIdentity)?.RemoveClaim((User.Identity as ClaimsIdentity)?.FindFirst(ClaimTypes.Name));
                            (User.Identity as ClaimsIdentity)?.AddClaim(new Claim(ClaimTypes.Name, _changemodel.NewLoginProp));

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
