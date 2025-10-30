using BugStore.Api.Requests.Products;
using BugStore.Api.Responses.Products;

namespace BugStore.Api.Handlers.Products
{
    public interface ICreateProductHandler
    {
        CreateProductResponse Handle(CreateProductRequest request);
    }
}
