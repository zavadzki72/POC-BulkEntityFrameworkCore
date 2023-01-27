using Bulk.App.Infra;
using Bulk.App.Interfaces;
using Bulk.App.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bulk.App.Services
{
    public class TeamService : ITeamService
    {
        private readonly ApplicationContext _applicationContext;

        public TeamService(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }

        public async Task<List<TeamEntity>> GetAll()
        {
            return await _applicationContext.Set<TeamEntity>().ToListAsync();
        }

        public async Task PopuleDatabase(int quantity)
        {
            var teams = Utils.GetTeamListRandom(quantity, false);

            await _applicationContext.AddRangeAsync(teams);
            await _applicationContext.SaveChangesAsync();
        }

        public async Task UpdateWithoutBulk(List<int> ids)
        {
            List<TeamEntity> teams = new();

            if (!ids.Any())
            {
                teams = await GetAll();
            }
            else
            {
                teams = await _applicationContext.Set<TeamEntity>().Where(x => ids.Contains(x.Id)).ToListAsync();
            }

            teams.ForEach(x =>
            {
                x.Name = "São Paulo FC";
                x.Initials = "SPFC";
                x.Country = "Brasil";
            });

            _applicationContext.UpdateRange(teams);
            await _applicationContext.SaveChangesAsync();
        }

        public async Task UpdateWithBulk(List<int> ids)
        {
            if (!ids.Any())
            {
                await _applicationContext.Set<TeamEntity>()
                    .ExecuteUpdateAsync(x =>
                        x.SetProperty(x => x.Name, "São Paulo FC")
                        .SetProperty(x => x.Initials, "SPFC")
                        .SetProperty(x => x.Country, "Brasil")
                    );
            }
            else
            {
                await _applicationContext.Set<TeamEntity>()
                    .Where(x => ids.Contains(x.Id))
                    .ExecuteUpdateAsync(x =>
                        x.SetProperty(x => x.Name, "São Paulo FC")
                        .SetProperty(x => x.Initials, "SPFC")
                        .SetProperty(x => x.Country, "Brasil")
                    );
            }
        }

        public async Task DeleteWithoutBulk(List<int> ids)
        {
            List<TeamEntity> teams = new();
            
            if (!ids.Any())
            {
                teams = await GetAll();
            }
            else
            {
                teams = await _applicationContext.Set<TeamEntity>().Where(x => ids.Contains(x.Id)).ToListAsync();
            }

            _applicationContext.RemoveRange(teams);
            await _applicationContext.SaveChangesAsync();
        }

        public async Task DeleteWithBulk(List<int> ids)
        {
            if (!ids.Any())
            {
                await _applicationContext.Set<TeamEntity>()
                    .ExecuteDeleteAsync();
            }
            else
            {
                await _applicationContext.Set<TeamEntity>()
                    .Where(x => ids.Contains(x.Id))
                    .ExecuteDeleteAsync();
            }
        }
    }
}
