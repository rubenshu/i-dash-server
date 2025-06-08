using AutoMapper;
using ItemDashServer.Application;
using ItemDashServer.Application.Products.Commands;
using ItemDashServer.Application.Products.Repositories;
using ItemDashServer.Domain.Entities;
using MediatR;

namespace ItemDashServer.Application.Products.CommandHandlers;

public class CreateProductCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<CreateProductCommand, ProductDto>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<ProductDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var entity = new Product
        {
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
        };

        if (request.CategoryIds != null)
        {
            var productCategories = new List<ProductCategory>();
            foreach (var categoryId in request.CategoryIds)
            {
                var category = await _unitOfWork.Categories.GetByIdAsync(categoryId, cancellationToken);
                if (category != null)
                {
                    productCategories.Add(new ProductCategory
                    {
                        Product = entity,
                        CategoryId = category.Id
                    });
                }
            }
            entity.ProductCategories = productCategories;
        }

        await _unitOfWork.Products.AddAsync(entity, cancellationToken);
        await _unitOfWork.CommitAsync();
        return _mapper.Map<ProductDto>(entity);
    }
}