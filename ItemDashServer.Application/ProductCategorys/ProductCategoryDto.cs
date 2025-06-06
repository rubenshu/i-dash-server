namespace ItemDashServer.Application.ProductCategorys;

public class ProductCategoryDto(int productId, int categoryId)
{
    public int ProductId { get; set; } = productId;
    public int CategoryId { get; set; } = categoryId;
}