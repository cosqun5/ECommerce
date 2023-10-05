using Entities.Concrate;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrate
{
    public class Context : IdentityDbContext
    {
    

        public Context(DbContextOptions<Context> options): base(options) 
        {
            
        }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Product> Products { get; set; }     
        public DbSet<Blog> Blogs { get; set; }  
    }
}
