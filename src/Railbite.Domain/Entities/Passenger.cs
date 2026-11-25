namespace Railbite.Domain.Entities;

public class Passenger : User
{
    public string? Address { get; set; }

    public Cart? Cart { get; set; }
    public Wallet? Wallet { get; set; }
    public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    public ICollection<Order> Orders { get; set; } = new List<Order>();
}
