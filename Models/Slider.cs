using System.ComponentModel.DataAnnotations;

namespace Furn.Models
{
	public class Slider
	{
		public int Id { get; set; }

		[Required, MaxLength(100)]

		public string Name { get; set; }

		[Required, MaxLength(500)]

		public string Description { get; set; }

		public double InitialPrice { get; set; }
		public double FinalPrice { get; set; }
		public string ImagePath { get; set; }
		public bool IsActive { get; set; }
	}
}
