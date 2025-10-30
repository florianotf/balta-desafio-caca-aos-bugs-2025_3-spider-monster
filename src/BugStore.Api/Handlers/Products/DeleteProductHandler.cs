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
        var existingProduct = _context.Products.Find(request.ProductId)
            ?? throw new KeyNotFoundException("Product not found.");

        _context.Products.Remove(existingProduct);
        _context.SaveChanges();

        return new DeleteProductResponse
        {
            Id = request.ProductId,
            DeletedAt = DateTime.UtcNow
        };
    }
}
