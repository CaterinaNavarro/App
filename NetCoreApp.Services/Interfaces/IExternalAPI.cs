using Refit;
using System.Threading.Tasks;

namespace NetCoreApp.Services.Interfaces
{
    public interface IExternalAPI
    {
        [Get("/api/provincias?nombre={nombre}")]
        public Task<string> SearchProvinces(string nombre);
    }
}
