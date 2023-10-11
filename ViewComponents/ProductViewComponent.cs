using Furn.DAL;
using Furn.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Furn.ViewComponents
{
	public class ProductViewComponent : ViewComponent
	{
		private readonly AppDbContext _appDbContext;
		public ProductViewComponent(AppDbContext appDbContext)
		{
			_appDbContext = appDbContext;

		}
		public async Task<IViewComponentResult> InvokeAsync()
		{
			List<Product> products = await _appDbContext.Products
				.OrderByDescending(s => s.Id)
				.Take(8)
				.ToListAsync();
			return View(products);
		}
	}
}
