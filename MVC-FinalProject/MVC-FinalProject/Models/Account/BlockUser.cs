using System.ComponentModel.DataAnnotations;

namespace MVC_FinalProject.Models.Account
{
    public class BlockUser
    {
        [Required]
        public string Username { get; set; }

        [Range(1, 1440, ErrorMessage = "Block duration must be between 1 and 1440 minutes")]
        public int BlockDurationMinutes { get; set; } = 5;
    }
}
