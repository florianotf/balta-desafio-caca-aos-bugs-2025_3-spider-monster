using BugStore.Api.Data;
using BugStore.Api.Requests.Products;
using BugStore.Api.Responses.Products;

namespace BugStore.Api.Handlers.Products;

public class DeleteProductHandler : IDeleteProductHandler
{
    private readonly AppDbContext _context;

    public DeleteProductHandler(AppDbContext context)
    {
        _context = context;
    }

    public DeleteProductResponse Handle(DeleteProductRequest request)
    {
        var result = _context.Products.Remove(new Models.Product
        {
            Id = request.ProductId
        });

        _context.SaveChanges();

        return new DeleteProductResponse
        {
            Id = request.ProductId,
            DeletedAt = DateTime.UtcNow
        };
    }
}
