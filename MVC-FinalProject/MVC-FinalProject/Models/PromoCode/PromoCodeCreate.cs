using System.ComponentModel.DataAnnotations;

namespace MVC_FinalProject.Models.PromoCode
{
    public class PromoCodeCreate
    {
        [Required]
        public string Code { get; set; }
        [Required]
        [Range(5, int.MaxValue, ErrorMessage = "Discount percent must be at least 5")]
        public int DiscountPercent { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Usage limit must be at least 1")]
        public int UsageLimit { get; set; }
    }
}
