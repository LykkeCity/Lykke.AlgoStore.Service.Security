using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AzureStorage;
using Lykke.AlgoStore.Service.Security.AzureRepositories.Entities;
using Lykke.AlgoStore.Service.Security.AzureRepositories.Mappers;
using Lykke.AlgoStore.Service.Security.Core.Domain;
using Lykke.AlgoStore.Service.Security.Core.Repositories;

namespace Lykke.AlgoStore.Service.Security.AzureRepositories.Repositories
{
    public class UserRolesRepository : IUserRolesRepository
    {
        public static readonly string TableName = "UserRoles";

        private readonly INoSQLTableStorage<UserRoleEntity> _table;

        public UserRolesRepository(INoSQLTableStorage<UserRoleEntity> table)
        {
            _table = table;
        }

        public async Task<List<UserRoleData>> GetAllRolesAsync()
        {
            var result = await _table.GetDataAsync();

            return result.ToModel();
        }

        public async Task<UserRoleData> GetRoleByIdAsync(string roleId)
        {
            var result = await _table.GetDataAsync(roleId);
            return result.FirstOrDefault()?.ToModel();
        }       

        public async Task<UserRoleData> SaveRoleAsync(UserRoleData role)
        {
            var entity = role.ToEntity();

            await _table.InsertOrReplaceAsync(entity);
            return role;
        }

        public async Task DeleteRoleAsync(UserRoleData role)
        {
            await _table.DeleteIfExistAsync(role.Id, role.Name);
        }
    }
}
