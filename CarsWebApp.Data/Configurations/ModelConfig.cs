namespace CarsWebApp.Data.Configurations
{
    using CarsWebApp.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class ModelConfig : IEntityTypeConfiguration<Model>
    {
        public void Configure(EntityTypeBuilder<Model> builder)
        {
            builder.HasOne(x => x.Brand)
                .WithMany(x => x.Models)
                .HasForeignKey(x => x.BrandId);

            builder.HasMany(x => x.Cars)
                .WithOne(x => x.Model)
                .HasForeignKey(x => x.ModelId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
