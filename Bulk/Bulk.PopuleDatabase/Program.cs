using Bulk.App.Infra;
using Bulk.App.Interfaces;
using Bulk.App.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Bulk.PopuleDatabase
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("INICIANDO POPULAR BASE");

            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            IConfigurationRoot configuration = configurationBuilder.Build();

            var services = new ServiceCollection();
            ConfigureServices(services, configuration);
            var serviceProvider = services.BuildServiceProvider();

            var teamService = serviceProvider.GetRequiredService<ITeamService>();

            var quantityItemsToInsertStr = configuration["QuantityItemsToInsert"];

            var quantityItemsToInsert = string.IsNullOrWhiteSpace(quantityItemsToInsertStr) ? 500000 : long.Parse(quantityItemsToInsertStr);
            int cont = 0;

            for(int i=0; i<quantityItemsToInsert; i++)
            {
                var startTime = DateTime.Now;
                int insertQuantity = (quantityItemsToInsert < 10000) ? Convert.ToInt32(quantityItemsToInsert) : 10000;
                Console.Write($"\nRealizando insercao de {insertQuantity} items, step: {++cont}");

                try
                {
                    teamService.PopuleDatabase(insertQuantity).Wait();
                    quantityItemsToInsert -= insertQuantity;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"ERRO AO POPULAR BASE -> {ex.Message}");
                }
                finally
                {
                    var endTime = DateTime.Now;
                    var timeToExecute = endTime - startTime;

                    Console.Write($" -> Finalizado em {timeToExecute}");
                }
            }

            Console.WriteLine("\nFINALIZANDO POPULAR BASE");
        }

        static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            string? connectionString = configuration["SqlServer"];

            services.AddDbContext<ApplicationContext>(options => {
                options.UseSqlServer(connectionString);
            }, ServiceLifetime.Scoped);

            services.AddScoped<ITeamService, TeamService>();
        }
    }
}


