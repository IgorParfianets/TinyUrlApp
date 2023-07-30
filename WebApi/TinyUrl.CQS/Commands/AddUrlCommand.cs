using MediatR;
using TinyUrl.Database.Entities;

namespace TinyUrl.CQS.Commands
{
    public class AddUrlCommand : IRequest<int>
    {
        public Url Url { get; set; }
    }
}
