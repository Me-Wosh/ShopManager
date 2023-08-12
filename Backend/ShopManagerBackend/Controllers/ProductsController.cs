using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopManagerBackend.Entities;
using ShopManagerBackend.Models;
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

    [HttpGet("Available")]
    public ActionResult<IEnumerable<Product>> GetAvailableProducts()
    {
        return Ok(_productsService.GetAvailableProducts());
    }

    [HttpGet("{id}")]
    public ActionResult<Product> GetProductById([FromRoute]int id)
    {
        return Ok(_productsService.GetProductById(id));
    }

    [HttpPut("Delivery")]
    public ActionResult Delivery([FromBody]List<DeliveryProductDto> products)
    {
        _productsService.Delivery(products);
        
        return Ok("Successful delivery");
    }

    [HttpPatch("UpdateQuantities")]
    public ActionResult UpdateProductsQuantities([FromBody] List<UpdateProductQuantityDto> products)
    {
        _productsService.UpdateProductsQuantities(products);

        return Ok("Quantities updated");
    }

    [HttpPatch("Edit"), Authorize(Roles = "Admin")]
    public ActionResult EditProduct([FromBody]EditProductDto product)
    {
        _productsService.EditProduct(product);

        return Ok("Product updated");
    }

    [HttpDelete("Delete/{id}"), Authorize(Roles = "Admin")]
    public ActionResult DeleteProduct([FromRoute] int id)
    {
        _productsService.DeleteProduct(id);

        return Ok("Product deleted");
    }
}