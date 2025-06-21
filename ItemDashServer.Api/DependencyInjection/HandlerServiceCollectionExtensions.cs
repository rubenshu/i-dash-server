using ItemDashServer.Application.Products.QueryHandlers;
using ItemDashServer.Application.Products.CommandHandlers;
using ItemDashServer.Application.Categories.QueryHandlers;
using ItemDashServer.Application.Categories.CommandHandlers;
using ItemDashServer.Application.Users.QueryHandlers;
using ItemDashServer.Application.Users.CommandHandlers;
using ItemDashServer.Application.Services;
using ItemDashServer.Application.Products.Repositories;
using ItemDashServer.Application.Categories.Repositories;
using ItemDashServer.Application.Users.Repositories;
using ItemDashServer.Infrastructure.Persistence;
using ItemDashServer.Api.Services;
using ItemDashServer.Application;

namespace ItemDashServer.Api.DependencyInjection;

public static class HandlerServiceCollectionExtensions
{
    public static IServiceCollection AddHandlerServices(this IServiceCollection services)
    {
        // Unit of Work
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Logger
        services.AddSingleton<Application.Common.ILogger, ConsoleLogger>();

        // Services
        services.AddScoped<IAuthService, AuthService>();

        // Repositories
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        // Products
        services.AddScoped<IGetProductsQueryHandler, GetProductsQueryHandler>();
        services.AddScoped<IGetProductByIdQueryHandler, GetProductByIdQueryHandler>();
        services.AddScoped<ICreateProductCommandHandler, CreateProductCommandHandler>();
        services.AddScoped<IUpdateProductCommandHandler, UpdateProductCommandHandler>();
        services.AddScoped<IDeleteProductCommandHandler, DeleteProductCommandHandler>();

        // Categories
        services.AddScoped<IGetCategoriesQueryHandler, GetCategoriesQueryHandler>();
        services.AddScoped<IGetCategoryByIdQueryHandler, GetCategoryByIdQueryHandler>();
        services.AddScoped<ICreateCategoryCommandHandler, CreateCategoryCommandHandler>();
        services.AddScoped<IUpdateCategoryCommandHandler, UpdateCategoryCommandHandler>();
        services.AddScoped<IDeleteCategoryCommandHandler, DeleteCategoryCommandHandler>();

        // Users/Auth
        services.AddScoped<ILoginUserQueryHandler, LoginUserQueryHandler>();
        services.AddScoped<IGetUserByRefreshTokenQueryHandler, GetUserByRefreshTokenQueryHandler>();
        services.AddScoped<IRegisterUserCommandHandler, RegisterUserCommandHandler>();
        services.AddScoped<IUpdateUserRefreshTokenCommandHandler, UpdateUserRefreshTokenCommandHandler>();
        services.AddScoped<IRefreshUserCommandHandler, RefreshUserCommandHandler>();

        return services;
    }
}
