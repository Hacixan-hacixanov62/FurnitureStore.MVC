using System.ComponentModel.DataAnnotations;

namespace MVC_FinalProject.Models.AskUsFrom
{
    public class AskUsFromCreate
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Message { get; set; }
    }
}
