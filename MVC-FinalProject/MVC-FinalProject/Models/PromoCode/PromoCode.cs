using System.ComponentModel.DataAnnotations;

namespace MVC_FinalProject.Models.PromoCode
{
    public class PromoCode
    {
        public int Id { get; set; }
        [Required]
        public string Code { get; set; }

        [Range(1, 100)]
        public int DiscountPercent { get; set; }

        [Range(0, int.MaxValue)]
        public int UsageLimit { get; set; }

        public bool IsActive { get; set; }

        public int UsageCount { get; set; }
    }
}
