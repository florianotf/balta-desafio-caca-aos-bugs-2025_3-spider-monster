namespace BugStore.Api.Requests.Products;

public class CreateProductRequest
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string Slug { get; set; }
    public decimal Price { get; set; }
}