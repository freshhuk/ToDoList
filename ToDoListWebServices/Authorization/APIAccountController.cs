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
using System.Text;
using ToDoListWebDomain.Domain.Models;
using ToDoListWebInfrastructure.Context;

namespace ToDoListWebServices.Authorization
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class APIAccountController : Controller
    {
        private readonly ILogger<APIAccountController> _logger;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private IConfiguration _config;

        public APIAccountController(IConfiguration config, SignInManager<User> signInManager, UserManager<User> userManager, ILogger<APIAccountController> logger)
        {
            _config = config;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [HttpPost("LoginAccount")]
        public async Task<IActionResult> Login(UserLogin model)
        {

            
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.LoginProp);
                if (user == null)
                {
                    return BadRequest("Uncorrect account data.");
                }

                var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
                if (result.Succeeded)
                {
                    // Успешная аутентификация, генерируем JWT-токен
                    var token = GenerateJwtToken(user);
                    _logger.LogInformation(message: $"Токен - {token}");
                    return Ok(new { token });
                }
                else
                {
                    return BadRequest("Неверные учетные данные.");
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
           
        }



        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] UserRegistration model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = new User()
                    {
                        UserName = model.LoginProp,
                        Email = model.EmailProp                       
                    };
                    
                    var result = await _userManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        // Успешно создан пользователь, генерируем JWT токен
                        var token = GenerateJwtToken(user);
                        _logger.LogInformation(message: $"token -- {token}");
                        return Ok(new { token });
                    }
                    
                    else
                    {
                        return BadRequest($"Register not Succeeded - {result.ToString()}");
                    }
                }
                else
                {
                    return BadRequest("User model not valid");
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
        public async Task<IActionResult> Logout()
        {

            await _signInManager.SignOutAsync();
            return Ok();
        }

        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var secret = _config["Jwt:Secret"];                    
            var key = Encoding.ASCII.GetBytes(secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
            {
             new Claim(ClaimTypes.Name, user.UserName),
                // Другие утверждения (claims) пользователя
            }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
