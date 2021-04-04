using CommunityBoard.Core.Interfaces.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
			services.AddFluentEmail(configuration["DefaultEmailSender"])
				.AddRazorRenderer()
				.AddSmtpSender(new SmtpClient(configuration["EmailHost"])
				{
					DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory,
					PickupDirectoryLocation = configuration["TestEmailDirectory"]
					//EnableSsl = false,
					//DeliveryMethod = SmtpDeliveryMethod.Network,
					//Port = 25,
					//UseDefaultCredentials= false,
					//Credentials = new NetworkCredential("test@localhost.com", "Test1234")
				});
		}
	}
}