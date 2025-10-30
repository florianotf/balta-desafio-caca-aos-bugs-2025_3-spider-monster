using BugStore.Api.Data;
using BugStore.Api.Handlers.Orders;
using BugStore.Api.Models;
using BugStore.Api.Requests.Orders;
using Microsoft.EntityFrameworkCore;

namespace BugStore.Test.Handlers.Orders;

public class GetByIdOrderHandlerTests
{
    [Fact]
    public void Should_Return_Order_By_Id()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb_" + Guid.NewGuid().ToString())
            .Options;

        var dbContext = new AppDbContext(options);

        // Create customer and products
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

        // Create order
        var order = new Order
        {
            CustomerId = customer.Id,
            Lines = new List<OrderLine>
            {
                new OrderLine { ProductId = products[0].Id, Quantity = 2 },
                new OrderLine { ProductId = products[1].Id, Quantity = 1 }
            }
        };
        dbContext.Orders.Add(order);
        dbContext.SaveChanges();

        var handler = new GetByIdOrderHandler(dbContext);

        var request = new GetByIdOrderRequest
        {
            OrderId = order.Id
        };

        // Act
        var response = handler.Handle(request);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(order.Id, response.Id);
        Assert.Equal(customer.Id, response.CustomerId);
        // Não há Lines em GetByIdOrderResponse, então não validar linhas aqui
    }

    [Fact]
    public void Should_Throw_Exception_When_Order_Not_Found()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb_" + Guid.NewGuid().ToString())
            .Options;

        var dbContext = new AppDbContext(options);
        var handler = new GetByIdOrderHandler(dbContext);

        var request = new GetByIdOrderRequest
        {
            OrderId = Guid.NewGuid()
        };

        // Act & Assert
        Assert.Throws<KeyNotFoundException>(() => handler.Handle(request));
    }
}