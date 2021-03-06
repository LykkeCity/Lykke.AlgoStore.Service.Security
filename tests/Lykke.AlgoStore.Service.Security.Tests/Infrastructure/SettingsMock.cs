﻿using System.Threading.Tasks;
using Lykke.SettingsReader;
using Moq;

namespace Lykke.AlgoStore.Service.Security.Tests.Infrastructure
{
    public static class SettingsMock
    {
        private const string LocalStorageConnectionString = "UseDevelopmentStorage=true";

        public static IReloadingManager<string> GetTableStorageConnectionString()
        {
            Task<string> currentTask = null;

            var mock = new Mock<IReloadingManager<string>>();

            mock.Setup(x => x.Reload())
                .Returns(() => currentTask = Task.FromResult(LocalStorageConnectionString));

            mock.Setup(x => x.CurrentValue)
                .Returns(() => (currentTask ?? mock.Object.Reload()).Result);

            return mock.Object;
        }

        public static IReloadingManager<string> GetLogsConnectionString()
        {
            return GetTableStorageConnectionString();
        }
    }
}
