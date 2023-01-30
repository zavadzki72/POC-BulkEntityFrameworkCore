using Bulk.App.Infra;
using Bulk.App.Interfaces;
using Bulk.App.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Z.BulkOperations;

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

        public async Task PopuleDatabaseDev()
        {
            var teams = Utils._teamsToDev;

            await _applicationContext.AddRangeAsync(teams);
            await _applicationContext.SaveChangesAsync();
        }

        public async Task UpsertWithBulkLib(List<TeamEntity> teams)
        {
            await _applicationContext.Set<TeamEntity>()
                .BulkMergeAsync(teams, x =>
                {
                    x.IncludeGraph = true;
                    x.IncludeGraphOperationBuilder = op =>
                    {
                        if(op is BulkOperation<StadiumEntity>)
                        {
                            BulkOperation<StadiumEntity> bulk = (BulkOperation<StadiumEntity>)op;
                            bulk.ColumnPrimaryKeyExpression = y => y.Name;
                        }
                        else if (op is BulkOperation<TeamEntity>)
                        {
                            BulkOperation<TeamEntity> bulk = (BulkOperation<TeamEntity>)op;
                            bulk.ColumnPrimaryKeyExpression = y => y.Name;
                        }
                    };
                });
        }

        public async Task UpsertWithBulk(List<TeamEntity> teams)
        {
            foreach(var team in teams)
            {
                var teamInBase = await _applicationContext.Set<TeamEntity>().Where(x => x.Name.Equals(team.Name)).ToListAsync();
                if (teamInBase != null && teamInBase.Any())
                {
                    await _applicationContext.Set<TeamEntity>()
                        .Where(x => x.Name.Equals(team.Name))
                        .ExecuteUpdateAsync(x =>
                            x.SetProperty(x => x.Initials, team.Initials)
                            .SetProperty(x => x.Founded_At, team.Founded_At)
                            .SetProperty(x => x.Country, team.Country)
                        );
                }
                else
                {
                    await _applicationContext.Set<TeamEntity>()
                        .AddAsync(team);
                }
            }

            await _applicationContext.SaveChangesAsync();
        }

        public async Task UpsertWithoutBulk(List<TeamEntity> teams)
        {
            foreach (var team in teams)
            {
                var teamInBase = await _applicationContext.Set<TeamEntity>().Where(x => x.Name.Equals(team.Name)).ToListAsync();
                if (teamInBase != null && teamInBase.Any())
                {
                    foreach(var teamToUpdate in teamInBase)
                    {
                        teamToUpdate.Initials = team.Initials;
                        teamToUpdate.Founded_At = team.Founded_At;
                        teamToUpdate.Country = team.Country;

                        _applicationContext.Set<TeamEntity>().Update(teamToUpdate);
                    }
                }
                else
                {
                    await _applicationContext.Set<TeamEntity>()
                        .AddAsync(team);
                }
            }

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
                x.Name = Guid.NewGuid().ToString();
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
                        x.SetProperty(x => x.Name, Guid.NewGuid().ToString())
                        .SetProperty(x => x.Initials, "SPFC")
                        .SetProperty(x => x.Country, "Brasil")
                    );
            }
            else
            {
                await _applicationContext.Set<TeamEntity>()
                    .Where(x => ids.Contains(x.Id))
                    .ExecuteUpdateAsync(x =>
                        x.SetProperty(x => x.Name, Guid.NewGuid().ToString())
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
