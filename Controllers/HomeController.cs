using Microsoft.AspNetCore.Mvc;

namespace Furn.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
