
using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.Security.Client.AutorestClient.Models;

namespace Lykke.AlgoStore.Service.Security.Client
{
    public interface ISecurityClient
    {
        /// <summary>
        /// Get all permissions
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<UserPermissionModel>> GetAllPermissionsAsync();

        /// <summary>
        /// Get permission by id
        /// </summary>
        /// <param name="permissionId">Permission Id</param>
        /// <returns>User permission or NULL if no such permission exists</returns>
        Task<UserPermissionModel> GetPermissionByIdAsync(string permissionId);

        /// <summary>
        /// Get permissions based on role id
        /// </summary>
        /// <param name="roleId">Role Id</param>
        /// <returns>Permissions found for provided role id</returns>
        Task<IEnumerable<UserPermissionModel>> GetPermissionsByRoleIdAsync(string roleId);

        /// <summary>
        /// Assign multiple permission for provided role
        /// </summary>
        /// <param name="permissions">Permissions to assign</param>
        /// <returns></returns>
        Task AssignMultiplePermissionToRole(List<RolePermissionMatchModel> permissions);

        /// <summary>
        /// Revoke multiple permissions from role
        /// </summary>
        /// <param name="role">Role to remove permissions from</param>
        /// <returns></returns>
        Task RevokeMultiplePermissions(List<RolePermissionMatchModel> role);

        /// <summary>
        /// Check if client has specific permission
        /// </summary>
        /// <param name="clientId">Client Id</param>
        /// <param name="permissionId">Permission Id</param>
        /// <returns>TRUE when client has specific permission, otherwise FALSE</returns>
        Task<bool> HasPermission(string clientId, string permissionId);
    }
}
