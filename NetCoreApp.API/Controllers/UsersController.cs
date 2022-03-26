using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using NetCoreApp.API.Dtos;
using NetCoreApp.Crosscutting.Exceptions;
using NetCoreApp.Services.Interfaces;
using System.Net;
using System.Linq;
using System.Threading.Tasks;
using NetCoreApp.Security.Attributes;

namespace NetCoreApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController (IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Authenticate an user
        /// </summary>
        /// <param name="request"> User information </param>
        /// <returns> User token generated </returns>
        [HttpPost("authenticate")]
        public async Task<ActionResult<UserAuthenticateResponseDto>> Authenticate([FromBody] UserAuthenticateRequestDto request)
        {
            var authenticate = await _userService.Authenticate(request.Username, request.Password);

            return Ok(new UserAuthenticateResponseDto() { Token = authenticate });
        }

        /// <summary>
        /// Get User list
        /// </summary>
        /// <returns> Users </returns>
        [JwtAuthorize]
        [HttpGet]
        public async Task<ActionResult<UserListResponseDto>> UserList()
        {
            return Ok((await _userService.UserList()).Select(u => new UserListResponseDto()
            {
                Username = u.Username,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email
            }));
        }
    }
}
