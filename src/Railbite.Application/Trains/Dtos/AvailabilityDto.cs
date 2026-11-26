namespace Railbite.Application.Trains.Dtos;

public class AvailabilityDto
{
    public Guid TrainId { get; set; }
    public string TrainNumber { get; set; } = string.Empty;
    public string TrainName { get; set; } = string.Empty;
    public Guid StopId { get; set; }
    public string StationName { get; set; } = string.Empty;
    public int StopOrder { get; set; }
    public TimeSpan? ArrivalTime { get; set; }
    public TimeSpan? DepartureTime { get; set; }
    public bool IsDeliveryAvailable { get; set; }
}
