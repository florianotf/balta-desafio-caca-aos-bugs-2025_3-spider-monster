using BugStore.Api.Data;
using BugStore.Api.Handlers.Products;
using BugStore.Api.Models;
using BugStore.Api.Requests.Products;
using Microsoft.EntityFrameworkCore;

namespace BugStore.Test.Handlers.Products;

public class UpdateProductHandlerTests
{
    [Fact]
    public async Task Should_Update_Existing_Product()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb_" + Guid.NewGuid().ToString())
            .Options;

        var dbContext = new AppDbContext(options);

        var product = new Product { Title = "Test Product", Price = 99.99m };
        dbContext.Products.Add(product);
        await dbContext.SaveChangesAsync();

        var handler = new UpdateProductHandler(dbContext);

        var request = new UpdateProductRequest
        {
            ProductId = product.Id,
            Title = "Updated Product",
            Price = 149.99m
        };

        // Act
        var response = await handler.HandleAsync(request);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(request.Id, response.Id);
        Assert.Equal(request.Title, response.Title);
        Assert.Equal(request.Price, response.Price);

        var updatedProduct = await dbContext.Products.FindAsync(response.Id);
        Assert.NotNull(updatedProduct);
        Assert.Equal(request.Title, updatedProduct.Title);
        Assert.Equal(request.Price, updatedProduct.Price);
    }

    [Fact]
    public async Task Should_Throw_Exception_When_Product_Not_Found()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb_" + Guid.NewGuid().ToString())
            .Options;

        var dbContext = new AppDbContext(options);
        var handler = new UpdateProductHandler(dbContext);

        var request = new UpdateProductRequest
        {
            Id = 999,
            Title = "Updated Product",
            Price = 149.99m
        };

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => handler.HandleAsync(request));
    }

    [Theory]
    [InlineData("", 99.99)]
    [InlineData("Updated Product", -1)]
    [InlineData(null, 99.99)]
    public async Task Should_Validate_Required_Fields(string title, decimal price)
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb_" + Guid.NewGuid().ToString())
            .Options;

        var dbContext = new AppDbContext(options);
        var handler = new UpdateProductHandler(dbContext);

        var request = new UpdateProductRequest
        {
            Id = 1,
            Title = title,
            Price = price
        };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => handler.HandleAsync(request));
    }
}