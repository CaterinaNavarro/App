using Microsoft.AspNetCore.Mvc;
using NetCoreApp.API.Dtos;
using NetCoreApp.Security.Attributes;
using NetCoreApp.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeographicalDataController : ControllerBase
    {
        private readonly IExternalService _externalService;

        public GeographicalDataController(IExternalService externalService)
        {
            _externalService = externalService;
        }

        /// <summary>
        /// Search provinces
        /// </summary>
        /// <param name="name"> Province name </param>
        /// <returns> Filtered provinces list </returns>
        [JwtAuthorize]
        [HttpGet("provinces/search")]
        public async Task<ActionResult<ProvinceSearchResponseDto>> ProvinceSearch([FromQuery] string name)
        {
            return Ok((await _externalService.ProvinceSearch(name)).Select(p => new ProvinceSearchResponseDto()
            {
                Name = p.Name,
                Latitude = p.Latitude,
                Longitude = p.Longitude
            }));
        }

    }
}
