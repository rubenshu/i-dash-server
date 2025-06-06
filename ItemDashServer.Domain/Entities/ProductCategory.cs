using System.ComponentModel.DataAnnotations;

namespace ItemDashServer.Domain.Entities;

public class ProductCategory
{
    [Required]
    public int ProductId { get; set; }
    [Required]
    public Product Product { get; set; } = default!;
    [Required]
    public int CategoryId { get; set; }
    [Required]
    public Category Category { get; set; } = default!;
}