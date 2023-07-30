using MediatR;
using TinyUrl.Database.Entities;

namespace TinyUrl.CQS.Commands
{
    public class AddUserCommand : IRequest<int>
    {
        public User User { get; set; }
    }
}
