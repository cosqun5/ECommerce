using Furn.Areas.Admin.Utilites.Extensions;
using Furn.Areas.Admin.ViewModels;
using Furn.DAL;
using Furn.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Furn.Areas.Admin.Controllers
{

		[Area("Admin")]
		public class SlidersController : Controller
		{
			private readonly AppDbContext _context;
			private readonly IWebHostEnvironment _environment;
			public SlidersController(AppDbContext context, IWebHostEnvironment enviroment)
			{
				_context = context;
				_environment = enviroment;
			}
			public async Task<IActionResult> Index()
			{
				ICollection<Slider> sliders = await _context.Sliders.OrderByDescending(p => p.Id).ToListAsync();

				return View(sliders);
			}

			public IActionResult Create()
			{
				return View();
			}

			[HttpPost]
			[ValidateAntiForgeryToken]

			public async Task<IActionResult> Create(CreateSliderVM SliderVM)
			{
				if (!ModelState.IsValid) return View(SliderVM);
				if (!SliderVM.Photo.CheckContentType("image/"))
				{
					ModelState.AddModelError("Photo", $"{SliderVM.Photo.FileName}-must be image type");
				}
				if (!SliderVM.Photo.CheckSize(200))
				{
					ModelState.AddModelError("Photo", $"{SliderVM.Photo.FileName} -must be imgae size 200kb");
				}
				string rootpath = Path.Combine(_environment.WebRootPath, "skydash", "images");
				string FileName = await SliderVM.Photo.SaveAsync(rootpath);
				Slider Slider = new Slider()
				{
					Name = SliderVM.Name,
					Description = SliderVM.Description,
					InitialPrice = SliderVM.InitialPrice,
					FinalPrice = SliderVM.FinalPrice,
					IsActive = SliderVM.IsActive,
					ImagePath = FileName
				};
				await _context.Sliders.AddAsync(Slider);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			public async Task<IActionResult> Delete(int id)
			{
				Slider slider = await _context.Sliders.FindAsync(id);
				if (slider == null) return NotFound();
				string roolpath = Path.Combine(_environment.WebRootPath, "skydash", "images", slider.ImagePath);
				if (System.IO.File.Exists(roolpath))
				{
					System.IO.File.Delete(roolpath);
				}
				_context.Remove(slider);
				await _context.SaveChangesAsync();
				return RedirectToAction("Index");

			}
			public IActionResult Update(int id)
			{
				if (!ModelState.IsValid) return View();
				Slider slider = _context.Sliders.Find(id);
				if (slider == null)
				{
					return View("Error");
				}
				UpdateSliderVM updateTeamVM = new UpdateSliderVM
				{
					Id = id,
					Name = slider.Name,
					Description = slider.Description,
					InitialPrice = slider.InitialPrice,
					FinalPrice = slider.FinalPrice,
					IsActive = slider.IsActive,
				};
				return View(updateTeamVM);
			}

			[HttpPost]
			[ValidateAntiForgeryToken]
			public async Task<IActionResult> Update(UpdateSliderVM sliderVM)
			{
				if (!ModelState.IsValid) return View(sliderVM);
				if (!sliderVM.Photo.CheckContentType("image/"))
				{
					ModelState.AddModelError("Photo", $"{sliderVM.Photo.FileName} -must be imgae type");
				}
				if (!sliderVM.Photo.CheckSize(200))
				{
					ModelState.AddModelError("Photo", $"{sliderVM.Photo.FileName} -must be imgae size 200kb");
				}
				string rootpath = Path.Combine(_environment.WebRootPath, "skydash", "images");
				Slider Slider = await _context.Sliders.FindAsync(sliderVM.Id);
				string deletepath = Path.Combine(rootpath, Slider.ImagePath);

				if (System.IO.File.Exists(deletepath))
				{
					System.IO.File.Delete(deletepath);
				}
				string FileName = await sliderVM.Photo.SaveAsync(rootpath);
				Slider.ImagePath = FileName;
				Slider.Name = sliderVM.Name;
				Slider.Description = sliderVM.Description;
				Slider.InitialPrice = sliderVM.InitialPrice;
				Slider.FinalPrice = sliderVM.FinalPrice;
				Slider.IsActive = sliderVM.IsActive;
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));


			}

		}
	}

