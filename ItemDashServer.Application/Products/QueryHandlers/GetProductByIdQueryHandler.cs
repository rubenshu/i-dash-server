using AutoMapper;
using ItemDashServer.Application.Products.Queries;
using ItemDashServer.Application.Products.Repositories;
using ItemDashServer.Application.Common;

namespace ItemDashServer.Application.Products.QueryHandlers;

public class GetProductByIdQueryHandler(IProductRepository productRepository, IMapper mapper) : IGetProductByIdQueryHandler
{
    private readonly IProductRepository _productRepository = productRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<Result<ProductDto>> ExecuteAsync(GetProductByIdQuery query, CancellationToken cancellationToken)
    {
        var entity = await _productRepository.GetByIdAsync(query.Id, cancellationToken);
        if (entity == null) return Result<ProductDto>.Failure("Product not found");
        return Result<ProductDto>.Success(_mapper.Map<ProductDto>(entity));
    }
}