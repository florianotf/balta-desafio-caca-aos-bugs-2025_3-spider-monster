using BugStore.Api.Data;
using BugStore.Api.Requests.Products;
using BugStore.Api.Responses.Products;

namespace BugStore.Api.Handlers.Products;

public class GetByIdProductHandler : IGetByIdProductHandler
{
    private readonly AppDbContext _context;

    public GetByIdProductHandler(AppDbContext context)
    {
        _context = context;
    }

    public GetByIdProductResponse Handle(GetByIdProductRequest request)
    {
        var result = _context.Products.Find(request.ProductId);

        return new GetByIdProductResponse
        {
            Id = result.Id,
            Title = result.Title,
            Description = result.Description,
            Slug = result.Slug,
            Price = result.Price
        };
    }
}
