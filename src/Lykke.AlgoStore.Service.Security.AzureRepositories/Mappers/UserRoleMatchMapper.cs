using System.Collections.Generic;
using System.Linq;
using Lykke.AlgoStore.Service.Security.AzureRepositories.Entities;
using Lykke.AlgoStore.Service.Security.Core.Domain;

namespace Lykke.AlgoStore.Service.Security.AzureRepositories.Mappers
{
    public static class UserRoleMatchMapper
    {
        public static UserRoleMatchData ToModel(this UserRoleMatchEntity entity)
        {
            var result = new UserRoleMatchData
            {
                ClientId = entity.PartitionKey,
                RoleId = entity.RowKey
            };

            return result;
        }

        public static List<UserRoleMatchData> ToModel(this IEnumerable<UserRoleMatchEntity> entities)
        {
            return entities.Select(entity => new UserRoleMatchData
            {
                ClientId = entity.PartitionKey,
                RoleId = entity.RowKey
            }).ToList();
        }

        public static UserRoleMatchEntity ToEntity(this UserRoleMatchData data)
        {
            var result = new UserRoleMatchEntity
            {
                PartitionKey = data.ClientId,
                RowKey = data.RoleId,
                ETag = "*"
            };

            return result;
        }
    }
}
