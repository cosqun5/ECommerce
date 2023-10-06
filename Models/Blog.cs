using System.ComponentModel.DataAnnotations;

namespace Furn.Models
{
	public class Blog
	{
		public int Id { get; set; }
		[Required, MaxLength(50)]
		public string Title { get; set; }
		[Required, MaxLength(500)]
		public string Description { get; set; }
		public string ImagePath { get; set; }
		public DateTime DateTime { get; set; }
	}
}
