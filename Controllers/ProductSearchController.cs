using Furn.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Furn.Controllers
{
	public class ProductSearchController : Controller
	{
		private readonly AppDbContext _context;

		public ProductSearchController(AppDbContext context)
		{
			_context = context;
		}

		public async Task<IActionResult> Index(string searchString)
		{
			var products = from product in _context.Products select product;
			if (!string.IsNullOrEmpty(searchString))
			{
				products = products.Where(u => u.Name.Contains(searchString));
			}
			return View(await products.ToListAsync());
		}
	}
}
