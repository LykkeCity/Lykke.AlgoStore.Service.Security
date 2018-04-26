using System.Collections.Generic;
using System.Linq;
using Lykke.AlgoStore.Service.Security.AzureRepositories.Entities;
using Lykke.AlgoStore.Service.Security.Core.Domain;

namespace Lykke.AlgoStore.Service.Security.AzureRepositories.Mappers
{
    public static class RolePermissionMatchMapper
    {
        public static RolePermissionMatchData ToModel(this RolePermissionMatchEntity entity)
        {
            var result = new RolePermissionMatchData
            {
                RoleId = entity.PartitionKey,
                PermissionId = entity.RowKey
            };

            return result;
        }

        public static List<RolePermissionMatchData> ToModel(this IEnumerable<RolePermissionMatchEntity> entities)
        {
            return entities.Select(entity => new RolePermissionMatchData
            {
                RoleId = entity.PartitionKey,
                PermissionId = entity.RowKey
            }).ToList();
        }

        public static RolePermissionMatchEntity ToEntity(this RolePermissionMatchData data)
        {
            var result = new RolePermissionMatchEntity
            {
                PartitionKey = data.RoleId,
                RowKey = data.PermissionId,
                ETag = "*"
            };

            return result;
        }
    }
}
