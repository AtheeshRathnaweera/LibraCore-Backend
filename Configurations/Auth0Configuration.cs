using System.Security.Claims;
using LibraCore.Backend.Authorization.Requirements;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace LibraCore.Backend.Configurations;

public static class Auth0Configuration
{
  public static void AddAuth0Authentication(this IServiceCollection services, IConfiguration configuration)
  {
    services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
          options.Authority = $"https://{configuration["AUTH0_DOMAIN"]}/";
          options.Audience = configuration["AUTH0_AUDIENCE"];
          options.TokenValidationParameters = new TokenValidationParameters
          {
            NameClaimType = ClaimTypes.NameIdentifier
          };
        });
  }

  public static void AddAuth0Authorization(this IServiceCollection services, IConfiguration configuration)
  {
    var auth0Domain = configuration["AUTH0_DOMAIN"] ?? throw new InvalidOperationException("Auth0:Domain is not configured.");

    var policies = new Dictionary<string, string>
    {
        { "role:read", "role:read" },
        { "role:write", "role:write" },
        { "role:delete", "role:delete" },
        { "user_status:read", "user_status:read" },
        { "user_status:write", "user_status:write" },
        { "user_status:delete", "user_status:delete" },
        { "user:read", "user:read" },
        { "user:write", "user:write" },
        { "user:delete", "user:delete" },
        { "user_active_status:read", "user_active_status:read" },
        { "user_active_status:write", "user_active_status:write" },
        { "user_active_status:delete", "user_active_status:delete" },
        { "author:read", "author:read" },
        { "author:write", "author:write" },
        { "author:delete", "author:delete" },
    };

    var authorizationBuilder = services.AddAuthorizationBuilder();

    foreach (var policy in policies)
    {
      authorizationBuilder.AddPolicy(policy.Key, policyBuilder =>
          policyBuilder.Requirements.Add(new HasPolicyRequirement(policy.Value, auth0Domain))
      );
    }
  }
}