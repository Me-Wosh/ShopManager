using ShopManagerBackend.Entities;
using ShopManagerBackend.Exceptions;
using ShopManagerBackend.Models;

namespace ShopManagerBackend.Services;

public interface IProductsService
{
    IEnumerable<Product> GetAllProducts();
    IEnumerable<Product> GetAvailableProducts();
    Product GetProductById(int id);
    void Delivery(List<Product> products);
    void EditProduct(EditProductDto product);
    void DeleteProduct(int id);
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

    public IEnumerable<Product> GetAvailableProducts()
    {
        return _dbContext.Products.Where(p => p.Quantity > 0).ToList();
    }

    public Product GetProductById(int id)
    {
        Product? product = _dbContext.Products.SingleOrDefault(p => p.Id == id);
        
        return product ?? throw new NotFoundException($"Product of id: {id} not found");
    }

    public void Delivery(List<Product> productsFromDelivery)
    {
        if (productsFromDelivery.Count < 1)
            return;

        List<Product> productsFromDb = GetAllProducts().ToList();

        foreach (Product product in productsFromDelivery)
        {
            Product? productFromDb = productsFromDb.SingleOrDefault(p => p.Sku == product.Sku);

            if (productFromDb is not null)
                productFromDb.Quantity += product.Quantity;

            else
                _dbContext.Products.Add(product);
        }

        _dbContext.SaveChanges();
    }

    public void EditProduct(EditProductDto product)
    {
        Product? productFromDb = _dbContext.Products.SingleOrDefault(p => p.Id == product.Id);
        
        if (productFromDb is null)
            throw new NotFoundException($"Product of id: {product.Id} not found");
        
        if (product.Sku != productFromDb.Sku)
            productFromDb.Sku = product.Sku;
        
        if (product.Name != productFromDb.Name)
            productFromDb.Name = product.Name;

        if (product.Quantity != productFromDb.Quantity)
            productFromDb.Quantity = product.Quantity;

        if (product.Price != productFromDb.Price)
            productFromDb.Price = product.Price;

        if (product.ImagePath != productFromDb.ImagePath)
            productFromDb.ImagePath = product.ImagePath;

        _dbContext.SaveChanges();
    }

    public void DeleteProduct(int id)
    {
        Product? product = _dbContext.Products.SingleOrDefault(p => p.Id == id);

        if (product is null)
            throw new NotFoundException($"Product of id: {id} not found");

        _dbContext.Products.Remove(product);
        _dbContext.SaveChanges();
    }
}