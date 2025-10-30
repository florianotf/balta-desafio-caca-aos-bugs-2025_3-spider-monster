using BugStore.Api.Data;
using BugStore.Api.Handlers.Products;
using BugStore.Api.Models;
using BugStore.Api.Requests.Products;
using Microsoft.EntityFrameworkCore;

namespace BugStore.Test.Handlers.Products;

public class CreateProductHandlerTests
{
    [Fact]
    public void Should_Create_New_Product()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb_" + Guid.NewGuid().ToString())
            .Options;

        var dbContext = new AppDbContext(options);
        var handler = new CreateProductHandler(dbContext);

        var request = new CreateProductRequest
        {
            Title = "Test Product",
            Price = 99.99m,
            Description = "This is a test product",
            Slug = "test-product"
        };

        // Act
        var response = handler.Handle(request);

        // Assert
        Assert.NotNull(response);
        Assert.NotEqual(Guid.Empty, response.Id);
        Assert.Equal(request.Title, response.Title);
        Assert.Equal(request.Price, response.Price);

        var product = dbContext.Products.Find(response.Id);
        Assert.NotNull(product);
        Assert.Equal(request.Title, product.Title);
        Assert.Equal(request.Price, product.Price);
    }

    [Theory]
    [InlineData("", 99.99)]
    [InlineData("Test Product", -1)]
    public void Should_Validate_Required_Fields(string title, decimal price)
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb_" + Guid.NewGuid().ToString())
            .Options;

        var dbContext = new AppDbContext(options);
        var handler = new CreateProductHandler(dbContext);

        var request = new CreateProductRequest
        {
            Title = title,
            Price = price,
            Description = "This is a test product",
            Slug = "test-product"
        };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => handler.Handle(request));
    }
}