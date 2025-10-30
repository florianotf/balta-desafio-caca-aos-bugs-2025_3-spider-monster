using BugStore.Api.Data;
using BugStore.Api.Handlers.Products;
using BugStore.Api.Models;
using BugStore.Api.Requests.Products;
using Microsoft.EntityFrameworkCore;

namespace BugStore.Test.Handlers.Products;

public class UpdateProductHandlerTests
{
    [Fact]
    public void Should_Update_Existing_Product()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb_" + Guid.NewGuid().ToString())
            .Options;

        var dbContext = new AppDbContext(options);

        var product = new Product { Title = "Test Product", Price = 99.99m, Description = "This is a test product", Slug = "test-product" };
        dbContext.Products.Add(product);
        dbContext.SaveChanges();

        var handler = new UpdateProductHandler(dbContext);

        var request = new UpdateProductRequest
        {
            ProductId = product.Id,
            Title = "Updated Product",
            Price = 149.99m,
            Description = "This is an updated test product",
            Slug = "updated-test-product"
        };

        // Act
        var response = handler.Handle(request);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(request.ProductId, response.Id);
        Assert.Equal(request.Title, response.Title);
        Assert.Equal(request.Price, response.Price);

        var updatedProduct = dbContext.Products.Find(response.Id);
        Assert.NotNull(updatedProduct);
        Assert.Equal(request.Title, updatedProduct.Title);
        Assert.Equal(request.Price, updatedProduct.Price);
    }

    [Fact]
    public void Should_Throw_Exception_When_Product_Not_Found()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb_" + Guid.NewGuid().ToString())
            .Options;

        var dbContext = new AppDbContext(options);
        var handler = new UpdateProductHandler(dbContext);

        var request = new UpdateProductRequest
        {
            ProductId = Guid.NewGuid(),
            Title = "Updated Product",
            Price = 149.99m,
            Description = "This is an updated test product",
            Slug = "updated-test-product"
        };

        // Act & Assert
        Assert.Throws<KeyNotFoundException>(() => handler.Handle(request));
    }

    [Theory]
    [InlineData("", 99.99)]
    [InlineData("Updated Product", -1)]
    public void Should_Validate_Required_Fields(string title, decimal price)
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb_" + Guid.NewGuid().ToString())
            .Options;

        var dbContext = new AppDbContext(options);
        var handler = new UpdateProductHandler(dbContext);

        var request = new UpdateProductRequest
        {
            ProductId = Guid.NewGuid(),
            Title = title,
            Price = price
        };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => handler.Handle(request));
    }
}