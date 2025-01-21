using System.Text.Json;
using LibraCore.Backend.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;

namespace LibraCore.Backend.Middlewares;

public class AuthorizationErrorHandlingMiddleware : IAuthorizationMiddlewareResultHandler
{
  private readonly IAuthorizationMiddlewareResultHandler _defaultHandler;
  private readonly ILogger<AuthorizationErrorHandlingMiddleware> _logger;

  public AuthorizationErrorHandlingMiddleware(ILogger<AuthorizationErrorHandlingMiddleware> logger)
  {
    _logger = logger;
    _defaultHandler = new AuthorizationMiddlewareResultHandler();
  }

  public async Task HandleAsync(RequestDelegate next, HttpContext context, AuthorizationPolicy policy, PolicyAuthorizationResult policyAuthorizationResult)
  {
    if (policyAuthorizationResult.Succeeded)
    {
      _logger.LogInformation("Authorization succeeded.");
      // Authorization succeeded, continue to the next middleware
      await next(context);
    }
    else if (policyAuthorizationResult.Forbidden)
    {
      _logger.LogWarning("Authorization failed: Access denied due to insufficient permissions.");

      var errorResponse = new AuthorizationErrorResponse
      {
        Message = "Access denied due to insufficient permissions.",
        Reasons = []
      };

      var failureReasons = policyAuthorizationResult.AuthorizationFailure?.FailureReasons
          .Select(reason => reason.Message)
          .ToList();

      if (failureReasons != null && failureReasons.Count > 0)
      {
        _logger.LogWarning($"Authorization failure reasons: {string.Join(", ", failureReasons)}");
        context.Items["AuthorizationFailureReason"] = failureReasons;
        errorResponse.Reasons = failureReasons;
      }
      else
      {
        _logger.LogWarning("No specific failure reasons provided.");
      }

      context.Response.ContentType = "application/json";
      context.Response.StatusCode = StatusCodes.Status403Forbidden;
      await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
    }
    else if (policyAuthorizationResult.Challenged)
    {
      _logger.LogWarning("Authorization challenge: Access denied due to missing credentials.");

      var errorResponse = new AuthorizationErrorResponse
      {
        Message = "Access denied due to missing credentials.",
        Reasons = []
      };

      context.Response.ContentType = "application/json";
      context.Response.StatusCode = StatusCodes.Status401Unauthorized;
      await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
    }
    else
    {
      _logger.LogInformation("Handling authorization result with default handler.");
      await _defaultHandler.HandleAsync(next, context, policy, policyAuthorizationResult);
    }
  }
}