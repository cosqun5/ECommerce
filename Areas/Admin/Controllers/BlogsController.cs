using Furn.Areas.Admin.Utilites.Extensions;
using Furn.Areas.Admin.ViewModels;
using Furn.DAL;
using Furn.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Furn.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BlogsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;
        public BlogsController(AppDbContext context, IWebHostEnvironment enviroment)
        {
            _context = context;
            _environment = enviroment;
        }
        public async Task<IActionResult> Index()
        {
            ICollection<Blog> Blogs = await _context.Blogs.OrderByDescending(p => p.Id).ToListAsync();

            return View(Blogs);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(CreateBlogVM BlogVM)
        {
            if (!ModelState.IsValid) return View(BlogVM);
            if (!BlogVM.Photo.CheckContentType("image/"))
            {
                ModelState.AddModelError("Photo", $"{BlogVM.Photo.FileName}-must be image type");
            }
            if (!BlogVM.Photo.CheckSize(200))
            {
                ModelState.AddModelError("Photo", $"{BlogVM.Photo.FileName} -must be imgae size 200kb");
            }
            string rootpath = Path.Combine(_environment.WebRootPath, "skydash", "images");
            string FileName = await BlogVM.Photo.SaveAsync(rootpath);
            Blog Blog = new Blog()
            {
                Title = BlogVM.Title,
                Description = BlogVM.Description,
                DateTime = BlogVM.DateTime,
                ImagePath = FileName
            };
            await _context.Blogs.AddAsync(Blog);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int id)
        {
            Blog Blog = await _context.Blogs.FindAsync(id);
            if (Blog == null) return NotFound();
            string roolpath = Path.Combine(_environment.WebRootPath, "skydash", "images", Blog.ImagePath);
            if (System.IO.File.Exists(roolpath))
            {
                System.IO.File.Delete(roolpath);
            }
            _context.Remove(Blog);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");

        }
        public IActionResult Update(int id)
        {
            if (!ModelState.IsValid) return View();
            Blog Blog = _context.Blogs.Find(id);
            if (Blog == null)
            {
                return View("Error");
            }
            UpdateBlogVM updateTeamVM = new UpdateBlogVM
            {
                Id = id,
                Title = Blog.Title,
                Description = Blog.Description,
                DateTime = Blog.DateTime,
            };
            return View(updateTeamVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(UpdateBlogVM BlogVM)
        {
            if (!ModelState.IsValid) return View(BlogVM);
            if (!BlogVM.Photo.CheckContentType("image/"))
            {
                ModelState.AddModelError("Photo", $"{BlogVM.Photo.FileName} -must be imgae type");
            }
            if (!BlogVM.Photo.CheckSize(200))
            {
                ModelState.AddModelError("Photo", $"{BlogVM.Photo.FileName} -must be imgae size 200kb");
            }
            string rootpath = Path.Combine(_environment.WebRootPath, "skydash", "images");
            Blog Blog = await _context.Blogs.FindAsync(BlogVM.Id);
            string deletepath = Path.Combine(rootpath, Blog.ImagePath);

            if (System.IO.File.Exists(deletepath))
            {
                System.IO.File.Delete(deletepath);
            }
            string FileName = await BlogVM.Photo.SaveAsync(rootpath);
            Blog.ImagePath = FileName;
            Blog.Title = BlogVM.Title;
            Blog.Description = BlogVM.Description;
            Blog.DateTime = BlogVM.DateTime;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));


        }
    }
}
