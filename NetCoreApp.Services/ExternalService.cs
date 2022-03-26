using Microsoft.Extensions.Configuration;
using NetCoreApp.Crosscutting.Exceptions;
using Serilog;
using NetCoreApp.Services.Interfaces;
using NetCoreApp.Services.Models;
using Newtonsoft.Json.Linq;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace NetCoreApp.Services
{
    public class ExternalService : IExternalService
    {
        private readonly IExternalAPI _externalAPI;
        
        public ExternalService(IConfiguration configuration)
        {
            _externalAPI = RestService.For<IExternalAPI>(configuration.GetSection("ExternalAPI:GovernmentBaseUrl").Value,
                new RefitSettings());
        }

        public async Task<IEnumerable<ProvinceSearchResponseModel>> ProvinceSearch(string name)
        {
            try
            {
                return JObject.Parse(await _externalAPI.SearchProvinces(name))
                .SelectToken("provincias").Select(item => new ProvinceSearchResponseModel()
                {
                    Id = (long)item.SelectToken("id"),
                    Name = item.SelectToken("nombre").ToString(),
                    Latitude = item.SelectToken("centroide").SelectToken("lat").ToString(),
                    Longitude = item.SelectToken("centroide").SelectToken("lon").ToString()
                });
            }
            catch (ApiException apiEx) 
            {
                Log.Logger.ForContext<ExternalService>().Error(apiEx, $"Governmnet API error - Search provinces\nStatusCode: {apiEx.StatusCode}\nMessage: {apiEx.Message}\nContent: {apiEx.Content}");
                throw new ClientErrorException(apiEx.Content, (int)HttpStatusCode.BadRequest); 
            }

            catch (Exception ex) 
            {
                Log.Logger.ForContext<ExternalService>().Error(ex, $"Governmnet API error - Message: {ex.Message}");
                throw new ClientErrorException(ex.Message, (int)HttpStatusCode.BadRequest); 
            }
        }
    }
}
