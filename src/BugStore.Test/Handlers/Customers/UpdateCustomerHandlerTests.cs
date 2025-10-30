using BugStore.Api.Data;
using BugStore.Api.Handlers.Customers;
using BugStore.Api.Models;
using BugStore.Api.Requests.Customers;
using Microsoft.EntityFrameworkCore;

namespace BugStore.Test.Handlers.Customers;

public class UpdateCustomerHandlerTests
{
    [Fact]
    public async Task Should_Update_Existing_Customer()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb_" + Guid.NewGuid().ToString())
            .Options;

        var dbContext = new AppDbContext(options);

        var customer = new Customer { Name = "John Doe", Email = "john@example.com" };
        dbContext.Customers.Add(customer);
        await dbContext.SaveChangesAsync();

        var handler = new UpdateCustomerHandler(dbContext);

        var request = new UpdateCustomerRequest
        {
            Id = customer.Id,
            Name = "John Updated",
            Email = "john.updated@example.com"
        };

        // Act
        var response = await handler.HandleAsync(request);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(request.Id, response.Id);
        Assert.Equal(request.Name, response.Name);
        Assert.Equal(request.Email, response.Email);

        var updatedCustomer = await dbContext.Customers.FindAsync(response.Id);
        Assert.NotNull(updatedCustomer);
        Assert.Equal(request.Name, updatedCustomer.Name);
        Assert.Equal(request.Email, updatedCustomer.Email);
    }

    [Fact]
    public async Task Should_Throw_Exception_When_Customer_Not_Found()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb_" + Guid.NewGuid().ToString())
            .Options;

        var dbContext = new AppDbContext(options);
        var handler = new UpdateCustomerHandler(dbContext);

        var request = new UpdateCustomerRequest
        {
            Id = 999,
            Name = "John Updated",
            Email = "john.updated@example.com"
        };

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => handler.HandleAsync(request));
    }

    [Theory]
    [InlineData("", "john@example.com")]
    [InlineData("John Doe", "")]
    [InlineData(null, "john@example.com")]
    [InlineData("John Doe", null)]
    public async Task Should_Validate_Required_Fields(string name, string email)
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb_" + Guid.NewGuid().ToString())
            .Options;

        var dbContext = new AppDbContext(options);
        var handler = new UpdateCustomerHandler(dbContext);

        var request = new UpdateCustomerRequest
        {
            Id = 1,
            Name = name,
            Email = email
        };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => handler.HandleAsync(request));
    }
}