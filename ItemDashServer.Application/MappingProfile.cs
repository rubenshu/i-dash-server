using AutoMapper;
using ItemDashServer.Domain.Entities;
using ItemDashServer.Application.Users;
using ItemDashServer.Application.Products;
using ItemDashServer.Application.Categorys;
using ItemDashServer.Application.ProductCategorys;

namespace ItemDashServer.Application;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, UserDto>();
        CreateMap<ProductCategory, ProductCategoryDto>();

        CreateMap<Product, ProductDto>()
            .ForMember(dest => dest.CategoryIds,
                opt => opt.MapFrom(src => src.ProductCategories.Select(pc => pc.CategoryId).ToList()));

        CreateMap<Category, CategoryDto>()
            .ForMember(dest => dest.ProductIds,
                opt => opt.MapFrom(src => src.ProductCategories.Select(pc => pc.ProductId).ToList()));
    }
}