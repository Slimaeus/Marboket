using CloudinaryDotNet;
using Marboket.Infrastructure.Photos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Marboket.Presentation.Extensions.ConfigureServices;

public static class ConfigureInfrastructureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        string? tokenKey = configuration["TokenKey"];
        ArgumentException.ThrowIfNullOrEmpty(tokenKey);
        SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(tokenKey));

        services.AddAuthorization();
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromSeconds(5)
                };
            });

        services.Configure<CloudinarySettings>(configuration.GetSection("Cloudinary"));
        services.AddSingleton(sp =>
        {
            var settings = sp.GetRequiredService<IOptions<CloudinarySettings>>().Value;

            return new Cloudinary(new Account(
                settings.CloudName,
                settings.ApiKey,
                settings.ApiSecret
            ));
        });
        services.AddScoped<IPhotoService, CloudinaryService>();

        return services;
    }
}
