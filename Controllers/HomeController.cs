using Furn.DAL;
using Furn.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Furn.Controllers
{
	public class HomeController : Controller
	{
		private readonly AppDbContext _context;

		public HomeController(AppDbContext context)
		{
			_context = context;
		}

		//public async Task<IActionResult> Index(string searchString)
		//{
		//	var products = from product in _context.Products select product;
		//	if (!string.IsNullOrEmpty(searchString))
		//	{
		//		products = products.Where(u => u.Name.Contains(searchString));
		//	}
		//	return View(await products.ToListAsync());
		//}
		public IActionResult Index()
		{
			return View();
		}
		public async Task<IActionResult> Contact(Message m)
		{
			m.Tarix = Convert.ToDateTime(DateTime.Now.ToShortDateString());
			await _context.Messages.AddAsync(m);
			await _context.SaveChangesAsync();
			return RedirectToAction("Index");

		}
	}
}
