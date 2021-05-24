using CommunityBoard.Core.Interfaces.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CommunityBoard.BackEnd.Installers
{
    public class ControllerInstaller : IInstaller
    {
        public void InstallServices(
            IServiceCollection services, 
            IConfiguration configuration,
            IWebHostEnvironment environment)
        {
            services.AddControllers()
                .AddJsonOptions(o =>
                {
                    //To display the enum name and not number
                    o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });;
        }
    }
}
