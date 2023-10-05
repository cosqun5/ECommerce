using System.ComponentModel.DataAnnotations;

namespace Furn.Areas.Admin.ViewModels
{
    public class CreateProductVM
    {
        [Required, MaxLength(100)]
        public string Name { get; set; }

        [Required, MaxLength(300)]
        public string Description { get; set; }
        public double Price { get; set; }

        [Required]
        public IFormFile Photo { get; set; }
    }
}
