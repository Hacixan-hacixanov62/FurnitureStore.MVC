using System.ComponentModel.DataAnnotations;

namespace MVC_FinalProject.Models.Subscription
{
    public class SubscriptionCreate
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
