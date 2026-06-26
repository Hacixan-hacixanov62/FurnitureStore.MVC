using System.ComponentModel.DataAnnotations;

namespace MVC_FinalProject.Models.AboutBannerImg
{
    public class AboutBannerImgCreate
    {
        [Required]
        public IFormFile Img { get; set; }
    }
}
