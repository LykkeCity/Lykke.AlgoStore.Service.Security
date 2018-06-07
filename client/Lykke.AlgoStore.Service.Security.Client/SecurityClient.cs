using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Common.Log;
using Lykke.Service.Security.Client.AutorestClient;
using Lykke.Service.Security.Client.AutorestClient.Models;

namespace Lykke.AlgoStore.Service.Security.Client
{
    public class SecurityClient : ISecurityClient, IDisposable
    {
        private readonly ILog _log;
        private ISecurityAPI _service;

        public SecurityClient(string serviceUrl, ILog log)
        {
            _log = log;
            _service = new SecurityAPI(new Uri(serviceUrl), new HttpClient());
        }

        public async Task<IEnumerable<UserPermissionModel>> GetAllPermissionsAsync()
        {
            return await _service.GetAllPermissionsAsync();
        }

        public async Task<UserPermissionModel> GetPermissionByIdAsync(string permissionId)
        {
            return await _service.GetPermissionByIdAsync(permissionId);
        }

        public async Task<IEnumerable<UserPermissionModel>> GetPermissionsByRoleIdAsync(string roleId)
        {
            return await _service.GetPermissionsByRoleIdAsync(roleId);
        }

        public async Task AssignMultiplePermissionToRoleAsync(List<RolePermissionMatchModel> permissions)
        {
            await _service.AssignMultiplePermissionToRoleAsync(permissions);
        }

        public async Task RevokeMultiplePermissionsAsync(List<RolePermissionMatchModel> role)
        {
            await _service.RevokeMultiplePermissionsAsync(role);
        }

        public async Task<bool> HasPermissionAsync(string clientId, string permissionId)
        {
            var result = await _service.HasPermissionAsync(clientId, permissionId);

            return result.HasValue && result.Value;
        }

        public async Task<IEnumerable<UserRoleModel>> GetAllUserRolesAsync()
        {
            return await _service.GetAllUserRolesAsync();
        }

        public async Task<UserRoleModel> GetRoleByIdAsync(string roleId)
        {
            return await _service.GetRoleByIdAsync(roleId);
        }

        public async Task<IEnumerable<UserRoleModel>> GetRolesByClientIdAsync(string clientId)
        {
            return await _service.GetRolesByClientIdAsync(clientId);
        }

        public async Task<UserRoleModel> SaveUserRoleAsync(UserRoleModel role)
        {
            return await _service.SaveUserRoleAsync(role);
        }

        public async Task<UserRoleModel> UpdateUserRoleAsync(UserRoleUpdateModel role)
        {
            return await _service.UpdateUserRoleAsync(role);
        }

        public async Task AssignUserRoleAsync(UserRoleMatchModel role)
        {
            await _service.AssignUserRoleAsync(role);
        }

        public async Task RevokeRoleFromUserAsync(UserRoleMatchModel role)
        {
            await _service.RevokeRoleFromUserAsync(role);
        }

        public async Task VerifyUserRoleAsync(string clientId)
        {
            await _service.VerifyUserRoleAsync(clientId);
        }

        public async Task DeleteUserRoleAsync(string roleId)
        {
            await _service.DeleteUserRoleAsync(roleId);
        }

        public async Task<IEnumerable<AlgoStoreUserData>> GetAllUsersWithRolesAsync()
        {
            return await _service.GetAllUsersWithRolesAsync();
        }

        public async Task<AlgoStoreUserData> GetUserByIdWithRolesAsync(string clientId)
        {
            return await _service.GetUserByIdWithRolesAsync(clientId);
        }

        public void Dispose()
        {
            if (_service == null)
                return;
            _service.Dispose();
            _service = null;
        }
    }
}
