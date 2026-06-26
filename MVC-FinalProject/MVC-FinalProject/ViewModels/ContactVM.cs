using MVC_FinalProject.Models.AskUsFrom;
using MVC_FinalProject.Models.HelpSection;

namespace MVC_FinalProject.ViewModels
{
    public class ContactVM
    {
        public IEnumerable<HelpSection> HelpSections { get; set; }
        public AskUsFromCreate AskUsFrom { get; set; }
        public IEnumerable<SettingVM> Setting { get; set; }

    }
}
