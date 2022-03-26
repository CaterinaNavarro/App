using NetCoreApp.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NetCoreApp.Services.Interfaces
{
    public interface IUserService
    {
        Task<string> Authenticate(string username, string password);
        Task<User> GetUserByUsername(string username);
        Task<IEnumerable<User>> UserList();
    }
}
