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
      options.IncludeXmlComments(xmlPath);
    });
  }
}