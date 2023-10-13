using Furn.DAL;
using Furn.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Furn.ViewComponents
{
	public class FeatureViewComponent:ViewComponent
	{
		private readonly AppDbContext _appDbContext;

		public FeatureViewComponent(AppDbContext appDbContext)
		{
			_appDbContext = appDbContext;

		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			List<FeaturedProduct> collections = await _appDbContext.FeaturedProducts
				 .OrderByDescending(s => s.Id)
				.Take(4)
				.ToListAsync();
			return View(collections);
		}
	}
}
