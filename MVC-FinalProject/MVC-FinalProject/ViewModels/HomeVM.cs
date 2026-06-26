using MVC_FinalProject.Models.Category;
using MVC_FinalProject.Models.Instagram;
using MVC_FinalProject.Models.LandingBanner;
using MVC_FinalProject.Models.Product;
using MVC_FinalProject.Models.Slider;
using MVC_FinalProject.Models.SubscribeImg;
using MVC_FinalProject.Models.Subscription;

namespace MVC_FinalProject.ViewModels
{
    public class HomeVM
    {
        public IEnumerable<Slider> Sliders { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<LandingBanner> LandingBanners { get; set; }
        public IEnumerable<Instagram> Instagrams { get; set; }
        public IEnumerable<SubscribeImg> SubscribeImgs { get; set; }
        public SubscriptionCreate Subscribe { get; set; } = new();
        public IEnumerable<Product> SearchResults { get; set; }
        public IEnumerable<SettingVM> Settings { get; set; }

    }
}
