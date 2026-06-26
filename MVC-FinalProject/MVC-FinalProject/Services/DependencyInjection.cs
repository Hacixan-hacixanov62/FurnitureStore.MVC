using MVC_FinalProject.Services;
using MVC_FinalProject.Services.Interfaces;

namespace Service
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServiceLayer(this IServiceCollection services)
        {         
            services.AddScoped<ISliderService, SliderService>();         
            services.AddScoped<ICategoryService, CategoryService>();         
            services.AddScoped<IAboutBannerImgService, AboutBannerImgService >();         
            services.AddScoped<ILandingBannerService, LandingBannerService>();         
            services.AddScoped<IInstagramService, InstagramService>();         
            services.AddScoped<ISubscribeImgService, SubscribeImgService>();         
            services.AddScoped<IBrandService, BrandService>();         
            services.AddScoped<IColorService, ColorService>();         
            services.AddScoped<ITagService, TagService>();         
            services.AddScoped<IBlogCategoryService, BlogCategoryService>();         
            services.AddScoped<IBlogPostService, BlogPostService>();         
            services.AddScoped<IProductService, ProductService>();         
            services.AddScoped<IAccountService, AccountService>();         
            services.AddScoped<ISubscriptionService, SubscriptionService>();         
            services.AddScoped<IAboutPromoService, AboutPromoService>();         
            services.AddScoped<IHelpSectionService, HelpSectionService>();         
            services.AddScoped<IAskUsFromService, AskUsFromService>();         
            services.AddScoped<IReviewService, ReviewService>();         
            services.AddScoped<IBlogReviewService, BlogReviewService>();         
            services.AddScoped<IBasketService, BasketService>();
            services.AddScoped<ISettingService, SettingService>();
            services.AddScoped<IWishlistService, WishlistService>();
            services.AddScoped<IPromoCodeService, PromoCodeService>();
            services.AddScoped<ITaskService, TaskService>();
            services.AddScoped<IOrderService, OrderService>();
            return services;
        }
    }
}