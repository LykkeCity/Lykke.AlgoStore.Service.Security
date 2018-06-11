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
        private UserPermissionsService _userPermissionsService;
        private UserRolesService _userRolesService;
        private IUserPermissionsRepository _userPermissionsRepository;
        private IRolePermissionMatchRepository _rolePermissionMatchRepository;
        private IUserRolesRepository _userRolesRepository;
        private IUserRoleMatchRepository _userRoleMatchRepository;
        private readonly IPersonalDataService _personalDataService = Given_Correct_PersonalDataService();
        private readonly Fixture _fixture = new Fixture();

        [SetUp]
        public void SetUp()
        {
            _userPermissionsRepository = Given_Correct_UserPermissionsRepository();
            _rolePermissionMatchRepository = Given_Correct_RolePermissionMatchRepository();
            _userRolesRepository = Given_Correct_UserRolesRepository();
            _userRoleMatchRepository = Given_Correct_UserRoleMatchRepository();

            _userRolesService = Given_Correct_UserRolesService();
            _userPermissionsService = Given_Correct_PermissionsService();
        }

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

        [Test]
        public void HasPermissionForEmptyClientIdWillThrowException()
        {
            Assert.Throws<AggregateException>(() => When_Invoke_HasPermission(string.Empty, "test"));
        }

        [Test]
        public void HasPermissionForEmptyPermissionIdWillThrowException()
        {
            Assert.Throws<AggregateException>(() => When_Invoke_HasPermission("test", string.Empty));
        }

        [Test]
        public void HasPermissionForExistingClientWithoutPermissionWillReturnFalse()
        {
            var result = When_Invoke_HasPermission("test", "unknown");

            Assert.IsFalse(result);
        }

        //REMARK: Need to rework test from below when refactoring is done
        //Until then, test will be commented out
        //[Test]
        //public void HasPermissionForExistingClientWithPermissionWillReturnTrue()
        //{
        //    var result = When_Invoke_HasPermission("test", "test");

        //    Assert.IsTrue(result);
        //}

        private bool When_Invoke_HasPermission(string clientId, string permissionId)
        {
            return _userPermissionsService.HasPermission(clientId, permissionId).Result;
        }

        private List<UserPermissionData> When_Invoke_GetPermissionsByRoleId()
        {
            return _userPermissionsService.GetPermissionsByRoleIdAsync(_roleId).Result;
        }

        private UserPermissionData When_Invoke_GetPermissionById()
        {
            return _userPermissionsService.GetPermissionByIdAsync(_permissionId).Result;
        }

        private List<UserPermissionData> When_Invoke_GetAllPermissions()
        {
            return _userPermissionsService.GetAllPermissionsAsync().Result;
        }

        public UserPermissionsService Given_Correct_PermissionsService()
        {
            return new UserPermissionsService(_userPermissionsRepository, _rolePermissionMatchRepository, _userRolesRepository,
                _userRolesService);
        }

        public UserRolesService Given_Correct_UserRolesService()
        {
            return new UserRolesService(_userRolesRepository, _userPermissionsRepository, _userRoleMatchRepository,
                _rolePermissionMatchRepository, _personalDataService);
        }

        public static IPersonalDataService Given_Correct_PersonalDataService()
        {
            var result = new Mock<IPersonalDataService>();

            return result.Object;
        }

        public IUserRoleMatchRepository Given_Correct_UserRoleMatchRepository()
        {
            var result = new Mock<IUserRoleMatchRepository>();

            result.Setup(repo => repo.GetUserRoleAsync(It.IsAny<string>(), It.IsAny<string>())).Returns(
                (string clientId, string roleId) =>
                {
                    var role = _fixture.Build<UserRoleMatchData>().With(d => d.ClientId, clientId)
                        .With(d => d.RoleId, roleId).Create();
                    return Task.FromResult(role);
                });

            result.Setup(repo => repo.GetUserRolesAsync(It.IsAny<string>())).Returns((string clientId) =>
            {
                var roles = _fixture.Build<UserRoleMatchData>().With(d => d.ClientId, clientId).CreateMany().ToList();

                roles.Add(new UserRoleMatchData { ClientId = "test", RoleId = "test" });

                return Task.FromResult(roles);
            });

            result.Setup(repo => repo.SaveUserRoleAsync(It.IsAny<UserRoleMatchData>()))
                .Returns((UserRoleMatchData data) => Task.FromResult(data));

            result.Setup(repo => repo.RevokeUserRoleAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(() => Task.CompletedTask);

            return result.Object;
        }

        public IUserRolesRepository Given_Correct_UserRolesRepository()
        {
            var result = new Mock<IUserRolesRepository>();

            result.Setup(repo => repo.GetAllRolesAsync()).Returns(() =>
            {
                var roles = new List<UserRoleData>();
                roles.AddRange(_fixture.Build<UserRoleData>().CreateMany());
                return Task.FromResult(roles);
            });

            result.Setup(repo => repo.GetRoleByIdAsync(It.IsAny<string>())).Returns((string roleId) =>
            {
                var role = _fixture.Build<UserRoleData>()
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

        public IUserPermissionsRepository Given_Correct_UserPermissionsRepository()
        {
            var result = new Mock<IUserPermissionsRepository>();

            result.Setup(repo => repo.GetAllPermissionsAsync()).Returns(() =>
            {
                var permissions = new List<UserPermissionData>();
                permissions.AddRange(_fixture.Build<UserPermissionData>().CreateMany());
                return Task.FromResult(permissions);
            });

            result.Setup(repo => repo.GetPermissionByIdAsync(It.IsAny<string>())).Returns((string permissionId) =>
            {
                var permission = _fixture.Build<UserPermissionData>().With(p => p.Id, permissionId).Create();
                return Task.FromResult(permission);
            });

            result.Setup(repo => repo.SavePermissionAsync(It.IsAny<UserPermissionData>()))
                .Returns((UserPermissionData data) => { return Task.FromResult(data); });

            result.Setup(repo => repo.DeletePermissionAsync(It.IsAny<UserPermissionData>()))
                .Returns(() => { return Task.CompletedTask; });

            return result.Object;
        }

        public IRolePermissionMatchRepository Given_Correct_RolePermissionMatchRepository()
        {
            var result = new Mock<IRolePermissionMatchRepository>();

            result.Setup(repo => repo.GetPermissionIdsByRoleIdAsync(It.IsAny<string>())).Returns((string roleId) =>
            {
                var role = _fixture.Build<RolePermissionMatchData>().With(d => d.RoleId, roleId).CreateMany().ToList();
                return Task.FromResult(role);
            });

            result.Setup(repo => repo.AssignPermissionToRoleAsync(It.IsAny<RolePermissionMatchData>()))
                .Returns((RolePermissionMatchData data) => Task.FromResult(data));

            result.Setup(repo => repo.RevokePermissionAsync(It.IsAny<RolePermissionMatchData>()))
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
