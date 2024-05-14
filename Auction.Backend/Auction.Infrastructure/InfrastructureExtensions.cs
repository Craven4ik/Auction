using Auction.Application.Auth;
using Auction.Domain.Interfaces.Repositories;
using Auction.Infrastructure.Authentification;
using Auction.Infrastructure.Persistence;
using Auction.Infrastructure.Persistence.Factory;
using Auction.Infrastructure.Persistence.Repositories;
using Auction.Infrastructure.Persistence.Schedulers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Auction.Infrastructure;

public static class InfrastructureExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AuctionDbContext>(
            options =>
            {
                options.UseNpgsql(configuration.GetConnectionString(nameof(AuctionDbContext)));
            });

        services.AddSingleton<DbContextFactory>();

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();

        services.AddScoped<IJwtProvider, JwtProvider>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();

        services.AddHostedService<CheckProductStateService>();

        return services;
    }
}
