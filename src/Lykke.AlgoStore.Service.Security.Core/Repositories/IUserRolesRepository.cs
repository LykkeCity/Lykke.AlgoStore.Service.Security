using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.AlgoStore.Service.Security.Core.Domain;

namespace Lykke.AlgoStore.Service.Security.Core.Repositories
{
    public interface IUserRolesRepository
    {
        Task<List<UserRoleData>> GetAllRolesAsync();
        Task<UserRoleData> GetRoleByIdAsync(string roleId);
        Task<UserRoleData> SaveRoleAsync(UserRoleData role);
        Task DeleteRoleAsync(UserRoleData data);
    }
}
