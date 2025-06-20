namespace ItemDashServer.Application.Categories;

public class CategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public List<ProductSimpleDto> Products { get; set; } = [];

}

public class ProductSimpleDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}