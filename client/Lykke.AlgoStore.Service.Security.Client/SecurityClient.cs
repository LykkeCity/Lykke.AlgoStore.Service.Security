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

        public async Task AssignMultiplePermissionToRole(List<RolePermissionMatchModel> permissions)
        {
            await _service.AssignMultiplePermissionToRoleAsync(permissions);
        }

        public async Task RevokeMultiplePermissions(List<RolePermissionMatchModel> role)
        {
            await _service.RevokeMultiplePermissionsAsync(role);
        }

        public async Task<bool> HasPermission(string clientId, string permissionId)
        {
            var result = await _service.HasPermissionAsync(clientId, permissionId);

            return result.HasValue && result.Value;
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
