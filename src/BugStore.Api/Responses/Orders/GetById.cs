namespace BugStore.Api.Responses.Orders;

public class GetByIdOrderResponse
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public DateTime OrderDate { get; set; }
}