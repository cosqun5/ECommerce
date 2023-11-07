using Furn.DAL;
using Furn.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Furn.Controllers
{
	public class BasketController : Controller
	{
		private const string COOKIES_BASKET = "basketVM";
		private readonly AppDbContext _context;
		public BasketController(AppDbContext context)
		{
			_context = context;
		}


        public IActionResult GetBasket()
        {
            var model = new List<BasketItemVM>(); // Sepet verilerini yükleyen bir işlev çağırılabilir

            return ViewComponent("Basket", model);
        }
        [HttpPost]
        public IActionResult AddBasket(int id)
        {
            // Əlavə etmək istədiyiniz məhsulun id-sini alın
            List<BasketVM> basket;

            if (Request.Cookies[COOKIES_BASKET] != null)
            {
                basket = JsonConvert.DeserializeObject<List<BasketVM>>(Request.Cookies[COOKIES_BASKET]);

            }
            else
            {
                basket = new List<BasketVM>();
            }

            // Basket-da bu məhsulu axtar
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

            // Yenidən cookies-i yazın
            Response.Cookies.Append(COOKIES_BASKET, JsonConvert.SerializeObject(basket), new CookieOptions
            {
                Expires = DateTimeOffset.Now.AddMinutes(30) // Expires'i istəyinizə görə tənzimləyin
            });

            // JSON cavabını qaytarın ki AJAX-a göndərə bilsin
            return Json(new { success = true, message = "Məhsul baskete əlavə edildi" });
        }
        public IActionResult RemoveBasket(int id)
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
				basket.Remove(cookiesBasket);
			}

			HttpContext.Response.Cookies.Append(COOKIES_BASKET, JsonConvert.SerializeObject(basket));
			return RedirectToAction("Index", "Home");
		}

	}
}
