using System.ComponentModel.DataAnnotations;

namespace Furn.Areas.Admin.ViewModels
{
    public class CreateBlogVM
    {
        [Required, MaxLength(50)]
        public string Title { get; set; }
        [Required, MaxLength(500)]
        public string Description { get; set; }
        public IFormFile Photo { get; set; }
        public DateTime DateTime { get; set; }
    }
}
