using Microsoft.AspNetCore.Authorization;

namespace LibraCore.Backend.Authorization.Requirements;

public class HasPolicyRequirement : IAuthorizationRequirement
{
  public string Issuer { get; }
  
  public string Policy { get; }

  public HasPolicyRequirement(string policy, string issuer)
  {
    Policy = policy ?? throw new ArgumentNullException(nameof(policy));
    Issuer = issuer ?? throw new ArgumentNullException(nameof(issuer));
  }
}