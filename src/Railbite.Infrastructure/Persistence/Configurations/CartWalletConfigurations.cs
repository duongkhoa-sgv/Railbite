using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Railbite.Domain.Entities;

namespace Railbite.Infrastructure.Persistence.Configurations;

public class CartConfiguration : IEntityTypeConfiguration<Cart>
{
    public void Configure(EntityTypeBuilder<Cart> b)
    {
        b.ToTable("Carts");
        b.HasKey(x => x.Id);
        b.HasIndex(x => x.PassengerId).IsUnique();

        b.HasMany(x => x.Items)
            .WithOne(x => x.Cart)
            .HasForeignKey(x => x.CartId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
{
    public void Configure(EntityTypeBuilder<CartItem> b)
    {
        b.ToTable("CartItems");
        b.HasKey(x => x.Id);
        b.Property(x => x.UnitPrice).HasColumnType("decimal(10,2)");

        b.HasOne(x => x.Food)
            .WithMany(x => x.CartItems)
            .HasForeignKey(x => x.FoodId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}

public class WalletConfiguration : IEntityTypeConfiguration<Wallet>
{
    public void Configure(EntityTypeBuilder<Wallet> b)
    {
        b.ToTable("Wallets");
        b.HasKey(x => x.Id);
        b.Property(x => x.Balance).HasColumnType("decimal(18,2)");
        b.HasIndex(x => x.PassengerId).IsUnique();

        b.HasMany(x => x.Transactions)
            .WithOne(x => x.Wallet)
            .HasForeignKey(x => x.WalletId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> b)
    {
        b.ToTable("Transactions");
        b.HasKey(x => x.Id);
        b.Property(x => x.Amount).HasColumnType("decimal(18,2)");
        b.Property(x => x.Type).HasConversion<string>().HasMaxLength(20);
        b.Property(x => x.Description).HasMaxLength(300);
    }
}
