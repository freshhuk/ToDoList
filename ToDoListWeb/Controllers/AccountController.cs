using Microsoft.AspNetCore.Mvc;

namespace ToDoListWeb.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
