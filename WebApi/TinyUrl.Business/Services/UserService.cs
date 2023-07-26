using AutoMapper;
using MediatR;
using Microsoft.Extensions.Configuration;
using TinyUrl.Core.Abstractions;
using TinyUrl.Core.DataTransferObjects;
using TinyUrl.CQS.Commands;
using TinyUrl.Database.Entities;

namespace TinyUrl.Business.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly IConfiguration _configuration;

        public UserService(IMapper mapper,
            IConfiguration configuration,
            IMediator mediator)        
        {
            _mapper = mapper;
            _configuration = configuration;
            _mediator = mediator;
        }

        public async Task<int> RegisterUserAsync(UserDto userDto)
        {
            var entity = _mapper.Map<User>(userDto);

            if (entity == null)
                throw new NullReferenceException($"{entity} mapped in null");

            entity.PasswordHash = CreateMd5(userDto.Password);

            var result = await _mediator.Send(new AddUserCommand() { User = entity });
            return result;
        }

        private string CreateMd5(string password)
        {
            var passwordSalt = _configuration["Secrets:PasswordSalt"];

            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                var inputBytes = System.Text.Encoding.UTF8.GetBytes(password + passwordSalt);
                var hashBytes = md5.ComputeHash(inputBytes);

                return Convert.ToHexString(hashBytes);
            }
        }
    }
}
