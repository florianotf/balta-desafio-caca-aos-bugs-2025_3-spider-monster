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
        if (string.IsNullOrWhiteSpace(request.Name))
            throw new ArgumentException("Name is required.", nameof(request.Name));
        if (string.IsNullOrWhiteSpace(request.Email))
            throw new ArgumentException("Email is required.", nameof(request.Email));
        if (string.IsNullOrWhiteSpace(request.Phone))
            throw new ArgumentException("Phone is required.", nameof(request.Phone));
        if (request.BirthDate == null)
            throw new ArgumentException("BirthDate is required.", nameof(request.BirthDate));

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