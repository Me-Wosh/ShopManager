using Microsoft.AspNetCore.Mvc;
using ShopManagerBackend.Entities;
using ShopManagerBackend.Services;

namespace ShopManagerBackend.Controllers;

[ApiController]
[Route("Api/[Controller]/V1")]
public class ProductsController : ControllerBase
{
    private readonly IProductsService _productsService;

    public ProductsController(IProductsService productsService)
    {
        _productsService = productsService;
    }
    
    [HttpGet]
    public ActionResult<IEnumerable<Product>> GetAllProducts()
    {
        return Ok(_productsService.GetAllProducts());
    }

    [HttpGet("{id}")]
    public ActionResult<Product> GetProductById([FromRoute]int id)
    {
        return Ok(_productsService.GetProductById(id));
    }
}