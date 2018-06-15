using JetBrains.Annotations;

namespace Lykke.AlgoStore.Service.Security.Settings.ServiceSettings
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class SecuritySettings
    {
        public DbSettings Db { get; set; }
    }
}
