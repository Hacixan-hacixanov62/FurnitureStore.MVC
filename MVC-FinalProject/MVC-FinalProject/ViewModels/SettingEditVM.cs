using System.ComponentModel.DataAnnotations;

namespace MVC_FinalProject.ViewModels
{
    public class SettingEditVM
    {
        [Required]
        public string Key { get; set; }
        [Required]
        public string Value { get; set; }
    }
}
