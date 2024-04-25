using Auction.Application.Services;
using Auction.Domain.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Auction.Application;

public static class ApplicationExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IBetService, BetService>();

        return services;
    }
}
