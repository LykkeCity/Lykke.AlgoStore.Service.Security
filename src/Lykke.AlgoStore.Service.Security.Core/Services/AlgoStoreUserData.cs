using System.Collections.Generic;
using Lykke.AlgoStore.Service.Security.Core.Domain;

namespace Lykke.AlgoStore.Service.Security.Core.Services
{
    public class AlgoStoreUserData
    {
        public string ClientId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public List<UserRoleData> Roles { get; set; }
    }
}
