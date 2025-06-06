using AutoMapper;
using ItemDashServer.Application.Products.Commands;
using ItemDashServer.Domain.Entities;
using MediatR;
using ItemDashServer.Infrastructure.Persistence; 

namespace ItemDashServer.Application.Products.CommandHandlers;

public class CreateProductCommandHandler(ApplicationDbContext context, IMapper mapper) : IRequestHandler<CreateProductCommand, ProductDto>
{
    private readonly ApplicationDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task<ProductDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var entity = new Product
        {
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
        };

        _context.Products.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<ProductDto>(entity);
    }
}