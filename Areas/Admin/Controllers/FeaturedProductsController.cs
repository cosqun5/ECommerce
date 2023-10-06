using Furn.Areas.Admin.Utilites.Extensions;
using Furn.Areas.Admin.ViewModels;
using Furn.DAL;
using Furn.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Furn.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class FeaturedProductsController : Controller
	{
		private readonly AppDbContext _context;
		private readonly IWebHostEnvironment _environment;
		public FeaturedProductsController(AppDbContext context, IWebHostEnvironment enviroment)
		{
			_context = context;
			_environment = enviroment;
		}
		public async Task<IActionResult> Index()
		{
			ICollection<FeaturedProduct> FeaturedProducts = await _context.FeaturedProducts.OrderByDescending(p => p.Id).ToListAsync();

			return View(FeaturedProducts);
		}
		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]

		public async Task<IActionResult> Create(CreateFeaturedProductVM FeaturedProductVM)
		{
			if (!ModelState.IsValid) return View(FeaturedProductVM);
			if (!FeaturedProductVM.Photo.CheckContentType("image/"))
			{
				ModelState.AddModelError("Photo", $"{FeaturedProductVM.Photo.FileName}-must be image type");
			}
			if (!FeaturedProductVM.Photo.CheckSize(200))
			{
				ModelState.AddModelError("Photo", $"{FeaturedProductVM.Photo.FileName} -must be imgae size 200kb");
			}
			string rootpath = Path.Combine(_environment.WebRootPath, "skydash", "images");
			string FileName = await FeaturedProductVM.Photo.SaveAsync(rootpath);
			FeaturedProduct FeaturedProduct = new FeaturedProduct()
			{
				Name = FeaturedProductVM.Name,
				Price = FeaturedProductVM.Price,
				ImagePath = FileName
			};
			await _context.FeaturedProducts.AddAsync(FeaturedProduct);
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}
		public async Task<IActionResult> Delete(int id)
		{
			FeaturedProduct FeaturedProduct = await _context.FeaturedProducts.FindAsync(id);
			if (FeaturedProduct == null) return NotFound();
			string roolpath = Path.Combine(_environment.WebRootPath, "skydash", "images", FeaturedProduct.ImagePath);
			if (System.IO.File.Exists(roolpath))
			{
				System.IO.File.Delete(roolpath);
			}
			_context.Remove(FeaturedProduct);

			await _context.SaveChangesAsync();

			return RedirectToAction("Index");

		}

		public IActionResult Update(int id)
		{
			if (!ModelState.IsValid) return View();
			FeaturedProduct FeaturedProduct = _context.FeaturedProducts.Find(id);
			if (FeaturedProduct == null)
			{
				return View("Error");
			}
			UpdateFeaturedProductVM updateTeamVM = new UpdateFeaturedProductVM
			{
				Id = id,
				Name = FeaturedProduct.Name,
				Price = FeaturedProduct.Price,
			};
			return View(updateTeamVM);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Update(UpdateFeaturedProductVM FeaturedProductVM)
		{
			if (!ModelState.IsValid) return View(FeaturedProductVM);
			if (!FeaturedProductVM.Photo.CheckContentType("image/"))
			{
				ModelState.AddModelError("Photo", $"{FeaturedProductVM.Photo.FileName} -must be imgae type");
			}
			if (!FeaturedProductVM.Photo.CheckSize(200))
			{
				ModelState.AddModelError("Photo", $"{FeaturedProductVM.Photo.FileName} -must be imgae size 200kb");
			}
			string rootpath = Path.Combine(_environment.WebRootPath, "skydash", "images");
			FeaturedProduct FeaturedProduct = await _context.FeaturedProducts.FindAsync(FeaturedProductVM.Id);
			string deletepath = Path.Combine(rootpath, FeaturedProduct.ImagePath);

			if (System.IO.File.Exists(deletepath))
			{
				System.IO.File.Delete(deletepath);
			}
			string FileName = await FeaturedProductVM.Photo.SaveAsync(rootpath);
			FeaturedProduct.ImagePath = FileName;
			FeaturedProduct.Name = FeaturedProductVM.Name;
			FeaturedProduct.Price = FeaturedProductVM.Price;
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));


		}
	}
}
