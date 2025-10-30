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
        var order = new Order
        {
            Id = Guid.NewGuid(),
            CustomerId = request.CustomerId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            Lines = request.Lines ?? new List<OrderLine>()
        };

        if (order.Lines.Count == 0)
        {
            throw new ArgumentException("Order must have at least one order line.");
        }

        if (!_context.Customers.Any(c => c.Id == order.CustomerId))
        {
            throw new KeyNotFoundException($"Customer with Id {order.CustomerId} not found.");
        }

        if (order.Lines.Any(ol => !_context.Products.Any(p => p.Id == ol.ProductId)))
        {
            throw new KeyNotFoundException("One or more products in the order lines were not found.");
        }

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