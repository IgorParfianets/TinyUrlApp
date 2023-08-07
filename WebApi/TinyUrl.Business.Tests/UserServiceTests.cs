using Microsoft.Extensions.Configuration;
using Moq;
using AutoMapper;
using MediatR;
using TinyUrl.Core.DataTransferObjects;
using TinyUrl.Business.Services;
using TinyUrl.CQS.Commands;
using TinyUrl.CQS.Queries;
using TinyUrl.CQS.Handlers.QueryHandlers;
using TinyUrl.Database.Entities;

namespace TinyUrl.Business.Tests
{
    public class UserServiceTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _mapperMock = new Mock<IMapper>();
            _configurationMock = new Mock<IConfiguration>();
            _userService = new UserService(_mapperMock.Object, _configurationMock.Object, _mediatorMock.Object);
        }

        [Fact]
        public async Task RegisterUserAsync_WhenValidUserDto_ReturnsOne()
        {
            // Arrange
            var expectedResult = 1;
            var userDto = new UserDto()
            {
                Username = "Test",
                Email = "test@google.com",
                Password = "password",
            };
            var user = new User()
            {
                Id = Guid.NewGuid(),
                UserName = "Test",
                Email = "test@google.com",
                PasswordHash = "ad1dsa2"
            };

            _mapperMock.Setup(x => x.Map<User>(It.IsAny<UserDto>())).Returns(user);
            _mediatorMock.Setup(x => x.Send(It.IsAny<AddUserCommand>(), default)).ReturnsAsync(expectedResult);

            var userService = new UserService(_mapperMock.Object, _configurationMock.Object, _mediatorMock.Object);

            // Act
            var result = await userService.RegisterUserAsync(userDto);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task IsExistEmailAsync_WhenEmailExists_ReturnsTrue()
        {
            // Arrange
            var email = "test@example.com";
            var expectedUser = new User()
            {
                Id = Guid.NewGuid(),
                UserName = "Test",
                Email = "test@google.com",
                PasswordHash = "ad1dsa2"
            };

            _mediatorMock.Setup(x => x.Send(It.IsAny<GetUserByEmailQuery>(), default)).ReturnsAsync(expectedUser);

            // Act
            var result = await _userService.IsExistEmailAsync(email);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task GetUserByEmailAsync_WhenEmailExists_ReturnsUserDto()
        {
            // Arrange
            var email = "test@example.com";
            Guid userId = Guid.NewGuid();
            var expectedUser = new User() 
            {
                Id = userId,
                UserName = "Test",
                Email = "test@google.com",
                PasswordHash = "ad1dsa2"
            };
            var expectedUserDto = new UserDto()
            {
                Id = userId,
                Username = "Test",
                Email = "test@google.com",
                Password = "ad1dsa2"
            };

            _mediatorMock.Setup(x => x.Send(It.IsAny<GetUserByEmailQuery>(), default)).ReturnsAsync(expectedUser);
            _mapperMock.Setup(x => x.Map<UserDto>(expectedUser)).Returns(expectedUserDto);

            // Act
            var result = await _userService.GetUserByEmailAsync(email);

            // Assert
            Assert.Equal(expectedUserDto, result);
        }

        [Fact]
        public async Task GetUserByRefreshTokenAsync_WhenValidToken_ReturnsUserDto()
        {
            // Arrange
            var refreshToken = Guid.NewGuid();
            var expectedUser = new User(); //
            var expectedUserDto = new UserDto(); 

            _mediatorMock.Setup(x => x.Send(It.IsAny<GetUserByRefreshTokenQuery>(), default)).ReturnsAsync(expectedUser);
            _mapperMock.Setup(x => x.Map<UserDto>(expectedUser)).Returns(expectedUserDto);

            // Act
            var result = await _userService.GetUserByRefreshTokenAsync(refreshToken);

            // Assert
            Assert.Equal(expectedUserDto, result);
        }
    }
}


