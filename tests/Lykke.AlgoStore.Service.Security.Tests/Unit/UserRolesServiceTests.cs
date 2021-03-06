﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Lykke.AlgoStore.Service.Security.Core.Domain;
using Lykke.AlgoStore.Service.Security.Core.Repositories;
using Lykke.AlgoStore.Service.Security.Core.Services;
using Lykke.AlgoStore.Service.Security.Services;
using Lykke.Service.PersonalData.Contract;
using Moq;
using NUnit.Framework;

namespace Lykke.AlgoStore.Service.Security.Tests.Unit
{
    [TestFixture]
    public class UserRolesServiceTests
    {
        private readonly string _clientId = Guid.NewGuid().ToString();
        private readonly string _roleId = Guid.NewGuid().ToString();
        private readonly UserRolesService _service = Given_Correct_UserRolesService();

        [Test]
        public void GetAllRolesTest()
        {
            var result = When_Invoke_GetAllRoles();
            Then_Result_ShouldNotBeEmpty(result);
            Then_Result_ShouldHavePermissions(result);
        }

        [Test]
        public void GetRoleByIdTest()
        {
            var result = When_Invoke_GetById();
            Then_Result_ShouldNotBeNull(result);
            Then_Result_ShouldHavePermissions(result);
        }

        [Test]
        public void GetRolesByClientIdTest()
        {
            var result = When_Invoke_GetByClientId();
            Then_Result_ShouldNotBeEmpty(result);
            Then_Result_ShouldHavePermissions(result);
        }

        private void When_Invoke_AssignRoleToUser()
        {
            var roleMatchData = new UserRoleMatchData
            {
                RoleId = _roleId,
                ClientId = _clientId
            };
            _service.AssignRoleToUserAsync(roleMatchData).Wait();
        }

        private List<UserRoleData> When_Invoke_GetByClientId()
        {
            return _service.GetRolesByClientIdAsync(_clientId).Result;
        }

        private UserRoleData When_Invoke_GetById()
        {
            return _service.GetRoleByIdAsync(_roleId).Result;
        }

        private List<UserRoleData> When_Invoke_GetAllRoles()
        {
            return _service.GetAllRolesAsync().Result;
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
                .Returns((UserPermissionData data) => Task.FromResult(data));

            result.Setup(repo => repo.DeletePermissionAsync(It.IsAny<UserPermissionData>()))
                .Returns(() => Task.CompletedTask);

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

            result.Setup(repo => repo.RevokeUserRoleAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(() => Task.CompletedTask);

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

            result.Setup(repo => repo.RevokePermissionAsync(It.IsAny<RolePermissionMatchData>()))
                .Returns(() => Task.CompletedTask);

            return result.Object;
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

        public static IUserPermissionsService Given_Correct_PermissionsService()
        {
            var userPermissionsRepository = Given_Correct_UserPermissionsRepository();
            var rolePermissionMatchRepository = Given_Correct_RolePermissionMatchRepository();
            var userRolesRepository = Given_Correct_UserRolesRepository();
            var userRolesService = Given_Correct_UserRolesService();
            return new UserPermissionsService(userPermissionsRepository, rolePermissionMatchRepository,
                userRolesRepository, userRolesService);
        }

        public static IPersonalDataService Given_Correct_PersonalDataservice()
        {
            var result = new Mock<IPersonalDataService>();

            return result.Object;
        }

        private static void Then_Result_ShouldHavePermissions(List<UserRoleData> result)
        {
            foreach (var item in result)
            {
                Assert.NotNull(item.Permissions);
                Assert.NotZero(item.Permissions.Count);
            }
        }

        private static void Then_Result_ShouldNotBeEmpty(List<UserRoleData> result)
        {
            Assert.NotNull(result);
            Assert.NotZero(result.Count);
        }

        private static void Then_Result_ShouldHavePermissions(UserRoleData result)
        {
            Assert.NotNull(result);
            Assert.NotZero(result.Permissions.Count);
        }

        private static void Then_Result_ShouldNotBeNull(UserRoleData result)
        {
            Assert.NotNull(result);
        }
    }
}
