using CommunityBoard.BackEnd.Repositories;
using CommunityBoard.BackEnd.Repositories.CommunicationRepos;
using CommunityBoard.Core.Interfaces.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CommunityBoard.BackEnd.Installers
{
    public class RepositoryInstaller : IInstaller
    {
        public void InstallServices(
            IServiceCollection services, 
            IConfiguration configuration, 
            IWebHostEnvironment environment)
        {
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IIdentityRepository, IdentityRepository>();
            services.AddScoped<IAnnouncementsRepository, AnnouncementsRepository>();
            services.AddScoped<IReportsRepository, ReportsRepository>();
            services.AddScoped<IChatsRepository, ChatsRepository>();
            services.AddScoped<IMessagesRepository, MessagesRepository>();
        }
    }
}