using BugStore.Api.Data;
using BugStore.Api.Handlers.Customers;
using BugStore.Api.Models;
using BugStore.Api.Requests.Customers;
using Microsoft.EntityFrameworkCore;

namespace BugStore.Test.Handlers.Customers;

public class UpdateCustomerHandlerTests
{
    [Fact]
    public void Should_Update_Existing_Customer()
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

        var handler = new UpdateCustomerHandler(dbContext);

        var request = new UpdateCustomerRequest
        {
            Id = customer.Id,
            Name = "John Updated",
            Email = "john.updated@example.com",
            Phone = "987-654-3210",
            BirthDate = new DateTime(1991, 2, 2)
        };

        // Act
        var response = handler.Handle(request);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(request.Id, response.Id);
        Assert.Equal(request.Name, response.Name);
        Assert.Equal(request.Email, response.Email);

        var updatedCustomer = dbContext.Customers.Find(response.Id);
        Assert.NotNull(updatedCustomer);
        Assert.Equal(request.Name, updatedCustomer.Name);
        Assert.Equal(request.Email, updatedCustomer.Email);
    }

    [Fact]
    public void Should_Throw_Exception_When_Customer_Not_Found()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb_" + Guid.NewGuid().ToString())
            .Options;

        var dbContext = new AppDbContext(options);
        var handler = new UpdateCustomerHandler(dbContext);

        var request = new UpdateCustomerRequest
        {
            Id = Guid.NewGuid(),
            Name = "John Updated",
            Email = "john.updated@example.com",
            Phone = "987-654-3210",
            BirthDate = new DateTime(1991, 2, 2)
        };

        // Act & Assert
        Assert.Throws<KeyNotFoundException>(() => handler.Handle(request));
    }

    [Theory]
    [InlineData("", "john@example.com", "123-456-7890", "1990-01-01")]
    [InlineData("John Doe", "", "123-456-7890", "1990-01-01")]
    [InlineData("John Doe", "john@example.com", "", "1990-01-01")]
    [InlineData("John Doe", "john@example.com", "123-456-7890", null)]
    public void Should_Validate_Required_Fields(
        string name, string email, string phone, DateTime birthDate)
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb_" + Guid.NewGuid().ToString())
            .Options;

        var dbContext = new AppDbContext(options);

        var customer = new Customer
        {
            Name = name,
            Email = email,
            Phone = phone,
            BirthDate = birthDate
        };
        dbContext.Customers.Add(customer);
        dbContext.SaveChanges();

        var handler = new UpdateCustomerHandler(dbContext);

        var request = new UpdateCustomerRequest
        {
            Id = customer.Id,
            Name = name,
            Email = email,
            Phone = phone,
            BirthDate = birthDate
        };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => handler.Handle(request));
    }
}