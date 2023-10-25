using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using ToDoListWebDomain.Domain.Entity;
using ToDoListWebDomain.Domain.Models;
using ToDoListWebServices.Authorization;

namespace ToListWebUI.HttpServisec
{
    [Route("myhttpresults")]
    public class MyHttpResultsController : Controller
    {
        private readonly APIToDoListHttpServices _apiHttpServisec;
        private readonly AuthorizationHttpServisec _authorizationHttpServisec;
        private readonly ILogger<MyHttpResultsController> _logger;
        public MyHttpResultsController(AuthorizationHttpServisec authorizationHttpServisec, APIToDoListHttpServices apiHttpServisec,  ILogger<MyHttpResultsController> logger)
        {
            _apiHttpServisec = apiHttpServisec;
            _authorizationHttpServisec = authorizationHttpServisec;
            _logger = logger;
        }
        #region API Results


        [HttpPost]
        [Route("resultaddtask")]
        public async Task<IActionResult> ResultAddTaskDbAsync(string TaskName, string TaskDescription, DateTime TaskTime, string TaskStatus)
        {
            var model = new ToDoTask()
            {
                NameTask = TaskName,
                DescriptionTask = TaskDescription,
                TaskTime = TaskTime.Date,
                Status = TaskStatus
            };
            _logger.LogInformation($"Sending task to APIHttpServices: {JsonSerializer.Serialize(model)}");
            var result = await _apiHttpServisec.AddTaskDbAsync(model);
            if (result == "successful")
            {
                return Redirect("~/Home/Index");
            }
            else if (result == "nosuccessful")
            {
                //error
                return Redirect("~/Home/Index");
            }
            else
            {
                return Redirect("~/Home/StartPage");
            }
        }
        [HttpPost]
        [Route("resultdeletetaskdb")]
        public async Task<IActionResult> ResultDeleteTaskDbAsync([FromForm] int Id)
        {
            var result = await _apiHttpServisec.DeleteTaskDbAsync(Id);
            if (result == "successful")
            {
                return Redirect("~/Home/Index");
            }

            else if (result == "nosuccessful")
            {
                //error
                return Redirect("~/Home/Index");
            }

            else
            {
                return Redirect("~/Home/StartPage");
            }
        }

        [HttpPost]
        [Route("resultchangetaskdb")]
        public async Task<IActionResult> ResultChangeTaskDbAsync([FromForm] int _Id, string _TaskName, string _TaskDescription, DateTime _TimeTask, string _TaskStatus)
        {
            var chamgemodel = new ChangeTaskModel()
            {
                Id = _Id,
                TaskName = _TaskName,
                TaskDescription = _TaskDescription,
                TaskStatus = _TaskStatus,
                TaskData = _TimeTask.Date,
            };
            _logger.LogInformation($"Sending task to APIHttpServices: {JsonSerializer.Serialize(chamgemodel)}");

            var result = await _apiHttpServisec.ChangeTaskDbAsync(chamgemodel);
            if (result == "successful")
            {
                return Redirect("~/Home/Index");
            }

            else if (result == "nosuccessful")
            {
                //error
                return Redirect("~/Home/Index");
            }
            else
            {
                return Redirect("~/Home/StartPage");
            }
        }
        #endregion

        #region AuthorizationServices
        [HttpPost]
        [Route("ResultRegisterUser")]
        public async Task<IActionResult> ResultRegisterUserAsync(string _LoginProp, string _Password, string _Email, string _ConfirmPassword)
        {
            var model = new UserRegistration()
            {
                LoginProp =  _LoginProp,
                Password = _Password,
                EmailProp = _Email, 
                ConfirmPassword = _ConfirmPassword
                
            };
            _logger.LogInformation($"Sending task to APIHttpServices: {JsonSerializer.Serialize(model)}");
            var result = await _authorizationHttpServisec.RegisterUserAsync(model);
            if (result == "successful")
            {
                return Redirect("~/Home/Index");
            }
            else if (result == "nosuccessful")
            {
                //error
                return Redirect("~/Home/Settings");
            }
            else
            {
                return Redirect("~/Home/StartPage");
            }
        }
        [HttpPost]
        [Route("ResultLoginUser")]
        public async Task<IActionResult> ResultLoginUserAsync(string _LoginProp, string _Password, string _ReturnUrl)
        {
            var model = new UserLogin()
            {
                LoginProp = _LoginProp,
                Password = _Password,
                ReturnUrl = _ReturnUrl
            };
            _logger.LogInformation($"Sending task to APIHttpServices: {JsonSerializer.Serialize(model)}");
            var result = await _authorizationHttpServisec.LoginUserAsync(model);
            if (result == "successful")
            {
                return Redirect("~/Home/Index");
            }
            else if (result == "nosuccessful")
            {
                //error
                return Redirect("~/Home/Index");
            }
            else
            {
                return Redirect("~/Home/StartPage");
            }
        }
        [HttpPost]
        [Route("ResultLogout")]
        public async Task<IActionResult> ResultLogoutAsync()
        {
            var result = await _authorizationHttpServisec.LogoutAsync();
            if (result == "successful")
            {
                return Redirect("~/Home/StartPage");
            }
            else if (result == "nosuccessful")
            {
                //error
                return Redirect("~/Home/Settings");
            }
            else
            {
                return Redirect("~/Home/Index");
            }
        }

        [HttpPost("ResultChangeDataAccount")]
        public async Task<IActionResult> ResultChangeDataAccountAsync(string _NewLoginProp, string _NewEmailProp, string _PassWord, string _NewPassword)
        {
            var model = new ChangeDataAccountModel() 
            {
                NewEmailProp = _NewLoginProp ,
                NewLoginProp = _NewLoginProp,
                PassWord = _PassWord,
                NewPassword = _NewPassword
            };
            var result = await _authorizationHttpServisec.ChangeDataAccountAsync(model);
            if (result == "successful")
            {
                return Redirect("~/Home/StartPage");
            }
            else if (result == "nosuccessful")
            {
                //error
                return Redirect("~/Home/Settings");
            }
            else
            {
                return Redirect("~/Home/Index");
            }
        }
        #endregion

    }
}
