using System.ComponentModel.DataAnnotations;

namespace MVC_FinalProject.Models.Account
{
    public class User
    {
        [Required]
        public string email { get; set; }
        [Required]
        public string token { get; set; }
        [Required]
        public string password { get; set; }
        [Required]
        public string confirmPassword { get; set; }
    }
}
