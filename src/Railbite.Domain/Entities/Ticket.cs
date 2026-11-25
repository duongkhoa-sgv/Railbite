namespace Railbite.Domain.Entities;

public class Ticket
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid PassengerId { get; set; }
    public Passenger? Passenger { get; set; }

    public string PnrNumber { get; set; } = string.Empty;
    public Guid TrainId { get; set; }
    public Train? Train { get; set; }
    public Guid FromStopId { get; set; }
    public TrainStop? FromStop { get; set; }
    public Guid ToStopId { get; set; }
    public TrainStop? ToStop { get; set; }

    public string? Coach { get; set; }
    public string? SeatNumber { get; set; }
    public DateOnly TravelDate { get; set; }
    public bool IsConfirmed { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Order> Orders { get; set; } = new List<Order>();
}
