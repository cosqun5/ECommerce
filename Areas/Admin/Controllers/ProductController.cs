
using Furn.Areas.Admin.Utilites.Extensions;
using Furn.Areas.Admin.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using Furn.Models;
using Furn.DAL;
using Microsoft.AspNetCore.Authorization;

namespace Furn.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;
        public ProductController(AppDbContext context, IWebHostEnvironment enviroment)
        {
            _context = context;
            _environment = enviroment;
        }
        public async Task<IActionResult> Index()
        {
            ICollection<Product> products = await _context.Products.OrderByDescending(p => p.Id).ToListAsync();

            return View(products);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(CreateProductVM productVM)
        {
            if(!ModelState.IsValid) return View(productVM);
            if (!productVM.Photo.CheckContentType("image/"))
            {
                ModelState.AddModelError("Photo",$"{productVM.Photo.FileName}-must be image type");
            }
            if (!productVM.Photo.CheckSize(200))
            {
                ModelState.AddModelError("Photo", $"{productVM.Photo.FileName} -must be imgae size 200kb");
            }
            string rootpath = Path.Combine(_environment.WebRootPath, "skydash", "images");
            string FileName = await productVM.Photo.SaveAsync(rootpath);
            Product product = new Product()
            {
                Name = productVM.Name,
                Description = productVM.Description,
                Price = productVM.Price,
                ImagePath = FileName
            };
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
		public async Task<IActionResult> Delete(int id)
		{
			Product product = await _context.Products.FindAsync(id);
			if (product == null) return NotFound();
			string roolpath = Path.Combine(_environment.WebRootPath, "skydash", "images", product.ImagePath);
			if (System.IO.File.Exists(roolpath))
			{
				System.IO.File.Delete(roolpath);
			}
			_context.Remove(product);

			await _context.SaveChangesAsync();

			return RedirectToAction("Index");

		}

		public IActionResult Update(int id)
		{
			if (!ModelState.IsValid) return View();
			Product product = _context.Products.Find(id);
			if (product == null)
			{
				return View("Error");
			}
			UpdateProductVM updateTeamVM = new UpdateProductVM
			{
				Id = id,
				Name = product.Name,
				Description = product.Description,
				Price = product.Price,
			};
			return View(updateTeamVM);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Update(UpdateProductVM productVM)
		{
			if (!ModelState.IsValid) return View(productVM);
			if (!productVM.Photo.CheckContentType("image/"))
			{
				ModelState.AddModelError("Photo", $"{productVM.Photo.FileName} -must be imgae type");
			}
			if (!productVM.Photo.CheckSize(200))
			{
				ModelState.AddModelError("Photo", $"{productVM.Photo.FileName} -must be imgae size 200kb");
			}
			string rootpath = Path.Combine(_environment.WebRootPath, "skydash", "images");
			Product product = await _context.Products.FindAsync(productVM.Id);
			string deletepath = Path.Combine(rootpath, product.ImagePath);

			if (System.IO.File.Exists(deletepath))
			{
				System.IO.File.Delete(deletepath);
			}
			string FileName = await productVM.Photo.SaveAsync(rootpath);
			product.ImagePath = FileName;
			product.Name = productVM.Name;
			product.Description = productVM.Description;
			product.Price = productVM.Price;
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));


		}

	}
}
