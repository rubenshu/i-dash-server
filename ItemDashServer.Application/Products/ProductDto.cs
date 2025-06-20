namespace ItemDashServer.Application.Products;

public class ProductDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public List<CategorySimpleDto> Categories { get; set; } = [];
}

public class CategorySimpleDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}