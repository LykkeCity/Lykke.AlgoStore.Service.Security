using JetBrains.Annotations;
using Lykke.AlgoStore.Service.Security.Settings.ServiceSettings;
using Lykke.AlgoStore.Service.Security.Settings.SlackNotifications;
using Lykke.Service.PersonalData.Settings;

namespace Lykke.AlgoStore.Service.Security.Settings
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class AppSettings
    {
        public SecuritySettings AlgoStoreSecurityService { get; set; }
        public SlackNotificationsSettings SlackNotifications { get; set; }
        public PersonalDataServiceClientSettings PersonalDataServiceClient { get; set; }
    }
}
