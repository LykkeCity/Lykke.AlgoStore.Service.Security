using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Common.Log;
using Lykke.AlgoStore.Service.Security.Core.Domain;
using Lykke.AlgoStore.Service.Security.Core.Services;
using Lykke.AlgoStore.Service.Security.Core.Utils;
using Lykke.AlgoStore.Service.Security.Services.Strings;
using Lykke.Common.Api.Contract.Responses;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Lykke.AlgoStore.Service.Security.Controllers
{
    [Route("api/v1/users")]
    public class AlgoStoreUsersController: Controller
    {
        private readonly IUserRolesService _userRolesService;
        private readonly ILog _log;

        public AlgoStoreUsersController(IUserRolesService userRolesService, ILog log)
        {
            _userRolesService = userRolesService;
            _log = log;
        }

        [HttpGet("getAllWithRoles")]
        [SwaggerOperation("GetAllUsersWithRoles")]
        [ProducesResponseType(typeof(List<AlgoStoreUserData>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllUsersWithRoles()
        {
            var result =
                await _log.LogElapsedTime(null, async () => await _userRolesService.GetAllUsersWithRolesAsync());

            return Ok(result);
        }

        [HttpGet("getByIdWithRoles")]
        [SwaggerOperation("GetUserByIdWithRoles")]
        [ProducesResponseType(typeof(AlgoStoreUserData), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetUserByIdWithRoles(string clientId)
        {
            if (string.IsNullOrEmpty(clientId))
                return BadRequest(ErrorResponse.Create(Phrases.ClientIdEmpty));

            var result =
                await _log.LogElapsedTime(null, async () => await _userRolesService.GeyUserByIdWithRoles(clientId));

            return Ok(result);
        }
    }
}
