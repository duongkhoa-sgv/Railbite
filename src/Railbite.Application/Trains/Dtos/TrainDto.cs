namespace Railbite.Application.Trains.Dtos;

public class TrainDto
{
    public Guid Id { get; set; }
    public string TrainNumber { get; set; } = string.Empty;
    public string TrainName { get; set; } = string.Empty;
    public IReadOnlyList<TrainStopDto> Stops { get; set; } = Array.Empty<TrainStopDto>();
}
