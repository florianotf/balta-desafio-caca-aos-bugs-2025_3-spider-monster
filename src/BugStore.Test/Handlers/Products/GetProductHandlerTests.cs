using BugStore.Api.Data;
using BugStore.Api.Handlers.Products;
using BugStore.Api.Models;
using BugStore.Api.Requests.Products;
using Microsoft.EntityFrameworkCore;

namespace BugStore.Test.Handlers.Products;

public class GetProductHandlerTests
{
    [Fact]
    public async Task Should_Return_All_Products()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb_" + Guid.NewGuid().ToString())
            .Options;

        var dbContext = new AppDbContext(options);

        var products = new List<Product>
        {
            new Product { Title = "Product 1", Price = 99.99m },
            new Product { Title = "Product 2", Price = 149.99m },
            new Product { Title = "Product 3", Price = 199.99m }
        };

        dbContext.Products.AddRange(products);
        await dbContext.SaveChangesAsync();

        var handler = new GetProductHandler(dbContext);
        var request = new GetProductRequest();

        // Act
        var response = await handler.HandleAsync(request);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(3, response.Products.Count);
        Assert.Contains(response.Products, p => p.Title == "Product 1" && p.Price == 99.99m);
        Assert.Contains(response.Products, p => p.Title == "Product 2" && p.Price == 149.99m);
        Assert.Contains(response.Products, p => p.Title == "Product 3" && p.Price == 199.99m);
    }

    [Fact]
    public async Task Should_Return_Empty_List_When_No_Products()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb_" + Guid.NewGuid().ToString())
            .Options;

        var dbContext = new AppDbContext(options);
        var handler = new GetProductHandler(dbContext);
        var request = new GetProductRequest();

        // Act
        var response = await handler.HandleAsync(request);

        // Assert
        Assert.NotNull(response);
        Assert.Empty(response.Products);
    }
}