// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Lykke.Service.Security.Client.AutorestClient.Models
{
    using Newtonsoft.Json;
    using System.Linq;

    public partial class UserPermissionModel
    {
        /// <summary>
        /// Initializes a new instance of the UserPermissionModel class.
        /// </summary>
        public UserPermissionModel()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the UserPermissionModel class.
        /// </summary>
        public UserPermissionModel(string id = default(string), string name = default(string), string displayName = default(string))
        {
            Id = id;
            Name = name;
            DisplayName = displayName;
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
        [JsonProperty(PropertyName = "DisplayName")]
        public string DisplayName { get; set; }

    }
}
