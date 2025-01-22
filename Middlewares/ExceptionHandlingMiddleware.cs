using System.Net;
using System.Text.Json;
using LibraCore.Backend.DTOs;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;

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
      DbUpdateException dbUpdateEx when dbUpdateEx.InnerException is MySqlException mySqlEx => HandleMySqlException(mySqlEx),
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

  private (int statusCode, string message) HandleMySqlException(MySqlException mySqlEx)
  {
    var baseMessage = mySqlEx.Message;

    return mySqlEx.Number switch
    {
      1062 => ((int)HttpStatusCode.Conflict, $"A unique constraint violation occurred: {baseMessage}"),
      1451 => ((int)HttpStatusCode.BadRequest, "Cannot delete or update because of existing references to this resource."),
      1452 => ((int)HttpStatusCode.BadRequest, "Cannot add or update due to invalid foreign key reference."),
      1146 => ((int)HttpStatusCode.InternalServerError, "The specified table does not exist. Please contact support."),
      1048 => ((int)HttpStatusCode.BadRequest, $"A required column is missing or contains null data: {baseMessage}"),
      1364 => ((int)HttpStatusCode.BadRequest, $"A required field is missing and has no default value: {baseMessage}"),
      1054 => ((int)HttpStatusCode.InternalServerError, $"The specified column does not exist in the database: {baseMessage}"),
      1406 => ((int)HttpStatusCode.BadRequest, $"The provided data exceeds the maximum allowed length: {baseMessage}"),
      1216 => ((int)HttpStatusCode.BadRequest, $"Foreign key constraint failed while adding or updating the record: {baseMessage}"),
      1217 => ((int)HttpStatusCode.BadRequest, $"Foreign key constraint failed while deleting the record: {baseMessage}"),
      _ => ((int)HttpStatusCode.InternalServerError, $"A database error occurred: {baseMessage}")
    };
  }
}
