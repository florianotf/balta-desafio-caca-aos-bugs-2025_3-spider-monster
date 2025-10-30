using BugStore.Api.Requests.Products;
using BugStore.Api.Responses.Products;

namespace BugStore.Api.Handlers.Products
{
    public interface IDeleteProductHandler
    {
        DeleteProductResponse Handle(DeleteProductRequest request);
    }
}
