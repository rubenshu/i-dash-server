using System.ComponentModel.DataAnnotations;

namespace ItemDashServer.Domain.Entities;

public class Product
{
    [Required]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; } = default!;
    [Required]
    public string Description { get; set; } = default!;
    [Required]
    public decimal Price { get; set; } = default!;

    public ICollection<ProductCategory> ProductCategories { get; set; } = [];
}