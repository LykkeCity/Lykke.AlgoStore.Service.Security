using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.AlgoStore.Service.Security.Core.Domain;

namespace Lykke.AlgoStore.Service.Security.Core.Repositories
{
    public interface IUserRoleMatchRepository
    {
        Task<List<UserRoleMatchData>> GetAllMatchesAsync();
        Task<UserRoleMatchData> GetUserRoleAsync(string clientId, string roleId);
        Task<List<UserRoleMatchData>> GetUserRolesAsync(string clientId);
        Task<UserRoleMatchData> SaveUserRoleAsync(UserRoleMatchData data);
        Task RevokeUserRoleAsync(string clientId, string roleId);
    }
}
