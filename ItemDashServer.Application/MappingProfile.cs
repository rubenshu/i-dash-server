using AutoMapper;
using ItemDashServer.Domain.Entities;
using ItemDashServer.Application.Users;
using ItemDashServer.Application.Products;

namespace ItemDashServer.Application;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Product, ProductDto>().ReverseMap();
        CreateMap<User, UserDto>();
    }
}