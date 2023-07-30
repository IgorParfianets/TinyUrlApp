using Castle.Core.Configuration;
using Moq;
using AutoMapper;
using MediatR;
using TinyUrl.Core.DataTransferObjects;

namespace TinyUrl.Business.Tests
{
    public class UserServiceTests
    {                               
        private readonly Mock<IMapper> _mapperMock = new Mock<IMapper>();
        private readonly Mock<IMediator> _mediatorMock = new Mock<IMediator>();
        private readonly Mock<IConfiguration> _configurationMock = new Mock<IConfiguration>();

        [Fact]
        public void RegisterUserAsync_WithCorrectData_Return1()
        {
            var dto = new UserDto()
            string email = "8899b@gmail.com";
            string password = "qwerty123";
            string confirmPassword = "qwerty123";

            _mediatorMock.SetupSet(set => set.Send(new ))
        }
    }
}
