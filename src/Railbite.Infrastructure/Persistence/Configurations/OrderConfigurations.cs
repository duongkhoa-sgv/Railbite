using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Railbite.Domain.Entities;

namespace Railbite.Infrastructure.Persistence.Configurations;

public class TicketConfiguration : IEntityTypeConfiguration<Ticket>
{
    public void Configure(EntityTypeBuilder<Ticket> b)
    {
        b.ToTable("Tickets");
        b.HasKey(x => x.Id);
        b.Property(x => x.PnrNumber).IsRequired().HasMaxLength(30);
        b.Property(x => x.Coach).HasMaxLength(10);
        b.Property(x => x.SeatNumber).HasMaxLength(10);
        b.HasIndex(x => x.PnrNumber);

        b.HasOne(x => x.Passenger)
            .WithMany(x => x.Tickets)
            .HasForeignKey(x => x.PassengerId)
            .OnDelete(DeleteBehavior.NoAction);

        b.HasOne(x => x.Train)
            .WithMany(x => x.Tickets)
            .HasForeignKey(x => x.TrainId)
            .OnDelete(DeleteBehavior.NoAction);

        b.HasOne(x => x.FromStop)
            .WithMany()
            .HasForeignKey(x => x.FromStopId)
            .OnDelete(DeleteBehavior.NoAction);

        b.HasOne(x => x.ToStop)
            .WithMany()
            .HasForeignKey(x => x.ToStopId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> b)
    {
        b.ToTable("Orders");
        b.HasKey(x => x.Id);
        b.Property(x => x.OrderNumber).IsRequired().HasMaxLength(30);
        b.HasIndex(x => x.OrderNumber).IsUnique();
        b.Property(x => x.Status).HasConversion<string>().HasMaxLength(30);
        b.Property(x => x.TotalAmount).HasColumnType("decimal(18,2)");
        b.Property(x => x.DeliveryAddress).HasMaxLength(300);
        b.Property(x => x.Notes).HasMaxLength(500);

        b.HasOne(x => x.Passenger)
            .WithMany(x => x.Orders)
            .HasForeignKey(x => x.PassengerId)
            .OnDelete(DeleteBehavior.NoAction);

        b.HasOne(x => x.Ticket)
            .WithMany(x => x.Orders)
            .HasForeignKey(x => x.TicketId)
            .OnDelete(DeleteBehavior.NoAction);

        b.HasOne(x => x.Restaurant)
            .WithMany(x => x.Orders)
            .HasForeignKey(x => x.RestaurantId)
            .OnDelete(DeleteBehavior.NoAction);

        b.HasOne(x => x.DeliveryPerson)
            .WithMany(x => x.AssignedOrders)
            .HasForeignKey(x => x.DeliveryPersonId)
            .OnDelete(DeleteBehavior.NoAction);

        b.HasMany(x => x.Items)
            .WithOne(x => x.Order)
            .HasForeignKey(x => x.OrderId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> b)
    {
        b.ToTable("OrderItems");
        b.HasKey(x => x.Id);
        b.Property(x => x.FoodName).IsRequired().HasMaxLength(150);
        b.Property(x => x.UnitPrice).HasColumnType("decimal(10,2)");

        b.HasOne(x => x.Food)
            .WithMany(x => x.OrderItems)
            .HasForeignKey(x => x.FoodId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}

public class DeliveryConfiguration : IEntityTypeConfiguration<Delivery>
{
    public void Configure(EntityTypeBuilder<Delivery> b)
    {
        b.ToTable("Deliveries");
        b.HasKey(x => x.Id);
        b.HasIndex(x => x.OrderId).IsUnique();

        b.HasOne(x => x.Order)
            .WithOne(x => x.Delivery)
            .HasForeignKey<Delivery>(x => x.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        b.HasOne(x => x.DeliveryPerson)
            .WithMany(x => x.Deliveries)
            .HasForeignKey(x => x.DeliveryPersonId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
