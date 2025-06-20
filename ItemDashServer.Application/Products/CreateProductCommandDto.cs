namespace ItemDashServer.Application.Products;

public class CreateProductCommandDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public List<int> CategoryIds { get; set; } = [];
}