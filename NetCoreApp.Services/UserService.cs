using NetCoreApp.Domain.Entities;
using NetCoreApp.Infrastructure.Data;
using NetCoreApp.Services.Interfaces;
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using NetCoreApp.Crosscutting.Helpers;
using Microsoft.Extensions.Options;
using NetCoreApp.Crosscutting.Exceptions;
using System.Net;

namespace NetCoreApp.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly AppSettings _appSettings;

        public UserService (ApplicationDbContext context, IOptions<AppSettings> appSettings)
        {
            _context = context;
            _appSettings = appSettings.Value;
        }

        public async Task<string> Authenticate(string username, string password)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == username && u.PasswordHash == CryptographyHelper.EncryptPassword(password));

            if (user is null) throw new ClientErrorException("Username or password incorrect", (int)HttpStatusCode.BadRequest);

            return generateJwtToken(user);
        }

        public async Task<User> GetUserByUsername(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<IEnumerable<User>> UserList()
        {
            return await _context.Users.ToListAsync();
        }

        #region Privates

        private string generateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("username", user.Username), new Claim("firstname", user.LastName), new Claim("lastname", user.FirstName) }),
                Expires = DateTime.UtcNow.AddMinutes(10),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.Secret)), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        #endregion Privates

    }
}
