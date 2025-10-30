using BugStore.Api.Data;
using BugStore.Api.Handlers.Customers;
using BugStore.Api.Models;
using BugStore.Api.Requests.Customers;
using Microsoft.EntityFrameworkCore;

namespace BugStore.Test.Handlers.Customers;

public class GetByIdCustomerHandlerTests
{
    [Fact]
    public async Task Should_Return_Customer_By_Id()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb_" + Guid.NewGuid().ToString())
            .Options;

        var dbContext = new AppDbContext(options);

        var customer = new Customer { Name = "John Doe", Email = "john@example.com" };
        dbContext.Customers.Add(customer);
        await dbContext.SaveChangesAsync();

        var handler = new GetByIdCustomerHandler(dbContext);

        var request = new GetByIdCustomerRequest
        {
            Id = customer.Id
        };

        // Act
        var response = await handler.HandleAsync(request);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(customer.Id, response.Id);
        Assert.Equal(customer.Name, response.Name);
        Assert.Equal(customer.Email, response.Email);
    }

    [Fact]
    public async Task Should_Throw_Exception_When_Customer_Not_Found()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb_" + Guid.NewGuid().ToString())
            .Options;

        var dbContext = new AppDbContext(options);
        var handler = new GetByIdCustomerHandler(dbContext);

        var request = new GetByIdCustomerRequest
        {
            Id = 999
        };

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => handler.HandleAsync(request));
    }
}