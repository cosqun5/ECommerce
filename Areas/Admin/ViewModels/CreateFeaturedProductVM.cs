using System.ComponentModel.DataAnnotations;

namespace Furn.Areas.Admin.ViewModels
{
	public class CreateFeaturedProductVM
	{

		[Required,MaxLength(100)]
		public string Name { get; set; }
		public double Price { get; set; }

		[Required]
		public IFormFile Photo { get; set; }
	}
}
