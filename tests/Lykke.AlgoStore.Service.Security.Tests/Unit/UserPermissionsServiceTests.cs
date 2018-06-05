using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Lykke.AlgoStore.Service.Security.Core.Domain;
using Lykke.AlgoStore.Service.Security.Core.Repositories;
using Lykke.AlgoStore.Service.Security.Services;
using Lykke.Service.PersonalData.Contract;
using Moq;
using NUnit.Framework;

namespace Lykke.AlgoStore.Service.Security.Tests.Unit
{
    [TestFixture]
    public class UserPermissionsServiceTests
    {
        private readonly string _permissionId = Guid.NewGuid().ToString();
        private readonly string _roleId = Guid.NewGuid().ToString();
        private readonly UserPermissionsService _service = Given_Correct_PermissionsService();

        [Test]
        public void GetAllPermissionsTest()
        {
            var result = When_Invoke_GetAllPermissions();
            Then_Result_ShouldNotBeEmpty(result);
        }

        [Test]
        public void GetPermissionByIdTest()
        {
            var result = When_Invoke_GetPermissionById();
            Then_Result_ShouldNotBeNull(result);
        }

        [Test]
        public void GetPermissionsByRoleIdTest()
        {
            var result = When_Invoke_GetPermissionsByRoleId();
            Then_Result_ShouldNotBeEmpty(result);
        }

        private List<UserPermissionData> When_Invoke_GetPermissionsByRoleId()
        {
            return _service.GetPermissionsByRoleIdAsync(_roleId).Result;
        }

        private UserPermissionData When_Invoke_GetPermissionById()
        {
            return _service.GetPermissionByIdAsync(_permissionId).Result;
        }

        private List<UserPermissionData> When_Invoke_GetAllPermissions()
        {
            return _service.GetAllPermissionsAsync().Result;
        }

        public static UserPermissionsService Given_Correct_PermissionsService()
        {
            var userPermissionsRepository = Given_Correct_UserPermissionsRepository();
            var rolePermissionMatchRepository = Given_Correct_RolePermissionMatchRepository();
            var rolesRepository = Given_Correct_UserRolesRepository();
            var userRolesService = Given_Correct_UserRolesService();

            return new UserPermissionsService(userPermissionsRepository, rolePermissionMatchRepository, rolesRepository,
                userRolesService);
        }

        public static UserRolesService Given_Correct_UserRolesService()
        {
            var userRolesRepository = Given_Correct_UserRolesRepository();
            var userPermissionsRepository = Given_Correct_UserPermissionsRepository();
            var userRoleMatchRepository = Given_Correct_UserRoleMatchRepository();
            var rolePermissionMatchRepository = Given_Correct_RolePermissionMatchRepository();
            var personalDataService = Given_Correct_PersonalDataservice();

            return new UserRolesService(userRolesRepository, userPermissionsRepository, userRoleMatchRepository,
                rolePermissionMatchRepository, personalDataService);
        }

        public static IPersonalDataService Given_Correct_PersonalDataservice()
        {
            var result = new Mock<IPersonalDataService>();

            return result.Object;
        }

        public static IUserRoleMatchRepository Given_Correct_UserRoleMatchRepository()
        {
            var fixture = new Fixture();
            var result = new Mock<IUserRoleMatchRepository>();

            result.Setup(repo => repo.GetUserRoleAsync(It.IsAny<string>(), It.IsAny<string>())).Returns(
                (string clientId, string roleId) =>
                {
                    var role = fixture.Build<UserRoleMatchData>().With(d => d.ClientId, clientId)
                        .With(d => d.RoleId, roleId).Create();
                    return Task.FromResult(role);
                });

            result.Setup(repo => repo.GetUserRolesAsync(It.IsAny<string>())).Returns((string clientId) =>
            {
                var roles = fixture.Build<UserRoleMatchData>().With(d => d.ClientId, clientId).CreateMany().ToList();
                return Task.FromResult(roles);
            });

            result.Setup(repo => repo.SaveUserRoleAsync(It.IsAny<UserRoleMatchData>()))
                .Returns((UserRoleMatchData data) => Task.FromResult(data));

            result.Setup(repo => repo.RevokeUserRole(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(() => Task.CompletedTask);

            return result.Object;
        }

        public static IUserRolesRepository Given_Correct_UserRolesRepository()
        {
            var fixture = new Fixture();
            var result = new Mock<IUserRolesRepository>();

            result.Setup(repo => repo.GetAllRolesAsync()).Returns(() =>
            {
                var roles = new List<UserRoleData>();
                roles.AddRange(fixture.Build<UserRoleData>().CreateMany());
                return Task.FromResult(roles);
            });

            result.Setup(repo => repo.GetRoleByIdAsync(It.IsAny<string>())).Returns((string roleId) =>
            {
                var role = fixture.Build<UserRoleData>()
                    .With(r => r.Id, roleId)
                    .Create();
                return Task.FromResult(role);
            });

            result.Setup(repo => repo.SaveRoleAsync(It.IsAny<UserRoleData>()))
                .Returns((UserRoleData data) => Task.FromResult(data));

            result.Setup(repo => repo.DeleteRoleAsync(It.IsAny<UserRoleData>()))
                .Returns((UserRoleData data) => Task.CompletedTask);

            return result.Object;
        }

        public static IUserPermissionsRepository Given_Correct_UserPermissionsRepository()
        {
            var fixture = new Fixture();
            var result = new Mock<IUserPermissionsRepository>();

            result.Setup(repo => repo.GetAllPermissionsAsync()).Returns(() =>
            {
                var permissions = new List<UserPermissionData>();
                permissions.AddRange(fixture.Build<UserPermissionData>().CreateMany());
                return Task.FromResult(permissions);
            });

            result.Setup(repo => repo.GetPermissionByIdAsync(It.IsAny<string>())).Returns((string permissionId) =>
            {
                var permission = fixture.Build<UserPermissionData>().With(p => p.Id, permissionId).Create();
                return Task.FromResult(permission);
            });

            result.Setup(repo => repo.SavePermissionAsync(It.IsAny<UserPermissionData>()))
                .Returns((UserPermissionData data) => { return Task.FromResult(data); });

            result.Setup(repo => repo.DeletePermissionAsync(It.IsAny<UserPermissionData>()))
                .Returns(() => { return Task.CompletedTask; });

            return result.Object;
        }

        public static IRolePermissionMatchRepository Given_Correct_RolePermissionMatchRepository()
        {
            var fixture = new Fixture();
            var result = new Mock<IRolePermissionMatchRepository>();

            result.Setup(repo => repo.GetPermissionIdsByRoleIdAsync(It.IsAny<string>())).Returns((string roleId) =>
            {
                var role = fixture.Build<RolePermissionMatchData>().With(d => d.RoleId, roleId).CreateMany().ToList();
                return Task.FromResult(role);
            });

            result.Setup(repo => repo.AssignPermissionToRoleAsync(It.IsAny<RolePermissionMatchData>()))
                .Returns((RolePermissionMatchData data) => Task.FromResult(data));

            result.Setup(repo => repo.RevokePermission(It.IsAny<RolePermissionMatchData>()))
                .Returns(() => Task.CompletedTask);

            return result.Object;
        }

        private static void Then_Result_ShouldNotBeEmpty(List<UserPermissionData> result)
        {
            Assert.NotNull(result);
            Assert.NotZero(result.Count);
        }

        private static void Then_Result_ShouldNotBeNull(UserPermissionData result)
        {
            Assert.NotNull(result);
        }
    }
}
