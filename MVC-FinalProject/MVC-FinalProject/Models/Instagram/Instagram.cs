namespace MVC_FinalProject.Models.Instagram
{
    public class Instagram
    {
        public int Id { get; set; }
        public string Img { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
