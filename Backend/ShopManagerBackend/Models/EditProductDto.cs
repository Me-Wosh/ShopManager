using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace ShopManagerBackend.Models;

public class EditProductDto
{
    [Required]
    public int Id { get; set; }
    [Required]
    [MaxLength(50)]
    public string Sku { get; set; } = string.Empty;
    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;
    [Required]
    public int Quantity { get; set; }
    [Required]
    [Precision(14, 2)]
    public decimal Price { get; set; }
    public string? ImagePath { get; set; }
}