using System.Collections.Generic;
using System.Threading.Tasks;
using AzureStorage;
using Lykke.AlgoStore.Service.Security.AzureRepositories.Entities;
using Lykke.AlgoStore.Service.Security.AzureRepositories.Mappers;
using Lykke.AlgoStore.Service.Security.Core.Domain;
using Lykke.AlgoStore.Service.Security.Core.Repositories;

namespace Lykke.AlgoStore.Service.Security.AzureRepositories.Repositories
{
    public class RolePermissionMatchRepository : IRolePermissionMatchRepository
    {

        public static readonly string TableName = "RolePermissionMatch";

        private readonly INoSQLTableStorage<RolePermissionMatchEntity> _table;

        public RolePermissionMatchRepository(INoSQLTableStorage<RolePermissionMatchEntity> table)
        {
            _table = table;
        }

        public async Task<RolePermissionMatchData> AssignPermissionToRoleAsync(RolePermissionMatchData data)
        {
            var entity = data.ToEntity();
            await _table.InsertOrReplaceAsync(entity);

            return data;
        }

        public async Task<List<RolePermissionMatchData>> GetPermissionIdsByRoleIdAsync(string roleId)
        {
            var result = await _table.GetDataAsync(roleId);

            return result.ToModel();
        }

        public async Task RevokePermission(RolePermissionMatchData data)
        {
            await _table.DeleteAsync(data.RoleId, data.PermissionId);
        }
    }
}
