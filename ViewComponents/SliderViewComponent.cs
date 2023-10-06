using Furn.DAL;
using Furn.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Furn.ViewComponents
{
	public class SliderViewComponent :ViewComponent
	{
		private readonly AppDbContext _context;
		public SliderViewComponent(AppDbContext context)
		{
			_context = context;
		}
		public async Task<IViewComponentResult> InvokeAsync()
		{
			List<Slider> sliders = await _context.Sliders.ToListAsync();
			return View(sliders);


		}
	}
}
