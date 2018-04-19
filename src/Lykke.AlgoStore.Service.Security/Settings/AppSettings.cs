using JetBrains.Annotations;
using Lykke.AlgoStore.Service.Security.Settings.ServiceSettings;
using Lykke.AlgoStore.Service.Security.Settings.SlackNotifications;

namespace Lykke.AlgoStore.Service.Security.Settings
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class AppSettings
    {
        public SecuritySettings AlgoStoreSecurityService { get; set; }
        public SlackNotificationsSettings SlackNotifications { get; set; }
    }
}
