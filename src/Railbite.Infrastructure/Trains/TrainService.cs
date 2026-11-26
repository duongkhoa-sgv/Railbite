using Microsoft.EntityFrameworkCore;
using Railbite.Application.Common.Exceptions;
using Railbite.Application.Trains;
using Railbite.Application.Trains.Dtos;
using Railbite.Domain.Entities;
using Railbite.Infrastructure.Persistence;

namespace Railbite.Infrastructure.Trains;

public class TrainService : ITrainService
{
    private readonly AppDbContext _db;

    public TrainService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<TrainDto>> GetTrainsAsync(CancellationToken ct = default)
    {
        var trains = await _db.Trains
            .AsNoTracking()
            .Include(t => t.Stops)
            .OrderBy(t => t.TrainNumber)
            .ToListAsync(ct);

        return trains.Select(MapTrain).ToList();
    }

    public async Task<TrainDto> GetTrainAsync(Guid id, CancellationToken ct = default)
    {
        var train = await _db.Trains
            .AsNoTracking()
            .Include(t => t.Stops)
            .FirstOrDefaultAsync(t => t.Id == id, ct)
            ?? throw new NotFoundException("Train not found.");

        return MapTrain(train);
    }

    public async Task<IReadOnlyList<TrainStopDto>> GetStopsAsync(Guid trainId, CancellationToken ct = default)
    {
        var exists = await _db.Trains.AnyAsync(t => t.Id == trainId, ct);
        if (!exists)
        {
            throw new NotFoundException("Train not found.");
        }

        var stops = await _db.TrainStops
            .AsNoTracking()
            .Where(s => s.TrainId == trainId)
            .OrderBy(s => s.StopOrder)
            .ToListAsync(ct);

        return stops.Select(MapStop).ToList();
    }

    public async Task<IReadOnlyList<AvailabilityDto>> GetAvailabilityAsync(Guid trainId, CancellationToken ct = default)
    {
        var train = await _db.Trains
            .AsNoTracking()
            .Include(t => t.Stops)
            .FirstOrDefaultAsync(t => t.Id == trainId, ct)
            ?? throw new NotFoundException("Train not found.");

        return train.Stops
            .OrderBy(s => s.StopOrder)
            .Select(s => MapAvailability(train, s))
            .ToList();
    }

    public async Task<AvailabilityDto> GetAvailabilityAsync(Guid trainId, Guid stopId, CancellationToken ct = default)
    {
        var train = await _db.Trains
            .AsNoTracking()
            .Include(t => t.Stops)
            .FirstOrDefaultAsync(t => t.Id == trainId, ct)
            ?? throw new NotFoundException("Train not found.");

        var stop = train.Stops.FirstOrDefault(s => s.Id == stopId)
            ?? throw new NotFoundException("Train stop not found.");

        return MapAvailability(train, stop);
    }

    private static TrainDto MapTrain(Train train) => new()
    {
        Id = train.Id,
        TrainNumber = train.TrainNumber,
        TrainName = train.TrainName,
        Stops = train.Stops
            .OrderBy(s => s.StopOrder)
            .Select(MapStop)
            .ToList()
    };

    private static TrainStopDto MapStop(TrainStop stop) => new()
    {
        Id = stop.Id,
        StationName = stop.StationName,
        StopOrder = stop.StopOrder,
        ArrivalTime = stop.ArrivalTime,
        DepartureTime = stop.DepartureTime,
        IsDeliveryAvailable = stop.IsDeliveryAvailable
    };

    private static AvailabilityDto MapAvailability(Train train, TrainStop stop) => new()
    {
        TrainId = train.Id,
        TrainNumber = train.TrainNumber,
        TrainName = train.TrainName,
        StopId = stop.Id,
        StationName = stop.StationName,
        StopOrder = stop.StopOrder,
        ArrivalTime = stop.ArrivalTime,
        DepartureTime = stop.DepartureTime,
        IsDeliveryAvailable = stop.IsDeliveryAvailable
    };
}
