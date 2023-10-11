using Furn.DAL;
using Furn.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Furn.ViewComponents
{
    public class CollectionViewComponent: ViewComponent
    {
        private readonly AppDbContext _appDbContext;
        public CollectionViewComponent(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;

        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<Collection> collections = await _appDbContext.Collections.ToListAsync();
            return View(collections);
        }
    }
}
