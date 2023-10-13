using Furn.DAL;
using Furn.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Furn.ViewComponents
{
	public class NewDesignViewComponent:ViewComponent
	{
		private readonly AppDbContext _appDbContext;
		public NewDesignViewComponent(AppDbContext appDbContext)
		{
			_appDbContext = appDbContext;

		}
		public async Task<IViewComponentResult> InvokeAsync()
		{
			List<NewDesign> designs = await _appDbContext.NewDesigns
				.OrderByDescending(s => s.Id)
				.Take(4)
				.ToListAsync();
			return View(designs);
		}
	}
}
