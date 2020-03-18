namespace CarsWebApp.Data
{
    using CarsWebApp.Data.Configurations;
    using CarsWebApp.Models;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    public class CarsDbContext : IdentityDbContext
    {
        public CarsDbContext(DbContextOptions<CarsDbContext> options)
            : base(options)
        {
        }

        public DbSet<Car> Cars { get; set; }

        public DbSet<Brand> Brands { get; set; }

        public DbSet<Model> Models { get; set; }

        public DbSet<Pictures> Pictures { get; set; }

        public DbSet<FirstExtras> FirstExtras { get; set; }

        public DbSet<SecondExtras> SecondExtras { get; set; }

        public DbSet<ThirdExtras> ThirdExtras { get; set; }

        public DbSet<Messages> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new CarConfig());
            builder.ApplyConfiguration(new BrandConfig());
            builder.ApplyConfiguration(new ModelConfig());

            base.OnModelCreating(builder);
        }
    }
}
