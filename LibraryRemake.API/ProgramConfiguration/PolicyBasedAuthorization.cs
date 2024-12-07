using System.Security.Claims;

namespace API.ProgramConfiguration
{
    public static class PolicyBasedAuthorization
    {
        public static void AddPolicyBasedAuthorization(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy => policy.RequireClaim(ClaimTypes.Role, "Admin"));
            });
        }
    }
}
