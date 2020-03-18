namespace CarsWebApp.Data.Configurations
{
    using CarsWebApp.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class BrandConfig : IEntityTypeConfiguration<Brand>
    {
        public void Configure(EntityTypeBuilder<Brand> builder)
        {
            builder.HasMany(x => x.Models)
                .WithOne(x => x.Brand)
                .HasForeignKey(x => x.BrandId);

            builder.HasMany(x => x.Cars)
                .WithOne(x => x.Brand)
                .HasForeignKey(x => x.BrandId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
