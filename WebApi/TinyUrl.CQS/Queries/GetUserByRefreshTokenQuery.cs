using MediatR;
using TinyUrl.Database.Entities;

namespace TinyUrl.CQS.Handlers.QueryHandlers
{
    public class GetUserByRefreshTokenQuery : IRequest<User?>
    {
        public Guid RefreshToken { get; set; }
    }
}
