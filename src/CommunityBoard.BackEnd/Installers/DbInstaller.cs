﻿using CommunityBoard.BackEnd.Data;
using CommunityBoard.Core.Interfaces.Repositories;
using CommunityBoard.Core.Models.CoreModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CommunityBoard.BackEnd.Installers
{
	public class DbInstaller : IInstaller
    {
        public void InstallServices(
            IServiceCollection services, 
            IConfiguration configuration,
            IWebHostEnvironment environment)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("CommunityBoard")));
            services.AddIdentity<User, IdentityRole<int>>(options => 
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = false;
            })
            .AddRoles<IdentityRole<int>>()
            .AddEntityFrameworkStores<ApplicationDbContext>();
        }
    }
}