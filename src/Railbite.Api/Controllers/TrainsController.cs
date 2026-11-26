using Microsoft.AspNetCore.Mvc;
using Railbite.Application.Trains;
using Railbite.Application.Trains.Dtos;

namespace Railbite.Api.Controllers;

[ApiController]
[Route("api/v1")]
public class TrainsController : ControllerBase
{
    private readonly ITrainService _trains;

    public TrainsController(ITrainService trains)
    {
        _trains = trains;
    }

    [HttpGet("trains")]
    public async Task<ActionResult<IReadOnlyList<TrainDto>>> GetTrains(CancellationToken ct)
    {
        return Ok(await _trains.GetTrainsAsync(ct));
    }

    [HttpGet("trains/{id:guid}")]
    public async Task<ActionResult<TrainDto>> GetTrain(Guid id, CancellationToken ct)
    {
        return Ok(await _trains.GetTrainAsync(id, ct));
    }

    [HttpGet("trains/{id:guid}/stops")]
    public async Task<ActionResult<IReadOnlyList<TrainStopDto>>> GetStops(Guid id, CancellationToken ct)
    {
        return Ok(await _trains.GetStopsAsync(id, ct));
    }

    [HttpGet("availability")]
    public async Task<ActionResult<IReadOnlyList<AvailabilityDto>>> GetAvailability(
        [FromQuery] Guid trainId, CancellationToken ct)
    {
        return Ok(await _trains.GetAvailabilityAsync(trainId, ct));
    }

    [HttpGet("availability/{trainId:guid}/{stopId:guid}")]
    public async Task<ActionResult<AvailabilityDto>> GetAvailabilityForStop(
        Guid trainId, Guid stopId, CancellationToken ct)
    {
        return Ok(await _trains.GetAvailabilityAsync(trainId, stopId, ct));
    }
}
