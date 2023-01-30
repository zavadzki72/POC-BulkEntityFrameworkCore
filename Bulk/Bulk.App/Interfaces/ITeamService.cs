using Bulk.App.Models.Entities;

namespace Bulk.App.Interfaces
{
    public interface ITeamService
    {
        Task<List<TeamEntity>> GetAll();
        Task UpsertWithBulkLib(List<TeamEntity> teams);
        Task UpsertWithBulk(List<TeamEntity> teams);
        Task UpsertWithoutBulk(List<TeamEntity> teams);
        Task PopuleDatabaseDev();
        Task PopuleDatabase(int quantity);
        Task UpdateWithoutBulk(List<int> ids);
        Task UpdateWithBulk(List<int> ids);
        Task DeleteWithoutBulk(List<int> ids);
        Task DeleteWithBulk(List<int> ids);
    }
}
