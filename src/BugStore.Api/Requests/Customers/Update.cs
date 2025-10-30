namespace BugStore.Api.Requests.Customers;

public class UpdateCustomerRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public DateTime BirthDate { get; set; }
    public string Phone { get; set; }
}