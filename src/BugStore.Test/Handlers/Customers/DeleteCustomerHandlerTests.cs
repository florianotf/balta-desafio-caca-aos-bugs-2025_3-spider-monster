using BugStore.Api.Data;
using BugStore.Api.Handlers.Customers;
using BugStore.Api.Models;
using BugStore.Api.Requests.Customers;
using Microsoft.EntityFrameworkCore;

namespace BugStore.Test.Handlers.Customers;

public class DeleteCustomerHandlerTests
{
    [Fact]
    public async Task Should_Delete_Existing_Customer()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb_" + Guid.NewGuid().ToString())
            .Options;

        var dbContext = new AppDbContext(options);

        var customer = new Customer { Name = "John Doe", Email = "john@example.com" };
        dbContext.Customers.Add(customer);
        await dbContext.SaveChangesAsync();

        var handler = new DeleteCustomerHandler(dbContext);

        var request = new DeleteCustomerRequest
        {
            Id = customer.Id
        };

        // Act
        var response = await handler.HandleAsync(request);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(request.Id, response.Id);

        var deletedCustomer = await dbContext.Customers.FindAsync(request.Id);
        Assert.Null(deletedCustomer);
    }

    [Fact]
    public async Task Should_Throw_Exception_When_Customer_Not_Found()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb_" + Guid.NewGuid().ToString())
            .Options;

        var dbContext = new AppDbContext(options);
        var handler = new DeleteCustomerHandler(dbContext);

        var request = new DeleteCustomerRequest
        {
            Id = 999
        };

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => handler.HandleAsync(request));
    }
}