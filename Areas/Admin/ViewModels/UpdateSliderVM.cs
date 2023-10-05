using System.ComponentModel.DataAnnotations;

namespace Furn.Areas.Admin.ViewModels
{
    public class UpdateSliderVM
    {
        public int Id { get; set; }
        [Required, MaxLength(100)]

        public string Name { get; set; }

        [Required, MaxLength(500)]

        public string Description { get; set; }
        [Required]

        public double InitialPrice { get; set; }
        [Required]
        public double FinalPrice { get; set; }
        public bool IsActive { get; set; }
        [Required]
        public IFormFile Photo { get; set; }
    }
}
