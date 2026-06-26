namespace MVC_FinalProject.Models.LandingBanner
{
    public class LandingBanner
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
