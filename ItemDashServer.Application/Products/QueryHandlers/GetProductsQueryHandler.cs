using AutoMapper;
using ItemDashServer.Application.Products.Queries;
using MediatR;
using ItemDashServer.Application.Products.Repositories;
using ItemDashServer.Application.Common;

namespace ItemDashServer.Application.Products.QueryHandlers;

public class GetProductsQueryHandler(IProductRepository productRepository, IMapper mapper) : IRequestHandler<GetProductsQuery, Result<IEnumerable<ProductDto>>>
{
    private readonly IProductRepository _productRepository = productRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<Result<IEnumerable<ProductDto>>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await _productRepository.GetAllAsync(cancellationToken);
        return Result<IEnumerable<ProductDto>>.Success(_mapper.Map<IEnumerable<ProductDto>>(products));
    }
}