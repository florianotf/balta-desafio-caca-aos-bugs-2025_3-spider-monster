using BugStore.Api.Data;
using BugStore.Api.Handlers.Customers;
using BugStore.Api.Models;
using BugStore.Api.Requests.Customers;
using Microsoft.EntityFrameworkCore;

namespace BugStore.Test.Handlers.Customers;

public class DeleteCustomerHandlerTests
{
    [Fact]
    public void Should_Delete_Existing_Customer()
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

        var handler = new DeleteCustomerHandler(dbContext);

        var request = new DeleteCustomerRequest
        {
            Id = customer.Id
        };

        // Act
        var response = handler.Handle(request);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(request.Id, response.Id);

        var deletedCustomer = dbContext.Customers.Find(request.Id);
        Assert.Null(deletedCustomer);
    }

    [Fact]
    public void Should_Throw_Exception_When_Customer_Not_Found()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb_" + Guid.NewGuid().ToString())
            .Options;

        var dbContext = new AppDbContext(options);
        var handler = new DeleteCustomerHandler(dbContext);

        var request = new DeleteCustomerRequest
        {
            Id = Guid.NewGuid()
        };

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => handler.Handle(request));
    }
}