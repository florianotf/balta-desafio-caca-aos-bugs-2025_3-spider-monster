using BugStore.Api.Data;
using BugStore.Api.Handlers.Products;
using BugStore.Api.Models;
using BugStore.Api.Requests.Products;
using Microsoft.EntityFrameworkCore;

namespace BugStore.Test.Handlers.Products;

public class GetByIdProductHandlerTests
{
    [Fact]
    public async Task Should_Return_Product_By_Id()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb_" + Guid.NewGuid().ToString())
            .Options;

        var dbContext = new AppDbContext(options);

        var product = new Product { Title = "Test Product", Price = 99.99m };
        dbContext.Products.Add(product);
        await dbContext.SaveChangesAsync();

        var handler = new GetByIdProductHandler(dbContext);

        var request = new GetByIdProductRequest
        {
            Id = product.Id
        };

        // Act
        var response = await handler.HandleAsync(request);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(product.Id, response.Id);
        Assert.Equal(product.Title, response.Title);
        Assert.Equal(product.Price, response.Price);
    }

    [Fact]
    public async Task Should_Throw_Exception_When_Product_Not_Found()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb_" + Guid.NewGuid().ToString())
            .Options;

        var dbContext = new AppDbContext(options);
        var handler = new GetByIdProductHandler(dbContext);

        var request = new GetByIdProductRequest
        {
            Id = 999
        };

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => handler.HandleAsync(request));
    }
}