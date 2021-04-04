using CommunityBoard.Core.Interfaces.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
        }
    }
}