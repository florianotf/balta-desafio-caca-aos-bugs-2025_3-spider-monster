using BugStore.Api.Requests.Orders;
using BugStore.Api.Responses.Orders;

namespace BugStore.Api.Handlers.Orders
{
    public interface ICreateOrderHandler
    {
        CreateOrderResponse Handle(CreateOrderRequest request);
    }
}
