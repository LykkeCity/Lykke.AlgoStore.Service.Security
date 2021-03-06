﻿using System.Collections.Generic;

namespace Lykke.AlgoStore.Service.Security.Models
{
    public class UserRoleModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool CanBeDeleted { get; set; }
        public bool CanBeModified { get; set; }
        public List<UserPermissionModel> Permissions { get; set; }
    }
}
