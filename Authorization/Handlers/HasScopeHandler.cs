using LibraCore.Backend.Authorization.Requirements;
using Microsoft.AspNetCore.Authorization;

namespace LibraCore.Backend.Authorization.Handlers;

public class HasScopeHandler : AuthorizationHandler<HasScopeRequirement>
{
  private readonly ILogger<HasScopeHandler> _logger;

  public HasScopeHandler(ILogger<HasScopeHandler> logger)
  {
    _logger = logger;
  }

  protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, HasScopeRequirement requirement)
  {
    _logger.LogInformation("Validating authorization for scope: '{Scope}' with issuer: '{Issuer}'.",
        requirement.Scope, requirement.Issuer);

    // Validate if the user has the scope claim with the correct issuer
    var scopeClaim = context.User.FindFirst(c =>
        c.Type == "scope" &&
        string.Equals(new Uri(c.Issuer).Host, requirement.Issuer, StringComparison.OrdinalIgnoreCase));

    if (scopeClaim == null)
    {
      var message = $"Authorization failed: Scope claim is missing or issuer does not match the expected issuer '{requirement.Issuer}'.";
      _logger.LogWarning(message);
      context.Fail(new AuthorizationFailureReason(this, message));
      return Task.CompletedTask;
    }

    // Parse the scope claim value into an array and check if it contains the required scope
    var availableScopes = scopeClaim.Value.Split(' ', StringSplitOptions.RemoveEmptyEntries);
    if (availableScopes.Contains(requirement.Scope, StringComparer.OrdinalIgnoreCase))
    {
      _logger.LogInformation("Authorization succeeded: Required scope '{Scope}' is present.", requirement.Scope);
      context.Succeed(requirement);
    }
    else
    {
      var message = $"Authorization failed: Required scope '{requirement.Scope}' not found. Available scopes: [{string.Join(", ", availableScopes)}].";
      _logger.LogWarning(message);
      context.Fail(new AuthorizationFailureReason(this, message));
    }

    return Task.CompletedTask;
  }
}