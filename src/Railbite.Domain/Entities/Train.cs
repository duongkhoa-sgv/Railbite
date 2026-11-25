namespace Railbite.Domain.Entities;

public class Train
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string TrainNumber { get; set; } = string.Empty;
    public string TrainName { get; set; } = string.Empty;

    public ICollection<TrainStop> Stops { get; set; } = new List<TrainStop>();
    public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
