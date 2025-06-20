using AutoMapper;
using ItemDashServer.Application.Common;
using ItemDashServer.Application.Products.Commands;
using ItemDashServer.Domain.Entities;

namespace ItemDashServer.Application.Products.CommandHandlers;

public class CreateProductCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : ICreateProductCommandHandler
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<Result<ProductDto>> ExecuteAsync(CreateProductCommand request, CancellationToken cancellationToken)
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
        return Result<ProductDto>.Success(_mapper.Map<ProductDto>(entity));
    }
}