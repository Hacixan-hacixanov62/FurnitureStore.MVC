using System.ComponentModel.DataAnnotations;

namespace MVC_FinalProject.Models.SubscribeImg
{
    public class SubscribeImgCreate
    {
        [Required]
        public IFormFile Img { get; set; }
    }
}
