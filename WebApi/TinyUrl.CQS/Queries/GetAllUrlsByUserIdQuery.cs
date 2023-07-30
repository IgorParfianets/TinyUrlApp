using MediatR;
using TinyUrl.Database.Entities;

namespace TinyUrl.CQS.Queries
{
    public class GetAllUrlsByUserIdQuery : IRequest<IEnumerable<Url>>
    {
        public Guid UserId { get; set; }
    }
}
