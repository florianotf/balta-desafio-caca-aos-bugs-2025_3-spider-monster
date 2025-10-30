namespace BugStore.Api.Responses.Customers;

public class DeleteCustomerResponse
{
    public Guid Id { get; set; }
    public DateTime DeletedAt { get; set; }
    public string Message { get; set; } = "Customer deleted successfully.";
}