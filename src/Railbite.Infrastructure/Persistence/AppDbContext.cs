using Microsoft.EntityFrameworkCore;
using Railbite.Domain.Entities;

namespace Railbite.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Passenger> Passengers => Set<Passenger>();
    public DbSet<Restaurant> Restaurants => Set<Restaurant>();
    public DbSet<DeliveryPerson> DeliveryPersons => Set<DeliveryPerson>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Food> Foods => Set<Food>();
    public DbSet<Train> Trains => Set<Train>();
    public DbSet<TrainStop> TrainStops => Set<TrainStop>();
    public DbSet<Cart> Carts => Set<Cart>();
    public DbSet<CartItem> CartItems => Set<CartItem>();
    public DbSet<Wallet> Wallets => Set<Wallet>();
    public DbSet<Transaction> Transactions => Set<Transaction>();
    public DbSet<Ticket> Tickets => Set<Ticket>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<Delivery> Deliveries => Set<Delivery>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
