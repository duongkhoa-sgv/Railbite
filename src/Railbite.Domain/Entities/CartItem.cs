namespace Railbite.Domain.Entities;

public class CartItem
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid CartId { get; set; }
    public Cart? Cart { get; set; }
    public Guid FoodId { get; set; }
    public Food? Food { get; set; }

    public int Quantity { get; set; } = 1;
    public decimal UnitPrice { get; set; }
}
