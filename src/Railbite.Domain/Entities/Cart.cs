namespace Railbite.Domain.Entities;

public class Cart
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid PassengerId { get; set; }
    public Passenger? Passenger { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<CartItem> Items { get; set; } = new List<CartItem>();
}
