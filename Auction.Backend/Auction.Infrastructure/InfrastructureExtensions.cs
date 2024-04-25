﻿using Auction.Application.Auth;
using Auction.Domain.Interfaces.Repositories;
using Auction.Infrastructure.Authentification;
using Auction.Infrastructure.Persistence.Repositories;
using Auction.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

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

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IBetRepository, BetRepository>();

        services.AddScoped<IJwtProvider, JwtProvider>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();

        return services;
    }
}
