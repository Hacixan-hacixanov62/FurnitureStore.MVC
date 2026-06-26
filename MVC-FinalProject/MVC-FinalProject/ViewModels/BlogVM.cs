using MVC_FinalProject.Helpers;
using MVC_FinalProject.Models.BlogCategory;
using MVC_FinalProject.Models.BlogPost;
using MVC_FinalProject.Models.Category;
using MVC_FinalProject.Models.Product;

namespace MVC_FinalProject.ViewModels
{
    public class BlogVM
    {
        public IEnumerable<BlogCategory> BlogCategories { get; set; }
        public PaginationResponse<BlogPost> Paginate { get; set; }
        public IEnumerable<BlogPost> SearchResults { get; set; }
        public string SearchText { get; set; }
        public IEnumerable<SettingVM> Setting { get; set; }

    }
}
