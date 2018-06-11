using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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
        private readonly IUserRolesService _userRolesService;

        public UserPermissionsService(IUserPermissionsRepository permissionsRepository,
            IRolePermissionMatchRepository rolePermissionMatchRepository,
            IUserRolesRepository rolesRepository, IUserRolesService userRolesService)
        {
            _permissionsRepository = permissionsRepository;
            _rolePermissionMatchRepository = rolePermissionMatchRepository;
            _rolesRepository = rolesRepository;
            _userRolesService = userRolesService;
        }

        public async Task AssignPermissionsToRoleAsync(List<RolePermissionMatchData> data)
        {
            foreach (var permission in data)
            {
                if (string.IsNullOrEmpty(permission.PermissionId))
                    throw new ValidationException(Phrases.PermissionIdEmpty);

                if (string.IsNullOrEmpty(permission.RoleId))
                    throw new ValidationException(Phrases.RoleIdEmpty);

                var dbPermission = await _permissionsRepository.GetPermissionByIdAsync(permission.PermissionId);

                if (dbPermission == null)
                    throw new ValidationException(string.Format(Phrases.PermissionDoesNotExist, permission.PermissionId));

                var role = await _rolesRepository.GetRoleByIdAsync(permission.RoleId);

                if (role == null)
                    throw new ValidationException(string.Format(Phrases.RoleDoesNotExist, permission.RoleId));

                if (!role.CanBeModified)
                    throw new ValidationException(Phrases.PermissionsAreImmutable);

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
                throw new ValidationException(Phrases.PermissionIdEmpty);

            var result = await _permissionsRepository.GetPermissionByIdAsync(permissionId);
            return result;
        }

        public async Task<List<UserPermissionData>> GetPermissionsByRoleIdAsync(string roleId)
        {
            if (string.IsNullOrEmpty(roleId))
                throw new ValidationException(Phrases.RoleIdEmpty);

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
                throw new ValidationException(Phrases.PermissionIdEmpty);

            var permission = await GetPermissionByIdAsync(permissionId);

            if (permission == null)
                throw new ValidationException(string.Format(Phrases.PermissionDoesNotExist, permissionId));

            await _permissionsRepository.DeletePermissionAsync(permission);
        }

        public async Task RevokePermissionsFromRole(List<RolePermissionMatchData> data)
        {
            foreach (var permission in data)
            {
                if (string.IsNullOrEmpty(permission.RoleId))
                    throw new ValidationException(Phrases.RoleIdEmpty);

                if (string.IsNullOrEmpty(permission.PermissionId))
                    throw new ValidationException(Phrases.PermissionIdEmpty);

                var role = await _rolesRepository.GetRoleByIdAsync(permission.RoleId);

                if (!role.CanBeModified)
                    throw new ValidationException(Phrases.PermissionsAreImmutable);

                await _rolePermissionMatchRepository.RevokePermissionAsync(permission);
            }
        }

        public async Task<bool> HasPermission(string clientId, string permissionId)
        {
            if (string.IsNullOrEmpty(clientId))
                throw new ValidationException(Phrases.ClientIdEmpty);

            if (string.IsNullOrEmpty(permissionId))
                throw new ValidationException(Phrases.PermissionIdEmpty);

            var userRoles = await _userRolesService.GetRolesByClientIdAsync(clientId);

            return userRoles.Any(x => x.Permissions.Any(y => y.Id == permissionId));
        }

        public async Task SeedPermissions(List<UserPermissionData> permissions)
        {
            // check if we should delete any old permissions
            var allPermissions = await GetAllPermissionsAsync();

            var permissionsToDelete = allPermissions
                .Where(x => !permissions.Any(y => y.Name == x.Name && y.Id == x.Id)) //Must compare by Id and Name
                .ToList();

            if (permissionsToDelete.Any())
            {
                var allRoles = await _userRolesService.GetAllRolesAsync();

                // delete old unneeded permissions
                foreach (var permissionToDelete in permissionsToDelete)
                {
                    // first check if the permission has been referenced in any role
                    var matches = allRoles.Where(role =>
                            role.Permissions.Any(
                                p => p.Id == permissionToDelete.Id && p.Name == permissionToDelete.Name))
                        .ToList();

                    // if the permission is referenced, remove the reference
                    if (matches.Any())
                    {
                        foreach (var reference in matches)
                        {
                            await _rolePermissionMatchRepository.RevokePermissionAsync(new RolePermissionMatchData
                            {
                                RoleId = reference.Id,
                                PermissionId = permissionToDelete.Id
                            });
                        }
                    }

                    // finally delete the permission
                    await DeletePermissionAsync(permissionToDelete.Id);
                }
            }

            // refresh current permissions
            foreach (var permission in permissions)
            {
                await SavePermissionAsync(permission);
            }
        }
    }
}
