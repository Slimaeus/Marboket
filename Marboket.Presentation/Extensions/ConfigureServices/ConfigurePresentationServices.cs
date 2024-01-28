using Marboket.Presentation.Endpoints.Api;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.OpenApi.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Marboket.Presentation.Extensions.ConfigureServices;

public static class ConfigurePresentationServices
{
    public static IServiceCollection AddPresentationServices(this IServiceCollection services)
    {
        services.Configure<JsonOptions>(options =>
        {
            options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        });
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            var securitySchema = new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            };

            options.AddSecurityDefinition("Bearer", securitySchema);

            var securityRequirement = new OpenApiSecurityRequirement
                {
                    { securitySchema, ["Bearer"] }
                };

            options.AddSecurityRequirement(securityRequirement);
        });
        return services;
    }
    public static WebApplication UsePresentationServices(this WebApplication app)
    {
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.EnableFilter();
                options.EnableTryItOutByDefault();
                options.EnablePersistAuthorization();
            });
        }

        // --- Map Endpoints
        app.MapGet("antiforgery/token", (IAntiforgery forgeryService, HttpContext context) =>
        {
            var tokens = forgeryService.GetAndStoreTokens(context);
            var xsrfToken = tokens.RequestToken!;
            return TypedResults.Content(xsrfToken, "text/plain");
        });
        //.RequireAuthorization();
        var endpointClasses = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())
            .Where(x => typeof(IEndpoints).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
            .ToList();

        foreach (var endpointClass in endpointClasses)
        {
            if (Activator.CreateInstance(endpointClass, app.MapGroup("api")) is not IEndpoints endpointInstance)
            {
                throw new ArgumentNullException(nameof(endpointInstance).ToString());
            }
        }
        // --- Map Endpoints

        app.UseHttpsRedirection();
        return app;
    }
}


