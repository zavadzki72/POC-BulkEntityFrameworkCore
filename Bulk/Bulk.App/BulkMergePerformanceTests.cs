using BenchmarkDotNet.Attributes;
using Bulk.App.Infra;
using Bulk.App.Interfaces;
using Bulk.App.Models.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Z.EntityFramework.Extensions;

namespace Bulk.App
{
    [MarkdownExporter, AsciiDocExporter, HtmlExporter, CsvExporter, RPlotExporter]
    public class BulkMergePerformanceTests
    {
        private ITeamService _teamService;
        private ServiceProvider _serviceProvider;

        private readonly List<TeamEntity> _teamList = new();

        [GlobalSetup]
        public void Setup()
        {
            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            IConfigurationRoot configuration = configurationBuilder.Build();
            EntityFrameworkManager.ContextFactory = context => new ApplicationContext(Program.GetDbContextOptions(configuration));

            _serviceProvider = Program.GetServiceProvider(configuration);
            _teamService = _serviceProvider.GetRequiredService<ITeamService>();
            var context = _serviceProvider.GetRequiredService<ApplicationContext>();

            int quantityItemsFilter = Convert.ToInt32(configuration["QuantityFilterTeams"] ?? "500");
            int quantityFilterTeamsInsert = Convert.ToInt32(configuration["QuantityFilterTeamsInsert"] ?? "10");

            var teams = context.Set<TeamEntity>()
                .Take(quantityItemsFilter-quantityFilterTeamsInsert)
                .ToList();

            for (int i = 1; i <= quantityFilterTeamsInsert; i++)
            {
                var team = Utils.GetTeamListRandom(1, false).First();
                _teamList.Add(team);
            }

            int count = 0;
            foreach(var team in teams)
            {
                if(count < 100)
                {
                    team.Initials = "AAA";
                }
                else if (count < 200)
                {
                    team.Initials = "BBB";
                }
                else if (count < 300)
                {
                    team.Initials = "CCC";
                }
                else if (count < 400)
                {
                    team.Initials = "DDD";
                }
                else if (count < 500)
                {
                    team.Initials = "EEE";
                }
                else
                {
                    team.Initials = "ZZZ";
                }

                _teamList.Add(team);
            }
        }

        [Benchmark]
        public async Task UpsertWithBulkLib()
        {
            await _teamService.UpsertWithBulkLib(_teamList);
        }

        [Benchmark]
        public async Task UpsertWithBulk()
        {
            await _teamService.UpsertWithBulk(_teamList);
        }

        [Benchmark]
        public async Task UpsertWithoutBulk()
        {
            await _teamService.UpsertWithoutBulk(_teamList);
        }

        [GlobalCleanup]
        public void Cleanup()
        {
            _serviceProvider.Dispose();
        }
    }
}
