using Autofac;
using Autofac.Extensions.DependencyInjection;
using AzureStorage.Tables;
using Common.Log;
using Lykke.AlgoStore.Service.Security.AzureRepositories.Entities;
using Lykke.AlgoStore.Service.Security.AzureRepositories.Repositories;
using Lykke.AlgoStore.Service.Security.Core.Repositories;
using Lykke.AlgoStore.Service.Security.Core.Services;
using Lykke.AlgoStore.Service.Security.Services;
using Lykke.AlgoStore.Service.Security.Settings;
using Lykke.Service.PersonalData.Client;
using Lykke.Service.PersonalData.Contract;
using Lykke.SettingsReader;
using Microsoft.Extensions.DependencyInjection;

namespace Lykke.AlgoStore.Service.Security.Modules
{
    public class ServiceModule : Module
    {
        private readonly IReloadingManager<AppSettings> _settings;
        private readonly ILog _log;
        // NOTE: you can remove it if you don't need to use IServiceCollection extensions to register service specific dependencies
        private readonly IServiceCollection _services;

        public ServiceModule(IReloadingManager<AppSettings> settings, ILog log)
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

            builder.RegisterInstance(new PersonalDataService(_settings.CurrentValue.PersonalDataServiceClient, null))
                .As<IPersonalDataService>()
                .SingleInstance();

            // Custom dependencies here
            var reloadingDbManager = _settings.ConnectionString(x => x.AlgoStoreSecurityService.Db.DataStorageConnectionString);

            builder.RegisterInstance(AzureTableStorage<UserRoleEntity>.Create(reloadingDbManager, UserRolesRepository.TableName, _log));
            builder.RegisterInstance(AzureTableStorage<UserPermissionEntity>.Create(reloadingDbManager, UserPermissionsRepository.TableName, _log));
            builder.RegisterInstance(AzureTableStorage<UserRoleMatchEntity>.Create(reloadingDbManager, UserRolesMatchRepository.TableName, _log));
            builder.RegisterInstance(AzureTableStorage<RolePermissionMatchEntity>.Create(reloadingDbManager, RolePermissionMatchRepository.TableName, _log));

            builder.RegisterType<UserRolesRepository>().As<IUserRolesRepository>();
            builder.RegisterType<UserPermissionsRepository>().As<IUserPermissionsRepository>();
            builder.RegisterType<UserRolesMatchRepository>().As<IUserRoleMatchRepository>();
            builder.RegisterType<RolePermissionMatchRepository>().As<IRolePermissionMatchRepository>();

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
