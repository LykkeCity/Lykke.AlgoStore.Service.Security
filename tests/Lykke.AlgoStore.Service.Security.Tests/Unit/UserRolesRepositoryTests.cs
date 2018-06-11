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
    public class UserRolesRepositoryTests
    {
        private readonly Fixture _fixture = new Fixture();

        private readonly Mock<INoSQLTableStorage<UserRoleEntity>> _storage =
            new Mock<INoSQLTableStorage<UserRoleEntity>>();

        private IUserRolesRepository _repository;

        private UserRoleEntity _roleEntity;
        private UserRoleData _roleData;
        private IEnumerable<UserRoleEntity> _roleEntities;
        private IEnumerable<UserRoleData> _rolesData;

        [SetUp]
        public void SetUp()
        {
            //Reset should not be used in production code. It is intended to support testing scenarios only.
            Mapper.Reset();

            Mapper.Initialize(cfg => cfg.AddProfile<AzureRepositories.AutoMapperProfile>());
            Mapper.AssertConfigurationIsValid();

            _roleEntity = _fixture.Build<UserRoleEntity>().Create();
            _roleData = Mapper.Map<UserRoleData>(_roleEntity);

            _roleEntities = _fixture.Build<UserRoleEntity>().CreateMany();
            _rolesData = Mapper.Map<List<UserRoleData>>(_roleEntities);

            _storage.Setup(x => x.InsertOrReplaceAsync(_roleEntity))
                .Returns(Task.FromResult(_roleEntity));

            _storage.Setup(x => x.GetDataAsync(null))
                .Returns(() =>
                {
                    IList<UserRoleEntity> roles = new List<UserRoleEntity>();
                    ((List<UserRoleEntity>)roles).AddRange(_roleEntities);

                    return Task.FromResult(roles);
                });

            _storage.Setup(x => x.GetDataAsync(It.IsAny<string>(), It.IsAny<Func<UserRoleEntity, bool>>()))
                .Returns((string partitionKey, Func<UserRoleEntity, bool> filter) => Task.FromResult(_roleEntities));

            _storage.Setup(x => x.DeleteIfExistAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(true));

            _storage.Setup(x => x.GetDataAsync(It.IsAny<Func<UserRoleEntity, bool>>()))
                .Returns((Func<UserRoleEntity, bool> filter) => {
                    IList<UserRoleEntity> roles = new List<UserRoleEntity>();
                    ((List<UserRoleEntity>)roles).AddRange(_roleEntities);

                    return Task.FromResult(roles);
                });

            _repository = new UserRolesRepository(_storage.Object);
        }

        [Test]
        public void CreateRoleTest()
        {
            var result = _repository.SaveRoleAsync(_roleData).Result;

            result.Should().BeEquivalentTo(_roleData);
        }

        [Test]
        public void GetAllRolesTest()
        {
            var result = _repository.GetAllRolesAsync().Result;

            result.Should().BeEquivalentTo(_rolesData);
        }

        [Test]
        public void GetByIdTest()
        {
            var result = _repository.GetRoleByIdAsync(_rolesData.First().Id).Result;

            result.Should().BeEquivalentTo(_rolesData.First());
        }

        [Test]
        public void DeleteRoleTest()
        {
            _repository.DeleteRoleAsync(_roleData).Wait();
        }

        [Test]
        public void RoleExistsTest()
        {
            var result = _repository.RoleExistsAsync(_roleData.Id).Result;

            Assert.IsTrue(result);
        }
    }
}
