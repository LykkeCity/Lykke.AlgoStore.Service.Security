// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Lykke.Service.Security.Client.AutorestClient
{
    using Models;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Extension methods for SecurityAPI.
    /// </summary>
    public static partial class SecurityAPIExtensions
    {
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            public static IList<UserPermissionData> GetAllPermissions(this ISecurityAPI operations)
            {
                return operations.GetAllPermissionsAsync().GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<IList<UserPermissionData>> GetAllPermissionsAsync(this ISecurityAPI operations, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.GetAllPermissionsWithHttpMessagesAsync(null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='permissionId'>
            /// </param>
            public static UserPermissionData GetPermissionById(this ISecurityAPI operations, string permissionId = default(string))
            {
                return operations.GetPermissionByIdAsync(permissionId).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='permissionId'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<UserPermissionData> GetPermissionByIdAsync(this ISecurityAPI operations, string permissionId = default(string), CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.GetPermissionByIdWithHttpMessagesAsync(permissionId, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='roleId'>
            /// </param>
            public static IList<UserPermissionData> GetPermissionsByRoleId(this ISecurityAPI operations, string roleId = default(string))
            {
                return operations.GetPermissionsByRoleIdAsync(roleId).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='roleId'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<IList<UserPermissionData>> GetPermissionsByRoleIdAsync(this ISecurityAPI operations, string roleId = default(string), CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.GetPermissionsByRoleIdWithHttpMessagesAsync(roleId, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='permissions'>
            /// </param>
            public static void AssignMultiplePermissionToRole(this ISecurityAPI operations, IList<RolePermissionMatchModel> permissions = default(IList<RolePermissionMatchModel>))
            {
                operations.AssignMultiplePermissionToRoleAsync(permissions).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='permissions'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task AssignMultiplePermissionToRoleAsync(this ISecurityAPI operations, IList<RolePermissionMatchModel> permissions = default(IList<RolePermissionMatchModel>), CancellationToken cancellationToken = default(CancellationToken))
            {
                (await operations.AssignMultiplePermissionToRoleWithHttpMessagesAsync(permissions, null, cancellationToken).ConfigureAwait(false)).Dispose();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='role'>
            /// </param>
            public static void RevokeMultiplePermissions(this ISecurityAPI operations, IList<RolePermissionMatchModel> role = default(IList<RolePermissionMatchModel>))
            {
                operations.RevokeMultiplePermissionsAsync(role).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='role'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task RevokeMultiplePermissionsAsync(this ISecurityAPI operations, IList<RolePermissionMatchModel> role = default(IList<RolePermissionMatchModel>), CancellationToken cancellationToken = default(CancellationToken))
            {
                (await operations.RevokeMultiplePermissionsWithHttpMessagesAsync(role, null, cancellationToken).ConfigureAwait(false)).Dispose();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='clientId'>
            /// </param>
            /// <param name='permissionId'>
            /// </param>
            public static bool? HasPermission(this ISecurityAPI operations, string clientId = default(string), string permissionId = default(string))
            {
                return operations.HasPermissionAsync(clientId, permissionId).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='clientId'>
            /// </param>
            /// <param name='permissionId'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<bool?> HasPermissionAsync(this ISecurityAPI operations, string clientId = default(string), string permissionId = default(string), CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.HasPermissionWithHttpMessagesAsync(clientId, permissionId, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='permissions'>
            /// </param>
            public static bool? SeedPermissions(this ISecurityAPI operations, IList<UserPermissionData> permissions = default(IList<UserPermissionData>))
            {
                return operations.SeedPermissionsAsync(permissions).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='permissions'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<bool?> SeedPermissionsAsync(this ISecurityAPI operations, IList<UserPermissionData> permissions = default(IList<UserPermissionData>), CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.SeedPermissionsWithHttpMessagesAsync(permissions, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            public static IList<UserRoleData> GetAllUserRoles(this ISecurityAPI operations)
            {
                return operations.GetAllUserRolesAsync().GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<IList<UserRoleData>> GetAllUserRolesAsync(this ISecurityAPI operations, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.GetAllUserRolesWithHttpMessagesAsync(null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='roleId'>
            /// </param>
            public static UserRoleData GetRoleById(this ISecurityAPI operations, string roleId = default(string))
            {
                return operations.GetRoleByIdAsync(roleId).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='roleId'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<UserRoleData> GetRoleByIdAsync(this ISecurityAPI operations, string roleId = default(string), CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.GetRoleByIdWithHttpMessagesAsync(roleId, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='clientId'>
            /// </param>
            public static IList<UserRoleData> GetRolesByClientId(this ISecurityAPI operations, string clientId = default(string))
            {
                return operations.GetRolesByClientIdAsync(clientId).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='clientId'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<IList<UserRoleData>> GetRolesByClientIdAsync(this ISecurityAPI operations, string clientId = default(string), CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.GetRolesByClientIdWithHttpMessagesAsync(clientId, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='role'>
            /// </param>
            public static UserRoleData SaveUserRole(this ISecurityAPI operations, UserRoleModel role = default(UserRoleModel))
            {
                return operations.SaveUserRoleAsync(role).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='role'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<UserRoleData> SaveUserRoleAsync(this ISecurityAPI operations, UserRoleModel role = default(UserRoleModel), CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.SaveUserRoleWithHttpMessagesAsync(role, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='role'>
            /// </param>
            public static UserRoleData UpdateUserRole(this ISecurityAPI operations, UserRoleUpdateModel role = default(UserRoleUpdateModel))
            {
                return operations.UpdateUserRoleAsync(role).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='role'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<UserRoleData> UpdateUserRoleAsync(this ISecurityAPI operations, UserRoleUpdateModel role = default(UserRoleUpdateModel), CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.UpdateUserRoleWithHttpMessagesAsync(role, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='role'>
            /// </param>
            public static void AssignUserRole(this ISecurityAPI operations, UserRoleMatchModel role = default(UserRoleMatchModel))
            {
                operations.AssignUserRoleAsync(role).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='role'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task AssignUserRoleAsync(this ISecurityAPI operations, UserRoleMatchModel role = default(UserRoleMatchModel), CancellationToken cancellationToken = default(CancellationToken))
            {
                (await operations.AssignUserRoleWithHttpMessagesAsync(role, null, cancellationToken).ConfigureAwait(false)).Dispose();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='role'>
            /// </param>
            public static void RevokeRoleFromUser(this ISecurityAPI operations, UserRoleMatchModel role = default(UserRoleMatchModel))
            {
                operations.RevokeRoleFromUserAsync(role).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='role'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task RevokeRoleFromUserAsync(this ISecurityAPI operations, UserRoleMatchModel role = default(UserRoleMatchModel), CancellationToken cancellationToken = default(CancellationToken))
            {
                (await operations.RevokeRoleFromUserWithHttpMessagesAsync(role, null, cancellationToken).ConfigureAwait(false)).Dispose();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='clientId'>
            /// </param>
            public static void VerifyUserRole(this ISecurityAPI operations, string clientId = default(string))
            {
                operations.VerifyUserRoleAsync(clientId).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='clientId'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task VerifyUserRoleAsync(this ISecurityAPI operations, string clientId = default(string), CancellationToken cancellationToken = default(CancellationToken))
            {
                (await operations.VerifyUserRoleWithHttpMessagesAsync(clientId, null, cancellationToken).ConfigureAwait(false)).Dispose();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='roleId'>
            /// </param>
            public static void DeleteUserRole(this ISecurityAPI operations, string roleId = default(string))
            {
                operations.DeleteUserRoleAsync(roleId).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='roleId'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task DeleteUserRoleAsync(this ISecurityAPI operations, string roleId = default(string), CancellationToken cancellationToken = default(CancellationToken))
            {
                (await operations.DeleteUserRoleWithHttpMessagesAsync(roleId, null, cancellationToken).ConfigureAwait(false)).Dispose();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='permissions'>
            /// </param>
            public static bool? SeedRoles(this ISecurityAPI operations, IList<UserPermissionData> permissions = default(IList<UserPermissionData>))
            {
                return operations.SeedRolesAsync(permissions).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='permissions'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<bool?> SeedRolesAsync(this ISecurityAPI operations, IList<UserPermissionData> permissions = default(IList<UserPermissionData>), CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.SeedRolesWithHttpMessagesAsync(permissions, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            public static IList<AlgoStoreUserData> GetAllUsersWithRoles(this ISecurityAPI operations)
            {
                return operations.GetAllUsersWithRolesAsync().GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<IList<AlgoStoreUserData>> GetAllUsersWithRolesAsync(this ISecurityAPI operations, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.GetAllUsersWithRolesWithHttpMessagesAsync(null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='clientId'>
            /// </param>
            public static AlgoStoreUserData GetUserByIdWithRoles(this ISecurityAPI operations, string clientId = default(string))
            {
                return operations.GetUserByIdWithRolesAsync(clientId).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='clientId'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<AlgoStoreUserData> GetUserByIdWithRolesAsync(this ISecurityAPI operations, string clientId = default(string), CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.GetUserByIdWithRolesWithHttpMessagesAsync(clientId, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Checks service is alive
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            public static object IsAlive(this ISecurityAPI operations)
            {
                return operations.IsAliveAsync().GetAwaiter().GetResult();
            }

            /// <summary>
            /// Checks service is alive
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<object> IsAliveAsync(this ISecurityAPI operations, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.IsAliveWithHttpMessagesAsync(null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

    }
}
