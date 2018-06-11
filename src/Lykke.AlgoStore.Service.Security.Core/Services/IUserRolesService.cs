using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.AlgoStore.Service.Security.Core.Domain;

namespace Lykke.AlgoStore.Service.Security.Core.Services
{
    public interface IUserRolesService
    {
        Task<List<UserRoleData>> GetAllRolesAsync();
        Task<UserRoleData> GetRoleByIdAsync(string roleId);
        Task<List<UserRoleData>> GetRolesByClientIdAsync(string clientId);
        Task<List<AlgoStoreUserData>> GetAllUsersWithRolesAsync();
        Task<AlgoStoreUserData> GetUserByIdWithRolesAsync(string clientId);
        Task AssignRoleToUserAsync(UserRoleMatchData data);
        Task<UserRoleData> SaveRoleAsync(UserRoleData role);
        Task RevokeRoleFromUserAsync(UserRoleMatchData data);
        Task VerifyUserRoleAsync(string clientId);
        Task DeleteRoleAsync(string roleId);
        Task SeedRolesAsync(List<UserPermissionData> permissions);
    }
}
