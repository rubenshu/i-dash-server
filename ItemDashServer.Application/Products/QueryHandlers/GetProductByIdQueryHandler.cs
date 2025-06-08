using AutoMapper;
using ItemDashServer.Application.Products.Queries;
using MediatR;
using ItemDashServer.Application.Products.Repositories;

namespace ItemDashServer.Application.Products.QueryHandlers;

public class GetProductByIdQueryHandler(IProductRepository productRepository, IMapper mapper) : IRequestHandler<GetProductByIdQuery, ProductDto?>
{
    private readonly IProductRepository _productRepository = productRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<ProductDto?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _productRepository.GetByIdAsync(request.Id, cancellationToken);
        if (entity == null) return null;
        return _mapper.Map<ProductDto>(entity);
    }
}