﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Lykke.AlgoStore.Service.Security.Core;
using Lykke.AlgoStore.Service.Security.Core.Domain;
using Lykke.AlgoStore.Service.Security.Core.Repositories;
using Lykke.AlgoStore.Service.Security.Core.Services;
using Lykke.AlgoStore.Service.Security.Services.Strings;
using Lykke.Service.PersonalData.Contract;

namespace Lykke.AlgoStore.Service.Security.Services
{
    public class UserRolesService : IUserRolesService
    {
        private readonly IUserRolesRepository _rolesRepository;
        private readonly IUserPermissionsRepository _permissionsRepository;
        private readonly IUserRoleMatchRepository _userRoleMatchRepository;
        private readonly IRolePermissionMatchRepository _rolePermissionMatchRepository;
        private readonly IPersonalDataService _personalDataService;

        public UserRolesService(IUserRolesRepository rolesRepository,
            IUserPermissionsRepository permissionsRepository,
            IUserRoleMatchRepository userRoleMatchRepository,
            IRolePermissionMatchRepository rolePermissionMatchRepository,
            IPersonalDataService personalDataService)
        {
            _rolesRepository = rolesRepository;
            _permissionsRepository = permissionsRepository;
            _userRoleMatchRepository = userRoleMatchRepository;
            _rolePermissionMatchRepository = rolePermissionMatchRepository;
            _personalDataService = personalDataService;
        }

        public async Task<List<UserRoleData>> GetAllRolesAsync()
        {
            var roles = await _rolesRepository.GetAllRolesAsync();

            foreach (var role in roles)
            {
                var permissionIds = await _rolePermissionMatchRepository.GetPermissionIdsByRoleIdAsync(role.Id);

                var permissions = new List<UserPermissionData>();
                foreach (var permission in permissionIds)
                {
                    var perm = await _permissionsRepository.GetPermissionByIdAsync(permission.PermissionId);
                    permissions.Add(perm);
                }

                role.Permissions = permissions;
            }

            return roles;
        }

        public async Task<UserRoleData> GetRoleByIdAsync(string roleId)
        {
            if (string.IsNullOrEmpty(roleId))
                throw new ValidationException(Phrases.RoleIdEmpty);

            var role = await _rolesRepository.GetRoleByIdAsync(roleId);

            if (role == null)
                return null;

            var permissionIds = await _rolePermissionMatchRepository.GetPermissionIdsByRoleIdAsync(roleId);

            var permissions = new List<UserPermissionData>();
            foreach (var permission in permissionIds)
            {
                var perm = await _permissionsRepository.GetPermissionByIdAsync(permission.PermissionId);
                permissions.Add(perm);
            }

            role.Permissions = permissions;
            return role;
        }

        public async Task<List<UserRoleData>> GetRolesByClientIdAsync(string clientId)
        {
            if (string.IsNullOrEmpty(clientId))
                throw new ValidationException(Phrases.ClientIdEmpty);

            var roleMatches = await _userRoleMatchRepository.GetUserRolesAsync(clientId);
            var roles = new List<UserRoleData>();
            foreach (var roleMatch in roleMatches)
            {
                var role = await GetRoleByIdAsync(roleMatch.RoleId);
                roles.Add(role);
            }

            return roles;
        }

        public async Task<List<AlgoStoreUserData>> GetAllUsersWithRolesAsync()
        {
            var result = new List<AlgoStoreUserData>();
            var matches = await _userRoleMatchRepository.GetAllMatchesAsync();
            var groupedClientIds = matches.GroupBy(m => m.ClientId).ToList();

            foreach (var item in groupedClientIds)
            {
                var data = new AlgoStoreUserData();
                var personalInformation = await _personalDataService.GetAsync(item.Key);
                data.ClientId = item.Key;
                data.FirstName = personalInformation?.FirstName;
                data.LastName = personalInformation?.LastName;
                data.FullName = personalInformation?.FullName;
                data.Email = personalInformation?.Email;

                data.Roles = item.Select(match => _rolesRepository.GetRoleByIdAsync(match.RoleId).Result).ToList();

                result.Add(data);
            }

            return result;
        }

        public async Task<AlgoStoreUserData> GetUserByIdWithRolesAsync(string clientId)
        {
            if (string.IsNullOrEmpty(clientId))
                throw new ValidationException(Phrases.ClientIdEmpty);

            var matches = await _userRoleMatchRepository.GetUserRolesAsync(clientId);

            var data = new AlgoStoreUserData();
            var personalInformation = await _personalDataService.GetAsync(clientId);
            data.ClientId = clientId;
            data.FirstName = personalInformation?.FirstName;
            data.LastName = personalInformation?.LastName;
            data.FullName = personalInformation?.FullName;
            data.Email = personalInformation?.Email;

            data.Roles = matches.Select(match => _rolesRepository.GetRoleByIdAsync(match.RoleId).Result).ToList();

            return data;
        }

        public async Task AssignRoleToUserAsync(UserRoleMatchData data)
        {
            if (string.IsNullOrEmpty(data.ClientId))
                throw new ValidationException(Phrases.ClientIdEmpty);

            if (string.IsNullOrEmpty(data.RoleId))
                throw new ValidationException(Phrases.RoleIdEmpty);

            var role = await _rolesRepository.GetRoleByIdAsync(data.RoleId);

            if (role == null)
                throw new ValidationException(string.Format(Phrases.RoleDoesNotExist, data.RoleId));

            var clientData = await _personalDataService.GetAsync(data.ClientId);

            if (clientData == null)
                throw new ValidationException(string.Format(Phrases.ClientDoesNotExist, data.ClientId));

            await _userRoleMatchRepository.SaveUserRoleAsync(data);
        }

