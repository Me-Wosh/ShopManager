using ShopManagerBackend.Entities;
using ShopManagerBackend.Exceptions;
using ShopManagerBackend.Models;

namespace ShopManagerBackend.Services;

public interface IProductsService
{
    IEnumerable<Product> GetAllProducts();
    IEnumerable<Product> GetAvailableProducts();
    Product GetProductById(int id);
    void Delivery(List<DeliveryProductDto> products);
    void UpdateProductsQuantities(List<UpdateProductQuantityDto> products);
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

    public void Delivery(List<DeliveryProductDto> productsFromDelivery)
    {
        if (productsFromDelivery.Count < 1)
            return;

        List<Product> productsFromDb = GetAllProducts().ToList();

        foreach (DeliveryProductDto product in productsFromDelivery)
        {
            Product? productFromDb = productsFromDb.SingleOrDefault(p => p.Sku == product.Sku);

            if (productFromDb is not null)
                productFromDb.Quantity += product.Quantity;

            else
            {
                _dbContext.Products.Add(new Product
                {
                    Sku = product.Sku,
                    Name = product.Name,
                    Quantity = product.Quantity,
                    Price = product.Price,
                    ImagePath = product.ImagePath
                });
            }
        }

        _dbContext.SaveChanges();
    }

    public void UpdateProductsQuantities(List<UpdateProductQuantityDto> products)
    {
        if (products.Count < 1)
            return;
        
        List<Product> availableProducts = GetAvailableProducts().ToList();
        
        foreach (UpdateProductQuantityDto product in products)
        {
            Product? productFromDb = availableProducts.SingleOrDefault(p => p.Id == product.Id);

            if (productFromDb is not null)
                productFromDb.Quantity += product.Quantity;
        }

        _dbContext.SaveChanges();
    }

    public void EditProduct(EditProductDto product)
    {
        Product? productFromDb = _dbContext.Products.SingleOrDefault(p => p.Id == product.Id);
        
        if (productFromDb is null)
            throw new NotFoundException($"Product of id: {product.Id} not found");
        
        if (product.Sku != null && product.Sku != productFromDb.Sku)
            productFromDb.Sku = product.Sku;
        
        if (product.Name != null && product.Name != productFromDb.Name)
            productFromDb.Name = product.Name;

        if (product.Quantity != null && product.Quantity != productFromDb.Quantity)
            productFromDb.Quantity = (int)product.Quantity;

        if (product.Price != null && product.Price != productFromDb.Price)
            productFromDb.Price = (decimal)product.Price;

        if (product.ImagePath != null && product.ImagePath != productFromDb.ImagePath)
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