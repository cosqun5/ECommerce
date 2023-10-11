using System.ComponentModel.DataAnnotations;

namespace Furn.Areas.Admin.ViewModels
{
    public class UpdateNewDesignVM
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public IFormFile Photo { get; set; }
    }
}
