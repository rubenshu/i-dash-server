using AutoMapper;
using ItemDashServer.Application.Products.Commands;
using ItemDashServer.Application.Products.Repositories;
using ItemDashServer.Domain.Entities;
using MediatR;

namespace ItemDashServer.Application.Products.CommandHandlers;

public class CreateProductCommandHandler(IProductRepository productRepository, IMapper mapper) : IRequestHandler<CreateProductCommand, ProductDto>
{
    private readonly IProductRepository _productRepository = productRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<ProductDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var entity = new Product
        {
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
        };

        await _productRepository.AddAsync(entity, cancellationToken);
        return _mapper.Map<ProductDto>(entity);
    }
}