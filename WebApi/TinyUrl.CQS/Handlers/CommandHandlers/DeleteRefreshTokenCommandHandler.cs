using MediatR;
using Microsoft.EntityFrameworkCore;
using TinyUrl.CQS.Commands;
using TinyUrl.Database;

namespace TinyUrl.CQS.Handlers.CommandHandlers
{
    public class DeleteRefreshTokenCommandHandler : IRequestHandler<DeleteRefreshTokenCommand, int>
    {
        private readonly TinyUrlContext _context;

        public DeleteRefreshTokenCommandHandler(TinyUrlContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(DeleteRefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var token = await _context.RefreshTokens
                .FirstOrDefaultAsync(token => request.TokenValue.Equals(token.Token));

            if (token != null)
                _context.RefreshTokens.Remove(token);  
            
            return await _context.SaveChangesAsync();
        }
    }
}
