using BugStore.Api.Requests.Orders;
using BugStore.Api.Responses.Orders;

namespace BugStore.Api.Handlers.Orders
{
    public interface IGetByIdOrderHandler
    {
        GetByIdOrderResponse Handle(GetByIdOrderRequest request);
    }
}
