using MediatR;
using TinyUrl.CQS.Commands;
using TinyUrl.Database;

namespace TinyUrl.CQS.Handlers.CommandHandlers
{
    public class AddUserCommandHandler : IRequestHandler<AddUserCommand, int>
    {
        private readonly TinyUrlContext _context;

        public AddUserCommandHandler(TinyUrlContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            await _context.Users.AddAsync(request.User);
            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
