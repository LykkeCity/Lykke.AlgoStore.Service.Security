using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Lykke.AlgoStore.Service.Security.Core.Domain;
using Lykke.AlgoStore.Service.Security.Core.Services;
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

        public AlgoStoreUserRolesController(
            IUserRolesService userRolesService)
        {
            _userRolesService = userRolesService;
        }

        [HttpGet("getAll")]
        [SwaggerOperation("GetAllUserRoles")]
        [ProducesResponseType(typeof(List<UserRoleModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllUserRoles()
        {
            var result = await _userRolesService.GetAllRolesAsync();
            return Ok(result);
        }

        [HttpGet("getById")]
        [SwaggerOperation("GetRoleById")]
        [ProducesResponseType(typeof(UserRoleModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetRoleById(string roleId)
        {
            var result = await _userRolesService.GetRoleByIdAsync(roleId);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpGet("getByClientId")]
        [SwaggerOperation("GetRolesByClientId")]
        [ProducesResponseType(typeof(List<UserRoleModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetRolesByClientId(string clientId)
        {
            if (string.IsNullOrEmpty(clientId))
                return BadRequest(ErrorResponse.Create(Phrases.ClientIdEmpty));

            var result = await _userRolesService.GetRolesByClientIdAsync(clientId);

            return Ok(result);
        }

        [HttpPost("saveRole")]
        [SwaggerOperation("SaveUserRole")]
        [ProducesResponseType(typeof(UserRoleModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> SaveUserRole([FromBody] UserRoleModel role)
        {
            var data = AutoMapper.Mapper.Map<UserRoleData>(role);

            var result = await _userRolesService.SaveRoleAsync(data);

            return Ok(result);
        }

        [HttpPost("updateRole")]
        [SwaggerOperation("UpdateUserRole")]
        [ProducesResponseType(typeof(UserRoleModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateUserRole([FromBody] UserRoleUpdateModel role)
        {
            var data = AutoMapper.Mapper.Map<UserRoleData>(role);

            var result = await _userRolesService.SaveRoleAsync(data);

            return Ok(result);
        }

        [HttpPost("assignRole")]
        [SwaggerOperation("AssignUserRole")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> AssignUserRole([FromBody] UserRoleMatchModel role)
        {
            var data = AutoMapper.Mapper.Map<UserRoleMatchData>(role);

            await _userRolesService.AssignRoleToUser(data);

            return NoContent();
        }

        [HttpPost("revokeRole")]
        [SwaggerOperation("RevokeRoleFromUser")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> RevokeRoleFromUser([FromBody] UserRoleMatchModel role)
        {
            var data = AutoMapper.Mapper.Map<UserRoleMatchData>(role);

            await _userRolesService.RevokeRoleFromUser(data);

            return NoContent();
        }

        [HttpGet("verifyRole")]
        [SwaggerOperation("VerifyUserRole")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> VerifyUserRole(string clientId)
        {
            if (string.IsNullOrEmpty(clientId))
                return BadRequest(ErrorResponse.Create(Phrases.ClientIdEmpty));

            await _userRolesService.VerifyUserRole(clientId);

            return Ok();
        }

        [HttpDelete("deleteRole")]
        [SwaggerOperation("DeleteUserRole")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> DeleteUserRole(string roleId)
        {
            await _userRolesService.DeleteRoleAsync(roleId);

            return NoContent();
        }
    }
}
