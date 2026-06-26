namespace MVC_FinalProject.Models.AskUsFrom
{
    public class AskUsFrom
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public bool IsApproved { get; set; }
    }
}
