using JuanBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace JuanBackend.DAL
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext>options):base(options)
        {

        }
        public DbSet<Slider>Sliders { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Product>Products { get; set; }
        public DbSet<Banner>Banners { get; set; }
        public DbSet<Seller>Sellers { get; set; }
        public DbSet<Blog>Blogs { get; set; }
        public DbSet<Brand>Brands { get; set; }
        public DbSet<Bio>Bios { get; set; }


    }
}
