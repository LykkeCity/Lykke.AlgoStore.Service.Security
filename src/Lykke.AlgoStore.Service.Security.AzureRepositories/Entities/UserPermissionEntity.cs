using Microsoft.WindowsAzure.Storage.Table;

namespace Lykke.AlgoStore.Service.Security.AzureRepositories.Entities
{
    public class UserPermissionEntity: TableEntity
    {
        public string DisplayName { get; set; }
        public string Description { get; set; }
    }
}
