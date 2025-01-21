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
        { "roles:read", "roles:read" },
        { "roles:write", "roles:write" },
        { "user_status:read", "user_status:read" },
        { "user_status:write", "user_status:write" }
    };

    var authorizationBuilder = services.AddAuthorizationBuilder();

    foreach (var policy in policies)
    {
      authorizationBuilder.AddPolicy(policy.Key, policyBuilder =>
          policyBuilder.Requirements.Add(new HasScopeRequirement(policy.Value, auth0Domain))
      );
    }
  }
}