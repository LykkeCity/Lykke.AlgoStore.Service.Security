using Autofac;
using Autofac.Extensions.DependencyInjection;
using Common.Log;
using Lykke.AlgoStore.Service.Security.Core.Services;
using Lykke.AlgoStore.Service.Security.Services;
using Lykke.AlgoStore.Service.Security.Settings.ServiceSettings;
using Lykke.SettingsReader;
using Microsoft.Extensions.DependencyInjection;

namespace Lykke.AlgoStore.Service.Security.Modules
{
    public class ServiceModule : Module
    {
        private readonly IReloadingManager<SecuritySettings> _settings;
        private readonly ILog _log;
        // NOTE: you can remove it if you don't need to use IServiceCollection extensions to register service specific dependencies
        private readonly IServiceCollection _services;

        public ServiceModule(IReloadingManager<SecuritySettings> settings, ILog log)
        {
            _settings = settings;
            _log = log;

            _services = new ServiceCollection();
        }

        protected override void Load(ContainerBuilder builder)
        {
            // TODO: Do not register entire settings in container, pass necessary settings to services which requires them
            // ex:
            //  builder.RegisterType<QuotesPublisher>()
            //      .As<IQuotesPublisher>()
            //      .WithParameter(TypedParameter.From(_settings.CurrentValue.QuotesPublication))

            builder.RegisterInstance(_log)
                .As<ILog>()
                .SingleInstance();

            builder.RegisterType<HealthService>()
                .As<IHealthService>()
                .SingleInstance();

            builder.RegisterType<StartupManager>()
                .As<IStartupManager>();

            builder.RegisterType<ShutdownManager>()
                .As<IShutdownManager>();

            builder.RegisterType<UserRolesService>()
                .As<IUserRolesService>()
                .SingleInstance();

            builder.RegisterType<UserPermissionsService>()
                .As<IUserPermissionsService>()
                .SingleInstance();

            builder.Populate(_services);
        }
    }
}
