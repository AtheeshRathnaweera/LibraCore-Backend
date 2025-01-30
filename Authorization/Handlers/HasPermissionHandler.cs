using LibraCore.Backend.Authorization.Requirements;
using Microsoft.AspNetCore.Authorization;

namespace LibraCore.Backend.Authorization.Handlers;

public class HasPermissionHandler : AuthorizationHandler<HasPolicyRequirement>
{
  private readonly ILogger<HasPermissionHandler> _logger;

  public HasPermissionHandler(ILogger<HasPermissionHandler> logger)
  {
    _logger = logger;
  }

  protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, HasPolicyRequirement requirement)
  {
    _logger.LogInformation("Validating authorization for policy: '{Policy}' with issuer: '{Issuer}'.",
        requirement.Policy, requirement.Issuer);

    var permissionClaim = context.User.FindAll(c =>
      c.Type == "permissions" &&
      string.Equals(new Uri(c.Issuer).Host, requirement.Issuer, StringComparison.OrdinalIgnoreCase));

    if (permissionClaim == null)
    {
      var message = $"Authorization failed: Permissions claim is missing or issuer does not match the expected issuer '{requirement.Issuer}'.";
      _logger.LogWarning(message);
      context.Fail(new AuthorizationFailureReason(this, message));
      return Task.CompletedTask;
    }

    var updatedPermissionClaims = permissionClaim.Select(c => c.Value.Replace("permissions:", string.Empty));
    if (updatedPermissionClaims.Contains(requirement.Policy, StringComparer.OrdinalIgnoreCase))
    {
      _logger.LogInformation("Authorization succeeded: Required permission '{Policy}' is present.", requirement.Policy);
      context.Succeed(requirement);
    }
    else
    {
      var message = $"Authorization failed: Required scope '{requirement.Policy}' not found.";
      _logger.LogWarning(message);
      context.Fail(new AuthorizationFailureReason(this, message));
    }

    return Task.CompletedTask;
  }
}