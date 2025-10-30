using BugStore.Api.Data;
using BugStore.Api.Handlers.Customers;
using BugStore.Api.Models;
using BugStore.Api.Requests.Customers;
using Microsoft.EntityFrameworkCore;

namespace BugStore.Test.Handlers.Customers;

public class GetCustomerHandlerTests
{
    [Fact]
    public void Should_Return_All_Customers()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb_" + Guid.NewGuid().ToString())
            .Options;

        var dbContext = new AppDbContext(options);

        var customers = new List<Customer>
        {
            new Customer { Name = "John Doe", Email = "john@example.com" , Phone = "123-456-7890", BirthDate = new DateTime(1990, 1, 1) },
            new Customer { Name = "Jane Doe", Email = "jane@example.com" , Phone = "123-456-7890", BirthDate = new DateTime(1990, 1, 1) },
            new Customer { Name = "Bob Smith", Email = "bob@example.com" , Phone = "123-456-7890", BirthDate = new DateTime(1990, 1, 1) },
        };

        dbContext.Customers.AddRange(customers);
        dbContext.SaveChanges();

        var handler = new GetCustomerHandler(dbContext);
        var request = new GetCustomerRequest();

        // Act
        var response = handler.Handle();

        // Assert
        Assert.NotNull(response);
        Assert.Equal(3, response.Customers.Count());
        Assert.Contains(response.Customers, c => c.Name == "John Doe");
        Assert.Contains(response.Customers, c => c.Name == "Jane Doe");
        Assert.Contains(response.Customers, c => c.Name == "Bob Smith");
    }

    [Fact]
    public void Should_Return_Empty_List_When_No_Customers()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb_" + Guid.NewGuid().ToString())
            .Options;

        var dbContext = new AppDbContext(options);
        var handler = new GetCustomerHandler(dbContext);
        var request = new GetCustomerRequest();

        // Act
        var response = handler.Handle();

        // Assert
        Assert.NotNull(response);
        Assert.Empty(response.Customers);
    }
}