using Furn.DAL;
using Furn.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Furn.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class ContactController : Controller
	{
		private readonly AppDbContext _context;

		public ContactController(AppDbContext context)
		{
			_context = context;
		}

		public async Task<IActionResult> Index()
		{
			ICollection<Message> messages = await _context.Messages.OrderByDescending(p => p.Id).ToListAsync();

			return View(messages);
		}
		public IActionResult Delete(int Id)
		{
			Message? message = _context.Messages.Find(Id);
			if (message == null)
			{
				return NotFound();
			}
			_context.Messages.Remove(message);
			_context.SaveChanges();
			return RedirectToAction(nameof(Index));
		}
	}
}
