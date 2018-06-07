﻿using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Lykke.AlgoStore.Service.Security.Core.Domain;
using Lykke.AlgoStore.Service.Security.Core.Services;
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

        public AlgoStoreUsersController(
            IUserRolesService userRolesService)
        {
            _userRolesService = userRolesService;
        }

        [HttpGet("getAllWithRoles")]
        [SwaggerOperation("GetAllUsersWithRoles")]
        [ProducesResponseType(typeof(List<AlgoStoreUserData>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllUsersWithRoles()
        {
            var result = await _userRolesService.GetAllUsersWithRolesAsync();
            return Ok(result);
        }

        [HttpGet("getByIdWithRoles")]
        [SwaggerOperation("GetUserByIdWithRoles")]
        [ProducesResponseType(typeof(AlgoStoreUserData), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetUserByIdWithRoles(string clientId)
        {
            if (string.IsNullOrEmpty(clientId))
                return BadRequest(ErrorResponse.Create(Phrases.ClientIdEmpty));

            var result = await _userRolesService.GeyUserByIdWithRoles(clientId);

            return Ok(result);
        }
    }
}
