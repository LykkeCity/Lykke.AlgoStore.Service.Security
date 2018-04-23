using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Lykke.AlgoStore.Service.Security.Core.Domain;
using Lykke.AlgoStore.Service.Security.Core.Services;
using Lykke.AlgoStore.Service.Security.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Lykke.AlgoStore.Service.Security.Controllers
{
    [Route("api/v1/permissions")]
    public class AlgoStoreUserPermissionsController: Controller
    {
        private readonly IUserRolesService _userRolesService;
        private readonly IUserPermissionsService _permissionsService;

        public AlgoStoreUserPermissionsController(
            IUserRolesService userRolesService,
            IUserPermissionsService permissionsService)
        {
            _userRolesService = userRolesService;
            _permissionsService = permissionsService;
        }

        [HttpGet("getAll")]
        [SwaggerOperation("GetAllPermissions")]
        [ProducesResponseType(typeof(List<UserPermissionModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllPermissions()
        {
            var result = await _permissionsService.GetAllPermissionsAsync();

            return Ok(result);
        }

        [HttpGet("getById")]
        [SwaggerOperation("GetPermissionById")]
        [ProducesResponseType(typeof(UserPermissionModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetPermissionById(string permissionId)
        {
            var result = await _permissionsService.GetPermissionByIdAsync(permissionId);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpGet("getByRoleId")]
        [SwaggerOperation("GetPermissionsByRoleId")]
        [ProducesResponseType(typeof(List<UserPermissionModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetPermissionsByRoleId(string roleId)
        {          
            var result = await _permissionsService.GetPermissionsByRoleIdAsync(roleId);

            return Ok(result);
        }

        [HttpPost("savePermission")]
        [SwaggerOperation("SavePermission")]
        [ProducesResponseType(typeof(UserPermissionModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> SavePermission([FromBody] UserPermissionModel permission)
        {
            var data = AutoMapper.Mapper.Map<UserPermissionData>(permission);

            var result = await _permissionsService.SavePermissionAsync(data);

            return Ok(result);
        }

        [HttpPost("assignPermission")]
        [SwaggerOperation("AssignPermissionToRole")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> AssignPermissionToRole([FromBody] RolePermissionMatchModel role)
        {
            var data = AutoMapper.Mapper.Map<RolePermissionMatchData>(role);

            await _permissionsService.AssignPermissionToRoleAsync(data);

            return NoContent();
        }

        [HttpPost("assignPermissions")]
        [SwaggerOperation("AssignMultiplePermissionToRole")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> AssignMultiplePermissionToRole([FromBody] List<RolePermissionMatchModel> permissions)
        {
            var data = AutoMapper.Mapper.Map<List<RolePermissionMatchData>>(permissions);

            await _permissionsService.AssignPermissionsToRoleAsync(data);

            return NoContent();
        }

        [HttpPost("revokePermission")]
        [SwaggerOperation("RevokePermissionFromRole")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> RevokePermissionFromRole([FromBody] RolePermissionMatchModel role)
        {
            var data = AutoMapper.Mapper.Map<RolePermissionMatchData>(role);

            await _permissionsService.RevokePermissionFromRole(data);

            return NoContent();
        }

        [HttpPost("revokePermissions")]
        [SwaggerOperation("RevokeMultiplePermissions")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> RevokeMultiplePermissions([FromBody] List<RolePermissionMatchModel> role)
        {
            var data = AutoMapper.Mapper.Map<List<RolePermissionMatchData>>(role);

            await _permissionsService.RevokePermissionsFromRole(data);

            return NoContent();
        }

        [HttpDelete("deletePermission")]
        [SwaggerOperation("DeletePermission")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> DeletePermission(string permissionId)
        {
            await _permissionsService.DeletePermissionAsync(permissionId);

            return NoContent();
        }

    }
}
