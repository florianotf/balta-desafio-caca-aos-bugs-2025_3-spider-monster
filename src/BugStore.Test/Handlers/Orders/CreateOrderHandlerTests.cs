using BugStore.Api.Data;
using BugStore.Api.Handlers.Orders;
using BugStore.Api.Models;
using BugStore.Api.Requests.Orders;
using Microsoft.EntityFrameworkCore;

namespace BugStore.Test.Handlers.Orders;

public class CreateOrderHandlerTests
{
    [Fact]
    public void Should_Create_New_Order()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb_" + Guid.NewGuid().ToString())
            .Options;

        var dbContext = new AppDbContext(options);

        // Create a customer and products for the order
        var customer = new Customer
        {
            Name = "John Doe",
            Email = "john@example.com",
            Phone = "123-456-7890",
            BirthDate = new DateTime(1990, 1, 1)
        };
        dbContext.Customers.Add(customer);

        var products = new List<Product>
        {
            new Product { Title = "Product 1", Price = 99.99m, Description = "Description 1", Slug = "product-1" },
            new Product { Title = "Product 2", Price = 149.99m, Description = "Description 1", Slug = "product-1" }
        };
        dbContext.Products.AddRange(products);
        dbContext.SaveChanges();

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
        var response = handler.Handle(request);

        // Assert
        Assert.NotNull(response);
        Assert.NotEqual(Guid.Empty, response.Id);
        Assert.Equal(customer.Id, response.CustomerId);

        var order = dbContext.Orders
            .Include(o => o.Lines)
            .FirstOrDefault(o => o.Id == response.Id);

        Assert.NotNull(order);
        Assert.Equal(customer.Id, order.CustomerId);
        Assert.Equal(2, order.Lines.Count);
        Assert.Contains(order.Lines, l => l.ProductId == products[0].Id && l.Quantity == 2);
        Assert.Contains(order.Lines, l => l.ProductId == products[1].Id && l.Quantity == 1);
    }

    [Fact]
    public void Should_Throw_Exception_When_Customer_Not_Found()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb_" + Guid.NewGuid().ToString())
            .Options;

        var dbContext = new AppDbContext(options);
        var handler = new CreateOrderHandler(dbContext);

        var request = new CreateOrderRequest
        {
            CustomerId = Guid.NewGuid(),
            Lines = new List<OrderLine>
            {
                new OrderLine { ProductId = Guid.NewGuid(), Quantity = 1 }
            }
        };

        // Act & Assert
        Assert.Throws<KeyNotFoundException>(() => handler.Handle(request));
    }

    [Fact]
    public void Should_Throw_Exception_When_Product_Not_Found()
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

        var handler = new CreateOrderHandler(dbContext);

        var request = new CreateOrderRequest
        {
            CustomerId = customer.Id,
            Lines = new List<OrderLine>
            {
                new OrderLine { ProductId = Guid.NewGuid(), Quantity = 1 }
            }
        };

        // Act & Assert
        Assert.Throws<KeyNotFoundException>(() => handler.Handle(request));
    }

    [Fact]
    public void Should_Validate_Order_Lines()
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

        var handler = new CreateOrderHandler(dbContext);

        var request = new CreateOrderRequest
        {
            CustomerId = customer.Id,
            Lines = new List<OrderLine>()
        };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => handler.Handle(request));
    }
}