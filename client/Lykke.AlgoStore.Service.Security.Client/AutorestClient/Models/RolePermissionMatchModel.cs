// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Lykke.Service.Security.Client.AutorestClient.Models
{
    using Newtonsoft.Json;
    using System.Linq;

    public partial class RolePermissionMatchModel
    {
        /// <summary>
        /// Initializes a new instance of the RolePermissionMatchModel class.
        /// </summary>
        public RolePermissionMatchModel()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the RolePermissionMatchModel class.
        /// </summary>
        public RolePermissionMatchModel(string permissionId = default(string), string roleId = default(string))
        {
            PermissionId = permissionId;
            RoleId = roleId;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "PermissionId")]
        public string PermissionId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "RoleId")]
        public string RoleId { get; set; }

    }
}
