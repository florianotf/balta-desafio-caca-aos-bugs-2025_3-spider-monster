namespace BugStore.Api.Responses.Products;

public class DeleteProductResponse
{
    public Guid Id { get; set; }
    public DateTime DeletedAt { get; set; }
}