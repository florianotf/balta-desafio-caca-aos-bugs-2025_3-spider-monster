using BugStore.Api.Data;
using BugStore.Api.Handlers.Customers;
using BugStore.Api.Requests.Customers;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace BugStore.Test.Handlers.Customers;

public class CreateCustomerHandlerTests
{
    [Fact]
    public async Task Should_Create_New_Customer()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb_" + Guid.NewGuid().ToString())
            .Options;

        var dbContext = new AppDbContext(options);
        var handler = new CreateCustomerHandler(dbContext);

        var request = new CreateCustomerRequest
        {
            Name = "John Doe",
            Email = "john@example.com"
        };

        // Act
        var response = await handler.HandleAsync(request);

        // Assert
        Assert.NotNull(response);
        Assert.True(response.Id > 0);
        Assert.Equal(request.Name, response.Name);
        Assert.Equal(request.Email, response.Email);

        var customer = await dbContext.Customers.FindAsync(response.Id);
        Assert.NotNull(customer);
        Assert.Equal(request.Name, customer.Name);
        Assert.Equal(request.Email, customer.Email);
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
        var handler = new CreateCustomerHandler(dbContext);

        var request = new CreateCustomerRequest
        {
            Name = name,
            Email = email
        };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => handler.HandleAsync(request));
    }
}