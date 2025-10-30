using BugStore.Api.Data;
using BugStore.Api.Requests.Customers;
using BugStore.Api.Responses.Customers;

namespace BugStore.Api.Handlers.Customers;

public class GetByIdCustomerHandler : IGetByIdCustomerHandler
{
    private readonly AppDbContext _context;

    public GetByIdCustomerHandler(AppDbContext context)
    {
        _context = context;
    }

    public GetByIdCustomerResponse Handle(GetByIdCustomerRequest request)
    {
        var result = _context.Customers.Find(request.Id);

        return new GetByIdCustomerResponse
        {
            Id = result.Id,
            Name = result.Name,
            Email = result.Email,
            Phone = result.Phone,
            BirthDate = result.BirthDate
        };
    }
}