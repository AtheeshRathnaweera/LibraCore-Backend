using System.Reflection;
using Microsoft.OpenApi.Models;

namespace LibraCore.Backend.Configurations;

public static class SwaggerConfiguration
{
  public static void AddSwaggerConfiguration(this IServiceCollection services)
  {
    services.AddSwaggerGen(options =>
    {
      var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
      var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

      // Add Swagger document information
      options.SwaggerDoc("v1", new()
      {
        Title = "LibraCore API V1",
        Version = "v1",
        Description = "This is a hands-on project for learning .NET API development,"
          + "aimed at creating the backend for a Library Management System.",
        License = new OpenApiLicense
        {
          Name = "MIT",
          Url = new Uri("https://opensource.org/licenses/MIT")
        }
      });

      // Include XML comments in Swagger
      options.IncludeXmlComments(xmlPath);

      // Add security definition for JWT Bearer authentication
      options.AddSecurityDefinition("bearerAuth", new OpenApiSecurityScheme
      {
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Description = "JWT Authorization header using the Bearer scheme. The token must be generated using an Auth0 client.",
        In = ParameterLocation.Header,
      });

      // Add security requirement for JWT Bearer authentication
      options.AddSecurityRequirement(new OpenApiSecurityRequirement
      {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "bearerAuth" }
            },
            new string[] {}
        }
      });
    });
  }
}