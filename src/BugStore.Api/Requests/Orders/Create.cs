using BugStore.Api.Models;

namespace BugStore.Api.Requests.Orders;

public class CreateOrderRequest
{
    public Guid CustomerId { get; set; }
    public List<OrderLine> Lines { get; set; } = null;
}