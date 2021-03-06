﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.AlgoStore.Service.Security.Core.Domain;

namespace Lykke.AlgoStore.Service.Security.Core.Services
{
    public interface IUserPermissionsService
    {
        Task<List<UserPermissionData>> GetAllPermissionsAsync();
        Task<UserPermissionData> GetPermissionByIdAsync(string permissionId);
        Task<List<UserPermissionData>> GetPermissionsByRoleIdAsync(string roleId);
        Task<UserPermissionData> SavePermissionAsync(UserPermissionData data);
        Task RevokePermissionsFromRoleAsync(List<RolePermissionMatchData> data);
        Task AssignPermissionsToRoleAsync(List<RolePermissionMatchData> data);
        Task DeletePermissionAsync(string permissionId);
        Task<bool> HasPermissionAsync(string clientId, string permissionId);
        Task SeedPermissionsAsync(List<UserPermissionData> permissions);
    }
}
