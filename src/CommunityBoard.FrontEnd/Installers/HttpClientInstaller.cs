using CommunityBoard.Core.Interfaces.Clients;
using CommunityBoard.Core.Interfaces.Repositories;
using CommunityBoard.FrontEnd.Services.V1;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace CommunityBoard.FrontEnd.Installers
{
    public class HttpClientInstaller : IInstaller
    {
        public void InstallServices(
            IServiceCollection services, 
            IConfiguration configuration,
            IWebHostEnvironment environment)
        {
            //Needs refactoring...
            var serviceUrl = configuration["serviceUrl"];

            services.AddHttpClient<BaseClient>(client =>
            {
                client.BaseAddress = new Uri(serviceUrl);
            });
            services.AddHttpClient<IReportClient, ReportClient>(client =>
            {
                client.BaseAddress = new Uri(serviceUrl);
            });
            services.AddHttpClient<IAnnouncementClient, AnnouncementClient>(client =>
            {
                client.BaseAddress = new Uri(serviceUrl);
            });
            services.AddHttpClient<IIdentityClient, IdentityClient>(client =>
            {
                client.BaseAddress = new Uri(serviceUrl);
            });
            services.AddHttpClient<IChatClient, ChatClient>(client =>
            {
                client.BaseAddress = new Uri(serviceUrl);
            });
            services.AddHttpClient<IUserClient, UserClient>(client =>
            {
                client.BaseAddress = new Uri(serviceUrl);
            });
        }
    }
}