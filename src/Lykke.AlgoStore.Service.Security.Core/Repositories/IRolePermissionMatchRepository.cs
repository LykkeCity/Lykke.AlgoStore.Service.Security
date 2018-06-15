using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.AlgoStore.Service.Security.Core.Domain;

namespace Lykke.AlgoStore.Service.Security.Core.Repositories
{
    public interface IRolePermissionMatchRepository
    {
        Task<List<RolePermissionMatchData>> GetPermissionIdsByRoleIdAsync(string roleId);
        Task<RolePermissionMatchData> AssignPermissionToRoleAsync(RolePermissionMatchData data);
        Task RevokePermissionAsync(RolePermissionMatchData data);
    }
}
