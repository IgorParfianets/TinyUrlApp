using MediatR;
using TinyUrl.Database.Entities;

namespace TinyUrl.CQS.Queries
{
    public class GetUserByEmailQuery : IRequest<User?>
    {
        public string Email { get; set; }
    }
}
