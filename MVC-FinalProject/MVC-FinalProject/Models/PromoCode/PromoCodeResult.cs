namespace MVC_FinalProject.Models.PromoCode
{
    public class PromoCodeResult
    {
        public string Code { get; set; }
        public int DiscountPercent { get; set; }
        public bool IsActive { get; set; }
        public int UsageLimit { get; set; }
        public int UsageCount { get; set; }
    }
}
