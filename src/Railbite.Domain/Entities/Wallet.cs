namespace Railbite.Domain.Entities;

public class Wallet
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid PassengerId { get; set; }
    public Passenger? Passenger { get; set; }
    public decimal Balance { get; set; }

    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
