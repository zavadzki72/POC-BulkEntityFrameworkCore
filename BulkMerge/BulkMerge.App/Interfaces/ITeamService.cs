using BulkMerge.App.Models.Entities;

namespace BulkMerge.App.Interfaces
{
    public interface ITeamService
    {
        Task<List<TeamEntity>> GetAll();
        Task PopuleDatabase(int quantity);
        Task UpdateWithoutBulk(List<int> ids);
        Task UpdateWithBulk(List<int> ids);
        Task DeleteWithoutBulk(List<int> ids);
        Task DeleteWithBulk(List<int> ids);
    }
}
