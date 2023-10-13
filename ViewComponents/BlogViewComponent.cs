using Furn.DAL;
using Furn.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Furn.ViewComponents
{
	public class BlogViewComponent:ViewComponent
	{
		private readonly AppDbContext _appDbContext;

		public BlogViewComponent(AppDbContext appDbContext)
		{
			_appDbContext = appDbContext;

		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			List<Blog> blogs = await _appDbContext.Blogs
				 .OrderByDescending(b => b.Id)
				.Take(3)
				.ToListAsync();
			return View(blogs);
		}
	}
}
