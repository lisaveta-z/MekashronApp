using System.ComponentModel.DataAnnotations;

namespace MekashronApp.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Username is required")]
        //[EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}