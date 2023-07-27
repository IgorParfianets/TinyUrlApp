using MediatR;
using TinyUrl.Core.Abstractions;
using TinyUrl.CQS.Commands;
using TinyUrl.Database.Entities;

namespace TinyUrl.Business.Services
{
    public class TokenService : ITokenService
    {
        private readonly IMediator _mediator;
        public TokenService(IMediator mediator) 
        {
            _mediator = mediator;
        }

        public async Task<int> CreateRefreshTokenAsync(Guid tokenValue, Guid userId)
        {
            var refreshToken = new RefreshToken()
            {
                Id = Guid.NewGuid(),
                Token = tokenValue,
                UserId = userId
            };

            int result = await _mediator.Send(new AddRefreshTokenCommand() { RefreshToken = refreshToken });
            return result;
        }

        public async Task RemoveRefreshTokenAsync(Guid tokenValue)
        {
            await _mediator.Send(new DeleteRefreshTokenCommand() { TokenValue = tokenValue });
        }
    }
}
