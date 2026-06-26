namespace MVC_FinalProject.Models.Account
{
    public class UserRole
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }

        public string Email { get; set; }
        public List<string> Roles { get; set; }
    }
}
