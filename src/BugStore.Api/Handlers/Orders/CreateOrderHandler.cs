using BugStore.Api.Data;
using BugStore.Api.Models;
using BugStore.Api.Requests.Orders;
using BugStore.Api.Responses.Orders;

namespace BugStore.Api.Handlers.Orders;

public class CreateOrderHandler : ICreateOrderHandler
{
    private readonly AppDbContext _context;

    public CreateOrderHandler(AppDbContext context)
    {
        _context = context;
    }

    public CreateOrderResponse Handle(CreateOrderRequest request)
    {
        var order = new Models.Order
        {
            Id = Guid.NewGuid(),
            CustomerId = request.CustomerId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            Lines = request.Lines ?? new List<OrderLine>()
        };

        var result = _context.Orders.Add(order);
        _context.SaveChanges();

        return new CreateOrderResponse
        {
            Id = result.Entity.Id,
            CustomerId = result.Entity.CustomerId,
            OrderDate = result.Entity.CreatedAt
        };
    }
}