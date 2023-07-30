using MediatR;
using Microsoft.EntityFrameworkCore;
using TinyUrl.CQS.Commands;
using TinyUrl.Database;

namespace TinyUrl.CQS.Handlers.CommandHandlers
{
    public class DeleteUrlCommandByAliasHandler : IRequestHandler<DeleteUrlCommandByAlias, int>
    {
        public readonly TinyUrlContext _context;

        public DeleteUrlCommandByAliasHandler(TinyUrlContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(DeleteUrlCommandByAlias request, CancellationToken cancellationToken)
        {
            var entity = await _context.Urls
                .FirstOrDefaultAsync(url => url.Alias.Equals(request.Alias));

            if (entity != null)
                _context.Urls.Remove(entity);
            
            return await _context.SaveChangesAsync();
        }
    }
}
