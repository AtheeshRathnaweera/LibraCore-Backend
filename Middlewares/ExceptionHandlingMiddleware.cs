using System.Net;
using System.Text.Json;
using LibraCore.Backend.DTOs;

namespace LibraCore.Backend.Middlewares;

public class ExceptionHandlingMiddleware
{
  private readonly RequestDelegate _next;
  private readonly ILogger<ExceptionHandlingMiddleware> _logger;

  public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
  {
    _next = next;
    _logger = logger;
  }

  public async Task InvokeAsync(HttpContext context)
  {
    try
    {
      _logger.LogInformation("Processing request: {Method} {Path}", context.Request.Method, context.Request.Path);
      await _next(context); // Proceed to the next middleware
      _logger.LogInformation("Request processed successfully: {Method} {Path}", context.Request.Method, context.Request.Path);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "An error occurred while processing the request: {Method} {Path}", context.Request.Method, context.Request.Path);
      await HandleExceptionAsync(context, ex);
    }
  }

  private Task HandleExceptionAsync(HttpContext context, Exception exception)
  {
    _logger.LogInformation("Handling exception: {ExceptionType}", exception.GetType().Name);

    // Determine the status code and message based on exception type
    var (statusCode, message) = exception switch
    {
      ArgumentNullException => ((int)HttpStatusCode.BadRequest, "Required argument is missing."),
      UnauthorizedAccessException => ((int)HttpStatusCode.Unauthorized, "Access is denied. Please check your credentials."),
      InvalidOperationException => ((int)HttpStatusCode.Conflict, "The operation is not valid in the current state."),
      KeyNotFoundException => ((int)HttpStatusCode.NotFound, "The requested resource could not be found."),
      _ => ((int)HttpStatusCode.InternalServerError, "An unexpected error occurred. Please try again later.")
    };

    var response = new ExceptionHandlerResponse
    {
      Message = message,
      Timestamp = DateTime.UtcNow,
      Debug = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development"
        ? new ExceptionDebugInfo
        {
          ExceptionType = exception.GetType().Name,
          StackTrace = exception.StackTrace ?? string.Empty
        }
        : null
    };

    var options = context.RequestServices.GetRequiredService<JsonSerializerOptions>();

    context.Response.ContentType = "application/json";
    context.Response.StatusCode = statusCode;
    return context.Response.WriteAsync(JsonSerializer.Serialize(response, options));
  }
}
