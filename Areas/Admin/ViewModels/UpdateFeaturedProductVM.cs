using System.ComponentModel.DataAnnotations;

namespace Furn.Areas.Admin.ViewModels
{
	public class UpdateFeaturedProductVM
	{
		public int Id { get; set; }

		[Required, MaxLength(50)]
		public string Name { get; set; }
		public double Price { get; set; }

		public IFormFile Photo { get; set; }
	}
}
