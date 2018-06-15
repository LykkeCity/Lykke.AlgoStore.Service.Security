using JetBrains.Annotations;

namespace Lykke.AlgoStore.Service.Security.Settings.SlackNotifications
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class SlackNotificationsSettings
    {
        public AzureQueuePublicationSettings AzureQueue { get; set; }
    }
}
