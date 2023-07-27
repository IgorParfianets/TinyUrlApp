using MediatR;
using TinyUrl.Database.Entities;

namespace TinyUrl.CQS.Commands
{
    public class AddRefreshTokenCommand : IRequest<int>
    {
        public RefreshToken RefreshToken { get; set; }
    }
}
