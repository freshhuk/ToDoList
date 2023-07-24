using Microsoft.AspNetCore.Mvc;

namespace ToDoListWeb.Interfaces
{
    public interface ITaskController
    {
        Task<IActionResult> GetTaskDb(string TaskName, string TaskDescription, DateTime TaskData);
        Task<IActionResult> DeleteTaskDBb(int Id);
        Task<IActionResult> ChangeTaskDb(int Id, string TaskName, string TaskDescription, DateTime TaskData, string TaskStatus);


    }
}
