using Railbite.Application.Trains.Dtos;

namespace Railbite.Application.Trains;

public interface ITrainService
{
    Task<IReadOnlyList<TrainDto>> GetTrainsAsync(CancellationToken ct = default);
    Task<TrainDto> GetTrainAsync(Guid id, CancellationToken ct = default);
    Task<IReadOnlyList<TrainStopDto>> GetStopsAsync(Guid trainId, CancellationToken ct = default);
    Task<IReadOnlyList<AvailabilityDto>> GetAvailabilityAsync(Guid trainId, CancellationToken ct = default);
    Task<AvailabilityDto> GetAvailabilityAsync(Guid trainId, Guid stopId, CancellationToken ct = default);
}
