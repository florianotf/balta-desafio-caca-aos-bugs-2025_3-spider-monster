using BugStore.Api.Data;
using BugStore.Api.Models;
using BugStore.Api.Requests.Customers;
using BugStore.Api.Responses.Customers;

namespace BugStore.Api.Handlers.Customers;

public class GetCustomerHandler : IGetCustomerHandler
{
    private readonly AppDbContext _context;

    public GetCustomerHandler(AppDbContext context)
    {
        _context = context;
    }

    public GetCustomerResponse Handle()
    {
        var result = _context.Customers.ToList(); ;

        _context.SaveChanges();

        return new GetCustomerResponse
        {
            Customers = result.Select(c => new Customer
            {
                Id = c.Id,
                Name = c.Name,
                Email = c.Email,
                Phone = c.Phone
            }).ToList()
        };
    }
}