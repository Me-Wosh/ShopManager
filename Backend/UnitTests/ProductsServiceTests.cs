using Microsoft.AspNetCore.Mvc;
using Moq;
using ShopManagerBackend.Controllers;
using ShopManagerBackend.Entities;
using ShopManagerBackend.Exceptions;
using ShopManagerBackend.Services;

namespace UnitTests;

public class ProductsServiceTests
{
    private List<Product> _products;
    private Mock<IProductsService> _service;
    private readonly ProductsController _controller;

    public ProductsServiceTests()
    {
        // arrange
        _products = new List<Product>
        {
            new Product
            {
                Id = 1,
                Name = "Hat",
                Quantity = 40,
                Price = 16.90m,
                ImagePath = "Assets/Hat.svg"
            },

            new Product
            {
                Id = 2,
                Name = "Hoodie",
                Quantity = 80,
                Price = 35.90m,
                ImagePath = "Assets/Hoodie.svg"
            }
        };
        
        _service = new Mock<IProductsService>();
        _service
            .Setup(s => s.GetAllProducts())
            .Returns(_products);

        _service
            .Setup(s => s.GetProductById(It.IsAny<int>()))
            .Returns<int>(id =>
                _products.SingleOrDefault(p => p.Id == id) ??
                throw new NotFoundException($"Product of id: {id} not found"));

        _controller = new ProductsController(_service.Object);
    }

    [Fact]
    public void GetAllProducts_GivenSomeProducts_ReturnsIEnumerableOfTheseProducts()
    {
        // act
        var result = (ObjectResult)_controller.GetAllProducts().Result!;
        var resultValue = (List<Product>)result.Value!;
        
        // assert
        Assert.Equal(200, result.StatusCode);
        Assert.NotEmpty(resultValue);
        Assert.Equal(2, resultValue.Count);
        Assert.Equal("Hat", resultValue.ElementAt(0).Name);
        Assert.Equal("Hoodie", resultValue.ElementAt(1).Name);
    }

    [Fact]
    public void GetAllProducts_GivenNoProducts_ReturnsEmptyIEnumerable()
    {
        // arrange
        _products.Clear();
        
        // act
        var result = (ObjectResult)_controller.GetAllProducts().Result!;
        var resultValue = (List<Product>)result.Value!;
        
        // assert
        Assert.Equal(200, result.StatusCode);
        Assert.Empty(resultValue);
    }

    [Fact]
    public void GetProductById_GivenExistingId_ReturnsProduct()
    {
        // act
        var result = (ObjectResult)_controller.GetProductById(1).Result!;
        var resultValue = (Product?)result.Value;
        
        // assert
        Assert.Equal(200, result.StatusCode);
        Assert.NotNull(resultValue);
        Assert.Equal("Hat", resultValue?.Name);
    }

    [Fact]
    public void GetProductById_GivenNonExistingId_ThrowsNotFoundException()
    {
        // arrange
        const int id = -1;
        
        // assert
        var exception = Assert.Throws<NotFoundException>(() => _controller.GetProductById(id));
        Assert.Equal($"Product of id: {id} not found", exception.Message);
    }
}