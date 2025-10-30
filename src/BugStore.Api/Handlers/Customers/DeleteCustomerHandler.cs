using BugStore.Api.Data;
using BugStore.Api.Requests.Customers;
using BugStore.Api.Responses.Customers;

namespace BugStore.Api.Handlers.Customers;

public class DeleteCustomerHandler : IDeleteCustomerHandler
{
    private readonly AppDbContext _context;

    public DeleteCustomerHandler(AppDbContext context)
    {
        _context = context;
    }

    public DeleteCustomerResponse Handle(DeleteCustomerRequest request)
    {
        var customer = _context.Customers.Find(request.Id)
        ?? throw new InvalidOperationException("Customer not found");

        _context.Customers.Remove(customer);
        _context.SaveChanges();

        return new DeleteCustomerResponse
        {
            Id = request.Id,
            DeletedAt = DateTime.UtcNow,
            Message = "Customer deleted successfully"
        };
    }
}