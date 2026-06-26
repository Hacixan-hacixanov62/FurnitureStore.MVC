using MVC_FinalProject.Models.Account;
using MVC_FinalProject.Models.Order;

namespace MVC_FinalProject.ViewModels
{
    public class MyAccountVM
    {
        public IEnumerable<SettingVM> Setting { get; set; }
        public UpdateEmail UpdateEmail { get; set; } = new();     
        public UpdateUsername UpdateUsername { get; set; } = new();
        public List<Order> Orders { get; set; }
    }
}
