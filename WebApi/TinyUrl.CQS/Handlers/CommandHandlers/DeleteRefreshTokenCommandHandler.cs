using MediatR;
using Microsoft.EntityFrameworkCore;
using TinyUrl.CQS.Commands;
using TinyUrl.Database;

namespace TinyUrl.CQS.Handlers.CommandHandlers
{
    public class DeleteRefreshTokenCommandHandler : IRequestHandler<DeleteRefreshTokenCommand, Unit>
    {
        private readonly TinyUrlContext _context;

        public DeleteRefreshTokenCommandHandler(TinyUrlContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteRefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var token = await _context.RefreshTokens
                .FirstOrDefaultAsync(token => token.Equals(request.TokenValue));

            if (token != null)
            {
                _context.RefreshTokens.Remove(token);   
            }
            await _context.SaveChangesAsync();
            return Unit.Value;
        }
    }
}
