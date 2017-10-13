using System.Net.Http;
using IdentityServer.ExternalAuth.Data;
using IdentityServer.ExternalAuth.Entities;
using IdentityServer.ExternalAuth.ExtensionGrant;
using IdentityServer.ExternalAuth.Extensions;
using IdentityServer.ExternalAuth.Interfaces.Processors;
using IdentityServer.ExternalAuth.Processors;
using IdentityServer.ExternalAuth.Repositories;
using IdentityServer.ExternalAuth.Repositories.Interfaces;
using IdentityServer4.Validation;
using IdentityServerExternalAuth.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IdentityServerExternalAuth
{
    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddDatabaseConfiguration(Configuration.GetConnectionString("DefaultConnection"))
                    .AddIdentityServerConfig()
                    .AddServices()
                    .AddRepositories()
                    .AddProviders<ApplicationUser>();

            services.AddMvc();
        }

        public IConfiguration Configuration;
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();
            app.UseIdentityServer();
            app.UseMvcWithDefaultRoute();
        }
    }

    public static class Configurations
    {


        public static IServiceCollection AddIdentityServerConfig(this IServiceCollection services)
        {
            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddInMemoryApiResources(Config.GetApiResources())
                .AddInMemoryClients(Config.GetClients())
                .AddInMemoryIdentityResources(Config.GetIdentityResources())
                .AddAspNetIdentity<ApplicationUser>();

            return services;
        }

        public static IServiceCollection AddDatabaseConfiguration(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<DatabaseContext>(cfg => cfg.UseSqlServer(connectionString));
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<DatabaseContext>()
                .AddDefaultTokenProviders();

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<INonEmailUserProcessor, NonEmailUserProcessor<ApplicationUser>>();
            services.AddScoped<IEmailUserProcessor, EmailUserProcessor<ApplicationUser>>();
            services.AddScoped<IExtensionGrantValidator, ExternalAuthenticationGrant<ApplicationUser>>();
            services.AddSingleton<HttpClient>();
            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IProviderRepository, ProviderRepository>();
            return services;
        }
    }
}
