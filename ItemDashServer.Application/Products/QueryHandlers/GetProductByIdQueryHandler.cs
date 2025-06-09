using AutoMapper;
using ItemDashServer.Application.Products.Queries;
using MediatR;
using ItemDashServer.Application.Products.Repositories;
using ItemDashServer.Application.Common;

namespace ItemDashServer.Application.Products.QueryHandlers;

public class GetProductByIdQueryHandler(IProductRepository productRepository, IMapper mapper) : IRequestHandler<GetProductByIdQuery, Result<ProductDto>>
{
    private readonly IProductRepository _productRepository = productRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<Result<ProductDto>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _productRepository.GetByIdAsync(request.Id, cancellationToken);
        if (entity == null) return Result<ProductDto>.Failure("Product not found");
        return Result<ProductDto>.Success(_mapper.Map<ProductDto>(entity));
    }
}