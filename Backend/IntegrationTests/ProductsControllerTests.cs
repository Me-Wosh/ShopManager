using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopManagerBackend.Controllers;
using ShopManagerBackend.Entities;
using ShopManagerBackend.Exceptions;
using ShopManagerBackend.Models;
using ShopManagerBackend.Services;

namespace IntegrationTests;

public class ProductsControllerTests
{
    private List<Product> _products;
    private DbContextOptions<ShopManagerDbContext> _options;

    public ProductsControllerTests()
    {
        _products = new List<Product>
        {
            new Product
            {
                Id = 1,
                Sku = "HX421RO",
                Name = "Hat",
                Quantity = 40,
                Price = 16.90m,
                ImagePath = "Assets/Hat.svg"
            },
                
            new Product
            {
                Id = 2,
                Sku = "MN632FD",
                Name = "Hoodie",
                Quantity = 80,
                Price = 35.90m,
                ImagePath = "Assets/Hoodie.svg"
            }
        };

        _options = new DbContextOptionsBuilder<ShopManagerDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
    }
    
    [Fact]
    public void GetAllProducts_GivenSomeProducts_ReturnsTheseProducts()
    {
        var dbContext = CreateDbContext(_options, _products);
        var service = new ProductsService(dbContext);
        var controller = new ProductsController(service);

        var result = (ObjectResult)controller.GetAllProducts().Result!;
        var resultValue = (List<Product>)result.Value!;
        
        Assert.Equal(200, result.StatusCode);
        Assert.NotEmpty(resultValue);
        Assert.Equal("Hat", resultValue.ElementAt(0).Name);
        Assert.Equal("Hoodie", resultValue.ElementAt(1).Name);
    }

    [Fact]
    public void GetAllProducts_GivenNoProducts_ReturnsEmptyEnumerable()
    {
        _products.Clear();
        var dbContext = CreateDbContext(_options, _products);
        var service = new ProductsService(dbContext);
        var controller = new ProductsController(service);
        
        var result = (ObjectResult)controller.GetAllProducts().Result!;
        var resultValue = (List<Product>)result.Value!;
        
        Assert.Equal(200, result.StatusCode);
        Assert.Empty(resultValue);
    }

    [Fact]
    public void GetAvailableProducts_ReturnsOnlyProductsWithQuantityGreaterThanZero()
    {
        _products.First().Quantity = 0;
        var dbContext = CreateDbContext(_options, _products);
        var service = new ProductsService(dbContext);
        var controller = new ProductsController(service);

        var result = (ObjectResult)controller.GetAvailableProducts().Result!;
        var resultValue = (List<Product>)result.Value!;
        
        Assert.Equal(200, result.StatusCode);
        Assert.Single(resultValue);
        Assert.Equal("Hoodie", resultValue.First().Name);
    }

    [Fact]
    public void GetProductById_GivenExistingId_ReturnsProductOfGivenId()
    {
        var dbContext = CreateDbContext(_options, _products);
        var service = new ProductsService(dbContext);
        var controller = new ProductsController(service);

        var result = (ObjectResult)controller.GetProductById(1).Result!;
        var resultValue = (Product)result.Value!;

        Assert.Equal(200, result.StatusCode);
        Assert.NotNull(resultValue);
        Assert.Equal("Hat", resultValue.Name);
    }

    [Fact]
    public void GetProductById_GivenNonExistingId_ThrowsNotFoundException()
    {
        var dbContext = CreateDbContext(_options, _products);
        var service = new ProductsService(dbContext);
        var controller = new ProductsController(service);
        var id = -1;

        var result = () => controller.GetProductById(id);
        
        var exception = Assert.Throws<NotFoundException>(result);
        Assert.Equal($"Product of id: {id} not found", exception.Message);
    }

    [Fact]
    public void Delivery_GivenExistingProducts_UpdatesExistingProductsQuantities()
    {
        var dbContext = CreateDbContext(_options, _products);
        var service = new ProductsService(dbContext);
        var controller = new ProductsController(service);

        var result = (ObjectResult)controller.Delivery(_products);
        
        Assert.Equal(200, result.StatusCode);
        Assert.Equal(80, dbContext.Products.ToList().ElementAt(0).Quantity);
        Assert.Equal(160, dbContext.Products.ToList().ElementAt(1).Quantity);
    }

    [Fact]
    public void Delivery_GivenNonExistingProducts_CreatesNewProductsEntities()
    {
        var productsFromDelivery = new List<Product>
        {
            new Product
            {
                Sku = "DX218ED",
                Name = "Jacket",
                Quantity = 50,
                Price = 49.90m
            },

            new Product
            {
                Sku = "SI972YV",
                Name = "Pants",
                Quantity = 70,
                Price = 27.90m
            }
        };
        var dbContext = CreateDbContext(_options, _products);
        var service = new ProductsService(dbContext);
        var controller = new ProductsController(service);
        
        var result = (ObjectResult)controller.Delivery(productsFromDelivery);

        Assert.Equal(200, result.StatusCode);
        Assert.Equal("Successful delivery", result.Value);
        Assert.Equal(4, dbContext.Products.Count());
    }

    [Fact]
    public void EditProduct_GivenExistingId_UpdatesProduct()
    {
        var editedProduct = new EditProductDto
        {
            Id = 1,
            Name = "Shirt",
            Price = 19.90m
        };

        var dbContext = CreateDbContext(_options, _products); 
        var service = new ProductsService(dbContext);
        var controller = new ProductsController(service);

        var result = (ObjectResult)controller.EditProduct(editedProduct);
        
        Assert.Equal(200, result.StatusCode);
        Assert.Equal("Shirt", dbContext.Products.Single(p => p.Id == editedProduct.Id).Name);
        Assert.Equal(19.90m, dbContext.Products.Single(p => p.Id == editedProduct.Id).Price);
    }

    [Fact]
    public void EditProduct_GivenNonExistingId_ThrowsNotFoundException()
    {
        var editedProduct = new EditProductDto { Id = -1 };

        var dbContext = CreateDbContext(_options, _products); 
        var service = new ProductsService(dbContext);
        var controller = new ProductsController(service);

        var result = () => controller.EditProduct(editedProduct);

        var exception = Assert.Throws<NotFoundException>(result);
        Assert.Equal($"Product of id: {editedProduct.Id} not found", exception.Message);
    }

    [Fact]
    public void DeleteProduct_GivenExistingId_RemovesProductFromDatabase()
    {
        var dbContext = CreateDbContext(_options, _products);
        var service = new ProductsService(dbContext);
        var controller = new ProductsController(service);

        var result = (ObjectResult)controller.DeleteProduct(1);

        Assert.Equal(200, result.StatusCode);
        Assert.Equal("Product deleted", result.Value);
        Assert.Single(dbContext.Products);
        Assert.Equal("Hoodie", dbContext.Products.First().Name);
    }

    [Fact]
    public void DeleteProduct_GivenNonExistingId_ThrowsNotFoundException()
    {
        var dbContext = CreateDbContext(_options, _products);
        var service = new ProductsService(dbContext);
        var controller = new ProductsController(service);
        var id = -1;

        var result = () => controller.DeleteProduct(id);

        var exception = Assert.Throws<NotFoundException>(result);
        Assert.Equal($"Product of id: {id} not found", exception.Message);
    }

    private ShopManagerDbContext CreateDbContext(DbContextOptions<ShopManagerDbContext> options, List<Product> data)
    {
        var dbContext = new ShopManagerDbContext(options);
        dbContext.AddRange(data);
        dbContext.SaveChanges();

        return dbContext;
    }
}