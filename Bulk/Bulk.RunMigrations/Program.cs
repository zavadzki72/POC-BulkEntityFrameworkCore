using Bulk.App.Infra;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Bulk.RunMigrations
{
    public class Program : IDesignTimeDbContextFactory<ApplicationContext>
    {
        static void Main(string[] args)
        {
            Console.WriteLine("--INICIANDO MIGRATIONS--");

            Program p = new();

            using ApplicationContext context = p.CreateDbContext(Array.Empty<string>());
            context.Database.Migrate();
            context.SaveChanges();

            Console.WriteLine("--FINALIZANDO MIGRATIONS--");
        }

        public ApplicationContext CreateDbContext(string[] args)
        {
            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            IConfigurationRoot configuration = configurationBuilder.Build();
            string? connectionString = configuration["SqlServer"];

            DbContextOptionsBuilder<ApplicationContext> optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>()
                .UseSqlServer(connectionString);

            return new ApplicationContext(optionsBuilder.Options);
        }
    }
}


