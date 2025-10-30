using BugStore.Api.Data;
using BugStore.Api.Requests.Products;
using BugStore.Api.Responses.Products;

namespace BugStore.Api.Handlers.Products;

public class UpdateProductHandler : IUpdateProductHandler
{
    private readonly AppDbContext _context;

    public UpdateProductHandler(AppDbContext context)
    {
        _context = context;
    }

    public UpdateProductResponse Handle(UpdateProductRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Title))
            throw new ArgumentException("Product title is required.");
        if (request.Price <= 0)
            throw new ArgumentException("Product price must be greater than zero.");
        if (string.IsNullOrWhiteSpace(request.Slug))
            throw new ArgumentException("Product slug is required.");
        if (string.IsNullOrWhiteSpace(request.Description))
            throw new ArgumentException("Product description is required.");

        var product = _context.Products.Find(request.ProductId);
        if (product == null)
        {
            throw new KeyNotFoundException($"Product with Id {request.ProductId} not found.");
        }

        product.Title = request.Title;
        product.Description = request.Description;
        product.Slug = request.Slug;
        product.Price = request.Price;
        var result = _context.Products.Update(product);
        _context.SaveChanges();

        return new UpdateProductResponse
        {
            Id = result.Entity.Id,
            Title = result.Entity.Title,
            Description = result.Entity.Description,
            Slug = result.Entity.Slug,
            Price = result.Entity.Price
        };
    }
}
