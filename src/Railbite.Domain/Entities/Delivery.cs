namespace Railbite.Domain.Entities;

public class Delivery
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid OrderId { get; set; }
    public Order? Order { get; set; }
    public Guid DeliveryPersonId { get; set; }
    public DeliveryPerson? DeliveryPerson { get; set; }

    public DateTime AssignedAt { get; set; } = DateTime.UtcNow;
    public DateTime? PickedUpAt { get; set; }
    public DateTime? DeliveredAt { get; set; }
}
