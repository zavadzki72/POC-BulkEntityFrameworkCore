using Bulk.App;
using Bulk.App.Infra;
using Bulk.App.Interfaces;
using Bulk.App.Models.Entities;
using Bulk.App.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Z.EntityFramework.Extensions;

namespace Bulk.Test
{
    public class Program
    {
        static void Main(string[] args)
        {
            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            IConfigurationRoot configuration = configurationBuilder.Build();

            var provider = GetServiceProvider(configuration);
            var service = provider.GetRequiredService<ITeamService>();

            var actualDatabaseTeam = Utils._teamsToDev;
            var stadium = new StadiumEntity
            {
                Name = "Urbano Caldeira",
                Nickname = "Vila Belmiro",
                Capacity = 20000
            };

            actualDatabaseTeam.Add(new TeamEntity
            {
                Name = "Santos FC",
                Initials = "SAN",
                Country = "Brasil",
                Founded_At = DateTime.Now,
                Stadium = stadium
            });

            foreach(var t in actualDatabaseTeam)
            {
                if(t.Name == "Sociedade esportiva Palmeiras")
                {
                    t.Initials = "PSM"; //Palmeiras Sem Mundial
                }
            }

            try
            {
                EntityFrameworkManager.ContextFactory = context => new ApplicationContext(GetDbContextOptions(configuration));

                //service.UpsertWithBulkLib(actualDatabaseTeam).Wait();
                //service.UpsertWithBulk(actualDatabaseTeam).Wait();
                service.UpsertWithoutBulk(actualDatabaseTeam).Wait();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            Console.WriteLine("Terminando execução");
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
            string? connectionString = configuration["SqlServerDev"];

            services.AddDbContext<ApplicationContext>(options => {
                options.UseSqlServer(connectionString, opt => opt.CommandTimeout(7200));
            }, ServiceLifetime.Scoped);

            services.AddScoped<ITeamService, TeamService>();
        }

        static DbContextOptions<ApplicationContext> GetDbContextOptions(IConfiguration configuration)
        {
            string? connectionString = configuration["SqlServerDev"];

            DbContextOptionsBuilder<ApplicationContext> optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>()
                .UseSqlServer(connectionString);

            return optionsBuilder.Options;
        }
    }
}