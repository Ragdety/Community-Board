using CommunityBoard.Core.Interfaces.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

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
                options.Conventions.AuthorizePage("/Contact");
            }).AddRazorRuntimeCompilation();

            services.AddDistributedMemoryCache();
            services.AddSession();

            //This added authentication to our site!
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
                {
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(3);

                    options.Cookie.Name = "MyApplicationCookie";
                    options.LoginPath = "/Authentication/Register";
                    options.LogoutPath = "/Authentication/Logout";
                    options.AccessDeniedPath = "/accessDenied";
                });
        }
    }
}