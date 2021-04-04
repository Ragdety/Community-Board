using CommunityBoard.Core.Interfaces.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace CommunityBoard.FrontEnd.Installers
{
	public class AuthenticationInstaller : IInstaller
	{
		public void InstallServices(
			IServiceCollection services, 
			IConfiguration configuration, 
			IWebHostEnvironment environment)
		{
			services.AddDistributedMemoryCache();
			services.AddSession();

			//This added authentication to our site!
			services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
				.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
				{
					options.ExpireTimeSpan = TimeSpan.FromMinutes(6);

					options.Cookie.Name = "MyApplicationCookie";
					options.LoginPath = "/Authentication/Register";
					options.LogoutPath = "/Authentication/Logout";
					options.AccessDeniedPath = "/accessDenied";
				});
		}
	}
}