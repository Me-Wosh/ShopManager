using ShopManagerBackend.Entities;
using ShopManagerBackend.Exceptions;

namespace ShopManagerBackend.Services;

public interface IProductsService
{
    IEnumerable<Product> GetAllProducts();
    Product GetProductById(int id);
}

public class ProductsService : IProductsService
{
    private readonly ShopManagerDbContext _dbContext;

    public ProductsService(ShopManagerDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public IEnumerable<Product> GetAllProducts()
    {
        return _dbContext.Products.ToList();
    }

    public Product GetProductById(int id)
    {
        Product? product = _dbContext.Products.SingleOrDefault(p => p.Id == id);
        
        return product ?? throw new NotFoundException($"Product of id: {id} not found");
    }
}