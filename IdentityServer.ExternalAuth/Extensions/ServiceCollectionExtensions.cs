using IdentityServer.ExternalAuth.Interfaces;
using IdentityServer.ExternalAuth.Providers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityServer.ExternalAuth.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddProviders<TUser>(this IServiceCollection services) where TUser : IdentityUser, new()
        {
            services.AddTransient<IFacebookAuthProvider, FacebookAuthProvider<TUser>>();
            services.AddTransient<ITwitterAuthProvider, TwitterAuthProvider<TUser>>();
            services.AddTransient<IGoogleAuthProvider, GoogleAuthProvider<TUser>>();
            services.AddTransient<ILinkedInAuthProvider, LinkedInAuthProvider<TUser>>();
            return services;
        }
    }
}
