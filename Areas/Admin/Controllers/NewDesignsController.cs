using Furn.Areas.Admin.Utilites.Extensions;
using Furn.Areas.Admin.ViewModels;
using Furn.DAL;
using Furn.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Furn.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class NewDesignsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;
        public NewDesignsController(AppDbContext context, IWebHostEnvironment enviroment)
        {
            _context = context;
            _environment = enviroment;
        }
        public async Task<IActionResult> Index()
        {
            ICollection<NewDesign> NewDesigns = await _context.NewDesigns.OrderByDescending(p => p.Id).ToListAsync();

            return View(NewDesigns);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(CreateNewDesignVM NewDesignVM)
        {
            if (!ModelState.IsValid) return View(NewDesignVM);
            if (!NewDesignVM.Photo.CheckContentType("image/"))
            {
                ModelState.AddModelError("Photo", $"{NewDesignVM.Photo.FileName}-must be image type");
            }
            if (!NewDesignVM.Photo.CheckSize(200))
            {
                ModelState.AddModelError("Photo", $"{NewDesignVM.Photo.FileName} -must be imgae size 200kb");
            }
            string rootpath = Path.Combine(_environment.WebRootPath, "skydash", "images");
            string FileName = await NewDesignVM.Photo.SaveAsync(rootpath);
            NewDesign NewDesign = new NewDesign()
            {
                Name = NewDesignVM.Name,
                Description = NewDesignVM.Description,
                ImagePath = FileName
            };
            await _context.NewDesigns.AddAsync(NewDesign);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int id)
        {
            NewDesign NewDesign = await _context.NewDesigns.FindAsync(id);
            if (NewDesign == null) return NotFound();
            string roolpath = Path.Combine(_environment.WebRootPath, "skydash", "images", NewDesign.ImagePath);
            if (System.IO.File.Exists(roolpath))
            {
                System.IO.File.Delete(roolpath);
            }
            _context.Remove(NewDesign);

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");

        }

        public IActionResult Update(int id)
        {
            if (!ModelState.IsValid) return View();
            NewDesign NewDesign = _context.NewDesigns.Find(id);
            if (NewDesign == null)
            {
                return View("Error");
            }
            UpdateNewDesignVM updateTeamVM = new UpdateNewDesignVM
            {
                Id = id,
                Name = NewDesign.Name,
                Description = NewDesign.Description,
            };
            return View(updateTeamVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(UpdateNewDesignVM NewDesignVM)
        {
            if (!ModelState.IsValid) return View(NewDesignVM);
            if (!NewDesignVM.Photo.CheckContentType("image/"))
            {
                ModelState.AddModelError("Photo", $"{NewDesignVM.Photo.FileName} -must be imgae type");
            }
            if (!NewDesignVM.Photo.CheckSize(200))
            {
                ModelState.AddModelError("Photo", $"{NewDesignVM.Photo.FileName} -must be imgae size 200kb");
            }
            string rootpath = Path.Combine(_environment.WebRootPath, "skydash", "images");
            NewDesign NewDesign = await _context.NewDesigns.FindAsync(NewDesignVM.Id);
            string deletepath = Path.Combine(rootpath, NewDesign.ImagePath);

            if (System.IO.File.Exists(deletepath))
            {
                System.IO.File.Delete(deletepath);
            }
            string FileName = await NewDesignVM.Photo.SaveAsync(rootpath);
            NewDesign.ImagePath = FileName;
            NewDesign.Name = NewDesignVM.Name;
            NewDesign.Description = NewDesignVM.Description;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));


        }

    }
}
