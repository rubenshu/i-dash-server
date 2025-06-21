using AutoMapper;
using ItemDashServer.Application.Products.Queries;
using ItemDashServer.Application.Products.Repositories;
using ItemDashServer.Application.Common;

namespace ItemDashServer.Application.Products.QueryHandlers;

public class GetProductsQueryHandler(ILogger logger, IProductRepository productRepository, IMapper mapper) : AsyncQueryHandlerBase<GetProductsQuery, Result<IEnumerable<ProductDto>>>(logger), IGetProductsQueryHandler
{
    private readonly IProductRepository _productRepository = productRepository;
    private readonly IMapper _mapper = mapper;

    protected override async Task<Result<IEnumerable<ProductDto>>> DoExecute(GetProductsQuery query, CancellationToken cancellationToken)
    {
        var products = await _productRepository.GetAllAsync(cancellationToken);
        return Result<IEnumerable<ProductDto>>.Success(_mapper.Map<IEnumerable<ProductDto>>(products));
    }
}