using Business.Concrate;
using DataAccess.Concrate;
using DataAccess.EntityFramework;
using Microsoft.AspNetCore.Mvc;

namespace Furn.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class BlogController : Controller
	{
		private readonly Context _context;
		BlogManager blogManager = new BlogManager(new EfBlog());
		public  IActionResult Index()
		{
			var values = blogManager.TGetList();
			return View(values);
		}
	}
}
