namespace Lykke.AlgoStore.Service.Security.Models
{
    public class UserRoleUpdateModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool CanBeModified { get; set; }
        public bool CanBeDeleted { get; set; }
    }
}
