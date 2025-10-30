using BugStore.Api.Data;
using BugStore.Api.Handlers.Orders;
using BugStore.Api.Models;
using BugStore.Api.Requests.Orders;
using Microsoft.EntityFrameworkCore;

namespace BugStore.Test.Handlers.Orders;

public class CreateOrderHandlerTests
{
    [Fact]
    public async Task Should_Create_New_Order()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb_" + Guid.NewGuid().ToString())
            .Options;

        var dbContext = new AppDbContext(options);

        // Create a customer and products for the order
        var customer = new Customer { Name = "John Doe", Email = "john@example.com" };
        dbContext.Customers.Add(customer);

        var products = new List<Product>
        {
            new Product { Title = "Product 1", Price = 99.99m },
            new Product { Title = "Product 2", Price = 149.99m }
        };
        dbContext.Products.AddRange(products);
        await dbContext.SaveChangesAsync();

        var handler = new CreateOrderHandler(dbContext);

        var request = new CreateOrderRequest
        {
            CustomerId = customer.Id,
            Lines = new List<OrderLine>
            {
                new OrderLine { ProductId = products[0].Id, Quantity = 2 },
                new OrderLine { ProductId = products[1].Id, Quantity = 1 }
            }
        };

        // Act
        var response = await handler.HandleAsync(request);

        // Assert
        Assert.NotNull(response);
        Assert.True(response.Id > 0);
        Assert.Equal(customer.Id, response.CustomerId);
        Assert.Equal(2, response.Lines.Count);

        var order = await dbContext.Orders
            .Include(o => o.Lines)
            .FirstOrDefaultAsync(o => o.Id == response.Id);

        Assert.NotNull(order);
        Assert.Equal(customer.Id, order.CustomerId);
        Assert.Equal(2, order.Lines.Count);
        Assert.Contains(order.Lines, l => l.ProductId == products[0].Id && l.Quantity == 2);
        Assert.Contains(order.Lines, l => l.ProductId == products[1].Id && l.Quantity == 1);
    }

    [Fact]
    public async Task Should_Throw_Exception_When_Customer_Not_Found()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb_" + Guid.NewGuid().ToString())
            .Options;

        var dbContext = new AppDbContext(options);
        var handler = new CreateOrderHandler(dbContext);

        var request = new CreateOrderRequest
        {
            CustomerId = 999,
            Lines = new List<OrderLine>
            {
                new OrderLine { ProductId = 1, Quantity = 1 }
            }
        };

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => handler.HandleAsync(request));
    }

    [Fact]
    public async Task Should_Throw_Exception_When_Product_Not_Found()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb_" + Guid.NewGuid().ToString())
            .Options;

        var dbContext = new AppDbContext(options);

        var customer = new Customer { Name = "John Doe", Email = "john@example.com" };
        dbContext.Customers.Add(customer);
        await dbContext.SaveChangesAsync();

        var handler = new CreateOrderHandler(dbContext);

        var request = new CreateOrderRequest
        {
            CustomerId = customer.Id,
            Lines = new List<OrderLine>
            {
                new OrderLine { ProductId = 999, Quantity = 1 }
            }
        };

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => handler.HandleAsync(request));
    }

    [Fact]
    public async Task Should_Validate_Order_Lines()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb_" + Guid.NewGuid().ToString())
            .Options;

        var dbContext = new AppDbContext(options);

        var customer = new Customer { Name = "John Doe", Email = "john@example.com" };
        dbContext.Customers.Add(customer);
        await dbContext.SaveChangesAsync();

        var handler = new CreateOrderHandler(dbContext);

        var request = new CreateOrderRequest
        {
            CustomerId = customer.Id,
            Lines = new List<OrderLine>()
        };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => handler.HandleAsync(request));
    }
}