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
            services.AddRazorPages().AddRazorRuntimeCompilation();
            services.AddDistributedMemoryCache();
            services.AddSession();

            //This added authentication to our site!
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, cookieAuthOptions =>
                {
                    cookieAuthOptions.Cookie.Name = "MyApplicationCookie";
                    cookieAuthOptions.LoginPath = "/signIn";
                    cookieAuthOptions.LogoutPath = "/signOut";
                    cookieAuthOptions.AccessDeniedPath = "/accessDenied";
                });
        }
    }
}