using CommunityBoard.Core.Interfaces.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Mail;

namespace CommunityBoard.FrontEnd.Installers
{
	public class EmailInstaller : IInstaller
	{
		public void InstallServices(
			IServiceCollection services, 
			IConfiguration configuration, 
			IWebHostEnvironment environment)
		{
			services.AddFluentEmail(configuration["GmailEmail"])
				.AddRazorRenderer()
				.AddSmtpSender(new SmtpClient(configuration["GmailEmailHost"])
				{
					//DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory,
					//PickupDirectoryLocation = configuration["TestEmailDirectory"]
					Host = "smtp.gmail.com",
					EnableSsl = true,
					DeliveryMethod = SmtpDeliveryMethod.Network,
					Port = 587,
					UseDefaultCredentials= false,
					Credentials = new NetworkCredential(configuration["GmailEmail"], configuration["Password"])
				});
		}
	}
}