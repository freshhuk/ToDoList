using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
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

        private readonly IHubContext<TasksHub> _hubContext;
        public UserService(IHubContext<TasksHub> hubContext, UserDbContext userDbContext, ILogger<UserService> logger, UserManager<User> userManager)
        {
            _hubContext = hubContext;
            _logger = logger;
            _userdbContext = userDbContext;
            _userManager = userManager;
        }


        [HttpPost]
        public async Task InviteUserToHubByLoginAsync( string invitedUserLogin)
        {
            // Проверка, существует ли пользователь, которого мы хотим пригласить
            var invitedUser = await _userManager.FindByNameAsync(invitedUserLogin);
            if (invitedUser == null)
            {
                throw new Exception("Пользователь с таким логином не найден.");
            }

            // Создание объекта HubParticipant и добавление его к хабу
            var participant = new HubParticipant
            {
                UserLogin = invitedUser.UserName,
                HubId = "MyHub" // Здесь используйте идентификатор вашего хаба
            };

            TasksHub.HubParticipants.Add(participant);

            // Отправка сообщения об успешном приглашении
            await _hubContext.Clients.User(invitedUser.Id).SendAsync("InvitationAccepted");
        }
    }
}
