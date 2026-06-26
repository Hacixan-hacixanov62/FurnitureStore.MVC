using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MVC_FinalProject.Models.Task
{
    public class CreateTask
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        //public string AssignedTo { get; set; } 
        public string SelectedUsername { get; set; }
        public List<SelectListItem> AdminUsers { get; set; }
    }

    public class CreateTaskApi {
        public string Title { get; set; }
        public string Description { get; set; }
        public string AssignedTo { get; set; }
    }
}
