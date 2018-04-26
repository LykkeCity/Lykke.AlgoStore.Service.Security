using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Common.Log;
using Lykke.AlgoStore.Service.Security.Core.Domain;
using Lykke.AlgoStore.Service.Security.Core.Repositories;
using Lykke.AlgoStore.Service.Security.Core.Services;

namespace Lykke.AlgoStore.Service.Security.Services
{
    public class UserPermissionsService : IUserPermissionsService
    {
        private readonly IUserRolesRepository _rolesRepository;
        private readonly ILog _log;
        private readonly IUserPermissionsRepository _permissionsRepository;
        private readonly IRolePermissionMatchRepository _rolePermissionMatchRepository;

        public UserPermissionsService(IUserPermissionsRepository permissionsRepository,
            IRolePermissionMatchRepository rolePermissionMatchRepository,
            IUserRolesRepository rolesRepository,
            ILog log)
        {
            _permissionsRepository = permissionsRepository;
            _rolePermissionMatchRepository = rolePermissionMatchRepository;
            _rolesRepository = rolesRepository;
            _log = log;
        }

        public async Task AssignPermissionsToRoleAsync(List<RolePermissionMatchData> data)
        {
            foreach (var permission in data)
            {
                if (string.IsNullOrEmpty(permission.PermissionId))
                    throw new Exception("PermissionId is empty.");

                if (string.IsNullOrEmpty(permission.RoleId))
                    throw new Exception("RoleId is empty.");

                var dbPermission = await _permissionsRepository.GetPermissionByIdAsync(permission.PermissionId);

                if (dbPermission == null)
                    throw new Exception($"Permission with id {permission.PermissionId} does not exist.");

                var role = await _rolesRepository.GetRoleByIdAsync(permission.RoleId);

                if (role == null)
                    throw new Exception($"Role with id {permission.RoleId} does not exist.");

                if (!role.CanBeModified)
                    throw new Exception("The permissions of this role cannot be modified.");

                await _rolePermissionMatchRepository.AssignPermissionToRoleAsync(permission);
            }
        }

        public async Task<List<UserPermissionData>> GetAllPermissionsAsync()
        {
            var result = await _permissionsRepository.GetAllPermissionsAsync();
            return result;
        }

        public async Task<UserPermissionData> GetPermissionByIdAsync(string permissionId)
        {
            if (string.IsNullOrEmpty(permissionId))
                throw new Exception("PermissionId is empty.");

            var result = await _permissionsRepository.GetPermissionByIdAsync(permissionId);
            return result;
        }

        public async Task<List<UserPermissionData>> GetPermissionsByRoleIdAsync(string roleId)
        {
            if (string.IsNullOrEmpty(roleId))
                throw new Exception("RoleId is empty.");

            var matches = await _rolePermissionMatchRepository.GetPermissionIdsByRoleIdAsync(roleId);
            var permissions = new List<UserPermissionData>();

            foreach (var match in matches)
            {
                var permission = await GetPermissionByIdAsync(match.PermissionId);
                permissions.Add(permission);
            }

            return permissions;
        }

        public async Task<UserPermissionData> SavePermissionAsync(UserPermissionData data)
        {
            if (data.Id == null)
            {
                data.Id = Guid.NewGuid().ToString();
                data.DisplayName = Regex.Replace(data.Name, "([A-Z]{1,2}|[0-9]+)", " $1").TrimStart();
            }

            await _permissionsRepository.SavePermissionAsync(data);
            return data;
        }

        public async Task DeletePermissionAsync(string permissionId)
        {
            if (string.IsNullOrEmpty(permissionId))
                throw new Exception("PermissionId is empty.");

            var permission = await GetPermissionByIdAsync(permissionId);

            if (permission == null)
                throw new Exception("Permission with this ID does not exist");

            await _permissionsRepository.DeletePermissionAsync(permission);
        }

        public async Task RevokePermissionsFromRole(List<RolePermissionMatchData> data)
        {
            foreach (var permission in data)
            {
                if (string.IsNullOrEmpty(permission.RoleId))
                    throw new Exception("RoleId is empty.");

                if (string.IsNullOrEmpty(permission.PermissionId))
                    throw new Exception("PermissionId is empty.");

                var role = await _rolesRepository.GetRoleByIdAsync(permission.RoleId);

                if (!role.CanBeModified)
                    throw new Exception("The permissions of this role cannot be modified.");

                await _rolePermissionMatchRepository.RevokePermission(permission);
            }
        }
    }
}
