namespace ItemDashServer.Application.Categorys;

public record CategoryDto(int Id, string Name, string Description, decimal Price, List<int> ProductIds);