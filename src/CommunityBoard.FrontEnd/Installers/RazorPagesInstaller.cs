using CommunityBoard.Core.Interfaces.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net;
using System.Net.Mail;

namespace CommunityBoard.FrontEnd.Installers
{
    public class RazorPagesInstaller : IInstaller
    {
        public void InstallServices(
            IServiceCollection services, 
            IConfiguration configuration,
            IWebHostEnvironment environment)
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

			services.AddFluentEmail("test@localhost.com")
				.AddRazorRenderer()
				.AddSmtpSender(new SmtpClient("localhost")
				{
					DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory,
					PickupDirectoryLocation = configuration["TestEmailDirectory"]
				    //EnableSsl = false,
					//DeliveryMethod = SmtpDeliveryMethod.Network,
					//Port = 25,
					//UseDefaultCredentials= false,
					//Credentials = new NetworkCredential("test@localhost.com", "Test1234")
			    });

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