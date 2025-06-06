using AutoMapper;
using ItemDashServer.Application.Categorys.Commands;
using ItemDashServer.Domain.Entities;
using MediatR;
using ItemDashServer.Infrastructure.Persistence; 

namespace ItemDashServer.Application.Categorys.CommandHandlers;

public class CreateCategoryCommandHandler(ApplicationDbContext context, IMapper mapper) : IRequestHandler<CreateCategoryCommand, CategoryDto>
{
    private readonly ApplicationDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task<CategoryDto> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var entity = new Category
        {
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
        };

        _context.Categorys.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<CategoryDto>(entity);
    }
}