using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ToDo_API.Services;

namespace ToDo_API.Configurations;

public static class ServicesConfiguration
{
    public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddUtilities(configuration);
        services.AddMongoDBService(configuration);
        services.AddToDoServices(configuration);
        services.AddUserServices(configuration);
        
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.Authority = "https://your-identity-provider.com"; // Replace with your identity provider's URL
                options.Audience = "your-client-id"; // Replace with your client ID
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                };
            });
            
        services.AddControllers();

        services.AddCors(options =>
        {
            options.AddPolicy("AllowSpecificOrigins",
                configurePolicy =>
                {
                    configurePolicy.WithOrigins("http://localhost:3000")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
        });
        
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(x => x.SwaggerDoc("v1", new OpenApiInfo{ Title = "ToDo Api", Version = "v1", Description = "ToDo Api"}));
    }
}