using MagicalVilla_CoponAPI.models;
using Microsoft.EntityFrameworkCore;

namespace MagicalVilla_CoponAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        {

        }

        public DbSet<Coupon>  Coupons { get; set; }

       protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Coupon>().HasData(new Coupon()
            {
                Id= 1,
                Created = DateTime.Now,
                IsActive= true,
                Name= "DDDF",
                Percent= 12
            },
            new Coupon()
            {
                Id= 2,
                Created = DateTime.Now,
                IsActive= true,
                Name="EEFFF",
                Percent= 55
            }
            )
                ;
        }

        

    }
}
