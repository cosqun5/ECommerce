using System.ComponentModel.DataAnnotations;

namespace Furn.Models
{
	public class FeaturedProduct
	{
        public int Id { get; set; }

		[Required,MaxLength(50)]
		public string Name { get; set; }
		public double Price { get; set; }	

		public string ImagePath { get; set; }
    }
}
