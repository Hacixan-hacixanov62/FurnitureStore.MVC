using System.ComponentModel.DataAnnotations;

namespace MVC_FinalProject.Models.Account
{
    public class AdminMessage
    {
        public string Email { get; set; }
        [Required]
        public string Subject { get; set; }
        [Required]
        public string Body { get; set; }
    }
}
