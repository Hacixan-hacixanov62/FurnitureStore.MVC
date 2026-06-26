using System.ComponentModel.DataAnnotations;

namespace MVC_FinalProject.Models.Account
{
    public class Login
    {
       [Required(ErrorMessage = "Username or Email is required")]
       public string EmailOrUserName { get; set; }
    
       [Required(ErrorMessage = "Password is required")]
       public string Password { get; set; }
    }
}
