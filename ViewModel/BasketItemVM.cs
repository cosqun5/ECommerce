namespace Furn.ViewModel
{
	public class BasketItemVM
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public bool IsDeleted { get; set; }
		public double? Price { get; set; }
		public string ImagePath { get; set; }
		public int ProductCount { get; set; }
	}
}
