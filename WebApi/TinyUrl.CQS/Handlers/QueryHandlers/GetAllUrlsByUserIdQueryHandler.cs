using MediatR;
using Microsoft.EntityFrameworkCore;
using TinyUrl.CQS.Queries;
using TinyUrl.Database;
using TinyUrl.Database.Entities;

namespace TinyUrl.CQS.Handlers.QueryHandlers
{
    public class GetAllUrlsByUserIdQueryHandler : IRequestHandler<GetAllUrlsByUserIdQuery, IEnumerable<Url>>
    {
        private readonly TinyUrlContext _context;

        public GetAllUrlsByUserIdQueryHandler(TinyUrlContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Url>> Handle(GetAllUrlsByUserIdQuery request, CancellationToken cancellationToken)
        {
            var urls = await _context.Urls
                .AsNoTracking()
                .Where(url => url.UserId.Equals(request.UserId))
                .ToArrayAsync();

            return urls;
        }
    }
}
