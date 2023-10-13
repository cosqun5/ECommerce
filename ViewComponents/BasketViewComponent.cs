using Furn.DAL;
using Furn.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Furn.ViewComponents
{
	public class BasketViewComponent : ViewComponent
	{
		private const string COOKIES_BASKET = "basketVM";
		private readonly AppDbContext _context;
		public BasketViewComponent(AppDbContext context)
		{
			_context = context;
		}
		public async Task<IViewComponentResult> InvokeAsync()
		{
			List<BasketItemVM> basketItemVMs = new List<BasketItemVM>();

			// Əvvəlcə Request.Cookies[COOKIES_BASKET] -in null olub-olmadığını yoxlayın
			if (Request.Cookies[COOKIES_BASKET] != null)
			{
				List<BasketVM> basketVMs = JsonConvert.DeserializeObject<List<BasketVM>>(Request.Cookies[COOKIES_BASKET]);
				foreach (BasketVM item in basketVMs)
				{
					BasketItemVM basketItemVM = _context.Products
														.Where(s => s.Id == item.ProductId)
														.Select(s => new BasketItemVM
														{
															Name = s.Name,
															Id = s.Id,
															Price = s.Price,
															ProductCount = item.Count,
															ImagePath = s.ImagePath
														}).FirstOrDefault();
					basketItemVMs.Add(basketItemVM);
				}
			}

			return View(basketItemVMs);
		}
	}
}
