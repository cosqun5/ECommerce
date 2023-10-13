using Furn.Areas.Admin.Utilites.Extensions;
using Furn.Areas.Admin.ViewModels;
using Furn.DAL;
using Furn.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Furn.Areas.Admin.Controllers
{
	[Area("Admin")]
    [Authorize]

    public class CollectionsController : Controller
		{
			private readonly AppDbContext _context;
			private readonly IWebHostEnvironment _environment;
			public CollectionsController(AppDbContext context, IWebHostEnvironment enviroment)
			{
				_context = context;
				_environment = enviroment;
			}
			public async Task<IActionResult> Index()
			{
				ICollection<Collection> Collections = await _context.Collections.OrderByDescending(p => p.Id).ToListAsync();

				return View(Collections);
			}

			public IActionResult Create()
			{
				return View();
			}

			[HttpPost]
			[ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(CreateCollectionVM collectionVM)
        {
            if (!ModelState.IsValid) return View(collectionVM);
            if (!collectionVM.Photo.CheckContentType("image/"))
            {
                ModelState.AddModelError("Photo", $"{collectionVM.Photo.FileName}-must be image type");
            }
            if (!collectionVM.Photo.CheckSize(200))
            {
                ModelState.AddModelError("Photo", $"{collectionVM.Photo.FileName} -must be imgae size 200kb");
            }
            string rootpath = Path.Combine(_environment.WebRootPath, "skydash", "images");
            string FileName = await collectionVM.Photo.SaveAsync(rootpath);
            Collection collection = new Collection()
            {
                Title = collectionVM.Title,
                Description = collectionVM.Description,
                Price = collectionVM.Price,
                ImagePath = FileName
            };
            await _context.Collections.AddAsync(collection);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
			{
				Collection Collection = await _context.Collections.FindAsync(id);
				if (Collection == null) return NotFound();
				string roolpath = Path.Combine(_environment.WebRootPath, "skydash", "images", Collection.ImagePath);
				if (System.IO.File.Exists(roolpath))
				{
					System.IO.File.Delete(roolpath);
				}
				_context.Remove(Collection);
				await _context.SaveChangesAsync();
				return RedirectToAction("Index");

			}
			public IActionResult Update(int id)
			{
				if (!ModelState.IsValid) return View();
				Collection Collection = _context.Collections.Find(id);
				if (Collection == null)
				{
					return View("Error");
				}
				UpdateCollectionVM updateTeamVM = new UpdateCollectionVM
				{
					Id = id,
					Title = Collection.Title,
					Description = Collection.Description,
					Price = Collection.Price,
				};
				return View(updateTeamVM);
			}

			[HttpPost]
			[ValidateAntiForgeryToken]
			public async Task<IActionResult> Update(UpdateCollectionVM CollectionVM)
			{
				if (!ModelState.IsValid) return View(CollectionVM);
				if (!CollectionVM.Photo.CheckContentType("image/"))
				{
					ModelState.AddModelError("Photo", $"{CollectionVM.Photo.FileName} -must be imgae type");
				}
				if (!CollectionVM.Photo.CheckSize(200))
				{
					ModelState.AddModelError("Photo", $"{CollectionVM.Photo.FileName} -must be imgae size 200kb");
				}
				string rootpath = Path.Combine(_environment.WebRootPath, "skydash", "images");
				Collection Collection = await _context.Collections.FindAsync(CollectionVM.Id);
				string deletepath = Path.Combine(rootpath, Collection.ImagePath);

				if (System.IO.File.Exists(deletepath))
				{
					System.IO.File.Delete(deletepath);
				}
				string FileName = await CollectionVM.Photo.SaveAsync(rootpath);
				Collection.ImagePath = FileName;
				Collection.Title = CollectionVM.Title;
				Collection.Description = CollectionVM.Description;
			    Collection.Price = CollectionVM.Price;
		

				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));


			}
		}
	}
