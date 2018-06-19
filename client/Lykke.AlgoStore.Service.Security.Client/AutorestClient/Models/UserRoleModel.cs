// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Lykke.Service.Security.Client.AutorestClient.Models
{
    using Newtonsoft.Json;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public partial class UserRoleModel
    {
        /// <summary>
        /// Initializes a new instance of the UserRoleModel class.
        /// </summary>
        public UserRoleModel()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the UserRoleModel class.
        /// </summary>
        public UserRoleModel(bool canBeDeleted, bool canBeModified, string id = default(string), string name = default(string), IList<UserPermissionModel> permissions = default(IList<UserPermissionModel>))
        {
            Id = id;
            Name = name;
            CanBeDeleted = canBeDeleted;
            CanBeModified = canBeModified;
            Permissions = permissions;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Id")]
        public string Id { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Name")]
        public string Name { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "CanBeDeleted")]
        public bool CanBeDeleted { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "CanBeModified")]
        public bool CanBeModified { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Permissions")]
        public IList<UserPermissionModel> Permissions { get; set; }

        /// <summary>
        /// Validate the object.
        /// </summary>
        /// <exception cref="Microsoft.Rest.ValidationException">
        /// Thrown if validation fails
        /// </exception>
        public virtual void Validate()
        {
        }
    }
}
