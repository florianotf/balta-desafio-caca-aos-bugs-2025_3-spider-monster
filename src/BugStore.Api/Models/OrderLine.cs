namespace BugStore.Api.Models;

public class OrderLine
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }

    public int Quantity { get; set; }
    public decimal Total { get; set; }

    public Guid ProductId { get; set; }
}