using MediatR;

namespace TinyUrl.CQS.Commands
{
    public class DeleteRefreshTokenCommand : IRequest<int>
    {
        public Guid TokenValue { get; set; }
    }
}
