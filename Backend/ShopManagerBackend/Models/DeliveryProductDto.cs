using System.ComponentModel.DataAnnotations;

namespace ShopManagerBackend.Models;

public class DeliveryProductDto
{
    [Required]
    [MaxLength(50)]
    public string Sku { get; set; }
    [Required]
    [MaxLength(50)]
    public string Name { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public string? ImagePath { get; set; }
}
