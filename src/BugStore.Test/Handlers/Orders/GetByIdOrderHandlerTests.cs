using BugStore.Api.Data;
using BugStore.Api.Handlers.Orders;
using BugStore.Api.Models;
using BugStore.Api.Requests.Orders;
using Microsoft.EntityFrameworkCore;

namespace BugStore.Test.Handlers.Orders;

public class GetByIdOrderHandlerTests
{
    [Fact]
    public async Task Should_Return_Order_By_Id()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb_" + Guid.NewGuid().ToString())
            .Options;

        var dbContext = new AppDbContext(options);

        // Create customer and products
        var customer = new Customer { Name = "John Doe", Email = "john@example.com" };
        dbContext.Customers.Add(customer);

        var products = new List<Product>
        {
            new Product { Title = "Product 1", Price = 99.99m },
            new Product { Title = "Product 2", Price = 149.99m }
        };
        dbContext.Products.AddRange(products);

        // Create order
        var order = new Order
        {
            CustomerId = customer.Id,
            Lines = new List<OrderLine>
            {
                new OrderLine { ProductId = 1, Quantity = 2 },
                new OrderLine { ProductId = 2, Quantity = 1 }
            }
        };
        dbContext.Orders.Add(order);
        await dbContext.SaveChangesAsync();

        var handler = new GetByIdOrderHandler(dbContext);

        var request = new GetByIdOrderRequest
        {
            Id = order.Id
        };

        // Act
        var response = await handler.HandleAsync(request);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(order.Id, response.Id);
        Assert.Equal(customer.Id, response.CustomerId);
        Assert.Equal(2, response.Lines.Count);
        Assert.Contains(response.Lines, l => l.ProductId == 1 && l.Quantity == 2);
        Assert.Contains(response.Lines, l => l.ProductId == 2 && l.Quantity == 1);
    }

    [Fact]
    public async Task Should_Throw_Exception_When_Order_Not_Found()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb_" + Guid.NewGuid().ToString())
            .Options;

        var dbContext = new AppDbContext(options);
        var handler = new GetByIdOrderHandler(dbContext);

        var request = new GetByIdOrderRequest
        {
            Id = 999
        };

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => handler.HandleAsync(request));
    }
}