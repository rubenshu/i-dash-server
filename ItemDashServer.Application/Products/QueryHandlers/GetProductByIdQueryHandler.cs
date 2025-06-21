using AutoMapper;
using ItemDashServer.Application.Products.Queries;
using ItemDashServer.Application.Products.Repositories;
using ItemDashServer.Application.Common.Abstractions;
using ItemDashServer.Application.Common.Handlers;
using ItemDashServer.Application.Common.Results;

namespace ItemDashServer.Application.Products.QueryHandlers;

public class GetProductByIdQueryHandler(ILogger logger, IProductRepository productRepository, IMapper mapper) : AsyncQueryHandlerBase<GetProductByIdQuery, Result<ProductDto>>(logger), IGetProductByIdQueryHandler
{
    private readonly IProductRepository _productRepository = productRepository;
    private readonly IMapper _mapper = mapper;

    protected override async Task<Result<ProductDto>> DoExecute(GetProductByIdQuery query, CancellationToken cancellationToken)
    {
        var entity = await _productRepository.GetByIdAsync(query.Id, cancellationToken);
        if (entity == null) return Result<ProductDto>.Failure("Product not found");
        return Result<ProductDto>.Success(_mapper.Map<ProductDto>(entity));
    }
}