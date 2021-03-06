using CommunityBoard.Core.Interfaces.Clients;
using CommunityBoard.Core.Interfaces.Repositories;
using CommunityBoard.FrontEnd.Services.V1;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace CommunityBoard.FrontEnd.Installers
{
    public class HttpClientInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient<IAnnouncementClient, AnnouncementClient>(client =>
            {
                client.BaseAddress = new Uri(configuration["serviceUrl"]);
            });

            services.AddHttpClient<IIdentityClient, IdentityClient>(client =>
            {
                client.BaseAddress = new Uri(configuration["serviceUrl"]);
            });
        }
    }
}
