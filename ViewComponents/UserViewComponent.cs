using Furn.DAL;
using Furn.Models;
using Furn.Models.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Furn.ViewComponents
{
	public class UserViewComponent:ViewComponent
	{
		private readonly AppDbContext _context;
        public UserViewComponent(AppDbContext context)
        {
            _context = context;
        }
		public async Task<IViewComponentResult> InvokeAsync()
		{
			List<AppUser> users = await _context.Users.ToListAsync();
			return View(users);


		}
	}
}
