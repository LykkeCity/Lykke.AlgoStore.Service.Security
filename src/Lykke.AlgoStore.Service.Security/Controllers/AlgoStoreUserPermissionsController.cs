using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Common.Log;
using Lykke.AlgoStore.Service.Security.Core.Domain;
using Lykke.AlgoStore.Service.Security.Core.Services;
using Lykke.AlgoStore.Service.Security.Core.Utils;
using Lykke.AlgoStore.Service.Security.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Lykke.AlgoStore.Service.Security.Controllers
{
    [Route("api/v1/permissions")]
    public class AlgoStoreUserPermissionsController : Controller
    {
        private readonly IUserPermissionsService _permissionsService;
        private readonly ILog _log;

        public AlgoStoreUserPermissionsController(IUserPermissionsService permissionsService, ILog log)
        {
            _permissionsService = permissionsService;
            _log = log;
        }

        [HttpGet("getAll")]
        [SwaggerOperation("GetAllPermissions")]
        [ProducesResponseType(typeof(List<UserPermissionData>), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllPermissions()
        {
            var result =
                await _log.LogElapsedTimeAsync(null, async () => await _permissionsService.GetAllPermissionsAsync());

            return Ok(result);
        }

        [HttpGet("getById")]
        [SwaggerOperation("GetPermissionById")]
        [ProducesResponseType(typeof(UserPermissionData), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetPermissionById(string permissionId)
        {
            var result = 
                await _log.LogElapsedTimeAsync(null, async () => await _permissionsService.GetPermissionByIdAsync(permissionId));

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpGet("getByRoleId")]
        [SwaggerOperation("GetPermissionsByRoleId")]
        [ProducesResponseType(typeof(List<UserPermissionData>), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> GetPermissionsByRoleId(string roleId)
        {
            var result =
                await _log.LogElapsedTimeAsync(null, async () => await _permissionsService.GetPermissionsByRoleIdAsync(roleId));

            return Ok(result);
        }

        [HttpPost("assignPermissions")]
        [SwaggerOperation("AssignMultiplePermissionToRole")]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        public async Task<IActionResult> AssignMultiplePermissionToRole(
            [FromBody] List<RolePermissionMatchModel> permissions)
        {
            var data = AutoMapper.Mapper.Map<List<RolePermissionMatchData>>(permissions);

            await _log.LogElapsedTimeAsync(null, async () => await _permissionsService.AssignPermissionsToRoleAsync(data));

            return NoContent();
        }

        [HttpPost("revokePermissions")]
        [SwaggerOperation("RevokeMultiplePermissions")]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        public async Task<IActionResult> RevokeMultiplePermissions([FromBody] List<RolePermissionMatchModel> role)
        {
            var data = AutoMapper.Mapper.Map<List<RolePermissionMatchData>>(role);

            await _log.LogElapsedTimeAsync(null, async () => await _permissionsService.RevokePermissionsFromRoleAsync(data));

            return NoContent();
        }

        [HttpGet("hasPermission")]
        [SwaggerOperation("HasPermission")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> HasPermission(string clientId, string permissionId)
        {
            var result =
                await _log.LogElapsedTimeAsync(null, async () => await _permissionsService.HasPermissionAsync(clientId, permissionId));

            return Ok(result);
        }

        [HttpPost("seedPermissions")]
        [SwaggerOperation("SeedPermissions")]
        [ProducesResponseType(typeof(bool), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> SeedPermissions([FromBody] List<UserPermissionData> permissions)
        {
            await _permissionsService.SeedPermissionsAsync(permissions);

            return Ok();
        }
    }
}
