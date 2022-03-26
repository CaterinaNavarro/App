using NetCoreApp.Services.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NetCoreApp.Services.Interfaces
{
    public interface IExternalService
    {
        public Task<IEnumerable<ProvinceSearchResponseModel>> ProvinceSearch(string name);
    }
}
