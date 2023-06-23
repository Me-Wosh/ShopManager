using System.ComponentModel.DataAnnotations;

namespace ShopManagerBackend.Models;

public class UpdateProductQuantityDto
{
    [Required]
    public int Id { get; set; }
    [Required]
    public int Quantity { get; set; }
}