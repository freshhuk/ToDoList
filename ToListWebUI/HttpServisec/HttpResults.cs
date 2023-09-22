using Microsoft.AspNetCore.Mvc;
using ToDoListWebDomain.Domain.Entity;

namespace ToListWebUI.HttpServisec
{
    public class HttpResults : Controller
    {
        private readonly APIToDoListHttpServices _apiHttpServisec;

        public HttpResults(APIToDoListHttpServices apiHttpServisec)
        {
            _apiHttpServisec = apiHttpServisec;
        }

        [HttpPost]
        public async Task<IActionResult> ResultAddTaskDbAsync(ToDoTask model)
        {
            var result = await _apiHttpServisec.AddTaskDbAsync(model);
            if(result == "successful")
            {
                return Redirect("~/Home/Index");
            }
            /*
            else
            {
                return Redirect("~/Home/Index");
            }
            */
            return Redirect("https://localhost:44339/Home/Index");
        }
    }
}