        public async Task<UserRoleData> SaveRoleAsync(UserRoleData role)
        {
            if (role.Id == null)
            {
                if (String.IsNullOrEmpty(role.Name))
                    throw new ValidationException(Phrases.RoleNameRequired);

                if (await _rolesRepository.RoleExistsAsync(role.Name))
                {
                    throw new ValidationException(string.Format(Phrases.RoleNameExists, role.Name));
                }

                role.Id = Guid.NewGuid().ToString();
                role.CanBeModified = true;
                role.CanBeDeleted = true;
            }
            else
            {
                var dbRole = await _rolesRepository.GetRoleByIdAsync(role.Id);
                if (dbRole != null && !dbRole.CanBeModified)
                {
                    throw new ValidationException(Phrases.RoleIsImmutable);
                }

                // because the RK is the role name, in order to update it, first delete the old role and then replace it with the new one
                await _rolesRepository.DeleteRoleAsync(dbRole);
            }

            await _rolesRepository.SaveRoleAsync(role);
            return role;
        }

        public async Task RevokeRoleFromUserAsync(UserRoleMatchData data)
        {
            if (string.IsNullOrEmpty(data.RoleId))
                throw new ValidationException(Phrases.RoleIdEmpty);

            if (string.IsNullOrEmpty(data.ClientId))
                throw new ValidationException(Phrases.ClientIdEmpty);

            await _userRoleMatchRepository.RevokeUserRoleAsync(data.ClientId, data.RoleId);
        }

        public async Task VerifyUserRoleAsync(string clientId)
        {
            if (string.IsNullOrEmpty(clientId))
                throw new ValidationException(Phrases.ClientIdEmpty);

            var clientData = await _personalDataService.GetAsync(clientId);

            if (clientData == null)
                throw new ValidationException(string.Format(Phrases.ClientDoesNotExist, clientId));

            var roles = await _userRoleMatchRepository.GetUserRolesAsync(clientId);

            if (roles.Count == 0)
            {
                var allRoles = await _rolesRepository.GetAllRolesAsync();

                // original user role cannot be deleted
                var userRole = allRoles.FirstOrDefault(role => role.Name == "User" && !role.CanBeDeleted);

                if (userRole == null)
                    throw new ValidationException(Phrases.UserRoleNotAssigned);

                await _userRoleMatchRepository.SaveUserRoleAsync(new UserRoleMatchData
                {
                    RoleId = userRole.Id,
                    ClientId = clientId
                });
            }
        }

        public async Task DeleteRoleAsync(string roleId)
        {
            if (string.IsNullOrEmpty(roleId))
                throw new ValidationException(Phrases.RoleIdEmpty);

            var role = await _rolesRepository.GetRoleByIdAsync(roleId);

            if (role == null)
                throw new ValidationException(string.Format(Phrases.RoleDoesNotExist, roleId));

            //first check if the role has permissions assigned
            var permissionsForRole = await _rolePermissionMatchRepository.GetPermissionIdsByRoleIdAsync(roleId);
            foreach (var permissionForRole in permissionsForRole)
            {
                // if it does, delete them
                await _rolePermissionMatchRepository.RevokePermissionAsync(permissionForRole);
            }

            //then check if any user is assigned to this role
            var allMatches = await _userRoleMatchRepository.GetAllMatchesAsync();
            var usersWithRole = allMatches.Where(m => m.RoleId == roleId).ToList();

            if (usersWithRole.Count > 0)
            {
                // it there are any, revoke it
                foreach (var match in usersWithRole)
                {
                    await _userRoleMatchRepository.RevokeUserRoleAsync(match.ClientId, match.RoleId);
                }
            }

            if (!role.CanBeDeleted)
                throw new ValidationException(Phrases.RoleIsImmutable);

            await _rolesRepository.DeleteRoleAsync(role);
        }

        public async Task SeedRolesAsync(List<UserPermissionData> permissions)
        {
            var allRoles = await GetAllRolesAsync();

            // Check if administrator role exists, if not - seed it
            // Note: Only the original administrator role cannot be deleted
            var adminRole =
                allRoles.FirstOrDefault(role => role.Name == Constants.AdminRoleName && !role.CanBeDeleted);

            // If there is no administrator role, we need to seed it
            if (adminRole == null)
            {
                adminRole = new UserRoleData
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = Constants.AdminRoleName,
                    CanBeDeleted = false,
                    CanBeModified = false
                };

                // Create the administrator role
                await SaveRoleAsync(adminRole);
            }

            // Check if user role exists, if not - seed it. Don't touch it if it exists
            // Note: Only the original user role cannot be deleted
            var userRole =
                allRoles.FirstOrDefault(role => role.Name == Constants.UserRoleName && !role.CanBeDeleted);

            if (userRole == null)
            {
                userRole = new UserRoleData
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = Constants.UserRoleName,
                    CanBeDeleted = false,
                    CanBeModified = true
                };

                // Create the User role
                await SaveRoleAsync(userRole);
            }

            // Seed the permissions for the administrator role
            foreach (var permission in permissions)
            {
                var match = new RolePermissionMatchData
                {
                    RoleId = adminRole.Id,
                    PermissionId = permission.Id
                };

                await _rolePermissionMatchRepository.AssignPermissionToRoleAsync(match);
            }
        }
    }
}
