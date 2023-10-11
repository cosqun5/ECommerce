using Furn.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Furn.DAL
{
	public class AppDbContext: IdentityDbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
		{

		}
		public DbSet<Blog> Blogs { get; set; }
		public DbSet<Product> Products { get; set; }
		public DbSet<FeaturedProduct> FeaturedProducts { get; set; }
		public DbSet<Slider> Sliders { get; set; }
		public DbSet<Collection> Collections { get; set; }
		public DbSet <NewDesign> NewDesigns { get; set; } 

	}
}
