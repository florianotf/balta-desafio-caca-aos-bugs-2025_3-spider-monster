using BugStore.Api.Data;
using BugStore.Api.Handlers.Customers;
using BugStore.Api.Requests.Customers;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace BugStore.Test.Handlers.Customers;

public class CreateCustomerHandlerTests
{
    [Fact]
    public void Should_Create_New_Customer()
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
            Email = "john@example.com",
            Phone = "123-456-7890",
            BirthDate = new DateTime(1990, 1, 1)
        };

        // Act
        var response = handler.Handle(request);

        // Assert
        Assert.NotNull(response);
        Assert.NotEqual(Guid.Empty, response.Id);
        Assert.Equal(request.Name, response.Name);
        Assert.Equal(request.Email, response.Email);

        var customer = dbContext.Customers.Find(response.Id);
        Assert.NotNull(customer);
        Assert.Equal(request.Name, customer.Name);
        Assert.Equal(request.Email, customer.Email);
    }

    [Theory]
    [InlineData("", "john@example.com", "123-456-7890", null)]
    [InlineData("John Doe", "", null, "1990-01-01")]
    [InlineData(null, "john@example.com", "123-456-7890", "1990-01-01")]
    [InlineData("John Doe", null, "123-456-7890", "1990-01-01")]
    public void Should_Validate_Required_Fields(string name, string email, string phone, DateTime birthDate)
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
            Email = email,
            Phone = phone,
            BirthDate = birthDate
        };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => handler.Handle(request));
    }
}