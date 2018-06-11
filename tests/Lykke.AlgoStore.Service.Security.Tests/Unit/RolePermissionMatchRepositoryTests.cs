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
    public class RolePermissionMatchRepositoryTests
    {
        private readonly Fixture _fixture = new Fixture();

        private readonly Mock<INoSQLTableStorage<RolePermissionMatchEntity>> _storage =
            new Mock<INoSQLTableStorage<RolePermissionMatchEntity>>();

        private IRolePermissionMatchRepository _repository;

        private RolePermissionMatchData _rolePermissionMatchData;
        private RolePermissionMatchEntity _rolePermissionMatchEntity;
        private IEnumerable<RolePermissionMatchData> _rolePermissionMatchesData;
        private IEnumerable<RolePermissionMatchEntity> _rolePermissionMatchEntities;

        [SetUp]
        public void SetUp()
        {
            //Reset should not be used in production code. It is intended to support testing scenarios only.
            Mapper.Reset();

            Mapper.Initialize(cfg => cfg.AddProfile<AzureRepositories.AutoMapperProfile>());
            Mapper.AssertConfigurationIsValid();

            _rolePermissionMatchEntity = _fixture.Build<RolePermissionMatchEntity>().Create();
            _rolePermissionMatchData = Mapper.Map<RolePermissionMatchData>(_rolePermissionMatchEntity);

            _rolePermissionMatchEntities = _fixture.Build<RolePermissionMatchEntity>().CreateMany();
            _rolePermissionMatchesData = Mapper.Map<List<RolePermissionMatchData>>(_rolePermissionMatchEntities);

            _storage.Setup(x => x.GetDataAsync(It.IsAny<string>(), It.IsAny<Func<RolePermissionMatchEntity, bool>>()))
                .Returns((string partitionKey, Func<RolePermissionMatchEntity, bool> filter) =>
                    Task.FromResult(_rolePermissionMatchEntities));

            _storage.Setup(x => x.InsertOrReplaceAsync(_rolePermissionMatchEntity))
                .Returns(Task.FromResult(_rolePermissionMatchEntity));

            _storage.Setup(x => x.DeleteAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(_rolePermissionMatchEntity));

            _repository = new RolePermissionMatchRepository(_storage.Object);
        }

        [Test]
        public void GetPermissionsByRoleIdTest()
        {
            var result = _repository.GetPermissionIdsByRoleIdAsync(_rolePermissionMatchesData.First().RoleId).Result;

            result.Should().BeEquivalentTo(_rolePermissionMatchesData);
        }

        [Test]
        public void AssignPermissionToRoleTest()
        {
            var result = _repository.AssignPermissionToRoleAsync(_rolePermissionMatchData).Result;

            result.Should().BeEquivalentTo(_rolePermissionMatchData);
        }

        [Test]
        public void RevokePermissionFromRoleTest()
        {
            _repository.RevokePermission(_rolePermissionMatchData).Wait();
        }
    }
}
