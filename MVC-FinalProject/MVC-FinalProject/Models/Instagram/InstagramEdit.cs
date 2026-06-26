namespace MVC_FinalProject.Models.Instagram
{
    public class InstagramEdit
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public IFormFile Img { get; set; }
    }
}
