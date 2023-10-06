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
			List<BasketItemVM> basketItemVMs =  new List<BasketItemVM>();
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
			return View(basketItemVMs);
		}
		public IViewComponentResult AddBasket( int id)
		{

			List<BasketVM> basket;
			if (Request.Cookies[COOKIES_BASKET] != null)
			{
				basket = JsonConvert.DeserializeObject<List<BasketVM>>(Request.Cookies[COOKIES_BASKET]);
			}
			else
			{
				basket = new List<BasketVM> { };
			}
			BasketVM cookiesBasket = basket.Where(s => s.ProductId == id).FirstOrDefault();
			if (cookiesBasket != null)
			{
				cookiesBasket.Count++;
			}
			else
			{

				BasketVM basketVM = new BasketVM()
				{
					ProductId = id,
					Count = 1
				};
				basket.Add(basketVM);
			}

			HttpContext.Response.Cookies.Append(COOKIES_BASKET, JsonConvert.SerializeObject(basket));
			return View();
		}
	}
}
