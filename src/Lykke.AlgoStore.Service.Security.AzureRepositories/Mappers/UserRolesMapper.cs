using System.Collections.Generic;
using System.Linq;
using Lykke.AlgoStore.Service.Security.AzureRepositories.Entities;
using Lykke.AlgoStore.Service.Security.Core.Domain;

namespace Lykke.AlgoStore.Service.Security.AzureRepositories.Mappers
{
    public static class UserRolesMapper
    {
        public static UserRoleData ToModel(this UserRoleEntity entity)
        {
            var result = new UserRoleData
            {
                Id = entity.PartitionKey,
                Name = entity.RowKey,
                CanBeDeleted = entity.CanBeDeleted,
                CanBeModified = entity.CanBeModified
            };

            return result;
        }

        public static List<UserRoleData> ToModel(this IEnumerable<UserRoleEntity> entities)
        {
            return entities.Select(entity => new UserRoleData
            {
                Id = entity.PartitionKey,
                Name = entity.RowKey,
                CanBeDeleted = entity.CanBeDeleted,
                CanBeModified = entity.CanBeModified

            }).ToList();
        }

        public static UserRoleEntity ToEntity(this UserRoleData data)
        {
            var result = new UserRoleEntity
            {
                PartitionKey = data.Id,
                RowKey = data.Name,
                ETag = "*",
                CanBeDeleted = data.CanBeDeleted,
                CanBeModified = data.CanBeModified
            };

            return result;
        }
    }
}
