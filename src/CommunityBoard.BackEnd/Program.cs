using System.Threading.Tasks;
using CommunityBoard.BackEnd.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CommunityBoard.BackEnd
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using(var serviceScope = host.Services.CreateScope())
			{
                var dbContext = 
                    serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                //Doing this manually instead to prevent data loss or errors
                //await dbContext.Database.MigrateAsync();

                var roleManager = 
                    serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();

                if(!await roleManager.RoleExistsAsync("Admin"))
				{
                    var adminRole = new IdentityRole<int>("Admin");
                    await roleManager.CreateAsync(adminRole);
				}

                if (!await roleManager.RoleExistsAsync("User"))
                {
                    var userRole = new IdentityRole<int>("User");
                    await roleManager.CreateAsync(userRole);
                }
            }

            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}