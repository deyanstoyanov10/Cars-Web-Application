namespace CarsWebApp.Data.Configurations
{
    using CarsWebApp.Models;
    using CarsWebApp.Models.Enums;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class CarConfig : IEntityTypeConfiguration<Car>
    {
        public void Configure(EntityTypeBuilder<Car> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Brand)
                .WithMany(x => x.Cars)
                .HasForeignKey(x => x.BrandId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Model)
                .WithMany(x => x.Cars)
                .HasForeignKey(x => x.ModelId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.FirstExtras)
                .WithOne(x => x.Car)
                .HasForeignKey(x => x.CarId);

            builder.HasMany(x => x.SecondExtras)
                .WithOne(x => x.Car)
                .HasForeignKey(x => x.CarId);

            builder.HasMany(x => x.ThirdExtras)
                .WithOne(x => x.Car)
                .HasForeignKey(x => x.CarId);

            builder.Property(x => x.Price)
                .HasColumnType("decimal(15, 2)");

            builder.Property(x => x.Transmission)
                .HasDefaultValue(TransmissionType.Manual);

            builder.Property(x => x.HorsePower)
                .HasMaxLength(5000);

            builder.Property(x => x.EngineVolume)
                .HasMaxLength(30000);

            builder.Property(x => x.Metallic)
                .HasDefaultValue(false);

            builder.Property(x => x.EuroStandart)
                .HasDefaultValue(EuroStandartType.Euro0);

            builder.Property(x => x.Description)
                .HasMaxLength(2000);
        }
    }
}
