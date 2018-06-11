using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AzureStorage;
using Lykke.AlgoStore.Service.Security.AzureRepositories.Entities;
using Lykke.AlgoStore.Service.Security.Core.Domain;
using Lykke.AlgoStore.Service.Security.Core.Repositories;

namespace Lykke.AlgoStore.Service.Security.AzureRepositories.Repositories
{
    public class UserPermissionsRepository: IUserPermissionsRepository
    {
        public static readonly string TableName = "UserPermissions";

        private readonly INoSQLTableStorage<UserPermissionEntity> _table;

        public UserPermissionsRepository(INoSQLTableStorage<UserPermissionEntity> table)
        {
            _table = table;
        }

        public async Task<List<UserPermissionData>> GetAllPermissionsAsync()
        {
            var result = await _table.GetDataAsync();

            return Mapper.Map<List<UserPermissionData>>(result);
        }

        public async Task<UserPermissionData> GetPermissionByIdAsync(string permissionId)
        {
            var result = await _table.GetDataAsync(permissionId);

            return Mapper.Map<UserPermissionData>(result.FirstOrDefault());
        }

        public async Task<UserPermissionData> SavePermissionAsync(UserPermissionData permission)
        {
            var entity = Mapper.Map<UserPermissionEntity>(permission);

            await _table.InsertOrReplaceAsync(entity);

            return permission;
        }

        public async Task DeletePermissionAsync(UserPermissionData permission)
        {
            var entity = Mapper.Map<UserPermissionEntity>(permission);

            await _table.DeleteIfExistAsync(entity.PartitionKey, entity.RowKey);
        }
    }
}
