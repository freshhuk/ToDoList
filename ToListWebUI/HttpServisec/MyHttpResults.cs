using Microsoft.AspNetCore.Mvc;
using ToDoListWebDomain.Domain.Entity;

namespace ToListWebUI.HttpServisec
{
    public class MyHttpResultsController : Controller
    {
        private readonly APIToDoListHttpServices _apiHttpServisec;

        public MyHttpResultsController(APIToDoListHttpServices apiHttpServisec)
        {
            _apiHttpServisec = apiHttpServisec;
        }

        [HttpPost]
        public async Task<IActionResult> ResultAddTaskDbAsync(ToDoTask model)
        {
            /*
            var result = await _apiHttpServisec.AddTaskDbAsync(model);
            if(result == "successful")
            {
                return Redirect("~/Home/Index");
            }
            
            else
            {
                return Redirect("~/Home/Index");
            }*/
            return View("~/Views/Home/Index.cshtml");


        }
    }
}
