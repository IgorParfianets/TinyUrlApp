using MediatR;
using TinyUrl.CQS.Commands;
using TinyUrl.Database;

namespace TinyUrl.CQS.Handlers.CommandHandlers
{
    public class AddRefreshTokenCommandHandler : IRequestHandler<AddRefreshTokenCommand, int>
    {
        private readonly TinyUrlContext _context;

        public AddRefreshTokenCommandHandler(TinyUrlContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(AddRefreshTokenCommand request, CancellationToken cancellationToken)
        {
            await _context.RefreshTokens.AddAsync(request.RefreshToken);
            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
