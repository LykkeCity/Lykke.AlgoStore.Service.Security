using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using AutoMapper;
using AzureStorage;
using FluentAssertions;
using Lykke.AlgoStore.Service.Security.AzureRepositories.Entities;
using Lykke.AlgoStore.Service.Security.AzureRepositories.Repositories;
using Lykke.AlgoStore.Service.Security.Core.Domain;
using Lykke.AlgoStore.Service.Security.Core.Repositories;
using Moq;
using NUnit.Framework;

namespace Lykke.AlgoStore.Service.Security.Tests.Unit
{
    [TestFixture]
    public class UserPermissionsRepositoryTests
    {
        private readonly Fixture _fixture = new Fixture();

        private UserPermissionEntity _permissionEntity;
        private UserPermissionData _permissionData;
        private IEnumerable<UserPermissionEntity> _permissionEntitites;
        private IEnumerable<UserPermissionData> _permissionsData;

        private readonly Mock<INoSQLTableStorage<UserPermissionEntity>> _storage =
            new Mock<INoSQLTableStorage<UserPermissionEntity>>();

        private IUserPermissionsRepository _repository;

        [SetUp]
        public void SetUp()
        {
            //Reset should not be used in production code. It is intended to support testing scenarios only.
            Mapper.Reset();

            Mapper.Initialize(cfg => cfg.AddProfile<AzureRepositories.AutoMapperProfile>());
            Mapper.AssertConfigurationIsValid();

            _permissionEntity = _fixture.Build<UserPermissionEntity>().Create();
            _permissionData = Mapper.Map<UserPermissionData>(_permissionEntity);

            _permissionEntitites = _fixture.Build<UserPermissionEntity>().CreateMany();
            _permissionsData = Mapper.Map<IEnumerable<UserPermissionData>>(_permissionEntitites);

            _storage.Setup(x => x.InsertOrReplaceAsync(_permissionEntity))
                .Returns(Task.FromResult(_permissionEntity));

            _storage.Setup(x => x.GetDataAsync(null))
                .Returns(() =>
                {
                    IList<UserPermissionEntity> permissions = new List<UserPermissionEntity>();
                    ((List<UserPermissionEntity>)permissions).AddRange(_permissionEntitites);

                    return Task.FromResult(permissions);
                });

            _storage.Setup(x => x.GetDataAsync(It.IsAny<string>(), It.IsAny<Func<UserPermissionEntity, bool>>()))
                .Returns((string partitionKey, Func<UserPermissionEntity, bool> filter) =>
                    Task.FromResult(_permissionEntitites));

            _storage.Setup(x => x.DeleteIfExistAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(true));

            _repository = new UserPermissionsRepository(_storage.Object);
        }

        [Test]
        public void SavePermissionTest()
        {
            var result = _repository.SavePermissionAsync(_permissionData).Result;

            result.Should().BeEquivalentTo(_permissionData);
        }

        [Test]
        public void GetPermissionByIdTest()
        {
            var result = _repository.GetPermissionByIdAsync(_permissionsData.First().Id).Result;

            result.Should().BeEquivalentTo(_permissionsData.First());
        }

        [Test]
        public void GetAllPermissionsTest()
        {
            var result = _repository.GetAllPermissionsAsync().Result;

            result.Should().Equal(_permissionsData,
                (x1, x2) => x1.Id == x2.Id && x1.Name == x2.Name && x1.DisplayName == x2.DisplayName);
        }

        [Test]
        public void DeletePermissionTest()
        {
            _repository.DeletePermissionAsync(_permissionData).Wait();
        }
    }
}
