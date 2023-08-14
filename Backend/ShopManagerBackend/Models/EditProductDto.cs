using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace ShopManagerBackend.Models;

public class EditProductDto
{
    [Required]
    public int Id { get; set; }
    [MaxLength(50)]
    public string? Sku { get; set; }
    [MaxLength(50)]
    public string? Name { get; set; } 
    public int? Quantity { get; set; }
    [Precision(14, 2)]
    public decimal? Price { get; set; }
    public string? ImagePath { get; set; }
}