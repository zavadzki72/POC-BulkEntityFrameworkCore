using BenchmarkDotNet.Attributes;
using BulkMerge.App.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BulkMerge.App
{
    [MarkdownExporter, AsciiDocExporter, HtmlExporter, CsvExporter, RPlotExporter]
    public class PerformanceTests
    {
        private ITeamService _teamService;
        private ServiceProvider _serviceProvider;

        private readonly List<int> _idList = new();

        [GlobalSetup]
        public void Setup()
        {
            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            IConfigurationRoot configuration = configurationBuilder.Build();

            _serviceProvider = Program.GetServiceProvider(configuration);
            _teamService = _serviceProvider.GetRequiredService<ITeamService>();

            for(int i=1; i<=25000; i++)
            {
                _idList.Add(i);
            }
        }

        [Benchmark]
        public async Task UpdateWithoutBulkWithListOfIds()
        {
            await _teamService.UpdateWithoutBulk(_idList);
        }

        [Benchmark]
        public async Task UpdateWithBulkWithListOfIds()
        {
            await _teamService.UpdateWithBulk(_idList);
        }

        [Benchmark]
        public async Task UpdateWithoutBulk()
        {
            await _teamService.UpdateWithoutBulk(_idList);
        }

        [Benchmark]
        public async Task UpdateWithBulk()
        {
            await _teamService.UpdateWithBulk(_idList);
        }

        [GlobalCleanup]
        public void Cleanup()
        {
            _serviceProvider.Dispose();
        }
    }
}
