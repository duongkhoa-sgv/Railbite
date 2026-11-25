namespace Railbite.Domain.Entities;

public class DeliveryPerson : User
{
    public string? VehicleNumber { get; set; }
    public bool IsAvailable { get; set; } = true;

    public ICollection<Order> AssignedOrders { get; set; } = new List<Order>();
    public ICollection<Delivery> Deliveries { get; set; } = new List<Delivery>();
}
