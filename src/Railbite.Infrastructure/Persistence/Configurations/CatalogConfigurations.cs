using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Railbite.Domain.Entities;

namespace Railbite.Infrastructure.Persistence.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> b)
    {
        b.ToTable("Categories");
        b.HasKey(x => x.Id);
        b.Property(x => x.Name).IsRequired().HasMaxLength(100);
        b.Property(x => x.Description).HasMaxLength(500);
        b.HasIndex(x => x.Name).IsUnique();
    }
}

public class FoodConfiguration : IEntityTypeConfiguration<Food>
{
    public void Configure(EntityTypeBuilder<Food> b)
    {
        b.ToTable("Foods");
        b.HasKey(x => x.Id);
        b.Property(x => x.Name).IsRequired().HasMaxLength(150);
        b.Property(x => x.Description).HasMaxLength(500);
        b.Property(x => x.Price).HasColumnType("decimal(10,2)");
        b.Property(x => x.ImageUrl).HasMaxLength(500);
        b.HasIndex(x => x.Name);

        b.HasOne(x => x.Category)
            .WithMany(x => x.Foods)
            .HasForeignKey(x => x.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasOne(x => x.Restaurant)
            .WithMany(x => x.Foods)
            .HasForeignKey(x => x.RestaurantId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}

public class TrainConfiguration : IEntityTypeConfiguration<Train>
{
    public void Configure(EntityTypeBuilder<Train> b)
    {
        b.ToTable("Trains");
        b.HasKey(x => x.Id);
        b.Property(x => x.TrainNumber).IsRequired().HasMaxLength(20);
        b.Property(x => x.TrainName).IsRequired().HasMaxLength(150);
        b.HasIndex(x => x.TrainNumber).IsUnique();

        b.HasMany(x => x.Stops)
            .WithOne(x => x.Train)
            .HasForeignKey(x => x.TrainId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class TrainStopConfiguration : IEntityTypeConfiguration<TrainStop>
{
    public void Configure(EntityTypeBuilder<TrainStop> b)
    {
        b.ToTable("TrainStops");
        b.HasKey(x => x.Id);
        b.Property(x => x.StationName).IsRequired().HasMaxLength(150);
        b.HasIndex(x => new { x.TrainId, x.StopOrder }).IsUnique();
    }
}
