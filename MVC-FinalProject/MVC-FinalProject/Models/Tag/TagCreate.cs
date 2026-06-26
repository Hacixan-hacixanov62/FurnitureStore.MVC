using System.ComponentModel.DataAnnotations;

namespace MVC_FinalProject.Models.Tag
{
    public class TagCreate
    {

        [Required]
        [RegularExpression(@"^(?=.*[A-Za-z])[A-Za-z0-9\s%\-_,.()!]+$", ErrorMessage = "Name must contain at least one letter and cannot be only numbers.")]
        public string Name { get; set; }
    }
}
