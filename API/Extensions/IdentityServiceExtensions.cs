using System;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Infrastructure.Services;
using Core.Entities.Identity;

namespace API.Extensions;

public static class IdentityServiceExtensions
{
    public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddIdentityCore<AppUser>(opt =>
        {
            opt.Password.RequireNonAlphanumeric = false;
            opt.User.RequireUniqueEmail = true;
        })
        .AddEntityFrameworkStores<DataContext>()
        .AddDefaultTokenProviders();

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Token:Key"]));

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(opt =>
            {
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ValidIssuer = config["Token:Issuer"],
                    ValidateIssuer = true,
                    ValidateAudience = false
                };
            });
        services.AddScoped<TokenService>();

        return services;
    }
}

