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
    // If user does not have the scope claim, get out of here
    if (!context.User.HasClaim(c => c.Type == "scope" && new Uri(c.Issuer).Host == requirement.Issuer))
    {
      _logger.LogWarning("Authorization failed: User does not have the required scope claim.");
      context.Fail(new AuthorizationFailureReason(this, "User does not have the required scope claim."));
      return Task.CompletedTask;
    }

    var scopeClaim = context.User
      .FindFirst(c => c.Type == "scope" && new Uri(c.Issuer).Host == requirement.Issuer);

    if (scopeClaim == null)
    {
      _logger.LogWarning("Authorization failed: Scope claim is null.");
      context.Fail(new AuthorizationFailureReason(this, "Scope claim is null."));
      return Task.CompletedTask;
    }

    // Split the scopes string into an array
    var scopes = scopeClaim.Value.Split(' ');

    // Succeed if the scope array contains the required scope
    if (scopes.Any(scope => scope == requirement.Scope))
    {
      context.Succeed(requirement);
      return Task.CompletedTask;
    }

    _logger.LogWarning($"Authorization failed: Required scope '{requirement.Scope}' not found.");
    context.Fail(new AuthorizationFailureReason(this, $"Required scope '{requirement.Scope}' not found."));
    return Task.CompletedTask;
  }
}