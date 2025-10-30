using BugStore.Api.Requests.Products;
using BugStore.Api.Responses.Products;

namespace BugStore.Api.Handlers.Products
{
    public interface IGetByIdProductHandler
    {
        GetByIdProductResponse Handle(GetByIdProductRequest request);
    }
}
