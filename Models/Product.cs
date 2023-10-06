using System.ComponentModel.DataAnnotations;

namespace Furn.Models
{
	public class Product
	{
		public int Id { get; set; }
		[Required, MaxLength(100)]

		public string Name { get; set; }

		public string Description { get; set; }
		public double Price { get; set; }
		public DateTime Created { get; set; }
		public string ImagePath { get; set; }
	}
}
