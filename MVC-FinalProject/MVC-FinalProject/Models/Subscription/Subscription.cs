namespace MVC_FinalProject.Models.Subscription
{
    public class Subscription
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
