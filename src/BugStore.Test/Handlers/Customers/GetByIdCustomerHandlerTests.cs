using BugStore.Api.Data;
using BugStore.Api.Handlers.Customers;
using BugStore.Api.Models;
using BugStore.Api.Requests.Customers;
using Microsoft.EntityFrameworkCore;

namespace BugStore.Test.Handlers.Customers;

public class GetByIdCustomerHandlerTests
{
    [Fact]
    public void Should_Return_Customer_By_Id()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb_" + Guid.NewGuid().ToString())
            .Options;

        var dbContext = new AppDbContext(options);

        var customer = new Customer
        {
            Name = "John Doe",
            Email = "john@example.com",
            Phone = "123-456-7890",
            BirthDate = new DateTime(1990, 1, 1)
        };
        dbContext.Customers.Add(customer);
        dbContext.SaveChanges();

        var handler = new GetByIdCustomerHandler(dbContext);

        var request = new GetByIdCustomerRequest
        {
            Id = customer.Id
        };

        // Act
        var response = handler.Handle(request);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(customer.Id, response.Id);
        Assert.Equal(customer.Name, response.Name);
        Assert.Equal(customer.Email, response.Email);
    }

    [Fact]
    public void Should_Throw_Exception_When_Customer_Not_Found()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb_" + Guid.NewGuid().ToString())
            .Options;

        var dbContext = new AppDbContext(options);
        var handler = new GetByIdCustomerHandler(dbContext);

        var request = new GetByIdCustomerRequest
        {
            Id = Guid.NewGuid()
        };

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => handler.Handle(request));
    }
}