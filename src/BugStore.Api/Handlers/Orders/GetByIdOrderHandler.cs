using BugStore.Api.Data;
using BugStore.Api.Requests.Orders;
using BugStore.Api.Responses.Orders;

namespace BugStore.Api.Handlers.Orders;

public class GetByIdOrderHandler : IGetByIdOrderHandler
{
    private readonly AppDbContext _context;

    public GetByIdOrderHandler(AppDbContext context)
    {
        _context = context;
    }

    public GetByIdOrderResponse Handle(GetByIdOrderRequest request)
    {
        var result = _context.Orders.Find(request.OrderId);

        if (result == null)
        {
            throw new KeyNotFoundException($"Order with Id {request.OrderId} not found.");
        }

        return new GetByIdOrderResponse
        {
            Id = result.Id,
            CustomerId = result.CustomerId,
            OrderDate = result.CreatedAt
        };
    }
}
