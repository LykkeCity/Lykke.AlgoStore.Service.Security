using System.Collections.Generic;
using System.Threading.Tasks;
using AzureStorage;
using Lykke.AlgoStore.Service.Security.AzureRepositories.Entities;
using Lykke.AlgoStore.Service.Security.AzureRepositories.Mappers;
using Lykke.AlgoStore.Service.Security.Core.Domain;
using Lykke.AlgoStore.Service.Security.Core.Repositories;

namespace Lykke.AlgoStore.Service.Security.AzureRepositories.Repositories
{
    public class UserRolesMatchRepository : IUserRoleMatchRepository
    {
        public static readonly string TableName = "UserRolesMatch";

        private readonly INoSQLTableStorage<UserRoleMatchEntity> _table;

        public UserRolesMatchRepository(INoSQLTableStorage<UserRoleMatchEntity> table)
        {
            _table = table;
        }

        public async Task<List<UserRoleMatchData>> GetAllMatchesAsync()
        {
            var result = await _table.GetDataAsync();
            return result.ToModel();
        }

        public async Task RevokeUserRole(string clientId, string roleId)
        {
            await _table.DeleteIfExistAsync(clientId, roleId);
        }

        public async Task<UserRoleMatchData> GetUserRoleAsync(string clientId, string roleId)
        {
            var result = await _table.GetDataAsync(clientId, roleId);
            return result?.ToModel();
        }

        public async Task<List<UserRoleMatchData>> GetUserRolesAsync(string clientId)
        {
            var result = await _table.GetDataAsync(clientId);
            return result.ToModel();
        }

        public async Task<UserRoleMatchData> SaveUserRoleAsync(UserRoleMatchData data)
        {
            var entity = data.ToEntity();
            await _table.InsertOrReplaceAsync(entity);

            return data;
        }        
    }
}
