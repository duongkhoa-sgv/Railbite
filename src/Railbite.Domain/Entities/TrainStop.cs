namespace Railbite.Domain.Entities;

public class TrainStop
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid TrainId { get; set; }
    public Train? Train { get; set; }

    public string StationName { get; set; } = string.Empty;
    public int StopOrder { get; set; }
    public TimeSpan? ArrivalTime { get; set; }
    public TimeSpan? DepartureTime { get; set; }
    public bool IsDeliveryAvailable { get; set; } = true;
}
