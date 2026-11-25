using Microsoft.EntityFrameworkCore;
using Railbite.Domain.Entities;
using Railbite.Domain.Enums;

namespace Railbite.Infrastructure.Persistence;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext db, string? seedPassword, CancellationToken ct = default)
    {
        await db.Database.MigrateAsync(ct);

        if (await db.Users.AnyAsync(ct))
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(seedPassword))
        {
            throw new InvalidOperationException(
                "Seed:Password is not configured. Run: dotnet user-secrets set \"Seed:Password\" \"<value>\" --project src/Railbite.Api");
        }

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(seedPassword);

        var restaurant = new Restaurant
        {
            Name = "Railbite Kitchen",
            Email = "restaurant@railbite.local",
            Phone = "9000000001",
            PasswordHash = passwordHash,
            Role = UserRole.Restaurant,
            RestaurantName = "Railbite Onboard Kitchen",
            Address = "Platform 1"
        };

        var deliveryPerson = new DeliveryPerson
        {
            Name = "Delivery Ravi",
            Email = "delivery@railbite.local",
            Phone = "9000000002",
            PasswordHash = passwordHash,
            Role = UserRole.DeliveryPerson,
            VehicleNumber = "KA-01-1234",
            IsAvailable = true
        };

        var passenger = new Passenger
        {
            Name = "Asha Passenger",
            Email = "passenger@railbite.local",
            Phone = "9000000003",
            PasswordHash = passwordHash,
            Role = UserRole.Passenger,
            Address = "Coach B2"
        };

        var wallet = new Wallet { PassengerId = passenger.Id, Balance = 1000m };
        wallet.Transactions.Add(new Transaction
        {
            WalletId = wallet.Id,
            Amount = 1000m,
            Type = TransactionType.Credit,
            Description = "Welcome bonus"
        });
        passenger.Wallet = wallet;
        passenger.Cart = new Cart { PassengerId = passenger.Id };

        var starters = new Category { Name = "Starters" };
        var mainCourse = new Category { Name = "Main Course" };
        var beverages = new Category { Name = "Beverages" };
        var desserts = new Category { Name = "Desserts" };

        var foods = new[]
        {
            new Food
            {
                Name = "Veg Samosa", Description = "Crispy pastry with spiced potato filling",
                CategoryId = starters.Id, RestaurantId = restaurant.Id, Price = 40m,
                PreparationTimeMinutes = 8, IsAvailable = true,
                ImageUrl = "https://images.unsplash.com/photo-1601050690597-df0568f70950"
            },
            new Food
            {
                Name = "Paneer Tikka", Description = "Grilled cottage cheese with spices",
                CategoryId = starters.Id, RestaurantId = restaurant.Id, Price = 120m,
                PreparationTimeMinutes = 15, IsAvailable = true,
                ImageUrl = "https://images.unsplash.com/photo-1567188040759-fb8a883dc6d8"
            },
            new Food
            {
                Name = "Veg Biryani", Description = "Fragrant basmati rice with vegetables",
                CategoryId = mainCourse.Id, RestaurantId = restaurant.Id, Price = 180m,
                PreparationTimeMinutes = 20, IsAvailable = true,
                ImageUrl = "https://images.unsplash.com/photo-1563379091339-03b21ab4a4f8"
            },
            new Food
            {
                Name = "Chicken Curry Rice", Description = "Spicy chicken curry served with rice",
                CategoryId = mainCourse.Id, RestaurantId = restaurant.Id, Price = 220m,
                PreparationTimeMinutes = 25, IsAvailable = true,
                ImageUrl = "https://images.unsplash.com/photo-1603894584373-5ac82b2ae398"
            },
            new Food
            {
                Name = "Masala Chai", Description = "Hot spiced Indian tea",
                CategoryId = beverages.Id, RestaurantId = restaurant.Id, Price = 25m,
                PreparationTimeMinutes = 5, IsAvailable = true,
                ImageUrl = "https://images.unsplash.com/photo-1597318181409-cf64d0b5d8a2"
            },
            new Food
            {
                Name = "Gulab Jamun", Description = "Sweet fried milk dumplings in syrup",
                CategoryId = desserts.Id, RestaurantId = restaurant.Id, Price = 60m,
                PreparationTimeMinutes = 5, IsAvailable = true,
                ImageUrl = "https://images.unsplash.com/photo-1601303516361-3b7c1b9d4e0f"
            }
        };

        var train = new Train { TrainNumber = "12627", TrainName = "Karnataka Express" };
        train.Stops.Add(new TrainStop
        {
            TrainId = train.Id, StationName = "KSR Bengaluru", StopOrder = 1,
            ArrivalTime = new TimeSpan(6, 0, 0), DepartureTime = new TimeSpan(6, 10, 0),
            IsDeliveryAvailable = true
        });
        train.Stops.Add(new TrainStop
        {
            TrainId = train.Id, StationName = "Tumakuru", StopOrder = 2,
            ArrivalTime = new TimeSpan(7, 5, 0), DepartureTime = new TimeSpan(7, 12, 0),
            IsDeliveryAvailable = true
        });
        train.Stops.Add(new TrainStop
        {
            TrainId = train.Id, StationName = "Davangere", StopOrder = 3,
            ArrivalTime = new TimeSpan(9, 40, 0), DepartureTime = new TimeSpan(9, 48, 0),
            IsDeliveryAvailable = true
        });

        db.Users.AddRange(restaurant, deliveryPerson, passenger);
        db.Categories.AddRange(starters, mainCourse, beverages, desserts);
        db.Foods.AddRange(foods);
        db.Trains.Add(train);

        await db.SaveChangesAsync(ct);
    }
}
