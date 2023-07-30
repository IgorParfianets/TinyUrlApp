using MediatR;

namespace TinyUrl.CQS.Commands
{
    public class DeleteUrlCommandByAlias : IRequest<int>
    {
        public string Alias { get; set; }
        public Guid UserId { get; set; }
    }
}
