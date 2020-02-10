using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace single_sign_on_client
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddMvc(option => option.EnableEndpointRouting = false);

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            //Client - COnfigure Identity Service 
            ConfigureIdentityServer(services);

        }

        private void ConfigureIdentityServer(IServiceCollection services)
        {
            var builder = services.AddAuthentication(options => SetAuthenticationOptions(options));

            builder.AddCookie();
            builder.AddOpenIdConnect(options => SetOpenIdConnectOptions(options));
        }

        private void SetOpenIdConnectOptions(OpenIdConnectOptions options)
        {
            options.Authority = "http://localhost:4000";
            options.ClientId = "toll.portal";
            options.RequireHttpsMetadata = false;
            options.Scope.Add("profile");
            options.Scope.Add("openid");
            options.Scope.Add("toll.api");
            options.ResponseType = "code id_token";
            options.SaveTokens = true;
            options.ClientSecret = "0b4168e4-2832-48ea-8fc8-7e4686b3620b";

        }

        private void SetAuthenticationOptions(AuthenticationOptions options)
        {
            options.DefaultScheme = Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = Microsoft.AspNetCore.Authentication.OpenIdConnect.OpenIdConnectDefaults.AuthenticationScheme;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseCookiePolicy();

            // Identity Server configuration
            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
