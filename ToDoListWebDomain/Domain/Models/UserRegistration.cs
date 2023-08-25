using System.ComponentModel.DataAnnotations;

namespace ToDoListWebDomain.Domain.Models
{
    public class UserRegistration 
    {
        [Required, MaxLength(20)]
        public string LoginProp { get; set; }
        [Required, DataType(DataType.EmailAddress, ErrorMessage = "Please enter a valid email address")]
        [RegularExpression(@"^[\w-]+(\.[\w-]+)*@([\w-]+\.)+[a-zA-Z]{2,7}$", ErrorMessage = "Wrong email format")]
        public string EmailProp { get; set; }
        [Required, DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password), Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }


    }
}
