using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoListWebAPI.Controllers;
using ToDoListWebDomain.Domain.Models;
using ToDoListWebServices.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;

namespace UnitTests.AuthorizeTest
{
    public class ApiAuthorizationTest
    {
        [Fact]
        public async Task RegisterTest()
        {
            // Arrange
            var userRegistration = new UserRegistration
            {
                LoginProp = "testuser",
                EmailProp = "testuser@example.com",
                Password = "password123"
            };
            var loggerMock = new Mock<ILogger<APIAccountController>>();

            var userManager = new Mock<UserManager<User>>();
            userManager.Setup(u => u.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);


            var configuration = new Mock<IConfiguration>();
            configuration.Setup(c => c["Jwt:Secret"]).Returns("your-secret-key");

            var controller = new APIAccountController(configuration.Object, null, null, loggerMock.Object);

            // Act
            var result = await controller.Register(userRegistration);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);

            var token = okResult?.Value?.ToString();
            Assert.NotEmpty(token);
        }

    }


}
