using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Log;
using Lykke.AlgoStore.Service.Security.Core.Domain;
using Lykke.AlgoStore.Service.Security.Core.Repositories;
using Lykke.AlgoStore.Service.Security.Core.Services;
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
        private readonly ILog _log;

        public UserRolesService(IUserRolesRepository rolesRepository,
            IUserPermissionsRepository permissionsRepository,
            IUserRoleMatchRepository userRoleMatchRepository,
            IRolePermissionMatchRepository rolePermissionMatchRepository,
            IPersonalDataService personalDataService,
            ILog log)
        {
            _rolesRepository = rolesRepository;
            _permissionsRepository = permissionsRepository;
            _userRoleMatchRepository = userRoleMatchRepository;
            _rolePermissionMatchRepository = rolePermissionMatchRepository;
            _personalDataService = personalDataService;
            _log = log;
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
                throw new Exception("RoleId is empty.");

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
                throw new Exception("ClientId is empty.");

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

        public async Task<AlgoStoreUserData> GeyUserByIdWithRoles(string clientId)
        {
            if (string.IsNullOrEmpty(clientId))
                throw new Exception("ClientId is empty.");

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

        public async Task AssignRoleToUser(UserRoleMatchData data)
        {
            if (string.IsNullOrEmpty(data.ClientId))
                throw new Exception("ClientId is empty.");

            if (string.IsNullOrEmpty(data.RoleId))
                throw new Exception("RoleId is empty.");

            var role = await _rolesRepository.GetRoleByIdAsync(data.RoleId);

            if (role == null)
                throw new Exception($"Role with id {data.RoleId} does not exist.");

            var clientData = await _personalDataService.GetAsync(data.ClientId);

            if (clientData == null)
                throw new Exception($"Client with id {data.ClientId} does not exist.");

            await _userRoleMatchRepository.SaveUserRoleAsync(data);
        }

        public async Task<UserRoleData> SaveRoleAsync(UserRoleData role)
        {
            if (role.Id == null)
            {
                if (String.IsNullOrEmpty(role.Name))
                    throw new Exception("Role name is required.");

                if (await _rolesRepository.RoleExistsAsync(role.Name))
                {
                    throw new Exception($"Role {role.Name} already exists.");
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
                    throw new Exception("This role can't be modified.");
                }

                // because the RK is the role name, in order to update it, first delete the old role and then replace it with the new one
                await _rolesRepository.DeleteRoleAsync(dbRole);
            }

            await _rolesRepository.SaveRoleAsync(role);
            return role;
        }

        public async Task RevokeRoleFromUser(UserRoleMatchData data)
        {
            if (string.IsNullOrEmpty(data.RoleId))
                throw new Exception("RoleId is empty.");

            if (string.IsNullOrEmpty(data.ClientId))
                throw new Exception("ClientId is empty.");

            await _userRoleMatchRepository.RevokeUserRole(data.ClientId, data.RoleId);
        }

        public async Task VerifyUserRole(string clientId)
        {
            if (string.IsNullOrEmpty(clientId))
                throw new Exception("ClientId is empty.");

            var clientData = await _personalDataService.GetAsync(clientId);

            if (clientData == null)
                throw new Exception($"Client with id {clientId} does not exist.");

            var roles = await _userRoleMatchRepository.GetUserRolesAsync(clientId);

            if (roles.Count == 0)
            {
                var allRoles = await _rolesRepository.GetAllRolesAsync();

                // original user role cannot be deleted
                var userRole = allRoles.FirstOrDefault(role => role.Name == "User" && !role.CanBeDeleted);

                if (userRole == null)
                    throw new Exception("Current user does not belong to 'User' role.");

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
                throw new Exception("RoleId is empty.");

            var role = await _rolesRepository.GetRoleByIdAsync(roleId);

            if (role == null)
                throw new Exception($"Role with id {roleId} does not exist.");

            //first check if the role has permissions assigned
            var permissionsForRole = await _rolePermissionMatchRepository.GetPermissionIdsByRoleIdAsync(roleId);
            foreach (var permissionForRole in permissionsForRole)
            {
                // if it does, delete them
                await _rolePermissionMatchRepository.RevokePermission(permissionForRole);
            }

            //then check if any user is assigned to this role
            var allMatches = await _userRoleMatchRepository.GetAllMatchesAsync();
            var usersWithRole = allMatches.Where(m => m.RoleId == roleId).ToList();

            if (usersWithRole.Count > 0)
            {
                // it there are any, revoke it
                foreach (var match in usersWithRole)
                {
                    await _userRoleMatchRepository.RevokeUserRole(match.ClientId, match.RoleId);
                }
            }

            if (!role.CanBeDeleted)
                throw new Exception("This role cannot be deleted.");

            await _rolesRepository.DeleteRoleAsync(role);
        }
    }
}
