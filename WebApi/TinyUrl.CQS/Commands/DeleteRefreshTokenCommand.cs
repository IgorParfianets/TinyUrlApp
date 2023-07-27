using MediatR;

namespace TinyUrl.CQS.Commands
{
    public class DeleteRefreshTokenCommand : IRequest
    {
        public Guid TokenValue { get; set; }
    }
}
