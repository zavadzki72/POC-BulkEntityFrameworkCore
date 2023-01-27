using BenchmarkDotNet.Running;
using Bulk.App.Infra;
using Bulk.App.Interfaces;
using Bulk.App.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Bulk.App
{
    public class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<PerformanceTests>();
            Console.Read();
        }

        public static ServiceProvider GetServiceProvider(IConfiguration configuration)
        {
            var services = new ServiceCollection();
            ConfigureServices(services, configuration);
            var serviceProvider = services.BuildServiceProvider();

            return serviceProvider;
        }

        static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            string? connectionString = configuration["SqlServer"];

            services.AddDbContext<ApplicationContext>(options => {
                options.UseSqlServer(connectionString, opt => opt.CommandTimeout(7200));
            }, ServiceLifetime.Scoped);

            services.AddScoped<ITeamService, TeamService>();
        }
    }
}


