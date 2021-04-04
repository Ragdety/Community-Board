using CommunityBoard.Core.Interfaces.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace CommunityBoard.BackEnd.Installers
{
    public static class InstallerExtensions
    {
        public static void InstallServicesInAssembly(
            this IServiceCollection services, 
            IConfiguration configuration,
            IWebHostEnvironment environment)
        {
            var installers = typeof(Startup).Assembly.ExportedTypes.Where(x =>
               typeof(IInstaller).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                .Select(Activator.CreateInstance)
                .Cast<IInstaller>().ToList();

            installers.ForEach(installer => installer.InstallServices(services, configuration, environment));
        }
    }
}
