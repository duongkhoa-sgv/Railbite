namespace Railbite.Domain.Entities;

public class Food
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid CategoryId { get; set; }
    public Category? Category { get; set; }
    public Guid? RestaurantId { get; set; }
    public Restaurant? Restaurant { get; set; }

    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public string? ImageUrl { get; set; }
    public int PreparationTimeMinutes { get; set; } = 15;
    public bool IsAvailable { get; set; } = true;

    public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
