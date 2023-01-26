using BenchmarkDotNet.Running;
using BulkMerge.App.Infra;
using BulkMerge.App.Interfaces;
using BulkMerge.App.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BulkMerge.App
{
    public class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("INICIANDO APLICACAO");

            //var configurationBuilder = new ConfigurationBuilder()
            //    .SetBasePath(Directory.GetCurrentDirectory())
            //    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            //IConfigurationRoot configuration = configurationBuilder.Build();

            //var serviceProvider = GetServiceProvider(configuration);
            //var teamService = serviceProvider.GetRequiredService<ITeamService>();

            //teamService.UpdateWithoutBulk(new List<int>());
            //teamService.UpdateWithBulk(new List<int>());

            //Console.WriteLine("FINALIZANDO APLICACAO");

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


