using Railbite.Domain.Enums;

namespace Railbite.Domain.Entities;

public class Order
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string OrderNumber { get; set; } = string.Empty;

    public Guid PassengerId { get; set; }
    public Passenger? Passenger { get; set; }
    public Guid TicketId { get; set; }
    public Ticket? Ticket { get; set; }
    public Guid? RestaurantId { get; set; }
    public Restaurant? Restaurant { get; set; }
    public Guid? DeliveryPersonId { get; set; }
    public DeliveryPerson? DeliveryPerson { get; set; }

    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    public decimal TotalAmount { get; set; }
    public string? DeliveryAddress { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
    public Delivery? Delivery { get; set; }
}
