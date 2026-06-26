namespace MVC_FinalProject.Models.PromoCode
{
    public class PromoCodeListItem
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public int DiscountPercent { get; set; }
        public int UsageLimit { get; set; }
        public bool IsActive { get; set; }
    }
}
