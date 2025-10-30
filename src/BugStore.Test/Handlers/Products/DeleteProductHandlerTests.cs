using BugStore.Api.Data;
using BugStore.Api.Handlers.Products;
using BugStore.Api.Models;
using BugStore.Api.Requests.Products;
using Microsoft.EntityFrameworkCore;

namespace BugStore.Test.Handlers.Products;

public class DeleteProductHandlerTests
{
    [Fact]
    public void Should_Delete_Existing_Product()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb_" + Guid.NewGuid().ToString())
            .Options;

        var dbContext = new AppDbContext(options);

        var product = new Product
        {
            Title = "Test Product",
            Price = 99.99m,
            Description = "This is a test product",
            Slug = "test-product"
        };
        dbContext.Products.Add(product);
        dbContext.SaveChanges();

        var handler = new DeleteProductHandler(dbContext);

        var request = new DeleteProductRequest
        {
            ProductId = product.Id
        };

        // Act
        var response = handler.Handle(request);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(request.ProductId, response.Id);

        var deletedProduct = dbContext.Products.Find(request.ProductId);
        Assert.Null(deletedProduct);
    }

    [Fact]
    public void Should_Throw_Exception_When_Product_Not_Found()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb_" + Guid.NewGuid().ToString())
            .Options;

        var dbContext = new AppDbContext(options);
        var handler = new DeleteProductHandler(dbContext);

        var request = new DeleteProductRequest
        {
            ProductId = Guid.NewGuid()
        };

        // Act & Assert
        Assert.Throws<KeyNotFoundException>(() => handler.Handle(request));
    }
}