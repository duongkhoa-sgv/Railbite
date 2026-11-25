namespace Railbite.Domain.Entities;

public class Restaurant : User
{
    public string RestaurantName { get; set; } = string.Empty;
    public string? Address { get; set; }

    public ICollection<Food> Foods { get; set; } = new List<Food>();
    public ICollection<Order> Orders { get; set; } = new List<Order>();
}
