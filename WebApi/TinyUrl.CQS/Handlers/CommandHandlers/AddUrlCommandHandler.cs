using MediatR;
using TinyUrl.CQS.Commands;
using TinyUrl.Database;

namespace TinyUrl.CQS.Handlers.CommandHandlers
{
    public class AddUrlCommandHandler : IRequestHandler<AddUrlCommand, int>
    {
        private readonly TinyUrlContext _context;

        public AddUrlCommandHandler(TinyUrlContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(AddUrlCommand request, CancellationToken cancellationToken)
        {
            await _context.Urls.AddAsync(request.Url);
            return await _context.SaveChangesAsync();
        }
    }
}
