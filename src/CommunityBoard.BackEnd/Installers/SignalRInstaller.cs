using CommunityBoard.Core.Interfaces.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CommunityBoard.BackEnd.Installers
{
	public class SignalRInstaller : IInstaller
	{
		public void InstallServices(
			IServiceCollection services, 
			IConfiguration configuration, 
			IWebHostEnvironment environment)
		{
			services.AddSignalR();
		}
	}
}