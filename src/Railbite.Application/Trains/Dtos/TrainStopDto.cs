namespace Railbite.Application.Trains.Dtos;

public class TrainStopDto
{
    public Guid Id { get; set; }
    public string StationName { get; set; } = string.Empty;
    public int StopOrder { get; set; }
    public TimeSpan? ArrivalTime { get; set; }
    public TimeSpan? DepartureTime { get; set; }
    public bool IsDeliveryAvailable { get; set; }
}
