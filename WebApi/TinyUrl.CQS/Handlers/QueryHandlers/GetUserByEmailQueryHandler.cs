using MediatR;
using Microsoft.EntityFrameworkCore;
using TinyUrl.CQS.Queries;
using TinyUrl.Database;
using TinyUrl.Database.Entities;

namespace TinyUrl.CQS.Handlers.QueryHandlers
{
    public class GetUserByEmailQueryHandler : IRequestHandler<GetUserByEmailQuery, User?>
    {
        private readonly TinyUrlContext _context;

        public GetUserByEmailQueryHandler(TinyUrlContext context)
        {
            _context = context;
        }
        public async Task<User?> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
        {
            return await _context.Users
                .AsNoTracking()
                .Where(user => request.Email.Equals(user.Email))
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
