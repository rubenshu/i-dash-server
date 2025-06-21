using AutoMapper;
using ItemDashServer.Application.Common;
using ItemDashServer.Application.Products.Commands;
using ItemDashServer.Domain.Entities;

namespace ItemDashServer.Application.Products.CommandHandlers;

public class CreateProductCommandHandler(ILogger logger, IUnitOfWork unitOfWork, IMapper mapper) : AsyncCommandHandlerBase<CreateProductCommand, Result<ProductDto>>(logger, unitOfWork), ICreateProductCommandHandler
{
    private readonly IMapper _mapper = mapper;

    protected override async Task<Result<ProductDto>> DoHandle(CreateProductCommand request, CancellationToken cancellationToken)
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
                var category = await UnitOfWork.Categories.GetByIdAsync(categoryId, cancellationToken);
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
        await UnitOfWork.Products.AddAsync(entity, cancellationToken);
        return Result<ProductDto>.Success(_mapper.Map<ProductDto>(entity));
    }
}