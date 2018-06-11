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
    public class UserRolesMatchRepositoryTests
    {
        private readonly Fixture _fixture = new Fixture();

        private readonly Mock<INoSQLTableStorage<UserRoleMatchEntity>> _storage =
            new Mock<INoSQLTableStorage<UserRoleMatchEntity>>();

        private IUserRoleMatchRepository _repository;

        private UserRoleMatchEntity _roleMatchEntity;
        private UserRoleMatchData _roleMatchData;
        private IEnumerable<UserRoleMatchEntity> _roleMatchEntities;
        private IEnumerable<UserRoleMatchData> _roleMatchesData;

        [SetUp]
        public void SetUp()
        {
            //Reset should not be used in production code. It is intended to support testing scenarios only.
            Mapper.Reset();

            Mapper.Initialize(cfg => cfg.AddProfile<AzureRepositories.AutoMapperProfile>());
            Mapper.AssertConfigurationIsValid();

            _roleMatchEntity = _fixture.Build<UserRoleMatchEntity>().Create();
            _roleMatchData = Mapper.Map<UserRoleMatchData>(_roleMatchEntity);

            _roleMatchEntities = _fixture.Build<UserRoleMatchEntity>().CreateMany();
            _roleMatchesData = Mapper.Map<List<UserRoleMatchData>>(_roleMatchEntities);

            _storage.Setup(x => x.GetDataAsync(null))
                .Returns(() =>
                {
                    IList<UserRoleMatchEntity> roleMatchesEntitites = new List<UserRoleMatchEntity>();
                    ((List<UserRoleMatchEntity>)roleMatchesEntitites).AddRange(_roleMatchEntities);

                    return Task.FromResult(roleMatchesEntitites);
                });

            _storage.Setup(x => x.InsertOrReplaceAsync(_roleMatchEntity))
                .Returns(Task.FromResult(_roleMatchEntity));

            _storage.Setup(x => x.GetDataAsync(It.IsAny<string>(), It.IsAny<Func<UserRoleMatchEntity, bool>>()))
                .Returns((string partitionKey, Func<UserRoleEntity, bool> filter) => Task.FromResult(_roleMatchEntities));

            _storage.Setup(x => x.GetDataAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(_roleMatchEntities.First()));

            _storage.Setup(x => x.DeleteIfExistAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(true));

            _repository = new UserRolesMatchRepository(_storage.Object);
        }

        [Test]
        public void GetAllMatchesTest()
        {
            var result = _repository.GetAllMatchesAsync().Result;

            result.Should().BeEquivalentTo(_roleMatchesData);
        }

        [Test]
        public void SaveUserRoleTest()
        {
            var result = _repository.SaveUserRoleAsync(_roleMatchData).Result;

            result.Should().BeEquivalentTo(_roleMatchData);
        }

        [Test]
        public void GetUserRolesTest()
        {
            var result = _repository.GetUserRolesAsync(_roleMatchesData.First().ClientId).Result;

            result.Should().BeEquivalentTo(_roleMatchesData);
        }

        [Test]
        public void GetUserRoleTest()
        {
            var result = _repository
                .GetUserRoleAsync(_roleMatchesData.First().ClientId, _roleMatchesData.First().RoleId).Result;
            
            result.Should().BeEquivalentTo(_roleMatchesData.First());
        }

        [Test]
        public void RevokeUserRoleTest()
        {
            _repository.RevokeUserRoleAsync(_roleMatchesData.First().ClientId, _roleMatchesData.First().RoleId).Wait();
        }
    }
}
