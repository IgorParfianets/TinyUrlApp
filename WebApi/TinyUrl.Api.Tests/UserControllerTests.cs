using AutoMapper;
using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json.Linq;
using TinyUrl.API.Controllers;
using TinyUrl.API.Models.Request;
using TinyUrl.API.Models.Responce;
using TinyUrl.API.Utils;
using TinyUrl.Core.Abstractions;
using TinyUrl.Core.DataTransferObjects;

namespace TinyUrl.Api.Tests
{
    public class UserControllerTests
    {
        private readonly Mock<IUserService> _userServiceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IJwtUtil> _jwtUtilMock;
        private readonly UserController _userController;

        public UserControllerTests()
        {
            _userServiceMock = new Mock<IUserService>();
            _mapperMock = new Mock<IMapper>();
            _jwtUtilMock = new Mock<IJwtUtil>();
            _userController = new UserController(_userServiceMock.Object, _mapperMock.Object, _jwtUtilMock.Object);
        }

        [Fact]
        public async Task Create_WhenValidUserModel_ReturnsJwtToken()
        {
            // Arrange
            var userModel = new RegistrationUserRequestModel
            {
                Username = "Test",
                Email = "test@example.com",
                Password = "password",
                PasswordConfirmation = "password"
            };

            var userDto = new UserDto
            {
                Username = userModel.Username,
                Email = userModel.Email,
                Password = userModel.Password
            };

            var expectedJwtToken = new TokenResponseModel 
            {
                AccessToken = "asddsfsdfsdfqqd",
                TokenExpiration = DateTime.Now,
                UserId = Guid.NewGuid(),
                RefreshToken = Guid.NewGuid()
            };

            _userServiceMock.Setup(x => x.IsExistEmailAsync(userModel.Email)).ReturnsAsync(false);
            _mapperMock.Setup(x => x.Map<UserDto>(userModel)).Returns(userDto);
            _userServiceMock.Setup(x => x.RegisterUserAsync(userDto)).ReturnsAsync(1);
            _userServiceMock.Setup(x => x.GetUserByEmailAsync(userModel.Email)).ReturnsAsync(userDto);
            _jwtUtilMock.Setup(x => x.GenerateTokenAsync(userDto)).ReturnsAsync(expectedJwtToken);

            // Act
            var result = await _userController.Create(userModel);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var tokenResponse = Assert.IsType<TokenResponseModel>(okResult.Value);
            Assert.Equal(expectedJwtToken, tokenResponse);
        }
    }
}
