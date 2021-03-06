﻿using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Common.Log;
using Lykke.AlgoStore.Service.Security.Core.Domain;
using Lykke.AlgoStore.Service.Security.Core.Services;
using Lykke.AlgoStore.Service.Security.Core.Utils;
using Lykke.AlgoStore.Service.Security.Models;
using Lykke.AlgoStore.Service.Security.Services.Strings;
using Lykke.Common.Api.Contract.Responses;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Lykke.AlgoStore.Service.Security.Controllers
{
    [Route("api/v1/roles")]
    public class AlgoStoreUserRolesController: Controller
    {
        private readonly IUserRolesService _userRolesService;
        private readonly ILog _log;

        public AlgoStoreUserRolesController(IUserRolesService userRolesService, ILog log)
        {
            _userRolesService = userRolesService;
            _log = log;
        }

        [HttpGet("getAll")]
        [SwaggerOperation("GetAllUserRoles")]
        [ProducesResponseType(typeof(List<UserRoleData>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllUserRoles()
        {
            var result =
                await _log.LogElapsedTimeAsync(null, async () => await _userRolesService.GetAllRolesAsync());

            return Ok(result);
        }

        [HttpGet("getById")]
        [SwaggerOperation("GetRoleById")]
        [ProducesResponseType(typeof(UserRoleData), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetRoleById(string roleId)
        {
            var result =
                await _log.LogElapsedTimeAsync(null, async () => await _userRolesService.GetRoleByIdAsync(roleId));

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpGet("getByClientId")]
        [SwaggerOperation("GetRolesByClientId")]
        [ProducesResponseType(typeof(List<UserRoleData>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetRolesByClientId(string clientId)
        {
            if (string.IsNullOrEmpty(clientId))
                return BadRequest(ErrorResponse.Create(Phrases.ClientIdEmpty));

            var result =
                await _log.LogElapsedTimeAsync(null, async () => await _userRolesService.GetRolesByClientIdAsync(clientId));

            return Ok(result);
        }

        [HttpPost("saveRole")]
        [SwaggerOperation("SaveUserRole")]
        [ProducesResponseType(typeof(UserRoleData), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> SaveUserRole([FromBody] UserRoleModel role)
        {
            var data = AutoMapper.Mapper.Map<UserRoleData>(role);

            var result =
                await _log.LogElapsedTimeAsync(null, async () => await _userRolesService.SaveRoleAsync(data));

            return Ok(result);
        }

        [HttpPost("updateRole")]
        [SwaggerOperation("UpdateUserRole")]
        [ProducesResponseType(typeof(UserRoleData), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateUserRole([FromBody] UserRoleUpdateModel role)
        {
            var data = AutoMapper.Mapper.Map<UserRoleData>(role);

            var result =
                await _log.LogElapsedTimeAsync(null, async () => await _userRolesService.SaveRoleAsync(data));

            return Ok(result);
        }

        [HttpPost("assignRole")]
        [SwaggerOperation("AssignUserRole")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> AssignUserRole([FromBody] UserRoleMatchModel role)
        {
            var data = AutoMapper.Mapper.Map<UserRoleMatchData>(role);

            await _log.LogElapsedTimeAsync(null, async () => await _userRolesService.AssignRoleToUserAsync(data));

            return NoContent();
        }

        [HttpPost("revokeRole")]
        [SwaggerOperation("RevokeRoleFromUser")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> RevokeRoleFromUser([FromBody] UserRoleMatchModel role)
        {
            var data = AutoMapper.Mapper.Map<UserRoleMatchData>(role);

            await _log.LogElapsedTimeAsync(null, async () => await _userRolesService.RevokeRoleFromUserAsync(data));

            return NoContent();
        }

        [HttpGet("verifyRole")]
        [SwaggerOperation("VerifyUserRole")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> VerifyUserRole(string clientId)
        {
            if (string.IsNullOrEmpty(clientId))
                return BadRequest(ErrorResponse.Create(Phrases.ClientIdEmpty));

            await _log.LogElapsedTimeAsync(null, async () => await _userRolesService.VerifyUserRoleAsync(clientId));

            return Ok();
        }

        [HttpDelete("deleteRole")]
        [SwaggerOperation("DeleteUserRole")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> DeleteUserRole(string roleId)
        {
            await _log.LogElapsedTimeAsync(null, async () => await _userRolesService.DeleteRoleAsync(roleId));

            return NoContent();
        }

        [HttpPost("seedRoles")]
        [SwaggerOperation("SeedRoles")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> SeedRoles([FromBody] List<UserPermissionData> permissions)
        {
            await _userRolesService.SeedRolesAsync(permissions);

            return Ok();
        }
    }
}
