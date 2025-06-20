using AutoMapper;
using ItemDashServer.Domain.Entities;
using ItemDashServer.Application.Users;
using ItemDashServer.Application.Products;
using ItemDashServer.Application.Categories;

namespace ItemDashServer.Application;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, UserDto>()
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role))
            .ForMember(dest => dest.Rights, opt => opt.MapFrom(src => src.Rights));

        // Product -> ProductDto (with simple categories)
        CreateMap<Product, ProductDto>()
            .ForMember(dest => dest.Categories,
                opt => opt.MapFrom(src => src.ProductCategories.Select(pc => pc.Category)));

        // Category -> CategorySimpleDto
        CreateMap<Category, CategorySimpleDto>();

        // Category -> CategoryDto (with simple products)
        CreateMap<Category, CategoryDto>()
            .ForMember(dest => dest.Products,
                opt => opt.MapFrom(src => src.ProductCategories.Select(pc => pc.Product)));

        // Product -> ProductSimpleDto
        CreateMap<Product, ProductSimpleDto>();
    }
}