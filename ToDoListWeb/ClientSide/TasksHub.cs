using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;
using ToDoListWeb.Entity;

namespace ToDoListWeb.ClientSide
{
    [Authorize]
    public class TasksHub : Hub
    {
        public async Task SendNewTask(ToDoTask toDoTask)
        {
            await Clients.Caller.SendAsync("Receive", toDoTask);
        }
    }
}
