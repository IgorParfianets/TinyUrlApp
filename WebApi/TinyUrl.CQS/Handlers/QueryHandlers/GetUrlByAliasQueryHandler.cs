using MediatR;
using Microsoft.EntityFrameworkCore;
using TinyUrl.CQS.Queries;
using TinyUrl.Database;
using TinyUrl.Database.Entities;

namespace TinyUrl.CQS.Handlers.QueryHandlers
{
    public class GetUrlByAliasQueryHandler : IRequestHandler<GetUrlByAliasQuery, Url?>
    {
        private readonly TinyUrlContext _context;

        public GetUrlByAliasQueryHandler(TinyUrlContext context)
        {
            _context = context;
        }

        public async Task<Url?> Handle(GetUrlByAliasQuery request, CancellationToken cancellationToken)
        {
            return await _context.Urls
                .AsNoTracking()
                .FirstOrDefaultAsync(link => link.Alias.Equals(request.Alias));
        }
    }
}
