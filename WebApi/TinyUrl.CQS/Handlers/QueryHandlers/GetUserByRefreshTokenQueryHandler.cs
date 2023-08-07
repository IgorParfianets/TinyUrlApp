using MediatR;
using Microsoft.EntityFrameworkCore;
using TinyUrl.Database;
using TinyUrl.Database.Entities;

namespace TinyUrl.CQS.Handlers.QueryHandlers
{
    public class GetUserByRefreshTokenQueryHandler : IRequestHandler<GetUserByRefreshTokenQuery, User?>
    {
        private readonly TinyUrlContext _context;

        public GetUserByRefreshTokenQueryHandler(TinyUrlContext context)
        {
            _context = context;
        }

        public async Task<User?> Handle(GetUserByRefreshTokenQuery request, CancellationToken cancellationToken)
        {
            return (await _context.RefreshTokens
                .Include(token => token.User)
                .AsNoTracking()
                .FirstOrDefaultAsync(token => token.Token.Equals(request.RefreshToken)))
                ?.User;
        }
    }
}
