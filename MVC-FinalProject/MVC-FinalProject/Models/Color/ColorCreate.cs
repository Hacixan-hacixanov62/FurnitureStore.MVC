using System.ComponentModel.DataAnnotations;

namespace MVC_FinalProject.Models.Color
{
    public class ColorCreate
    {
        [Required]
        [RegularExpression(@"^(?=.*[A-Za-z])[A-Za-z0-9\s%\-_,.()!]+$", ErrorMessage = "Color name must contain at least one letter and cannot be only numbers.")]
        public string Name { get; set; }
    }
}
