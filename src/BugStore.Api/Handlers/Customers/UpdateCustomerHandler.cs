using BugStore.Api.Data;
using BugStore.Api.Requests.Customers;
using BugStore.Api.Responses.Customers;

namespace BugStore.Api.Handlers.Customers;

public class UpdateCustomerHandler : IUpdateCustomerHandler
{
    private readonly AppDbContext _context;

    public UpdateCustomerHandler(AppDbContext context)
    {
        _context = context;
    }

    public UpdateCustomerResponse Handle(UpdateCustomerRequest request)
    {
        var customer = _context.Customers.Find(request.Id);
        if (customer == null)
        {
            throw new KeyNotFoundException($"Customer with Id {request.Id} not found.");
        }

        customer.Name = request.Name;
        customer.Email = request.Email;
        customer.Phone = request.Phone;
        customer.BirthDate = request.BirthDate;
        var result = _context.Customers.Update(customer);
        _context.SaveChanges();

        return new UpdateCustomerResponse
        {
            Id = result.Entity.Id,
            Name = result.Entity.Name,
            Email = result.Entity.Email,
            Phone = result.Entity.Phone,
            BirthDate = result.Entity.BirthDate
        };
    }
}