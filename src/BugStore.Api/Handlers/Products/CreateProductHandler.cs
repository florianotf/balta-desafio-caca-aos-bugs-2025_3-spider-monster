using BugStore.Api.Data;
using BugStore.Api.Requests.Products;
using BugStore.Api.Responses.Products;

namespace BugStore.Api.Handlers.Products;

public class CreateProductHandler : ICreateProductHandler
{
    private readonly AppDbContext _context;

    public CreateProductHandler(AppDbContext context)
    {
        _context = context;
    }

    public CreateProductResponse Handle(CreateProductRequest request)
    {
        var result = _context.Products.Add(new Models.Product
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Description = request.Description,
            Slug = request.Slug,
            Price = request.Price
        });

        _context.SaveChanges();

        return new CreateProductResponse
        {
            Id = result.Entity.Id,
            Title = result.Entity.Title,
            Description = result.Entity.Description,
            Slug = result.Entity.Slug,
            Price = result.Entity.Price
        };
    }
}
