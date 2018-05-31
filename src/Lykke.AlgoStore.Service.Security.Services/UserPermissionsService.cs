using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Lykke.AlgoStore.Service.Security.Core.Domain;
using Lykke.AlgoStore.Service.Security.Core.Repositories;
using Lykke.AlgoStore.Service.Security.Core.Services;
using Lykke.AlgoStore.Service.Security.Services.Strings;

namespace Lykke.AlgoStore.Service.Security.Services
{
    public class UserPermissionsService : IUserPermissionsService
    {
        private readonly IUserRolesRepository _rolesRepository;
        private readonly IUserPermissionsRepository _permissionsRepository;
        private readonly IRolePermissionMatchRepository _rolePermissionMatchRepository;

        public UserPermissionsService(IUserPermissionsRepository permissionsRepository,
            IRolePermissionMatchRepository rolePermissionMatchRepository,
            IUserRolesRepository rolesRepository)
        {
            _permissionsRepository = permissionsRepository;
            _rolePermissionMatchRepository = rolePermissionMatchRepository;
            _rolesRepository = rolesRepository;
        }

        public async Task AssignPermissionsToRoleAsync(List<RolePermissionMatchData> data)
        {
            foreach (var permission in data)
            {
                if (string.IsNullOrEmpty(permission.PermissionId))
                    throw new Exception(Phrases.PermissionIdEmpty);

                if (string.IsNullOrEmpty(permission.RoleId))
                    throw new Exception(Phrases.RoleIdEmpty);

                var dbPermission = await _permissionsRepository.GetPermissionByIdAsync(permission.PermissionId);

                if (dbPermission == null)
                    throw new Exception(string.Format(Phrases.PermissionDoesNotExist, permission.PermissionId));

                var role = await _rolesRepository.GetRoleByIdAsync(permission.RoleId);

                if (role == null)
                    throw new Exception(string.Format(Phrases.RoleDoesNotExist, permission.RoleId));

                if (!role.CanBeModified)
                    throw new Exception(Phrases.PermissionsAreImmutable);

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
                throw new Exception(Phrases.PermissionIdEmpty);

            var result = await _permissionsRepository.GetPermissionByIdAsync(permissionId);
            return result;
        }

        public async Task<List<UserPermissionData>> GetPermissionsByRoleIdAsync(string roleId)
        {
            if (string.IsNullOrEmpty(roleId))
                throw new Exception(Phrases.RoleIdEmpty);

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
                throw new Exception(Phrases.PermissionIdEmpty);

            var permission = await GetPermissionByIdAsync(permissionId);

            if (permission == null)
                throw new Exception(string.Format(Phrases.PermissionDoesNotExist, permissionId));

            await _permissionsRepository.DeletePermissionAsync(permission);
        }

        public async Task RevokePermissionsFromRole(List<RolePermissionMatchData> data)
        {
            foreach (var permission in data)
            {
                if (string.IsNullOrEmpty(permission.RoleId))
                    throw new Exception(Phrases.RoleIdEmpty);

                if (string.IsNullOrEmpty(permission.PermissionId))
                    throw new Exception(Phrases.PermissionIdEmpty);

                var role = await _rolesRepository.GetRoleByIdAsync(permission.RoleId);

                if (!role.CanBeModified)
                    throw new Exception(Phrases.PermissionsAreImmutable);

                await _rolePermissionMatchRepository.RevokePermission(permission);
            }
        }
    }
}
