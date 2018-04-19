﻿using JetBrains.Annotations;
using Lykke.SettingsReader.Attributes;

namespace Lykke.AlgoStore.Service.Security.Settings.SlackNotifications
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class AzureQueuePublicationSettings
    {
        [AzureQueueCheck]
        public string ConnectionString { get; set; }

        public string QueueName { get; set; }
    }
}
