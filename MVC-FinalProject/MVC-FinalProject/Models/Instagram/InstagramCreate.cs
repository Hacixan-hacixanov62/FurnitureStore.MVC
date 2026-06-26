using System.ComponentModel.DataAnnotations;

namespace MVC_FinalProject.Models.Instagram
{
    public class InstagramCreate
    {
        [Required]
        public IFormFile Img { get; set; }
    }
}
