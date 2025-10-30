using BugStore.Api.Data;
using BugStore.Api.Requests.Customers;
using BugStore.Api.Responses.Customers;

namespace BugStore.Api.Handlers.Customers;

public class CreateCustomerHandler : ICreateCustomerHandler
{
    private readonly AppDbContext _context;

    public CreateCustomerHandler(AppDbContext context)
    {
        _context = context;
    }

    public CreateCustomerResponse Handle(CreateCustomerRequest request)
    {
        var result = _context.Customers.Add(new Models.Customer
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Email = request.Email,
            BirthDate = request.BirthDate,
            Phone = request.Phone
        });

        _context.SaveChanges();

        return new CreateCustomerResponse
        {
            Id = result.Entity.Id,
            Name = result.Entity.Name,
            Email = result.Entity.Email,
            Phone = result.Entity.Phone,
            BirthDate = result.Entity.BirthDate
        };
    }
}