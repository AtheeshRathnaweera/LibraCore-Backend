using System.Text.Json;
using LibraCore.Backend.Data;
using LibraCore.Backend.Services;
using LibraCore.Backend.Services.Implementations;
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
  var mysqlDbUser = builder.Configuration["MYSQL_LOCAL_USER"];
  var mysqlDbPassword = builder.Configuration["MYSQL_LOCAL_PASSWORD"];

  // Ensure variables are not null or empty
  if (string.IsNullOrEmpty(mysqlDbUser) || string.IsNullOrEmpty(mysqlDbPassword))
  {
    throw new InvalidOperationException("Database credentials are not properly configured in environment variables.");
  }

  var connectionString = $"server=localhost;port=3306;user={mysqlDbUser};password={mysqlDbPassword};database=libra_core";

  builder.Services.AddDbContext<MainDBContext>(options =>
  {
    options.UseMySQL(connectionString);
  });
}

// Method to register services
void RegisterServices(WebApplicationBuilder builder)
{
  builder.Services.AddScoped<IRoleService, RoleService>();

  // Configure global JsonSerializerOptions in the DI container
  var globalJsonOptions = new JsonSerializerOptions
  {
    PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower, // Snake_case conversion
    WriteIndented = false
  };
  builder.Services.AddSingleton(globalJsonOptions);

  builder.Services.AddControllers()
  .AddJsonOptions(options =>
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
    app.UseSwaggerUI();
  }

  app.UseMiddleware<ExceptionHandlingMiddleware>();

  app.UseHttpsRedirection();
  app.UseAuthorization();
  app.MapControllers();
}