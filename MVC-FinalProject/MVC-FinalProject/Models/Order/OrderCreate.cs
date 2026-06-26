namespace MVC_FinalProject.Models.Order
{
    public class OrderCreate
    {
        public string AppUserId { get; set; }
        public string StripeSessionId { get; set; }
        public string? PromoCode { get; set; }
    }
}
