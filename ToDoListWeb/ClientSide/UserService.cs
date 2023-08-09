using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using ToDoListWeb.Controllers;
using ToDoListWeb.Entity;
using ToDoListWeb.Models;

namespace ToDoListWeb.ClientSide
{
    [Authorize]
    public class UserService
    {
        private readonly UserDbContext _userdbContext;
        private readonly ILogger<UserService> _logger;
        private readonly UserManager<User> _userManager;
        public UserService(UserDbContext userDbContext, ILogger<UserService> logger, UserManager<User> userManager)
        {
            _logger = logger;
            _userdbContext = userDbContext;
            _userManager = userManager;
        }
        public async Task AddUserToHubAsync(string userLogin,  string hubId)
        {

        }
    }
}
