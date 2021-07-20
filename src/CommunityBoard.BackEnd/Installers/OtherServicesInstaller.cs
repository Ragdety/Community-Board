using Microsoft.Extensions.Configuration;
using CommunityBoard.Core.Interfaces.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace CommunityBoard.BackEnd.Installers
{
    public class OtherServicesInstaller : IInstaller
    {
        public void InstallServices(
            IServiceCollection services, 
            IConfiguration configuration, 
            IWebHostEnvironment environment)
        {
            services.AddSignalR();
            services.AddAutoMapper(typeof(Startup));
        }

    }
}