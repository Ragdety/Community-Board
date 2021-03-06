using CommunityBoard.Core.Interfaces.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CommunityBoard.FrontEnd.Installers
{
    public class RazorPagesInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddRazorPages(options =>
            {
                options.Conventions.AuthorizePage("/PostManagement/Create");
                options.Conventions.AuthorizePage("/PostManagement/Update");
                options.Conventions.AuthorizePage("/PostManagement/Manage");
            }).AddRazorRuntimeCompilation();

            services.AddDistributedMemoryCache();
            services.AddSession();

            //This added authentication to our site!
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, cookieAuthOptions =>
                {
                    cookieAuthOptions.Cookie.Name = "MyApplicationCookie";
                    cookieAuthOptions.LoginPath = "/Authentication/Register";
                    cookieAuthOptions.LogoutPath = "/Authentication/Logout";
                    cookieAuthOptions.AccessDeniedPath = "/accessDenied";
                });
        }
    }
}