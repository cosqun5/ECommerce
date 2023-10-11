using System.ComponentModel.DataAnnotations;

namespace Furn.Areas.Admin.ViewModels
{
	public class CreateCollectionVM
	{
		[Required,MaxLength(100)]
		public string Title { get; set; }
		[Required,MaxLength(500)]
		public string Description { get; set; }
		public IFormFile Photo { get; set; }
		public double Price { get; set; }
	}
}
