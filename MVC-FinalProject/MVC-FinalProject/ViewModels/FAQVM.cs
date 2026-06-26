using MVC_FinalProject.Models.AskUsFrom;
using MVC_FinalProject.Models.Category;

namespace MVC_FinalProject.ViewModels
{
    public class FAQVM
    {
        public IEnumerable<AskUsFrom> AskUsFroms { get; set; }
        public IEnumerable<SettingVM> Setting { get; set;}

    }
}
