
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
        /// <returns>All permissions</returns>
        Task<IEnumerable<UserPermissionModel>> GetAllPermissionsAsync();

        /// <summary>
        /// Get permission by id
        /// </summary>
        /// <param name="permissionId">Permission Id</param>
        /// <returns>User permission or NULL otherwise</returns>
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

        /// <summary>
        /// Get all roles
        /// </summary>
        /// <returns>All roles</returns>
        Task<IEnumerable<UserRoleModel>> GetAllUserRoles();

        /// <summary>
        /// Get role by Id
        /// </summary>
        /// <param name="roleId">Role Id</param>
        /// <returns>Found role or NULL otherwise</returns>
        Task<UserRoleModel> GetRoleById(string roleId);

        /// <summary>
        /// Get client roles
        /// </summary>
        /// <param name="clientId">Client Id</param>
        /// <returns>Client roles</returns>
        Task<IEnumerable<UserRoleModel>> GetRolesByClientId(string clientId);

        /// <summary>
        /// Save user role
        /// </summary>
        /// <param name="role">user role to save</param>
        /// <returns>User role</returns>
        Task<UserRoleModel> SaveUserRole(UserRoleModel role);

        /// <summary>
        /// Update user role
        /// </summary>
        /// <param name="role">User role to update</param>
        /// <returns>User role</returns>
        Task<UserRoleModel> UpdateUserRole(UserRoleUpdateModel role);

        /// <summary>
        /// Assign specific role to user
        /// </summary>
        /// <param name="role">User role to assign</param>
        /// <returns></returns>
        Task AssignUserRole(UserRoleMatchModel role);

        /// <summary>
        /// Remove specific role from user
        /// </summary>
        /// <param name="role">User role to revoke</param>
        /// <returns></returns>
        Task RevokeRoleFromUser(UserRoleMatchModel role);

        /// <summary>
        /// Verify user role
        /// </summary>
        /// <param name="clientId">Client Id</param>
        /// <returns></returns>
        Task VerifyUserRole(string clientId);

        /// <summary>
        /// Delete user role
        /// </summary>
        /// <param name="roleId">Role Id</param>
        /// <returns></returns>
        Task DeleteUserRole(string roleId);

        /// <summary>
        /// Get all users with their roles
        /// </summary>
        /// <returns>All users with their roles</returns>
        Task<IEnumerable<AlgoStoreUserData>> GetAllUsersWithRoles();

        /// <summary>
        /// Get user with roles
        /// </summary>
        /// <param name="clientId">Client Id</param>
        /// <returns>User with roles</returns>
        Task<IEnumerable<AlgoStoreUserData>> GetUserByIdWithRoles(string clientId);
    }
}
