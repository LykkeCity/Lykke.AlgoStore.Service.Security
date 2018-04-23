using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.AlgoStore.Service.Security.Core.Domain;

namespace Lykke.AlgoStore.Service.Security.Core.Repositories
{
    public interface IUserPermissionsRepository
    {
        Task<List<UserPermissionData>> GetAllPermissionsAsync();
        Task<UserPermissionData> GetPermissionByIdAsync(string permissionId);
        Task<UserPermissionData> SavePermissionAsync(UserPermissionData permission);
        Task DeletePermissionAsync(UserPermissionData permission);
    }
}
