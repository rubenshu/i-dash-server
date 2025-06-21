using System.ComponentModel.DataAnnotations;

namespace ItemDashServer.Domain.Entities;

public class Product
{
    [Required]
    public int Id { get; set; }
    [Required]
    public required string Name { get; set; }
    [Required]
    public required string Description { get; set; }
    [Required]
    public required decimal Price { get; set; }

    public ICollection<ProductCategory> ProductCategories { get; set; } = [];
}