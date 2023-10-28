using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ToDoListWebDomain.Domain.Models;
using ToDoListWebInfrastructure.Context;

namespace ToDoListWebServices.Authorization
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class APIAccountController : Controller
    {
        private readonly TaskDbContex _dbContext;
        private readonly UserDbContext _userdbContext;
        private readonly ILogger<APIAccountController> _logger;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private IConfiguration _config;
        private const string Secret = "db3OIsj+BXE9NZDy0t8W3TcNekrF+2d/1sFnWG4HnV8TZY30iTOdtVWJG8abWvB1GlOgJuQZdcF2Luqm/hccMw==";

        public APIAccountController(IConfiguration config, TaskDbContex dbContext, UserDbContext userdbContext, SignInManager<User> signInManager, UserManager<User> userManager, ILogger<APIAccountController> logger)
        {
            _config = config;
            _dbContext = dbContext;
            _userdbContext = userdbContext;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }
        
        [HttpPost("LoginAccount")]
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
        public async Task<IActionResult> Register(UserRegistration model)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            try
            {
                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при создании пользователя");
                ModelState.AddModelError("", "Произошла ошибка при создании пользователя.");
                return BadRequest(ModelState);
            }
        }

        [HttpPost("LogoutAccount")]
        public async Task<IActionResult> Logout()
        {
            
            await _signInManager.SignOutAsync();
            return Ok();
        }

        private string CreateToken(string username)
        {

            List<Claim> claims = new()
            {                    
                //list of Claims - we only checking username - more claims can be added.
                new Claim("username", Convert.ToString(username)),
            };

            var key = new Convert.FromBase64String(Secret);
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: cred
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;

        }
}
