using System.ComponentModel.DataAnnotations;

namespace ToDoListWeb.Models
{
    public class UserLogin
    {
        [Required, MaxLength(20)]
        public string LoginProp { get; set; }
        [Required, DataType(DataType.Password)]
        public string Password { get; set; }
        public string ReturnUrl { get; set; }


    }
}
