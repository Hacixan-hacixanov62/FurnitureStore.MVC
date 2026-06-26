using MVC_FinalProject.Models.Basket;

namespace MVC_FinalProject.ViewModels
{
    public class BasketVM
    {
        public IEnumerable<SettingVM> Setting { get; set; }
        public Basket Basket { get; set; }
    }
}
