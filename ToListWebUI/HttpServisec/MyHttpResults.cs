using Microsoft.AspNetCore.Mvc;
using ToDoListWebDomain.Domain.Entity;

namespace ToListWebUI.HttpServisec
{
    [Route("myhttpresults")]
    public class MyHttpResultsController : Controller
    {
        private readonly APIToDoListHttpServices _apiHttpServisec;

        public MyHttpResultsController(APIToDoListHttpServices apiHttpServisec)
        {
            _apiHttpServisec = apiHttpServisec;
        }
        [HttpPost]
        [Route("resultaddtask")]
        public async Task<IActionResult> ResultAddTaskDbAsync(ToDoTask model)
        {

            var result = await _apiHttpServisec.AddTaskDbAsync(model);
            if(result == "successful")
            {
                return Redirect("~/Home/Index");
            }
            
            else if (result == "nosuccessful")
            {
                return Redirect("~/Home/Settings");
            }

            else
            {
                return Redirect("~/Home/StartPage");
            }


        }
    }
}
