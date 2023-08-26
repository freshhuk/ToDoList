using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;
using ToDoListWebDomain.Domain.Entity;


namespace ToDoListWebServices.ClientSide
{
    [Authorize]
    public class TasksHub : Hub
    {
        public static List<HubParticipant> HubParticipants = new List<HubParticipant>();

        public override async Task OnConnectedAsync()
        {
            var userLogin = Context.UserIdentifier;
            var participant = HubParticipants.FirstOrDefault(p => p.UserLogin == userLogin);

            if (participant != null)
            {
                // Добавьте пользователя к группе хаба
                await Groups.AddToGroupAsync(Context.ConnectionId, participant.HubId);
            }

            await base.OnConnectedAsync();
        }
        public async Task SendNewTask(ToDoTask toDoTask)
        {
            await Clients.Caller.SendAsync("Receive", toDoTask);
        }
    }
}
