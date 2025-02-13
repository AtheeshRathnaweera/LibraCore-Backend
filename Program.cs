using System.Text.Json;
using FluentValidation;
using LibraCore.Backend.Authorization.Handlers;
using LibraCore.Backend.Configurations;
using LibraCore.Backend.Data;
using LibraCore.Backend.Middlewares;
using LibraCore.Backend.Services.Implementations;
using LibraCore.Backend.Services.Interfaces;
using LibraCore.Backend.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add configuration providers
builder.Configuration.AddEnvironmentVariables();

// Configure Database Context
ConfigureDatabase(builder);

// Register services and add middleware
RegisterServices(builder);

// Build and configure the app
var app = builder.Build();

ConfigurePipeline(app);

app.Run();

void ConfigureDatabase(WebApplicationBuilder builder)
{
  // Retrieve environment variables from the configuration
  var dbHost = builder.Configuration["DB_HOST"];
  var dbPort = builder.Configuration["DB_PORT"];
  var dbName = builder.Configuration["DB_NAME"];
  var mysqlDbUser = builder.Configuration["MYSQL_LOCAL_USER"];
  var mysqlDbPassword = builder.Configuration["MYSQL_LOCAL_PASSWORD"];

  // Ensure variables are not null or empty
  if (string.IsNullOrEmpty(mysqlDbUser) || string.IsNullOrEmpty(mysqlDbPassword))
  {
    throw new InvalidOperationException("Database credentials are not properly configured in environment variables.");
  }

  var connectionString = $"server=localhost;port=3306;user={mysqlDbUser};password={mysqlDbPassword};database={dbName}";

  builder.Services.AddDbContext<MainDBContext>(options =>
  {
    options.UseMySQL(connectionString);
  });
}

// Method to register services
void RegisterServices(WebApplicationBuilder builder)
{
  builder.Services.AddApiVersioning(options =>
  {
    options.DefaultApiVersion = new ApiVersion(1, 0); // Default version: 1.0
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
  });

  SwaggerConfiguration.AddSwaggerConfiguration(builder.Services);

  Auth0Configuration.AddAuth0Authentication(builder.Services, builder.Configuration);
  Auth0Configuration.AddAuth0Authorization(builder.Services, builder.Configuration);

  builder.Services.AddSingleton<IAuthorizationHandler, HasPermissionHandler>();
  builder.Services.AddScoped<
    IAuthorizationMiddlewareResultHandler, AuthorizationErrorHandlingMiddleware>();

  builder.Services.AddScoped<IRoleService, RoleService>();
  builder.Services.AddScoped<IUserStatusService, UserStatusService>();
  builder.Services.AddScoped<IUserService, UserService>();
  builder.Services.AddScoped<IUserActiveStatusService, UserActiveStatusService>();
  builder.Services.AddScoped<IAuthorService, AuthorService>();

  // Configure global JsonSerializerOptions in the DI container
  var globalJsonOptions = new JsonSerializerOptions
  {
    PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower, // Snake_case conversion
    WriteIndented = false
  };
  builder.Services.AddSingleton(globalJsonOptions);

  // Registers all validators from the assembly that contains the CreateRoleRequestValidator class
  builder.Services.AddValidatorsFromAssemblyContaining<CreateRoleRequestValidator>();

  builder.Services.AddControllers().AddJsonOptions(options =>
  {
    options.JsonSerializerOptions.PropertyNamingPolicy = globalJsonOptions.PropertyNamingPolicy;
    options.JsonSerializerOptions.WriteIndented = globalJsonOptions.WriteIndented;
  });

  builder.Services.AddEndpointsApiExplorer();
  builder.Services.AddSwaggerGen();
}

// Method to configure the HTTP request pipeline
void ConfigurePipeline(WebApplication app)
{
  if (app.Environment.IsDevelopment())
  {
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
      options.SwaggerEndpoint("/swagger/v1/swagger.json", "LibraCore API V1");
    });
  }

  app.UseMiddleware<ExceptionHandlingMiddleware>();

  app.UseHttpsRedirection();
  app.UseAuthorization();
  app.MapControllers();
}