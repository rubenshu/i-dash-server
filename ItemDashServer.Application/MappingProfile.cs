using AutoMapper;
using ItemDashServer.Domain.Entities;
using ItemDashServer.Application.Users;
using ItemDashServer.Application.Products;
using ItemDashServer.Application.Categorys;

namespace ItemDashServer.Application;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, UserDto>(); // No ReverseMap needed for UserDto as it is read-only in this context;
        CreateMap<Category, CategoryDto>().ReverseMap();
        CreateMap<Product, ProductDto>().ReverseMap();
    }
}