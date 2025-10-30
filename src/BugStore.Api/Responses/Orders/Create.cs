namespace BugStore.Api.Responses.Orders;

public class CreateOrderResponse
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public DateTime OrderDate { get; set; }
}
