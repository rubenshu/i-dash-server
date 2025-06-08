using AutoMapper;
using ItemDashServer.Application.Products.Queries;
using MediatR;
using ItemDashServer.Application.Products.Repositories;

namespace ItemDashServer.Application.Products.QueryHandlers;

public class GetProductsQueryHandler(IProductRepository productRepository, IMapper mapper) : IRequestHandler<GetProductsQuery, IEnumerable<ProductDto>>
{
    private readonly IProductRepository _productRepository = productRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<ProductDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await _productRepository.GetAllAsync(cancellationToken);
        return _mapper.Map<IEnumerable<ProductDto>>(products);
    }
}