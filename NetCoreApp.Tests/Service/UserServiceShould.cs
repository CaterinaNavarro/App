using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;
using NetCoreApp.Crosscutting.Helpers;
using NetCoreApp.Domain.Entities;
using NetCoreApp.Infrastructure.Data;
using NetCoreApp.Services;
using NetCoreApp.Services.Interfaces;
using NetCoreApp.Tests.Async;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace NetCoreApp.Tests.Service
{
    public class UserServiceShould
    {
        #region Property  

        public Mock<IUserService> mockService = new Mock<IUserService>();

        #endregion

        [Fact]
        public async Task NotAuthenticate()
        {

            //Arrange

            var data = new List<User>
            {
                new User () {
                    FirstName = "mock username",
                    LastName = "mock lastname",
                    Username = "mock username",
                    Email = "mock email",
                    PasswordHash = "5497534645sdfa"
                }
            }.AsQueryable();

            var mockSet = DbSetMock.CreateDbSetMock(data);
            
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer(@"Server=(localDb)\\MSSQLLocalDB;Database=NetCoreApp;Trusted_Connection=True");

            Mock<ApplicationDbContext> mockContext = new Mock<ApplicationDbContext>(optionsBuilder.Options);
            mockContext.Setup(c => c.Users).Returns(mockSet.Object);

            IOptions<AppSettings> appSettings = Options.Create(new AppSettings() { Secret = "secret" });

            string username = "wrong username";
            string password = "wrong password";

            var userService = new UserService(mockContext.Object, appSettings);

            //Act
            var token = await userService.Authenticate(username, password);

            //Assert
            Assert.Null(token);
        }
    }
}
