using System.Collections.Generic;
using System.Linq;
using Lykke.AlgoStore.Service.Security.AzureRepositories.Entities;
using Lykke.AlgoStore.Service.Security.Core.Domain;

namespace Lykke.AlgoStore.Service.Security.AzureRepositories.Mappers
{
    public static class UserPermissionsMapper
    {
        public static UserPermissionData ToModel(this UserPermissionEntity entity)
        {
            var result = new UserPermissionData
            {
                Id = entity.PartitionKey,
                Name = entity.RowKey,
                DisplayName = entity.DisplayName
            };

            return result;
        }

        public static List<UserPermissionData> ToModel(this IEnumerable<UserPermissionEntity> entities)
        {
            return entities.Select(entity => new UserPermissionData
            {
                Id = entity.PartitionKey,
                Name = entity.RowKey,
                DisplayName = entity.DisplayName
            }).ToList();
        }

        public static UserPermissionEntity ToEntity(this UserPermissionData data)
        {
            var result = new UserPermissionEntity
            {
                PartitionKey = data.Id,
                RowKey = data.Name,
                DisplayName = data.DisplayName,
                ETag = "*"
            };

            return result;
        }
    }
}
