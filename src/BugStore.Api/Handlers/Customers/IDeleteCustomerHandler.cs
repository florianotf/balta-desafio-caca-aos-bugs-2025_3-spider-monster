using BugStore.Api.Requests.Customers;
using BugStore.Api.Responses.Customers;

namespace BugStore.Api.Handlers.Customers
{
    public interface IDeleteCustomerHandler
    {
        DeleteCustomerResponse Handle(DeleteCustomerRequest request);
    }
}