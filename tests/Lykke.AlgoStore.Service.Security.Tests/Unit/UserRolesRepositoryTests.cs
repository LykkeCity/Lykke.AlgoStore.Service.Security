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
        }

        [TearDown]
        public void CleanUp()
        {
        }

        [Test]
        public void CreateRoleTest()
        {
            _storage.Setup(x => x.InsertOrReplaceAsync(_roleEntity))
                .Returns(Task.FromResult(_roleEntity));

            var repo = new UserRolesRepository(_storage.Object);

            var result = repo.SaveRoleAsync(_roleData).Result;

            result.Should().BeEquivalentTo(_roleData);
        }

        [Test]
        public void GetAllRolesTest()
        {
            _storage.Setup(x => x.GetDataAsync(null))
                .Returns(() =>
                {
                    IList<UserRoleEntity> roles = new List<UserRoleEntity>();
                    ((List<UserRoleEntity>) roles).AddRange(_roleEntities);

                    return Task.FromResult(roles);
                });

            var repo = new UserRolesRepository(_storage.Object);

            var result = repo.GetAllRolesAsync().Result;

            //REMARK: For some reason must exclude permissions property from comparison 
            result.Should().Equal(_rolesData,
                (x1, x2) => x1.CanBeDeleted == x2.CanBeDeleted && x1.CanBeModified == x2.CanBeModified &&
                            x1.Id == x2.Id && x1.Name == x2.Name);
        }

        [Test]
        public void GetByIdTest()
        {
            _storage.Setup(x => x.GetDataAsync(It.IsAny<string>(), It.IsAny<Func<UserRoleEntity, bool>>()))
                .Returns((string partitionKey, Func<UserRoleEntity, bool> filter) => Task.FromResult(_roleEntities));

            var repo = new UserRolesRepository(_storage.Object);

            var result = repo.GetRoleByIdAsync(_rolesData.First().Id).Result;

            result.Should().BeEquivalentTo(_rolesData.First());
        }

        [Test]
        public void DeleteRoleTest()
        {
            _storage.Setup(x => x.DeleteIfExistAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(true));

            var repo = new UserRolesRepository(_storage.Object);

            repo.DeleteRoleAsync(_roleData).Wait();
        }

        [Test]
        public void RoleExistsTest()
        {
            _storage.Setup(x => x.GetDataAsync(It.IsAny<Func<UserRoleEntity, bool>>()))
                .Returns((Func<UserRoleEntity, bool> filter) => {
                    IList<UserRoleEntity> roles = new List<UserRoleEntity>();
                    ((List<UserRoleEntity>)roles).AddRange(_roleEntities);

                    return Task.FromResult(roles);
                });

            var repo = new UserRolesRepository(_storage.Object);

            var result = repo.RoleExistsAsync(_roleData.Id).Result;

            Assert.IsTrue(result);
        }
    }
}
