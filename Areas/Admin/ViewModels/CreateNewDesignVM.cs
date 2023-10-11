using Microsoft.Build.Framework;

namespace Furn.Areas.Admin.ViewModels
{
    public class CreateNewDesignVM
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public IFormFile Photo { get; set; }
    }
}
