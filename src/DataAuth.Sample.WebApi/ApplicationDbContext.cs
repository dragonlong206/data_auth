using DataAuth.Sample.WebApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAuth.Sample.WebApi
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Region> Regions { get; set; }
        public DbSet<Province> Provinces { get; set; }
        public DbSet<Store> Stores { get; set; }
    }
}
