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
    public class AlgoStoreUserPermissionsController : Controller
    {
        private readonly IUserPermissionsService _permissionsService;

        public AlgoStoreUserPermissionsController(IUserPermissionsService permissionsService)
        {
            _permissionsService = permissionsService;
        }

        [HttpGet("getAll")]
        [SwaggerOperation("GetAllPermissions")]
        [ProducesResponseType(typeof(List<UserPermissionModel>), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllPermissions()
        {
            var result = await _permissionsService.GetAllPermissionsAsync();

            return Ok(result);
        }

        [HttpGet("getById")]
        [SwaggerOperation("GetPermissionById")]
        [ProducesResponseType(typeof(UserPermissionModel), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetPermissionById(string permissionId)
        {
            var result = await _permissionsService.GetPermissionByIdAsync(permissionId);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpGet("getByRoleId")]
        [SwaggerOperation("GetPermissionsByRoleId")]
        [ProducesResponseType(typeof(List<UserPermissionModel>), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> GetPermissionsByRoleId(string roleId)
        {
            var result = await _permissionsService.GetPermissionsByRoleIdAsync(roleId);

            return Ok(result);
        }

        [HttpPost("assignPermissions")]
        [SwaggerOperation("AssignMultiplePermissionToRole")]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        public async Task<IActionResult> AssignMultiplePermissionToRole(
            [FromBody] List<RolePermissionMatchModel> permissions)
        {
            var data = AutoMapper.Mapper.Map<List<RolePermissionMatchData>>(permissions);

            await _permissionsService.AssignPermissionsToRoleAsync(data);

            return NoContent();
        }

        [HttpPost("revokePermissions")]
        [SwaggerOperation("RevokeMultiplePermissions")]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        public async Task<IActionResult> RevokeMultiplePermissions([FromBody] List<RolePermissionMatchModel> role)
        {
            var data = AutoMapper.Mapper.Map<List<RolePermissionMatchData>>(role);

            await _permissionsService.RevokePermissionsFromRole(data);

            return NoContent();
        }

        [HttpGet("hasPermission")]
        [SwaggerOperation("HasPermission")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> HasPermission(string clientId, string permissionId)
        {
            var result = await _permissionsService.HasPermission(clientId, permissionId);

            return Ok(result);
        }
    }
}
