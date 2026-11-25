using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Railbite.Domain.Entities;

namespace Railbite.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> b)
    {
        b.ToTable("Users");
        b.HasKey(x => x.Id);
        b.Property(x => x.Name).IsRequired().HasMaxLength(150);
        b.Property(x => x.Email).IsRequired().HasMaxLength(200);
        b.HasIndex(x => x.Email).IsUnique();
        b.Property(x => x.Phone).HasMaxLength(30);
        b.Property(x => x.PasswordHash).IsRequired();
        b.Property(x => x.Role).HasConversion<string>().HasMaxLength(30);
    }
}

public class PassengerConfiguration : IEntityTypeConfiguration<Passenger>
{
    public void Configure(EntityTypeBuilder<Passenger> b)
    {
        b.ToTable("Passengers");
        b.HasBaseType<User>();
        b.Property(x => x.Address).HasMaxLength(300);

        b.HasOne(x => x.Wallet)
            .WithOne(x => x.Passenger)
            .HasForeignKey<Wallet>(x => x.PassengerId)
            .OnDelete(DeleteBehavior.Cascade);

        b.HasOne(x => x.Cart)
            .WithOne(x => x.Passenger)
            .HasForeignKey<Cart>(x => x.PassengerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class RestaurantConfiguration : IEntityTypeConfiguration<Restaurant>
{
    public void Configure(EntityTypeBuilder<Restaurant> b)
    {
        b.ToTable("Restaurants");
        b.HasBaseType<User>();
        b.Property(x => x.RestaurantName).IsRequired().HasMaxLength(200);
        b.Property(x => x.Address).HasMaxLength(300);
    }
}

public class DeliveryPersonConfiguration : IEntityTypeConfiguration<DeliveryPerson>
{
    public void Configure(EntityTypeBuilder<DeliveryPerson> b)
    {
        b.ToTable("DeliveryPersons");
        b.HasBaseType<User>();
        b.Property(x => x.VehicleNumber).HasMaxLength(50);
    }
}
