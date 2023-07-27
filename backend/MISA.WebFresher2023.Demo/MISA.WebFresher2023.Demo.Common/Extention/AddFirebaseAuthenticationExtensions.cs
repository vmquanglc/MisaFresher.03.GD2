using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;

namespace MISA.WebFresher2023.Demo.Common
{
    public static class AddFirebaseAuthenticationExtensions
    {
        public static IServiceCollection AddFirebaseAuthentication(this IServiceCollection services)
        {
            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddScheme<AuthenticationSchemeOptions, FirebaseAuthenticationHandler>(JwtBearerDefaults.AuthenticationScheme, (o) => { });

            services.AddScoped<FirebaseAuthenticationFunctionHandler>();

            return services;
        }
    }
}
