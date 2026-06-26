namespace MVC_FinalProject.Models.Order
{
    public class OrderConfirmationEmailDto
    {
        public string ToEmail { get; set; }
        public string FullName { get; set; }
        public List<OrderedProductDto> Products { get; set; }
        public decimal Total { get; set; }
        public decimal? DiscountPercent { get; set; }
        public string PromoCode { get; set; }
    }
}
