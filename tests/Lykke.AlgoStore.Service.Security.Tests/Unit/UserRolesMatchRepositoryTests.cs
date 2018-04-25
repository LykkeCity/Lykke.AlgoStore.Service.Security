﻿using System.Collections.Generic;
using AutoFixture;
using AzureStorage.Tables;
using Lykke.AlgoStore.Service.Security.AzureRepositories.Entities;
using Lykke.AlgoStore.Service.Security.AzureRepositories.Repositories;
using Lykke.AlgoStore.Service.Security.Core.Domain;
using Lykke.AlgoStore.Service.Security.Tests.Infrastructure;
using NUnit.Framework;

namespace Lykke.AlgoStore.Service.Security.Tests.Unit
{
    [TestFixture]
    public class UserRolesMatchRepositoryTests
    {
        private const string ClientId = "066ABDEF-F1CB-4B24-8EE6-6ACAF1FD623D";
        private UserRoleMatchData _entity;
        private readonly Fixture _fixture = new Fixture();

        private readonly UserRolesMatchRepository _repo = new UserRolesMatchRepository(
            AzureTableStorage<UserRoleMatchEntity>.Create(SettingsMock.GetTableStorageConnectionString(),
                UserRolesMatchRepository.TableName, new LogMock()));

        [SetUp]
        public void SetUp()
        {
            _entity = _fixture.Build<UserRoleMatchData>().With(data => data.RoleId, "TestRoleId")
                .With(data => data.ClientId, ClientId).Create();
        }

        [TearDown]
        public void CleanUp()
        {
            _repo.RevokeUserRole(_entity.ClientId, _entity.RoleId).Wait();
            _entity = null;
        }

        [Test, Explicit("Should run manually only. Manipulate data in Table Storage")]
        public void AssignUserRoleTest()
        {
            When_Invoke_AssignUserRole();
            Then_Data_ShouldBeSaved();
        }

        [Test, Explicit("Should run manually only. Manipulate data in Table Storage")]
        public void GetUserRolesTest()
        {
            var result = When_Invoke_GetUserRoles();
            Then_Result_ShouldNotBe_Null(result);
        }

        [Test, Explicit("Should run manually only. Manipulate data in Table Storage")]
        public void GetUserRoleTest()
        {
            var result = When_Invoke_GetUserRole();
            Then_Data_ShouldNotBeNull(result);
        }

        [Test, Explicit("Should run manually only. Manipulate data in Table Storage")]
        public void RevokeUserRoleTest()
        {
            When_Invoke_RevokeUserRole();
            Then_Role_ShouldNotExist();
        }

        private void Then_Role_ShouldNotExist()
        {
            var result = _repo.GetUserRoleAsync(_entity.ClientId, _entity.RoleId).Result;
            Assert.IsNull(result);
        }

        private void When_Invoke_RevokeUserRole()
        {
            _repo.RevokeUserRole(_entity.ClientId, _entity.RoleId).Wait();
        }

        private void Then_Data_ShouldNotBeNull(UserRoleMatchData result)
        {
            Assert.NotNull(result);
        }

        private UserRoleMatchData When_Invoke_GetUserRole()
        {
            // be sure to have a role
            _repo.SaveUserRoleAsync(_entity).Wait();

            return _repo.GetUserRoleAsync(_entity.ClientId, _entity.RoleId).Result;
        }

        private List<UserRoleMatchData> When_Invoke_GetUserRoles()
        {
            // be sure to have a role
            _repo.SaveUserRoleAsync(_entity).Wait();

            return _repo.GetUserRolesAsync(_entity.ClientId).Result;
        }

        private UserRoleMatchData When_Invoke_AssignUserRole()
        {
            return _repo.SaveUserRoleAsync(_entity).Result;
        }

        private void Then_Data_ShouldBeSaved()
        {
            var result = _repo.GetUserRoleAsync(_entity.ClientId, _entity.RoleId).Result;
            Assert.NotNull(result);
        }

        private static void Then_Result_ShouldNotBe_Null(List<UserRoleMatchData> data)
        {
            Assert.NotNull(data);
            Assert.NotZero(data.Count);
        }
    }
}
